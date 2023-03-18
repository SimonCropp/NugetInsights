// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Insights.Worker;
using NuGet.Insights.Worker.KustoIngestion;
using NuGet.Insights.Worker.Workflow;

namespace NuGet.Insights.Website
{
    public class ViewModelFactory
    {
        private const int HistoryCount = 10;

        private readonly CatalogCommitTimestampProvider _catalogCommitTimestampProvider;
        private readonly IRawMessageEnqueuer _rawMessageEnqueuer;
        private readonly CatalogScanStorageService _catalogScanStorageService;
        private readonly CatalogScanCursorService _catalogScanCursorService;
        private readonly CatalogScanService _catalogScanService;
        private readonly IRemoteCursorClient _remoteCursorClient;
        private readonly TimerExecutionService _timerExecutionService;
        private readonly WorkflowService _workflowService;
        private readonly WorkflowStorageService _workflowStorageService;
        private readonly KustoIngestionStorageService _kustoIngestionStorageService;
        private readonly MoveMessagesTaskQueue _moveMessagesTaskQueue;

        public ViewModelFactory(
            CatalogCommitTimestampProvider catalogCommitTimestampProvider,
            IRawMessageEnqueuer rawMessageEnqueuer,
            CatalogScanStorageService catalogScanStorageService,
            CatalogScanCursorService catalogScanCursorService,
            CatalogScanService catalogScanService,
            IRemoteCursorClient remoteCursorClient,
            TimerExecutionService timerExecutionService,
            WorkflowService workflowService,
            WorkflowStorageService workflowStorageService,
            KustoIngestionStorageService kustoIngestionStorageService,
            MoveMessagesTaskQueue moveMessagesTaskQueue)
        {
            _catalogCommitTimestampProvider = catalogCommitTimestampProvider;
            _rawMessageEnqueuer = rawMessageEnqueuer;
            _catalogScanStorageService = catalogScanStorageService;
            _catalogScanCursorService = catalogScanCursorService;
            _catalogScanService = catalogScanService;
            _remoteCursorClient = remoteCursorClient;
            _timerExecutionService = timerExecutionService;
            _workflowService = workflowService;
            _workflowStorageService = workflowStorageService;
            _kustoIngestionStorageService = kustoIngestionStorageService;
            _moveMessagesTaskQueue = moveMessagesTaskQueue;
        }

        public async Task<AdminViewModel> GetAdminViewModelAsync()
        {
            var workQueueTask = GetQueueAsync(QueueType.Work);
            var expandQueueTask = GetQueueAsync(QueueType.Expand);
            var cursorsTask = _catalogScanCursorService.GetCursorsAsync();
            var catalogScansTask = _catalogScanStorageService.GetAllLatestIndexScansAsync(HistoryCount);
            var isWorkflowRunningTask = _workflowService.IsWorkflowRunningAsync();
            var timerStatesTask = _timerExecutionService.GetStateAsync();
            var workflowRunsTask = GetWorkflowRunsAsync();
            var kustoIngestionsTask = GetKustoIngestionsAsync();
            var catalogCommitTimestampTask = _remoteCursorClient.GetCatalogAsync();

            await Task.WhenAll(
                workQueueTask,
                expandQueueTask,
                catalogScansTask,
                cursorsTask,
                isWorkflowRunningTask,
                timerStatesTask,
                workflowRunsTask,
                kustoIngestionsTask,
                catalogCommitTimestampTask);

            // Build the catalog scan (driver) view models
            var catalogCommitTimestamp = await catalogCommitTimestampTask;
            var catalogScans = new List<CatalogScanViewModel>();
            foreach ((var driverType, var latestScans) in catalogScansTask.Result.OrderBy(x => x.Key))
            {
                var cursor = cursorsTask.Result[driverType];
                var nextCommitTimestampForCursor = await _catalogCommitTimestampProvider.GetNextAsync(cursor.Value);

                var min = cursor.Value;
                if (min < CatalogClient.NuGetOrgMin)
                {
                    min = CatalogClient.NuGetOrgMin;
                }

                catalogScans.Add(new CatalogScanViewModel
                {
                    Cursor = cursor,
                    DefaultMax = nextCommitTimestampForCursor ?? CatalogClient.NuGetOrgFirstCommit,
                    CursorAge = catalogCommitTimestamp - min,
                    DriverType = driverType,
                    LatestScans = latestScans,
                    Dependencies = CatalogScanCursorService.GetDependencies(driverType),
                    OnlyLatestLeavesSupport = _catalogScanService.GetOnlyLatestLeavesSupport(driverType),
                    SupportsBucketRangeProcessing = _catalogScanService.SupportsBucketRangeProcessing(driverType),
                    IsEnabled = _catalogScanService.IsEnabled(driverType),
                });
            }

            // Calculate the next default max, which supports processing the catalog one commit at a time.
            var catalogScanMin = catalogScans.Where(x => x.IsEnabled).Min(x => x.Cursor.Value);
            var nextCommitTimestamp = await _catalogCommitTimestampProvider.GetNextAsync(catalogScanMin);

            var model = new AdminViewModel
            {
                DefaultMax = nextCommitTimestamp ?? catalogScanMin,
                IsWorkflowRunning = await isWorkflowRunningTask,
                WorkQueue = await workQueueTask,
                ExpandQueue = await expandQueueTask,
                CatalogScans = catalogScans,
                WorkflowRuns = await workflowRunsTask,
                KustoIngestions = await kustoIngestionsTask,
                TimerStates = await timerStatesTask,
            };

            return model;
        }

        private async Task<IReadOnlyList<WorkflowRun>> GetWorkflowRunsAsync()
        {
            return await _workflowStorageService.GetLatestRunsAsync(HistoryCount);
        }

        private async Task<IReadOnlyList<KustoIngestionEntity>> GetKustoIngestionsAsync()
        {
            return await _kustoIngestionStorageService.GetLatestIngestionsAsync(HistoryCount);
        }

        private async Task<QueueViewModel> GetQueueAsync(QueueType queue)
        {
            var approximateMessageCountTask = _rawMessageEnqueuer.GetApproximateMessageCountAsync(queue);
            var poisonApproximateMessageCountTask = _rawMessageEnqueuer.GetPoisonApproximateMessageCountAsync(queue);
            var availableMessageCountLowerBoundTask = _rawMessageEnqueuer.GetAvailableMessageCountLowerBoundAsync(queue, StorageUtility.MaxDequeueCount);
            var poisonAvailableMessageCountLowerBoundTask = _rawMessageEnqueuer.GetPoisonAvailableMessageCountLowerBoundAsync(queue, StorageUtility.MaxDequeueCount);

            var model = new QueueViewModel
            {
                QueueType = queue,
                ApproximateMessageCount = await approximateMessageCountTask,
                PoisonApproximateMessageCount = await poisonApproximateMessageCountTask,
                AvailableMessageCountLowerBound = await availableMessageCountLowerBoundTask,
                PoisonAvailableMessageCountLowerBound = await poisonAvailableMessageCountLowerBoundTask,
                MoveMainToPoisonState = GetMoveQueueMessagesState(queue, fromPoison: false),
                MovePoisonToMainState = GetMoveQueueMessagesState(queue, fromPoison: true),
            };

            model.AvailableMessageCountIsExact = model.AvailableMessageCountLowerBound < StorageUtility.MaxDequeueCount;
            model.PoisonAvailableMessageCountIsExact = model.PoisonAvailableMessageCountLowerBound < StorageUtility.MaxDequeueCount;

            return model;
        }

        private MoveQueueMessagesState GetMoveQueueMessagesState(QueueType queueType, bool fromPoison)
        {
            var task = new MoveMessagesTask(queueType, fromPoison, queueType, !fromPoison);
            if (_moveMessagesTaskQueue.IsScheduled(task))
            {
                return MoveQueueMessagesState.Scheduled;
            }
            else if (_moveMessagesTaskQueue.IsWorking(task))
            {
                return MoveQueueMessagesState.Working;
            }
            else
            {
                return MoveQueueMessagesState.None;
            }
        }
    }
}

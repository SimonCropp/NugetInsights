﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.ExplorePackages.Commands;
using Microsoft.Extensions.Logging;

namespace Knapcode.ExplorePackages
{
    public class CommandExecutor
    {
        private readonly ICommand _command;
        private readonly ILogger<CommandExecutor> _logger;

        public CommandExecutor(ICommand command, ILogger<CommandExecutor> logger)
        {
            _command = command;
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken token)
        {
            var commandName = _command.GetType().Name;
            var suffix = "Command";
            if (commandName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                commandName = commandName.Substring(0, commandName.Length - suffix.Length);
            }
            var heading = $"===== {commandName.ToLowerInvariant()} =====";
            _logger.LogInformation(heading);
            try
            {
                await _command.ExecuteAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred.");
            }
            _logger.LogInformation(new string('=', heading.Length) + Environment.NewLine);
        }
    }
}

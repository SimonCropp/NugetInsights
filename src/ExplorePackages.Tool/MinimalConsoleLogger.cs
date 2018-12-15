﻿using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Knapcode.ExplorePackages.Tool
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class MinimalConsoleLogger : ConsoleLogger
#pragma warning restore CS0618 // Type or member is obsolete
    {
        private static readonly Func<string, LogLevel, bool> _trueFilter = (c, l) => true;

        public MinimalConsoleLogger(string name)
            : base(name, _trueFilter, includeScopes: true)
        {
        }

        public override void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.Append(message);
            if (exception != null)
            {
                messageBuilder.AppendLine();
                messageBuilder.Append(exception);
            }

            ConsoleUtility.LogToConsole(logLevel, messageBuilder.ToString());
        }
    }
}

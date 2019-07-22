using System;

namespace VolleyManagement.Crosscutting
{
    using Contracts.Infrastructure;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    /// <summary>
    /// The enterprise library log.
    /// </summary>
    public class EnterpriseLibraryLog : ILog
    {
        /// <summary>
        /// Writes message to the log
        /// </summary>
        /// <param name="level">Level of the message</param>
        /// <param name="message">Message text</param>
        public void Write(LogLevelEnum level, string message)
        {
            var logEntry = new LogEntry { Message = message };
            SetCategory(logEntry, level);
            Logger.Write(logEntry);
        }

        /// <summary>
        /// Sets proper category for the log entry
        /// </summary>
        /// <param name="logEntry">The log entry. </param>
        /// <param name="level">The level. </param>
        private static void SetCategory(LogEntry logEntry, LogLevelEnum level)
        {
            switch (level)
            {
                case LogLevelEnum.Information:
                    logEntry.Categories.Add("Information");
                    break;
                case LogLevelEnum.Error:
                    logEntry.Categories.Add("Error");
                    break;
                case LogLevelEnum.Debug:
                    logEntry.Categories.Add("Debug");
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

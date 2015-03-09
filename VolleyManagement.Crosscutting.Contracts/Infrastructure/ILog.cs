namespace VolleyManagement.Crosscutting.Contracts.Infrastructure
{
    /// <summary>
    /// Log of the system.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Writes message to the log
        /// </summary>
        /// <param name="level">Level of the message</param>
        /// <param name="message">Message text</param>
        void Write(LogLevelEnum level, string message);
    }
}

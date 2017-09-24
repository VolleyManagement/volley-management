namespace VolleyManagement.UI.Infrastructure
{
    using System.Diagnostics;
    using Crosscutting.Contracts.Infrastructure;

    public class TraceLogger : ILog
    {
        public void Write(LogLevelEnum level, string message)
        {
            Trace.Write(message, level.ToString());
        }
    }
}
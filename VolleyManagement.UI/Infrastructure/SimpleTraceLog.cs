namespace VolleyManagement.UI.Infrastructure
{
    using System.Diagnostics;
    using Crosscutting.Contracts.Infrastructure;

    public class SimpleTraceLog : ILog
    {
        public void Write(LogLevelEnum level, string message)
        {
            Trace.WriteLine(message, level.ToString());
        }
    }
}
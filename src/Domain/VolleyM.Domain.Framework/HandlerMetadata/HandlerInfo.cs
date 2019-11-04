namespace VolleyM.Domain.Framework.HandlerMetadata
{
    public class HandlerInfo
    {
        public string Context { get; }

        public string Action { get; }

        public HandlerInfo(string context, string action)
        {
            Context = context;
            Action = action;
        }

        public override string ToString()
        {
            return $"{Context}:{Action}";
        }
    }
}
namespace VolleyM.Domain.Contracts
{
    public class Error
    {
        private Error(ErrorType type, string message)
        {
            Type = type;
            Message = message;
        }

        public ErrorType Type { get; }

        public string Message { get; }

        public static Error Conflict(string message = "Entity already exists")
            => new Error(ErrorType.Conflict, message);
        public static Error NotFound(string message = "Entity not found")
            => new Error(ErrorType.NotFound, message);
        public static Error InternalError(string message)
            => new Error(ErrorType.InternalError, message);
        public static Error NotAuthorized(string message)
            => new Error(ErrorType.NotAuthorized, message);
    }
}
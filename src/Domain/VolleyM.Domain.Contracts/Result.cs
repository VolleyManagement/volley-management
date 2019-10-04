namespace VolleyM.Domain.Contracts
{
    public class Result<TResult> : IResult
        where TResult : class
    {
        public TResult Value { get; }
        public Error Error { get; }

        public bool IsSuccessful => Error == null;

        private Result(TResult value, Error error)
        {
            Value = value;
            Error = error;
        }

        public static implicit operator Result<TResult>(TResult result)
            => new Result<TResult>(result, null);

        public static implicit operator Result<TResult>(Error error)
            => new Result<TResult>(null, error);
    }

    /// <summary>
    /// Needed to use in places where we can't specify generic type
    /// </summary>
    public interface IResult
    {
        bool IsSuccessful { get; }

        Error Error { get; }
    }
}
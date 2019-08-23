namespace VolleyM.Domain.Contracts
{
    /// <summary>
    /// Used to indicate that no query parameters needed
    /// </summary>
    public sealed class Null
    {
        private Null()
        {
        }

        public static Null Value = new Null();
    }
}
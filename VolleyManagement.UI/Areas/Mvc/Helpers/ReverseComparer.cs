namespace VolleyManagement.UI.Helpers
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents reverse comparer
    /// </summary>
    /// <typeparam name="T">Type to compare</typeparam>
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> original;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseComparer{T}"/> class.
        /// </summary>
        /// <param name="original">Original comparer</param>
        public ReverseComparer(IComparer<T> original)
        {
            this.original = original;
        }

        /// <summary>
        /// Compare two elements.
        /// </summary>
        /// <param name="left">First element</param>
        /// <param name="right">Second element</param>
        /// <returns>Integer value of comparison result.</returns>
        public int Compare(T left, T right)
        {
            return original.Compare(right, left);
        }
    }
}
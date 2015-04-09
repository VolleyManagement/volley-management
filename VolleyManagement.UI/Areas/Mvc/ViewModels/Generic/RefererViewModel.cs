namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Generic
{
    /// <summary>
    /// Represents ViewModel and referer link.
    /// </summary>
    /// <typeparam name="T">ViewModel type.</typeparam>
    public class RefererViewModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefererViewModel{T}" /> class.
        /// </summary>
        /// <param name="model">View model.</param>
        /// <param name="referer">Referer controller name.</param>
        public RefererViewModel(T model, string referer)
        {
            Model = model;
            Referer = referer;
        }

        /// <summary>
        /// Gets ViewModel.
        /// </summary>
        public T Model { get; private set; }

        /// <summary>
        /// Gets referer controller name.
        /// </summary>
        public string Referer { get; private set; }
    }
}

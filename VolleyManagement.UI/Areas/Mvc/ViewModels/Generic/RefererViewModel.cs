namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Generic
{
    /// <summary>
    /// Represents ViewModel and referrer link.
    /// </summary>
    /// <typeparam name="T">ViewModel type.</typeparam>
    public class RefererViewModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefererViewModel{T}" /> class.
        /// </summary>
        /// <param name="model">View model.</param>
        /// <param name="referer">Referrer controller name.</param>
        public RefererViewModel(T model, string referer, string referrerAction, int? id)
        {
            Model = model;
            Referer = referer;
            ReferrerAction = referrerAction;
            ReferrerId = id;
        }

        /// <summary>
        /// Gets ViewModel.
        /// </summary>
        public T Model { get; private set; }

        /// <summary>
        /// Gets referrer controller name.
        /// </summary>
        public string Referer { get; private set; }

        /// <summary>
        /// Gets referrer Action name.
        /// </summary>
        public string ReferrerAction { get; private set; }

        /// <summary>
        /// Gets referrer Id value.
        /// </summary>
        public int? ReferrerId { get; private set; }
    }
}

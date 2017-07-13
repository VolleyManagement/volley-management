namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Generic
{
    /// <summary>
    /// Represents ViewModels and referrer link.
    /// </summary>
    /// <typeparam name="T">First ViewModel type.</typeparam>
    /// <typeparam name="J">Second ViewModel type.</typeparam>
    public class ReferrersViewModel<T, J>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferrersViewModel{T, J}" /> class.
        /// </summary>
        /// <param name="fModel">First View model.</param>
        /// <param name="sModel">Second View model.</param>
        /// <param name="referrer">Referrer controller name.</param>
        public ReferrersViewModel(T fModel, J sModel, string referrer)
        {
            FirstModel = fModel;
            SecondModel = sModel;
            Referrer = referrer;
        }

        /// <summary>
        /// Gets First ViewModel.
        /// </summary>
        public T FirstModel { get; private set; }

        /// <summary>
        /// Gets Second ViewModel.
        /// </summary>
        public J SecondModel { get; private set; }

        /// <summary>
        /// Gets referrer controller name.
        /// </summary>
        public string Referrer { get; private set; }
    }
}
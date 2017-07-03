namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Menu
{
    /// <summary>
    /// Represents the navigation menu item
    /// </summary>
    public class MenuItemViewModel
    {
        /// <summary>
        /// Gets or sets item menu name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets is item menu current
        /// </summary>
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Gets or sets item menu controller name
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets item menu action name
        /// </summary>
        public string Action { get; set; }
    }
}
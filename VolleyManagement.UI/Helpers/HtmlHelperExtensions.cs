namespace VolleyManagement.UI.HtmlHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Extensions for Html helpers
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Helper to show TextBox for date.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the value.</typeparam>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to render.</param>
        /// <param name="initialValue">Value to default initialize.</param>
        /// <param name="htmlAttributes">Html attributes input must have.</param>
        /// <returns>TextBox for date representing.</returns>
        public static MvcHtmlString TextBoxForShortDate<TModel, TProperty>(
                                                                this HtmlHelper<TModel> helper,
                                                                System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression,
                                                                DateTime? initialValue,
                                                                object htmlAttributes = null)
        {
            initialValue = initialValue == null ? DateTime.Now : initialValue;
            return new MvcHtmlString(
              System.Web.Mvc.Html.InputExtensions.TextBoxFor<TModel, TProperty>(
                                                                                helper,
                                                                                expression,
                                                                                string.Format("{0:d}", initialValue),
                                                                                htmlAttributes)
                                                                                .ToString());
        }
    }
}
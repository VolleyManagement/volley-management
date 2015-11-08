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
        private const string dotnetDayLongNameMarker = "dddd";
        private const string jqueryuiDayLongNameMarker = "DD";
        private const string dotnetDayShortNameMarker = "ddd";
        private const string jqueryuiDayShortNameMarker = "D";
        private const string dotnetMonthLongNameMarker = "MMMM";
        private const string jqueryuiMonthLongNameMarker = "MM";
        private const string dotnetMonthShortNameMarker = "MMM";
        private const string jqueryuiMonthShortNameMarker = "M";
        private const string dotnetMonthOfYearTwoDigitMarker = "MM";
        private const string jqueryuiMonthOfYearTwoDigitMarker = "mm";
        private const string dotnetMonthOfYearOneDigitMarker = "M";
        private const string jqueryuiMonthOfYearOneDigitMarker = "m";
        private const string dotnetYearFourDigitMarker = "yyyy";
        private const string jqueryuiYearFourDigitMarker = "yy";
        private const string dotnetYearTwoDigitMarker = "yy";
        private const string jqueryuiTwoFourDigitMarker = "y";

        /// <summary>
        /// Convert current .Net Culture to the jQuery format.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <returns>jQuery format of current culture.</returns>
        public static string jQueryJqueryuiFormat(this HtmlHelper helper)
        {
            string currentFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
            
            // Convert the date
            currentFormat = currentFormat.Replace(dotnetDayLongNameMarker, jqueryuiDayLongNameMarker);
            currentFormat = currentFormat.Replace(dotnetDayShortNameMarker, jqueryuiDayShortNameMarker);

            // Convert month
            if (currentFormat.Contains(dotnetMonthLongNameMarker))
            {
                currentFormat = currentFormat.Replace(dotnetMonthLongNameMarker, jqueryuiMonthLongNameMarker);
            }
            else if (currentFormat.Contains(dotnetMonthShortNameMarker))
            {
                currentFormat = currentFormat.Replace(dotnetMonthShortNameMarker, jqueryuiMonthShortNameMarker);
            }
            else if (currentFormat.Contains(dotnetMonthOfYearTwoDigitMarker))
            {
                currentFormat = currentFormat.Replace(dotnetMonthOfYearTwoDigitMarker, jqueryuiMonthOfYearTwoDigitMarker);
            }
            else
            {
                currentFormat = currentFormat.Replace(dotnetMonthOfYearOneDigitMarker, jqueryuiMonthOfYearOneDigitMarker);
            }

            // Convert year
            currentFormat = currentFormat.Contains(dotnetYearFourDigitMarker) ?
                currentFormat.Replace(dotnetYearFourDigitMarker, jqueryuiYearFourDigitMarker)
                : currentFormat.Replace(dotnetYearTwoDigitMarker, jqueryuiTwoFourDigitMarker);

            return currentFormat;
        }

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
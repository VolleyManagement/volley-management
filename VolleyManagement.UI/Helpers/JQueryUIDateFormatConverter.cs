namespace VolleyManagement.UI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides methods and properties to convert system date format to jQuery UI date format.
    /// Represent properties to get current globalization settings.
    /// </summary>
    public static class JQueryUIDateFormatConverter
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
        /// Gets abbreviated month names of current system culture.
        /// </summary>
        public static HtmlString AbbreviatedMonthNames
        {
            get
            {
                return new HtmlString(JsonConvert.SerializeObject(
                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames, Formatting.None));
            }
        }

        /// <summary>
        /// Gets month names of current system culture.
        /// </summary>
        public static HtmlString MonthNames
        {
            get
            {
                return new HtmlString(JsonConvert.SerializeObject(
                                Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthGenitiveNames, Formatting.None));
            }
        }

        /// <summary>
        /// Gets day names of current system culture.
        /// </summary>
        public static HtmlString DayNames
        {
            get
            {
                return new HtmlString(JsonConvert.SerializeObject(
                        Thread.CurrentThread.CurrentCulture.DateTimeFormat.DayNames, Formatting.None));
            }
        }

        /// <summary>
        /// Gets abbreviated day names of current system culture.
        /// </summary>
        public static HtmlString AbbreviatedDayNames
        {
            get
            {
                return new HtmlString(JsonConvert.SerializeObject(
                        Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames, Formatting.None));
            }
        }

        /// <summary>
        /// Gets shortest month names of current system culture.
        /// </summary>
        public static HtmlString ShortestDayNames
        {
            get
            {
                return new HtmlString(JsonConvert.SerializeObject(
                        Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortestDayNames, Formatting.None));
            }
        }

        /// <summary>
        /// Gets two letter language name of current system culture.
        /// </summary>
        public static string Localization
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            }
        }

        /// <summary>
        /// Convert current .Net Culture to the jQuery format.
        /// </summary>
        /// <returns>jQuery format of current culture.</returns>
        public static string JQueryUICurrentDateFormat()
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
    }
}
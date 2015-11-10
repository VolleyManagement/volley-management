namespace VolleyManagement.UI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
        private static DateMarkers dotNetMarkers = new DateMarkers("dddd", "ddd", "MMMM", "MMM", "MM", "M", "yyyy", "yy");
        private static DateMarkers jqueryuiMarkers = new DateMarkers("DD", "D", "MM", "M", "mm", "m", "yy", "y");

        /// <summary>
        /// Gets abbreviated month names of current system culture.
        /// </summary>
        public static HtmlString AbbreviatedMonthNames
        {
            get
            {
                return new HtmlString(JsonConvert.SerializeObject(
                     CultureInfo.CurrentUICulture.DateTimeFormat.AbbreviatedMonthNames, Formatting.None));
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
                               CultureInfo.CurrentUICulture.DateTimeFormat.MonthGenitiveNames, Formatting.None));
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
                         CultureInfo.CurrentUICulture.DateTimeFormat.DayNames, Formatting.None));
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
                         CultureInfo.CurrentUICulture.DateTimeFormat.AbbreviatedDayNames, Formatting.None));
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
                         CultureInfo.CurrentUICulture.DateTimeFormat.ShortestDayNames, Formatting.None));
            }
        }

        /// <summary>
        /// Gets two letter language name of current system culture.
        /// </summary>
        public static string Localization
        {
            get
            {
                return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            }
        }

        /// <summary>
        /// Convert current .Net Culture to the jQuery format.
        /// </summary>
        /// <returns>jQuery format of current culture.</returns>
        public static string JQueryUICurrentDateFormat()
        {
            string currentFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;

            // Convert the date
            currentFormat = currentFormat.Replace(dotNetMarkers.DayLongName, jqueryuiMarkers.DayLongName);
            currentFormat = currentFormat.Replace(dotNetMarkers.DayShortName, jqueryuiMarkers.DayShortName);

            // Convert month
            if (currentFormat.Contains(dotNetMarkers.MonthLongName))
            {
                currentFormat = currentFormat.Replace(dotNetMarkers.MonthLongName, jqueryuiMarkers.MonthLongName);
            }
            else if (currentFormat.Contains(dotNetMarkers.MonthShortName))
            {
                currentFormat = currentFormat.Replace(dotNetMarkers.MonthShortName, jqueryuiMarkers.MonthShortName);
            }
            else if (currentFormat.Contains(dotNetMarkers.MonthTwoDigit))
            {
                currentFormat = currentFormat.Replace(dotNetMarkers.MonthTwoDigit, jqueryuiMarkers.MonthTwoDigit);
            }
            else
            {
                currentFormat = currentFormat.Replace(dotNetMarkers.MonthOneDigit, jqueryuiMarkers.MonthOneDigit);
            }

            // Convert year
            currentFormat = currentFormat.Contains(dotNetMarkers.YearFourDigit) ?
                currentFormat.Replace(dotNetMarkers.YearFourDigit, jqueryuiMarkers.YearFourDigit)
                : currentFormat.Replace(dotNetMarkers.YearTwoDigit, jqueryuiMarkers.YearTwoDigit);

            return currentFormat;
        }

        /// <summary>
        /// An instance of <see cref="DateMarkers"/> represents markers of date format. 
        /// </summary>
        private class DateMarkers
        {
            private readonly string _dayLongName;
            private readonly string _dayShortName;
            private readonly string _monthLongName;
            private readonly string _monthShortName;
            private readonly string _monthTwoDigit;
            private readonly string _monthOneDigit;
            private readonly string _yearFourDigit;
            private readonly string _yearTwoDigit;

            /// <summary>
            /// Initializes a new instance of the <see cref="DateMarkers"/> class. 
            /// </summary>
            /// <param name="dayLongName">Long day format</param>
            /// <param name="dayShortName">Short day format</param>
            /// <param name="monthLongName">Long month format</param>
            /// <param name="monthShortName">Short month format</param>
            /// <param name="monthTwoDigit">Two digit month format</param>
            /// <param name="monthOneDigit">Month format without leading zero</param>
            /// <param name="yearFourDigit">Four digit year format</param>
            /// <param name="yearTwoDigit">Two digit year format</param>
            public DateMarkers(
                string dayLongName,
                string dayShortName,
                string monthLongName,
                string monthShortName,
                string monthTwoDigit,
                string monthOneDigit,
                string yearFourDigit,
                string yearTwoDigit)
            {
                _dayLongName = dayLongName;
                _dayShortName = dayShortName;
                _monthLongName = monthLongName;
                _monthShortName = monthShortName;
                _monthTwoDigit = monthTwoDigit;
                _monthOneDigit = monthOneDigit;
                _yearFourDigit = yearFourDigit;
                _yearTwoDigit = yearTwoDigit;
            }

            /// <summary>
            /// Marker represent long format of day.
            /// </summary>
            public string DayLongName
            {
                get
                {
                    return _dayLongName;
                }
            }

            /// <summary>
            /// Marker represent short format of day.
            /// </summary>
            public string DayShortName
            {
                get
                {
                    return _dayShortName;
                }
            }

            /// <summary>
            /// Marker represent long format of month.
            /// </summary>
            public string MonthLongName
            {
                get
                {
                    return _monthLongName;
                }
            }

            /// <summary>
            /// Marker represent short format of month.
            /// </summary>
            public string MonthShortName
            {
                get
                {
                    return _monthShortName;
                }
            }

            /// <summary>
            /// Marker represent two digit format of month.
            /// </summary>
            public string MonthTwoDigit
            {
                get
                {
                    return _monthTwoDigit;
                }
            }

            /// <summary>
            /// Marker represent one digit format of month.
            /// </summary>
            public string MonthOneDigit
            {
                get
                {
                    return _monthOneDigit;
                }
            }

            /// <summary>
            /// Marker represent two digit format of year.
            /// </summary>
            public string YearTwoDigit
            {
                get
                {
                    return _yearTwoDigit;
                }
            }

            /// <summary>
            /// Marker represent four digit format of year.
            /// </summary>
            public string YearFourDigit
            {
                get
                {
                    return _yearFourDigit;
                }
            }
        }
    }
}
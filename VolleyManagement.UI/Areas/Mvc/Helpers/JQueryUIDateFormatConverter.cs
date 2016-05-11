namespace VolleyManagement.UI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides methods and properties to convert UI date format to jQuery UI date format.
    /// Represent properties to get current globalization settings.
    /// </summary>
    public class JQueryUIDateFormatConverter
    {
        private static SortedDictionary<string, string> _replaceTokens = new SortedDictionary<string, string>(
                                                            new ReverseComparer<string>(Comparer<string>.Default))
        {
            { "dddd", "DD" },
            { "ddd", "D" },
            { "M", "m" },
            { "MM", "mm" },
            { "MMMM", "MM" },
            { "MMM", "M" },
            { "yy", "y" },
            { "yyyy", "yy" }
        };

        /// <summary>
        /// Gets abbreviated month names of current UI culture.
        /// </summary>
        public static HtmlString AbbreviatedMonthNames
        {
            get
            {
                return ObjectToJson(DateTimeFormat.AbbreviatedMonthNames);
            }
        }

        /// <summary>
        /// Gets month names of current UI culture.
        /// </summary>
        public static HtmlString MonthNames
        {
            get
            {
                return ObjectToJson(DateTimeFormat.MonthGenitiveNames);
            }
        }

        /// <summary>
        /// Gets day names of current UI culture.
        /// </summary>
        public static HtmlString DayNames
        {
            get
            {
                return ObjectToJson(DateTimeFormat.DayNames);
            }
        }

        /// <summary>
        /// Gets abbreviated day names of current UI culture.
        /// </summary>
        public static HtmlString AbbreviatedDayNames
        {
            get
            {
                return ObjectToJson(DateTimeFormat.AbbreviatedDayNames);
            }
        }

        /// <summary>
        /// Gets shortest month names of current UI culture.
        /// </summary>
        public static HtmlString ShortestDayNames
        {
            get
            {
                return ObjectToJson(DateTimeFormat.ShortestDayNames);
            }
        }

        /// <summary>
        /// Gets two letter language name of current UI culture.
        /// </summary>
        public static string Localization
        {
            get
            {
                return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            }
        }

        private static DateTimeFormatInfo DateTimeFormat
        {
            get
            {
                return CultureInfo.CurrentUICulture.DateTimeFormat;
            }
        }

        /// <summary>
        /// Convert current .Net Culture to the jQuery format.
        /// </summary>
        /// <returns>jQuery format of current culture.</returns>
        public static string JqueryUICurrentDateFormat()
        {
            string currentFormat = DateTimeFormat.ShortDatePattern;
            List<char> changedTypes = new List<char>();
            foreach (var kvp in _replaceTokens)
            {
                if (!changedTypes.Contains(kvp.Key[0]) && currentFormat.Contains(kvp.Key))
                {
                    changedTypes.Add(kvp.Key[0]);
                    currentFormat = currentFormat.Replace(kvp.Key, kvp.Value);
                }
            }

            return currentFormat;
        }

        private static HtmlString ObjectToJson(object obj)
        {
            return new HtmlString(JsonConvert.SerializeObject(obj, Formatting.None));
        }
    }
}
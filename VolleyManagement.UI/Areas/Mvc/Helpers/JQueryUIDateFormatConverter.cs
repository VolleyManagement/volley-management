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
        private static DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentUICulture.DateTimeFormat;

        private static SortedDictionary<string, string> replaceTokens = new SortedDictionary<string, string>(
                                                            new ReverseComparer<string>(Comparer<string>.Default))
        {
            { "dddd", "DD" }, { "ddd", "D" }, { "M", "m" }, { "MM", "mm" },  { "MMMM", "MM" }, { "MMM", "M" },
            { "yy", "y" }, { "yyyy", "yy" }
        };

        /// <summary>
        /// Gets abbreviated month names of current system culture.
        /// </summary>
        public static HtmlString AbbreviatedMonthNames
        {
            get
            {
                return new HtmlString(JsonConvert.SerializeObject(
                     dateTimeFormat.AbbreviatedMonthNames, Formatting.None));
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
                               dateTimeFormat.MonthGenitiveNames, Formatting.None));
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
                         dateTimeFormat.DayNames, Formatting.None));
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
                         dateTimeFormat.AbbreviatedDayNames, Formatting.None));
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
                         dateTimeFormat.ShortestDayNames, Formatting.None));
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
            HashSet<char> changedTypes = new HashSet<char>();
            foreach (var kvp in replaceTokens)
            {
                char ch = kvp.Key[0];
                if (!changedTypes.Contains(kvp.Key[0]) && currentFormat.Contains(kvp.Key))
                {
                    changedTypes.Add(kvp.Key[0]);
                    currentFormat = currentFormat.Replace(kvp.Key, kvp.Value);
                }
            }

            return currentFormat;
        }

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
}
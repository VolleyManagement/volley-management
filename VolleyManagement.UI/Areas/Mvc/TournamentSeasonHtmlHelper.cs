namespace VolleyManagement.UI.Areas.Mvc
{
    using System.Web.Mvc;

    /// <summary>
    /// Represent extentions of HtmlHelper class.
    /// </summary>
    public static class TournamentSeasonHtmlHelper
    {
        /// <summary>
        /// Provides the tournament season label
        /// </summary>
        /// <param name="html">HtmlHelper class</param>
        /// <param name="seasonDate">Date of season</param>
        /// <returns>html string with a displayed season</returns>
        public static MvcHtmlString DisplaySeason(this HtmlHelper html, short seasonDate)
        {
            TagBuilder tag = new TagBuilder("label");
            tag.SetInnerText(string.Format("{0}/{1}", seasonDate, ++seasonDate));
            return new MvcHtmlString(tag.ToString());
        }
    }
}
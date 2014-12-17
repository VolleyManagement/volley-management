namespace VolleyManagement.Mvc.EnumHtmlHelper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// Html helper for dropdown list with enumeration
    /// </summary>
    public static class EnumHtmlHelper
    {
        /// <summary>
        /// Creates the DropDown List (HTML Select Element) from LINQ
        /// Expression where the expression returns an Enumeration.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>MVC Html string</returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
            where TModel : class
        {
            TProperty value = htmlHelper.ViewData.Model == null
                ? default(TProperty)
                : expression.Compile()(htmlHelper.ViewData.Model);
            string selected = value == null ? string.Empty : value.ToString();
            return htmlHelper.DropDownListFor(expression, CreateSelectList(expression.ReturnType, selected));
        }

        /// <summary>
        /// Creates the select list.
        /// </summary>
        /// <param name="enumType">Type of the enumeration.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <returns>Select list items.</returns>
        private static IEnumerable<SelectListItem> CreateSelectList(Type enumType, string selectedItem)
        {
            return (from object item in Enum.GetValues(enumType)
                    let fi = enumType.GetField(item.ToString())
                    let attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault()
                    let title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description
                    select new SelectListItem
                    {
                        Value = item.ToString(),
                        Text = title,
                        Selected = selectedItem == item.ToString()
                    }).ToList();
        }
    }
}
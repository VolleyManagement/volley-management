namespace EnumHelper
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
        /// Generic method to get any type of attribute.
        /// </summary>
        /// <typeparam name="T">Type of attribute.</typeparam>
        /// <param name="value">Enumeration value.</param>
        /// <returns>Specific attribute</returns>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();
        }

        /// <summary>
        /// Method to get description for specific enumeration value.
        /// </summary>
        /// <param name="value">Enumeration value</param>
        /// <returns>Description string</returns>
        public static string ToDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

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
                    let attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault()
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
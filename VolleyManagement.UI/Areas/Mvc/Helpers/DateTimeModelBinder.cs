namespace VolleyManagement.UI.Helpers
{
    using System;
    using System.Web.Http;
    using System.Web.Mvc;

    /// <summary>
    /// Binder to convert
    /// </summary>
    public class DateTimeModelBinder : IModelBinder
    {
        /// <summary>
        /// Fixes date parsing issue.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The converted bound value or null if the raw value is null or empty or cannot be parsed.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var vpr = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (vpr == null)
            {
                return null;
            }

            var date = vpr.AttemptedValue;

            if (string.IsNullOrEmpty(date))
            {
                return null;
            }

            // Set the ModelState to the first attempted value before we have converted the date. This is to ensure that the ModelState has
            // a value. When we have converted it, we will override it with a full universal date.
            bindingContext.ModelState.SetModelValue(
                                                bindingContext.ModelName,
                                                bindingContext.ValueProvider.GetValue(bindingContext.ModelName));

            try
            {
                var realDate = DateTime.Parse(date, System.Globalization.CultureInfo.CurrentUICulture);

                // Now set the ModelState value to a full value so that it can always be parsed using InvarianCulture, which is the
                // default for QueryStringValueProvider.
                bindingContext.ModelState.SetModelValue(
                                                        bindingContext.ModelName,
                                                        new ValueProviderResult(
                                                            date,
                                                            realDate.ToString("yyyy-MM-dd hh:mm:ss"),
                                                            System.Globalization.CultureInfo.CurrentUICulture));

                return realDate;
            }
            catch (Exception)
            {
                string message = string.Format("\"{0}\" is invalid.", bindingContext.ModelName);
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, message);
                return null;
            }
        }
    }
}
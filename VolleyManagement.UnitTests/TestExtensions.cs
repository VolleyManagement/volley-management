namespace VolleyManagement.UnitTests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Web.Mvc;

    /// <summary>
    /// Class to operate with model
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class TestExtensions
    {
        /// <summary>
        /// Gets generic T model from response content
        /// </summary>
        /// <typeparam name="T">Model type</typeparam>
        /// <param name="response">Http response message</param>
        /// <returns>T model</returns>
        public static T GetModelFromResponse<T>(HttpResponseMessage response) where T : class
        {
            ObjectContent content = response.Content as ObjectContent;
            return (T)content.Value;
        }

        /// <summary>
        /// Get generic T model by ViewResult from action view
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <param name="result">object to convert and return</param>
        /// <returns>T result by ViewResult from action view</returns>
        public static T GetModel<T>(object result) where T : class
        {
            return (T)(result as ViewResult).ViewData.Model;
        }
    }
}
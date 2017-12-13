namespace VolleyManagement.UI.Areas.WebApi.Controllers
{
    using System.Collections.Generic;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    internal class DivisionTitleComparer : IEqualityComparer<DivisionTitle>
    {
        public bool Equals(DivisionTitle x, DivisionTitle y)
        {
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(DivisionTitle obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}

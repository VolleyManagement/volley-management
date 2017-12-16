namespace VolleyManagement.UI.Areas.WebApi.Controllers
{
    using System.Collections.Generic;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    internal class DivisionTitleComparer : IEqualityComparer<DivisionTitleViewModel>
    {
        public bool Equals(DivisionTitleViewModel x, DivisionTitleViewModel y)
        {
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(DivisionTitleViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}

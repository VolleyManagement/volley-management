namespace VolleyManagement.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    /// <summary>
    /// Defines AboutController which returns contributors for About page
    /// </summary>
    public class AboutController : ApiController
    {
        /// <summary>
        /// Gets all contributors
        /// </summary>
        /// <returns>All contributors</returns>
        public IEnumerable<string> Get()
        {
            string[] contributors = new string[]
            {
                "Igor Leta",
                "Ielyzaveta Kalinchuk",
                "Evgenij Kozhan",
                "Dmytro Balayev",
                "Oleg Petrushov",
                "Julia Bykova",
                "Alexandr Marchotko",
                "Pavel Goncharenko",
                "Anastacia Necheporenko",
                "Egor Mesherjakov",
                "Pavlo Dragobetskij",
                "Mariia Haponova",
                "Sofia Shaposhnik",
                "Pavel Pokhylenko",
                "Sergii Kuzmin",
                "Nikita Gordienko",
                "Ryndina Viktoriia",
                "Alex Lapin",
                "Chernyshov Dmytro",
                "Kochetkova Maria",
                "Vovkodav Oleh",
                "Alexandr Maha"
            };
            Array.Sort(contributors);
            return contributors;
        }
    }
}

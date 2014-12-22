namespace VolleyManagement.Mvc.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// UserViewModel for Create and Edit actions
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of user.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where UserName.
        /// </summary>
        /// <value>User name.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Password.
        /// </summary>
        /// <value>Password of user.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Full Name.
        /// </summary>
        /// <value>Full name of user.</value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Telephone.
        /// </summary>
        /// <value>Telephone of user.</value>
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating email of user.
        /// </summary>
        /// <value>Email of user.</value>
        public string Email { get; set; }
    }
}
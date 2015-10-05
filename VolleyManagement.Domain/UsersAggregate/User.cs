namespace VolleyManagement.Domain.Users
{
    using System;
    using VolleyManagement.Domain.Properties;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// User domain class.
    /// </summary>
    public class User
    {
        private string _userName;
        private string _password;
        private string _fullName;
        private string _cellPhone;
        private string _email;

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of user.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where UserName.
        /// </summary>
        /// <value>User name.</value>
        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                if (UserValidation.ValidateUserName(value))
                {
                    throw new ArgumentException(Resources.ValidationUserName);
                }

                _userName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Password.
        /// </summary>
        /// <value>Password of user.</value>
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (UserValidation.ValidatePassword(value))
                {
                    throw new ArgumentException(Resources.ValidationPassword);
                }

                _password = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Full Name.
        /// </summary>
        /// <value>Full name of user.</value>
        public string FullName
        {
            get
            {
                return _fullName;
            }

            set
            {
                if (UserValidation.ValidateFullName(value))
                {
                    throw new ArgumentException(Resources.ValidationFullName);
                }

                _fullName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Telephone.
        /// </summary>
        /// <value>Telephone of user.</value>
        public string CellPhone
        {
            get
            {
                return _cellPhone;
            }

            set
            {
                if (UserValidation.ValidateCellPhone(value))
                {
                    throw new ArgumentException(Resources.ValidationCellPhone);
                }

                _cellPhone = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating email of user.
        /// </summary>
        /// <value>Email of user.</value>
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                if (UserValidation.ValidateEmail(value))
                {
                    throw new ArgumentException(Resources.ValidationEmail);
                }

                _email = value;
            }
        }
    }
}

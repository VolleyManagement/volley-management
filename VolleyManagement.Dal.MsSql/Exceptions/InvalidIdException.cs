

namespace VolleyManagement.Dal.MsSql.Exceptions
{
    using System;

    class InvalidIdException : ArgumentException
    {
        public InvalidIdException():base("Specified id is not exist")
        { }
    }
}

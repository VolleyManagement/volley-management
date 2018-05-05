using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyManagement.Specs.Infrastructure
{
    public static class SpecsHelper
    {
        public static (string FirstName, string LastName) SplitFullNameToFirstLastNames(string fullName)
        {
            var whitespaceCharIndex = fullName.IndexOf(' ');
            var firstName = fullName.Substring(0, whitespaceCharIndex);
            var lastName = fullName.Substring(whitespaceCharIndex + 1);

            return (firstName, lastName);
        }
    }
}

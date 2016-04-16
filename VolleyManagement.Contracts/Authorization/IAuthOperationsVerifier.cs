using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyManagement.Domain.RolesAggregate;

namespace VolleyManagement.Contracts.Authorization
{
    public interface IAuthOperationsVerifier
    {
        bool IsAllowed(AppOperations operation);
    }
}

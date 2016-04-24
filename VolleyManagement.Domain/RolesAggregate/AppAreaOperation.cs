using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyManagement.Domain.RolesAggregate
{
    public class AppAreaOperation
    {
        public short Id { get; set; }

        public static implicit operator AppAreaOperation(short id)
        {
            return new AppAreaOperation() { Id = id };
        }
    }
}

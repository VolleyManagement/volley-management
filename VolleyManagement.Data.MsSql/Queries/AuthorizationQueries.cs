using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyManagement.Data.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Data.Queries.Common;
using VolleyManagement.Domain.RolesAggregate;

namespace VolleyManagement.Data.MsSql.Queries
{
    public class AuthorizationQueries : IQuery<List<AppOperations>, FindByIdCriteria>
    {
        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly DbSet<UserEntity> _dalUsers;
        private readonly DbSet<RoleEntity> _dalRoles;
        private readonly DbSet<RoleToOperationEntity> _dalRolesToOperations;

        public AuthorizationQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalUsers = _unitOfWork.Context.Users;
            _dalRoles = _unitOfWork.Context.Roles;
            _dalRolesToOperations = _unitOfWork.Context.RolesToFeatures;
            
        }

        public List<AppOperations> Execute(FindByIdCriteria criteria)
        {
            var data = _dalUsers.Where(u => u.Id == criteria.Id)
                                .SelectMany(u => u.Roles)
                                .Join(_dalRolesToOperations,
                                      r => r.Id,
                                      o => o.RoleId,
                                      (r, o) => o.OperationId)
                                .Distinct()
                                .AsEnumerable()
                                .Select(o => (AppOperations)o)
                                .ToList();

            return data;
        }
    }
}

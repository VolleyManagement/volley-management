﻿using System.Collections.Generic;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Infrastructure.Hardcoded
{
	public class HardcodedRolesStore : IRolesStore
	{
		private static readonly RoleId _visitor = new RoleId("visitor");
		private static readonly RoleId _sysAdmin = new RoleId("sysadmin");

		private readonly Dictionary<RoleId, Role> _roles = new Dictionary<RoleId, Role>();

		public HardcodedRolesStore()
		{
			var visitor = new Role(_visitor);
			visitor.AddPermission(new Permission("contributors", "getall"));

			_roles[_visitor] = visitor;

			var sysadmin = new Role(_sysAdmin);
			sysadmin.AddPermission(new Permission("contributors", "getall"));
			sysadmin.AddPermission(new Permission("players", "create"));
			sysadmin.AddPermission(new Permission("players", "getall"));

			_roles[_sysAdmin] = sysadmin;
		}

		public EitherAsync<Error, Role> Get(RoleId roleId)
		{
			if (_roles.TryGetValue(roleId, out var role))
			{
				return role;
			}
			return Error.NotFound();
		}
	}
}
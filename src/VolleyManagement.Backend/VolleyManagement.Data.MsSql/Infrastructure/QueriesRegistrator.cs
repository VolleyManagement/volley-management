using System;
using System.Linq;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Data.Contracts;
using VolleyManagement.Data.MsSql.Queries;

namespace VolleyManagement.Data.MsSql.Infrastructure
{
    static class QueriesRegistrator
    {
        public static void Register(IIocContainer container)
        {
            var queriesAssembly = typeof(TournamentQueries).Assembly;

            var registrations =
                from type in queriesAssembly.GetExportedTypes()
                where type.Namespace == "VolleyManagement.Data.MsSql.Queries"
                where type.GetInterfaces().Any(InterfaceIsQuery)
                select new
                {
                    Contracts = type.GetInterfaces().Where(InterfaceIsQuery),
                    Implementation = type
                };

            foreach (var item in registrations)
            {
                foreach (var contract in item.Contracts)
                {
                    container.Register(contract, item.Implementation, IocLifetimeEnum.Scoped);
                }
            }
        }

        private static bool InterfaceIsQuery(Type type)
        {
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(IQuery<,>) || typeDefinition == typeof(IQueryAsync<,>);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Data.MsSql.Context.Configurators;
using VolleyManagement.Data.MsSql.Context.Interfaces;

namespace VolleyManagement.Data.MsSql.Infrastructure
{
    static class EntityConfigurationRegistrator
    {
        internal static void Register(IIocContainer container)
        {
            container
                .Register<IVolleyManagementEntitiesConfigurator, VolleyManagementEntitiesConfigurator>(IocLifetimeEnum.Scoped)
                .Register<IUserEntitiesConfigurator, UserEntitiesConfigurator>(IocLifetimeEnum.Scoped)
                .Register<IGameDataEntitiesConfigurator, GameDataEntitiesConfigurator>(IocLifetimeEnum.Scoped)
                .Register<IGameParticipantEntitiesConfigurator, GameParticipantEntitiesConfigurator>(IocLifetimeEnum.Scoped)
                .Register<IEntityRelationshipConfigurator, EntityRelationshipConfigurator>(IocLifetimeEnum.Scoped);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players;

namespace VolleyM.Infrastructure.Players
{
    public class GetAllPlayersQuery : IQuery<TenantId, List<PlayerDto>>
    {
        public Task<Either<Error, List<PlayerDto>>> Execute(TenantId param)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using reexmonkey.xmisc.backbone.repositories.contracts;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using System;

namespace Speechless.Core.Repositories.Contracts
{
    /// <summary>
    /// Specifies a repository for <see cref="BusinessCard"/> items.
    /// </summary>
    public interface IBusinessCardRepository :
        IReadRepository<Guid, BusinessCard>,
        IWriteRepository<Guid, BusinessCard>,
        IEraseRepository<Guid, BusinessCard>,
        ITrashRepository<Guid, BusinessCard>,
        IRegistryRepository<Guid, BusinessCard>
        {
        }
}
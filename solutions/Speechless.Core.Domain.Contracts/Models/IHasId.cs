using System;
using System.Collections.Generic;
using System.Text;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Models
{
    public interface IHasId<TKey>
    {
        TKey Id { get; }
    }
}

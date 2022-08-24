using System;
using System.Collections.Generic;
using System.Text;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Models
{
    public interface IHasExpiry
    {
        public DateTime Expiry { get; set; }
    }
}

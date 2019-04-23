using System;
using System.Collections.Generic;
using System.Text;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Models
{
    /// <summary>
    /// Specifies an interface that adds a time period to an object to show when it is effective.
    /// <para /> Please consult https://martinfowler.com/eaaDev/Effectivity.html
    /// </summary>
    public interface ITemporal
    {
        /// <summary>
        /// The date range in which the object is valid.
        /// </summary>
        Period? Effective { get; }
    }
}

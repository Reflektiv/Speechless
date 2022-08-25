using System;
using System.Collections.Generic;
using System.Text;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Models
{
    /// <summary>
    /// Specifies a trash bin for temporarily deleted items.
    /// </summary>
    public interface ITrashable
    {
        /// <summary>
        /// Gets or sets whether an item has been temporarily deleted (soft-delete).
        /// <para/>
        /// true if it has been temporarily deleted; otherwise false.
        /// </summary>
        bool IsTrashed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Models
{
    /// <summary>
    /// Specifies an interface for an object that has a unique identifier.
    /// </summary>
    /// <typeparam name="TKey">The type of unique identifier.</typeparam>
    public interface IHasId<TKey>
        where TKey: IComparable<TKey>, IComparable, IEquatable<TKey>
    {
        /// <summary>
        /// Gets the unique identifier of the object.
        /// </summary>
        TKey Id { get; }
    }

}

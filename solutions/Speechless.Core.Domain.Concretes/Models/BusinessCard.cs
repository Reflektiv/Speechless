using MixERP.Net.VCards;
using Reflektiv.Speechless.Core.Domain.Concretes.Interfaces;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    /// <summary>
    /// Represents a business contact card.
    /// </summary>
    public class BusinessCard : VCard, IHasId<Guid>, ITrashable, IEquatable<BusinessCard>
    {
        /// <summary>
        /// The unique identifier of the business card.
        /// </summary>
        public Guid Id
        {
            get => Guid.TryParse(UniqueIdentifier, out Guid guid) ? guid : Guid.Empty;
            set => UniqueIdentifier = value.ToString("D");
        }

        /// <summary>
        /// The custom label (human-friendly identifier) for the business card.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// The date of creation of the business card.
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// The date when the business card was last modified.
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// The date of expiration of the business card. After this date, the business card becomes invalid.
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Labels to identify or provide extra information about the business card.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets whether an item has been temporarily deleted (soft-delete).
        /// <para/>
        /// true if it has been temporarily deleted; otherwise false.
        /// </summary>
        public bool IsTrashed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCard"/> class.
        /// </summary>
        public BusinessCard()
        {
            Tags = new List<string>();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BusinessCard);
        }

        public bool Equals(BusinessCard? other)
        {
            return other is not null &&
                   Title == other.Title &&
                   LastName == other.LastName &&
                   FirstName == other.FirstName &&
                   MiddleName == other.MiddleName &&
                   Prefix == other.Prefix &&
                   Suffix == other.Suffix &&
                   Id.Equals(other.Id);
        }

        public override int GetHashCode() => HashCode.Combine(Title, LastName, FirstName, MiddleName, Prefix, Suffix, Id);

        public static bool operator ==(BusinessCard? left, BusinessCard? right) => EqualityComparer<BusinessCard>.Default.Equals(left, right);

        public static bool operator !=(BusinessCard? left, BusinessCard? right) => !(left == right);
    }
}
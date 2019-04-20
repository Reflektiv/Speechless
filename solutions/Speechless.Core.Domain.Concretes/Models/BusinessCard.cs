using MixERP.Net.VCards;
using Reflektiv.Speechless.Core.Domain.Contracts.Models;
using System;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    /// <summary>
    /// Represents a business card.
    /// </summary>
    public class BusinessCard : VCard, IHasId<Guid>, IBusinessCard, ITemporal, ITrashable
    {
        /// <summary>
        /// Gets or sets the unique identifier of the business card.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a custom label (human-friendly identifier) for the business card.
        /// </summary>
        public string Label { get; set; }

        public Period? Effective { get; set; }

        public bool IsTrashed { get; set; }

        public BusinessCard()
        {
            UniqueIdentifier = Id.ToString();
        }


        public BusinessCard(IBusinessCard other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            Import(other);
        }

        public void Import(IBusinessCard other)
        {
            Label = other.Label;
            if (other is ITemporal temporal)
                Effective = temporal.Effective;
            if (other is ITrashable trashable)
                IsTrashed = trashable.IsTrashed;
        }
    }
}
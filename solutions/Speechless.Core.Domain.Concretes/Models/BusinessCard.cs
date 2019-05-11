using MixERP.Net.VCards;
using MixERP.Net.VCards.Models;
using Reflektiv.Speechless.Core.Domain.Contracts.Extensions;
using Reflektiv.Speechless.Core.Domain.Contracts.Models;
using System;
using System.Collections.Generic;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    /// <summary>
    /// Represents a business card.
    /// </summary>
    public class BusinessCard : VCard, IBusinessCard, IHasId<Guid>, ITemporal, ITrash
    {
        /// <summary>
        /// Gets or sets the unique identifier of the business card.
        /// </summary>
        public Guid Id
        {
            get => Guid.TryParse(UniqueIdentifier, out Guid guid) ? guid : Guid.Empty;
            set => UniqueIdentifier = value.ToString("D");
        }   

        /// <summary>
        /// Gets or sets a custom label (human-friendly identifier) for the business card.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the date range in which the business card is valid.
        /// </summary>
        public Period? Effective { get; set; }

        /// <summary>
        /// Gets or sets whether an item has been temporarily deleted (soft-delete).
        /// <para/> true if it has been temporarily deleted; otherwise false.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCard"/> class.
        /// </summary>
        public BusinessCard()
        {
            Impps = new List<Impp>();
            Languages = new List<Language>();
            Relations = new List<Relation>();
            CalendarUserAddresses = new List<Uri>();
            CalendarAddresses = new List<Uri>();
            Addresses = new List<Address>();
            Telephones = new List<Telephone>();
            Emails = new List<Email>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCard"/> class from another.
        /// </summary>
        /// <param name="other">The instance of <see cref="IBusinessCard"/> to initialize this instance with.</param>
        public BusinessCard(IBusinessCard other) : this()
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            Import(other);
        }

        private void Import(VCard card)
        {
            FirstName = card.FirstName;
            MiddleName = card.MiddleName;
            Prefix = card.Prefix;
            Suffix = card.Suffix;
            NickName = card.NickName;
            card.Categories.SafeCopyTo(Categories);
            SortString = card.SortString;
            Sound = card.Sound;
            LastName = card.LastName;
            Key = card.Key;
            Source = card.Source;
            Kind = card.Kind;
            Anniversary = card.Anniversary;
            Gender = card.Gender;
            card.Impps.SafeCopyTo(Impps as List<Impp>);
            card.Languages.SafeCopyTo(Languages as List<Language>);
            card.Relations.SafeCopyTo(Relations as List<Relation>);
            card.CalendarUserAddresses.SafeCopyTo(CalendarUserAddresses as List<Uri>);
            Classification = card.Classification;
            card.CalendarAddresses.SafeCopyTo(CalendarAddresses as List<Uri>);
            FormattedName = card.FormattedName;
            Longitude = card.Longitude;
            BirthDay = card.BirthDay;
            card.Addresses.SafeCopyTo(Addresses as List<Address>);
            DeliveryAddress = card.DeliveryAddress;
            card.Telephones.SafeCopyTo(Telephones as List<Telephone>);
            card.Emails.SafeCopyTo(Emails as List<Email>);
            Mailer = card.Mailer;
            Title = card.Title;
            Role = card.Role;
            Latitude = card.Latitude;
            TimeZone = card.TimeZone;
            Photo = card.Photo;
            Note = card.Note;
            LastRevision = card.LastRevision;
            Url = card.Url;
            UniqueIdentifier = card.UniqueIdentifier;
            Version = card.Version;
            Organization = card.Organization;
            OrganizationalUnit = card.OrganizationalUnit;
            Logo = card.Logo;
            card.CustomExtensions.SafeCopyTo(CustomExtensions as List<CustomExtension>);
        }

        /// <summary>
        /// Imports data from the specified business card.
        /// </summary>
        /// <param name="other">The business card, to import data from.</param>
        public void Import(IBusinessCard other)
        {
            Label = other.Label;
            if (other is VCard card) Import(card);
            if (other is ITemporal temporal) Effective = temporal.Effective;
            if (other is ITrash trashable) IsDeleted = trashable.IsDeleted;
        }
    }
}
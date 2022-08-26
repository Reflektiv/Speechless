using MixERP.Net.VCards;
using MixERP.Net.VCards.Serializer;
using reexmonkey.xmisc.core.system.xmltools.extensions;
using Reflektiv.Speechless.Core.Domain.Concretes.Extensions;
using Reflektiv.Speechless.Core.Domain.Concretes.Interfaces;
using ServiceStack;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    /// <summary>
    /// Represents a business contact card.
    /// </summary>
    public class BusinessCard : VCard, IHasId<Guid>, ITrashable, IEquatable<BusinessCard>, IXmlSerializable
    {
        public const string Namespace = "https://github.com/reflektiv/speechless";

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

        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            var subreader = reader.ReadSubtree();

            if (subreader.LocalName.EqualsIgnoreCase(nameof(BusinessCard)))
            {
                for (int i = 0; i < subreader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    if (subreader.LocalName.EqualsIgnoreCase("id"))
                    {
                        Id = new Guid(subreader.Value);
                        break;
                    }
                }
            }

            while (subreader.Read())
            {
                if (subreader.NodeType != XmlNodeType.Element) continue;

                if (reader.LocalName.EqualsIgnoreCase(nameof(DisplayName)))
                    DisplayName = reader.SafeReadElementContentAsString();

                if (reader.LocalName.EqualsIgnoreCase(nameof(CreatedDate)))
                    CreatedDate = reader.SafeReadElementContentAsNullableDateTime(XmlDateTimeSerializationMode.Utc);

                if (reader.LocalName.EqualsIgnoreCase(nameof(LastModifiedDate)))
                    LastModifiedDate = reader.SafeReadElementContentAsNullableDateTime(XmlDateTimeSerializationMode.Utc);

                if (reader.LocalName.EqualsIgnoreCase(nameof(ExpiryDate)))
                    ExpiryDate = reader.SafeReadElementContentAsNullableDateTime(XmlDateTimeSerializationMode.Utc);

                if (reader.LocalName.EqualsIgnoreCase(nameof(Tags)))
                {
                    var value = reader.SafeReadElementContentAsString();
                    var tags = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tags) Tags.Add(tag);
                }

                if (reader.LocalName.EqualsIgnoreCase(nameof(IsTrashed)))
                    IsTrashed = reader.SafeReadElementContentAsBoolean();

                if (reader.LocalName.EqualsIgnoreCase(nameof(VCard)))
                {
                    var content = reader.SafeReadElementContentAsString();
                    var vcard = Deserializer.GetVCard(content);
                    if (vcard != null) this.PopulateFrom(vcard);
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            const string prefix = "bc";

            writer.SafeWriteAttributeString(prefix, nameof(BusinessCard), Namespace, Id.ToString());
            if (!string.IsNullOrEmpty(DisplayName))
                writer.SafeWriteElementString(prefix, nameof(DisplayName), Namespace, DisplayName);
            if (CreatedDate.HasValue)
                writer.SafeWriteElementString(prefix, nameof(CreatedDate), Namespace, CreatedDate.Value, XmlDateTimeSerializationMode.Utc);
            if (LastModifiedDate.HasValue)
                writer.SafeWriteElementString(prefix, nameof(LastModifiedDate), Namespace, LastModifiedDate.Value, XmlDateTimeSerializationMode.Utc);
            if (ExpiryDate.HasValue)
                writer.SafeWriteElementString(prefix, nameof(ExpiryDate), Namespace, ExpiryDate.Value, XmlDateTimeSerializationMode.Utc);
            if (Tags.Any())
                writer.SafeWriteElementString(prefix, nameof(Tags), Namespace, string.Join(';', Tags));
            if (IsTrashed)
                writer.SafeWriteElementString(prefix, nameof(IsTrashed), Namespace, IsTrashed);

            var vcard = this.Serialize();
            if (!string.IsNullOrEmpty(vcard))
                writer.SafeWriteElementString(prefix, nameof(VCard), Namespace, vcard);
        }

        public static bool operator ==(BusinessCard? left, BusinessCard? right) => EqualityComparer<BusinessCard>.Default.Equals(left, right);

        public static bool operator !=(BusinessCard? left, BusinessCard? right) => !(left == right);
    }
}
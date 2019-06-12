using MixERP.Net.VCards.Models;
using MixERP.Net.VCards.Types;
using Reflektiv.Speechless.Core.Domain.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    public class VCardAddress : IHasId<Guid>
    {
        private readonly Address address;

        public Guid Id { get; set; }

        public AddressType Type { get => address.Type; set => address.Type = value; }
        public string PoBox { get => address.PoBox; set => PoBox = value; }

        public string ExtendedAddress { get => address.ExtendedAddress; set => address.ExtendedAddress = value; }
        public string Street { get => address.Street ; set => address.Street = value; }

        public string Locality { get => address.Locality; set => address.Locality = value; }
        public string Region { get => address.Region; set => address.Region = value; }
        public string PostalCode { get => address.PostalCode; set => address.PostalCode = value; }
        public string Country { get => address.Country; set => address.Country = value; }
        public double? Longitude { get => address.Longitude; set => address.Longitude = value; }
        public double? Latitude { get => address.Latitude; set => address.Latitude = value; }
        public string Label { get => address.Label; set => address.Label = value; }
        public TimeZoneInfo TimeZone { get => address.TimeZone ; set => address.TimeZone = value; }
        public int Preference { get => address.Preference; set => address.Preference = value; }

        public IEquatable<CustomExtension> Extensions
        {
            get => address.Extensions;
            set => address.Extensions = value;
        }

        public VCardAddress()
        {
            address = new Address();
        }

        public VCardAddress(Address address)
        {
            this.address = address ?? throw new ArgumentNullException(nameof(address));

            VCardAddress address1 = new VCardAddress();
            Address address2 = new Address();
        }

        public static implicit operator VCardAddress(Address address) => new VCardAddress(address);

        public static explicit operator Address(VCardAddress address)
            => new Address
            {
                Type = address.Type,
                PoBox = address.PoBox,
                ExtendedAddress = address.ExtendedAddress,
                Street = address.Street,
                Locality = address.Locality,
                Region = address.Region,
                PostalCode = address.PostalCode,
                Country = address.Country,
                Longitude = address.Longitude,
                Latitude = address.Latitude,
                Label = address.Label,
                TimeZone = address.TimeZone,
                Preference = address.Preference
            };
    }
}

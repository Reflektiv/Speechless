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

        public AddressType Type { get; set; }
        public string PoBox { get; set; }

        public string ExtendedAddress { get; set; }
        public string Street { get; set; }

        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Label { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
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

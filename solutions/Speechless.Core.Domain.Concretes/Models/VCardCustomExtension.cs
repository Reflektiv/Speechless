using MixERP.Net.VCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    public class VCardCustomExtension
    {
        private CustomExtension extension;

        public Guid Id { get; set; }

        public string Key { get => extension.Key; set => extension.Key = value; }

        public List<string> Values
        {
            get => extension.Values as List<string> ?? extension.Values.ToList();
            set => extension.Values = value;
        }

        public string Value { set => extension.Value = value; }

        public VCardCustomExtension()
        {
            extension = new CustomExtension();
        }

        public VCardCustomExtension(CustomExtension extension)
        {
            this.extension = extension ?? throw new ArgumentNullException(nameof(extension));
        }

        public static implicit operator VCardCustomExtension(CustomExtension extension)
            => new VCardCustomExtension(extension);

        public static explicit operator CustomExtension(VCardCustomExtension extension)
            => new CustomExtension
            {
                Key = extension.Key,
                Values = extension.Values
            };
    }
}

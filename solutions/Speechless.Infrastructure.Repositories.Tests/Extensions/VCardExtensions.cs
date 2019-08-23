using MixERP.Net.VCards.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reflektiv.Speechless.Infrastructure.Repositories.Tests.Extensions
{
    public static class VCardExtensions
    {
        public static string AsString(this Address address)
        {
            return $"{address.Street}, {address.PostalCode}, {address.Locality}, {address.Region}, { address.Country }";
        }

        public static string GetImageAsBase64(this string path)
        {
            var bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }
    }
}

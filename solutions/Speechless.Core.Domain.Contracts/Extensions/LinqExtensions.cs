using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Extensions
{
    public static class LinqExtensions
    {
        public static void SafeCopyTo<T>(this IEnumerable<T> source, List<T> destination)
        {
            if (source != null && source.Any())
                destination.AddRange(source);
        }

        public static void SafeCopyTo<T>(this T[] source, T[] destination)
        {
            if (source != null && source.Any())
            {
                if (destination == null || destination.Length != source.Length)
                    destination = new T[source.Length];

                Buffer.BlockCopy(source, 0, destination, 0, source.Length);
            }
        }
    }
}
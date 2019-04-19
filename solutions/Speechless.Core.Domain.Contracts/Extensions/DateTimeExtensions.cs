using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime Min(this DateTime date, DateTime other)
            => date < other ? date : other;

        public static DateTime Max(this DateTime date, DateTime other)
            => date > other ? date : other;

        public static DateTime Min(this DateTime date, IEnumerable<DateTime> others)
        {
            DateTime result = default;
            foreach (var other in others)
            {
                result = date.Min(other);
            }
            return result;

        }

        public static DateTime Max(this DateTime date, IEnumerable<DateTime> others)
        {
            DateTime result = default;
            foreach (var other in others)
            {
                result = date.Max(other);
            }
            return result;
        }

    }
}
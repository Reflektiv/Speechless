using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime Earlier(this DateTime date, DateTime other)
            => date < other ? date : other;

        public static DateTime Later(this DateTime date, DateTime other)
            => date > other ? date : other;

        public static DateTime Earlier(this DateTime date, IEnumerable<DateTime> others)
        {
            DateTime result = default;
            foreach (var other in others)
            {
                result = date.Earlier(other);
            }
            return result;

        }

        public static DateTime Later(this DateTime date, IEnumerable<DateTime> others)
        {
            DateTime result = default;
            foreach (var other in others)
            {
                result = date.Later(other);
            }
            return result;
        }

    }
}
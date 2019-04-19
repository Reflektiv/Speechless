using Reflektiv.Speechless.Core.Domain.Contracts.Extensions;
using System;
using System.Collections.Generic;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Models
{
    /// <summary>
    /// Represents a date time range.
    /// </summary>
    public struct Period : IEquatable<Period>
    {
        /// <summary>
        /// Gets the start of the period.
        /// </summary>
        public DateTime Start { get; }

        /// <summary>
        /// Gets the end of the period.
        /// </summary>
        public DateTime End { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Period"/> struct.
        /// </summary>
        /// <param name="start">The start of the period./param>
        /// <param name="end">The end of the period.</param>
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool Equals(Period other) => Start == other.Start && End == other.End;

        public override bool Equals(object obj)
        {
            return obj is Period period &&
                   Start == period.Start &&
                   End == period.End;
        }

        public override int GetHashCode()
        {
            var hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + End.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Period left, Period right) => left.Equals(right);

        public static bool operator !=(Period left, Period right) => !left.Equals(right);

     
    }
}
﻿using Reflektiv.Speechless.Core.Domain.Contracts.Models;
using System;
using System.Collections.Generic;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Extensions
{
    public static class PeriodExtensions
    {
        public static Period Add(this Period period, Period other)
        {
            var start = period.Start.Earlier(other.Start);
            var end = period.End.Later(other.End);
            return new Period(start, end);
        }

        public static Period Add(this Period period, IEnumerable<Period> others)
        {
            DateTime start = default;
            DateTime end = default;
            foreach (var other in others)
            {
                start = period.Start.Earlier(other.Start);
                end = period.Start.Later(other.End);
            }
            return new Period(start, end);
        }

        public static Period Negate(this Period period) => new Period(period.End, period.Start);
    }
}
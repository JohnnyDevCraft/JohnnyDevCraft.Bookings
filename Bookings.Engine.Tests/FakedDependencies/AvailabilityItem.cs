using System;
using System.Collections.Generic;
using Bookings.Engine.Abstractions.Core;

namespace Bookings.Engine.Tests.FakedDependencies
{
    public class AvailabilityItem: IAvailabilityItem
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<DayOfWeek> AvailableDays { get; set; }
        public int SimultaneousLimit { get; set; }
        public decimal PriceOverride { get; set; }
    }
}
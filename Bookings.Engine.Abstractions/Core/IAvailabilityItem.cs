using System;
using System.Collections.Generic;

namespace Bookings.Engine.Abstractions.Core
{
  public interface IAvailabilityItem
  {
    TimeSpan StartTime { get; set; }
    TimeSpan EndTime { get; set; }
    List<DayOfWeek> AvailableDays { get; set; }
    int SimultaneousLimit { get; set; }
    decimal PriceOverride { get; set; }
  }
}
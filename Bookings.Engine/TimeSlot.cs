using System;
using Bookings.Engine.Abstractions.Core;

namespace Bookings.Engine
{
  public class TimeSlot: ITimeSlot
  {
    public string TypeIdentifier { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal Price { get; set; }
  }
}
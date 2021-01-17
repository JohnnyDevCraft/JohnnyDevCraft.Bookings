using System;

namespace Bookings.Engine.Abstractions.Core
{
  public interface ITimeSlot
  {
    string TypeIdentifier { get; set; }
    DateTime Start { get; set; }
    DateTime End { get; set; }
    decimal Price { get; set; }
  }
}
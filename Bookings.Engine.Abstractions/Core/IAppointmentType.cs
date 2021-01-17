using System;
using System.Collections.Generic;

namespace Bookings.Engine.Abstractions.Core
{
  public interface IAppointmentType
  {
    string Identity { get; }
    string Name { get; set; }
    TimeSpan Duration { get; set; }
    decimal Price { get; set; }
    List<IAvailabilityItem>  Availability { get; set; }
  }
}
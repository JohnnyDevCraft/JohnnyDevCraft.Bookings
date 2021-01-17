using System;

namespace Bookings.Engine.Abstractions.Core
{
  public interface IAppointment
  {
    DateTime StartTime { get; set; }
    TimeSpan Duration { get; set; }
    string CustomerIdentity { get; }
    string PaymentIdentity { get; }
    IAvailabilityItem AvailabilityItem { get; set; }
  }
}
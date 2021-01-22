using System;

namespace Bookings.Engine.Abstractions.Core
{
  public interface IAppointment<TAppointmentType, TAvailItem> 
    where TAppointmentType: class, IAppointmentType<TAvailItem>
    where TAvailItem: class, IAvailabilityItem
  {
    DateTime StartTime { get; set; }
    TimeSpan Duration { get; set; }
    string CustomerIdentity { get; }
    string PaymentIdentity { get; }
    TAppointmentType AppointmentType { get; set; }
  }
}
using System;
using System.Collections.Generic;

namespace Bookings.Engine.Abstractions.Core
{
  public interface IBookingsManager<TAppointment, TAppointmentType, out TTimeSlot, TAvailItem> 
    where TAppointment: class, IAppointment<TAppointmentType, TAvailItem>
    where TAppointmentType: class, IAppointmentType<TAvailItem>
    where TTimeSlot: class, ITimeSlot
    where TAvailItem: class, IAvailabilityItem
  {
    IEnumerable<TTimeSlot> GetTimeSlots(DateTime day, string availabilityName);
    List<DateTime> GetAvailableDates(DateTime startDate, DateTime endDate, string identity);
    TAppointment SaveAppointment(TAppointment appointment);
  }
}
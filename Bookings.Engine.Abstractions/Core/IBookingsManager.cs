using System;
using System.Collections.Generic;

namespace Bookings.Engine.Abstractions.Core
{
  public interface IBookingsManager
  {
    List<ITimeSlot> GetTimeSlots(DateTime day, string availabilityName);
    List<DateTime> GetAvailableDates(DateTime startDate, DateTime endDate);
    IAppointment SaveAppointment(IAppointment);
  }
}
using System;
using System.Collections.Generic;

namespace Bookings.Engine.Abstractions.Core
{
  public interface IBookingRepository<T, TType, TItem> 
    where T: class, IAppointment<TType, TItem> 
    where TType: class, IAppointmentType<TItem>
  where TItem: class, IAvailabilityItem
  {
    T SaveAppointment(T appointment);
    bool CancelAppointment(T appointment);
    IEnumerable<T> GetAppointmentsByDate(DateTime date, TType appointmentType);
    TType GetAppointmentTypeByStringIdentity(string identity);
  }
}
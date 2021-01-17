using System;
using System.Collections.Generic;

namespace Bookings.Engine.Abstractions.Core
{
  public interface IBookingRepository
  {
    IAppointment SaveAppointment(IAppointment appointment);
    bool CancelAppointment(IAppointment appointment);
    List<IAppointment> GetAppointmentsByDate(DateTime date, IAppointmentType appointmentType);
    IAppointmentType GetAppointmentTypeByStringIdentity(string identity);
  }
}
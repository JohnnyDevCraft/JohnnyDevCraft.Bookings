using System;
using System.Collections.Generic;
using System.Linq;
using Bookings.Engine.Abstractions.Core;

namespace Bookings.Engine
{
  public class BookingsManager: IBookingsManager
  {
    private readonly IBookingRepository repo;
    private readonly BookingConfiguration config;

    public BookingsManager(IBookingRepository repo, BookingConfiguration config)
    {
      this.repo = repo;
      this.config = config;
    }

    public List<ITimeSlot> GetTimeSlots(DateTime day, string availabilityName)
    {
      var type = repo.GetAppointmentTypeByStringIdentity(availabilityName);
      var appointments = repo.GetAppointmentsByDate(day, type);

      var availability = GetAvailableTimesForDateAndType(type, day);
      var unfilteredSlots = GetPossibleTimeSlots(day, availability);

      var usableSlots = AvailableTimes(unfilteredSlots, availability);
      var slots = AppointmentsNotMaxed(usableSlots, appointments, type);

    }

    private List<ITimeSlot> AppointmentsNotMaxed(List<ITimeSlot> usableSlots, List<IAppointment> appointments, IAppointmentType type)
    {
      foreach (var timeSlot in usableSlots)
      {
        var apptCounter = 0;
        var item = GetAvailabilityITemForSlot(type, timeSlot);

        foreach (var appointment in appointments)
        {
          if(appointment.StartTime >= timeSlot.Start && a)
        }

      }

      
    }

    private IAvailabilityItem GetAvailabilityITemForSlot(IAppointmentType type, ITimeSlot timeSlot)
    {
      return type.Availability.SingleOrDefault(x =>
        timeSlot.Start.TimeOfDay >= x.StartTime && timeSlot.End.TimeOfDay <= x.EndTime &&
        x.AvailableDays.Any(d => d == timeSlot.Start.DayOfWeek));
    }

    private List<ITimeSlot> AvailableTimes(List<ITimeSlot> unfilteredSlots, List<IAvailabilityItem> availability)
    {
      return unfilteredSlots.Where(x=> TimeIsAvailable(x, availability)).ToList();
    }

    private List<IAvailabilityItem> GetAvailableTimesForDateAndType(IAppointmentType type, DateTime date)
    {
      return type.Availability.Where(x => x.AvailableDays.Contains(date.DayOfWeek)).ToList();
    }

    private (TimeSpan, TimeSpan) GetTimeRangeForDateAndType(DateTime date, List<IAvailabilityItem> availabilityItems)
    {
      var startTime = availabilityItems.Min(x => x.StartTime);
      var endTime = availabilityItems.Max(x => x.EndTime);
      return (startTime, endTime);
    }

    private List<ITimeSlot> GetPossibleTimeSlots(DateTime date, List<IAvailabilityItem> items)
    {
      var range = GetTimeRangeForDateAndType(date, items);
      var list = new List<ITimeSlot>();

      while (range.Item1 < range.Item2)
      {
        var endTime = range.Item1 + config.TimeBlockLength;

        list.Add(new TimeSlot()
        {
          End = date.Date.AddHours(endTime.Hours).AddMinutes(endTime.Minutes),
          Start = date.Date.AddHours(range.Item1.Hours).AddMinutes(range.Item1.Minutes)
        });

        range.Item1 = endTime;
      }

      return list;
    }

    private bool AppointmentNotMaxed()
    {

    }

    private bool TimeIsAvailable(ITimeSlot timeSlot, List<IAvailabilityItem> availabilityItems)
    {
      return availabilityItems.Any(x =>
        timeSlot.Start.TimeOfDay >= x.StartTime && timeSlot.End.TimeOfDay <= x.EndTime);
    }

    public List<DateTime> GetAvailableDates(DateTime startDate, DateTime endDate)
    {
      throw new NotImplementedException();
    }

    public IAppointment SaveAppointment(IAppointment missing_name)
    {
      throw new NotImplementedException();
    }
  }
}
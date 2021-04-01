using System;
using System.Collections.Generic;
using System.Linq;
using Bookings.Engine.Abstractions.Core;

namespace Bookings.Engine
{
  public class BookingsManager<TAppointment, TAppointmentType, TAvailItem> : IBookingsManager<TAppointment, TAppointmentType, TimeSlot, TAvailItem>
    where TAppointment : class, IAppointment<TAppointmentType, TAvailItem>
    where TAppointmentType : class, IAppointmentType<TAvailItem>
    where TAvailItem : class, IAvailabilityItem
  {
    private readonly IBookingRepository<TAppointment, TAppointmentType, TAvailItem> repo;
    private readonly BookingConfiguration config;

    public BookingsManager(IBookingRepository<TAppointment, TAppointmentType, TAvailItem> repo, BookingConfiguration config)
    {
      this.repo = repo;
      this.config = config;
    }

    public IEnumerable<TimeSlot> GetTimeSlots(DateTime day, string availabilityName, TimeSpan duration)
    {
      var type = repo.GetAppointmentTypeByStringIdentity(availabilityName);
      var appointments = repo.GetAppointmentsByDate(day, type);

      var availability = GetAvailableTimesForDateAndType(type, day);
      var availabilityItems = availability.ToList();

      if (!availabilityItems.Any()) return new List<TimeSlot>();

      var unfilteredSlots = GetPossibleTimeSlots(day, availabilityItems);
      
      var timeSlots = unfilteredSlots.ToList();
      if (!timeSlots.Any()) return new List<TimeSlot>();

      var availableSlots = AvailableTimes(timeSlots, availabilityItems);
      var usableSlots = UsableSlots(availableSlots, availabilityItems, duration);
      var slots = AppointmentsNotMaxed(usableSlots, appointments, type);

      return slots;

    }

    private IEnumerable<TimeSlot> AppointmentsNotMaxed(IEnumerable<TimeSlot> usableSlots, IEnumerable<TAppointment> appointments, TAppointmentType type)
    {
      var availabilities = type.Availability;
      var timeSlots = usableSlots.ToList();
      var appointmentList = appointments.ToList();
      var duration = type.Duration;
      
      var result = new List<TimeSlot>();

      foreach (var item in availabilities)
      {
        foreach (var slot in timeSlots)
        {
          if (TimeSlotFallsInAvailability(slot, item))
          {
            var counter = 0;

            foreach (var appointment in appointmentList)
            {
              if (AppointmentFallsInTimeslot(slot, appointment, duration))
              {
                counter += 1;
              }
            }

            if (counter < item.SimultaneousLimit)
            {
              result.Add(slot);
            }
          }
        }
      }

      return result;

    }

    private static IAvailabilityItem GetAvailabilityItemForTimeSlot(TAppointmentType type, TimeSlot timeSlot)
    {
      IAvailabilityItem avail = null;

      type.Availability.ForEach(a =>
      {
        if (TimeSlotFallsInAvailability(timeSlot, a))
        {
          avail = a;
        }
      });

      return avail;
    }

    private static bool TimeSlotInAvailability(TimeSlot timeSlot, IAvailabilityItem availabilityItem)
    {
      var aStart = availabilityItem.StartTime;
      var aEnd = availabilityItem.EndTime;
      var tsStart = timeSlot.Start.TimeOfDay;
      var tsEnd = timeSlot.End.TimeOfDay;

      return tsStart >= aStart && tsEnd <= aEnd;
    }

    private bool AppointmentFallsInTimeslot(TimeSlot timeSlot, TAppointment appointment, TimeSpan duration)
    {
      var tsStart = timeSlot.Start;
      var tsEnd = timeSlot.Start.Add(duration);
      var aStart = appointment.StartTime;
      var aEnd = appointment.StartTime + appointment.Duration;

      if (aStart >= tsStart && aStart < tsEnd)
      {
        return true;
      }

      if (aEnd > tsStart && aEnd <= tsEnd)
      {
        return true;
      }

      if (aStart < tsStart && aEnd > tsEnd)
      {
        return true;
      }

      return aStart < tsStart && aEnd > tsEnd;
    }

    private static bool TimeSlotFallsInAvailability(TimeSlot timeSlot, TAvailItem availItem)
    {
      var slotStart = timeSlot.Start.TimeOfDay;
      var slotEnd = timeSlot.End.TimeOfDay;
      var aStart = availItem.StartTime;
      var aEnd = availItem.EndTime;

      if (slotStart >= aStart && slotStart < aEnd)
      {
        return true;
      }

      if (slotEnd > aStart && slotEnd <= aEnd)
      {
        return true;
      }

      if (slotStart < aStart && slotEnd > aEnd)
      {
        return true;
      }

      return slotStart < aStart && slotEnd > aEnd;
    }

    private IAvailabilityItem GetAvailabilityItemForSlot(TAppointmentType type, TimeSlot timeSlot)
    {
      return type.Availability.SingleOrDefault(x =>
        timeSlot.Start.TimeOfDay >= x.StartTime && timeSlot.End.TimeOfDay <= x.EndTime &&
        x.AvailableDays.Any(d => d == timeSlot.Start.DayOfWeek));
    }

    private static IEnumerable<TimeSlot> AvailableTimes(IEnumerable<TimeSlot> unfilteredSlots, IEnumerable<IAvailabilityItem> availability)
    {
      return unfilteredSlots.Where(x => TimeIsAvailable(x, availability)).ToList();
    }

    private static IEnumerable<TimeSlot> UsableSlots(IEnumerable<TimeSlot> unfilteredSlots, IEnumerable<IAvailabilityItem> availability, TimeSpan duration)
    {
      return unfilteredSlots.Where(x => availability.Any(y => x.Start.TimeOfDay >= y.StartTime && x.Start.Add(duration).TimeOfDay <= y.EndTime));
    }

    private static IEnumerable<IAvailabilityItem> GetAvailableTimesForDateAndType(TAppointmentType type, DateTime date)
    {
      return type.Availability.Where(x => x.AvailableDays.Contains(date.DayOfWeek)).ToList();
    }

    private static (TimeSpan, TimeSpan) GetTimeRangeForDateAndType(DateTime date, IEnumerable<IAvailabilityItem> availabilityItems)
    {
      var items = availabilityItems.ToList();
      var day = date.DayOfWeek;

      var startTime = items.Where(i => i.AvailableDays.Contains(day)).Min(x => x.StartTime);
      var endTime = items.Where(i => i.AvailableDays.Contains(day)).Max(x => x.EndTime);

      return (startTime, endTime);
    }

    private IEnumerable<TimeSlot> GetPossibleTimeSlots(DateTime date, IEnumerable<IAvailabilityItem> items)
    {
      var (start, end) = GetTimeRangeForDateAndType(date, items);
      var list = new List<TimeSlot>();

      while (start < end)
      {
        var endTime = start + config.TimeBlockLength;

        list.Add(new TimeSlot()
        {
          End = date.Date.AddHours(endTime.Hours).AddMinutes(endTime.Minutes),
          Start = date.Date.AddHours(start.Hours).AddMinutes(start.Minutes)
        });

        start = endTime;
      }

      return list;
    }


    private static bool TimeIsAvailable(TimeSlot timeSlot, IEnumerable<IAvailabilityItem> availabilityItems)
    {
      return availabilityItems.Any(x =>
        timeSlot.Start.TimeOfDay >= x.StartTime && timeSlot.End.TimeOfDay <= x.EndTime);
    }

    public List<DateTime> GetAvailableDates(DateTime startDate, DateTime endDate, string identity, TimeSpan duration)
    {
      var dates = new List<DateTime>();

      while (startDate <= endDate)
      {
        var slots = GetTimeSlots(startDate, identity, duration);
        if (slots.Any())
        {
          dates.Add(startDate);
        }
        startDate = startDate.AddDays(1);
      }

      return dates;
    }

    public TAppointment SaveAppointment(TAppointment appointment)
    {
      TAppointment savedAppointment = null;

      var timeSlots = GetTimeSlots(appointment.StartTime, appointment.AppointmentType.Identity, appointment.Duration);

      if (timeSlots.Any(x => x.Start == appointment.StartTime && x.End == appointment.StartTime + appointment.Duration)) savedAppointment = repo.SaveAppointment(appointment);

      return savedAppointment;
    }
  }
}
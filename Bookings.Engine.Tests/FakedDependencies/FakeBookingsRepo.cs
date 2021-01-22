using System;
using System.Collections.Generic;
using System.Linq;
using Bookings.Engine.Abstractions.Core;

namespace Bookings.Engine.Tests.FakedDependencies
{
    public class FakeBookingsRepo: IBookingRepository<Appointment, AppointmentType, AvailabilityItem>
    {
        public FakeBookingsRepo()
        {
            var tomorrow = DateTime.Today.AddDays(1);

            var apptType = new AppointmentType()
            {
                Availability = new List<AvailabilityItem>()
                {
                    new AvailabilityItem()
                    {
                        AvailableDays = new List<DayOfWeek>()
                        {
                            tomorrow.DayOfWeek,
                            tomorrow.AddDays(1).DayOfWeek
                        },
                        EndTime = new TimeSpan(17, 0, 0),
                        StartTime = new TimeSpan(13, 0, 0),
                        PriceOverride = 0.00M,
                        SimultaneousLimit = 1
                    },
                    new AvailabilityItem()
                    {
                        AvailableDays = new List<DayOfWeek>()
                        {
                            tomorrow.DayOfWeek,
                            tomorrow.AddDays(1).DayOfWeek
                            
                        },
                        EndTime = new TimeSpan(12, 0, 0),
                        PriceOverride = 0.00M,
                        SimultaneousLimit = 2,
                        StartTime = new TimeSpan(09, 0, 0)
                    }
                },
                Duration = new TimeSpan(0, 30, 0),
                Identity = "kjlskdjf",
                Name = "My Type",
                Price = 30.00M
            };
            
            appointments = new List<Appointment>()
            {
                new Appointment()
                {
                    Duration = TimeSpan.FromMinutes(30),
                    CustomerIdentity = "939090139",
                    PaymentIdentity = "lkjlkja",
                    StartTime = tomorrow.AddHours(13).AddMinutes(30),
                    AppointmentType = apptType
                },
                new Appointment()
                {
                    Duration = TimeSpan.FromMinutes(30),
                    CustomerIdentity = "Johnny5",
                    PaymentIdentity = "MC332232",
                    StartTime = tomorrow.Add(new TimeSpan(9,30,00)),
                    AppointmentType = apptType
                }
            };
            
            types = new List<AppointmentType>()
            {
                apptType
            };
        }
        
        
        
        private readonly List<Appointment> appointments;
        
        private readonly List<AppointmentType> types;
        
        public Appointment SaveAppointment(Appointment appointment)
        {
            appointments.Add(appointment);
            return appointment;
        }

        public bool CancelAppointment(Appointment appointment)
        {
            var appt = appointments.SingleOrDefault(x =>
                x.CustomerIdentity == appointment.CustomerIdentity &&
                x.PaymentIdentity == appointment.PaymentIdentity &&
                x.StartTime == appointment.StartTime);
            appointments.Remove(appt);

            return true;
        }

        public IEnumerable<Appointment> GetAppointmentsByDate(DateTime date, AppointmentType appointmentType)
        {
            return appointments.Where(x =>
                x.AppointmentType.Identity == appointmentType.Identity &&
                    x.StartTime.Date == date.Date
            ).ToList();
        }

        public AppointmentType GetAppointmentTypeByStringIdentity(string identity)
        {
            return types.SingleOrDefault(x => x.Identity == identity);
        }
    }
}
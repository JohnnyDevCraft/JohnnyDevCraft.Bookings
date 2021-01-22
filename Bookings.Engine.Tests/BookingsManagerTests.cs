using System;
using System.Linq;
using Bookings.Engine.Tests.FakedDependencies;
using NUnit.Framework;

namespace Bookings.Engine.Tests
{
    public class BookingsManagerTests
    {
        private BookingConfiguration config;
        private BookingsManager<Appointment, AppointmentType, AvailabilityItem> mgr;
        private FakeBookingsRepo repo;


        [SetUp]
        public void Setup()
        {
            config = new BookingConfiguration {TimeBlockLength = TimeSpan.FromMinutes(30)};
            repo = new FakeBookingsRepo();
            mgr = new BookingsManager<Appointment, AppointmentType, AvailabilityItem>(repo, config);
        }

        [Test]
        public void GetTimeSlotsForTomorrow_ShouldReturnThirteenSlots_WhenRanUntouched()
        {
            //Test
            var timeSlots = mgr.GetTimeSlots(DateTime.Today.AddDays(1), "kjlskdjf").ToList();
            
            Assert.NotNull(timeSlots);
            Assert.AreEqual(13,timeSlots.Count);
        }

        [Test]
        public void GetTimeSlotsForNextDay_ShouldReturnFourteenSots()
        {
            var timeSlots = mgr.GetTimeSlots(DateTime.Today.AddDays(2), "kjlskdjf").ToList();
            
            Assert.NotNull(timeSlots);
            Assert.AreEqual(14, timeSlots.Count);
        }

        [Test]
        public void GetDates_shouldReturnTwoDates()
        {
            var dates = mgr.GetAvailableDates(DateTime.Today, DateTime.Today.AddDays(5), "kjlskdjf").ToList();
            
            Assert.NotNull(dates);
            Assert.AreEqual(2, dates.Count);
        }

        [Test]
        public void SaveAppointment_ShouldSucceed()
        {
            var appointment = new Appointment
            {
                Duration = TimeSpan.FromMinutes(30),
                AppointmentType = repo.GetAppointmentTypeByStringIdentity("kjlskdjf"),
                CustomerIdentity = "jd49490",
                StartTime = DateTime.Today.AddDays(1) + new TimeSpan(13, 00, 00)
            };

            var saved = mgr.SaveAppointment(appointment);
            
            Assert.NotNull(saved);
        }
        
        [Test]
        public void SaveAppointment_ShouldFail()
        {
            var appointment = new Appointment
            {
                Duration = TimeSpan.FromMinutes(30),
                AppointmentType = repo.GetAppointmentTypeByStringIdentity("kjlskdjf"),
                CustomerIdentity = "jd49490",
                StartTime = DateTime.Today.AddDays(1) + new TimeSpan(13, 30, 00)
            };

            var saved = mgr.SaveAppointment(appointment);
            
            Assert.IsNull(saved);
        }

        [Test]
        public void SaveAppointment_ShouldFail_WhenAppointmentsMaxed()
        {
            var appointment1 = new Appointment
            {
                Duration = TimeSpan.FromMinutes(30),
                AppointmentType = repo.GetAppointmentTypeByStringIdentity("kjlskdjf"),
                CustomerIdentity = "jd49490",
                StartTime = DateTime.Today.AddDays(1) + new TimeSpan(9, 30, 00)
            };
            var appointment2 = new Appointment
            {
                Duration = TimeSpan.FromMinutes(30),
                AppointmentType = repo.GetAppointmentTypeByStringIdentity("kjlskdjf"),
                CustomerIdentity = "jimbean",
                StartTime = DateTime.Today.AddDays(1) + new TimeSpan(9, 30, 00)
            };

            var save1 = mgr.SaveAppointment(appointment1);
            var save2 = mgr.SaveAppointment(appointment2);
            
            Assert.NotNull(save1);
            Assert.IsNull(save2);
        }
    }
}
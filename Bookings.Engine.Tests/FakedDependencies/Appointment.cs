using System;
using Bookings.Engine.Abstractions.Core;

namespace Bookings.Engine.Tests.FakedDependencies
{
    public class Appointment: IAppointment<AppointmentType, AvailabilityItem>
    {
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string CustomerIdentity { get; set; }
        public string PaymentIdentity { get; set; }
        public AppointmentType AppointmentType { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Bookings.Engine.Abstractions.Core;

namespace Bookings.Engine.Tests.FakedDependencies
{
    public class AppointmentType: IAppointmentType<AvailabilityItem>
    {
        public string Identity { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public List<AvailabilityItem> Availability { get; set; }
    }
}
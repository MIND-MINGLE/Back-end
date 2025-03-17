using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Appointment
{
    public class AppointmentUpdateRequest
    {
        public string? CoWorkingSpaceId { get; set; }
        public string? SessionId { get; set; }
        public string? EmergencyEndId { get; set; }
        public AppointmentType? AppointmentType { get; set; }
        public Status? Status { get; set; }
        public double? TotalFee { get; set; }
        public double? PlatformFee { get; set; }
    }
}

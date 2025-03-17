using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Appointment
{
    internal class AppointmentResponse
    {
        public string AppointmentId { get; set; } = null!;
        public string PatientId { get; set; } = null!;
        public string TherapistId { get; set; } = null!;
        public string? CoWorkingSpaceId { get; set; }
        public string SessionId { get; set; } = null!;
        public string EmergencyEndId { get; set; } = null!;
        public AppointmentType AppointmentType { get; set; }
        public Status Status { get; set; }
        public double TotalFee { get; set; }
        public double PlatformFee { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

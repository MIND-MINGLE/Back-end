using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Payment
{
    public class PaymentRequestAppointment
    {
        public string PatientId { get; set; } = null!;
        public string AppointmentId { get; set; } = null!;
        public double Amount { get; set; }
        public double? TherapistReceive { get; set; }
        public string? PaymentUrl { get; set; }
        public PaymentMethod PaymentMethod { get; set; } // Thêm PaymentMethod
        public PaymentStatus PaymentStatus { get; set; }
    }
}

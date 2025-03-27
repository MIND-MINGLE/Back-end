using Application.Response.Appointment;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Payment
{
    public class PaymentResponse
    {
        public string PaymentId { get; set; } = null!;
        public string PatientId { get; set; } = null!;
        public double Amount { get; set; }
        public AppointmentResponse? Appointment { get; set; }
        public double TherapistReceive { get; set; }
        public string? PaymentUrl { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ResponsePatient Patient { get; set; }


        public DateTime CreatedAt { get; set; } // Thêm trường này
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Rating
{
    public class ResponseRating
    {
        public required string RatingId { get; set; }
        public required string PatientId { get; set; }
        required public string TherapistId { get; set; }
        public required string AppointmentId { get; set; }
        public required string Comment { get; set; }
        public double Score { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

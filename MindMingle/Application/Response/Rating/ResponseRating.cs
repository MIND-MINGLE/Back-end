using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Rating
{
    public class ResponseRating
    {
        public string PatientId { get; set; }
        public string AppointmentId { get; set; }
        public string Comment { get; set; }
        public double Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

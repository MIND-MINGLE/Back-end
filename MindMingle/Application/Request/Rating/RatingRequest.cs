using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Rating
{
    public class RatingRequest
    {
        required public string PatientId { get; set; }
        
        required public string AppointmentId { get; set; }
        required public string Comment { get; set; }
        required public double Score { get; set; }
    }
}

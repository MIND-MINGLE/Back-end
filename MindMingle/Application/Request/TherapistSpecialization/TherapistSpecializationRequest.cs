using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Therapist_Specialization
{
    public class TherapistSpecializationRequest
    {
        required public string TherapistId { get; set; }
        required public string SpecializationId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.PatientSurvey
{
    public class PatientSurveyRequest
    {
        public required string PatientId { get; set; }
        public required string Summary { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Mặc định là thời gian hiện tại
    }
}

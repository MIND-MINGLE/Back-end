using Application.Response.PatientResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.PatientSurvey
{
    public class PatientSurveyResponse
    {
        public string PatientSurveyId { get; set; } = null!;
        public string PatientId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Summary { get; set; } = null!;
        public List<PatientResResponse>? PatientResponses { get; set; } // Bao gồm danh sách câu trả lời nếu cần
    }
}

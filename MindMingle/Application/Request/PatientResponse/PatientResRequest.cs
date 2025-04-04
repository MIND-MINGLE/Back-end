using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.PatientResponse
{
    public class PatientResRequest
    {
       required public string PatientSurveyId { get; set; }
        required public string QuestionId { get; set; }
        required public string AnswerId { get; set; } 
        public string? CustomAnswer { get; set; }
    }
}

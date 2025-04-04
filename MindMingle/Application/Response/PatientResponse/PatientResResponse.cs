using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.PatientResponse
{
    public class PatientResResponse
    {
        public string ResponseId { get; set; }
        public string SurveyId { get; set; }
        public string QuestionId { get; set; }
        public string? AnswerId { get; set; }
        public string? CustomAnswer { get; set; }
        public int? Score { get; set; }
    }
}

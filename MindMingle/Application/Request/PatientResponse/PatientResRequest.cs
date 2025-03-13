using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.PatientResponse
{
    public class PatientResRequest
    {
        public string SurveyId { get; set; }
        public string QuestionId { get; set; }
        public string? AnswerId { get; set; } // Nullable vì có thể không chọn câu trả lời
        public string? CustomerAnswer { get; set; } // Nullable vì có thể không có câu trả lời tùy chỉnh
    }
}

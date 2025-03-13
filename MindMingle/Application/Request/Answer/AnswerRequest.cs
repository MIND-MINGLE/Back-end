using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Answer
{
    public class AnswerRequest
    {
        required public string QuestionId { get; set; }
        required public string Content { get; set; }
    }
}

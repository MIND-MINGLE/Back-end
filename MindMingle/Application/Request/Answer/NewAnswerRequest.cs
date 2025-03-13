using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Answer
{
    public class NewAnswerRequest
    {
        required public string QuestionId { get; set; }
        required public string AnswerContent { get; set; }
        required public int Score { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Question
{
    public class QuestionRequest
    {
        required public string QuestionContent { get; set; }
        required public string CategoryId { get; set; }
    }
}

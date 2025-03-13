using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ResponseAnswer
    {
        required public string AnswerId { get; set; }
        required public string AnswerContent { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ResponseQuestion
    {
        required public string QuestionId { get; set; }
        required public string QuestionContent { get; set; }
        required public string QuestionCategory { get; set; }
        required public string QuestionType { get; set; }
        required public DateTime CreatedAt { get; set; }
    }
}

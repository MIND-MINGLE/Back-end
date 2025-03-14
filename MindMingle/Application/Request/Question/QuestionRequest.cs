using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Request.Question
{
    public class QuestionRequest
    {
        required public string QuestionContent { get; set; }
        required public string CategoryId { get; set; }
        required public QType QuestionType { get; set; }
    }
}

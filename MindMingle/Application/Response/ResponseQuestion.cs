using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Response
{
    public class ResponseQuestion
    {
        public required string QuestionId { get; set; }
        public required string QuestionContent { get; set; }
        public required string CategoryId { get; set; }
        required public QType QuestionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ResponseAnswer> Answers { get; set; } = null!; // Add this property to fix CS0117
    }
}

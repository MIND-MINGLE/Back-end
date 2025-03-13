using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ResponseQuestion
    {
        public required string QuestionId { get; set; }
        public required string QuestionContent { get; set; }
        public required string CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ResponseAnswer> Answers { get; set; } // Add this property to fix CS0117
    }
}

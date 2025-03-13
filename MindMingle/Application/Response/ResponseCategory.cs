using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ResponseCategory
    {
        public required string CategoryId { get; set; }
        public required QuestionType CategoryName { get; set; }
        public required string Description { get; set; }
        public required List<ResponseQuestion> Questions { get; set; }
    }
}

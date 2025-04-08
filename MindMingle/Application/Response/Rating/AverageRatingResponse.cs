using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Rating
{
    public class AverageRatingResponse
    {
        public string TherapistId { get; set; } = null!;
        public double AverageStar { get; set; }
        public int TotalRatings { get; set; }
    }
}

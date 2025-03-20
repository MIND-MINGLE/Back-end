using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Subcription
{
    public class ResponseSubscription
    {
        public string SubscriptionId { get; set; }
        public string PackageName { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDisabled { get; set; }

    }
}

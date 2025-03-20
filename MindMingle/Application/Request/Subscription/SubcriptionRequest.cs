using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Subcription
{
    public class SubscriptionRequest
    {
        required public string packageName { get; set; }
        required public double price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.PurchasedPackage
{
    public class PurchasedPackageRequest
    {
        required public string PatientId { get; set; }
        required public string SubscriptionId { get; set; }
        required public DateTime EndDate { get; set; }
        required public DateTime StartDate { get; set; }
    }
}

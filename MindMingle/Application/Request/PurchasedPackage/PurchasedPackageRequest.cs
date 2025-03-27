using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Request.PurchasedPackage
{
    public class PurchasedPackageRequest:Norms
    {
        required public string PatientId { get; set; }
        required public string SubscriptionId { get; set; }
    }
}

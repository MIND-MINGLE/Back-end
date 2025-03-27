using Application.Response.Subcription;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.PurchasedPackage
{
    public class ResponsePurchasedPackage:Norms
    {
        public string PurchasedPackageId { get; set; }
        public string PatientId { get; set; }
        public string SubscriptionId { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public ResponseSubscription Subscription { get; set; } // Add this property
    }
}

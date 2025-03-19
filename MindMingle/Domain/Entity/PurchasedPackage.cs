using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class PurchasedPackage
	{
        [Key]
        public required string PurchasedPackageId { get; set; }
        public required string PatientId { get; set; }
        public required string SubscriptionId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }

        public required Subcription Subcription { get; set; }
        public required Patient Patient { get; set; }
    }
}


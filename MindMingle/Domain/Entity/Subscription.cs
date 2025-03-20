using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entity
{
	public class Subscription:Norms
	{
        [Key]
        public required string SubscriptionId { get; set; }
        public required string PackageName { get; set; }
        public required double Price { get; set; }

        public ICollection<PurchasedPackage>? PurchasedPackages { get; set; }
    }
}


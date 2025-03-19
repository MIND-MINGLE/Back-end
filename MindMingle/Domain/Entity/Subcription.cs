using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entity
{
	public class Subcription:Norms
	{
        [Key]
        public required string SubcriptionId { get; set; }
        public required string PackageName { get; set; }
        public required double Price { get; set; }

        public ICollection<PurchasedPackage>? PurchasedPackages { get; set; }
    }
}


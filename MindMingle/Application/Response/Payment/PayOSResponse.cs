using System;
namespace Application.Response.Payment
{
	public class PayOSResponse
	{
		public required bool PaymentStatus { get; set; }
        public required string PaymentId { get; set; }
    }
}


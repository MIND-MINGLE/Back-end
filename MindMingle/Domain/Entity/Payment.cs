using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Entity
{
	public class Payment:Norms
	{
		[Key]
		public required string PaymentId { get; set; }
        public required string PatientId { get; set; }
        public string? AppointmentId { get; set; }
        public required double Amount { get; set; }
        public required double TherapistReceive { get; set; }
        public string? PaymentUrl { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public required PaymentStatus PaymentStatus { get; set; }

        public required Patient Patient { get; set; }
        public required Appointment Appointment { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentMethod
    {
        [EnumMember(Value = "PayOS")]
        MOMO,
        //[EnumMember(Value = "VNPay")]
        //VNPAY,
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentStatus
    {
        [EnumMember(Value = "Pending")]
        PENDING,
        [EnumMember(Value = "Paid")]
        PAID,
        [EnumMember(Value = "Canceled")]
        CANCELED,
    }
}


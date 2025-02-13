using System;
namespace Domain.Entity
{
    public class EmailVerification
    {
        required public string VerificationId { get; set; }
        public string VerificationCode { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }

        //navigation property
        public string AccountId { get; set; } = null!;
        public Account? Account { get; set; }
    }
}


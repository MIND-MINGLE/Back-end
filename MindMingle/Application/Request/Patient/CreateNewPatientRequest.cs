using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Patient
{
	public class CreateNewPatientRequest
	{
		public required string AccountId { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required DateTime Dob { get; set; }
		public required string Gender { get; set; }
		public required string PhoneNumber { get; set; }
	}
}

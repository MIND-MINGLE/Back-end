using System;
namespace Application.Response
{
	public class ResponsePatient
	{
			public required string PatientId { get; set; }
			public required string AccountId { get; set; }
			public required string FirstName { get; set; }
			public required string LastName { get; set; }
			public required DateOnly Dob { get; set; }
			public required string Gender { get; set; }
			public required string PhoneNumber { get; set; }
	}
}


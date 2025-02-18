using Application.Request.Patient;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
	public interface IPatientService
	{
		Task<ApiResponse> AddNewPatient(CreateNewPatientRequest newPatient);
		Task<ApiResponse> GetPatientByAccountIdAsync(string accountId);

	}
}

using Application.Interface;
using Application.Request.Patient;
using Application.Response;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class PatientService : IPatientService
	{
		private IUnitOfWorks _unitOfWorks;
		private IMapper _mapper;

		public PatientService(IUnitOfWorks unitOfWorks, IMapper mapper)
		{
			_unitOfWorks = unitOfWorks;
			_mapper = mapper;
		}

		public async Task<ApiResponse> AddNewPatient(CreateNewPatientRequest newPatient)
		{
			ApiResponse response = new ApiResponse();

			//Check if account was create
			var patientAccount = _unitOfWorks.AccountRepo.GetAsync(x => x.AccountId == newPatient.AccountId);
			if(patientAccount == null )
			{
				response.SetBadRequest(message: "Account not found or created !");
				return response;
			}

			//Create new patient
			var patient = _mapper.Map<Patient>(patientAccount);

			await _unitOfWorks.PatientRepo.AddAsync(patient);
			await _unitOfWorks.SaveChangeAsync();

			response.SetOk(patient);
			return response;
		}

		public async Task<ApiResponse> GetPatientByAccountIdAsync(string accountId)
		{
			ApiResponse response = new ApiResponse();

			var patient = await _unitOfWorks.PatientRepo.GetAsync(x => x.PatientId == accountId);
			if (patient == null)
			{
				response.SetBadRequest("Patient profile not found.");
				return response;
			}

			response.SetOk(patient);
			return response;
		}
	}
}

using Application.Interface;
using Application.Request.Account;
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
			var patientAccount = await _unitOfWorks.AccountRepo.GetAsync(x => x.AccountId == newPatient.AccountId);
			if (patientAccount == null)
			{
				response.SetBadRequest(message: "Account not found nor created!");
				return response;
			}

			//Create new patient
			var patient = _mapper.Map<Patient>(newPatient);
			await _unitOfWorks.PatientRepo.AddAsync(patient);
			await _unitOfWorks.SaveChangeAsync();
			response.SetOk(newPatient);
			//Console.WriteLine("Fixing Bug");
			return response;
		}

		public async Task<ApiResponse> GetAllPatientsAsync()
		{
			ApiResponse response = new ApiResponse();
			try
			{
				var patientsModel = await _unitOfWorks.PatientRepo.GetAllAsync(null);
			
				if (patientsModel.Count == 0)
				{
					return response.SetNotFound("No patient profile found.");
				}
				List<ResponsePatient> listPatientRes = new List<ResponsePatient>();
				foreach(Patient patient in patientsModel) {
					var formattedDob = patient.Dob.Date.ToString("dd/MM/yyyy");
					var resPatient = _mapper.Map<ResponsePatient>(patient);
					resPatient.Dob = formattedDob;
					listPatientRes.Add(resPatient);
                };
                return response.SetOk(listPatientRes);
			}
			catch (Exception ex)
			{
				return response.SetBadRequest(ex);
			}
		}

		public async Task<ApiResponse> GetPatientByAccountIdAsync(string accountId)
		{
			ApiResponse response = new ApiResponse();

			var patient = await _unitOfWorks.PatientRepo.GetAsync(x => x.AccountId == accountId);
			if (patient == null)
			{
				response.SetBadRequest("Patient profile not found.");
				return response;
			}
			var formattedDob = patient.Dob.Date.ToString("dd/MM/yyyy");
			var responsePatient = _mapper.Map<ResponsePatient>(patient);
			responsePatient.Dob = formattedDob;
			response.SetOk(responsePatient);
			return response;
		}

        public async Task<ApiResponse> UpdatePatientAsync(UpdatePersonRequest updatePatient)
        {
            ApiResponse response = new ApiResponse();
            try
			{
                var patient = await _unitOfWorks.PatientRepo.GetAsync(x => x.PatientId == updatePatient.Id);
                if (patient == null)
                {
                    response.SetBadRequest("Patient profile not found.");
                    return response;
                }

                _mapper.Map(updatePatient, patient);
                await _unitOfWorks.PatientRepo.UpdateFieldsAsync(patient.PatientId, new Dictionary<string, object>
            {
                { nameof(patient.FirstName), patient.FirstName },
                { nameof(patient.LastName), patient.LastName },
                { nameof(patient.Dob), patient.Dob },
                { nameof(patient.Gender), patient.Gender },
                { nameof(patient.PhoneNumber), patient.PhoneNumber }
            });
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk(updatePatient);
            }
			catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }
	}
}
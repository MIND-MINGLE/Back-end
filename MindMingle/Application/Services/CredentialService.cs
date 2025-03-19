using Application.Interface;
using Application.Request.Credential;
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
    public class CredentialService : ICredentialService
    {
        private IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public CredentialService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddNewCredentials(CredentialRequest newCredentials)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                //Check if account was create
                var account = await _unitOfWorks.TherapistRepo.GetAsync(x => x.TherapistId == newCredentials.TherapistId);
                if (account == null)
                {
                    response.SetBadRequest(message: "Account not found nor created!");
                    return response;
                }
                //Create new credential
                var credential = _mapper.Map<Credentials>(newCredentials);
                await _unitOfWorks.CredentialRepo.AddAsync(credential);
                await _unitOfWorks.SaveChangeAsync();
                response.SetOk(newCredentials);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> DisableCredentials(string credentailId)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var credential = await _unitOfWorks.CredentialRepo.GetAsync(x => x.CredentialsId == credentailId);
                if (credential == null)
                {
                    return response.SetNotFound(message: "Credentials not found!");
                }
                credential.IsDisabled = true;
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk("Credentials disabled successfully!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetCredentialsByTherapistId(string therapistId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var credentials = _unitOfWorks.CredentialRepo.GetAsync(x => x.TherapistId == therapistId);
                var credentialsList = _mapper.Map<List<RepsonseCredential>>(credentials);
                if (credentialsList.Count == 0)
                {
                    return response.SetNotFound(message: "Credentials not found!");
                }
                return response.SetOk(credentialsList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }

        public async Task<ApiResponse> UpdateCredentails(string credentialId, UpdateCredentialRequest updateCredentialRequest)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var credential = await _unitOfWorks.CredentialRepo.GetAsync(x => x.CredentialsId == credentialId);
                if (credential == null)
                {
                    return response.SetNotFound(message: "Credentials not found!");
                }
                await _unitOfWorks.CredentialRepo.UpdateFieldAsync(credentialId, x => x.ImageURL, updateCredentialRequest.imageUrl);
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk("Credentials updated successfully!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

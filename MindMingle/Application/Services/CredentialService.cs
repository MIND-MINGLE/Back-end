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
    }
}

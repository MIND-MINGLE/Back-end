using Application.Request.Credential;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICredentialService
    {
        Task<ApiResponse> AddNewCredentials(CredentialRequest newCredentials);
    }
}

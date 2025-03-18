using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Credential
{
    public class CredentialRequest
    {
        required public string TherapistId { get; set; }
        required public string imageUrl { get; set; }
    }
}

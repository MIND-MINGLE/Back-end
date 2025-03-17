using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Account
{
    public class AvatarRequest
    {
        required public string AccountId { get; set; }
        required public string? NewAvatar { get; set; }
    }
}

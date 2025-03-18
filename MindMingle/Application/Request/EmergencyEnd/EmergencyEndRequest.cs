using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.EmergencyEnd
{
    public class EmergencyEndRequest
    {
        required public string AppointmentId { get; set; }
        required public string AccountId { get; set; }
        required public string Reason { get; set; }
    }
}

using System;
namespace Application.Response
{
	public class ResponseEmergencyEnd
	{
        required public string EmergencyEndId { get; set; }
        required public string AppointmentId { get; set; }
        required public string AccountId { get; set; }
        required public string Reason { get; set; }
    }
}


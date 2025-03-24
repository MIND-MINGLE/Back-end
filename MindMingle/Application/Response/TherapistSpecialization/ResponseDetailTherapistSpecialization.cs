using Application.Response.Specialization;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.TherapistSpecialization
{
    public class ResponseDetailTherapistSpecialization
    {
        public string TherapistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public double PricePerHour { get; set; }
        public List<ResponseSpecialization> Specializations { get; set; }

    }
}

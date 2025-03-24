using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Specialization
{
    public class SpecializationRequest
    {
        required public string Name { get; set; }
        required public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ResponseCategory
    {
        required public string CategoryId { get; set; }
        required public string CategoryName { get; set; }
        required public string Description { get; set; }
    }
}

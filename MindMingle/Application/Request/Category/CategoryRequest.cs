using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Category
{
    public class CategoryRequest
    {
        required public string CategoryName { get; set; }
        required public string Description { get; set; }
    }
}

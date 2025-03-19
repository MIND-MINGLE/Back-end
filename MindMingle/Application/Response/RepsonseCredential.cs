using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class RepsonseCredential
    {
        public string credentialId { get; set; }
        public string imageUrl { get; set; }
        public string therapistId { get; set; }
        public int isDisabled { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

    }
}

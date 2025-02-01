using RannaTask.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.SupportForms
{
    public class CreateSupportFormDto
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public SupportFormStatus Status { get; set; }
        public int CustomerId { get; set; }
    }
}

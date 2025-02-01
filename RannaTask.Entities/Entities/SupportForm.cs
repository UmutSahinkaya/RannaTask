using RannaTask.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Entities.Entities
{
    public class SupportForm : BaseEntity<int>
    {
        public SupportForm()
        {
            
        }

        public string Subject { get; set; }
        public string Message { get; set; }
        public SupportFormStatus Status { get; set; }

        public int CustomerId { get; set; }
    }
}

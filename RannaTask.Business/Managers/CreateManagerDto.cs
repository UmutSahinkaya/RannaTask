using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Managers
{
    public class CreateManagerDto
    {
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Managers
{
    public class CreateManagerResponse
    {
        public CreateManagerResponse()
        {
            
        }

        public CreateManagerResponse(int id, string message)
        {
            Id = id;
            Message = message;
        }

        public CreateManagerResponse(string message)
        {
            Message = message;
        }

        public int Id { get; set; }
        public string Message { get; set; }
    }
}

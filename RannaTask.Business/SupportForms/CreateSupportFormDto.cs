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
        public CreateSupportFormDto(string subject, string message, int userId)
        {
            Subject = subject;
            Message = message;
            UserId = userId;
        }

        public string Subject { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }
}

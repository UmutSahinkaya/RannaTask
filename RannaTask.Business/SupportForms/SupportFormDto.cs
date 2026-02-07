using RannaTask.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.SupportForms
{
    public class SupportFormDto
    {
        public SupportFormDto()
        {

        }

        public SupportFormDto(int id, string subject, string message, SupportFormStatus status, int userId)
        {
            Id = id;
            Subject = subject;
            Message = message;
            Status = status;
            UserId = userId;
        }

        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public SupportFormStatus Status { get; set; }
        public int UserId { get; set; }
        public string CloseReason { get; set; }  // Admin'in kapama sebebi
    }
}

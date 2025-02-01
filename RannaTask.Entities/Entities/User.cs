using RannaTask.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Entities.Entities
{
    public class User : BaseEntity<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? ManagerId { get; set; }
        public Manager? Manager { get; set; }

        [ForeignKey("UserRole")]
        public int RoleId { get; set; }
        public UserRole Role { get; set; }

    }
}

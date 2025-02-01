using RannaTask.Entities.Common;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Users
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}

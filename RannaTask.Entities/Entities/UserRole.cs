using RannaTask.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Entities.Entities
{
    public class UserRole:BaseEntity<int>
    {
        public string Name { get; set; }
        public byte Type { get; set; } // 1 => manager, 2 => Customer
    }
}

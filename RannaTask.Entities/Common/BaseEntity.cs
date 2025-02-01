using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Entities.Common
{
    public class BaseEntity<T>
    {
        public T Id { get; set; } = default!;
        public DateTime Created { get; set; }=DateTime.Now;
    }
}

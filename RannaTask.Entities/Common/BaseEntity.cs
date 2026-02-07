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
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false; // Soft delete flag
        public DateTime? DeletedAt { get; set; } // When was it deleted
    }
}

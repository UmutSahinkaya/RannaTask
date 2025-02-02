using RannaTask.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Entities.Entities
{
    public class Product : BaseEntity<int>
    {
        public Product()
        {
            
        }
        public Product(int id,string name, string code, decimal price, string? image)
        {
            this.Id= id;
            Name = name;
            Code = code;
            Price = price;
            Image = image;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }

    }
}

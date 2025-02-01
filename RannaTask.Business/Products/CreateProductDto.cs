using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Products
{
    public  class CreateProductDto
    {
        public CreateProductDto(string name, string code, decimal price, string? ımage)
        {
            Name = name;
            Code = code;
            Price = price;
            Image = ımage;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}

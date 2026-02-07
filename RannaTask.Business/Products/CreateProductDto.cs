using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Products
{
    public  class CreateProductDto
    {
        public CreateProductDto()
        {

        }
        public CreateProductDto(string name, string code, decimal price, string? image)
        {
            Name = name;
            Code = code;
            Price = price;
            Image = image;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public int? CreatedBy { get; set; }
    }
}

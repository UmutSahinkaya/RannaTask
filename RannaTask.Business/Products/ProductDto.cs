using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Products
{
    public class ProductDto
    {
        public ProductDto(int id, string name, decimal price,string image)
        {
            Id = id;
            Name = name;
            Price = price;
            Image = image;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}

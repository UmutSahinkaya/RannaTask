using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RannaTask.DAL.Repositories.Products;
using RannaTask.DAL.UnitOfWorks;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Products
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductManager(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateProductResponse> CreateAsync(CreateProductDto request)
        {
            var anyProduct = await _productRepository.Where(p => p.Name == request.Name).AnyAsync();
            if (anyProduct)
            {
                throw new Exception("Veri tabanında bu isimde ürün vardır.");
            }

            //var addProduct = new Product()
            //{
            //    Name = request.Name,
            //    Code = request.Code,
            //    Price = request.Price,
            //    Image = request.Image
            //};

            var product = _mapper.Map<Product>(request);

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return new CreateProductResponse { Id = product.Id };
        }

        public async Task<NoContent> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return new NoContent { Message = "Ürün Bulunamadı." };
            _productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent();
        }

        public async Task<List<ProductDto>> GetAllListAsync()
        {
            var products = await _productRepository.GetAll().ToListAsync();

            var productsAsDto = _mapper.Map<List<ProductDto>>(products);

            return productsAsDto;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                throw new Exception("Product not found");
            }

            var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Image);

            return productAsDto;
        }

        public async Task<NoContent> UpdateAsync(int id, ProductDto request)
        {
            var isProductNameExist = await _productRepository.Where(p => p.Name == request.Name && p.Id != id).AnyAsync();
            if (!isProductNameExist)
            {
                throw new Exception("Ürün ismi zaten bulunmakta!");
            }

            //Product product = new()
            //{
            //    Id = id,
            //    Name = request.Name,
            //    Code = request.Code,
            //    Price = request.Price,
            //    Image = request.Image
            //};
            var product = _mapper.Map<Product>(request);
            product.Id = id;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent();
        }
    }
}

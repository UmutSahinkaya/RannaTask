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

            var product = _mapper.Map<Product>(request);
            // CreatedBy request'ten gelir (API'de token'dan set edilir)

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return new CreateProductResponse { Id = product.Id };
        }

        public async Task<NoContent> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return new NoContent { Message = "Ürün Bulunamadı." };

            // Soft delete
            _productRepository.SoftDelete(product);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Ürün başarıyla silindi.");
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
                return null;

            var productAsDto = new ProductDto(product.Id, product.Code, product.Name, product.Price, product.Image)
            {
                CreatedBy = product.CreatedBy
            };

            return productAsDto;
        }

        public async Task<NoContent> UpdateAsync(int id, ProductDto request)
        {
            // Önce mevcut ürünü al
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct is null)
                return new NoContent("Ürün bulunamadı!");

            // İsim kontrolü (sadece farklı bir üründe aynı isim varsa)
            var isProductNameExist = await _productRepository.Where(p => p.Name == request.Name && p.Id != id).AnyAsync();
            if (isProductNameExist)
                return new NoContent("Ürün ismi zaten bulunmakta!");

            // Sadece değişen alanları güncelle (CreatedBy'ı KORUMAK ÖNEMLİ!)
            existingProduct.Name = request.Name;
            existingProduct.Code = request.Code;
            existingProduct.Price = request.Price;
            existingProduct.Image = request.Image;
            // CreatedBy değiştirilmez, mevcut değeri korunur!

            _productRepository.Update(existingProduct);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent($"{existingProduct.Id} Id'li Ürün Güncellendi");
        }
    }
}

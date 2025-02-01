using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RannaTask.DAL.Repositories.Customers;
using RannaTask.DAL.Repositories.Products;
using RannaTask.DAL.UnitOfWorks;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Customers
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerManager(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateCustomerResponse> CreateAsync(CreateCustomerDto request)
        {
            var anyCustomer = await _customerRepository.Where(c => c.Email == request.Email).AnyAsync();
            if (anyCustomer)
                return new CreateCustomerResponse("Bu Email'e kayıtlı başka bir kullanıcı mevcut");
            var customer = _mapper.Map<Customer>(request);
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            return new CreateCustomerResponse(customer.Id);
        }

        public async Task<NoContent> DeleteAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null)
                return new NoContent("Böyle bir müşteri bulunmamakta!");
            _customerRepository.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Müşteri silindi.");
        }

        public async Task<List<CustomerDto>> GetAllListAsync()
        {
            var customers = await _customerRepository.GetAll().ToListAsync();
            var customersAsDto = _mapper.Map<List<CustomerDto>>(customers);
            return customersAsDto;
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null)
                return null;
            var customerAsDto= _mapper.Map<CustomerDto>(customer);
            return customerAsDto;
        }

        public async Task<NoContent> UpdateAsync(int id, CustomerDto request)
        {
            var existCustomer = await _customerRepository.GetByIdAsync(id);
            if (existCustomer is null)
                return new NoContent("Böyle bir müşteri bulunmamakta");
            var customer = _mapper.Map<Customer>(request);
            customer.Id= id;

            _customerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Güncelleme başarılı.");
        }
    }
}

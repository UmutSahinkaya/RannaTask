using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RannaTask.Business.Customers;
using RannaTask.Business.Helpers;
using RannaTask.DAL.Repositories.Customers;
using RannaTask.DAL.Repositories.Managers;
using RannaTask.DAL.Repositories.Users;
using RannaTask.DAL.UnitOfWorks;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Managers
{
    public class ManagerManager : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManagerManager(IManagerRepository managerRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
        {
            _managerRepository = managerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<CreateManagerResponse> CreateAsync(CreateManagerDto request)
        {
            var anyManager=await _managerRepository.Where(c => c.Email == request.Email).AnyAsync();
            if(anyManager)
                return new CreateManagerResponse("Bu Email'e kayıtlı başka bir kullanıcı mevcut");
            var manager = _mapper.Map<Manager>(request);
            await _managerRepository.AddAsync(manager);
            var newUser = new User
            {
                Username = manager.FirstName + manager.LastName,
                //Password = request.Password,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                ManagerId = manager.Id,
                RoleId = 1
            };
            await _userRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
            return new CreateManagerResponse(manager.Id,"Yönetici eklendi.");
        }

        public async Task<NoContent> DeleteAsync(int id)
        {
            var manager = await _managerRepository.GetByIdAsync(id);
            if (manager is null)
                return new NoContent("Böyle bir yönetici bulunmamakta!");
            _managerRepository.Delete(manager);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Yönetici silindi.");
        }

        public async Task<List<ManagerDto>> GetAllListAsync()
        {
            var managers = await _managerRepository.GetAll().ToListAsync();
            var managersAsDto = _mapper.Map<List<ManagerDto>>(managers);
            return managersAsDto;
        }

        public async Task<ManagerDto?> GetByIdAsync(int id)
        {
            var manager = await _managerRepository.GetByIdAsync(id);
            if (manager is null)
                return null;
            var managerAsDto = _mapper.Map<ManagerDto>(manager);
            return managerAsDto;
        }

        public async Task<NoContent> UpdateAsync(int id, ManagerDto request)
        {
            var existManager = await _managerRepository.GetByIdAsync(id);
            if (existManager is null)
                return new NoContent("Böyle bir yönetici bulunmamakta");
            var manager = _mapper.Map<Manager>(request);
            manager.Id = id;

            _managerRepository.Update(manager);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Güncelleme başarılı.");
        }
    }
}

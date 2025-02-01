using AutoMapper;
using RannaTask.DAL.Repositories.Users;
using RannaTask.DAL.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Users
{
    public class UserManager:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserManager(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<UserDto> CreateAsync(CreateUserDto request)
        {
            //Hash leme yapacağız
            throw new NotImplementedException();
        }

        public Task<NoContent> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetAllListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<NoContent> UpdateAsync(int id, UserDto request)
        {
            throw new NotImplementedException();
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RannaTask.Business.Helpers;
using RannaTask.DAL.Repositories.Users;
using RannaTask.DAL.UnitOfWorks;
using RannaTask.Entities.Entities;
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

        public async Task<UserDto> CreateAsync(CreateUserDto request)
        {
            var anyUser = await _userRepository.Where(x => x.Username == request.Username).AnyAsync();
            if (anyUser)
                throw new Exception("Kullanıcı Adı zaten alınmış");
            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            User newUser = new();
            if(request.RoleId == 2)
            {
                newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = hashedPassword,
                    RoleId = request.RoleId,
                    Customer=new Customer(request.Username,)
                };
            }
            var 
            await _userRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
            var userDto=_mapper.Map<UserDto>(request);
            return userDto;
        }

        public async Task<NoContent> DeleteByUsernameAsync(string username)
        {
            var user=await _userRepository.Where(x=>x.Username==username).FirstOrDefaultAsync();
            if (user != null)
                return new NoContent("Böyle bir kullanıcı adı bulunmamakta!");
            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Kullanıcı silindi.");
        }

        public async Task<List<UserDto>> GetAllListAsync()
        {
            var userList = await _userRepository.GetAll().ToListAsync();
            var userAsDto=_mapper.Map<List<UserDto>>(userList);
            return userAsDto;
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user=await _userRepository.Where(x=>x.Username==username).FirstOrDefaultAsync();
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<User> GetByUsernameAndPassword(string username, string password)
        {
            var user = await _userRepository.Where(u => u.Username == username).Include(r=>r.Role).FirstOrDefaultAsync();
            if (user is null)
                return null;
            var verifyPassword=PasswordHasher.VerifyPassword(password,user.PasswordHash);
            if (verifyPassword)
                return user;
            else
                return null;
        }

        public async Task<NoContent> UpdateAsync(string username, UserDto request)
        {
            var user = await _userRepository.Where(u => u.Username == username).FirstOrDefaultAsync();
            if (user is null)
                return null;
            var verifyPassword = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!verifyPassword)
                user.PasswordHash = PasswordHasher.HashPassword(request.Password);
            
            var verifyUsername = await _userRepository.Where(x => x.Username == request.Username).AnyAsync();
            if (verifyUsername)
                return new NoContent("Bu kullanıcı adı daha önceden alınmış");
            user.Username = request.Username;

            if (!string.IsNullOrEmpty(request.RoleId.ToString()))
                user.RoleId = request.RoleId;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Kullanıcı güncellendi.");

        }
    }
}

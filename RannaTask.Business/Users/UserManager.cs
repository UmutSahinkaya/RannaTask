using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RannaTask.Business.Helpers;
using RannaTask.DAL.Repositories.Users;
using RannaTask.DAL.UnitOfWorks;
using RannaTask.Entities.Common;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Users
{
    public class UserManager : IUserService
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
            // Check username uniqueness
            var anyUsername = await _userRepository.Where(x => x.Username == request.Username).AnyAsync();
            if (anyUsername)
                throw new Exception("Kullanıcı adı zaten kullanılıyor");

            // Check email uniqueness
            var anyEmail = await _userRepository.Where(x => x.Email == request.Email).AnyAsync();
            if (anyEmail)
                throw new Exception("Bu email adresi zaten kayıtlı");

            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            User newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role,
                IsActive = true
            };

            await _userRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }

        public async Task<NoContent> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return new NoContent("Kullanıcı bulunamadı!");

            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Kullanıcı silindi.");
        }

        public async Task<List<UserDto>> GetAllListAsync()
        {
            var userList = await _userRepository.GetAll().ToListAsync();
            var userAsDto = _mapper.Map<List<UserDto>>(userList);
            return userAsDto;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return null;
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.Where(x => x.Username == username).FirstOrDefaultAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.Where(x => x.Email == email).FirstOrDefaultAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<User> GetByUsernameAndPassword(string username, string password)
        {
            var user = await _userRepository.Where(u => u.Username == username && u.IsActive).FirstOrDefaultAsync();
            if (user is null)
                return null;

            var verifyPassword = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            return verifyPassword ? user : null;
        }

        public async Task<User> GetByEmailAndPassword(string email, string password)
        {
            var user = await _userRepository.Where(u => u.Email == email && u.IsActive).FirstOrDefaultAsync();
            if (user is null)
                return null;

            var verifyPassword = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            return verifyPassword ? user : null;
        }

        public async Task<NoContent> UpdateAsync(int id, UserDto request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return new NoContent("Kullanıcı bulunamadı!");

            // Check username uniqueness (if changed)
            if (user.Username != request.Username)
            {
                var usernameExists = await _userRepository.Where(x => x.Username == request.Username && x.Id != id).AnyAsync();
                if (usernameExists)
                    return new NoContent("Bu kullanıcı adı zaten kullanılıyor");
                user.Username = request.Username;
            }

            // Check email uniqueness (if changed)
            if (user.Email != request.Email)
            {
                var emailExists = await _userRepository.Where(x => x.Email == request.Email && x.Id != id).AnyAsync();
                if (emailExists)
                    return new NoContent("Bu email adresi zaten kayıtlı");
                user.Email = request.Email;
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Role = request.Role;
            user.IsActive = request.IsActive;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Kullanıcı güncellendi.");
        }

        public async Task<NoContent> UpdateRoleAsync(int id, UserRole role)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return new NoContent("Kullanıcı bulunamadı!");

            user.Role = role;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Kullanıcı rolü güncellendi.");
        }

        public async Task<NoContent> ToggleActiveAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return new NoContent("Kullanıcı bulunamadı!");

            user.IsActive = !user.IsActive;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent($"Kullanıcı {(user.IsActive ? "aktif" : "pasif")} edildi.");
        }

        public async Task<NoContent> UpdatePasswordAsync(int id, string newPasswordHash)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return new NoContent("Kullanıcı bulunamadı!");

            user.PasswordHash = newPasswordHash;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Şifre güncellendi.");
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RannaTask.Business.Products;
using RannaTask.DAL.Repositories.SupportForms;
using RannaTask.DAL.UnitOfWorks;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.SupportForms
{
    public class SupportFormManager : ISupportFormService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupportFormRepository _supportFormRepository;
        private readonly IMapper _mapper;

        public SupportFormManager(ISupportFormRepository supportFormRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _supportFormRepository = supportFormRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SupportFormDto> CreateAsync(CreateSupportFormDto request)
        {
            var supportForm = _mapper.Map<SupportForm>(request);
            // Created AppDbContext.SaveChangesAsync'te otomatik set edilecek
            // Ama emin olmak için kontrol edelim
            if (supportForm.Created == default)
            {
                supportForm.Created = DateTime.UtcNow;
            }

            await _supportFormRepository.AddAsync(supportForm);
            await _unitOfWork.SaveChangesAsync();
            return new SupportFormDto { Id = supportForm.Id };
        }

        public async Task<List<SupportFormDto>> GetAllListAsync()
        {
            var supportForms = await _supportFormRepository.GetAll().ToListAsync();

            // AutoMapper yerine manuel map - Created dahil
            var supportFormsAsDto = supportForms.Select(sf => new SupportFormDto
            {
                Id = sf.Id,
                Subject = sf.Subject,
                Message = sf.Message,
                Status = sf.Status,
                UserId = sf.UserId,
                CloseReason = sf.CloseReason,
                Created = sf.Created
            }).ToList();

            return supportFormsAsDto;
        }

        public async Task<SupportFormDto?> GetByIdAsync(int id)
        {
            var supportForm = await _supportFormRepository.GetByIdAsync(id);
            if (supportForm is null)
                return null;

            var supportFormAsDto = new SupportFormDto(supportForm.Id, supportForm.Subject, supportForm.Message, supportForm.Status, supportForm.UserId)
            {
                CloseReason = supportForm.CloseReason,
                Created = supportForm.Created
            };

            return supportFormAsDto;
        }

        public async Task<NoContent> UpdateAsync(int id, SupportFormDto request)
        {
            var supportForm = await _supportFormRepository.GetByIdAsync(id);
            if (supportForm is null)
                throw new Exception("SupportForm not found!");

            supportForm = _mapper.Map(request, supportForm);
            supportForm.Id = id;

            _supportFormRepository.Update(supportForm);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent();
        }

        public async Task<NoContent> DeleteAsync(int id)
        {
            var supportForm = await _supportFormRepository.GetByIdAsync(id);
            if (supportForm is null)
                return new NoContent("Destek talebi bulunamadı");

            // Soft delete
            _supportFormRepository.SoftDelete(supportForm);
            await _unitOfWork.SaveChangesAsync();
            return new NoContent("Destek talebi silindi");
        }
    }
}

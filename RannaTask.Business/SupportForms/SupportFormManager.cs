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
            await _supportFormRepository.AddAsync(supportForm);
            await _unitOfWork.SaveChangesAsync();
            return new SupportFormDto { Id = supportForm.Id };
        }

        public async Task<List<SupportFormDto>> GetAllListAsync()
        {
            var supportForms = await _supportFormRepository.GetAll().ToListAsync();

            var supportFormsAsDto = _mapper.Map<List<SupportFormDto>>(supportForms);

            return supportFormsAsDto;
        }

        public async Task<SupportFormDto?> GetByIdAsync(int id)
        {
            var supportForm = await _supportFormRepository.GetByIdAsync(id);
            if (supportForm is null)
                return null;

            var supportFormAsDto = new SupportFormDto(supportForm.Id, supportForm.Subject, supportForm.Message, supportForm.Status, supportForm.UserId);

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

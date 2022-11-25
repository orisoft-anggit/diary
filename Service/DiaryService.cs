using System.Security.Claims;
using AutoMapper;
using Diary.Data;
using Diary.DTOs;
using Diary.Filter;
using Diary.Models;
using Microsoft.EntityFrameworkCore;

namespace Diary.Service
{
    public class DiaryService : IDiaryService
    {
        private AppDataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DiaryService(AppDataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<GetDiaryDTO>> AddDiary(AddDiaryDTO newDiary)
        {
            var serviceResponse = new ServiceResponse<GetDiaryDTO>();
            DiaryEntity diary = _mapper.Map<DiaryEntity>(newDiary);
            diary.AuthEntity = await _context.AuthEntities.FirstOrDefaultAsync(x => x.Id == GetAuthId());

            _context.DiaryEntitys.Add(diary);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetDiaryDTO>(diary);

            return serviceResponse;
        }

        private int GetAuthId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<GetDiaryDTO>> DeleteDiary(int id)
        {
            ServiceResponse<GetDiaryDTO> serviceResponse = new ServiceResponse<GetDiaryDTO>();

            DiaryEntity diary = await _context.DiaryEntitys
                                .FirstOrDefaultAsync(x => x.Id == id && x.AuthEntity.Id == GetAuthId());

            if (diary != null)
            {
                _context.DiaryEntitys.Remove(diary);
                await _context.SaveChangesAsync();
            }
            serviceResponse.Data = _mapper.Map<GetDiaryDTO>(diary);

            return serviceResponse;
        }
        public async Task<ServiceResponse<GetDiaryDTO>> UpdateDiary(UpdateDiaryDTO updateDiary)
        {
            ServiceResponse<GetDiaryDTO> serviceResponse = new ServiceResponse<GetDiaryDTO>();

            var diary = await _context.DiaryEntitys
                    .Include(x => x.AuthEntity)
                    .FirstOrDefaultAsync(x => x.Id == updateDiary.Id);

            if (diary.AuthEntity.Id == GetAuthId())
            {
                diary.Id = updateDiary.Id;
                diary.Note = updateDiary.Note;
                diary.Tittle = updateDiary.Tittle;
                diary.Content = updateDiary.Content;
                diary.Is_Archieved = updateDiary.isArchieved;
                diary.Update_At = updateDiary.updateAt;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetDiaryDTO>(diary);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetDiaryDTO>> GetDiaryById(int id)
        {
            var serviceResponse = new ServiceResponse<GetDiaryDTO>();

            var diary = await _context.DiaryEntitys.FirstOrDefaultAsync(x => x.Id == id && x.AuthEntity.Id == GetAuthId());

            serviceResponse.Data = _mapper.Map<GetDiaryDTO>(diary);

            return serviceResponse;
        }

        public async Task<PagedResponse<GetDiaryDTO>> GetAllDiarys(PaginationFilter filter)
        {
            var serviceResponse = new ServiceResponse<List<GetDiaryDTO>>();

            //get data diary
            var diary = await _context.DiaryEntitys
                .Where(x => x.AuthEntity.Id == GetAuthId())
                .ToListAsync();
            
            //get dto paginition
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = new List<DiaryEntity>();
            var totalRecords = 0;

            //search data by title
            if (filter.Search != null)
            {
                pagedData = await _context.DiaryEntitys
                    .Where(e => e.Tittle.Contains(filter.Search))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
                totalRecords = await _context.DiaryEntitys.Where(e => e.Tittle.Contains(filter.Search)).CountAsync();
            }

            if (filter.Search == null)
            {
                pagedData = await _context.DiaryEntitys
                   .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize)
                   .ToListAsync();
                totalRecords = await _context.DiaryEntitys.CountAsync();
            }

            //mapping data and paginiation
            var pagedResponse = new PagedResponse<GetDiaryDTO>();

                pagedResponse.Data = pagedData.Select(x => _mapper.Map<GetDiaryDTO>(x)).ToList();

            return pagedResponse;
        }
    }
}
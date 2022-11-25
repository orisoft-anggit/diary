using Diary.DTOs;
using Diary.Filter;

namespace Diary.Service
{
    public interface IDiaryService
    {
        //Task<ServiceResponse<List<GetDiaryDTO>>>GetAllDiarys(int limit, int page, CancellationToken cancellationToken);
        Task<PagedResponse<GetDiaryDTO>>GetAllDiarys(PaginationFilter filter);
        Task<ServiceResponse<GetDiaryDTO>> GetDiaryById(int id);
        Task<ServiceResponse<GetDiaryDTO>> AddDiary(AddDiaryDTO newDiary);
        Task<ServiceResponse<GetDiaryDTO>> UpdateDiary(UpdateDiaryDTO updateDiary);
        Task<ServiceResponse<GetDiaryDTO>> DeleteDiary(int id);
    }
}
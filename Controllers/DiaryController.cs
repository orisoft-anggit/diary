using Diary.Data;
using Diary.DTOs;
using Diary.Filter;
using Diary.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DiaryController : ControllerBase
    {
        private AppDataContext _context;
        private readonly IDiaryService _diaryService;

        public DiaryController(AppDataContext context, IDiaryService diaryService)
        {
            _context = context;
            _diaryService = diaryService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResponse<GetDiaryDTO>>> Get([FromQuery]PaginationFilter filter)
        {
            var pagedResponse = await _diaryService.GetAllDiarys(filter);
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetDiaryDTO>>> GetSingle(int id)
        {
            return Ok(await _diaryService.GetDiaryById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetDiaryDTO>>> AddDiary(AddDiaryDTO newDiary)
        {
            return Ok(await _diaryService.AddDiary(newDiary));
        }    

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetDiaryDTO>>> UpdateDiary(UpdateDiaryDTO updateDiary)
        {
            var serviceResponse = await _diaryService.UpdateDiary(updateDiary);
            if (serviceResponse.Data == null)
            {   
                return NotFound(serviceResponse);
            };
            return Ok(serviceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetDiaryDTO>>> Delete(int id)
        {
            var serviceResponse = await _diaryService.DeleteDiary(id);
            if (serviceResponse.Data == null)
            {
                return NotFound(serviceResponse);
            };

            return Ok(serviceResponse);
        }
    }
}
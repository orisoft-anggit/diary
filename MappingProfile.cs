using AutoMapper;
using Diary.DTOs;
using Diary.Models;

namespace Diary;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequestDTO, AuthEntity>();
        
        CreateMap<AuthEntity, RegisterResponseDTO>();

        CreateMap<AddDiaryDTO, DiaryEntity>();
        
        CreateMap<DiaryEntity, GetDiaryDTO>();
        
        CreateMap<UpdateDiaryDTO, DiaryEntity>();
    }
}
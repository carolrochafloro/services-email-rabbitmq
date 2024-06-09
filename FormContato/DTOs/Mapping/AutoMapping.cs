using AutoMapper;
using FormContato.Models;

namespace FormContato.DTOs.Mapping;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<UserModel, RegisterDTO>().ReverseMap();
    }
}

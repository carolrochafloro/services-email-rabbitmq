using AutoMapper;
using FormContato.Models;

namespace FormContato.DTOs.Mapping;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<UserModel, RegisterDTO>().ReverseMap();
        CreateMap<UserModel, LoginDTO>().ReverseMap();
        CreateMap<ContactDTO, ContactModel>().ReverseMap();
        CreateMap<ProfileDTO, UserModel>().ReverseMap();

    }
}

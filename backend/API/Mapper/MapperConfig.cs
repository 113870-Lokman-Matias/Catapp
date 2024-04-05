using AutoMapper;
using API.Models;

using API.Dtos.UsuarioDtos;
using API.Services.UsuarioServices.Commands.CreateUsuarioCommand;
using API.Services.UsuarioServices.Commands.CreateUsuarioNoLogueadoCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioCommand;
using API.Services.UsuarioServices.Commands.UpdateActivoUsuarioCommand;
using API.Services.UsuarioServices.Commands.DeleteUsuarioCommand;
using API.Services.UsuarioServices.Commands.SearchUsuarioCommand;
using API.Services.UsuarioServices.Commands.VerifyUsuarioCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioNoLogueadoPasswordCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioPasswordCommand;

namespace API.Mapper
{
  public class MapperConfig : Profile
  {
    public MapperConfig()
    {
      // Mapper para usuarios
      CreateMap<Usuario, UsuarioDto>()
          .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.IdRolNavigation.Nombre));
      CreateMap<Usuario, CreateUsuarioCommand>().ReverseMap();
      CreateMap<Usuario, CreateUsuarioNoLogueadoCommand>().ReverseMap();
      CreateMap<Usuario, UpdateUsuarioCommand>().ReverseMap();
      CreateMap<Usuario, UpdateActivoUsuarioCommand>().ReverseMap();
      CreateMap<Usuario, DeleteUsuarioCommand>().ReverseMap();
      CreateMap<Usuario, SearchUsuarioCommand>().ReverseMap();
      CreateMap<Usuario, VerifyUsuarioCommand>().ReverseMap();
      CreateMap<Usuario, UpdateUsuarioNoLogueadoPasswordCommand>().ReverseMap();
      CreateMap<Usuario, UpdateUsuarioPasswordCommand>().ReverseMap();
    }
  }
}

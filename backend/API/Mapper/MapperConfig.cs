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

using API.Dtos.CategoriaDtos;
using API.Services.CategoriaServices.Commands.CreateCategoriaCommand;
using API.Services.CategoriaServices.Commands.UpdateCategoriaCommand;
using API.Services.CategoriaServices.Commands.DeleteCategoriaCommand;

using API.Dtos.CotizacionDto;
using API.Services.CotizacionServices.Commands.UpdateCotizacionDolarCommand;

using API.Dtos.ProductoDtos;
using API.Services.ProductoServices.Commands.CreateProductoCommand;
using API.Services.ProductoServices.Commands.UpdateProductoCommand;
using API.Services.ProductoServices.Commands.DeleteProductoCommand;
using API.Services.ProductoServices.Commands.UpdateStockProductoCommand;

using API.Dtos.EnvioDto;
using API.Services.EnvioServices.Commands.UpdateCostoEnvioCommand;

using API.Dtos.StockDtos;
using API.Services.StockServices.Commands.CreateDetalleStockCommand;

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

      // Mapper para Categorías
      CreateMap<CategoriaDto, Categoria>().ReverseMap();
      CreateMap<Categoria, CreateCategoriaCommand>().ReverseMap();
      CreateMap<Categoria, UpdateCategoriaCommand>().ReverseMap();
      CreateMap<Categoria, DeleteCategoriaCommand>().ReverseMap();

      // Mapper para Cotizacion
      CreateMap<CotizacionDto, Cotizacion>().ReverseMap();
      CreateMap<Cotizacion, UpdateCotizacionDolarCommand>().ReverseMap();

      // Mapper para Productos
      CreateMap<Producto, ProductoDto>()
          .ForMember(dest => dest.NombreCategoria, opt => opt.MapFrom(src => src.IdCategoriaNavigation.Nombre));
      CreateMap<Producto, CreateProductoCommand>().ReverseMap();
      CreateMap<Producto, UpdateProductoCommand>().ReverseMap();
      CreateMap<Producto, UpdateStockProductoCommand>().ReverseMap();
      CreateMap<Producto, DeleteProductoCommand>().ReverseMap();

      // Mapper para envios
      CreateMap<EnvioDto, Envio>().ReverseMap();
      CreateMap<Envio, UpdateCostoEnvioCommand>().ReverseMap();

      // Mapper para Detalles de stock
      CreateMap<DetallesStock, StockDto>()
          .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre));
      CreateMap<DetallesStock, CreateDetalleStockCommand>().ReverseMap();
    }
  }
}

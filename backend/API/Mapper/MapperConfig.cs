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
using API.Services.EnvioServices.Commands.UpdateEnvioCommand;

using API.Dtos.StockDtos;
using API.Services.StockServices.Commands.CreateDetalleStockCommand;

using API.Dtos.PedidoDtos;
using API.Services.PedidoServices.Commands.CreatePedidoCommand;
using API.Services.PedidoServices.Commands.UpdatePedidoCommand;
using API.Services.PedidoServices.Commands.DeletePedidoCommand;
using API.Services.PedidoServices.Commands.UpdateVerificadoPedidoCommand;

using API.Dtos.PagoDto;
using API.Services.PagoServices.Commands.CreatePagoCommand;

using API.Dtos.MetodoPagoDto;
using API.Services.MetodoPagoServices.Commands.CreateMetodoPagoCommand;
using API.Services.MetodoPagoServices.Commands.UpdateMetodoPagoCommand;
using API.Services.MetodoPagoServices.Commands.DeleteMetodoPagoCommand;

using API.Dtos.ConfiguracionDto;
using API.Services.ConfiguracionServices.Commands.UpdateConfiguracionCommand;

using API.Dtos.SubcategoriaDtos;
using API.Services.SubcategoriaServices.Commands.CreateSubcategoriaCommand;
using API.Services.SubcategoriaServices.Commands.UpdateSubcategoriaCommand;
using API.Services.SubcategoriaServices.Commands.DeleteSubcategoriaCommand;

namespace API.Mapper
{
  public class MapperConfig : Profile
  {
    public MapperConfig()
    {
      // Mapper para Usuarios
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
      CreateMap<Producto, ProductoDto>()
          .ForMember(dest => dest.NombreSubcategoria, opt => opt.MapFrom(src => src.IdSubcategoriaNavigation.Nombre));
      CreateMap<Producto, CreateProductoCommand>().ReverseMap();
      CreateMap<Producto, UpdateProductoCommand>().ReverseMap();
      CreateMap<Producto, UpdateStockProductoCommand>().ReverseMap();
      CreateMap<Producto, DeleteProductoCommand>().ReverseMap();

      // Mapper para Envios
      CreateMap<EnvioDto, Envio>().ReverseMap();
      CreateMap<Envio, UpdateEnvioCommand>().ReverseMap();

      // Mapper para Detalles de stock
      CreateMap<DetallesStock, StockDto>()
          .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre));
      CreateMap<DetallesStock, CreateDetalleStockCommand>().ReverseMap();

      // Mapper para Pedidos
      CreateMap<PedidoDto, Pedido>().ReverseMap();
      CreateMap<Pedido, CreatePedidoCommand>().ReverseMap();
      CreateMap<Pedido, UpdatePedidoCommand>().ReverseMap();
      CreateMap<Pedido, UpdateVerificadoPedidoCommand>().ReverseMap();
      CreateMap<Pedido, DeletePedidoCommand>().ReverseMap();

      // Mapper para Metodos de pago
      CreateMap<MetodoPagoDto, MetodosPago>().ReverseMap();
      CreateMap<MetodosPago, CreateMetodoPagoCommand>().ReverseMap();
      CreateMap<MetodosPago, UpdateMetodoPagoCommand>().ReverseMap();
      CreateMap<MetodosPago, DeleteMetodoPagoCommand>().ReverseMap();

      // Mapper para Configuraciones
      CreateMap<ConfiguracionDto, Configuracion>().ReverseMap();
      CreateMap<Configuracion, UpdateConfiguracionCommand>().ReverseMap();

      // Mapper para Subcategorías
      CreateMap<SubcategoriaDto, Subcategoria>().ReverseMap();
      CreateMap<Subcategoria, CreateSubcategoriaCommand>().ReverseMap();
      CreateMap<Subcategoria, UpdateSubcategoriaCommand>().ReverseMap();
      CreateMap<Subcategoria, DeleteSubcategoriaCommand>().ReverseMap();
    }
  }
}

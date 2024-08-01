using API.Data;
using AutoMapper.Execution;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Services.PedidoServices.Commands.CreatePedidoCommand
{
  public class CreatePedidoCommandValidator : AbstractValidator<CreatePedidoCommand>
  {
    private readonly CatalogoContext _context;

    public CreatePedidoCommandValidator(CatalogoContext context)
    {
      _context = context;
      // 1. validaciones requeridas para crear el cliente
      RuleFor(p => p.NombreCompleto)
     .NotEmpty().WithMessage("El nombre completo no puede estar vacío")
     .NotNull().WithMessage("El nombre completo no puede ser nulo");

      RuleFor(p => p.Dni)
     .NotEmpty().WithMessage("El DNI no puede estar vacío")
     .NotNull().WithMessage("El DNI no puede ser nulo");

      RuleFor(p => p.Telefono)
     .NotEmpty().WithMessage("El telefono no puede estar vacío")
     .NotNull().WithMessage("El telefono no puede ser nulo");

      //   RuleFor(p => p.Direccion)
      //  .NotEmpty().WithMessage("La direccion no puede estar vacía")
      //  .NotNull().WithMessage("La direccion no puede ser nula");

      //   RuleFor(p => p.EntreCalles)
      //  .NotEmpty().WithMessage("Las calles no pueden estar vacías")
      //  .NotNull().WithMessage("Las calles no pueden ser nulas");


      // 2. validaciones requeridas para crear el pedido
      RuleFor(p => p.CostoEnvio)
      .NotNull().WithMessage("El costo de envío no puede ser nulo");

      RuleFor(p => p.IdTipoPedido)
      .NotEmpty().WithMessage("El id del tipo de pedido no puede estar vacío")
      .NotNull().WithMessage("El id del tipo de pedido no puede ser nulo")
      .MustAsync(TipoPedidoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un tipo de pedido existente");

      RuleFor(p => p.IdVendedor)
      .NotEmpty().WithMessage("El id del vendedor no puede estar vacío")
      .NotNull().WithMessage("El id del vendedor no puede ser nulo")
      .MustAsync(VendedorExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un vendedor existente")
      .Unless(p => p.IdVendedor == -1);

      RuleFor(p => p.IdMetodoPago)
     .NotEmpty().WithMessage("El id del metodo de pago no puede estar vacío")
     .NotNull().WithMessage("El id del metodo de pago no puede ser nulo")
     .MustAsync(MetodoPagoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un metodo de pago existente");

      RuleFor(p => p.IdMetodoEntrega)
      .NotEmpty().WithMessage("El id del metodo de entrega no puede estar vacío")
      .NotNull().WithMessage("El id del metodo de entrega no puede ser nulo")
      .MustAsync(MetodoEntregaExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un metodo de entrega existente");


      // 3. validaciones requeridas para los detalles de pedido
      RuleFor(p => p.Detalles)
                      .NotEmpty().WithMessage("La lista de detalles no puede estar vacía")
                      .NotNull().WithMessage("La lista de detalles no puede ser nula");

      RuleForEach(p => p.Detalles)
          .ChildRules(detalle =>
          {
            detalle.RuleFor(d => d.IdProducto)
            .NotEmpty().WithMessage("El id del producto en el detalle no puede estar vacío")
            .NotNull().WithMessage("El id del producto en el detalle no puede ser nulo")
            .MustAsync(ProductoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un producto existente");

            detalle.RuleFor(d => d.Cantidad)
            .NotEmpty().WithMessage("La cantidad no puede estar vacía")
            .NotNull().WithMessage("La cantidad no puede ser nulo");

            // detalle.RuleFor(d => d.Aclaracion)
            // .NotEmpty().WithMessage("La aclaracion no puede estar vacía")
            // .NotNull().WithMessage("La aclaracion no puede ser nula");

            detalle.RuleFor(d => d.PrecioUnitario)
            .NotEmpty().WithMessage("El precio unitario no puede estar vacío")
            .NotNull().WithMessage("El precio unitario no puede ser nulo");
          });
    }

    // 2. validaciones personalizadas para crear el pedido
    private async Task<bool> TipoPedidoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.TiposPedidos.AnyAsync(p => p.IdTipoPedido == id);
      return existe;
    }

    private async Task<bool> VendedorExiste(int? id, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(p => p.IdUsuario == id);
      return existe;
    }

    private async Task<bool> MetodoPagoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.MetodosPagos.AnyAsync(p => p.IdMetodoPago == id);
      return existe;
    }

    private async Task<bool> MetodoEntregaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Envios.AnyAsync(p => p.IdEnvio == id);
      return existe;
    }


    // 2. validaciones personalizadas para crear el detalle de pedido
    private async Task<bool> ProductoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Productos.AnyAsync(p => p.IdProducto == id);
      return existe;
    }

    private async Task<bool> PedidoExiste(Guid id, CancellationToken token)
    {
      bool existe = await _context.Pedidos.AnyAsync(p => p.IdPedido == id);
      return existe;
    }
  }
}

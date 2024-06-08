using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using API.Models;

namespace API.Data
{
    public partial class CatalogoContext : DbContext
    {
        public CatalogoContext()
        {
        }

        public CatalogoContext(DbContextOptions<CatalogoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categorias { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Configuracion> Configuraciones { get; set; } = null!;
        public virtual DbSet<Cotizacion> Cotizaciones { get; set; } = null!;
        public virtual DbSet<DetallePedido> DetallePedidos { get; set; } = null!;
        public virtual DbSet<DetallesStock> DetallesStocks { get; set; } = null!;
        public virtual DbSet<Divisa> Divisas { get; set; } = null!;
        public virtual DbSet<Envio> Envios { get; set; } = null!;
        public virtual DbSet<MetodosEntrega> MetodosEntregas { get; set; } = null!;
        public virtual DbSet<MetodosPago> MetodosPagos { get; set; } = null!;
        public virtual DbSet<Pedido> Pedidos { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Rol> Roles { get; set; } = null!;
        public virtual DbSet<TiposPedido> TiposPedidos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("categorias_pkey");

                entity.ToTable("categorias");

                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");

                entity.Property(e => e.IdImagen).HasColumnName("id_imagen");

                entity.Property(e => e.Nombre).HasColumnName("nombre");

                entity.Property(e => e.Ocultar).HasColumnName("ocultar");

                entity.Property(e => e.UrlImagen).HasColumnName("url_imagen");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("clientes_pkey");

                entity.ToTable("clientes");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.Direccion).HasColumnName("direccion");

                entity.Property(e => e.Dni).HasColumnName("dni");

                entity.Property(e => e.EntreCalles).HasColumnName("entre_calles");

                entity.Property(e => e.NombreCompleto).HasColumnName("nombre_completo");

                entity.Property(e => e.Telefono).HasColumnName("telefono");
            });

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasKey(e => e.IdConfiguracion)
                    .HasName("configuraciones_pkey");

                entity.ToTable("configuraciones");

                entity.Property(e => e.IdConfiguracion).HasColumnName("id_configuracion");

                entity.Property(e => e.Alias).HasColumnName("alias");

                entity.Property(e => e.MontoMayorista).HasColumnName("monto_mayorista");

                entity.Property(e => e.Cbu).HasColumnName("cbu");

                entity.Property(e => e.Direccion).HasColumnName("direccion");

                entity.Property(e => e.Facebook).HasColumnName("facebook");

                entity.Property(e => e.Horarios).HasColumnName("horarios");

                entity.Property(e => e.Instagram).HasColumnName("instagram");

                entity.Property(e => e.Telefono).HasColumnName("telefono");

                entity.Property(e => e.UrlDireccion).HasColumnName("url_direccion");

                entity.Property(e => e.UrlFacebook).HasColumnName("url_facebook");

                entity.Property(e => e.UrlInstagram).HasColumnName("url_instagram");

                entity.Property(e => e.UrlLogo).HasColumnName("url_logo");

                entity.Property(e => e.Whatsapp)
                    .HasColumnName("whatsapp")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Cotizacion>(entity =>
            {
                entity.HasKey(e => e.IdCotizacion)
                    .HasName("cotizaciones_pkey");

                entity.ToTable("cotizaciones");

                entity.Property(e => e.IdCotizacion).HasColumnName("id_cotizacion");

                entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");

                entity.Property(e => e.Precio).HasColumnName("precio");

                entity.Property(e => e.UltimoModificador).HasColumnName("ultimo_modificador");
            });

            modelBuilder.Entity<DetallePedido>(entity =>
            {
                entity.HasKey(e => e.IdDetallePedidos)
                    .HasName("detalle_pedidos_pkey");

                entity.ToTable("detalle_pedidos");

                entity.HasIndex(e => e.IdPedido, "fki_fk_pedido");

                entity.HasIndex(e => e.IdProducto, "fki_fk_producto");

                entity.Property(e => e.IdDetallePedidos).HasColumnName("id_detalle_pedidos");

                entity.Property(e => e.Aclaracion).HasColumnName("aclaracion");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.IdPedido).HasColumnName("id_pedido");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.DetallePedidos)
                    .HasForeignKey(d => d.IdPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pedido");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.DetallePedidos)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_producto");
            });

            modelBuilder.Entity<DetallesStock>(entity =>
            {
                entity.HasKey(e => e.IdDetallesStock)
                    .HasName("detalles_stock_pkey");

                entity.ToTable("detalles_stock");

                entity.Property(e => e.IdDetallesStock).HasColumnName("id_detalles_stock");

                entity.Property(e => e.Accion).HasColumnName("accion");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Fecha).HasColumnName("fecha");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.Modificador).HasColumnName("modificador");

                entity.Property(e => e.Motivo).HasColumnName("motivo");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.DetallesStocks)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_producto");
            });

            modelBuilder.Entity<Divisa>(entity =>
            {
                entity.HasKey(e => e.IdDivisa)
                    .HasName("divisas_pkey");

                entity.ToTable("divisas");

                entity.Property(e => e.IdDivisa).HasColumnName("id_divisa");

                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });

            modelBuilder.Entity<Envio>(entity =>
            {
                entity.HasKey(e => e.IdEnvio)
                    .HasName("envios_pkey");

                entity.ToTable("envios");

                entity.Property(e => e.IdEnvio).HasColumnName("id_envio");

                entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");

                entity.Property(e => e.Habilitado).HasColumnName("habilitado");

                entity.Property(e => e.Precio).HasColumnName("precio");

                entity.Property(e => e.UltimoModificador).HasColumnName("ultimo_modificador");
            });

            modelBuilder.Entity<MetodosEntrega>(entity =>
            {
                entity.HasKey(e => e.IdMetodoEntrega)
                    .HasName("metodos_entrega_pkey");

                entity.ToTable("metodos_entrega");

                entity.Property(e => e.IdMetodoEntrega).HasColumnName("id_metodo_entrega");

                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });

            modelBuilder.Entity<MetodosPago>(entity =>
            {
                entity.HasKey(e => e.IdMetodoPago)
                    .HasName("metodos_pago_pkey");

                entity.ToTable("metodos_pago");

                entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");

                entity.Property(e => e.Disponibilidad).HasColumnName("disponibilidad");

                entity.Property(e => e.DisponibilidadCatalogo).HasColumnName("disponibilidad_catalogo");

                entity.Property(e => e.Habilitado).HasColumnName("habilitado");

                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido)
                    .HasName("pedidos_pkey");

                entity.ToTable("pedidos");

                entity.HasIndex(e => e.IdCliente, "fki_fk_cliente");

                entity.HasIndex(e => e.IdMetodoEntrega, "fki_fk_metodo_entrega");

                entity.HasIndex(e => e.IdMetodoPago, "fki_fk_metodo_pago");

                entity.HasIndex(e => e.IdTipoPedido, "fki_fk_tipo_pedido");

                entity.HasIndex(e => e.IdVendedor, "fki_fk_vendedor");

                entity.Property(e => e.IdPedido)
                    .ValueGeneratedNever()
                    .HasColumnName("id_pedido");

                entity.Property(e => e.CostoEnvio).HasColumnName("costo_envio");

                entity.Property(e => e.Direccion).HasColumnName("direccion");

                entity.Property(e => e.EntreCalles).HasColumnName("entre_calles");

                entity.Property(e => e.Fecha).HasColumnName("fecha");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.IdMetodoEntrega).HasColumnName("id_metodo_entrega");

                entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");

                entity.Property(e => e.IdTipoPedido).HasColumnName("id_tipo_pedido");

                entity.Property(e => e.IdVendedor).HasColumnName("id_vendedor");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Verificado).HasColumnName("verificado");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cliente");

                entity.HasOne(d => d.IdMetodoEntregaNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdMetodoEntrega)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_metodo_entrega");

                entity.HasOne(d => d.IdMetodoPagoNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdMetodoPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_metodo_pago");

                entity.HasOne(d => d.IdTipoPedidoNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdTipoPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tipo_pedido");

                entity.HasOne(d => d.IdVendedorNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdVendedor)
                    .HasConstraintName("fk_vendedor");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("productos_pkey");

                entity.ToTable("productos");

                entity.HasIndex(e => e.IdCategoria, "fki_fk_categoria");

                entity.HasIndex(e => e.IdDivisa, "fki_fk_divisa");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.Descripcion).HasColumnName("descripcion");

                entity.Property(e => e.EnDestacado).HasColumnName("en_destacado");

                entity.Property(e => e.EnPromocion).HasColumnName("en_promocion");

                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");

                entity.Property(e => e.IdDivisa).HasColumnName("id_divisa");

                entity.Property(e => e.IdImagen).HasColumnName("id_imagen");

                entity.Property(e => e.Nombre).HasColumnName("nombre");

                entity.Property(e => e.Ocultar).HasColumnName("ocultar");

                entity.Property(e => e.PorcentajeMayorista).HasColumnName("porcentaje_mayorista");

                entity.Property(e => e.PorcentajeMinorista).HasColumnName("porcentaje_minorista");

                entity.Property(e => e.Precio).HasColumnName("precio");

                entity.Property(e => e.PrecioMayorista).HasColumnName("precio_mayorista");

                entity.Property(e => e.PrecioMinorista).HasColumnName("precio_minorista");

                entity.Property(e => e.Stock).HasColumnName("stock");

                entity.Property(e => e.StockTransitorio).HasColumnName("stock_transitorio");

                entity.Property(e => e.UrlImagen).HasColumnName("url_imagen");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_categoria");

                entity.HasOne(d => d.IdDivisaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdDivisa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_divisa");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("roles_pkey");

                entity.ToTable("roles");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });

            modelBuilder.Entity<TiposPedido>(entity =>
            {
                entity.HasKey(e => e.IdTipoPedido)
                    .HasName("tipos_pedido_pkey");

                entity.ToTable("tipos_pedido");

                entity.Property(e => e.IdTipoPedido).HasColumnName("id_tipo_pedido");

                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("usuarios_pkey");

                entity.ToTable("usuarios");

                entity.HasIndex(e => e.IdRol, "fki_fk_rol");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.CodigoVerificacion).HasColumnName("codigo_verificacion");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.Nombre).HasColumnName("nombre");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Username).HasColumnName("username");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

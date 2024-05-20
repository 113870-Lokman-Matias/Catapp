using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using FluentValidation;

using API.Services.UsuarioServices.Commands.LoginUsuarioCommand;
using API.Services.UsuarioServices.Queries.GetUsuariosByRolManageQuery;
using API.Services.UsuarioServices.Commands.CreateUsuarioCommand;
using API.Services.UsuarioServices.Commands.CreateUsuarioNoLogueadoCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioCommand;
using API.Services.UsuarioServices.Commands.UpdateActivoUsuarioCommand;
using API.Services.UsuarioServices.Commands.DeleteUsuarioCommand;
using API.Services.UsuarioServices.Commands.SearchUsuarioCommand;
using API.Services.UsuarioServices.Commands.VerifyUsuarioCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioNoLogueadoPasswordCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioPasswordCommand;

using API.Services.CategoriaServices.Commands.CreateCategoriaCommand;
using API.Services.CategoriaServices.Commands.UpdateCategoriaCommand;
using API.Services.CategoriaServices.Commands.DeleteCategoriaCommand;

using API.Services.CotizacionServices.Commands.UpdateCotizacionDolarCommand;

using API.Services.ProductoServices.Queries.GetProductosByCategoryQuery;
using API.Services.ProductoServices.Queries.GetProductosByQueryQuery;
using API.Services.ProductoServices.Queries.GetProductoByIdQuery;
using API.Services.ProductoServices.Commands.CreateProductoCommand;
using API.Services.ProductoServices.Commands.UpdateProductoCommand;
using API.Services.ProductoServices.Commands.UpdateStockProductoCommand;
using API.Services.ProductoServices.Commands.DeleteProductoCommand;

using API.Services.EnvioServices.Commands.UpdateCostoEnvioCommand;

using API.Services.StockServices.Queries.GetDetallesStockByIdQuery;
using API.Services.StockServices.Commands.CreateDetalleStockCommand;

using API.Services.PedidoServices.Queries.GetPedidosByDateQuery;
using API.Services.PedidoServices.Queries.GetPedidosDataByYearQuery;
using API.Services.PedidoServices.Queries.GetPedidosDataByMonthYearQuery;
using API.Services.PedidoServices.Commands.CreatePedidoCommand;
using API.Services.PedidoServices.Commands.UpdatePedidoCommand;
using API.Services.PedidoServices.Commands.UpdateVerificadoPedidoCommand;
using API.Services.PedidoServices.Commands.DeletePedidoCommand;

using API.Services.PagoServices.Commands.CreatePagoCommand;

using MercadoPago.Config;

var builder = WebApplication.CreateBuilder(args);

// SignalR
builder.Services.AddSignalR();

// Credenciales MercadoPago
MercadoPagoConfig.AccessToken = "TEST-718354923242289-012400-c44c2ef741fc53df597d76a76ba62dbf-1070291733";

// Add services to the container.

// CORS
builder.Services.AddCors();

// PostgreSQL
builder.Services.AddDbContext<CatalogoContext>(options =>
 options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT
var _authkey = builder.Configuration.GetValue<string>("JwtSettings:securitykey");
builder.Services.AddAuthentication(item =>
{
  item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
  item.RequireHttpsMetadata = true;
  item.SaveToken = true;
  item.TokenValidationParameters = new TokenValidationParameters()
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
    ValidateIssuer = false,
    ValidateAudience = false,
    ClockSkew = TimeSpan.Zero
  };
});

//AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// mediatR
builder.Services.AddMediatR(opt =>
{
  opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Validaciones para los servicios de Usuarios
builder.Services.AddScoped<IValidator<LoginUsuarioCommand>, LoginUsuarioCommandValidator>();
builder.Services.AddScoped<IValidator<CreateUsuarioCommand>, CreateUsuarioCommandValidator>();
builder.Services.AddScoped<IValidator<CreateUsuarioNoLogueadoCommand>, CreateUsuarioNoLogueadoCommandValidator>();
builder.Services.AddScoped<IValidator<GetUsuariosByRolManageQuery>, GetUsuariosByRolManageQueryValidator>();
builder.Services.AddScoped<IValidator<UpdateUsuarioCommand>, UpdateUsuarioCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateActivoUsuarioCommand>, UpdateActivoUsuarioCommandValidator>();
builder.Services.AddScoped<IValidator<DeleteUsuarioCommand>, DeleteUsuarioCommandValidator>();
builder.Services.AddScoped<IValidator<SearchUsuarioCommand>, SearchUsuarioCommandValidator>();
builder.Services.AddScoped<IValidator<VerifyUsuarioCommand>, VerifyUsuarioCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateUsuarioNoLogueadoPasswordCommand>, UpdateUsuarioNoLogueadoPasswordCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateUsuarioPasswordCommand>, UpdateUsuarioPasswordCommandValidator>();

// Validaciones para los servicios de Categorias
builder.Services.AddScoped<IValidator<CreateCategoriaCommand>, CreateCategoriaCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoriaCommand>, UpdateCategoriaCommandValidator>();
builder.Services.AddScoped<IValidator<DeleteCategoriaCommand>, DeleteCategoriaCommandValidator>();

// Validacion para el servicio de Cotización
builder.Services.AddScoped<IValidator<UpdateCotizacionDolarCommand>, UpdateCotizacionDolarCommandValidator>();

// Validacion para los servicios de Productos
builder.Services.AddScoped<IValidator<CreateProductoCommand>, CreateProductoCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateProductoCommand>, UpdateProductoCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateStockProductoCommand>, UpdateStockProductoCommandValidator>();
builder.Services.AddScoped<IValidator<DeleteProductoCommand>, DeleteProductoCommandValidator>();
builder.Services.AddScoped<IValidator<GetProductosByCategoryQuery>, GetProductosByCategoryQueryValidator>();
builder.Services.AddScoped<IValidator<GetProductosByQueryQuery>, GetProductosByQueryQueryValidator>();
builder.Services.AddScoped<IValidator<GetProductoByIdQuery>, GetProductoByIdQueryValidator>();

// Validacion para el servicio de Envio
builder.Services.AddScoped<IValidator<UpdateCostoEnvioCommand>, UpdateCostoEnvioCommandValidator>();

// Validacion para el servicio de Detalles de stock
builder.Services.AddScoped<IValidator<CreateDetalleStockCommand>, CreateDetalleStockCommandValidator>();
builder.Services.AddScoped<IValidator<GetDetallesStockByIdQuery>, GetDetallesStockByIdQueryValidator>();

// Validaciones para el servicio de Pedidos
builder.Services.AddScoped<IValidator<GetPedidosByDateQuery>, GetPedidosByDateQueryValidator>();
builder.Services.AddScoped<IValidator<GetPedidosDataByYearQuery>, GetPedidosDataByYearQueryValidator>();
builder.Services.AddScoped<IValidator<GetPedidosDataByMonthYearQuery>, GetPedidosDataByMonthYearQueryValidator>();
builder.Services.AddScoped<IValidator<CreatePedidoCommand>, CreatePedidoCommandValidator>();
builder.Services.AddScoped<IValidator<UpdatePedidoCommand>, UpdatePedidoCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateVerificadoPedidoCommand>, UpdateVerificadoPedidoCommandValidator>();
builder.Services.AddScoped<IValidator<DeletePedidoCommand>, DeletePedidoCommandValidator>();

// Validacion para el servicio de Pago
builder.Services.AddScoped<IValidator<CreatePagoCommand>, CreatePagoCommandValidator>();

builder.Services.AddControllers();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// JWT
var _jwtsettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSetings>(_jwtsettings);

// SMTP
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// CORS
app.UseCors(c =>
{
  c.AllowAnyHeader();
  c.AllowAnyMethod();
  // c.AllowAnyOrigin();

  // SignalR
  c.WithOrigins("http://localhost:3000");
  c.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// SignalR Hub
app.MapHub<GeneralHub>("/generalHub");

app.Run();
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


var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();

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
  c.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
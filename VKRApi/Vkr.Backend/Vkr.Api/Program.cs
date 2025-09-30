using System.Net.Mime;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Vkr.DataAccess;
using Vkr.Application;
using Vkr.API.Extensions;
using Vkr.Application.Interfaces.Repositories.FilesRepositories;
using Vkr.Application.Interfaces.Services.FilesService;
using Vkr.Application.Services.FilesServices;
using Vkr.DataAccess.Repositories.OrganizationRepositories;
using Vkr.Infractructure;
using Vkr.FileStorage;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigins", // Changed from single to double quotes
                      policy =>
                      {
                          policy
                          .AllowAnyOrigin() // Allow any origin
                          .AllowAnyMethod() // Allow any HTTP method
                          .AllowAnyHeader(); // Allow any header
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection(nameof(MinioOptions)));
builder.Services.AddApiAuthentication(jwtOptions!);
builder.Services.ConfigureSwagger();
builder.Services.AddHttpContextAccessor();

// 1) EF-Core + DbContext
builder.Services
    .AddDataAccess(builder.Configuration.GetConnectionString("DefaultConnection")!);

// 2) файл-хранилище (Minio/S3)
var minioOpts = builder.Configuration.GetSection(nameof(MinioOptions)).Get<MinioOptions>()!;
builder.Services.AddFileStorage(minioOpts);

// 3) Репозиторий + сервис
builder.Services.AddScoped<IFilesRepository, FilesRepository>();
builder.Services.AddScoped<IFilesService, FilesService>();

// 4) Application + Infrastructure и т.д.
builder.Services
    .AddApplicationLayer()
    .AddInfrastructureLayer();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = MediaTypeNames.Text.Plain;

        await context.Response.WriteAsync("An exception was thrown.");

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
        {
            await context.Response.WriteAsync(" The file was not found.");
        }

        if (exceptionHandlerPathFeature?.Path == "/")
        {
            await context.Response.WriteAsync(" Page: Home.");
        }
    });
});

app.UseCors("AllowAnyOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

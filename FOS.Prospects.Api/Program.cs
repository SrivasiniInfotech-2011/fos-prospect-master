using FluentValidation;
using FluentValidation.AspNetCore;
using FOS.Infrastructure.Commands;
using FOS.Infrastructure.Queries;
using FOS.Infrastructure.Validators;
using FOS.Models.Constants;
using FOS.Models.Entities;
using FOS.Prospects.Api.Middleware;
using FOS.Repository.Implementors;
using FOS.Repository.Interfaces;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using System.ComponentModel;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
//builder.Services.AddValidatorsFromAssemblyContaining<BranchLocationRequestValidator>();
builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(FOS.Infrastructure.Startup))));
builder.Services.AddControllers();
var configuration = builder.Configuration
                         .AddJsonFile($"appsettings.json")
                         .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
                         .Build();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FOS.PROSPECT.API", Version = "v1" });
});

builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
             .AddIdentityServerAuthentication(options =>
             {
                 options.Authority = configuration["IdentityServerUrl"];
                 options.ApiName = Constants.ApiResource.UserApi;
                 options.ApiSecret = Constants.ApiResource.ApiResourceSecret;
                 options.RequireHttpsMetadata = false;
             });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.WithOrigins(configuration["AllowCORSUrls"]!.Split(','))
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<FOS.Prospects.Api.Startup>());
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(FOS.Models.Constants.Startup)));
builder.Services.AddTransient<IMediator, Mediator>();
builder.Services.AddTransient<IProspectRepository>(s => new ProspectRepository(configuration.GetConnectionString("FOSConnectionString")!));
builder.Services.AddTransient<IRequestHandler<GetBranchLocations.Query, List<Location>>, GetBranchLocations.Handler>();
builder.Services.AddTransient<IRequestHandler<GetExistingProspectCustomerDetails.Query, Prospect>, GetExistingProspectCustomerDetails.Handler>();
builder.Services.AddTransient<IRequestHandler<GetProspectLookups.Query, List<Lookup>>, GetProspectLookups.Handler>();
builder.Services.AddTransient<IRequestHandler<GetStates.Query, List<Lookup>>, GetStates.Handler>();
builder.Services.AddTransient<IRequestHandler<CreateProspectCommand.Command, int>, CreateProspectCommand.Handler>();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularApp");
app.UseMiddleware<FOSExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

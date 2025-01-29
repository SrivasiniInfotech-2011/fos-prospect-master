using FluentValidation.AspNetCore;
using FOS.Infrastructure;
using FOS.Infrastructure.Commands;
using FOS.Infrastructure.Queries;
using FOS.Infrastructure.Services.File;
using FOS.Infrastructure.Services.FileServer;
using FOS.Models.Configurations;
using FOS.Models.Constants;
using FOS.Models.Entities;
using FOS.Models.Requests;
using FOS.Prospects.Api.Middleware;
using FOS.Repository.Implementors;
using FOS.Repository.Interfaces;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
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
        builder => builder.WithOrigins(configuration["AllowCORSUrls"]!.Split(","))
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<FOS.Prospects.Api.Startup>());
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(FOS.Models.Constants.Startup)));
builder.Services.AddTransient<IMediator, Mediator>();
builder.Services.AddTransient<IProspectRepository>(s => new ProspectRepository(configuration.GetConnectionString("FOSConnectionString")!));
builder.Services.AddTransient<ILeadsRepository>(s => new LeadsRepository(configuration.GetConnectionString("FOSConnectionString")!));
builder.Services.AddTransient<IFieldVerficationRepository>(s => new FieldVerficationRepository(configuration.GetConnectionString("FOSConnectionString")!));
builder.Services.AddTransient<IRequestHandler<GetLeadStatuses.Query, IEnumerable<LeadStatus>>, GetLeadStatuses.Handler>();
builder.Services.AddTransient<IRequestHandler<GetAssetLookup.Query, IEnumerable<Lookup>?>, GetAssetLookup.Handler>();
builder.Services.AddTransient<IRequestHandler<GetFvrNeighbourLookup.Query, IEnumerable<Lookup>?>, GetFvrNeighbourLookup.Handler>();
builder.Services.AddTransient<IRequestHandler<GetLeadDetails.Query, Lead>, GetLeadDetails.Handler>();
builder.Services.AddTransient<IRequestHandler<GetLeadGenerationLookup.Query, IEnumerable<Lookup>?>, GetLeadGenerationLookup.Handler>();
builder.Services.AddTransient<IRequestHandler<GetLeadStatuses.Query, IEnumerable<LeadStatus>>, GetLeadStatuses.Handler>();
builder.Services.AddTransient<IRequestHandler<GetProspectDetailsForLead.Query, LeadProspectDetail>, GetProspectDetailsForLead.Handler>();
builder.Services.AddTransient<IRequestHandler<CreateGuarantorData.Command, bool>, CreateGuarantorData.Handler>();
builder.Services.AddTransient<IRequestHandler<CreateLeadDetails.Command, LeadHeader>, CreateLeadDetails.Handler>();
builder.Services.AddTransient<IRequestHandler<CreateLeadIndividualDetails.Command, int>, CreateLeadIndividualDetails.Handler>();
builder.Services.AddTransient<IRequestHandler<CreateNonIndividualDetail.Command, int>, CreateNonIndividualDetail.Handler>();
builder.Services.AddTransient<IRequestHandler<CreatetLeadGenerationHeader.Command, int>, CreatetLeadGenerationHeader.Handler>();
builder.Services.AddTransient<IRequestHandler<GetBranchLocations.Query, List<Location>>, GetBranchLocations.Handler>();
builder.Services.AddTransient<IRequestHandler<GetExistingProspectCustomerDetails.Query, Prospect>, GetExistingProspectCustomerDetails.Handler>();
builder.Services.AddTransient<IRequestHandler<GetProspectLookups.Query, List<Lookup>>, GetProspectLookups.Handler>();
builder.Services.AddTransient<IRequestHandler<GetStates.Query, List<Lookup>>, GetStates.Handler>();
builder.Services.AddTransient<IRequestHandler<GetFvrHirerLookup.Query, IEnumerable<Lookup>?>, GetFvrHirerLookup.Handler>();
builder.Services.AddTransient<IRequestHandler<GetFvrAssetLookup.Query, IEnumerable<Lookup>?>, GetFvrAssetLookup.Handler>();
builder.Services.AddTransient<IRequestHandler<GetFvrNeighbourHoodDetails.Query, FvrDetail?>, GetFvrNeighbourHoodDetails.Handler>();
builder.Services.AddTransient<IRequestHandler<GetLeadAssetDetails.Query, FvrAsset?>, GetLeadAssetDetails.Handler>();
builder.Services.AddTransient<IRequestHandler<GetLeadHirerDetails.Query, FvrDetail?>, GetLeadHirerDetails.Handler>();
builder.Services.AddTransient<IRequestHandler<AddFvrHirerDetail.Command, int>, AddFvrHirerDetail.Handler>();
builder.Services.AddTransient<IRequestHandler<AddFvrAssetDetail.Command, int>, AddFvrAssetDetail.Handler>();
builder.Services.AddTransient<IRequestHandler<CreateProspectCommand.Command, int>, CreateProspectCommand.Handler>();
builder.Services.AddTransient<IRequestHandler<GetLeadsForTranslander.Query, LeadsTranslander>, GetLeadsForTranslander.Handler>();
builder.Services.AddTransient<IRequestHandler<GetLobList.Query, IEnumerable<LineOfBusiness>?>, GetLobList.Handler>();
builder.Services.AddTransient<IRequestHandler<GetFieldExecutives.Query, IEnumerable<FieldExecutive>?>, GetFieldExecutives.Handler>();
builder.Services.AddTransient<IRequestHandler<GetDocumentCategories.Query, IEnumerable<DocumentCategory>?>, GetDocumentCategories.Handler>();
builder.Services.AddTransient<IRequestHandler<DownloadProspectReport.Query, Stream>, DownloadProspectReport.Handler>();
builder.Services.AddTransient<IRequestHandler<GetCompanyMaster.Query, CompanyMasterRequest>, GetCompanyMaster.Handler>();
builder.Services.AddTransient<IRequestHandler<GetGlobalParameter.Query, List<GlobalParameterRequest>>, GetGlobalParameter.Handler>();
//builder.Services.AddTransient<IRequestHandler<GetGlobalParameter.Query,GlobalParameterRequest>,GetGlobalParameter.Handler();

builder.Services.AddSingleton<FileServerConfiguration>();
builder.Services.AddSingleton<ExcelFileService>();
builder.Services.AddSingleton<PdfFileService>();
builder.Services.AddTransient<IRequestHandler<GetFvrDetails.Query, FvrDetail?>, GetFvrDetails.Handler>();
builder.Services.AddTransient<IFileServerService>(s => new FileServerService(
    new FileServerConfiguration
    {
        CmsUrl = configuration["CmsUrl"].ToString(),
        CmsFilePath = configuration["CmsPath"].ToString(),

    }, s.GetService<ILogger<FileServerService>>()));

builder.Services.AddTransient<FileServiceResolver>(serviceProvider => key =>
{
    return key switch
    {
        Constants.FileOutput.EXCEL => serviceProvider.GetService<ExcelFileService>()!,
        Constants.FileOutput.PDF => serviceProvider.GetService<PdfFileService>()!,
        _ => throw new KeyNotFoundException()
    };
});

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

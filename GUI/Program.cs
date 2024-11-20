using JustLabel.Data;
using JustLabel.DataMongoDb;
using JustLabel.Repositories.Interfaces;
using JustLabel.Repositories;
using JustLabel.Repositories.MongoDb;
using JustLabel.Services;
using JustLabel.Services.Interfaces;
using JustLabel.Middleware;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Serilog;
using Serilog.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .Filter.ByExcluding(logEvent =>
        logEvent.Properties.ContainsKey("SourceContext") &&
        logEvent.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore.Database.Command")
    )
    .CreateLogger();


builder.Host.ConfigureServices((context, services) =>
{
    services.Configure<FormOptions>(options =>
    {
        options.ValueCountLimit = 10;
        options.ValueLengthLimit = int.MaxValue;
        options.MultipartBodyLengthLimit = long.MaxValue;
    });

    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog();
    });

    services.AddMvc();

    services.AddControllers();

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });
    services.AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API", Version = "v1" });
        c.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API", Version = "v2" });

        c.AddSecurityDefinition("AccessToken", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Введите токен аутентификации",
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
        });

        // Добавляем требование безопасности
        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "AccessToken"
                    }
                },
                new string[] {}
            }
        });
    });


    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
});

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IDatasetService, DatasetService>();
builder.Services.AddTransient<ILabelService, LabelService>();
builder.Services.AddTransient<IMarkedService, MarkedService>();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddTransient<ISchemeService, SchemeService>();
builder.Services.AddTransient<IUserService, UserService>();

var memoryCache = new MemoryCache(new MemoryCacheOptions());
builder.Services.AddSingleton<IMemoryCache>(memoryCache);

bool envConf;
if (!bool.TryParse(builder.Configuration["EnvironmentConfig"], out envConf))
{
    envConf = false;
}

var dbms = builder.Configuration["DBMS"];
if (!envConf)
{
    if (dbms == "MongoDb")
    {
        builder.Services.AddTransient<IDatasetRepository, DatasetRepositoryMongoDb>();
        builder.Services.AddTransient<IImageRepository, ImageRepositoryMongoDb>();
        builder.Services.AddTransient<ILabelRepository, LabelRepositoryMongoDb>();
        builder.Services.AddTransient<IMarkedRepository, MarkedRepositoryMongoDb>();
        builder.Services.AddTransient<IReportRepository, ReportRepositoryMongoDb>();
        builder.Services.AddTransient<ISchemeRepository, SchemeRepositoryMongoDb>();
        builder.Services.AddTransient<IUserRepository, UserRepositoryMongoDb>();
        var client = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
        builder.Services.AddDbContext<AppDbContextMongoDb>(options =>
        {
            options.UseMongoDB(client, "JustLabelDB");
        });
        builder.Services.AddScoped<AppDbContextMongoDb>();
    }
    else if (dbms == "PostgreSQL")
    {
        builder.Services.AddTransient<IDatasetRepository, DatasetRepository>();
        builder.Services.AddTransient<IImageRepository, ImageRepository>();
        builder.Services.AddTransient<ILabelRepository, LabelRepository>();
        builder.Services.AddTransient<IMarkedRepository, MarkedRepository>();
        builder.Services.AddTransient<IReportRepository, ReportRepository>();
        builder.Services.AddTransient<ISchemeRepository, SchemeRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
        });
        builder.Services.AddScoped<AppDbContext>();
    }
    else
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("AppDatabase");
        });
        builder.Services.AddScoped<AppDbContext>();
    }
}
else
{
    if (dbms == "PostgreSQL")
    {
        builder.Services.AddTransient<IDatasetRepository, DatasetRepository>();
        builder.Services.AddTransient<IImageRepository, ImageRepository>();
        builder.Services.AddTransient<ILabelRepository, LabelRepository>();
        builder.Services.AddTransient<IMarkedRepository, MarkedRepository>();
        builder.Services.AddTransient<IReportRepository, ReportRepository>();
        builder.Services.AddTransient<ISchemeRepository, SchemeRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            string host = Environment.GetEnvironmentVariable("POSTGRESQL_HOST")!;
            string port = Environment.GetEnvironmentVariable("POSTGRESQL_PORT")!;
            string username = Environment.GetEnvironmentVariable("POSTGRESQL_USERNAME")!;
            string password = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")!;
            string database = Environment.GetEnvironmentVariable("POSTGRESQL_DATABASE")!;
            options.UseNpgsql($"Host={host};Port={port};Database={database};User Id={username};Password={password};");
        });
        builder.Services.AddScoped<AppDbContext>();
    }
    else
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("AppDatabase");
        });
        builder.Services.AddScoped<AppDbContext>();
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Your API V2");
    // c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    c.RoutePrefix = "api";
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.UseMiddleware<AccessTokenMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "api/v{version:apiVersion}/{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();

namespace JustLabel
{
    public class Program { }
}

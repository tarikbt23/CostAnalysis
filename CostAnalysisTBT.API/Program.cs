using Microsoft.EntityFrameworkCore;
using MediatR;
using CostAnalysisTBT.Business;
using CostAnalysisTBT.Data.EF;
using AutoMapper;
using CostAnalysisTBT.Business.Services;
using System.Text.Json;
using CostAnalysisTBT.Business.Services.RowParsing;
using CostAnalysisTBT.Business.Services.RequestHandling;
using CostAnalysisTBT.Business.Services.SchemaValidation;
using CostAnalysisTBT.Business.Services.ExcelDataOverWrite;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

// CORS ayarlarý
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DbContext
builder.Services.AddDbContext<CostAnalysisDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        b => b.MigrationsAssembly("CostAnalysisTBT.Data")
    ).UseLazyLoadingProxies()
);

// DI servisleri
builder.Services.AddAutoMapper(typeof(AssemblyReference).Assembly);
builder.Services.AddMediatR(typeof(AssemblyReference).Assembly);
builder.Services.AddScoped<InvalidRowExcelExporter>();
builder.Services.AddScoped<IRowParserService, RowParserService>();
builder.Services.AddScoped<IRequestFactory, RequestFactory>();
builder.Services.AddScoped<IExcelSchemaValidator, ExcelSchemaValidator>();
builder.Services.AddScoped<IExcelDataOverwriteService, ExcelDataOverwriteService>();

// API ayarlarý
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global exception middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        if (exception != null)
        {
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = exception.Message,
                stackTrace = exception.StackTrace
            }));
        }
    });
});

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

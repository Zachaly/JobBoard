using JobBoard.Api.Extensions;
using JobBoard.Application.Service;
using JobBoard.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharp;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using Serilog;
[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

if (Capabilities.Build.IsCoreBuild)
    GlobalFontSettings.FontResolver = new FailsafeFontResolver();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddDatabase();
builder.ConfigureSwagger();
builder.AddServices();
builder.Services.AddCors();
builder.ConfigureAuthorization();
builder.Services.Configure<FileConfiguration>(builder.Configuration.GetSection("File"));

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();
}

app.CreateDefaultAdmin();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000")
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
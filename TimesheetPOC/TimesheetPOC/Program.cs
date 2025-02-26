using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using TimesheetPOC.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext for SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:3000")  
              .AllowAnyHeader()
              .AllowAnyMethod());
});
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();
app.Run();


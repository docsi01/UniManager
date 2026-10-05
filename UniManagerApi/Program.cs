using Microsoft.EntityFrameworkCore;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UniDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository<Student>, GenericRepo<Student>>();
builder.Services.AddScoped<IRepository<Course>, GenericRepo<Course>>();
builder.Services.AddScoped<IRepository<Enrollment>, GenericRepo<Enrollment>>();

builder.Services.AddScoped<IPersonLogic<Student>,PersonLogic<Student>>();
builder.Services.AddScoped<ICourseLogic<Course>, CourseLogic<Course>>();
builder.Services.AddScoped<IEnrollmentLogic, EnrollmentLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

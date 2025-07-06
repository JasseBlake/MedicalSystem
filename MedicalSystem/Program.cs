using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<MedicalDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MedicalSystemDb;Trusted_Connection=true;MultipleActiveResultSets=true"));


builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<NotificationService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MedicalDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
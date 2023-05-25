using ProjectAttendance.CrossCutting.IoC;
using ProjectAttendance.Host.IoC;
using ProjectAttendance.Host.Seed;
using ProjectAttendance.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AnyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseApiServices();

app.MapControllers();

app.Services.CreateScope().ServiceProvider.GetRequiredService<IDatabaseSeed>().InitializeAndSeedDatabase();

app.Run();
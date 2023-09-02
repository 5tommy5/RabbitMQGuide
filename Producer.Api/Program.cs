using Producer.Api.Interfaces;
using Producer.Api.Models;
using Producer.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .Configure<RabbitConfiguration>(opt => builder.Configuration.GetSection(nameof(RabbitConfiguration)).Bind(opt));

builder.Services.AddScoped<IRabbitService, RabbitService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

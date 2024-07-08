using RabbitMQ.Client;
using UserDataHub.Consumer.Core.Interfaces;
using UserDataHub.Consumer.Core.Services;
using UserDataHub.WebAPI.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHostedService<RabbitMQHosted>();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "UserDataHub.Consumer is running");

app.Run();

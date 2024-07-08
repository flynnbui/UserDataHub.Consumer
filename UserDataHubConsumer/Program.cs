using RabbitMQ.Client;
using UserDataHub.Consumer.Core.Interfaces;
using UserDataHub.Consumer.Core.Services;
using UserDataHub.WebAPI.Settings;
using UserDataHubConsumer.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register RabbitMQ
var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
var factory = new ConnectionFactory()
{
    HostName = rabbitMQSettings.HostName,
    Port = rabbitMQSettings.Port,
    UserName = rabbitMQSettings.UserName,
    Password = rabbitMQSettings.Password,
    VirtualHost = rabbitMQSettings.VirtualHost
};

// Add services to the container.
builder.Services.AddSingleton(factory);
builder.Services.AddSingleton<IRabbitMQService>(sp => new RabbitMQService(sp.GetRequiredService<ConnectionFactory>(), "UserRegistrationQueue"));
builder.Services.AddScoped<ConsumerHostedService>();
builder.Services.AddHostedService<ConsumerHostedService>();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

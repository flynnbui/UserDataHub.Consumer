using UserDataHub.Consumer.Core.Services;
using UserDataHub.Consumer.Core.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var rabbitMQSettings = new RabbitMQSettings
{
    RouteKey = "",
    QueueList = new List<string> { "UserRegistrationQueue", "FPT.QA." } 
};

builder.Services.AddSingleton(rabbitMQSettings);
builder.Services.AddHostedService<RabbitMQHosted>();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "UserDataHub.Consumer is running");

app.Run();

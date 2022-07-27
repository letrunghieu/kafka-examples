using System.Net;
using PG.BDS.MessageBus;
using PG.BDS.MessageBus.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMessageBus(options =>
{
    options.UseKafka(kafkaOptions =>
    {
        kafkaOptions.MainConfig.Add("client.id", Dns.GetHostName());
        kafkaOptions.MainConfig.Add("bootstrap.servers", "broker:29092");
        kafkaOptions.SchemaRegistryUrl = "http://schema-registry:8081";
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

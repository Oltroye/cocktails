using System;
using Cocktails;
using Cocktails.Class;
using Cocktails.Controllers;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDB")));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cocktails API", Version = "v1" });
});
var port = "80";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cocktails API v1"));

app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

const string connectionUri = "mongodb+srv://apicocktails:gpasdidee@apicocktails.5yszwys.mongodb.net/?retryWrites=true&w=majority&appName=APIcocktails";

var settings = MongoClientSettings.FromConnectionString(connectionUri);

// Set the ServerApi field of the settings object to set the version of the Stable API on the client
settings.ServerApi = new ServerApi(ServerApiVersion.V1);

// Create a new client and connect to the server
var client = new MongoClient(settings);

// Send a ping to confirm a successful connection
try
{
    var result = client.GetDatabase("cocktails_project").GetCollection<User>("users");
    Console.WriteLine(result);
    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
}
catch (Exception ex)
{
    Console.WriteLine("Erreur connection bdd");
    Console.WriteLine(ex);
}

app.Run();

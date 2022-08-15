using System;
using Azure.Identity;
using AzureAppConfigApi;
using Microsoft.Extensions.Configuration;



var builder = WebApplication.CreateBuilder(args);

//Retrieve the Connection String from the secrets manager 
var connectionString = builder.Configuration["AppConfig:ServiceApiKey"];

builder.Host.ConfigureAppConfiguration(builder =>
{
    builder.AddAzureAppConfiguration(options =>
    {

        options.Connect(connectionString);
        //var credential = new InteractiveBrowserCredential();
        //options.Connect(new Uri(appUri), credential);
        options.ConfigureRefresh(refresh =>
        {
            refresh.Register("refreshAll", true)
            .SetCacheExpiration(TimeSpan.FromSeconds(5));
        });
    });
})
.ConfigureServices(services =>
{
    services.AddControllersWithViews();
});

// Add services to the container.
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("TestApp:Settings"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAzureAppConfiguration();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseAzureAppConfiguration();
app.Run();

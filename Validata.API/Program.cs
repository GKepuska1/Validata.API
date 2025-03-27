using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Validata.Application;
using Validata.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.AddJsonFile("appsettings.json",
                optional: true,
                reloadOnChange: false)
                .Build();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Validata.API", Version = "v1" });
});

var app = builder.Build();
app.Services.MigrateTables();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

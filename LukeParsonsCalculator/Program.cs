using Microsoft.OpenApi.Models;
using LukeParsonsCalculator.Services;
using LukeParsonsCalculator.Services.Interfaces;
using LukeParsonsCalculator.Services.Factories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddXmlSerializerFormatters()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddSingleton<IMathOperationFactory, MathOperationFactory>();
builder.Services.AddScoped<ICalculatorService, CalculatorService>();

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

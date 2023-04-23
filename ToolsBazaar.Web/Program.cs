using Microsoft.AspNetCore.Localization;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.ProductAggregate;
using ToolsBazaar.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services .AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) { }

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US")
});

app.UseRouting();

app.MapControllerRoute("default",
                       "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
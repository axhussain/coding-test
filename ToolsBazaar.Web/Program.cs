using Microsoft.AspNetCore.Localization;
using ToolsBazaar.Application.Features.Customers.QueryHandlers;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.CustomerAggregate.Queries;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Domain.ProductAggregate;
using ToolsBazaar.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(GetTopCustomersQuery).Assembly, 
        typeof(GetTopCustomersQueryHandler).Assembly);
});

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
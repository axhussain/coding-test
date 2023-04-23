using Microsoft.AspNetCore.Mvc;
using ToolsBazaar.Domain.ProductAggregate;

namespace ToolsBazaar.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase 
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    [HttpGet("most-expensive")]
    public IOrderedEnumerable<Product> GetMostExpensive()
    {
        _logger.LogInformation("Retriving the most expensive products...");

        var products = _productRepository
            .GetAll()
            .OrderByDescending(x => x.Price)
            .ThenBy(x => x.Name);

        return products;
    }
}
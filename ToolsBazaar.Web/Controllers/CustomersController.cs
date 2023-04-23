using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.CustomerAggregate.Queries;

namespace ToolsBazaar.Web.Controllers;

public record CustomerDto(string Name);

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomersController> _logger;
    private readonly ISender _mediator;

    //protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();



    public CustomersController(ILogger<CustomersController> logger, ICustomerRepository customerRepository, ISender mediator)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        _mediator = mediator;
    }

    [HttpPut("{customerId:int}")]
    public IActionResult UpdateCustomerName([FromRoute] int customerId, [FromBody] CustomerDto dto)
    {
        _logger.LogInformation($"Updating customer #{customerId} name...");

        try
        {
            _customerRepository.UpdateCustomerName(customerId, dto.Name);
            return NoContent();
        }
        catch (CustomerNotFoundException ex)
        {
            _logger.LogWarning($"Customer #{customerId} not found.");
            return NotFound(ex.Message);
        }
        
    }

    [HttpGet("top")]
    public Task<IEnumerable<Customer>> TopCustomers()
    {
        _logger.LogInformation("Retrieving the top 5 customers...");

        var dateFrom = DateTime.Parse("2015-01-01");
        var dateTo = DateTime.Parse("2022-12-31");

        return _mediator.Send(new GetTopCustomersQuery(dateFrom, dateTo));
    }
}
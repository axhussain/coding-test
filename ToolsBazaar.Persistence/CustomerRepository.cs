using ToolsBazaar.Domain.CustomerAggregate;

namespace ToolsBazaar.Persistence;

public class CustomerRepository : ICustomerRepository
{
    public IEnumerable<Customer> GetAll() => DataSet.AllCustomers;

    public void UpdateCustomerName(int customerId, string name)
    {
        var customer = 
            DataSet.AllCustomers.SingleOrDefault(c => c.Id == customerId) 
            ?? throw new CustomerNotFoundException();

        customer.UpdateName(name);
    }
}
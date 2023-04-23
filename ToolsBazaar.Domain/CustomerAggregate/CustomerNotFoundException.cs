namespace ToolsBazaar.Domain.CustomerAggregate
{
    public class CustomerNotFoundException : Exception
    {
        const string DEFAULT_MESSAGE = "Customer not found";

        public CustomerNotFoundException() : base(DEFAULT_MESSAGE)
        {
        }
    }
}

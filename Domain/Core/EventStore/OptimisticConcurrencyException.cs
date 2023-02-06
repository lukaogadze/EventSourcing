namespace Domain.Core.EventStore
{
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException(string errorMessage) : base(errorMessage)
        {
            
        }
    }
}
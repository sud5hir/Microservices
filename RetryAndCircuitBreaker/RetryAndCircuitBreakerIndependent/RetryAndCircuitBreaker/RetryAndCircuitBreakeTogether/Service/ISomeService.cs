namespace Service
{
    public interface ISomeService
    {
         Task<string> DoSomething(CancellationToken cancellationToken);
    }
}
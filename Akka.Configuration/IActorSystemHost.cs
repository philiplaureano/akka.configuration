namespace Akka.Configuration
{
    public interface IActorSystemHost
    {
        void Run(string systemName);
    }
}
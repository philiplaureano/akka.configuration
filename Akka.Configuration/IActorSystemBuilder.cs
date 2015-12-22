using Akka.Actor;

namespace Akka.Configuration
{
    public interface IActorSystemBuilder
    {
        ActorSystem Create(string systemName);
    }
}
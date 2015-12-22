using Akka.Actor;

namespace Akka.Configuration
{
    public interface IActorSystemBlockingStrategy
    {
        void AwaitTermination(ActorSystem actorSystem);
    }
}
using Akka.Actor;

namespace Akka.Configuration
{
    public interface IActorSystemInstaller
    {
        void InstallActors(ActorSystem actorSystem);
    }
}
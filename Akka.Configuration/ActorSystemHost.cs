using Akka.Actor;

namespace Akka.Configuration
{
    public class ActorSystemHost
    {
        private readonly IActorSystemBlockingStrategy _blockingStrategy;
        private readonly IActorSystemBuilder _builder;
        private readonly IActorSystemInstaller _installer;

        public ActorSystemHost(IActorSystemBuilder builder, IActorSystemInstaller installer, IActorSystemBlockingStrategy blockingStrategy)
        {
            _builder = builder;
            _installer = installer;
            _blockingStrategy = blockingStrategy;
        }

        public void Run(string systemName)
        {
            var actorSystem = _builder.Create(systemName);
            _installer.InstallActors(actorSystem);
            _blockingStrategy.AwaitTermination(actorSystem);
        }
    }
}
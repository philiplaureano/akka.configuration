using Akka.Actor;
using FakeItEasy;
using NUnit.Framework;

namespace Akka.Configuration.Tests
{
    [TestFixture]
    public class ActorSystemHostTests
    {
        [Test]
        public void Should_create_actor_system()
        {
            var blockingStrategy = A.Fake<IActorSystemBlockingStrategy>();
            var builder = A.Fake<IActorSystemBuilder>();
            var installer = A.Fake<IActorSystemInstaller>();
            
            var systemName = "FakeSystem";
            A.CallTo(() => builder.Create(systemName))
                .Returns(ActorSystem.Create(systemName));

            var host = new ActorSystemHost(builder, installer, blockingStrategy);
            host.Run(systemName);


            A.CallTo(() => blockingStrategy.AwaitTermination(A<ActorSystem>.Ignored))
                .MustHaveHappened();
            A.CallTo(() => builder.Create(systemName)).MustHaveHappened();
            A.CallTo(() => installer.InstallActors(A<ActorSystem>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void Should_create_actor_system_even_without_a_blocking_strategy()
        {
            var builder = A.Fake<IActorSystemBuilder>();
            var installer = A.Fake<IActorSystemInstaller>();

            var systemName = "FakeSystem";
            A.CallTo(() => builder.Create(systemName))
                .Returns(ActorSystem.Create(systemName));

            var host = new ActorSystemHost(builder, installer);
            host.Run(systemName);
          
            A.CallTo(() => builder.Create(systemName)).MustHaveHappened();
            A.CallTo(() => installer.InstallActors(A<ActorSystem>.Ignored)).MustHaveHappened();
        }
    }
}
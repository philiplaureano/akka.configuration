# akka.configuration
A library for [Akka.NET](http://getakka.net) that handles the boilerplate code for creating and configuring actor systems.

## Overview
Akka.Configuration is a very simple framework that provides the glue for creating and running actor systems in Akka.NET. It consists of the following interfaces:

```csharp
public interface IActorSystemBuilder
{
  ActorSystem Create(string systemName);
}
public interface IActorSystemInstaller
{
  void InstallActors(ActorSystem actorSystem);
}
public interface IActorSystemBlockingStrategy
{
  void AwaitTermination(ActorSystem actorSystem);
}
```

### IActorSystemBuilder
The *IActorSystemBuilder* interface (and its subsequent implementations) will be responsible for creating the actor systems in Akka.NET. This includes creating any HOCON configuration entries, as well as attaching those entries to whatever actor system is created.

### IActorSystemInstaller
The *IActorSystemInstaller* interface is responsible for adding your custom actors to any given actor system instance. In most cases, you'll probably want to create your own implementation of this class to ensure that you can install your actors into a particular actor system.

### IActorSystemBlockingStrategy
The *IActorSystemBlockingStrategy* interface (as its name implies) determines whether or not your actor system should be blocking your execution thread until the actor system terminates. If you're running your actor system as a console application, I recommend blocking the thread so that your application doesn't prematurely terminate, using a blocking implementation that looks like this:

```csharp
public class WaitForTermination : IActorSystemBlockingStrategy
{
    public void AwaitTermination(ActorSystem actorSystem)
    {
        actorSystem.AwaitTermination();
    }
}

```
Alternatively, if you prefer not to block the thread, you can also do the implementation like this:
```csharp
public class NonBlockingStrategy : IActorSystemBlockingStrategy
{
    public void AwaitTermination(ActorSystem actorSystem)
    {
        // Do nothing since no blocking needs to be done here
    }
}
```

## Open for Extension, but Closed to Modification
The idea behind Akka.Configuration is to create a simple framework that can be extended indefinitely without having to worrying about breaking changes caused by third-party libraries. That's why there are so few classes--this library encompasses the basic steps for setting up an Akka.NET actor system, and I'll add links to the extension libraries once they are available.

## Putting it all together
Once you have your custom implementations for all three interfaces in hand, then this is where the ActorSystemHost class can glue all of those implementations together:

```csharp
public class ActorSystemHost
{
    private readonly IActorSystemBlockingStrategy _blockingStrategy;
    private readonly IActorSystemBuilder _builder;
    private readonly IActorSystemInstaller _installer;

    public ActorSystemHost(IActorSystemBuilder builder, IActorSystemInstaller installer, 
        IActorSystemBlockingStrategy blockingStrategy)
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
```
As you can see, the implementation is very simple, and all the code you see in the *ActorSystemHost.Run* will cover 90% of the boilerplate code required to get your actor system up and running. Here's an example of how to use the *ActorSystemHost* class:

```csharp
var blockingStrategy = new WaitForTermination();
var builder = new YourBuilderImplementation();
var installer new YourActorSystemInstaller();

var host = new ActorSystemHost(builder, installer, blockingStrategy);
host.Run("MyActorSystem");
```

##Installation
You can download the latest NuGet packages for Akka.Configuration [here](https://www.nuget.org/packages/Akka.Configuration)

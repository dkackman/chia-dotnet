# chia-dotnet

Cross-platform .Net5 rpc client library for [chia](https://chia.net).

## Status

Still very much a work in progress. Websocket communcation to the `daemon` works and can communciate with other services. 

_Browse the [api documentation](http://localhost:8080/api/index.html) and [integration test code](https://github.com/dkackman/chia-dotnet/tree/main/src/chia-dotnet.tests) for more info and examples._

## Quick Start Notes:

```csharp
    using Daemon daemon = new Daemon(Config.Open().GetEndpoint("daemon"), "my-fancy-service");

    await daemon.Connect(CancellationToken.None);

    await daemon.Register(CancellationToken.None);
    var message = Message.Create("get_blockchain_state", new ExpandoObject(), ServiceNames.FullNode, daemon.ServiceName);

    var state = await daemon.SendMessage(message, CancellationToken.None);
```
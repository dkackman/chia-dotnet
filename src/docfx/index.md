# chia-dotnet

Cross-platform .Net5 rpc client library for [chia](https://chia.net).

## Quick Start Notes:

```csharp
    using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

    await daemon.Connect(CancellationToken.None);

    await daemon.Register(CancellationToken.None);
    var message = Message.Create("get_blockchain_state", new ExpandoObject(), ServiceNames.FullNode, daemon.ServiceName);

    var state = await daemon.SendMessage(message, CancellationToken.None);
```
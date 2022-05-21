# chia-dotnet

Cross-platform .Net5 [rpc client library](https://github.com/dkackman/chia-dotnet) for [chia](https://chia.net).

_Browse the [api documentation](https://dkackman.github.io/chia-dotnet/api/chia.dotnet.html) and 
[integration test code](https://github.com/dkackman/chia-dotnet/tree/main/src/chia-dotnet.tests) for more info and examples._

## Quick Start Example

```csharp
var endpoint = Config.Open().GetEndpoint("daemon");
using var rpcClient = new WebSocketRpcClient(endpoint);
await rpcClient.Connect();

var daemon = new DaemonProxy(rpcClient, "unit_tests");
await daemon.RegisterService();

var fullNode = new FullNodeProxy(rpcClient, "unit_tests");
var state = await fullNode.GetBlockchainState();
```

### Main Types and Relationships

The Wallet service API is segmented into classes for specific wallet types and a trade maanger for trades and offers.

![Class diagram](images/uml.svg "Class diagram")

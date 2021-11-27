# chia-dotnet

Cross-platform .Net5 [rpc client library](https://github.com/dkackman/chia-dotnet) for [chia](https://chia.net).

_Browse the [api documentation](https://dkackman.github.io/chia-dotnet/api/chia.dotnet.html) and 
[integration test code](https://github.com/dkackman/chia-dotnet/tree/main/src/chia-dotnet.tests) for more info and examples._

## Quick Start Notes

```csharp
var endpoint = Config.Open().GetEndpoint("daemon");
using var rpcClient = new WebSocketRpcClient(endpoint);
await rpcClient.Connect();

var daemon = new DaemonProxy(rpcClient, "unit_tests");
await daemon.RegisterService();

var fullNode = new FullNodeProxy(rpcClient, "unit_tests");
var state = await fullNode.GetBlockchainState(e);
```

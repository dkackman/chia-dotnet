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

## 1.3 breaking changes

- Changed `KValues` to `KSize` to match python naming
- Changed signature of wallet LogIn to match 1.3 changes
- Removed restore back LogIn overload to match 1.3 changes
- Introduced PrivateKeyDetails type instead of named tuple for GetPrivateKey

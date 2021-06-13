# chia-dotnet
A [.net 5](https://dotnet.microsoft.com/download/dotnet/5.0) client library for [chia](https://github.com/Chia-Network/chia-blockchain)™ RPC interfaces that runs on linux and windows.

![build](https://github.com/dkackman/chia-dotnet/actions/workflows/dotnet.yml/badge.svg)

## Getting Started

### Documentation
https://dkackman.github.io/chia-dotnet/

### Example

```csharp
using Daemon daemon = new Daemon(Config.Open().GetEndpoint("daemon"), "my-fancy-service");

await daemon.Connect(CancellationToken.None);

await daemon.Register(CancellationToken.None);
var message = Message.Create("get_blockchain_state", new ExpandoObject(), ServiceNames.FullNode, daemon.ServiceName);

var state = await daemon.SendMessage(message, CancellationToken.None);
```

### Build 

````bash
dotnet build ./src/chia-dotnet.sln
````

### Tests

Various unit and integration tests in the test project. Tests attributes with `[TestCategory("Integration")]` will use the local install of chia and the mainnet configuration to resolve RPC endpoints. 

_chia is a registered trademark or trademark of Chia Network, Inc. in the United States and worldwide._

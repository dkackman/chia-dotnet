# chia-dotnet
A [.net 5](https://dotnet.microsoft.com/download/dotnet/5.0) client library for [chia](https://github.com/Chia-Network/chia-blockchain)™ RPC interfaces that runs on linux and windows.

![build](https://github.com/dkackman/chia-dotnet/actions/workflows/dotnet.yml/badge.svg)

## Getting Started

### Documentation
https://dkackman.github.io/chia-dotnet/

### Example

```csharp
using var daemon = new Daemon(Config.Open().GetEndpoint("daemon"), "unit_tests");

await daemon.Connect(CancellationToken.None);
await daemon.Register(CancellationToken.None);

var fullNode = new FullNodeProxy(daemon);
var state = await fullNode.GetBlockchainState(CancellationToken.None);
```

### Build 

````bash
dotnet build ./src
````

### Tests

There are various unit and integration tests in the test project that have example usage. Tests attributes with `[TestCategory("Integration")]` will use the local install of chia and the mainnet configuration to resolve RPC endpoints. 

_chia is a registered trademark or trademark of Chia Network, Inc. in the United States and worldwide._

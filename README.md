# chia-dotnet

A [.net 5](https://dotnet.microsoft.com/download/dotnet/5.0) client library for [chia](https://github.com/Chia-Network/chia-blockchain)™ RPC interfaces that runs on linux and windows.

![build](https://github.com/dkackman/chia-dotnet/actions/workflows/dotnet.yml/badge.svg)

## Getting Started

### Documentation

https://dkackman.github.io/chia-dotnet/

### Service Coverage

- [x] Daemon
- [x] Full Node
- [x] Farmer
- [x] Harvester
- [ ] Wallet (in progress)
- [ ] Plotter

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

Tests decorated with `[TestCategory("CAUTION")]` update the state of whatever they are interacting with. Run these tests with caution. If run against a production node on `mainnet`, they might change things, up to and including deleting keys or sending chia.

### Some Notes About Types and Naming

In addition to static vs dynamic typing, C# and Python have very different conventions for naming and formatting. For the most part I've tried to make this library fit into dotnet conventions.

- Method and property names are `ProperCased`.
- Parameter names are `camelCased`.
- The chia RPC uses unsigned integers where dotnet might use signed. In cases where chia expects an unsigned number, it is unsigned on the dotnet side.
- [`BigInteger`](https://docs.microsoft.com/en-us/dotnet/api/system.numerics.biginteger?view=net-5.0) is used for the python unsigned 64 bit int.
- Where the RPC return a scalar value, the dotnet code will as well.
- Where the RPC returns a list of named scalar values, they are returned as a Tuple with named fields.
- Complex types and structs are currently returned as a `dynamic` [`ExpandoObject`](https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.expandoobject?view=net-5.0). This may change in the future.
- Lists of things are returned as [`IEnumberable<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=net-5.0).
- Where the python code returns a differently shaped object based on its input or logic, the dotnet code is turned into multiple methods.
- When the RPC returns a success flag equal to `false`, the dotnet code throws an exception.

___

_chia and its logo are the registered trademark or trademark of Chia Network, Inc. in the United States and worldwide._

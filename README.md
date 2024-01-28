# chia-dotnet

A [.net](https://dotnet.microsoft.com/download/dotnet/6.0) client library for [chia](https://github.com/Chia-Network/chia-blockchain)™ RPC interfaces that runs on linux and windows, including a bech32 implementation.

[![build](https://github.com/dkackman/chia-dotnet/actions/workflows/dotnet.yml/badge.svg)](https://github.com/dkackman/chia-dotnet/actions)
[![CodeQL](https://github.com/dkackman/chia-dotnet/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/dkackman/chia-dotnet/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/dt/chia-dotnet)](https://www.nuget.org/packages/chia-dotnet/)

## Getting Started

### See Also

- [Documentation](https://dkackman.github.io/chia-dotnet/)
- [chia-dotnet-bls](https://www.nuget.org/packages/chia-dotnet-bls/)
- [chia-blockchain](https://chia.net)

### Features

- Coverage of all of chia's rpc endpoints
  - Daemon, Full Node, Farmer, Harvester, Wallet, Plotter, Crawler, DataLayer
- Coverage of all of the methods at each endpoint
  - as of 2.1.4
- Static types for chia input and outputs
- Supports connecting via the `daemon` on `wss` or directly to each service with `https`
  - both `https` and `wss` use tha same interfaces so switching is seamless
  
### Examples

_Test carefully and in one of the testnets!_

#### Example app

Try the [example code](https://github.com/dkackman/chia-dotnet/tree/main/Examples/crops) or take a look at [`rchia` remote chia management CLI](https://github.com/dkackman/rchia).

#### Connect to the Node and find out about the blockchain

```csharp
var endpoint = Config.Open().GetEndpoint("daemon");
using var rpcClient = new WebSocketRpcClient(endpoint);
await rpcClient.Connect();

var daemon = new DaemonProxy(rpcClient, "unit_tests");
await daemon.RegisterService();

var fullNode = new FullNodeProxy(rpcClient, "unit_tests");
var state = await fullNode.GetBlockchainState();
Console.WriteLine($"This node is synced: {state.Sync.Synced}");
```

#### Send me some chia

```csharp
var endpoint = Config.Open().GetEndpoint("wallet");
using var rpcClient = new HttpRpcClient(endpoint);

var wallet = new WalletProxy(rpcClient, "unit_tests");
await wallet.WaitForSync();

// walletId of 1 is the main XCH wallet
var standardWallet = new Wallet(1, wallet);

// this is my receive address. feel free to run this code on mainnet as often as you like :-)
var transaction = await standardWallet.SendTransaction("xch1ls2w9l2tksmp8u3a8xewhn86na3fjhxq79gnsccxr0v3rpa5ejcsuugha7", 1, 1);
```

### Listen for events

```csharp
using chia.dotnet;

var endpoint = Config.Open().GetEndpoint("daemon");
using var rpcClient = new WebSocketRpcClient(endpoint);
await rpcClient.Connect();

var daemon = new DaemonProxy(rpcClient, "eventing_testharness");
// this listens for the messages sent to the ui
await daemon.RegisterService("wallet_ui"); 
daemon.StateChanged += (sender, data) => Console.WriteLine($"daemon state change: {data}");

var farmer = daemon.CreateProxyFrom<FarmerProxy>();
farmer.ConnectionAdded += (sender, data) => Console.WriteLine($"Connection added: {data}");
farmer.NewFarmingInfo += (sender, data) => Console.WriteLine($"Farming info: {data}");
farmer.NewSignagePoint += (sender, data) => Console.WriteLine($"Signage point: {data}");

while (true)
{
    await Task.Delay(100);
}

```

### Build

````bash
dotnet build ./src
````

### Install from nuget.org

````bash
dotnet add package chia-dotnet
````

### Tests

There are various unit and integration tests in the test project that have example usage. Some tests might be set to skip because they either need input data specific to a wallet or they may be destructive.

### Some Notes About Types and Naming

In addition to static vs dynamic typing, C# and Python have very different conventions for naming and formatting. For the most part I've tried to make this library fit into dotnet conventions. Please open an issue if something doesn't feel `dotnet-y`.

- Method and property names are `ProperCased`.
- Parameter names are `camelCased`.
- The chia RPC uses unsigned integers where dotnet might use signed. In cases where chia expects an unsigned number, it is unsigned on the dotnet side.
- `ulong` is used for the python 64 bit unsigned int.
- `System.Numerics.BigInteger` is used for the python 128 bit unsigned int.
- Where the RPC return a scalar value, the dotnet code will as well. If it is optional in python it will be `Nullable<T>` in dotnet
- Where the RPC returns a list of named scalar values, they are returned as a Tuple with named fields.
- Lists of things are returned as [`IEnumberable<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=net-5.0).
- Where the python code returns a differently shaped object based on its input or logic, the dotnet code is turned into multiple methods.
- When the RPC returns a success flag equal to `false`, the dotnet code throws an exception.

___

_chia and its logo are the registered trademark or trademark of Chia Network, Inc. in the United States and worldwide._

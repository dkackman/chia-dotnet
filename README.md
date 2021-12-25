# chia-dotnet

A [.net 5](https://dotnet.microsoft.com/download/dotnet/5.0) client library for [chia](https://github.com/Chia-Network/chia-blockchain)™ RPC interfaces that runs on linux and windows.

[![build](https://github.com/dkackman/chia-dotnet/actions/workflows/dotnet.yml/badge.svg)](https://github.com/dkackman/chia-dotnet/actions)
[![CodeQL](https://github.com/dkackman/chia-dotnet/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/dkackman/chia-dotnet/actions/workflows/codeql-analysis.yml)
[![NuGet](https://img.shields.io/nuget/dt/chia-dotnet)](https://www.nuget.org/packages/chia-dotnet/)

## Getting Started

### Documentation

https://dkackman.github.io/chia-dotnet/

### Features

- Coverage of all of chia's rpc endpoints
  - Daemon, Full Node, Farmer, Harvester Wallet, Plotter
- Coverage of all of the methods at each endpoint
  - as of 1.2.11 (if you find something missing please create an issue)
- Static types for chia input and outputs
- Supports connecting via the `daemon` on `wss` or directly to each service with `https`
  - both `https` and `wss` use tha same interfaces so switching is seemless
  
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
var state = await fullNode.GetBlockchainState(e);
Console.Log($"This node is synced: {state.Sync.Synced}")
```

#### Send me some chia

```csharp
var endpoint = Config.Open().GetEndpoint("wallet");
using var rpcClient = new HttpRpcClient(endpoint);

// walletId of 1 is the main wallet
var wallet = new Wallet(1, new WalletProxy(rpcClient, "unit_tests"));
_ = await wallet.Login();

// this is my receive address. feel free to run this code on mainnet as often as you like :-)
var transaction = await wallet.SendTransaction("xch1ls2w9l2tksmp8u3a8xewhn86na3fjhxq79gnsccxr0v3rpa5ejcsuugha7", 1, 1);
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

There are various unit and integration tests in the test project that have example usage. Tests attributes with `[TestCategory("Integration")]` will use the local install of chia and the mainnet configuration to resolve RPC endpoints.

Tests decorated with `[TestCategory("CAUTION")]` update the state of whatever they are interacting with. Run these tests with caution. If run against a production node on `mainnet`, they might change things, up to and including deleting keys or sending chia out of your wallet.

### Some Notes About Types and Naming

In addition to static vs dynamic typing, C# and Python have very different conventions for naming and formatting. For the most part I've tried to make this library fit into dotnet conventions. Please open an issue if something doesn't feel `dotnet-y`.

- Method and property names are `ProperCased`.
- Parameter names are `camelCased`.
- The chia RPC uses unsigned integers where dotnet might use signed. In cases where chia expects an unsigned number, it is unsigned on the dotnet side.
- `ulong` is used for the python 64 bit unsigned int.
- `BigInteger` is used for the python 128 bit unsigned int.
- Where the RPC return a scalar value, the dotnet code will as well. If it is optional in python it will be `Nullable<T>` in dotnet
- Where the RPC returns a list of named scalar values, they are returned as a Tuple with named fields.
- Lists of things are returned as [`IEnumberable<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=net-5.0).
- Where the python code returns a differently shaped object based on its input or logic, the dotnet code is turned into multiple methods.
- When the RPC returns a success flag equal to `false`, the dotnet code throws an exception.

___

_chia and its logo are the registered trademark or trademark of Chia Network, Inc. in the United States and worldwide._

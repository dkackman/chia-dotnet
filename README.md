# chia-dotnet
A [.net 5](https://dotnet.microsoft.com/download/dotnet/5.0) client library for [chia](https://github.com/Chia-Network/chia-blockchain) RPC interfaces.

## Build 

````bash
dotnet build ./src/chia-dotnet.tests/chia-dotnet.tests.csproj
````

## Tests

Various unit and integration tests in the test project. Tests attributes with `[TestCategory("Integration")]` will use the local install of chia and the mainnet configuration to resolve RPC endpoints. 

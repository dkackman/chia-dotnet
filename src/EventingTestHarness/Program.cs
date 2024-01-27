using chia.dotnet;

var endpoint = Config.Open().GetEndpoint("daemon");
using var rpcClient = new WebSocketRpcClient(endpoint);
await rpcClient.Connect();

var daemon = new DaemonProxy(rpcClient, "eventing_testharness");
await daemon.RegisterService("wallet_ui"); // this listens for the messages sent to the ui

var farmer = daemon.CreateProxyFrom<FarmerProxy>();
farmer.ConnectionsChanged += (sender, data) => Console.WriteLine($"Connections count: {data.Count()}");
farmer.NewFarmingInfo += (sender, data) => Console.WriteLine($"Farming info: {data}");
farmer.NewSignagePoint += (sender, data) => Console.WriteLine($"Signage Point: {data}");

while (true)
{
    await Task.Delay(100);
}

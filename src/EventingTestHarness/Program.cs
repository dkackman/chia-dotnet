using chia.dotnet;

var endpoint = Config.Open().GetEndpoint("daemon");
using var rpcClient = new WebSocketRpcClient(endpoint);
await rpcClient.Connect();

var daemon = new DaemonProxy(rpcClient, "eventing_testharness");
await daemon.RegisterService("wallet_ui"); // this listens for the messages sent to the ui
daemon.StateChanged += (sender, data) => Console.WriteLine($"daemon state change: {data}");

var farmer = daemon.CreateProxyFrom<FarmerProxy>();
farmer.ConnectionAdded += (sender, data) => Console.WriteLine($"Connection added: {data}");
farmer.NewFarmingInfo += (sender, data) => Console.WriteLine($"Farming info: {data}");
farmer.NewSignagePoint += (sender, data) => Console.WriteLine($"Signage point: {data}");

while (true)
{
    await Task.Delay(100);
}

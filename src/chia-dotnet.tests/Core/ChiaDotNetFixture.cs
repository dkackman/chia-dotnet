using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace chia.dotnet.tests.Core;

public class ChiaDotNetFixture : IDisposable
{
    public IHost TestHost { get; }
    internal CancellationTokenSource _cts;
    private string OriginService => "unit_tests";

    public ChiaDotNetFixture()
    {
        try
        {
            _cts = new CancellationTokenSource(55000);
            TestHost = CreateHostBuilder()!.Build();
            TestHost.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            Environment.Exit(1);
        }
    }

    public IHostBuilder? CreateHostBuilder()
    {
        return new HostBuilder().ConfigureWebHost(webhost =>
        {
            _ = webhost.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                _ = configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("testingappsettings.json", false)
                    .AddEnvironmentVariables("PREFIX_")
                    .AddUserSecrets<ChiaDotNetFixture>(true);
            }).ConfigureServices(async (hostContext, services) =>
            {
                try
                {
                    //appsettings mode is websockets
                    if (hostContext.Configuration["mode"] == "0")
                    {
                        //bind settings from secrets/environment/appsettings
                        var daemonConfig = new Endpoint();
                        var plotterConfig = new Endpoint();

                        hostContext.Configuration.GetSection("daemon").Bind(daemonConfig);
                        hostContext.Configuration.GetSection("plotter").Bind(plotterConfig);

                        // Get all endpoints
                        var daemonEndpointInfo = GetEndpointInfo(daemonConfig);
                        var plotterEndpointInfo = GetEndpointInfo(plotterConfig);
                        var wssClient = new WebSocketRpcClient(daemonEndpointInfo);
                        var cts = new CancellationTokenSource(120000);

                        //connect wss client
                        wssClient.Connect(cts.Token).ConfigureAwait(false).GetAwaiter().GetResult();
                        var daemon = new DaemonProxy(wssClient, OriginService);
                        daemon.RegisterService(cts.Token).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                        //proxies
                        var nodeRpcClient = new FullNodeProxy(wssClient, OriginService);
                        var farmerRpcProxy = new FarmerProxy(wssClient, OriginService);
                        var walletRpcProxy = new WalletProxy(wssClient, OriginService);
                        var harvesterRpcProxy = new HarvesterProxy(wssClient, OriginService);
                        var crawlerRpcProxy = new CrawlerProxy(wssClient, OriginService);
                        var plotterRpcProxy = new PlotterProxy(wssClient, OriginService);

                        var (Wallets, Fingerprint) = walletRpcProxy.GetWallets(false, cts.Token).ConfigureAwait(false).GetAwaiter().GetResult();
                        var walletFactory = new WalletFactory(walletRpcProxy, Wallets, Fingerprint);

                        //register test dependencies 
                        _ = services.AddSingleton(daemon)
                            .AddSingleton(nodeRpcClient)
                            .AddSingleton(wssClient)
                            .AddSingleton(farmerRpcProxy)
                            .AddSingleton(walletRpcProxy)
                            .AddSingleton(harvesterRpcProxy)
                            .AddSingleton(crawlerRpcProxy)
                            .AddSingleton(plotterRpcProxy)
                            .AddSingleton(walletFactory)
                            .AddSingleton(new Wallet(1, walletRpcProxy))
                            .AddSingleton(new VerifiedCredentialManager(walletRpcProxy))
                            .AddSingleton(new TradeManager(walletRpcProxy));
                    }
                    else
                    {
                        // Daemon and Plotter proxies require wss so no http version
                        var fullNodeConfig = new Endpoint();
                        var farmerConfig = new Endpoint();
                        var walletConfig = new Endpoint();
                        var harvesterConfig = new Endpoint();
                        var crawlerConfig = new Endpoint();

                        hostContext.Configuration.GetSection("fullnode").Bind(fullNodeConfig);
                        hostContext.Configuration.GetSection("farmer").Bind(farmerConfig);
                        hostContext.Configuration.GetSection("harvester").Bind(harvesterConfig);
                        hostContext.Configuration.GetSection("wallet").Bind(walletConfig);
                        hostContext.Configuration.GetSection("crawler").Bind(crawlerConfig);

                        // Get all endpoints
                        var fullNodeEndpointInfo = GetEndpointInfo(fullNodeConfig);
                        var farmerEndpointInfo = GetEndpointInfo(farmerConfig);
                        var walletEndpointInfo = GetEndpointInfo(walletConfig);
                        var harvesterEndpointInfo = GetEndpointInfo(harvesterConfig);
                        var crawlerEndpointInfo = GetEndpointInfo(crawlerConfig);

                        //appsettings mode is httpsClient
                        var cts = new CancellationTokenSource(120000);
                        var nodeHttpClient = new HttpRpcClient(fullNodeEndpointInfo);
                        var farmerHttpClient = new HttpRpcClient(farmerEndpointInfo);
                        var walletHttpClient = new HttpRpcClient(walletEndpointInfo);
                        var harvesterHttpClient = new HttpRpcClient(harvesterEndpointInfo);
                        var crawlerHttpClient = new HttpRpcClient(crawlerEndpointInfo);

                        //Proxies
                        var nodeRpcClient = new FullNodeProxy(nodeHttpClient, OriginService);
                        var farmerRpcProxy = new FarmerProxy(farmerHttpClient, OriginService);
                        var walletRpcProxy = new WalletProxy(walletHttpClient, OriginService);
                        var harvesterRpcProxy = new HarvesterProxy(harvesterHttpClient, OriginService);
                        var crawlerRpcProxy = new HarvesterProxy(crawlerHttpClient, OriginService);

                        var (Wallets, Fingerprint) = walletRpcProxy.GetWallets(false, cts.Token).ConfigureAwait(false).GetAwaiter().GetResult();
                        var walletFactory = new WalletFactory(walletRpcProxy, Wallets, Fingerprint);

                        //register test dependencies 
                        _ = services.AddSingleton(nodeRpcClient)
                            .AddSingleton(farmerRpcProxy)
                            .AddSingleton(walletRpcProxy)
                            .AddSingleton(harvesterRpcProxy)
                            .AddSingleton(crawlerRpcProxy)
                            .AddSingleton(walletFactory)
                            .AddSingleton(new Wallet(1, walletRpcProxy))
                            .AddSingleton(new VerifiedCredentialManager(walletRpcProxy))
                            .AddSingleton(new TradeManager(walletRpcProxy));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Environment.Exit(1);
                }
            });

            _ = webhost.UseTestServer().Configure(app =>
            {
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("TestHost HttpServer Started").ConfigureAwait(false);
                });
            });
        });


    }

    private static EndpointInfo GetEndpointInfo(Endpoint ep)
    {
        return new EndpointInfo() { KeyPath = ep.KeyPath, CertPath = ep.CertPath, Uri = new Uri(ep.Uri) };
    }

    public void Dispose()
    {
        TestHost?.Dispose();
    }
}

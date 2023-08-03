using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace chia.dotnet.tests.Core;

public class ChiaDotNetFixture : IDisposable
{
    public IHost TestHost { get; }
    private CancellationTokenSource _cts;

    public ChiaDotNetFixture()
    {
        try
        {
            _cts = new CancellationTokenSource(5000);
            TestHost = CreateHostBuilder().Build();
            TestHost.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    public IHostBuilder? CreateHostBuilder()
    {
        return new HostBuilder().ConfigureWebHost(webhost =>
        {
            webhost.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configurationBuilder.AddJsonFile("testingappsettings.json", false);
                configurationBuilder.AddEnvironmentVariables("PREFIX_");
                configurationBuilder.AddUserSecrets<ChiaDotNetFixture>(true);
            });

            webhost.ConfigureServices((hostContext, services) =>
            {
                //register services e.g. websockets clients/httpclients
                
            });
        });
    }
    
    public void Dispose()
    {
        TestHost?.Dispose();
    }
}

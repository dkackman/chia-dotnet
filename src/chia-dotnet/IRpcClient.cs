using System;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    public interface IRpcClient
    {
        EndpointInfo Endpoint { get; init; }

        event EventHandler<Message> BroadcastMessageReceived;

        Task Close(CancellationToken cancellationToken = default);

        Task Connect(CancellationToken cancellationToken = default);

        Task PostMessage(string command, dynamic data, CancellationToken cancellationToken = default);

        Task<Message> SendMessage(string command, dynamic data, CancellationToken cancellationToken = default);
    }
}

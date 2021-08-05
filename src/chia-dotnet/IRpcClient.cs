using System;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    public interface IRpcClient : IDisposable
    {
        EndpointInfo Endpoint { get; init; }

        event EventHandler<Message> BroadcastMessageReceived;

        Task Close(CancellationToken cancellationToken = default);

        Task Connect(CancellationToken cancellationToken = default);

        Task PostMessage(Message message, CancellationToken cancellationToken = default);

        Task<dynamic> SendMessage(Message message, CancellationToken cancellationToken = default);
    }
}

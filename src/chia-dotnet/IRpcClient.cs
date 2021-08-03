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

        Task PostMessage(Message message, CancellationToken cancellationToken = default);

        Task<Message> SendMessage(Message message, CancellationToken cancellationToken = default);
    }
}

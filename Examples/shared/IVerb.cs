using System.Threading.Tasks;

namespace chia.dotnet.console
{
    interface IVerb
    {
        Task<int> Run();
    }
}

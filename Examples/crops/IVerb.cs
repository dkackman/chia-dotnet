using System.Threading.Tasks;

namespace crops
{
    interface IVerb
    {
        Task<int> Run();
    }
}

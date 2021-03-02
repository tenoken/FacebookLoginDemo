using System.Threading.Tasks;

namespace LoginDemo.Servcices.Interfaces
{
    public interface IRequest
    {
        Task<string> GetHomePage(string user, string password);
    }
}

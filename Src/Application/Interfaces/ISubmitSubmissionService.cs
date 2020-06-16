using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubmitSubmissionService
    {
        Task SubmitAsync(PolApiContext polApiContext);
    }
}
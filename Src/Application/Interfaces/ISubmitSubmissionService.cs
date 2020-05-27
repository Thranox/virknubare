using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubmitSubmissionService
    {
        Task<object> SubmitAsync(PolApiContext polApiContext);
    }
}
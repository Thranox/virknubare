using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IInvitationService
    {
        Task<int> AcceptWaitingInvitations(PolApiContext polApiContext);
    }
}
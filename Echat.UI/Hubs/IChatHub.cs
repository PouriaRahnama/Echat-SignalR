
namespace Echat.UI.Hubs
{
    public interface IChatHub
    {
        Task JoinGroup(string token, long currentGroupId);
        Task JoinPrivateGroup(long receiverId, long currentGroupId);
    }
}
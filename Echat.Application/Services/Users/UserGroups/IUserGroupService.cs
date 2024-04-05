using Echat.Application.ViewModels.Chats;

namespace Echat.Application.Services.Users.UserGroups
{
    public interface IUserGroupService
    {
        Task<List<UserGroupViewModel>> GetUserGroups(long userId);
        Task<List<string>> GetUserIds(long groupId);
        Task JoinGroup(long userId, long groupId);
        Task JoinGroup(List<long> userIds, long groupId);
        Task<bool> IsUserInGroup(long userId, long groupId);
        Task<bool> IsUserInGroup(long userId, string token);
    }
}
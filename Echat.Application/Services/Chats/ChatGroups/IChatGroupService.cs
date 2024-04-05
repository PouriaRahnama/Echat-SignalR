using Echat.Application.ViewModels.Chats;

namespace Echat.Application.Services.Chats.ChatGroups
{
    public interface IChatGroupService
    {
        Task<List<SearchResultViewModel>> Search(string title, long userId);
        Task<List<ChatGroupViewModel>> GetUserGroups(long userId);
        Task<ChatGroupViewModel> InsertGroup(CreateGroupViewModel model);
        Task<ChatGroupViewModel> InsertPrivateGroup(long userId, long receiverId);
        Task<ChatGroupViewModel> GetGroupBy(long id);
        Task<ChatGroupViewModel> GetGroupBy(string token);

    }
}
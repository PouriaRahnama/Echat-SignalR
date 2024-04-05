using Echat.Application.ViewModels.Chats;

namespace Echat.Application.Services.Chats
{
    public interface IChatService
    {
        Task<ChatViewModel> SendMessage(InsertChatVIewModel chat);
        Task<List<ChatViewModel>> GetChatGroup(long groupId);
    }
}
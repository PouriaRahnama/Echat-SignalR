using Microsoft.AspNetCore.Http;

namespace Echat.Application.ViewModels.Chats
{
    public class InsertChatVIewModel
    {
        public string ChatBody { get; set; }
        public long GroupId { get; set; }
        public long UserId { get; set; }
        public IFormFile FileAttach { get; set; }
    }
}
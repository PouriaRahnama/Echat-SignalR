using System;

namespace Echat.Application.ViewModels.Chats
{
    public class ChatViewModel
    {
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public string ChatBody { get; set; }
        public string? FileAttach { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public string CreateDate { get; set; }
    }
}
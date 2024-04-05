using Echat.Domain.Entities.Chats;

namespace Echat.Application.ViewModels.Chats
{
    public class UserGroupViewModel
    {
        public long groupId { get; set; }
        public string GroupName { get; set; }
        public string Token { get; set; }
        public string ImageName { get; set; }
        public Chat LastChat { get; set; }
    }
}
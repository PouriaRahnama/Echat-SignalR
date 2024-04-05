namespace Echat.Application.ViewModels.Chats
{
    public class ChatGroupViewModel
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string GroupTitle { get; set; }
        public string GroupToken { get; set; }
        public string ImageName { get; set; }
        public long OwnerId { get; set; }
        public long? ReceiverId { get; set; }
        public bool IsPrivate { get; set; }

        public UserViewModel? User { get; set; }
        public UserViewModel? Receiver { get; set; }
        public ICollection<ChatViewModel>? Chats { get; set; }
        public ICollection<UserGroupModel>? UserGroups { get; set; }
    }

    public class UserGroupModel
    {
        public long UserId { get; set; }
        public long GroupId { get; set; }
    }

    public class UserViewModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
    }
}

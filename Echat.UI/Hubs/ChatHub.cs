
using Echat.Application.Services.Chats;
using Echat.Application.Services.Chats.ChatGroups;
using Echat.Application.Services.Users.UserGroups;
using Echat.Application.Utilities;
using Echat.Application.ViewModels.Chats;
using Microsoft.AspNetCore.SignalR;

namespace Echat.UI.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private IChatGroupService _groupService;
        private IUserGroupService _userGroup;
        private IChatService _chatService;

        public ChatHub(IChatGroupService groupService, IUserGroupService userGroup, IChatService chatService)
        {
            _groupService = groupService;
            _userGroup = userGroup;
            _chatService = chatService;
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Welcome", Context.User.GetUserId());
            return base.OnConnectedAsync();
        }

        public async Task JoinGroup(string token, long currentGroupId)
        {
            var group = await _groupService.GetGroupBy(token);
            var groupDto = FixGroupModel(group);
            if (group == null)
                await Clients.Caller.SendAsync("Error", "Group Not Found");
            else
            {
                var chats = await _chatService.GetChatGroup(group.Id);
                if (!await _userGroup.IsUserInGroup(Context.User.GetUserId(), token))
                {
                    await _userGroup.JoinGroup(Context.User.GetUserId(), group.Id);
                    await Clients.Caller.SendAsync("NewGroup", groupDto.GroupTitle, groupDto.GroupToken, groupDto.ImageName);
                }
                if (currentGroupId > 0)
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());

                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Caller.SendAsync("JoinGroup", groupDto, chats);
            }
        }

        public async Task JoinPrivateGroup(long receiverId, long currentGroupId)
        {
            if (currentGroupId > 0)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());

            var group = await _groupService.InsertPrivateGroup(Context.User.GetUserId(), receiverId);
            var groupDto = FixGroupModel(group);

            if (!await _userGroup.IsUserInGroup(Context.User.GetUserId(), group.GroupToken))
            {
                await _userGroup.JoinGroup(new List<long>()
                    { groupDto.ReceiverId ?? 0, group.OwnerId }, group.Id);

                await Clients.Caller.SendAsync("NewGroup", groupDto.GroupTitle, groupDto.GroupToken, groupDto.ImageName);
                await Clients.User(groupDto.ReceiverId.ToString()).SendAsync("NewGroup", Context.User.GetUserName(), groupDto.GroupToken, groupDto.ImageName);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());

            var chats = await _chatService.GetChatGroup(group.Id);

            await Clients.Caller.SendAsync("JoinGroup", groupDto, chats);
        }


        #region Utils

        private ChatGroupViewModel FixGroupModel(ChatGroupViewModel chatGroup)
        {
            if (chatGroup.IsPrivate)
            {
                if (chatGroup.OwnerId == Context.User.GetUserId())
                {
                    return new ChatGroupViewModel()
                    {
                        Id = chatGroup.Id,
                        GroupToken = chatGroup.GroupToken,
                        CreateDate = chatGroup.CreateDate,
                        GroupTitle = chatGroup.Receiver.UserName,
                        ImageName = chatGroup.Receiver.Avatar,
                        IsPrivate = false,
                        OwnerId = chatGroup.OwnerId,
                        ReceiverId = chatGroup.ReceiverId
                    };
                }
                return new ChatGroupViewModel()
                {
                    Id = chatGroup.Id,
                    GroupToken = chatGroup.GroupToken,
                    CreateDate = chatGroup.CreateDate,
                    GroupTitle = chatGroup.User.UserName,
                    ImageName = chatGroup.User.Avatar,
                    IsPrivate = false,
                    OwnerId = chatGroup.OwnerId,
                    ReceiverId = chatGroup.ReceiverId
                };
            }
            return new ChatGroupViewModel()
            {
                Id = chatGroup.Id,
                GroupToken = chatGroup.GroupToken,
                CreateDate = chatGroup.CreateDate,
                GroupTitle = chatGroup.GroupTitle,
                ImageName = chatGroup.ImageName,
                IsPrivate = false,
                OwnerId = chatGroup.OwnerId,
                ReceiverId = chatGroup.ReceiverId
            };
        }

        #endregion
    }
}

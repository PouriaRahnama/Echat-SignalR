using Echat.Application.ViewModels.Chats;
using Echat.Domain.Entities.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echat.Application.Mapper
{
    public static class ChatGroupMapper
    {
        public static ChatGroupViewModel GroupMapToViewModel(ChatGroup group)
        {
            if (group == null)
                return new ChatGroupViewModel();

            return new ChatGroupViewModel()
            {
                GroupTitle = group.GroupTitle,
                GroupToken = group.GroupToken,
                ImageName = group.ImageName,
                IsPrivate = group.IsPrivate,
                OwnerId = group.OwnerId,
                ReceiverId = group.ReceiverId,
                Id = group.Id,
                CreateDate = group.CreateDate,
                User = group.User != null ? new UserViewModel()
                {
                    Avatar = group.User.Avatar,
                    Password = group.User.Password,
                    UserName = group.User.UserName
                } : new UserViewModel(),
                Receiver = group.Receiver != null ? new UserViewModel()
                {
                    Avatar = group.Receiver.Avatar,
                    Password = group.Receiver.Password,
                    UserName = group.Receiver.UserName
                } : new UserViewModel(),
                Chats = group.Chats != null ? group.Chats.Select(c => new ChatViewModel()
                {
                    ChatBody = c.ChatBody,
                    CreateDate = $"{c.CreateDate.Hour}:{c.CreateDate.Minute}",
                    FileAttach = c.FileAttach,
                    UserId = c.UserId,
                    GroupId = c.GroupId
                }).ToList() : new List<ChatViewModel>()
            };

        }
    }
}

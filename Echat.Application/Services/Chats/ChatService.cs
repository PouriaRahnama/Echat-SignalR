using Echat.Application.Utilities;
using Echat.Application.ViewModels.Chats;
using Echat.Domain.Context;
using Echat.Domain.Entities.Chats;
using Microsoft.EntityFrameworkCore;

namespace Echat.Application.Services.Chats
{
    public class ChatService : BaseService, IChatService
    {
        public ChatService(EchatContext context) : base(context)
        {
        }

        public async Task<ChatViewModel> SendMessage(InsertChatVIewModel chat)
        {
            var group = await GetById<ChatGroup>(chat.GroupId);
            var chatModel = new Chat()
            {
                CreateDate = DateTime.Now,
                GroupId = chat.GroupId,
                UserId = chat.UserId
            };
            if (chat.FileAttach != null)
            {
                var fileName = await chat.FileAttach.SaveFile("wwwroot/files/");
                chatModel.ChatBody = chat.FileAttach.FileName;
                chatModel.FileAttach = fileName;
                Insert(chatModel);
                await Save();
                return new ChatViewModel()
                {
                    UserName = " ",
                    CreateDate = $"{chatModel.CreateDate.Hour}:{chatModel.CreateDate.Minute}",
                    ChatBody = chatModel.ChatBody,
                    GroupName = group.GroupTitle,
                    GroupId = group.Id,
                    UserId = chat.UserId,
                    FileAttach = fileName
                };
            }
            chatModel.ChatBody = chat.ChatBody;
            Insert(chatModel);
            await Save();
            return new ChatViewModel()
            {
                UserName = " ",
                CreateDate = $"{chatModel.CreateDate.Hour}:{chatModel.CreateDate.Minute}",
                ChatBody = chatModel.ChatBody,
                GroupName = group.GroupTitle,
                GroupId = group.Id,
                UserId = chat.UserId
            };
        }

        public async Task<List<ChatViewModel>> GetChatGroup(long groupId)
        {
            var res=  await Table<Chat>().Include(s => s.User).Include(c => c.CharGroup)
                .Where(g => g.GroupId == groupId).ToListAsync();
            List<ChatViewModel> chatvm = new();

            res.ForEach(s => chatvm.Add(new ChatViewModel()
            {
                UserName = s.User.UserName,
                CreateDate = $"{s.CreateDate.Hour}:{s.CreateDate.Minute}",
                ChatBody = s.ChatBody,
                GroupName = s.CharGroup.GroupTitle,
                UserId = s.UserId,
                FileAttach = s.FileAttach,
                GroupId = s.GroupId
            }));

            return chatvm;  

        }
    }
}
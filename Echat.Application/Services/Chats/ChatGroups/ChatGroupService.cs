using Echat.Application.Mapper;
using Echat.Application.Services.Users.UserGroups;
using Echat.Application.Utilities;
using Echat.Application.ViewModels.Chats;
using Echat.Domain.Context;
using Echat.Domain.Entities.Chats;
using Echat.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Echat.Application.Services.Chats.ChatGroups
{
    public class ChatGroupService : BaseService, IChatGroupService
    {
        private IUserGroupService _Usergroup;

        public ChatGroupService(EchatContext context, IUserGroupService usergroup) : base(context)
        {
            _Usergroup = usergroup;
        }

        public async Task<List<SearchResultViewModel>> Search(string title, long userId)
        {
            var result = new List<SearchResultViewModel>();
            if (string.IsNullOrWhiteSpace(title))
                return result;

            var groups = await Table<ChatGroup>()
                .Where(g => g.GroupTitle.Contains(title) && !g.IsPrivate)
                .Select(s => new SearchResultViewModel()
                {
                    ImageName = s.ImageName,
                    Token = s.GroupToken,
                    IsUser = false,
                    Title = s.GroupTitle
                }).ToListAsync();

            var users = await Table<User>()
                .Where(g => g.UserName.Contains(title) && g.Id != userId)
                .Select(s => new SearchResultViewModel()
                {
                    ImageName = s.Avatar,
                    Token = s.Id.ToString(),
                    IsUser = true,
                    Title = s.UserName
                }).ToListAsync();
            result.AddRange(groups);
            result.AddRange(users);
            return result;
        }

        public async Task<List<ChatGroupViewModel>> GetUserGroups(long userId)
        {
            return await Table<ChatGroup>()
                .Include(c => c.Chats)
                .Where(g => g.OwnerId == userId)
                .OrderByDescending(d => d.CreateDate).Select(s => ChatGroupMapper.GroupMapToViewModel(s)).ToListAsync();
        }

        public async Task<ChatGroupViewModel> InsertGroup(CreateGroupViewModel model)
        {
            if (model.ImageFile == null || !FileValidation.IsValidImageFile(model.ImageFile.FileName))
                throw new Exception();


            var imageName = await model.ImageFile.SaveFile("wwwroot/image/groups");

            var chatGroup = new ChatGroup()
            {
                CreateDate = DateTime.Now,
                GroupTitle = model.GroupName,
                OwnerId = model.UserId,
                GroupToken = Guid.NewGuid().ToString(),
                ImageName = imageName
            };
            Insert(chatGroup);
            await Save();
            await _Usergroup.JoinGroup(model.UserId, chatGroup.Id);

            var res = ChatGroupMapper.GroupMapToViewModel(chatGroup);

            return new ChatGroupViewModel()
            {
                GroupTitle = chatGroup.GroupTitle,
                GroupToken = chatGroup.GroupToken,
                ImageName = chatGroup.ImageName,
                IsPrivate = chatGroup.IsPrivate,
                OwnerId = chatGroup.OwnerId,
                Id = chatGroup.Id,
                CreateDate = chatGroup.CreateDate,
            };

 
        }

        public async Task<ChatGroupViewModel> InsertPrivateGroup(long userId, long receiverId)
        {
            var group = await Table<ChatGroup>()
                .Include(c => c.User)
                .Include(c => c.Receiver)
                .SingleOrDefaultAsync(s =>
                    s.OwnerId == userId && s.ReceiverId == receiverId
                    || s.OwnerId == receiverId && s.ReceiverId == userId);

            if (group == null)
            {
                var groupCreated = new ChatGroup()
                {
                    CreateDate = DateTime.Now,
                    GroupTitle = $"Chat With {receiverId}",
                    GroupToken = Guid.NewGuid().ToString(),
                    ImageName = "Default.jpg",
                    IsPrivate = true,
                    OwnerId = userId,
                    ReceiverId = receiverId
                };
                Insert(groupCreated);
                await Save();
                var res = await GetGroupBy(groupCreated.Id);
                return res;

            }
            return ChatGroupMapper.GroupMapToViewModel(group);
        }

        public async Task<ChatGroupViewModel> GetGroupBy(long id)
        {
            var group = await Table<ChatGroup>()
                .Include(c => c.User)
                .Include(c => c.Receiver)
                .FirstOrDefaultAsync(g => g.Id == id);

            return ChatGroupMapper.GroupMapToViewModel(group);
        }

        public async Task<ChatGroupViewModel> GetGroupBy(string token)
        {
            var group = await Table<ChatGroup>()
                .Include(c => c.User)
                .Include(c => c.Receiver)
                .FirstOrDefaultAsync(g => g.GroupToken == token);

            return ChatGroupMapper.GroupMapToViewModel(group);
        }
    }
}
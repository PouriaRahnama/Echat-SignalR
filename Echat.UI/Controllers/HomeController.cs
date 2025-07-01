using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Echat.UI.Hubs;
using Echat.Application.Services.Chats.ChatGroups;
using Echat.Application.Services.Users.UserGroups;
using Echat.Application.Services.Chats;
using Echat.Application.ViewModels.Chats;
using Echat.Application.Utilities;

namespace Echat.UI.Controllers
{
    public class HomeController : Controller
    {
        private IChatGroupService _chatGroup;
        private IHubContext<ChatHub> _chatHub;
        private IUserGroupService _userGroup;
        private IChatService _chatService;

        public HomeController(IChatGroupService chatGroup, IHubContext<ChatHub> chatHub, IUserGroupService userGroup, IChatService chatService)
        {
            _chatGroup = chatGroup;
            _chatHub = chatHub;
            _userGroup = userGroup;
            _chatService = chatService;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = await _userGroup.GetUserGroups(User.GetUserId());
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task CreateGroup([FromForm] CreateGroupViewModel model)
        {
            try
            {
                model.UserId = User.GetUserId();
                var result = await _chatGroup.InsertGroup(model);
                await _chatHub.Clients.User(User.GetUserId().ToString()).SendAsync("NewGroup", result.GroupTitle, result.GroupToken, result.ImageName);
            }
            catch
            {
                await _chatHub.Clients.User(User.GetUserId().ToString()).SendAsync("NewGroup", "ERROR");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task SendMessage([FromForm] InsertChatVIewModel model)
        {
            model.UserId = User.GetUserId();
            var result = await _chatService.SendMessage(model);
            result.UserName = User.GetUserName();
            var userIds = await _userGroup.GetUserIds(model.GroupId);
            await _chatHub.Clients.Users(userIds).SendAsync("ReceiveNotification", result);
            await _chatHub.Clients.Group(model.GroupId.ToString()).SendAsync("ReceiveMessage", result);
        }

        [Authorize]
        public async Task<IActionResult> Search(string title)
        {
            return new ObjectResult(await _chatGroup.Search(title, User.GetUserId()));
        }
    }
}

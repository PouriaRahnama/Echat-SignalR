using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Echat.Domain.Entities;
using Echat.Domain.Entities.Chats;

namespace Echat.Domain.Entities.Users
{
    public class UserGroup : BaseEntity
    {
        public long UserId { get; set; }
        public long GroupId { get; set; }

        #region Relations
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("GroupId")]
        public ChatGroup ChatGroup { get; set; }

        #endregion
    }
}
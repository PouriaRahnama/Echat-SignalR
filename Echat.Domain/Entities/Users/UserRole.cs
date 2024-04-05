using System.ComponentModel.DataAnnotations.Schema;
using Echat.Domain.Entities.Roles;

namespace Echat.Domain.Entities.Users
{
    public class UserRole : BaseEntity
    {
        public long RoleId { get; set; }
        public long UserId { get; set; }


        #region Relations
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        #endregion
    }
}
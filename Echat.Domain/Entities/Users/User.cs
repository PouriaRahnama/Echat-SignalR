using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Echat.Domain.Entities;
using Echat.Domain.Entities.Chats;

namespace Echat.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        [MaxLength(50)]
        public string UserName { get; set; }
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(110)]
        public string Avatar { get; set; }



        #region Relation
        [InverseProperty("User")]
        public ICollection<ChatGroup> ChatGroups { get; set; }
        [InverseProperty("Receiver")]
        public ICollection<ChatGroup> PrivateGroup { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }

        #endregion
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Echat.Domain.Enums;

namespace Echat.Domain.Entities.Roles
{
    public class RolePermission : BaseEntity
    {
        public long RoleId { get; set; }
        public Permission Permission { get; set; }

        #region Relation

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        #endregion
    }
}
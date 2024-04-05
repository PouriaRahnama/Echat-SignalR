using System.ComponentModel.DataAnnotations;

namespace Echat.Domain.Entities.Roles
{
    public class Role : BaseEntity
    {
        [MaxLength(50)]
        public string Title { get; set; }
    }
}
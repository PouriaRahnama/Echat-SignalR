using System;
using System.ComponentModel.DataAnnotations;

namespace Echat.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
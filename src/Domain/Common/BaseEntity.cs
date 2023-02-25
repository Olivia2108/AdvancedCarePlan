using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
        }

        protected BaseEntity(long id)
        {
            Id = id;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } 
        public bool IsActive { get; set; } 
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; } 
        public DateTime DateDeleted { get; set; }
        public string? IpAddress { get; set; }
        public string? DeviceChannel { get; set; }
         
    }
}

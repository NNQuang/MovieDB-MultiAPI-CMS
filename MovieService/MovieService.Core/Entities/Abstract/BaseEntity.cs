using System;

namespace MovieService.Core.Entities.Abstract
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string CreatedByName { get; set; } = "AutoCreate";
        public virtual string ModifiedByName { get; set; }
        public virtual bool IsActive { get; set; } = true;
        public virtual bool IsDeleted { get; set; } = false;
        public virtual string Note { get; set; }
    }
}
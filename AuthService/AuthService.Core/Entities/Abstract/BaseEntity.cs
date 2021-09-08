using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Core.Entities.Abstract
{
    public abstract class BaseEntity: IBaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime CreateDate { get; set; } = DateTime.Now;
        public virtual DateTime ModifiedDate { get; set; }
        public virtual bool IsActive { get; set; } = false;
        public virtual bool IsDeleted { get; set; } = false;
        public virtual string ModifiedByName { get; set; }
    }
}

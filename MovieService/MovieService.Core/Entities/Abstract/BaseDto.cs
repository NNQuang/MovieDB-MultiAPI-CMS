using System;
using System.Collections.Generic;
using System.Text;

namespace MovieService.Core.Entities.Abstract
{
    public abstract class BaseDto
    {
        public virtual DateTime CreateDate { get; set; }
        public virtual string CreatedByName { get; set; }
        public virtual bool IsActive { get; set; } = true;
        public virtual bool IsDeleted { get; set; } = false;
        public virtual string Note { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Lares.Entities
{
    public class Property : BaseEntity
    {
        public string OwnerUserId { get; set; }
        public virtual User OwnerUser { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public DateTime AcquiredDate { get; set; }
    }
}

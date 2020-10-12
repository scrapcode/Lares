using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lares.Entities
{
    public class Asset : BaseEntity
    {
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public DateTime AcquiredDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public virtual List<WorkTask> Tasks { get; set; }
    }
}

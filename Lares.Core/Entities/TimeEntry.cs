using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lares.Entities
{
    public class TimeEntry : BaseEntity
    {
        public int WorkTaskId { get; set; }
        public virtual WorkTask WorkTask { get; set; }
        public int ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
        public DateTime DateOfWork { get; set; }
        public int HoursWorked { get; set; }
        public string Description { get; set; }
    }
}

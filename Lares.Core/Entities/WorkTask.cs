using System;
using System.Collections.Generic;
using System.Text;

namespace Lares.Entities
{
    public class WorkTask : BaseEntity
    {
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkTaskStatus Status { get; set; }
        public virtual List<TimeEntry> TimeEntries { get; set; }
        public virtual List<MaterialEntry> MaterialEntries { get; set; }
    }

    public enum WorkTaskStatus
    {
        Pending,
        Active,
        Complete,
        Cancelled
    }
}

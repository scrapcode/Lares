using System;
using System.Collections.Generic;
using System.Text;

namespace Lares.Entities
{
    public class MaterialEntry : BaseEntity
    {
        public int WorkTaskId { get; set; }
        public virtual WorkTask WorkTask { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; }
        public decimal QuantityUsed { get; set; }
        public decimal CostPerUnit { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Lares.Entities;

namespace Lares.ViewModels
{
    public class AssetViewModel
    {
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [MaxLength(64)]
        public string Make { get; set; }

        [MaxLength(128)]
        public string Model { get; set; }

        [MaxLength(128)]
        public string SerialNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime AcquiredDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedOn { get; set; }

        public virtual List<WorkTask> Tasks { get; set; }
    }
}

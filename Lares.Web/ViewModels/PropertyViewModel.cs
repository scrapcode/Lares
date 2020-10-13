using System;
using System.ComponentModel.DataAnnotations;

using Lares.Entities;

namespace Lares.ViewModels
{
    public class PropertyViewModel
    {
        public int Id { get; set; }

        public int OwnerUserId { get; set; }
        public virtual User OwnerUser { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [MaxLength(128)]
        public string Address1 { get; set; }

        [MaxLength(128)]
        public string Address2 { get; set; }

        [DataType(DataType.Date)]
        public DateTime AcquiredDate { get; set; }
    }
}

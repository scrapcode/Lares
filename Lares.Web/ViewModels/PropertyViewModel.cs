using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

using Lares.Entities;
using System.Collections.Generic;

namespace Lares.ViewModels
{
    public class PropertyViewModel
    {
        public int Id { get; set; }

        public string OwnerUserId { get; set; }

        // This is populated in the controller
        public SelectList OwnerSelectList { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [Display(Name = "Address Line 1")]
        [MaxLength(128)]
        public string Address1 { get; set; }

        [Display(Name = "Address Line 2")]
        [MaxLength(128)]
        public string Address2 { get; set; }

        [Display(Name = "Date Acquired")]
        [DataType(DataType.Date)]
        public DateTime AcquiredDate { get; set; }

        public List<Asset> Assets { get; set; }

        [Display(Name = "Set as Current Property?")]
        public bool SetAsCurrentProp { get; set; }
    }
}

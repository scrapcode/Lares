using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Lares.Entities
{
    public class User : IdentityUser
    {
        public string Avatar { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastLogin { get; set; }
        public virtual List<Property> Properties { get; set; }
    }
}

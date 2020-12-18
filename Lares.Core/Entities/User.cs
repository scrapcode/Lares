using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Lares.Entities
{
    public class User : IdentityUser
    {
        /*
         * Profile Info
         */
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Avatar { get; set; }
        public virtual List<Property> Properties { get; set; }
        public int SelectedPropertyId { get; set; }

        /*
         * Implement Trackables
         */
        public DateTime CreatedOn { get; set; }
        public DateTime LastLogin { get; set; }
    }
}

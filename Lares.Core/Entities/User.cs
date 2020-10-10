using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Lares.Entities
{
    public class User : IdentityUser
    {
        public string Avatar { get; set; }
    }
}

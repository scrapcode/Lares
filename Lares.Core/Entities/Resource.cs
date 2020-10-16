using System;
using System.Collections.Generic;
using System.Text;

namespace Lares.Entities
{
    public class Resource : BaseEntity
    {
        public string ShortCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OnboardDate { get; set; }
        public DateTime TerminationDate { get; set; }
    }
}

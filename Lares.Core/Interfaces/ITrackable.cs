using System;
using System.Collections.Generic;
using System.Text;

namespace Lares.Interfaces
{
    public interface ITrackable
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}

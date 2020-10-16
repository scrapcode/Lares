using System;
using System.Collections.Generic;
using System.Text;

using Lares.Interfaces;

namespace Lares.Entities
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
    }
}

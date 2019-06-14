using System;
using System.Diagnostics;

namespace ServerTracker.Data.Models
{
    [DebuggerDisplay("{Name}")]
    public class Environment
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

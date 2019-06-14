using System;
using System.Diagnostics;

namespace ServerTracker.Data.Models
{
    [DebuggerDisplay("{Name} ({DomainName})")]
    public class Server
    {
        public long Id { get; set; }

        public long EnvironmentId { get; set; }

        public string Name { get; set; }

        public string DomainName { get; set; }

        public string IpAddress { get; set; }

        public string OperatingSystem { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

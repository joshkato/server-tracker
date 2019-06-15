using System;
using System.Diagnostics;

namespace ServerTracker.Data.Models
{
    [DebuggerDisplay("{Message}")]
    public class ServiceError
    {
        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}

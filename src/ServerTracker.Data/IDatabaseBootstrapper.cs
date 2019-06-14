using System;
using System.Collections.Generic;
using System.Text;

namespace ServerTracker.Data
{
    public interface IDatabaseBootstrapper
    {
        void BootstrapDatabase();
    }
}

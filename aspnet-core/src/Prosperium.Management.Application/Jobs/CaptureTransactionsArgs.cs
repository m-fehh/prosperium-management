using System;
using System.Collections.Generic;

namespace Prosperium.Management.Jobs
{
    public class CaptureTransactionsArgs
    {
        public int TenantId { get; set; }
        public List<string> IdsAccountOrCard { get; set; }
        public DateTime DateInitial { get; set; }
        public DateTime DateEnd { get; set; }
    }

}

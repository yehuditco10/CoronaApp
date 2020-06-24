using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fines_Service
{
   public class FinesPolicyData:ContainSagaData
    {
        public string UserId { get; set; }
        public bool IsUserTested { get; set; }
        public bool IsUserViolations { get; set; }
    }
}

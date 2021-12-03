using System;

namespace Contracts
{
    public interface SubmitOrder
    {
        Guid OrderId { get; set; }
        DateTime Timestamp { get; set; }
        string CustomerNumber { get; set; }
    }
}

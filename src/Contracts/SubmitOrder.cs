using System;

namespace Contracts
{
    public interface SubmitOrder
    {
        Guid OrderId { get; set; }
        DateTime Timestamp { get; set; }
        string CustomerNumber { get; set; }
    }

    public interface OrderSubmissionAccepted
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }
        string CustomerNumber { get;  }
    }
}

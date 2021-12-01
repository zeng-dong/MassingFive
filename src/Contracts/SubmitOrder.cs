using System;

namespace Contracts
{
    public interface SubmitOrder
    {
        Guid OrderId { get; set; }
        DateTime TimeStamp { get; set; }
        string CustomerNumber { get; set; }
    }
}

using Contracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Components.Consumers
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            await context.RespondAsync<OrderSubmissionAccepted>(new
            {
                InVar.Timestamp
            });
            
        }
    }
}

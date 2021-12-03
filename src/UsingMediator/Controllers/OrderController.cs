using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace UsingMediator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        readonly ILogger<OrderController> _logger;
        readonly IRequestClient<SubmitOrder> _submitOrderRequestClient;

        public OrderController(ILogger<OrderController> logger, IRequestClient<SubmitOrder> submitOrderRequestClient)
        {
            _logger = logger;
            _submitOrderRequestClient = submitOrderRequestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            //Response<OrderSubmissionAccepted> response = 
            //    await _submitOrderRequestClient
            //    .GetResponse<OrderSubmissionAccepted>(new
            //{
            //    OrderId = id,
            //    InVar.Timestamp,
            //    CustomerNumber = customerNumber,
            //});
            //return Ok(response.Message);

            var (accepted, rejected) =
                await _submitOrderRequestClient
                .GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(new
                {
                    OrderId = id,
                    InVar.Timestamp,
                    CustomerNumber = customerNumber,
                });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Accepted(response.Message);
            }
            else
            {
                var response = await rejected;
                return BadRequest(response.Message);
            }
        }
    }
}

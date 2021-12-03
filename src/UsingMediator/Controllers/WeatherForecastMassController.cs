using Contracts;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsingMediator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastMassController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IRequestClient<GetWeatherForecasts> _requestClient;

        public WeatherForecastMassController(IRequestClient<GetWeatherForecasts> requestClient, IMediator mediator)
        {
            _requestClient = requestClient;
            _mediator = mediator;
        }

        [HttpGet("OnDemandMediator")]
        public async Task<IEnumerable<WeatherForecast>> GetUsingDemandedRequestCleint()
        {
            var client = _mediator.CreateRequestClient<GetWeatherForecasts>();

            var response = await client.GetResponse<WeatherForecasts>(new { });
            return response.Message.Forecasts;
        }

        [HttpGet("JustGet")]
        public async Task<IEnumerable<WeatherForecast>> GetUsingInjectedRequestClient()
        {
            var response = await _requestClient.GetResponse<WeatherForecasts>(new { });
            return response.Message.Forecasts;
        }
    }
}

namespace Contracts
{
    public interface GetWeatherForecasts
    {
    }

    public interface WeatherForecasts
    {
        WeatherForecast[] Forecasts { get; }
    }
}
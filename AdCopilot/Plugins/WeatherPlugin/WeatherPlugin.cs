using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AdCopilot.Plugins.WeatherPlugin;

internal class WeatherPlugin
{
    [SKFunction, Description("Get the current weather details for a city.")]
    public static async Task<string> GetWeather(
        [Description("The city to retrieve weather information about.")] string city)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://api.weatherapi.com");

        var key = Environment.GetEnvironmentVariable("WEATHER_API_KEY");
        var response = await client.GetAsync($"v1/current.json?key={key}&q={city}");
        response.EnsureSuccessStatusCode();

        var results = await response.Content.ReadAsStringAsync();
        return results;
    }
}

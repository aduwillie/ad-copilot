using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AdCopilot.Plugins.VivaGlintPlugin;

internal class VivaGlintPlugin
{
    [SKFunction, Description("Get answers about Viva Glint or Glint.")]
    public static async Task<string> AskGlint(
        [Description("The question about Glint for which user needs an answer about.")] string question)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"https://localhost:5500/?q={question}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}

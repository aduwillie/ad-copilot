using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

namespace AdCopilot;

internal static class Helpers
{
    public static void PrintSkills(this IKernel kernel)
    {
        Console.WriteLine("Welcome to AdCopilot.\n\nHere is a list of all the plugins used.");
        foreach (var plugin in kernel.Functions.GetFunctionViews().Select((p, i) => new { Entry = p.Name, Index = i }))
        {
            Console.WriteLine($"{plugin.Index}: {plugin.Entry}");
        }
        Console.WriteLine("\n");
    }
}

using AdCopilot;
using AdCopilot.Plugins.MathPlugin;
using AdCopilot.Plugins.VivaGlintPlugin;
using AdCopilot.Plugins.WeatherPlugin;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planners;
using System.Text.Json;

var builder = new KernelBuilder();

builder.WithOpenAIChatCompletionService(
    modelId: "gpt-3.5-turbo",
    apiKey: Environment.GetEnvironmentVariable("OPENAI_KEY")!);

var kernel = builder.Build();

kernel.ImportSemanticFunctionsFromDirectory(
    parentDirectory: Path.Combine(Directory.GetCurrentDirectory(), "Plugins"),
    "GreetingsPlugin");
kernel.ImportFunctions(new MathPlugin(), nameof(MathPlugin));
kernel.ImportFunctions(new WeatherPlugin(), nameof(WeatherPlugin));
kernel.ImportFunctions(new VivaGlintPlugin(), nameof(VivaGlintPlugin));

// Display all skills
kernel.PrintSkills();

var planner = new SequentialPlanner(kernel);

// Run program
Console.WriteLine("How can I help you today?\n");
string? ask;
do
{
    ask = (Console.ReadLine() ?? string.Empty).Trim();
    if (string.IsNullOrEmpty(ask))
    {
        Console.WriteLine("Please provide an input or type .exit to quit.");
        continue;
    } else if(ask == ".exit")
    {
        Console.WriteLine("Thanks for using AdCopilot. Goodbye!");
        break;
    }

    try
    {
        var plan = await planner.CreatePlanAsync(ask);

        Console.WriteLine("###### Here is the plan prepared to execute this ######");
        Console.WriteLine("Plan:\n");
        Console.WriteLine(JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }));
        Console.WriteLine("######\n\n");

        var modelOutput = await kernel.RunAsync(
        variables: new ContextVariables
        {
            { "question", ask },
        }, 
        pipeline: plan);
        Console.WriteLine(modelOutput.GetValue<string>()!.Trim());
    }
    catch (Exception)
    {
        Console.WriteLine($"Unable to come up with plan for '{ask}'. Try again.\n");
        continue;
    }
} while (ask != "!exit");

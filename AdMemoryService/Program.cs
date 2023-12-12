using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.KernelMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var memory = new KernelMemoryBuilder()
    .WithOpenAIDefaults(Environment.GetEnvironmentVariable("OPENAI_KEY")!)
    .Build();

//await memory.ImportWebPageAsync("https://www.microsoft.com/en-us/microsoft-viva/glint");
var docPath = Path.Combine(Directory.GetCurrentDirectory(), "Docs", "viva-glint-doc.pdf");
await memory.ImportDocumentAsync(docPath);

builder.Services.AddSingleton(memory);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", async (
    [FromQuery] string q,
    [FromServices] IKernelMemory memory) =>
{
    var results = await memory.AskAsync(q);
    return results.Result;
})
.WithName("GetAnswer")
.WithOpenApi();

app.MapGet("/v2", async (
    [FromQuery] string q,
    [FromServices] IKernelMemory memory) =>
{
    var results = await memory.AskAsync(q);
    return new
    {
        Result = results.Result,
        Question = results.Question,
        Sources = results.RelevantSources,
    };
})
.WithName("GetAnswerV2")
.WithOpenApi();

app.Run();

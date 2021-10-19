using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarkdownQAGenerator;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using static CommandLine.Parser;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using IHost host = Host.CreateDefaultBuilder(args).Build();

static TService Get<TService>(IHost host) where TService : notnull => host.Services.GetRequiredService<TService>();

var parser = Default.ParseArguments<ActionInputs>(() => new(), args);
parser.WithNotParsed(
    errors =>
    {
        Get<ILoggerFactory>(host)
            .CreateLogger("MarkdownQAGenerator.Program").LogError(
                string.Join(Environment.NewLine, errors.Select(error => error.ToString())));
        Environment.Exit(2);
    });

await parser.WithParsedAsync(options => StartAnkiGenerator(options, host));
await host.RunAsync();


static async Task StartAnkiGenerator(ActionInputs inputs, IHost host)
{
    var logger = Get<ILoggerFactory>(host).CreateLogger("MarkdownQAGenerator");
    logger.LogInformation("MarkdownQAGenerator starting.");

    string destinationDirectory = inputs.DestinationDirectory;
    if (destinationDirectory.EndsWith(Path.DirectorySeparatorChar))
        destinationDirectory = destinationDirectory.Substring(0, destinationDirectory.Length - 1);
    string infos =
        AnkiJsonGenerator.GenerateAnkiJson(inputs.MarkDownFile, destinationDirectory, inputs.DeckName, logger);

    // https://docs.github.com/actions/reference/workflow-commands-for-github-actions#setting-an-output-parameter
    Console.WriteLine($"::set-output name=conversion-stats::{infos}");

    logger.LogInformation("MarkdownQAGenerator finished.");
    Environment.Exit(0);
}
using CommandLine;

namespace MarkdownQAGenerator
{
    public class ActionInputs
    {
        public ActionInputs() { }

        [Option('f', "markdown-file",
            Required = true,
            HelpText = "The markdown file to extract the questions and answers from to generate the .json file for CrowdAnki.")]
        public string MarkDownFile { get; set; } = null!;

        [Option('d', "destination-directory",
            Required = true,
            HelpText = "Where to save the generated files to.")]
        public string DestinationDirectory { get; set; } = null!;

        [Option('n', "deck-name",
            Default = "New Deck",
            Required = false,
            HelpText = "The name of the generated CrowdAnki deck.")]
        public string DeckName { get; set; } = null!;
    }
}
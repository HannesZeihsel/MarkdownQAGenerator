namespace MarkdownQAGenerator.CrowdAnkiJsonObjects
{
    public class Tmpl
    {
        public string afmt { get; } = "{{FrontSide}}\n\n<hr id=answer>\n\n{{Back}}";
        public string bafmt { get; } = "";
        public string bfont { get; } = "";
        public string bqfmt { get; } = "";
        public int bsize { get; } = 0;
        public object? did { get; } = null;
        public string name { get; } = "Card 1";
        public int ord { get; } = 0;
        public string qfmt { get; } = "{{Front}}";
    }
}
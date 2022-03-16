namespace MarkdownQAGenerator.CrowdAnkiJsonObjects
{
    public class Tmpl
    {
        public string afmt { get; } = "{{FrontSide}}\n\n<div class=\"solution\">\n{{Back}}\n</div>";
        public string bafmt { get; } = "";
        public string bfont { get; } = "";
        public string bqfmt { get; } = "";
        public int bsize { get; } = 0;
        public object? did { get; } = null;
        public string name { get; } = "Card 1";
        public int ord { get; } = 0;
        public string qfmt { get; } = "\\(\t\\renewcommand{\\vec}[1]{\\underline{#1}}\n\t\\newcommand{\\mat}[1]{\\underline{\\underline{#1}}}\n\t\\newcommand{\\dl}{\\mathrm{d}}\n\t\\newcommand{\\iu}{\\mathrm{i}}\n\t\\newcommand{\\transp}{\\mathsf{T}}\n\t\\newcommand{\\dp}{\\partial}\n\t\\newcommand{\\grad}{\\mathop{\\mathrm{grad}}}\n\t\\renewcommand{\\div}{\\mathop{\\mathrm{div}}}\n\t\\newcommand{\\diffp}[2]{\\frac{\\partial #1}{\\partial #2}}\n\\)\n<div class=\"question\">\n{{Front}}\n</div>";
    }
}

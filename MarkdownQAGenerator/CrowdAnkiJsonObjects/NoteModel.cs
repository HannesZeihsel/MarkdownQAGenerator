using System;

namespace MarkdownQAGenerator.CrowdAnkiJsonObjects
{
    public class NoteModel
    {
        public string __type__ { get; } = "NoteModel";
        public Guid crowdanki_uuid { get; } = new Guid("ccfe6806-ed3e-11eb-9d7c-00e04d766b9c");

        public string css { get; } =
            ".card {\n  font-family: arial;\n  font-size: 20px;\n  text-align: left;\n  color: black;\n  background-color: white;\n}\n";

        public Fld[] flds { get; } = { new Fld("Front", 0), new Fld("Back", 1) };
        public string latexPost { get; } = "\\end{document}";

        public string latexPre { get; } =
            "\\documentclass[12pt]{scrreprt}\n\\usepackage{booktabs}\n\\special{papersize=3in,5in}\n\\usepackage[T1]{fontenc}\n\\usepackage{tikz}\n\\usepackage{ucs}\n\\usepackage{lrtex.math}\n\\usepackage[utf8]{inputenc}\n\\usepackage[ngerman]{babel}\n\\usepackage{amssymb,amsmath}\n\\usepackage{siunitx}\n\\sisetup{exponent-product = \\cdot}\n\\pagestyle{empty}\n\\setlength{\\parindent}{0in}\n\\begin{document}";
        public bool latexsvg { get; } = false;
        public string name { get; } = "Basic";
        public object[][] req { get; } = { new object[] { 0 }, new object[] { "any" }, new object[] { 0 } };
        public int sortf { get; } = 0;
        public string[] tags { get; } = new string[0];
        public Tmpl[] tmpls { get; } = { new() };
        public int type { get; } = 0;
    }
}
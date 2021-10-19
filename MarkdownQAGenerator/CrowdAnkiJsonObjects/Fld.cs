namespace MarkdownQAGenerator.CrowdAnkiJsonObjects
{
    public class Fld
    {
        public string font { get; } = "Arial";
        public string name { get; set; }
        public int ord { get; set; }
        public bool rtl { get; } = false;
        public int size { get; } = 20;
        public bool sticky { get; } = false;

        public Fld(string name, int ord)
        {
            this.name = name;
            this.ord = ord;
        }
    }
}
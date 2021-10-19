namespace MarkdownQAGenerator.CrowdAnkiJsonObjects
{
    class RootDeck : Deck
    {
        public DeckConfiguration[] deck_configurations { get; } = { new() };
        public NoteModel[] note_models { get; set; } = { new() };

        public RootDeck(string name) : base(name) { }
    }
}
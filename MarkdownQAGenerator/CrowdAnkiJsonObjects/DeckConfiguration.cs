using System;
using System.Text.Json.Serialization;

namespace MarkdownQAGenerator.CrowdAnkiJsonObjects
{
    public class DeckConfiguration
    {
        public string __type__ { get; } = "DeckConfig";
        public bool autoplay { get; } = true;
        public Guid crowdanki_uuid { get; } = new Guid("31994d81-e956-11eb-8757-00e04d766b9c");
        public bool dyn { get; } = false;
        public int interdayLearningMix { get; } = 0;
        public Lapse lapse { get; } = new Lapse();
        public int maxTaken { get; } = 60;
        public string name { get; } = "Default";
        [JsonPropertyName("new")]
        public New _new { get; } = new New();

        public int newMix { get; } = 0;
        public int newPerDayMinimum { get; } = 0;
        public bool replayq { get; } = true;
        public Rev rev { get; } = new Rev();
        public int reviewOrder { get; } = 0;
        public int timer { get; } = 0;
    }
}
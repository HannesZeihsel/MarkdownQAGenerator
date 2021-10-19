using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MarkdownQAGenerator.CrowdAnkiJsonObjects
{
    public class Deck
    {
        public string __type__ { get; } = "Deck";
        public List<Deck> children { get; set; } = new List<Deck>();
        public Guid crowdanki_uuid { get; private set; }
        public Guid deck_config_uuid { get; } = new Guid("31994d81-e956-11eb-8757-00e04d766b9c");
        public string desc { get; } = "";
        public int dyn { get; } = 0;
        public int extendNew { get; } = 0;
        public int extendRev { get; } = 0;
        public List<string> media_files { get; set; } = new List<string>();
        public string name { get; private set; }
        public List<Note> notes { get; set; } = new List<Note>();
        public Guid note_model_uuid { get; } = new Guid("ccfe6806-ed3e-11eb-9d7c-00e04d766b9c");
        public List<string> tags { get; } = new List<string>();

        public Deck(string name)
        {
            this.name = name;
            using (MD5 md5 = MD5.Create())
            {
                crowdanki_uuid = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(name)));
            }
        }
    }
}
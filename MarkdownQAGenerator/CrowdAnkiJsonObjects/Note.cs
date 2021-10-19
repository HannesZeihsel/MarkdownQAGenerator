using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MarkdownQAGenerator.CrowdAnkiJsonObjects
{
    public class Note
    {
        public string __type__ { get; } = "Note";
        public string[] fields { get; private set; } = new string[2];
        public Guid guid { get; private set; }
        public Guid note_model_uuid { get; } = new Guid("ccfe6806-ed3e-11eb-9d7c-00e04d766b9c");
        public List<string> tags { get; } = new List<string>();

        public Note(string question)
        {
            fields[0] = question;
            fields[1] = "";
            using (MD5 md5 = MD5.Create())
            {
                guid = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(question)));
            }
        }

        public void AppendAnswer(string answer)
        {
            fields[1] += answer;
        }
    }
}
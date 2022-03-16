using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml;
using MarkdownQAGenerator.CrowdAnkiJsonObjects;
using Markdig;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;

namespace MarkdownQAGenerator
{
    public class AnkiJsonGenerator
    {
        /// <summary>
        /// Generates all the files required for the CrowdAnki representation of the given markdown file.
        /// </summary>
        /// <param name="markdownRegex">The regex path to the markdown file(s) that's CrowdAnki data should be generated.</param>
        /// <param name="destinationDirectory">The directory in which to save the generated files.</param>
        /// <param name="deckName">Name of the (root)deck.</param>
        /// <param name="logger">The logger to log data or null if no logging is required.</param>
        /// <param name="rootDirectory">The root directory at which to start searching for markdown
        /// files. If not set, attempts to get the root Directory from the markdownRegex string.</param>
        /// <returns>A string with infos that can be passed on to the user (line breaks in '%0A')</returns>
        public static string GenerateAnkiJson(string markdownRegex, string destinationDirectory, string deckName,
            ILogger? logger, string rootDirectory)
        {
            //generate AnkiJsonRepresentation
            RootDeck rootDeck = new(deckName);

            Matcher matcher = new();
            matcher.AddInclude(markdownRegex);
            
            if (string.IsNullOrEmpty(rootDirectory)) rootDirectory = Path.GetPathRoot(markdownRegex);
            logger.LogInformation($"Search for markdown files at '{markdownRegex}' in directory '{rootDirectory}'");
            IEnumerable<string?> files = matcher.GetResultsInFullPath(rootDirectory);

            foreach (var file in files)
            {
                var content = ConvertMarkdownToHtmlXml(file, logger);
                if (content is null)
                {
                    logger?.LogError($"Failed to convert markdown to html from file: {file}");
                    Environment.Exit(2);
                }

                string originDirectory = Path.GetDirectoryName(file);
                GenerateAnkiJsonFromXml(rootDeck, content.FirstChild, originDirectory, destinationDirectory, logger);
            }

            return SaveAnki(rootDeck, destinationDirectory, logger);
        }

        /// <summary>
        /// Saves the given deck as an anki format. (This will assume the pictures are already saved )
        /// </summary>
        /// <param name="rootDeck">The root deck to save to anki.</param>
        /// <param name="destinationDirectory"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private static string SaveAnki(RootDeck rootDeck, string destinationDirectory, ILogger? logger)
        {
            //save AnkiJson
            string jsonPath = Path.Combine(destinationDirectory, "deck.json");
            logger.LogInformation($"Write converted data to {jsonPath}");
            Directory.CreateDirectory(Path.GetDirectoryName(jsonPath));
            var opts = new JsonSerializerOptions(JsonSerializerDefaults.General)
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            File.WriteAllText(jsonPath, JsonSerializer.Serialize(rootDeck, opts));

            //generate info
            string info = "%0A";
            foreach (Deck deck in rootDeck.children)
                info += $"%0AFound {deck.notes.Count,3} Question(s) in Chapter '{deck.name}'.";
            return info;
        }

        private static XmlElement? ConvertMarkdownToHtmlXml(string markdownPath, ILogger? logger)
        {
            if (!File.Exists(markdownPath))
            {
                logger?.LogError($"Did not find Markdown file at path: {markdownPath}");
                Environment.Exit(2);
            }

            logger.LogInformation($"Loading Markdown file at {markdownPath}.");
            var xml = new XmlDocument();
            xml.LoadXml("<main>" + ConvertMarkdownToHtml(File.ReadAllText(markdownPath)) + "</main>");
            logger.LogInformation($"Converted Markdown to html");
            return xml["main"];
        }

        private static string ConvertMarkdownToHtml(string markdown)
        {
            return Markdown.ToHtml(markdown, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
        }

        /// <summary>
        /// Fills the chapters of the given root chapter with the XmlNode given in content and its
        /// following sibling XmlNodes.
        /// </summary>
        private static void GenerateAnkiJsonFromXml(RootDeck rootDeck, XmlNode? content, string originDirectory,
            string destinationDirectory, ILogger? logger)
        {
            if (content == null) return;
            do
            {
                switch (content.Name)
                {
                    case "h1":
                        //new Chapter
                        Deck chapter = new(content.InnerText);

                        content = GenerateAnkiJsonChapter(chapter, content.NextSibling, originDirectory,
                            destinationDirectory, logger)?.PreviousSibling;

                        logger?.LogInformation(
                            $"Found {chapter.notes.Count,3} Question(s) in Chapter '{chapter.name}'.");
                        rootDeck.children.Add(chapter);
                        break;
                    default:
                        //content between (and before/after) chapters
                        logger?.LogWarning("Following content of the markdown file was " +
                                           $"ignored: {content.InnerText}");
                        break;
                }
            } while ((content = content?.NextSibling) != null);
        }

        /// <summary>
        /// Fills the nodes of the given chapter with the XmlNode given in content and its following sibling XmlNodes.
        /// </summary>
        /// <returns>The XmlNote at which the parsing of the markdown (converted to html) should continue.
        /// That is an element that marks the end of a chapter (other chapter or separator)</returns>
        private static XmlNode? GenerateAnkiJsonChapter(Deck chapter, XmlNode? content, string originDirectory,
            string destinationDirectory, ILogger? logger)
        {
            if (content == null) return null;
            do
            {
                switch (content.Name)
                {
                    case "h1":
                        return content;
                    case "h2":
                        Note note = new(content.InnerXml);

                        content = GenerateAnkiJsonNote(chapter, note, content.NextSibling, originDirectory,
                            destinationDirectory, logger)?.PreviousSibling;

                        chapter.notes.Add(note);
                        break;
                    default:
                        //content between (and before/after) chapters
                        logger?.LogWarning("Following content of the markdown file was " +
                                           $"ignored: {content.InnerText}");
                        break;
                }
            } while ((content = content?.NextSibling) != null);

            return content;
        }

        /// <summary>
        /// Fills the answer of the given XmlNode with the answer given in content and its following sibling XmlNodes.
        /// </summary>
        /// <returns>The XmlNode at which the parsing of the markdown (converted to html) should continue.
        /// That is an element that marks the end of an answer (other chapter, other question or separator)</returns>
        private static XmlNode? GenerateAnkiJsonNote(Deck chapter, Note note, XmlNode? content,
            string originDirectory, string destinationDirectory, ILogger? logger)
        {
            if (content == null) return null;
            do
            {
                switch (content.Name)
                {
                    case "hr":
                        return content;
                    case "h1":
                        return content;
                    case "h2":
                        return content;
                    default:
                        string answer = ConvertToJsonStringAndManagePictures(chapter, content, originDirectory,
                            destinationDirectory, logger);
                        note.AppendAnswer(answer);
                        break;
                }
            } while ((content = content.NextSibling) != null);

            return content;
        }

        /// <summary>
        /// Converts the given content to a string while managing any pictures that may be present within content and
        /// handles all the files and references.
        /// </summary>
        private static string ConvertToJsonStringAndManagePictures(Deck chapter, XmlNode content,
            string originDirectory, string destinationDirectory, ILogger? logger)
        {
            ManagePictures(chapter, content, originDirectory, destinationDirectory, logger);
            return content.OuterXml;
        }

        /// <summary>
        /// This will change the src of any occurring pictures within the content and copies them to
        /// the appropriate folder with the new name for CrowdAnki.
        /// </summary>
        private static void ManagePictures(Deck chapter, XmlNode content, string originDirectory,
            string destinationDirectory, ILogger? logger)
        {
            if (content.Name == "img")
            {
                var att = content.Attributes["src"];
                string filepath = Path.Combine(originDirectory, att.Value);
                string newFilename = att.Value.Replace(Path.DirectorySeparatorChar, '_');
                string destinationFilepath = Path.Combine(destinationDirectory, "Media", newFilename);
                chapter.media_files.Add(newFilename);
                if (!File.Exists(filepath))
                {
                    logger?.LogError(
                        $"Failed to find a media file at the expected location {filepath}, " +
                        $"referenced with {content.OuterXml}");
                    Environment.Exit(2);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFilepath));
                    File.Copy(filepath, destinationFilepath, true);
                    logger?.LogInformation($"Media file copied from {filepath} to {destinationFilepath}");
                }

                att.Value = newFilename;
            }
            else
                foreach (XmlNode cNode in content.ChildNodes)
                    ManagePictures(chapter, cNode, originDirectory, destinationDirectory, logger);
        }
    }
}

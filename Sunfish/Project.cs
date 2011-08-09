using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Sunfish
{
    public class Project
    {
        public string Name;
        public string RootDirectory;
        public string SourceDirectory { get { return Path.Combine(RootDirectory, "source\\"); } }
        public string BinDirectory { get { return Path.Combine(RootDirectory, "bin\\"); } }
        public string Scenario { get; set; }
        public DateTime CacheCreationDate { get; set; }
        public List<string> SourceFiles { get; set; }
        public List<string> Strings { get; set; }
        public List<string> Includes = new List<string>();

        public static Project CurrentProject;

        public delegate void BeginImport();
        public event BeginImport OnImportBegin;

        public delegate void EndImport();
        public event BeginImport OnImportCompleted;

        public void SortSourceFiles()
        {
            int scnrindex = -1, globalsindex = -1, soundindex = -1;
            for (int i = 0; i < SourceFiles.Count; i++)
                if (Sunfish.Tag.Path.GetTagType(SourceFiles[i]) == "scnr") { scnrindex = i; }
                else if (Sunfish.Tag.Path.GetTagType(SourceFiles[i]) == "matg") { globalsindex = i; }
                else if (Sunfish.Tag.Path.GetTagType(SourceFiles[i]) == "ugh") { soundindex = i; }
            string matg = SourceFiles[globalsindex];
            string scnr = SourceFiles[scnrindex];
            string ugh = SourceFiles[soundindex];
            SourceFiles.RemoveAt(globalsindex);
            SourceFiles.RemoveAt(scnrindex);
            SourceFiles.RemoveAt(soundindex);
            SourceFiles.Insert(0, matg);
            SourceFiles.Insert(3, scnr);
            SourceFiles.Add(ugh);
        }

        public void Load(string filename)
        {
            Globals.Status = "Loading Project...";
            XmlReader xmlReader = XmlReader.Create(filename);
            while (xmlReader.Read())
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.LocalName)
                    {
                        case "Project":
                            Name = xmlReader.GetAttribute("name");
                            Scenario = xmlReader.GetAttribute("scenario");
                            CacheCreationDate = DateTime.FromBinary(long.Parse(xmlReader.GetAttribute("cache")));
                            break;
                        case "RootDirectory":
                            RootDirectory = xmlReader.GetAttribute("path");
                            break;
                        case "Includes":
                            XmlReader filesReader = xmlReader.ReadSubtree();
                            filesReader.Read();
                            while (filesReader.Read())
                            {
                                if (filesReader.NodeType == XmlNodeType.Element)
                                {
                                    switch (filesReader.LocalName)
                                    {
                                        case "include":
                                            string fName = filesReader.GetAttribute("path");
                                            Includes.Add(fName);
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            xmlReader.Close();
            SourceFiles = new List<string>(Directory.GetFiles(SourceDirectory, String.Format("*{0}", Sunfish.Tag.Path.Extension), SearchOption.AllDirectories));
            SortSourceFiles();
            List<CompilerTag> Tags = new List<CompilerTag>(SourceFiles.Count);
            Directory.SetCurrentDirectory(SourceDirectory);
            foreach (string filepath in SourceFiles)
                Tags.Add(new CompilerTag(filepath));
            Strings = new List<string>(10000);
            Strings.AddRange(Sunfish.Developmental.GlobalStringIDs.Values);
            foreach (CompilerTag tag in Tags)
                foreach (string str in tag.Strings)
                    if (!Strings.Contains(str)) Strings.Add(str);
            CurrentProject = this;
            Globals.ClearStatus();
        }

        public void Save()
        {
            Globals.Status = "Saving Project...";
            XmlWriter xmlWriter = XmlWriter.Create(Path.Combine(this.RootDirectory, Path.ChangeExtension(this.Name, ".h2proj")));
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Project");
            xmlWriter.WriteAttributeString("name", this.Name);
            xmlWriter.WriteAttributeString("scenario", this.Scenario);
            xmlWriter.WriteAttributeString("cache", this.CacheCreationDate.ToBinary().ToString());
            xmlWriter.WriteStartElement("RootDirectory");
            xmlWriter.WriteAttributeString("path", this.RootDirectory);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Includes");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            Globals.ClearStatus();
        }

        public static Project Create(string path, string name)
        {
            if (!Directory.Exists(path)) throw new Exception();
            Project p = new Project();
            p.RootDirectory = path;
            p.Name = name;
            Directory.CreateDirectory(p.SourceDirectory);
            Directory.CreateDirectory(p.BinDirectory);
            XmlWriter xmlWriter = XmlWriter.Create(Path.Combine(p.RootDirectory, Path.ChangeExtension(name, ".h2proj")));
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Project");
            xmlWriter.WriteAttributeString("name", name);
            xmlWriter.WriteAttributeString("cache", DateTime.MinValue.ToBinary().ToString());
            xmlWriter.WriteStartElement("RootDirectory");
            xmlWriter.WriteAttributeString("path", p.RootDirectory);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Includes");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return p;
        }

        public void ImportMap(Map map)
        {
            OnImportBegin();
            Decompiler decompiler = new Decompiler(map);
            Directory.SetCurrentDirectory(SourceDirectory);
            foreach (Index.TagInformation Entry in map.Index.TagEntries)
            {
                string filename = Path.ChangeExtension(map.Tagnames[Entry.Index & 0x0000FFFF], Index.GetCleanType(Entry.Type.ToString()).Trim()) + Tag.Path.Extension;
                if (Entry.Type == "sbsp" || Entry.Type == "ltmp")
                    decompiler.Decompile(Entry, filename, map.PrimaryMagic);
                else if (Entry.Type == "unic")
                    decompiler.DecompileUnic(Entry, filename, map.SecondaryMagic, map.Unicode[UnicodeTable.Language.English]);
                else
                    decompiler.Decompile(Entry, filename, map.SecondaryMagic);
                if (Entry.Type == "scnr") Scenario = filename;
            }
            Save();
            OnImportCompleted();
        }

        public void Compile()
        {
            Compiler c = new Compiler(File.Create(Path.Combine(BinDirectory, Path.ChangeExtension(Name, Map.Extension))));
            c.SetTagsDirectory(SourceDirectory);
            c.CompileFromScenario(this.Scenario);
        }

        public void ImportTag(string p)
        {
            ////local
            //string tagpath = string.Empty;
            //if (p.StartsWith(SourceDirectory)) { tagpath= p.Substring(SourceDirectory.Length); }
            ////external
            //else { }
            //if (tagpath == string.Empty) { return; }
            //this.SourceFiles.Add(tagpath);
            //Save();
        }
    }
}

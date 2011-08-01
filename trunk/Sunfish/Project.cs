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
        public List<string> SourceFiles { get { return _sourceFiles; } set { _sourceFiles = value; } }

        public delegate void BeginImport();
        public event BeginImport OnImportBegin;

        public delegate void EndImport();
        public event BeginImport OnImportCompleted;

        public void SortSourceFiles()
        {
            int scnrindex = -1, globalsindex = -1, soundindex = -1;
            for (int i = 0; i < _sourceFiles.Count; i++)
                if (Sunfish.Tag.Path.GetTagType(_sourceFiles[i]) == "scnr") { scnrindex = i; }
                else if (Sunfish.Tag.Path.GetTagType(_sourceFiles[i]) == "matg") { globalsindex = i; }
                else if (Sunfish.Tag.Path.GetTagType(_sourceFiles[i]) == "ugh") { soundindex = i; }
            string matg = _sourceFiles[globalsindex];
            string scnr = _sourceFiles[scnrindex];
            string ugh = _sourceFiles[soundindex];
            _sourceFiles.RemoveAt(globalsindex);
            _sourceFiles.RemoveAt(scnrindex);
            _sourceFiles.RemoveAt(soundindex);
            _sourceFiles.Insert(0, matg);
            _sourceFiles.Insert(3, scnr);
            _sourceFiles.Add(ugh);
        }

        public List<string> Includes = new List<string>();

        
        public List<string> _sourceFiles;

        public static Project Load(string filename)
        {
            Globals.Status = "Loading Project...";
            XmlReader xmlReader = XmlReader.Create(filename);
            Project p = new Project();
            while (xmlReader.Read())
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.LocalName)
                    {
                        case "Project":
                            p.Name = xmlReader.GetAttribute("name");
                            p.Scenario = xmlReader.GetAttribute("scenario");
                            p.CacheCreationDate = DateTime.FromBinary(long.Parse(xmlReader.GetAttribute("cache")));
                            break;
                        case "RootDirectory":
                            p.RootDirectory = xmlReader.GetAttribute("path");
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
                                            //Globals.Status = string.Format("Checking: {0}", fName);
                                            //string t = Path.Combine(p.SourceDirectory, fName);
                                            //if (!File.Exists(t)) throw new Exception();
                                            p.Includes.Add(fName);
                                            break;
                                    }
                                }
                                //Application.DoEvents();
                            }
                            break;
                    }
                }
            xmlReader.Close();
            //string[] oo = Directory.GetFiles(p.SourceDirectory, "*.h2tag", SearchOption.AllDirectories);
            //for (int i = 0; i < oo.Length; i++)
            //{
            //    oo[i] = oo[i].Replace(p.SourceDirectory + "\\", string.Empty);
            //}
            //p.SourceFiles.AddRange(oo);
            Globals.ClearStatus();
            return p;
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
            //Benchmark mark = new Benchmark();
            //mark.Begin();
            OnImportBegin();
            Decompiler decompiler = new Decompiler(map);
            Directory.SetCurrentDirectory(SourceDirectory);
            foreach (Index.TagInformation Entry in map.Index.TagEntries)
            {
                //Globals.Status = "Importing \"" + map.Tagnames[Entry.Index & 0x0000FFFF] + "\"";
                string filename = Path.ChangeExtension(map.Tagnames[Entry.Index & 0x0000FFFF], Index.GetCleanType(Entry.Type.ToString()).Trim()) + Tag.Path.Extension;
                //if (File.Exists(Path.Combine(this.SourceDirectory, filename)))
                //{
                //    if (MessageBox.Show("This tag already exists.\nDo you wish to overwrite the existing tag?", "File Conflict", MessageBoxButtons.YesNo) == DialogResult.No)
                //    { continue; }
                //}
                if (Entry.Type == "sbsp" || Entry.Type == "ltmp")
                    decompiler.Decompile(Entry, filename, map.PrimaryMagic);
                else if (Entry.Type == "unic")
                    decompiler.DecompileUnic(Entry, filename, map.SecondaryMagic, map.Unicode[UnicodeTable.Language.English]);
                else
                    decompiler.Decompile(Entry, filename, map.SecondaryMagic);
                if (Entry.Type == "scnr") Scenario = filename;
                //this.SourceFiles.Add(filename);
            }
            //Globals.ClearStatus();
            Save();
            OnImportCompleted();
            //mark.End();
            //MessageBox.Show(string.Format("Finished in: {0}", mark.Result), "Project Created");
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

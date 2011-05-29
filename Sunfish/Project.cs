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
        public string SourceDirectory { get { return Path.Combine(RootDirectory, "source"); } }
        public string BinDirectory { get { return Path.Combine(RootDirectory, "bin"); } }
        public string Scenario { get; set; }
        public List<string> SourceFiles = new List<string>();

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
                            break;
                        case "RootDirectory":
                            p.RootDirectory = xmlReader.GetAttribute("path");
                            break;
                        case "IncludedFiles":
                            XmlReader filesReader = xmlReader.ReadSubtree();
                            filesReader.Read();
                            while (filesReader.Read())
                            {
                                if (filesReader.NodeType == XmlNodeType.Element)
                                {
                                    switch (filesReader.LocalName)
                                    {
                                        case "File":
                                            string fName = filesReader.GetAttribute("path");
                                            //Globals.Status = string.Format("Checking: {0}", fName);
                                            //string t = Path.Combine(p.SourceDirectory, fName);
                                            //if (!File.Exists(t)) throw new Exception();
                                            p.SourceFiles.Add(fName);
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
            xmlWriter.WriteStartElement("RootDirectory");
            xmlWriter.WriteAttributeString("path", this.RootDirectory);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IncludedFiles");
            foreach (string fileName in this.SourceFiles)
            {
                xmlWriter.WriteStartElement("File");
                xmlWriter.WriteAttributeString("path", fileName);
                xmlWriter.WriteEndElement();
            }
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
            xmlWriter.WriteStartElement("RootDirectory");
            xmlWriter.WriteAttributeString("path", p.RootDirectory);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Files");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return p;
        }

        public void ImportMap(Map map)
        {
            //Benchmark mark = new Benchmark();
            //mark.Begin();
            Decompiler decompiler = new Decompiler(map);
            Directory.SetCurrentDirectory(SourceDirectory);
            foreach (Map.TagIndex.TagInfo Entry in map.Index.TagEntries)
            {
                Globals.Status = "Importing \"" + map.Tagnames[Entry.Id & 0x0000FFFF] + "\"";
                string filename = Path.ChangeExtension(map.Tagnames[Entry.Id & 0x0000FFFF], Map.TagIndex.GetCleanType(Entry.Type).Trim()) + Tag.Path.Extension;
                if (File.Exists(Path.Combine(this.SourceDirectory, filename)))
                {
                    if (MessageBox.Show("This tag already exists.\nDo you wish to overwrite the existing tag?", "File Conflict", MessageBoxButtons.YesNo) == DialogResult.No)
                    { continue; }
                }
                if (Entry.Type == "sbsp" || Entry.Type == "ltmp")
                    decompiler.Decompile(Entry, filename, map.PrimaryMagic);
                else if (Entry.Type == "unic")
                    decompiler.DecompileUnic(Entry, filename, map.SecondaryMagic, map.EnglishUnicode);
                else
                    decompiler.Decompile(Entry, filename, map.SecondaryMagic);
                if (Entry.Type == "scnr") Scenario = filename;
                Application.DoEvents();
                this.SourceFiles.Add(filename);
            }
            Globals.ClearStatus();
            Save();
            //mark.End();
            //MessageBox.Show(string.Format("Finished in: {0}", mark.Result), "Project Created");
        }

        public void Compile()
        {
            Compiler c = new Compiler(File.Create(Path.Combine(BinDirectory, Path.ChangeExtension(Name, Map.Extension))));
            c.SetTagsDirectory(SourceDirectory);
            c.CompileFromScenario(this.Scenario);
        }
    }
}

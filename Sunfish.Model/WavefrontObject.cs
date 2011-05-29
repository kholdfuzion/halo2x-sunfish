using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using System.IO;

namespace Sunfish.Mode
{
    public static class Wavefront
    {
        public static WavefrontObject ParseWavefrontOBJFile(string filename)
        {
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            string s = sr.ReadToEnd();
            sr.Close();
            string[] Lines = s.Split(new string[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            s = "";

            WavefrontObject Wavefront = new WavefrontObject();

            int Material = -1;
            int Group = -1;

            string Line;
            string[] components;
            string t;
            string last;
            int Length;

            bool Scan = true;

            for (int Pass = 0; Pass < 2; Pass++)
            {
                for (int i = 0; i < Lines.Length; i++)
                {
                    Line = Lines[i].Trim();
                    Length = Line.IndexOf(" ");
                    if (Length > 0)
                        Line = Line.Substring(0, Length);
                    last = s;
                    switch (Line)
                    {
                        case "v":
                            t = Lines[i].Trim();
                            components = t.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (Scan)
                                Wavefront.VertexCount++;
                            else
                            {
                                float x = float.Parse(components[1]);
                                float y = float.Parse(components[2]);
                                float z = float.Parse(components[3]);
                                Wavefront.Vertices.Add(new Vector3(x, y, z));
                            }
                            break;
                        case "vt":
                            t = Lines[i].Trim();
                            components = t.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (Scan)
                                Wavefront.TexcoordCount++;
                            else
                            {
                                float u = float.Parse(components[1]);
                                float v = float.Parse(components[2]);
                                Wavefront.Texcoords.Add(new Vector2(u, v));
                            }
                            break;
                        case "vn":
                            t = Lines[i].Trim();
                            components = t.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (Scan)
                                Wavefront.NormalCount++;
                            else
                            {
                                float x = float.Parse(components[1]);
                                float y = float.Parse(components[2]);
                                float z = float.Parse(components[3]);
                                Wavefront.Normals.Add(new Vector3(x, y, z));
                            }
                            break;
                        case "f":
                            t = Lines[i].Trim();
                            components = t.Split(new string[] { " ", "/" }, StringSplitOptions.RemoveEmptyEntries);
                            if (Scan)
                                Wavefront.FaceCount++;
                            else
                            {
                                Face temp = new Face(components);
                                temp.MaterialID = Material;
                                temp.GroupID = Group;
                                Wavefront.Faces.Add(temp);
                            }
                            break;
                        case "l":
                            t = Lines[i].Trim();
                            if (Scan)
                                Wavefront.LineCount++;
                            else
                            {
                                Line temp = new Line(t);
                                Wavefront.Lines.Add(temp);
                            }
                            break;
                        case "g":
                            t = Lines[i].Trim();
                            components = t.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (Scan)
                                Wavefront.GroupCount++;
                            else
                            {
                                if (!(components.Length == 1) && !Wavefront.Groups.ContainsKey(components[1]))
                                {
                                    Wavefront.Groups.Add(components[1], Wavefront.Groups.Count);
                                    Group = Wavefront.Groups[components[1]];
                                }
                                else if (!(components.Length == 1))
                                    Group = Wavefront.Groups[components[1]];
                            }
                            break;
                        case "usemtl":
                            t = Lines[i].Trim();
                            components = t.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (Scan)
                                Wavefront.MaterialCount++;
                            else
                            {
                                if (!Wavefront.Materials.ContainsKey(components[1]))
                                    Wavefront.Materials.Add(components[1], Wavefront.Materials.Count);
                                Material = Wavefront.Materials[components[1]];
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (Scan)
                {
                    Wavefront.Initialize();
                    Scan = false;
                }
            }
            return Wavefront;
        }

        public static void CreateWavefrontOBJFile(WavefrontObject wavefront, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fs);
            sw.NewLine = "\r\n";
            if (wavefront.VertexCount > 0)
                foreach (Vector3 v in wavefront.Vertices)
                {
                    sw.WriteLine("v " + Math.Round((double)v.X, 5, MidpointRounding.ToEven).ToString() + " " + Math.Round((double)v.Y, 5, MidpointRounding.ToEven).ToString() + " " + Math.Round((double)v.Z, 5, MidpointRounding.ToEven).ToString());
                }
            sw.WriteLine("# " + wavefront.VertexCount.ToString() + " Vertices");
            sw.WriteLine();

            if (wavefront.TexcoordCount > 0)
                foreach (Vector2 vt in wavefront.Texcoords)
                {
                    sw.WriteLine("vt " + Math.Round((double)vt.X, 5, MidpointRounding.ToEven).ToString() + " " + Math.Round((double)vt.Y, 5, MidpointRounding.ToEven).ToString());
                }
            sw.WriteLine("# " + wavefront.TexcoordCount.ToString() + " Texcoords");
            sw.WriteLine();
            if (wavefront.NormalCount > 0)
                foreach (Vector3 vn in wavefront.Normals)
                {
                    sw.WriteLine("vn " + Math.Round((double)vn.X, 5, MidpointRounding.ToEven).ToString() + " " + Math.Round((double)vn.Y, 5, MidpointRounding.ToEven).ToString() + " " + Math.Round((double)vn.Z, 5, MidpointRounding.ToEven).ToString());
                }
            sw.WriteLine("# " + wavefront.NormalCount.ToString() + " Normals");
            sw.WriteLine();

            if (wavefront.FaceCount > 0)
            {
                Dictionary<int, string> MaterialNames;
                MaterialNames = wavefront.Materials.ToDictionary(x => x.Value, x => x.Key);
                int[] Faces;
                string curMat = "";
                string newMat;
                Face temp;
                for (int Group = 0; Group < wavefront.GroupCount; Group++)
                {
                    Faces = wavefront.GetFaceIndicesUsingGroupID(Group);
                    for (int Face = 0; Face < Faces.Length; Face++)
                    {
                        temp = wavefront.Faces[Faces[Face]];
                        newMat = MaterialNames[temp.MaterialID];
                        if (curMat != newMat)
                        {
                            curMat = newMat;
                            sw.WriteLine("usemtl " + curMat);
                        }
                        sw.WriteLine("f " + temp.ToString());
                    }
                    sw.WriteLine("# " + Faces.Length.ToString() + " Faces");
                    sw.WriteLine();
                    curMat = "";
                }
            }

            sw.Dispose();
            fs.Dispose();
        }
    }

    public class WavefrontObject
    {
        public List<Vector3> Vertices;
        public List<Vector2> Texcoords;
        public List<Vector3> Normals;
        public Dictionary<string, int> Materials;
        public Dictionary<string, int> Groups;
        public List<Face> Faces;
        public List<Line> Lines;

        public int VertexCount;
        public int TexcoordCount;
        public int NormalCount;
        public int MaterialCount;
        public int FaceCount;
        public int LineCount;
        public int GroupCount;

        public WavefrontObject() { }

        public int[] GetFaceIndicesUsingMaterialID(int ID)
        {
            List<int> FaceIndices = new List<int>(FaceCount);
            {
                foreach (Face f in Faces)
                {
                    if (f.MaterialID == ID)
                        FaceIndices.Add(Faces.IndexOf(f));
                }
                FaceIndices.TrimExcess();
                return FaceIndices.ToArray();
            }
        }

        public int[] GetFaceIndicesUsingGroupID(int ID)
        {
            List<int> FaceIndices = new List<int>(FaceCount);
            {
                foreach (Face f in Faces)
                {
                    if (f.GroupID == ID)
                        FaceIndices.Add(Faces.IndexOf(f));
                }
                FaceIndices.TrimExcess();
                return FaceIndices.ToArray();
            }
        }

        public int[] GetFaceIndicesUsingGroupIDAndMaterialID(int GroupID, int MaterialID)
        {
            List<int> FaceIndices = new List<int>(FaceCount);
            {
                foreach (Face f in Faces)
                {
                    if (f.GroupID == GroupID && f.MaterialID == MaterialID)
                        FaceIndices.Add(Faces.IndexOf(f));
                }
                FaceIndices.TrimExcess();
                return FaceIndices.ToArray();
            }
        }

        public void Initialize()
        {
            Vertices = new List<Vector3>(VertexCount);
            Texcoords = new List<Vector2>(TexcoordCount);
            Normals = new List<Vector3>(NormalCount);
            Faces = new List<Face>(FaceCount);
            Materials = new Dictionary<string, int>(MaterialCount);
            Groups = new Dictionary<string, int>(GroupCount);
            Lines = new List<Line>(LineCount);
        }

        public BoundingBox GenerateBoundingBox()
        {
            BoundingBox bounds = new BoundingBox();
            foreach (Vector3 v in Vertices)
            {
                bounds.X.Update(v.X);
                bounds.Y.Update(v.Y);
                bounds.Z.Update(v.Z);
            }
            foreach (Vector2 vt in Texcoords)
            {
                bounds.U.Update(vt.X);
                bounds.V.Update(vt.Y);
            }
            return bounds;
        }
    }

    public struct Line
    {
        public int[] VertexIndices;

        public Line(int[] lineindices)
        {
            VertexIndices = lineindices;
        }

        public Line(string linestring)
        {
            bool HasTexcoords = linestring.Contains('/');
            string[] l = linestring.Split(new string[] { "/", " " }, StringSplitOptions.RemoveEmptyEntries);
            if (HasTexcoords)
            {
                VertexIndices = new int[(linestring.Length - 1) / 2];
                int Index = 0;
                for (int i = 1; i < VertexIndices.Length; i += 2)
                    VertexIndices[Index] = int.Parse(l[i]);
            }
            else
            {
                VertexIndices = new int[linestring.Length - 1];
                for (int i = 1; i < VertexIndices.Length; i++)
                    VertexIndices[i] = int.Parse(l[i]);
            }
        }
    }

    public struct Face
    {
        FaceUsage Usage;
        public short[] VertexIndices;
        public short[] TexcoordIndices;
        public short[] NormalIndices;

        public int MaterialID;

        public int GroupID;

        public Face(string[] components)
        {
            MaterialID = 0;
            GroupID = 0;
            VertexIndices = new short[0];
            TexcoordIndices = new short[0];
            NormalIndices = new short[0];
            Usage = (FaceUsage)0;
            int Count = (components.Length - 1) / 3;
            switch (Count)
            {
                case 1:
                    VertexIndices = new short[3];
                    VertexIndices[0] = (short)(short.Parse(components[1]) - 1);
                    VertexIndices[1] = (short)(short.Parse(components[2]) - 1);
                    VertexIndices[2] = (short)(short.Parse(components[3]) - 1);
                    Usage = FaceUsage.Vertex;
                    break;
                case 2:
                    VertexIndices = new short[3];
                    TexcoordIndices = new short[3];
                    VertexIndices[0] = (short)(short.Parse(components[1]) - 1);
                    VertexIndices[1] = (short)(short.Parse(components[3]) - 1);
                    VertexIndices[2] = (short)(short.Parse(components[5]) - 1);
                    TexcoordIndices[0] = (short)(short.Parse(components[2]) - 1);
                    TexcoordIndices[1] = (short)(short.Parse(components[4]) - 1);
                    TexcoordIndices[2] = (short)(short.Parse(components[6]) - 1);
                    Usage = FaceUsage.Vertex | FaceUsage.Texcoord;
                    break;
                case 3:
                    VertexIndices = new short[3];
                    TexcoordIndices = new short[3];
                    NormalIndices = new short[3];
                    VertexIndices[0] = (short)(short.Parse(components[1]) - 1);
                    VertexIndices[1] = (short)(short.Parse(components[4]) - 1);
                    VertexIndices[2] = (short)(short.Parse(components[7]) - 1);
                    TexcoordIndices[0] = (short)(short.Parse(components[2]) - 1);
                    TexcoordIndices[1] = (short)(short.Parse(components[5]) - 1);
                    TexcoordIndices[2] = (short)(short.Parse(components[8]) - 1);
                    NormalIndices[0] = (short)(short.Parse(components[3]) - 1);
                    NormalIndices[1] = (short)(short.Parse(components[6]) - 1);
                    NormalIndices[2] = (short)(short.Parse(components[9]) - 1);
                    Usage = FaceUsage.All;
                    break;
            }
        }

        public Face(short v1, short v2, short v3)
        {
            MaterialID = 0;
            GroupID = 0;
            TexcoordIndices = new short[0];
            NormalIndices = new short[0];
            VertexIndices = new short[3];
            VertexIndices[0] = v1;
            VertexIndices[1] = v2;
            VertexIndices[2] = v3;
            Usage = FaceUsage.Vertex;
        }

        public Face(short v1, short v2, short v3,
            short vt1, short vt2, short vt3)
        {
            MaterialID = 0;
            GroupID = 0;
            VertexIndices = new short[3];
            TexcoordIndices = new short[3];
            NormalIndices = new short[0];
            VertexIndices[0] = v1;
            VertexIndices[1] = v2;
            VertexIndices[2] = v3;
            TexcoordIndices[0] = vt1;
            TexcoordIndices[1] = vt2;
            TexcoordIndices[2] = vt3;
            Usage = FaceUsage.Vertex | FaceUsage.Texcoord;
        }

        public Face(short v1, short v2, short v3,
            short vt1, short vt2, short vt3,
            short vn1, short vn2, short vn3)
        {
            MaterialID = 0;
            GroupID = 0;
            VertexIndices = new short[3];
            TexcoordIndices = new short[3];
            NormalIndices = new short[3];
            VertexIndices[0] = v1;
            VertexIndices[1] = v2;
            VertexIndices[2] = v3;
            TexcoordIndices[0] = vt1;
            TexcoordIndices[1] = vt2;
            TexcoordIndices[2] = vt3;
            NormalIndices[0] = vn1;
            NormalIndices[1] = vn2;
            NormalIndices[2] = vn3;
            Usage = FaceUsage.All;
        }

        public bool IsDegenerate
        {
            get
            {
                if (VertexIndices[0] == VertexIndices[2] || VertexIndices[0] == VertexIndices[1] || VertexIndices[1] == VertexIndices[2])
                    return true;
                else return false;
            }
        }

        public override string ToString()
        {
            string v1 = "";
            string v2 = "";
            string v3 = "";
            string vt1 = "";
            string vt2 = "";
            string vt3 = "";
            string vn1 = "";
            string vn2 = "";
            string vn3 = "";
            if ((Usage & FaceUsage.Vertex) == FaceUsage.Vertex)
            {
                v1 = (VertexIndices[0] + 1).ToString();
                v2 = (VertexIndices[1] + 1).ToString();
                v3 = (VertexIndices[2] + 1).ToString();
            }
            if ((Usage & FaceUsage.Texcoord) == FaceUsage.Texcoord)
            {
                vt1 = (TexcoordIndices[0] + 1).ToString();
                vt2 = (TexcoordIndices[1] + 1).ToString();
                vt3 = (TexcoordIndices[2] + 1).ToString();
            }
            if ((Usage & FaceUsage.Normal) == FaceUsage.Normal)
            {
                vn1 = (NormalIndices[0] + 1).ToString();
                vn2 = (NormalIndices[1] + 1).ToString();
                vn3 = (NormalIndices[2] + 1).ToString();
            }
            string Val = "";
            switch (Usage)
            {
                case FaceUsage.Vertex:
                    Val = v1 + " " + v2 + " " + v3;
                    break;
                case FaceUsage.Vertex | FaceUsage.Texcoord:
                    Val = v1 + @"/" + vt1 + " " + v2 + @"/" + vt2 + " " + v3 + @"/" + vt3;
                    break;
                case FaceUsage.All:
                    Val = v1 + @"/" + vt1 + @"/" + vn1 + " " + v2 + @"/" + vt2 + @"/" + vn2 + " " + v3 + @"/" + vt3 + @"/" + vn3;
                    break;
            }
            return Val;
        }
    }

    public enum FaceUsage
    {
        Vertex,
        Texcoord,
        Normal,
        All = Vertex | Texcoord | Normal,
    }
}

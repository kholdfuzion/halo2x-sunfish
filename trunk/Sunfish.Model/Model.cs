using System;
using System.IO;
using Microsoft.DirectX;
using Sunfish.TagStructures;
using System.Text;

namespace Sunfish.Mode
{
    public class Range
    {
        public float Min;
        public float Max;

        public Range()
        {
            Min = 0;
            Max = 0;
        }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public byte[] ToByteArray()
        {
            byte[] buffer = new byte[8];
            Array.Copy(BitConverter.GetBytes(Min), 0, buffer, 0, 4);
            Array.Copy(BitConverter.GetBytes(Max), 0, buffer, 4, 4);
            return buffer;
        }

        public override string ToString()
        {
            return "Min: " + Min.ToString() + " Max: " + Max.ToString();
        }

        public void Update(float value)
        {
            if (value > Max)
                Max = value;
            else if (value < Min)
                Min = value;
        }

        public void Update(Range range)
        {
            if (range.Min > Max)
                Max = range.Min;
            if (range.Max > Max)
                Max = range.Max;
            if (range.Min < Min)
                Min = range.Min;
            if (range.Max < Min)
                Min = range.Max;
        }

        public void Commit()
        {
            Min = (float)Math.Round((double)Min, 5);
            Max = (float)Math.Round((double)Max, 5);
            Min -= 0.00001F;
            Max += 0.00001F;
        }
    }

    public enum VertexType
    {
        Rigid = 1,
        RigidBoned = 2,
        Skinned = 3,
    }

    [Flags]
    public enum Compression
    {
        None,
        Position,
        Texcoord,
        SecondaryTexcoord,
    }

    public class Model
    {
        public const int Size = 132;

        public string Name;
        public BoundingBox[] BoundingBoxes = new BoundingBox[0];
        public Region[] Regions = new Region[0];
        public Section[] Sections = new Section[0];
        public SectionGroup[] SectionGroups = new SectionGroup[0];
        public Node[] Nodes = new Node[0];
        public MarkerGroup[] MarkerGroups = new MarkerGroup[0];
        public Shader[] Shaders = new Shader[0];

        public Model(Tag tag)
        {
            BinaryReader br = new BinaryReader(tag.TagStream);
            tag.TagStream.Position = 0;
            Name = tag.StringReferenceNames[br.ReadInt32()];

            #region Bounding Boxes

            tag.TagStream.Position = 20;
            int Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                BoundingBoxes = new BoundingBox[Count];
                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * BoundingBox.Size);
                    BoundingBoxes[i] = new BoundingBox(tag.TagStream);
                }
            }

            #endregion

            #region Mesh Regions

            tag.TagStream.Position = 28;
            Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                Regions = new Region[Count];
                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * Region.Size);
                    Regions[i] = new Region(tag);
                }
            }

            #endregion

            #region Mesh Sections

            tag.TagStream.Position = 36;
            Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                Sections = new Section[Count];
                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * Section.Size);
                    Sections[i] = new Section(tag, BoundingBoxes[0]);
                }
            }

            #endregion

            #region Section Groups

            tag.TagStream.Position = 52;
            Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                SectionGroups = new SectionGroup[Count];
                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * MarkerGroup.Size);
                    SectionGroups[i] = new SectionGroup(tag.TagStream);
                }
            }

            #endregion

            #region Nodes

            tag.TagStream.Position = 72;
            Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                Nodes = new Node[Count];
                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * Node.Size);
                    Nodes[i] = new Node(tag);
                }
            }

            #endregion            

            #region Marker Groups

            tag.TagStream.Position = 88;
            Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                MarkerGroups = new MarkerGroup[Count];
                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * MarkerGroup.Size);
                    MarkerGroups[i] = new MarkerGroup(tag);
                }
            }

            #endregion

            #region Shaders

            tag.TagStream.Position = 96;
            Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                Shaders = new Shader[Count];
                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * Shader.Size);
                    Shaders[i] = new Shader(tag);
                }
            }

            #endregion
        }

        public byte[] Serialize(Tag tag)
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream); 
            if (!tag.StringReferenceNames.Contains(Name))
            {
                tag.StringReferenceNames.Add(Name);
            }
            bw.Write(tag.StringReferenceNames.IndexOf(Name));
            return stream.GetBuffer();
        }

        public Tag CreateTag()
        {
            Tag tag = new Tag();
            tag.Type = "mode";
            tag.Filename = Tag.Path.Create(Name, tag.Type);
            Sunfish.TagStructures.mode structure = new Sunfish.TagStructures.mode();
            structure.Data = this.Serialize(tag);

            float minx, maxx, minz, maxz, miny, maxy;
            minx = maxx = Sections[0].mesh.Vertices[0].X;
            miny = maxy = Sections[0].mesh.Vertices[0].Y;
            minz = maxz = Sections[0].mesh.Vertices[0].Z;
            foreach (Vector3 v in Sections[0].mesh.Vertices)
            {
                if (v.X < minx) minx = v.X;
                if (v.X > maxx) maxx = v.X;
                if (v.Y < miny) miny = v.Y;
                if (v.Y > maxy) maxy = v.Y;
                if (v.Z < minz) minz = v.Z;
                if (v.Z > maxz) maxz = v.Z;
            }

            BoundingBoxes[0].X = new Range(minx - 0.005f, maxx + 0.005f);
            BoundingBoxes[0].Y = new Range(miny - 0.005f, maxy + 0.005f);
            BoundingBoxes[0].Z = new Range(minz - 0.005f, maxz + 0.005f);

            foreach (BoundingBox boundingBox in this.BoundingBoxes)
            {
                TagBlockArray arr = structure.Values[structure.IndexOfValue("bounding box")] as TagBlockArray;
                arr.SetDataReference(structure.Data);
                TagBlock tb = arr.Default;
                tb.Data = boundingBox.Serialize();
                arr.Add(tb);
            }
            foreach (Region region in this.Regions)
            {
                TagBlockArray arr = structure.Values[structure.IndexOfValue("region")] as TagBlockArray;
                arr.SetDataReference(structure.Data);
                TagBlock tb = arr.Default;
                tb.Data = region.Serialize(tag);
                foreach (Permutation permuation in region.permutation)
                {
                    TagBlockArray permArr = tb.Values[tb.IndexOfValue("permutation")] as TagBlockArray;
                    permArr.SetDataReference(tb.Data);
                    TagBlock permTagBlock = permArr.Default;
                    permTagBlock.Data = permuation.Serialize(tag);
                    permArr.Add(permTagBlock);
                }
                arr.Add(tb);
            }
            foreach (Section section in this.Sections)
            {
                TagBlockArray arr = structure.Values[structure.IndexOfValue("section")] as TagBlockArray;
                arr.SetDataReference(structure.Data);
                TagBlock tb = arr.Default;
                tb.Data = section.Serialize(tag, BoundingBoxes[0]);
                foreach (Resource  resource in section.Resources)
                {
                    TagBlockArray permArr = tb.Values[tb.IndexOfValue("resource")] as TagBlockArray;
                    permArr.SetDataReference(tb.Data);
                    TagBlock permTagBlock = permArr.Default;
                    permTagBlock.Data = resource.Serialize();
                    permArr.Add(permTagBlock);
                }
                arr.Add(tb);
            }
            foreach (SectionGroup sectionGroup in this.SectionGroups)
            {
                TagBlockArray arr = structure.Values[structure.IndexOfValue("section group")] as TagBlockArray;
                arr.SetDataReference(structure.Data);
                TagBlock tb = arr.Default;
                tb.Data = sectionGroup.Serialize();
                foreach (CompoundNode compoundNode in sectionGroup.CompoundNodes)
                {
                    TagBlockArray permArr = tb.Values[tb.IndexOfValue("compound node")] as TagBlockArray;
                    permArr.SetDataReference(tb.Data);
                    TagBlock permTagBlock = permArr.Default;
                    permTagBlock.Data = compoundNode.Serialize();
                    permArr.Add(permTagBlock);
                }
                arr.Add(tb);
            } 
            foreach (Node node in this.Nodes)
            {
                TagBlockArray arr = structure.Values[structure.IndexOfValue("node")] as TagBlockArray;
                arr.SetDataReference(structure.Data);
                TagBlock tb = arr.Default;
                tb.Data = node.Serialize(tag);
                arr.Add(tb);
            }
            foreach (MarkerGroup markerGroup in this.MarkerGroups)
            {
                TagBlockArray arr = structure.Values[structure.IndexOfValue("marker group")] as TagBlockArray;
                arr.SetDataReference(structure.Data);
                TagBlock tb = arr.Default;
                tb.Data = markerGroup.Serialize(tag);
                foreach (Marker marker in markerGroup.Markers)
                {
                    TagBlockArray permArr = tb.Values[tb.IndexOfValue("marker")] as TagBlockArray;
                    permArr.SetDataReference(tb.Data);
                    TagBlock permTagBlock = permArr.Default;
                    permTagBlock.Data = marker.Serialize();
                    permArr.Add(permTagBlock);
                }
                arr.Add(tb);
            }
            foreach (Shader shader in this.Shaders)
            {
                TagBlockArray arr = structure.Values[structure.IndexOfValue("shader")] as TagBlockArray;
                arr.SetDataReference(structure.Data);
                TagBlock tb = arr.Default;
                tb.Data = shader.Serialize(tag);
                arr.Add(tb);
            }
            structure.Serialize(tag.TagStream, 0, structure.Size, 0);
            return tag;
        }
    }

    public class BoundingBox
    {
        public const float HalfRatio = ushort.MaxValue / 2;
        public const float FullRatio = ushort.MaxValue;
        public const int Size = 56;

        public Range X;
        public Range Y;
        public Range Z;
        public Range U;
        public Range V;
        public Range U2;
        public Range V2;

        public BoundingBox() { }

        public BoundingBox(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            X = new Range(br.ReadSingle(), br.ReadSingle());
            Y = new Range(br.ReadSingle(), br.ReadSingle());
            Z = new Range(br.ReadSingle(), br.ReadSingle());
            U = new Range(br.ReadSingle(), br.ReadSingle());
            V = new Range(br.ReadSingle(), br.ReadSingle());
            U2 = new Range(br.ReadSingle(), br.ReadSingle());
            V2 = new Range(br.ReadSingle(), br.ReadSingle());
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(X.Min); bw.Write(X.Max);
            bw.Write(Y.Min); bw.Write(Y.Max);
            bw.Write(Z.Min); bw.Write(Z.Max);
            bw.Write(U.Min); bw.Write(U.Max);
            bw.Write(V.Min); bw.Write(V.Max);
            bw.Write(U2.Min); bw.Write(U2.Max);
            bw.Write(V2.Min); bw.Write(V2.Max);
            bw.Close();
            return stream.ToArray();
        }
    }

    public class Region
    {
        public const int Size = 16;
        public string name;
        short nodemapoffset;
        short nodemapsize;
        public Permutation[] permutation = new Permutation[0];

        public Region(Tag tag)
        {
            BinaryReader br = new BinaryReader(tag.TagStream);
            name = tag.StringReferenceNames[br.ReadInt32()];
            nodemapoffset = br.ReadInt16();
            nodemapsize = br.ReadInt16();

            int Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                permutation = new Permutation[Count];

                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * Permutation.Size);
                    permutation[i] = new Permutation(tag);
                }
            }
        }

        public bool Contains(int i)
        {
            foreach (Permutation p in permutation)
            {
                if (p.Contains(i)) return true;
            }
            return false;
        }

        public int IndexOf(int i)
        {
            for (int x = 0; x < permutation.Length; x++)
            {
                if (permutation[x].Contains(i)) return x;
            }
            return -1;
        }

        public byte[] Serialize(Tag tag)
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            if (!tag.StringReferenceNames.Contains(name))
            {
                tag.StringReferenceNames.Add(name);
            }
            bw.Write(tag.StringReferenceNames.IndexOf(name));
            bw.Write(nodemapoffset);
            bw.Write(nodemapsize);
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class Permutation
    {
        public const int Size = 16;

        public string name;
        public short[] indices;

        public Permutation(Tag tag)
        {
            BinaryReader br = new BinaryReader(tag.TagStream);
            name = tag.StringReferenceNames[br.ReadInt32()];
            indices = new short[6];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = br.ReadInt16();
            }
        }

        public bool Contains(int i)
        {
            foreach (short s in indices)
            {
                if (s == i) return true;
            }
            return false;
        }

        public int IndexOf(int i)
        {
            for (int x = 0; x < 6; x++)
            {
                if (indices[x] == i) return x;
            }
            return -1;
        }

        public byte[] Serialize(Tag tag)
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            if (!tag.StringReferenceNames.Contains(name))
            {
                tag.StringReferenceNames.Add(name);
            }
            bw.Write(tag.StringReferenceNames.IndexOf(name));
            for (int i = 0; i < indices.Length; i++)
            {
                bw.Write(indices[i]);
            }
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class Resource
    {
        public const int Size = 16;

        public ResourceType MainRawDataType;
        public ResourceSubType SubRawDataType;
        public int RawDataOffset;
        public int RawDataSize;

        public Resource(ResourceType type, ResourceSubType subtype, int offset, int size)
        {
            MainRawDataType = type;
            SubRawDataType = subtype;
            RawDataOffset = offset;
            RawDataSize = size;
        }

        public Resource(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            MainRawDataType = (ResourceType)br.ReadInt32();
            SubRawDataType = (ResourceSubType)br.ReadInt32();
            RawDataSize = br.ReadInt32();
            RawDataOffset = br.ReadInt32();
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write((int)MainRawDataType);
            bw.Write((int)SubRawDataType);
            bw.Write(RawDataSize);
            bw.Write(RawDataOffset);
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class Section
    {
        public const int Size = 92;

        public VertexType VertexType;

        public short VertexCount;
        public short TriangleCount;

        public Mesh mesh;

        public Compression CompressionFlags;

        public int RawOffset;
        public int RawSize;
        public const int RawHeaderSize = 112;
        public int RawDataSize;
        public Resource[] Resources = new Resource[0];

        public Section(Tag tag, BoundingBox boundingBox)
        {
            BinaryReader br = new BinaryReader(tag.TagStream);

            int StartOffset = (int)tag.TagStream.Position;
            VertexType = (VertexType)br.ReadInt32();
            VertexCount = br.ReadInt16();
            TriangleCount = br.ReadInt16();

            tag.TagStream.Position = StartOffset + 26;
            CompressionFlags = (Compression)br.ReadInt32();

            tag.TagStream.Position = StartOffset + 56;
            RawOffset = br.ReadInt32();
            RawSize = br.ReadInt32();
            br.ReadInt32();
            RawDataSize = br.ReadInt32();

            tag.TagStream.Position = StartOffset + 72;
            int Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                Resources = new Resource[Count];
                for (int i = 0; i < Count; i++)
                {
                    tag.TagStream.Position = Offset + (i * Resource.Size);
                    Resources[i] = new Resource(tag.TagStream);
                }
            }
            if (Globals.IsExternalResource(RawOffset)) { return; }
            tag.ResourceStream.Position = tag.ResourceInformation[RawOffset].Address;
            mesh = new Mesh(tag.ResourceStream, Resources, this, boundingBox);
        }

        public byte[] Serialize(Tag tag, BoundingBox boundingBox)
        {
            RawOffset = tag.AddRaw(mesh.Serialize(this, boundingBox, out Resources));
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            VertexCount = (short)mesh.Vertices.Length;
            TriangleCount = (short)(mesh.Indices.Length - 2);
            VertexType = VertexType.Rigid;
            CompressionFlags = Compression.Position | Compression.Texcoord;
            bw.Write((int)VertexType);
            bw.Write(VertexCount);
            bw.Write(TriangleCount);
            stream.Position = 26;
            bw.Write((int)CompressionFlags);
            stream.Position = 56;
            bw.Write(RawOffset);//*
            bw.Write(0);//*
            bw.Write(RawHeaderSize);
            bw.Write(RawDataSize);
            if (!tag.TagReferences.Contains(tag.Filename))
            {
                tag.TagReferences.Add(tag.Filename);
            }
            bw.Write(tag.TagReferences.IndexOf(tag.Filename));
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class SectionGroup
    {
        public const int Size = 12;
        
        public DetailLevelFlags DetailLevels;
        public CompoundNode[] CompoundNodes = new CompoundNode[0];

        public SectionGroup()
        {
            DetailLevels = DetailLevelFlags.L1 | DetailLevelFlags.L2 | DetailLevelFlags.L3 | DetailLevelFlags.L4 | DetailLevelFlags.L5 | DetailLevelFlags.L6;
        }

        public SectionGroup(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            DetailLevels = (DetailLevelFlags)br.ReadInt32();
            int Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                CompoundNodes = new CompoundNode[Count];
                for (int i = 0; i < CompoundNodes.Length; i++)
                {
                    stream.Position = Offset + (i * CompoundNode.Size);
                    CompoundNodes[i] = new CompoundNode(stream);
                }
            }
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write((int)DetailLevels);
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class CompoundNode
    {
        public const int Size = 16;

        sbyte NodeIndex0;
        sbyte NodeIndex1;
        sbyte NodeIndex2;
        sbyte NodeIndex3;
        float NodeWeight0;
        float NodeWeight1;
        float NodeWeight2;

        public CompoundNode(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            NodeIndex0 = br.ReadSByte();
            NodeIndex1 = br.ReadSByte();
            NodeIndex2 = br.ReadSByte();
            NodeIndex3 = br.ReadSByte();
            NodeWeight0 = br.ReadSingle();
            NodeWeight1 = br.ReadSingle();
            NodeWeight2 = br.ReadSingle();
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(NodeIndex0);
            bw.Write(NodeIndex1);
            bw.Write(NodeIndex2);
            bw.Write(NodeIndex3);
            bw.Write(NodeWeight0);
            bw.Write(NodeWeight1);
            bw.Write(NodeWeight2);
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class Node
    {
        public const int Size = 96;

        public string Name;
        public short ParentNodeIndex;
        public short FirstChildNodeIndex;
        public short FirstSiblingNodeIndex;
        public short ImportNodeIndex;
        public Vector3 Translation;
        public Quaternion Rotation;
        public float Scale;
        public Vector3 InverseForward;
        public Vector3 InverseLeft;
        public Vector3 InverseUp;
        public Vector3 InverseTranslation;
        public float DistanceFromParent;

        public Node(Tag tag)
        {
            BinaryReader br = new BinaryReader(tag.TagStream);
            Name = tag.StringReferenceNames[br.ReadInt32()];
            ParentNodeIndex = br.ReadInt16();
            FirstChildNodeIndex = br.ReadInt16();
            FirstSiblingNodeIndex = br.ReadInt16();
            ImportNodeIndex = br.ReadInt16();
            Translation = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Rotation = new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Scale = br.ReadSingle();
            InverseForward = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            InverseLeft = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            InverseUp = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            InverseTranslation = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            DistanceFromParent = br.ReadSingle();
        }

        public byte[] Serialize(Tag tag)
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            if (!tag.StringReferenceNames.Contains(Name))
            {
                tag.StringReferenceNames.Add(Name);
            }
            bw.Write(tag.StringReferenceNames.IndexOf(Name));
            bw.Write(ParentNodeIndex);
            bw.Write(FirstChildNodeIndex);
            bw.Write(FirstSiblingNodeIndex);
            bw.Write(ImportNodeIndex);
            bw.Write(Translation.X);
            bw.Write(Translation.Y);
            bw.Write(Translation.Z);
            bw.Write(Rotation.W);
            bw.Write(Rotation.X);
            bw.Write(Rotation.Z);
            bw.Write(Rotation.Y);
            bw.Write(Scale);
            bw.Write(InverseForward.X);
            bw.Write(InverseForward.Y);
            bw.Write(InverseForward.Z);
            bw.Write(InverseLeft.X);
            bw.Write(InverseLeft.Y);
            bw.Write(InverseLeft.Z);
            bw.Write(InverseUp.X);
            bw.Write(InverseUp.Y);
            bw.Write(InverseUp.Z);
            bw.Write(InverseTranslation.X);
            bw.Write(InverseTranslation.Y);
            bw.Write(InverseTranslation.Z);
            bw.Write(DistanceFromParent);
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class Shader
    {
        public const int Size = 32;

        public int TagId;
        string filename;

        public Shader(Tag tag)
        {
            BinaryReader br = new BinaryReader(tag.TagStream);
            tag.TagStream.Position += 12;
            TagId = br.ReadInt32();
            filename = tag.TagReferences[TagId];
        }

        public byte[] Serialize(Tag tag)
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            stream.Position = 8;
            if (!tag.TagReferences.Contains(filename))
            {
                tag.TagReferences.Add(filename);
            }
            bw.Write(Encoding.UTF8.GetBytes(Tag.Path.GetTagType(filename)), 0, 4);
            TagId = tag.TagReferences.IndexOf(filename);
            bw.Write(TagId);
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class Marker
    {
        public const int Size = 36;
        public sbyte RegionIndex;
        public sbyte PermutationIndex;
        public short NodeIndex;
        public Vector3 Translation;
        public Quaternion Rotation;
        public int Scale;

        public Marker(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            RegionIndex = br.ReadSByte();
            PermutationIndex = br.ReadSByte();
            NodeIndex = br.ReadInt16();
            Translation = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Rotation = new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Scale = br.ReadInt32();
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(RegionIndex);
            bw.Write(PermutationIndex);
            bw.Write(NodeIndex);
            bw.Write(Translation.X);
            bw.Write(Translation.Y);
            bw.Write(Translation.Z);
            bw.Write(Rotation.W);
            bw.Write(Rotation.X);
            bw.Write(Rotation.Y);
            bw.Write(Rotation.Z);
            bw.Write(Scale);
            bw.Close();
            return stream.GetBuffer();
        }
    }

    public class MarkerGroup
    {
        public const int Size = 12;
        string Name;
        public Marker[] Markers = new Marker[0];

        public MarkerGroup(Tag tag)
        {
            BinaryReader br = new BinaryReader(tag.TagStream);
            Name = tag.StringReferenceNames[br.ReadInt32()];
            int Count = br.ReadInt32();
            if (Count > 0)
            {
                int Offset = br.ReadInt32();
                Markers = new Marker[Count];
                for (int i = 0; i < Markers.Length; i++)
                {
                    tag.TagStream.Position = Offset + (i * Marker.Size);
                    Markers[i] = new Marker(tag.TagStream);
                }
            }
        }

        public byte[] Serialize(Tag tag)
        {
            MemoryStream stream = new MemoryStream(Size);
            BinaryWriter bw = new BinaryWriter(stream);
            if (!tag.StringReferenceNames.Contains(Name))
            {
                tag.StringReferenceNames.Add(Name);
            }
            bw.Write(tag.StringReferenceNames.IndexOf(Name));
            bw.Close();
            return stream.GetBuffer();
        }
    }

    [FlagsAttribute]
    public enum DetailLevelFlags
    {
        L1,
        L2,
        L3,
        L4,
        L5,
        L6
    }

    public enum ResourceType
    {
        MeshInformation = 512,
        UnknownStruct8 = 524800,
        TriangleStrip = 2097664,
        Vertex = 3670530,
        BoneMap = 6554112,
        Unknown = 3670528,
    }

    public enum ResourceSubType
    {
        MeshInformationData = 4718592,
        UnknownStruct8 = 524296,
        IndiceStripData = 131104,
        UnknownData = 2097208,
        VertexData = 56,
        UVData = 65592,
        VectorData = 131128,
        BoneData = 65636,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Microsoft.DirectX;

namespace Sunfish.Model
{
    #region Enumerators

    public enum VertexType
    {
        Rigid = 1,
        RigidBoned = 2,
        Skinned = 3,
    }

    [Flags]
    public enum FaceUsage
    {
        Vertex,
        Texcoord,
        Normal,
        All = Vertex | Texcoord | Normal,
    }

    [Flags]
    public enum Compression
    {
        None,
        Position,
        Texcoord,
        SecondaryTexcoord,
    }

    #endregion

    #region Classes

    public class ModelRawData
    {
        const uint RawOffsetStart = 4 + RawHeaderData.Size;

        public RawHeaderData Header;
        public MeshInformationData[] MeshInformation;
        public UnknownStruct8Data[] Struct8;
        public TriangleIndiceData IndiceData;
        public UnknownStruct32Data[] Struct32;
        public VertexData PositionData;
        public TexcoordData TexcoordData;
        public VectorData VectorData;
        public BoneData BoneData;

        //Dictionary<Type, int> Offset;

        struct ResourceHeader
        {
            public const string Header = "hklb";
            public const uint Size = 4;
        }
        struct ResourceBlock
        {
            public const string Header = "crsr";
            public const uint Size = 4;
        }
        struct ResourceFooter
        {
            public const string Header = "fklb";
            public const uint Size = 4;
        }

        public ModelRawData()
        { }

        public ModelRawData(Stream stream, H2Model.Resource[] resources, H2Model.Section section)
        {
            //SectionData = section;

            int StartOffset = (int)stream.Position;
            Header = new RawHeaderData(stream);
            foreach (H2Model.Resource r in resources)
            {
                stream.Position = StartOffset + RawOffsetStart + r.RawDataOffset;
                
                switch (r.MainRawDataType)
                {
                    case H2Model.ResourceType.BoneMap:
                        BoneData = new BoneData(stream, (uint)r.RawDataSize);
                        break;
                    case H2Model.ResourceType.MeshInformation:
                        MeshInformation = new MeshInformationData[r.RawDataSize / MeshInformationData.Size];
                        for (int x = 0; x < MeshInformation.Length; x++)
                        {
                            MeshInformation[x] = new MeshInformationData(stream);
                        }
                        break;
                    case H2Model.ResourceType.UnknownStruct8:
                        Struct8 = new UnknownStruct8Data[r.RawDataSize / UnknownStruct8Data.Size];
                        for (int x = 0; x < Struct8.Length; x++)
                        {
                            Struct8[x] = new UnknownStruct8Data(stream);
                        }
                        break;
                    case H2Model.ResourceType.Unknown:
                        Struct32 = new UnknownStruct32Data[3];
                        for (int x = 0; x < 3; x++)
                        {
                            Struct32[x] = new UnknownStruct32Data(stream);
                        }
                        break;
                    case H2Model.ResourceType.Vertex:
                        switch (r.SubRawDataType)
                        {
                            case H2Model.ResourceSubType.VertexData:
                                PositionData = new VertexData(stream, (uint)r.RawDataSize);
                                break;
                            case H2Model.ResourceSubType.UVData:
                                TexcoordData = new TexcoordData(stream, (uint)r.RawDataSize);
                                break;
                            case H2Model.ResourceSubType.VectorData:
                                VectorData = new VectorData(stream, (uint)r.RawDataSize);
                                break;
                        }
                        break;
                    case H2Model.ResourceType.TriangleStrip:
                        IndiceData = new TriangleIndiceData(stream, (uint)r.RawDataSize);
                        break;
                }
            }
        }

        //public void InitializeOffsetDictionary()
        //{
        //    Offset = new Dictionary<Type, int>();
        //    uint Size = 0;
        //    Size += ResourceBlock.Size;
        //    Offset.Add(typeof(MeshInformationData), (int)Size);
        //    Size += (uint)(MeshInformationData.Size * MeshInformation.Length);
        //    Size += ResourceBlock.Size;
        //    Offset.Add(typeof(TriangleIndiceData), (int)Size);
        //    Size += IndiceData.Size;
        //    Size += ResourceBlock.Size;
        //    Offset.Add(typeof(UnknownStruct32Data), (int)Size);
        //    Size += (uint)(UnknownStruct32Data.Size * Struct32.Length);
        //    Size += ResourceBlock.Size;
        //    Offset.Add(typeof(VertexData), (int)Size);
        //    Size += PositionData.Size;
        //    Size += ResourceBlock.Size;
        //    Offset.Add(typeof(TexcoordData), (int)Size);
        //    Size += TexcoordData.Size;
        //    Size += ResourceBlock.Size;
        //    Offset.Add(typeof(VectorData), (int)Size);
        //    Size += VectorData.Size;
        //    Size += ResourceBlock.Size;
        //    Offset.Add(typeof(BoneData), (int)Size);
        //    Size += BoneData.Size == 1 ? 4 : BoneData.Size;
        //    Size += ResourceFooter.Size;
        //}

        //public uint RawSize
        //{
        //    get
        //    {
        //        uint Size = 0;
        //        Size += ResourceHeader.Size;
        //        Size += RawHeaderData.Size;
        //        Size += ResourceBlock.Size;
        //        Size += (uint)(MeshInformationData.Size * MeshInformation.Length);
        //        Size += ResourceBlock.Size;
        //        Size += IndiceData.Size;
        //        Size += ResourceBlock.Size;
        //        Size += (uint)(UnknownStruct32Data.Size * Struct32.Length);
        //        Size += ResourceBlock.Size;
        //        Size += PositionData.Size;
        //        Size += ResourceBlock.Size;
        //        Size += TexcoordData.Size;
        //        Size += ResourceBlock.Size;
        //        Size += VectorData.Size;
        //        Size += ResourceBlock.Size;
        //        Size += BoneData.Size == 1 ? 4 : BoneData.Size;
        //        Size += ResourceFooter.Size;
        //        return Size;
        //    }
        //}

        //public void SaveMeshInformation(MapStream map)
        //{
        //    FileStream DataSource = map.GetResourceStream((long)(SectionData.RawOffset & 0XC0000000));
        //    if (DataSource.CanWrite)
        //    {
        //        int Offset = SectionData.RawOffset & 0X3FFFFFFF;
        //        BinaryWriter bw = new BinaryWriter(DataSource);
        //        bw.BaseStream.Position = Offset;
        //        InitializeOffsetDictionary();
        //        bw.BaseStream.Position = SectionData.RawOffset + OffsetOf(typeof(MeshInformationData)) + RawOffsetStart;
        //        long StartOffset = bw.BaseStream.Position;
        //        for (int i = 0; i < MeshInformation.Length; i++)
        //        {
        //            bw.BaseStream.Position = StartOffset + (i * MeshInformationData.Size);
        //            MeshInformation[i].Save(bw);
        //        }
        //    }
        //}

        //public void RecompressRaw(Soffish.Model.BoundingBox oldbounds, Soffish.Model.BoundingBox newbounds)
        //{
        //    PositionData.RecompressRaw(oldbounds, newbounds);
        //    TexcoordData.RecompressRaw(oldbounds, newbounds);
        //}

        //public byte[] ToByteArray()
        //{
        //    uint Size = 0;
        //    Size += ResourceHeader.Size;
        //    Size += RawHeaderData.Size;
        //    Size += ResourceBlock.Size;
        //    Size += (uint)(MeshInformationData.Size * MeshInformation.Length);
        //    Size += ResourceBlock.Size;
        //    Size += IndiceData.Size;
        //    Size += ResourceBlock.Size;
        //    Size += (uint)(UnknownStruct32Data.Size * Struct32.Length);
        //    Size += ResourceBlock.Size;
        //    Size += PositionData.Size;
        //    Size += ResourceBlock.Size;
        //    Size += TexcoordData.Size;
        //    Size += ResourceBlock.Size;
        //    Size += VectorData.Size;
        //    Size += ResourceBlock.Size;
        //    Size += BoneData.Size == 1 ? 4 : BoneData.Size;
        //    Size += ResourceFooter.Size;

        //    byte[] Buffer = new byte[Size];

        //    MemoryStream Stream = new MemoryStream(Buffer, null);
        //    Stream.Write(ResourceHeader.Header);
        //    Stream.Write(ToByteArray(Header));
        //    Stream.Write(ResourceBlock.Header);
        //    foreach (MeshInformationData m in MeshInformation)
        //        Stream.Write(ToByteArray(m));
        //    Stream.Write(ResourceBlock.Header);
        //    Stream.Write(ToByteArray(IndiceData));
        //    Stream.Write(ResourceBlock.Header);
        //    foreach (UnknownStruct32Data u in Struct32)
        //        Stream.Write(ToByteArray(u));
        //    Stream.Write(ResourceBlock.Header);
        //    Stream.Write(ToByteArray(PositionData));
        //    Stream.Write(ResourceBlock.Header);
        //    Stream.Write(ToByteArray(TexcoordData));
        //    Stream.Write(ResourceBlock.Header);
        //    Stream.Write(ToByteArray(VectorData));
        //    Stream.Write(ResourceBlock.Header);
        //    Stream.Write(ToByteArray(BoneData));
        //    Stream.Write(ResourceFooter.Header);

        //    return Buffer;
        //}

        static public byte[] ToByteArray(object o)
        {
            FieldInfo[] Fields = o.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            MemoryStream Buffer = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(Buffer);
            foreach (FieldInfo f in Fields)
            {
                if (f.Name == "Size") { }
                else if (f.FieldType == typeof(uint))
                    bw.Write((uint)f.GetValue(o));
                else if (f.FieldType == typeof(byte[]))
                    bw.Write((byte[])f.GetValue(o));
                else if (f.FieldType == typeof(H2Model.Range))
                    bw.Write(((H2Model.Range)(f.GetValue(o))).ToByteArray());
                else if (f.FieldType == typeof(short))
                    bw.Write((short)(f.GetValue(o)));
                else if (f.FieldType == typeof(float))
                    bw.Write((float)(f.GetValue(o)));
                else if (f.FieldType == typeof(short[]))
                {
                    short[] t = (short[])f.GetValue(o);
                    foreach (short s in t)
                    {
                        bw.Write(s);
                    }
                }
                else if (f.FieldType == typeof(int[]))
                {
                    int[] t = (int[])f.GetValue(o);
                    foreach (int i in t)
                    {
                        bw.Write(i);
                    }
                }
            }
            return Buffer.ToArray();
        }

        //public int OffsetOf(Type t)
        //{
        //    return Offset[t];
        //}
    }

    public class RawHeaderData : RawData
    {
        public const uint Size = 116;

        public uint RawSize;
        public uint MeshInformationCount;
        private byte[] Zero1 = new byte[4];
        public uint UnknownStruct8Count;
        private byte[] Zero2 = new byte[20];
        public uint TriangleStripIndiceCount;
        private byte[] Zero3 = new byte[20];
        public uint UnknownStruct32Count;
        private byte[] Zero4 = new byte[40];
        public uint BoneMapCount;
        private byte[] Zero5 = new byte[8];

        public RawHeaderData(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            RawSize = br.ReadUInt32();
            MeshInformationCount = br.ReadUInt32();
            Zero1 = br.ReadBytes(4);
            UnknownStruct8Count = br.ReadUInt32();
            Zero2 = br.ReadBytes(20);
            TriangleStripIndiceCount = br.ReadUInt32();
            Zero3 = br.ReadBytes(20);
            UnknownStruct32Count = br.ReadUInt32();
            Zero4 = br.ReadBytes(40);
            BoneMapCount = br.ReadUInt32();
            Zero5 = br.ReadBytes(8);
        }
    }

    public class MeshInformationData : RawData
    {
        public const uint Size = 72;

        public short UnknownEnum1;
        public short UnknownEnum2;
        public short ShaderIndex;
        public short IndiceStart;
        public short IndiceLength;
        public byte[] UnknownMiddle;
        public short UnknownEnum3;
        public byte[] UnknownEnd;
        public float UnknownBound1;
        public float UnknownBound2;
        public float UnknownBound3;
        public float UnknownBound4;
        public H2Model.Range X;
        public H2Model.Range Y;
        public H2Model.Range Z;

        public MeshInformationData(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            UnknownEnum1 = br.ReadInt16();
            UnknownEnum2 = br.ReadInt16();
            ShaderIndex = br.ReadInt16();
            IndiceStart = br.ReadInt16();
            IndiceLength = br.ReadInt16();
            UnknownMiddle = br.ReadBytes(4);
            UnknownEnum3 = br.ReadInt16();
            UnknownEnd = br.ReadBytes(16);
            UnknownBound1 = br.ReadSingle();
            UnknownBound2 = br.ReadSingle();
            UnknownBound3 = br.ReadSingle();
            UnknownBound4 = br.ReadSingle();
            X = new H2Model.Range(br.ReadSingle(), br.ReadSingle());
            Y = new H2Model.Range(br.ReadSingle(), br.ReadSingle());
            Z = new H2Model.Range(br.ReadSingle(), br.ReadSingle());
        }
    }

    public class UnknownStruct8Data : RawData
    {
        public const uint Size = 8;

        public byte[] UnknownStructData;

        public UnknownStruct8Data()
        { UnknownStructData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00 }; }

        public UnknownStruct8Data(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            UnknownStructData = br.ReadBytes(8);
        }
    }

    public class UnknownStruct32Data : RawData
    {
        public const uint Size = 32;

        public short HeaderValue;
        public byte[] UnknownStructData;

        public UnknownStruct32Data(short header)
        {
            HeaderValue = header;
            UnknownStructData = new byte[30];
        }

        public UnknownStruct32Data(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            HeaderValue = br.ReadInt16();
            UnknownStructData = br.ReadBytes(30);
        }
    }

    public class TriangleIndiceData : RawData
    {
        public uint Size;

        public short[] IndiceArray;

        public TriangleIndiceData(short[] indices)
        {
            IndiceArray = indices;
            Size = (uint)indices.Length * 2;
        }

        public TriangleIndiceData(Stream stream, uint length)
        {
            uint Count = (uint)(length / 2);
            Size = length;

            IndiceArray = new short[Count];

            BinaryReader br = new BinaryReader(stream);
            for (int x = 0; x < Count; x++)
            {
                IndiceArray[x] = br.ReadInt16();
            }
        }

        public Face[] ToFaceArray(MeshInformationData[] meshdata)
        {
            List<Face> FaceArray = new List<Face>(IndiceArray.Length);

            for (int SubMesh = 0; SubMesh < meshdata.Length; SubMesh++)
            {
                int Start = meshdata[SubMesh].IndiceStart;
                int End = meshdata[SubMesh].IndiceStart + meshdata[SubMesh].IndiceLength - 2;
                bool Winding = true;

                for (int x = Start; x < End; x++)
                {
                    Face Temp = new Face(IndiceArray[x], IndiceArray[x + 1], IndiceArray[x + 2], IndiceArray[x], IndiceArray[x + 1], IndiceArray[x + 2], IndiceArray[x], IndiceArray[x + 1], IndiceArray[x + 2]);
                    Temp.MaterialID = meshdata[SubMesh].ShaderIndex;
                    Temp.GroupID = SubMesh;
                    if (!Temp.IsDegenerate)
                    {
                        FaceArray.Add(Temp);
                        if (Winding == false)
                        {
                            short y = Temp.VertexIndices[1];
                            short z = Temp.VertexIndices[2];
                            Temp.VertexIndices[1] = z;
                            Temp.VertexIndices[2] = y;
                            y = Temp.TexcoordIndices[1];
                            z = Temp.TexcoordIndices[2];
                            Temp.TexcoordIndices[1] = z;
                            Temp.TexcoordIndices[2] = y;
                            y = Temp.NormalIndices[1];
                            z = Temp.NormalIndices[2];
                            Temp.NormalIndices[1] = z;
                            Temp.NormalIndices[2] = y;
                            Winding = true;
                        }
                        else
                        {
                            Winding = false;
                        }

                    }
                    else
                    {
                        if (Winding == false) { Winding = true; }
                        else { Winding = false; }
                    }
                }
            }
            FaceArray.TrimExcess();
            return FaceArray.ToArray();
        }

        public Face[][] ToGroupFaceArray(MeshInformationData[] meshdata)
        {
            Face[][] GroupFaceArray = new Face[meshdata.Length][];

            for (int SubMesh = 0; SubMesh < meshdata.Length; SubMesh++)
            {
                List<Face> FaceArray = new List<Face>(meshdata[0].IndiceLength);

                int Start = meshdata[SubMesh].IndiceStart;
                int End = meshdata[SubMesh].IndiceStart + meshdata[SubMesh].IndiceLength - 2;
                bool Winding = true;

                int Index = 0;
                for (int x = Start; x < End; x++)
                {
                    Face Temp = new Face(IndiceArray[x], IndiceArray[x + 1], IndiceArray[x + 2]);
                    if (!Temp.IsDegenerate)
                    {
                        FaceArray.Add(Temp);
                        if (Winding == false)
                        {
                            short y = Temp.VertexIndices[1];
                            short z = Temp.VertexIndices[2];
                            Temp.VertexIndices[1] = z;
                            Temp.VertexIndices[2] = y;
                            Winding = true;
                        }
                        else
                        {
                            Winding = false;
                        }

                    }
                    else
                    {
                        if (Winding == false) { Winding = true; }
                        else { Winding = false; }
                    }
                    Index++;
                }
                FaceArray.TrimExcess();
                GroupFaceArray[SubMesh] = FaceArray.ToArray();
            }
            return GroupFaceArray;
        }
    }

    public class VertexData : RawData
    {
        public uint Size;

        public byte[] RawVertexData;
        //public Compression Compression;
        //public VertexType Usage;

        //public VertexData()
        //{
        //    Size = 0;
        //    Usage = VertexType.Rigid;
        //    Compression = Compression.SecondaryTexcoord;
        //    RawVertexData = new byte[0];
        //}

        public VertexData(Stream stream, uint size)
        {
            Size = size;
            BinaryReader br = new BinaryReader(stream);
            RawVertexData = br.ReadBytes((int)size);
        }

        //public VertexData(Vertex[] vertices, Compression c, VertexType t, Soffish.Model.BoundingBox b)
        //{
        //    int DataSize = 12;
        //    if ((c & Compression.Position) == Compression.Position) DataSize = 6;
        //    int Stride = 0;
        //    switch (t)
        //    {
        //        case VertexType.Rigid:
        //            Stride = 0;
        //            break;
        //        case VertexType.RigidBoned:
        //            Stride = 2;
        //            break;
        //        case VertexType.Skinned:
        //            Stride = 6;
        //            break;
        //        default:
        //            Stride = 0;
        //            break;
        //    }
        //    Size = (uint)(vertices.Length * (DataSize + Stride));
        //    List<byte> RawData = new List<byte>((int)Size);
        //    foreach (Vertex v in vertices)
        //    {
        //        short test = Compress(v.Position.X, b.X);
        //        float test2 = Decompress(test, b.X);
        //        RawData.AddRange(BitConverter.GetBytes(test));
        //        RawData.AddRange(BitConverter.GetBytes(Compress(v.Position.Y, b.Y)));
        //        RawData.AddRange(BitConverter.GetBytes(Compress(v.Position.Z, b.Z)));
        //    }
        //    RawVertexData = RawData.ToArray();
        //}

        //private short Compress(float value, H2Model.Range r)
        //{
        //    return (short)((((value - r.Min) / (r.Max - r.Min)) * Soffish.Model.BoundingBox.FullRatio) - Soffish.Model.BoundingBox.HalfRatio);
        //}

        //private float Decompress(short value, H2Model.Range r)
        //{
        //    double percent = (value + 32768F) / 65535F;
        //    double result = (percent * ((double)r.Max - (double)r.Min)) + (double)r.Min;
        //    return (float)result;
        //}

        //public Vertex[] ToVertexArray(Soffish.Model.BoundingBox boundingbox)
        //{
        //    Vertex[] Vertices;

        //    int Index = 0;

        //    int Stride = CalculateStride();

        //    if ((Compression & Compression.Position) != Compression.Position)
        //    {
        //        int VertexSize = 12 + Stride;
        //        int Count = RawVertexData.Length / VertexSize;
        //        Index = 0;
        //        Vertices = new Vertex[Count];

        //        for (int i = 0; i < Count; i++)
        //        {
        //            float x = BitConverter.ToSingle(RawVertexData, Index);
        //            float y = BitConverter.ToSingle(RawVertexData, Index + 4);
        //            float z = BitConverter.ToSingle(RawVertexData, Index + 8);
        //            Vertices[i] = new Vertex(new Vector3(x, y, z), Usage, Compression);
        //            Index += VertexSize;
        //        }
        //        return Vertices;
        //    }
        //    else
        //    {
        //        int VertexSize = 6 + Stride;
        //        int Count = RawVertexData.Length / VertexSize;
        //        Index = 0;
        //        Vertices = new Vertex[Count];

        //        for (int i = 0; i < Count; i++)
        //        {
        //            float x = (((BitConverter.ToInt16(RawVertexData, Index) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.X.Max - boundingbox.X.Min)) + boundingbox.X.Min;
        //            float y = (((BitConverter.ToInt16(RawVertexData, Index + 2) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.Y.Max - boundingbox.Y.Min)) + boundingbox.Y.Min;
        //            float z = (((BitConverter.ToInt16(RawVertexData, Index + 4) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.Z.Max - boundingbox.Z.Min)) + boundingbox.Z.Min;
        //            Vertices[i] = new Vertex(new Vector3(x, y, z), Usage, Compression);
        //            Index += VertexSize;
        //        }
        //        return Vertices;
        //    }
        //}

        //public void RecompressRaw(Soffish.Model.BoundingBox oldbounds, Soffish.Model.BoundingBox newbounds)
        //{
        //    if ((Compression & Compression.Position) != Compression.Position) { return; }

        //    int Stride = CalculateStride();
        //    int VertexSize = 6 + Stride;
        //    int Count = RawVertexData.Length / VertexSize;
        //    int Index = 0;
        //    List<byte> NewRawData = new List<byte>(RawVertexData.Length);
        //    for (int i = 0; i < Count; i++)
        //    {
        //        float x = Decompress(BitConverter.ToInt16(RawVertexData, Index), oldbounds.X);
        //        float y = Decompress(BitConverter.ToInt16(RawVertexData, Index + 2), oldbounds.Y);
        //        float z = Decompress(BitConverter.ToInt16(RawVertexData, Index + 4), oldbounds.Z);
        //        byte[] other = new byte[VertexSize - 6];
        //        Array.Copy(RawVertexData, Index + 6, other, 0, other.Length);
        //        short cx = Compress(x, newbounds.X);
        //        short cy = Compress(x, newbounds.Y);
        //        short cz = Compress(x, newbounds.Z);
        //        NewRawData.AddRange(BitConverter.GetBytes(cx));
        //        NewRawData.AddRange(BitConverter.GetBytes(cy));
        //        NewRawData.AddRange(BitConverter.GetBytes(cz));
        //        NewRawData.AddRange(other);
        //        Index += VertexSize;
        //    }
        //    RawVertexData = NewRawData.ToArray();
        //}

        //private int CalculateStride()
        //{
        //    int Stride;
        //    switch (Usage)
        //    {
        //        case VertexType.Rigid: Stride = 0;
        //            break;
        //        case VertexType.RigidBoned: Stride = 2;
        //            break;
        //        case VertexType.Skinned: Stride = 6;
        //            break;
        //        default: Stride = 0;
        //            break;
        //    }
        //    return Stride;
        //}

        //public Vector3[] ToVector3Array(Soffish.Model.BoundingBox boundingbox)
        //{
        //    Vector3[] Vertices;

        //    int Index = 0;

        //    int Stride = CalculateStride();

        //    if ((Compression & Compression.Position) != Compression.Position)
        //    {
        //        int VertexSize = 12 + Stride;
        //        int Count = RawVertexData.Length / VertexSize;
        //        Index = 0;
        //        Vertices = new Vector3[Count];

        //        for (int i = 0; i < Count; i++)
        //        {
        //            float x = BitConverter.ToSingle(RawVertexData, Index);
        //            float y = BitConverter.ToSingle(RawVertexData, Index + 4);
        //            float z = BitConverter.ToSingle(RawVertexData, Index + 8);
        //            Vertices[i] = new Vector3(x, y, z);
        //            Index += VertexSize;
        //        }
        //        return Vertices;
        //    }
        //    else
        //    {
        //        int VertexSize = 6 + Stride;
        //        int Count = RawVertexData.Length / VertexSize;
        //        Index = 0;
        //        Vertices = new Vector3[Count];

        //        for (int i = 0; i < Count; i++)
        //        {
        //            float x = (((BitConverter.ToInt16(RawVertexData, Index) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.X.Max - boundingbox.X.Min)) + boundingbox.X.Min;
        //            float y = (((BitConverter.ToInt16(RawVertexData, Index + 2) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.Y.Max - boundingbox.Y.Min)) + boundingbox.Y.Min;
        //            float z = (((BitConverter.ToInt16(RawVertexData, Index + 4) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.Z.Max - boundingbox.Z.Min)) + boundingbox.Z.Min;
        //            Vertices[i] = new Vector3(x, y, z);
        //            Index += VertexSize;
        //        }
        //        return Vertices;
        //    }
        //}
    }

    public class TexcoordData : RawData
    {
        public uint Size;

        public byte[] RawUVData;
        //public Compression Usage;

        //public TexcoordData()
        //{
        //    Size = 0;
        //    RawUVData = new byte[0];
        //    Usage = Compression.SecondaryTexcoord;
        //}

        public TexcoordData(Stream stream, uint size)
        {
            Size = size;
            BinaryReader br = new BinaryReader(stream);
            RawUVData = br.ReadBytes((int)size);
        }

        //public TexcoordData(Texcoord[] texcoords, Compression c, Soffish.Model.BoundingBox b)
        //{
        //    int DataSize = 8;
        //    if ((c & Compression.Texcoord) == Compression.Texcoord) DataSize = 4;

        //    Size = (uint)(texcoords.Length * DataSize);
        //    List<byte> rawData = new List<byte>();

        //    foreach (Texcoord t in texcoords)
        //    {
        //        rawData.AddRange(BitConverter.GetBytes(Compress(t.Coordinates.X, b.U)));
        //        rawData.AddRange(BitConverter.GetBytes(Compress(t.Coordinates.Y, b.V)));
        //    }
        //    RawUVData = rawData.ToArray();
        //}

        //private short Compress(float p, Soffish.Model.Range range)
        //{
        //    return (short)((((p - range.Min) / (range.Max - range.Min)) * Soffish.Model.BoundingBox.FullRatio) - Soffish.Model.BoundingBox.HalfRatio);
        //}

        //private float Decompress(short value, Soffish.Model.Range r)
        //{
        //    double percent = (value + 32768F) / 65535F;
        //    double result = (percent * ((double)r.Max - (double)r.Min)) + (double)r.Min;
        //    return (float)result;
        //}

        //public Texcoord[] ToTextureVertex(Soffish.Model.BoundingBox boundingbox)
        //{
        //    Texcoord[] Vertices;
        //    int Index = 0;
        //    if ((Usage & Compression.Texcoord) == Compression.Texcoord)
        //    {
        //        int TexcoordSize = 4;
        //        List<Texcoord> AllVertices = new List<Texcoord>(RawUVData.Length / TexcoordSize);
        //        int Count = RawUVData.Length / TexcoordSize;
        //        Vertices = new Texcoord[Count];
        //        for (int i = 0; i < Count; i++)
        //        {
        //            float u = (((BitConverter.ToInt16(RawUVData, Index) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.U.Max - boundingbox.U.Min)) + boundingbox.U.Min;
        //            float v = (((BitConverter.ToInt16(RawUVData, Index + 2) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.V.Max - boundingbox.V.Min)) + boundingbox.V.Min;
        //            Vertices[i] = new Texcoord(new Vector2(u, v), Usage);
        //            Index += TexcoordSize;
        //        }
        //        return Vertices;
        //    }
        //    else
        //    {
        //        int TexcoordSize = 8;
        //        int Count = RawUVData.Length / TexcoordSize;
        //        Vertices = new Texcoord[Count];
        //        for (int i = 0; i < Count; i++)
        //        {
        //            float u = BitConverter.ToSingle(RawUVData, Index);
        //            float v = BitConverter.ToSingle(RawUVData, Index + 4);
        //            Vertices[i] = new Texcoord(new Vector2(u, v), Usage);
        //            Index += TexcoordSize;
        //        }
        //        return Vertices;
        //    }
        //}

        //public void RecompressRaw(Soffish.Model.BoundingBox oldbounds, Soffish.Model.BoundingBox newbounds)
        //{
        //    if ((Usage & Compression.Texcoord) != Compression.Texcoord) { return; }

        //    int TexcoordSize = 4;
        //    int Count = RawUVData.Length / TexcoordSize;
        //    int Index = 0;
        //    List<byte> NewRawData = new List<byte>(RawUVData.Length);
        //    for (int i = 0; i < Count; i++)
        //    {
        //        float u = Decompress(BitConverter.ToInt16(RawUVData, Index), oldbounds.U);
        //        float v = Decompress(BitConverter.ToInt16(RawUVData, Index + 2), oldbounds.V);
        //        short cu = Compress(u, newbounds.U);
        //        short cv = Compress(v, newbounds.V);
        //        NewRawData.AddRange(BitConverter.GetBytes(cu));
        //        NewRawData.AddRange(BitConverter.GetBytes(cv));
        //        Index += TexcoordSize;
        //    }
        //    RawUVData = NewRawData.ToArray();
        //}

        //public Vector2[] ToVector2Array(Soffish.Model.BoundingBox boundingbox)
        //{
        //    Vector2[] Vertices;
        //    int Index = 0;
        //    if ((Usage & Compression.Texcoord) == Compression.Texcoord)
        //    {
        //        int TexcoordSize = 4;
        //        List<Texcoord> AllVertices = new List<Texcoord>(RawUVData.Length / TexcoordSize);
        //        int Count = RawUVData.Length / TexcoordSize;
        //        Vertices = new Vector2[Count];
        //        for (int i = 0; i < Count; i++)
        //        {
        //            float u = (((BitConverter.ToInt16(RawUVData, Index) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.U.Max - boundingbox.U.Min)) + boundingbox.U.Min;
        //            float v = (((BitConverter.ToInt16(RawUVData, Index + 2) + Soffish.Model.BoundingBox.HalfRatio) / Soffish.Model.BoundingBox.FullRatio) * (boundingbox.V.Max - boundingbox.V.Min)) + boundingbox.V.Min;
        //            Vertices[i] = new Vector2(u, v);
        //            Index += TexcoordSize;
        //        }
        //        return Vertices;
        //    }
        //    else
        //    {
        //        int TexcoordSize = 8;
        //        int Count = RawUVData.Length / TexcoordSize;
        //        Vertices = new Vector2[Count];
        //        for (int i = 0; i < Count; i++)
        //        {
        //            float u = BitConverter.ToSingle(RawUVData, Index);
        //            float v = BitConverter.ToSingle(RawUVData, Index + 4);
        //            Vertices[i] = new Vector2(u, v);
        //            Index += TexcoordSize;
        //        }
        //        return Vertices;
        //    }
        //}
    }

    public class Texcoord : RawData
    {
        Compression Usage;

        public Vector2 Coordinates;

        public Texcoord(Vector2 data, Compression usage)
        {
            Usage = usage;
            Coordinates.X = data.X;
            Coordinates.Y = data.Y;
        }

        public override string ToString()
        {
            return "vt " + Coordinates.X.ToString() + " " + Coordinates.Y.ToString();
        }
    }

    public class VectorData : RawData
    {
        public uint Size;

        int[] RawVectorData;

        public VectorData()
        {
            Size = 0;
            RawVectorData = new int[0];
        }

        public VectorData(int vertexcount, Vertex[] vertices, Vector[] normals, Texcoord[] texcoords, int trianglecount, Face[] triangles)
        {
            Vector[] Tangents, Bitangents;

            CalculateTangentArray(vertexcount, vertices, normals, texcoords, trianglecount, triangles, out Bitangents, out normals, out Tangents);

            Size = (uint)(normals.Length * 3 * 4);
            List<int> RawData = new List<int>((int)Size);
            for (int Index = 0; Index < normals.Length; Index++)
            {
                RawData.Add(Compress(ref normals[Index]));
                RawData.Add(Compress(ref Bitangents[Index]));
                RawData.Add(Compress(ref Tangents[Index]));
            }
            RawVectorData = RawData.ToArray();
        }

        private int Compress(ref Vector vector)
        {
            int X_sign = vector.Components.X < 0 ? 1 : 0;
            int Y_sign = vector.Components.Y < 0 ? 1 : 0;
            int Z_sign = vector.Components.Z < 0 ? 1 : 0;
            vector.Components.X = vector.Components.X < 0 ? (1 + vector.Components.X) : vector.Components.X;
            vector.Components.Y = vector.Components.Y < 0 ? (1 + vector.Components.Y) : vector.Components.Y;
            vector.Components.Z = vector.Components.Z < 0 ? (1 + vector.Components.Z) : vector.Components.Z;
            int X = (int)(vector.Components.X * 0x3FF);
            int Y = (int)(vector.Components.Y * 0x3FF);
            int Z = (int)(vector.Components.Z * 0x1FF);

            int integer = (Z_sign << 31) | (Z << 22) | (Y_sign << 21) | (Y << 11) | (X_sign << 10) | X;

            return integer;
        }

        public VectorData(Stream stream, uint size)
        {
            uint Count = size / 4;

            Size = size;

            RawVectorData = new int[Count];

            BinaryReader br = new BinaryReader(stream);
            for (int x = 0; x < Count; x++)
            {
                RawVectorData[x] = br.ReadInt32();
            }
        }

        private void CalculateTangentArray(int vertexCount, Vertex[] vertex, Vector[] normal, Texcoord[] texcoord,
            int triangleCount, Face[] triangles, out Vector[] Bitangent, out Vector[] Normals, out Vector[] Tangent)
        {
            Vector3[] tan1 = new Vector3[vertexCount];
            Vector3[] tan2 = new Vector3[vertexCount];
            Normals = new Vector[vertexCount];


            for (int a = 0; a < triangleCount; a++)
            {
                //Store Vertex Index's
                int i1 = triangles[a].VertexIndices[0];
                int i2 = triangles[a].VertexIndices[1];
                int i3 = triangles[a].VertexIndices[2];

                int n1 = triangles[a].NormalIndices[0];
                int n2 = triangles[a].NormalIndices[1];
                int n3 = triangles[a].NormalIndices[2];

                int vt1 = triangles[a].TexcoordIndices[0];
                int vt2 = triangles[a].TexcoordIndices[1];
                int vt3 = triangles[a].TexcoordIndices[2];

                //From Vertex Array Copy Coordinates of Vertex's
                Vertex v1 = vertex[i1];
                Vertex v2 = vertex[i2];
                Vertex v3 = vertex[i3];

                //From Texture Array Copy Coordinates of Vertex's
                Texcoord w1 = texcoord[vt1];
                Texcoord w2 = texcoord[vt2];
                Texcoord w3 = texcoord[vt3];

                //Make Relative to Vertex 1
                //Vertex Coordinates
                float x1 = v2.Position.X - v1.Position.X;
                float x2 = v3.Position.X - v1.Position.X;
                float y1 = v2.Position.Y - v1.Position.Y;
                float y2 = v3.Position.Y - v1.Position.Y;
                float z1 = v2.Position.Z - v1.Position.Z;
                float z2 = v3.Position.Z - v1.Position.Z;
                //Texture Coordinates
                float s1 = w2.Coordinates.X - w1.Coordinates.X;
                float s2 = w3.Coordinates.X - w1.Coordinates.X;
                float t1 = w2.Coordinates.Y - w1.Coordinates.Y;
                float t2 = w3.Coordinates.Y - w1.Coordinates.Y;

                float r = 1.0F / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
                AngleFucker AF = new AngleFucker(v1.Position, v2.Position, v3.Position);

                tan1[i1] += Vector3.Multiply(sdir, AF.Get_v1_Angle());
                tan1[i2] += Vector3.Multiply(sdir, AF.Get_v2_Angle());
                tan1[i3] += Vector3.Multiply(sdir, AF.Get_v3_Angle());

                tan2[i1] += Vector3.Multiply(tdir, AF.Get_v1_Angle());
                tan2[i2] += Vector3.Multiply(tdir, AF.Get_v2_Angle());
                tan2[i3] += Vector3.Multiply(tdir, AF.Get_v3_Angle());
            }

            Tangent = new Vector[vertexCount];
            Bitangent = new Vector[vertexCount];

            for (int a = 0; a < vertexCount; a++)
            {
                Vector3 n = normal[a].Components;
                Normals[a] = Round(normal[a]);
                Vector3 b = tan2[a];
                Vector3 t = tan1[a];

                // Gram-Schmidt orthogonalize
                Vector3 temp = Vector3.Subtract(t, Vector3.Multiply(n, Vector3.Dot(n, t)));
                temp.Normalize();

                Tangent[a] = new Vector(Vector3.Empty, Vector.VectorUsage.Tangent);
                Tangent[a].Components = temp;
                Tangent[a] = Round(Tangent[a]);

                // Calculate handedness
                float W = (Vector3.Dot(Vector3.Cross(n, t), b) < 0.0F) ? -1.0F : 1.0F;

                //Calculate Bitangent
                Bitangent[a] = new Vector(Vector3.Empty, Vector.VectorUsage.Bitangent);
                Bitangent[a].Components = Vector3.Multiply((Vector3.Cross(n, temp)), W);
                Bitangent[a] = Round(Bitangent[a]);
            }

        }

        private Vector Round(Vector vector)
        {
            vector.Components.X = (float)Math.Round((double)vector.Components.X, 5);
            vector.Components.Y = (float)Math.Round((double)vector.Components.Y, 5);
            vector.Components.Z = (float)Math.Round((double)vector.Components.Z, 5);
            return vector;
        }

        public float CalculateAngleFromFace(Vector3 _v1, Vector3 _v2)
        {
            float _Angle = (float)Math.Acos(Vector3.Dot(_v1, _v2));
            return _Angle;
        }

        public class AngleFucker
        {
            Vector3 v1;
            Vector3 v2;
            Vector3 v3;
            float v1Angle;
            float v2Angle;
            float v3Angle;
            public AngleFucker(Vector3 _v1, Vector3 _v2, Vector3 _v3)
            {
                v1 = _v1;
                v2 = _v2;
                v3 = _v3;

            }
            public float Get_v1_Angle()
            {
                Vector3 t1 = Vector3.Subtract(v2, v1);
                t1.Normalize();
                Vector3 t2 = Vector3.Subtract(v3, v1);
                t2.Normalize();
                v1Angle = (float)Math.Acos(Vector3.Dot(t1, t2));
                return v1Angle;
            }
            public float Get_v2_Angle()
            {
                Vector3 t1 = Vector3.Subtract(v1, v2);
                t1.Normalize();
                Vector3 t2 = Vector3.Subtract(v3, v2);
                t2.Normalize();
                v2Angle = (float)Math.Acos(Vector3.Dot(t1, t2));
                return v2Angle;
            }
            public float Get_v3_Angle()
            {
                Vector3 t1 = Vector3.Subtract(v2, v3);
                t1.Normalize();
                Vector3 t2 = Vector3.Subtract(v1, v3);
                t2.Normalize();
                v3Angle = (float)Math.Acos(Vector3.Dot(t1, t2));
                return v3Angle;
            }
        }

        public Vector[] ToVectorArray()
        {
            Vector[] Vectors = new Vector[RawVectorData.Length];

            for (int Count = 0; Count < RawVectorData.Length; Count++)
            {
                byte xSign = (byte)((RawVectorData[Count] >> 10) & 0x1);
                byte ySign = (byte)((RawVectorData[Count] >> 21) & 0x1);
                byte zSign = (byte)((RawVectorData[Count] >> 31) & 0x1);
                float x = (int)((RawVectorData[Count]) & 0x3FF);
                x /= (float)0x3FF;
                float y = (float)((RawVectorData[Count] >> 11) & 0x3FF);
                y /= (float)0x3FF;
                float z = (float)((RawVectorData[Count] >> 22) & 0x1FF);
                z /= (float)0x1FF;
                z = zSign == 1 ? (z - 1) : z;
                y = ySign == 1 ? (y - 1) : y;
                x = xSign == 1 ? (x - 1) : x;

                Vectors[Count] = new Vector(new Vector3(x, y, z), (Vector.VectorUsage)(Count % 3));
            }
            return Vectors;
        }
    }

    public class BoneData : RawData
    {
        public uint Size;

        byte[] RawBoneData;

        public BoneData()
        {
            Size = 1;
            RawBoneData = new byte[4];
        }

        public BoneData(Stream stream, uint size)
        {
            uint Count = size;

            Size = size;

            BinaryReader br = new BinaryReader(stream);
            RawBoneData = br.ReadBytes((int)Count);
        }
    }

    #endregion

    #region Structs
    public struct Vector
    {
        public VectorUsage Usage;

        public Vector3 Components;

        public enum VectorUsage
        {
            Normal = 0,
            Bitangent = 1,
            Tangent = 2,
        }

        public Vector(Vector3 data, VectorUsage usage)
        {
            Usage = usage;
            Components.X = data.X;
            Components.Y = data.Y;
            Components.Z = data.Z;
        }

        public override string ToString()
        {
            switch (Usage)
            {
                case VectorUsage.Normal:
                    return "vn " + Components.X.ToString() + " " + Components.Y.ToString() + " " + Components.Z.ToString();
                default:
                    return "# " + Components.X.ToString() + " " + Components.Y.ToString() + " " + Components.Z.ToString();
            }
        }
    }

    public struct Vertex
    {
        Compression Compression;
        VertexType Usage;

        public Vector3 Position;

        public Vertex(Vector3 data, VertexType t, Compression c)
        {
            Usage = t;
            Compression = c;
            Position.X = data.X;
            Position.Y = data.Y;
            Position.Z = data.Z;
        }

        public override string ToString()
        {
            return "v " + Position.X.ToString() + " " + Position.Y.ToString() + " " + Position.Z.ToString();
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
    #endregion

    #region Abstract

    public abstract class RawData
    {
        virtual public void Save(BinaryWriter bw)
        {
            bw.Write(ModelRawData.ToByteArray(this));
        }
    }

    #endregion
}
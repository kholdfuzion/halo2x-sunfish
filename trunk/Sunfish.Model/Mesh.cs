using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using System.IO;
using Microsoft.DirectX.Direct3D;
using System.Collections;

namespace Sunfish.Mode
{
    public class Mesh
    {
        int Size;
        public Group[] Groups;
        public short[] Indices;
        public Vector3[] Vertices;
        public Vector2[] Texcoords;
        public Vector3[] Normals;
        public Vector3[] Tangents;
        public Vector3[] Bitangents;
        public Vertex[] Vertexlist
        {
            get
            {
                Vertex[] vl = new Vertex[Vertices.Length];
                for (int i = 0; i < vl.Length; i++)
                    vl[i] = new Vertex(Vertices[i], Microsoft.Xna.Framework.Graphics.Color.Red, Normals[i]);
                return vl;
            }
        }
        byte[] BoneMap;

        public Mesh(Stream stream, Resource[] resources, Section section, BoundingBox boundingBox)
        {
            int StartOffset = (int)stream.Position;
            BinaryReader br = new BinaryReader(stream);
            stream.Position = 4;
            Size = br.ReadInt32();
            foreach (Resource r in resources)
            {
                stream.Position = StartOffset + 120 + r.RawDataOffset;
                int Count=0;
                switch (r.MainRawDataType)
                {
                    case ResourceType.BoneMap:
                        BoneMap = br.ReadBytes(r.RawDataSize);
                        break;
                    case ResourceType.MeshInformation:
                        Groups = new Group[r.RawDataSize / 72];
                        for (int i = 0; i < Groups.Length; i++)
                        {
                            Groups[i] = new Group();
                            stream.Position = StartOffset + 120 + r.RawDataOffset + (i * 72) + 4;
                            Groups[i].ShaderIndex = br.ReadInt16();
                            Groups[i].IndiceStart = br.ReadInt16();
                            Groups[i].IndiceCount = br.ReadInt16();
                        }
                        break;


                    case ResourceType.Vertex:
                        switch (r.SubRawDataType)
                        {
                            #region Vertices

                            case ResourceSubType.VertexData:

                                byte[] Bytes = br.ReadBytes(r.RawDataSize);
                                int Index = 0;

                                int Stride;
                                switch (section.VertexType)
                                {
                                    case VertexType.Rigid: Stride = 0;//xyz
                                        break;
                                    default: throw new Exception();
                                }

                                if ((section.CompressionFlags & Compression.Position) != Compression.Position)
                                {
                                    int VertexSize = 12 + Stride;
                                    int VertexCount = Bytes.Length / VertexSize;
                                    Index = 0;
                                    Vertices = new Vector3[VertexCount];

                                    for (int i = 0; i < VertexCount; i++)
                                    {
                                        float x = BitConverter.ToSingle(Bytes, Index + 0);
                                        float y = BitConverter.ToSingle(Bytes, Index + 4);
                                        float z = BitConverter.ToSingle(Bytes, Index + 8);
                                        Vertices[i] = new Vector3(x, y, z);
                                        Index += VertexSize;
                                    }
                                }
                                else
                                {
                                    int VertexSize = 6 + Stride;
                                    int VertexCount = Bytes.Length / VertexSize;
                                    Index = 0;
                                    Vertices = new Vector3[VertexCount];

                                    for (int i = 0; i < VertexCount; i++)
                                    {
                                        float x = (((BitConverter.ToInt16(Bytes, Index + 0) + BoundingBox.HalfRatio) / BoundingBox.FullRatio) * (boundingBox.X.Max - boundingBox.X.Min)) + boundingBox.X.Min;
                                        float y = (((BitConverter.ToInt16(Bytes, Index + 2) + BoundingBox.HalfRatio) / BoundingBox.FullRatio) * (boundingBox.Y.Max - boundingBox.Y.Min)) + boundingBox.Y.Min;
                                        float z = (((BitConverter.ToInt16(Bytes, Index + 4) + BoundingBox.HalfRatio) / BoundingBox.FullRatio) * (boundingBox.Z.Max - boundingBox.Z.Min)) + boundingBox.Z.Min;
                                        Vertices[i] = new Vector3(x, y, z);
                                        Index += VertexSize;
                                    }
                                }
                                break;

                            #endregion

                            #region Texcoords

                            case ResourceSubType.UVData:
                                Index = 0;
                                Bytes = br.ReadBytes(r.RawDataSize);
                                if ((section.CompressionFlags & Compression.Texcoord) == Compression.Texcoord)
                                {
                                    int TexcoordSize = 4;
                                    Count = Bytes.Length / TexcoordSize;
                                    Texcoords = new Vector2[Count];
                                    for (int i = 0; i < Count; i++)
                                    {
                                        float u = (((BitConverter.ToInt16(Bytes, Index + 0) + BoundingBox.HalfRatio) / BoundingBox.FullRatio) * (boundingBox.U.Max - boundingBox.U.Min)) + boundingBox.U.Min;
                                        float v = (((BitConverter.ToInt16(Bytes, Index + 2) + BoundingBox.HalfRatio) / BoundingBox.FullRatio) * (boundingBox.V.Max - boundingBox.V.Min)) + boundingBox.V.Min;
                                        Texcoords[i] = new Vector2(u, v);
                                        Index += TexcoordSize;
                                    }
                                }
                                else
                                {
                                    int TexcoordSize = 8;
                                    Count = Bytes.Length / TexcoordSize;
                                    Texcoords = new Vector2[Count];
                                    for (int i = 0; i < Count; i++)
                                    {
                                        float u = BitConverter.ToSingle(Bytes, Index);
                                        float v = BitConverter.ToSingle(Bytes, Index + 4);
                                        Texcoords[i] = new Vector2(u, v);
                                        Index += TexcoordSize;
                                    }
                                }
                                break;

                            #endregion

                            #region Normals, Tangents, and Bitangents

                            case ResourceSubType.VectorData:

                                int[] Ints = new int[r.RawDataSize / 4];
                                for (int i = 0; i < Ints.Length; i++)
                                    Ints[i] = br.ReadInt32();

                                Normals = new Vector3[Ints.Length / 3];
                                Tangents = new Vector3[Ints.Length / 3];
                                Bitangents = new Vector3[Ints.Length / 3];

                                int[] normInts = new int[Ints.Length / 3];
                                for (int i = 0; i < normInts.Length; i++)
                                {
                                    normInts[i] = Ints[i * 3];
                                }

                                int normalsCount = 0;
                                int tangentsCount = 0;
                                int bitangentsCount = 0;
                                for (int i = 0; i < normInts.Length; i++)
                                {
                                    int CompressedData = normInts[i];
                                    int x10 = (CompressedData & 0x000007FF);
                                    if ((x10 & 0x00000400) == 0x00000400) { 
                                        x10 = -((~x10) & 0x000007FF);
                                        if (x10 == 0) x10 = -1;
                                    }
                                    int y11 = (CompressedData >> 11) & 0x000007FF;
                                    if ((y11 & 0x00000400) == 0x00000400)
                                    {
                                        y11 = -((~y11) & 0x000007FF);
                                        if (y11 == 0) y11 = -1;
                                    }
                                    int z11 = (CompressedData >> 22) & 0x000003FF;
                                    if ((z11 & 0x00000200) == 0x00000200)
                                    {
                                        z11 = -((~z11) & 0x000003FF);
                                        if (z11 == 0) z11 = -1;
                                    }
                                    float x, y, z;
                                    x = -(x10 / (float)0x000003ff);
                                    y = -(y11 / (float)0x000003FF);
                                    z = -(z11 / (float)0x000001FF);
                                    int o = 0;
                                    //z /= (float)0x000007FF;
                                    //byte xSign = (byte)((Ints[i] >> 10) & 0x1);
                                    //byte ySign = (byte)((Ints[i] >> 21) & 0x1);
                                    //byte zSign = (byte)((Ints[i] >> 31) & 0x1);
                                    //float x = (int)((Ints[i]) & 0x3FF);
                                    //x /= (float)0x3FF;
                                    //float y = (float)((Ints[i] >> 11) & 0x3FF);
                                    //y /= (float)0x3FF;
                                    //float z = (float)((Ints[i] >> 22) & 0x1FF);
                                    //z /= (float)0x1FF;
                                    //z = zSign == 1 ? (z - 1) : z;
                                    //y = ySign == 1 ? (y - 1) : y;
                                    //x = xSign == 1 ? (x - 1) : x;

                                    Normals[normalsCount] = new Vector3(x, y, z);
                                    Normals[normalsCount].Multiply(-1.0f);
                                    Normals[normalsCount].Normalize();
                                    normalsCount++;

                                    //switch (i % 3)
                                    //{
                                    //    case 0:
                                    //        Normals[normalsCount] = new Vector3(x, y, z);
                                    //        normalsCount++;
                                    //        break;
                                    //    case 1:
                                    //        Tangents[tangentsCount] = new Vector3(x, y, z);
                                    //        tangentsCount++;
                                    //        break;
                                    //    case 2:
                                    //        Bitangents[bitangentsCount] = new Vector3(x, y, z);
                                    //        bitangentsCount++;
                                    //        break;
                                    //}
                                }
                                //Normals = GenerateNormals(Vertices, Indices);
                                break;

                            #endregion
                        }
                        break;

                    #region Triangle Strip Indices

                    case ResourceType.TriangleStrip:
                        Count = r.RawDataSize / 2;
                        Indices = new short[Count];
                        for (int x = 0; x < Count; x++)
                        {
                            Indices[x] = br.ReadInt16();
                        }
                        break;

                    #endregion
                }
            }
        }

        private Vector3[] GenerateNormals(Vector3[] vertices, short[] indices)
        {
            //VertexPositionNormalColored[] vertices = new VertexPositionNormalColored[WIDTH * HEIGHT];
            //vb.GetData(vertices);
            //int[] indices = new int[(WIDTH - 1) * (HEIGHT - 1) * 6];
            //ib.GetData(indices);

            Vector3[] normals = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                normals[i] = new Vector3(0, 0, 0);
            bool w = false;
            for (int i = 0; i < indices.Length - 2; i++)
            {
                if (indices[i + 1] == indices[i + 2] ||
                    indices[i + 1] == indices[i] ||
                    indices[i] == indices[i + 2]) continue;
                Vector3 firstvec = vertices[indices[i + 1]] - vertices[indices[i]];
                Vector3 secondvec = vertices[indices[i]] - vertices[indices[i + 2]];
                Vector3 normal;
                if (w)
                    normal = Vector3.Cross(firstvec, secondvec);
                else
                    normal = Vector3.Cross(secondvec, firstvec);
                normal.Normalize();
                normals[indices[i]] += normal;
                normals[indices[i + 1]] += normal;
                normals[indices[i + 2]] += normal;
                w = !w;
            }

            for (int i = 0; i < vertices.Length; i++)
                normals[i].Normalize();

            return normals;
        }

        public byte[] Serialize(Section section, BoundingBox boundingBox, out Resource[] resources)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);

            List<Resource> resourceList = new List<Resource>();
            
            byte[] fourCC = Encoding.UTF8.GetBytes("crsr");

            bw.Write(Encoding.UTF8.GetBytes("hklb"), 0, 4);
            bw.Write(new byte[116]);

            bw.Write(fourCC, 0, 4);
            resourceList.Add(new Resource(ResourceType.MeshInformation, ResourceSubType.MeshInformationData, (int)(stream.Position - 120), 72));
            for (int i = 0; i < Groups.Length; i++)
            {
                Groups[i].Serialize(stream);
            }

            bw.Write(fourCC, 0, 4);
            resourceList.Add(new Resource(ResourceType.UnknownStruct8, ResourceSubType.UnknownStruct8, (int)(stream.Position - 120), 8));
            bw.Write(0x00000000); bw.Write((ushort)0xFFFF); bw.Write((short)0x0000);

            bw.Write(fourCC, 0, 4);
            resourceList.Add(new Resource(ResourceType.TriangleStrip, ResourceSubType.IndiceStripData, (int)(stream.Position - 120), Indices.Length * 2));
            foreach (short i in Indices)
                bw.Write(i);
            bw.Write(Padding.GetBytes(stream.Position, 4));

            bw.Write(fourCC, 0, 4);
            resourceList.Add(new Resource(ResourceType.Unknown, ResourceSubType.UnknownData, (int)(stream.Position - 120), 96));
            bw.Write((byte)0x02); bw.Write((byte)0x06); bw.Write((byte)0x00); bw.Write((byte)0x00);
            bw.Write(new byte[28]);
            bw.Write((byte)0x19); bw.Write((byte)0x04); bw.Write((byte)0x00); bw.Write((byte)0x00);
            bw.Write(new byte[28]);
            bw.Write((byte)0x1B); bw.Write((byte)0x0C); bw.Write((byte)0x00); bw.Write((byte)0x00);
            bw.Write(new byte[28]);

            bw.Write(fourCC, 0, 4);
            resourceList.Add(new Resource(ResourceType.Vertex, ResourceSubType.VertexData, (int)(stream.Position - 120), Vertices.Length * 6));
            foreach (Vector3 v in Vertices)
            {
                bw.Write((short)((((v.X - boundingBox.X.Min) / (boundingBox.X.Max - boundingBox.X.Min)) * BoundingBox.FullRatio) - BoundingBox.HalfRatio));
                bw.Write((short)((((v.Y - boundingBox.Y.Min) / (boundingBox.Y.Max - boundingBox.Y.Min)) * BoundingBox.FullRatio) - BoundingBox.HalfRatio));
                bw.Write((short)((((v.Z - boundingBox.Z.Min) / (boundingBox.Z.Max - boundingBox.Z.Min)) * BoundingBox.FullRatio) - BoundingBox.HalfRatio));
            }

            bw.Write(fourCC, 0, 4);
            resourceList.Add(new Resource(ResourceType.Vertex, ResourceSubType.UVData, (int)(stream.Position - 120), Vertices.Length * 4));
            foreach (Vector2 t in Texcoords)
            {
                bw.Write((short)((((t.X - boundingBox.U.Min) / (boundingBox.U.Max - boundingBox.U.Min)) * BoundingBox.FullRatio) - BoundingBox.HalfRatio));
                bw.Write((short)((((t.Y - boundingBox.V.Min) / (boundingBox.V.Max - boundingBox.V.Min)) * BoundingBox.FullRatio) - BoundingBox.HalfRatio));
            }

            bw.Write(fourCC, 0, 4);
            resourceList.Add(new Resource(ResourceType.Vertex, ResourceSubType.VectorData, (int)(stream.Position - 120), Normals.Length * 4 * 3));
            for (int i = 0; i < Normals.Length; i++)
            {
                bw.Write(CompressVector(Normals[i]));
                bw.Write(CompressVector(Tangents[i]));
                bw.Write(CompressVector(Bitangents[i]));
            }

            bw.Write(fourCC, 0, 4);
            resourceList.Add(new Resource(ResourceType.BoneMap, ResourceSubType.BoneData, (int)(stream.Position - 120), BoneMap.Length));
            foreach (byte b in BoneMap)
                bw.Write(b);
            bw.Write(Padding.GetBytes(stream.Position, 4));

            bw.Write(Encoding.UTF8.GetBytes("fklb"), 0, 4);

            resources = resourceList.ToArray();
            return stream.ToArray();
        }

        public int CompressVector(Vector3 vector)
        {
            int X_sign = vector.X < 0 ? 1 : 0;
            int Y_sign = vector.Y < 0 ? 1 : 0;
            int Z_sign = vector.Z < 0 ? 1 : 0;
            vector.X = vector.X < 0 ? (1 + vector.X) : vector.X;
            vector.Y = vector.Y < 0 ? (1 + vector.Y) : vector.Y;
            vector.Z = vector.Z < 0 ? (1 + vector.Z) : vector.Z;
            int X = (int)(vector.X * 0x3FF);
            int Y = (int)(vector.Y * 0x3FF);
            int Z = (int)(vector.Z * 0x1FF);

            int integer = (Z_sign << 31) | (Z << 22) | (Y_sign << 21) | (Y << 11) | (X_sign << 10) | X;
            return integer;
        }

        public void ExportWavefrontObject(Shader[] shaders)
        {
            WavefrontObject wfo = new WavefrontObject();
            wfo.Vertices = new List<Vector3>( Vertices);
            wfo.VertexCount = Vertices.Length;
            wfo.Texcoords = new List<Vector2>(Texcoords);
            wfo.TexcoordCount = Texcoords.Length;
            wfo.Normals = new List<Vector3>(Normals);
            wfo.NormalCount = Normals.Length;
            wfo.Materials = new Dictionary<string, int>();
            for (int i = 0; i < shaders.Length; i++)
                wfo.Materials.Add("Default_" + i.ToString(), i);
            wfo.GroupCount = Groups.Length;


            List<Face> FaceArray = new List<Face>(Indices.Length);

            for (int i = 0; i < Groups.Length; i++)
            {
                int Start = Groups[i].IndiceStart;
                int End = Groups[i].IndiceStart + Groups[i].IndiceCount - 2;
                bool Winding = true;

                for (int x = Start; x < End; x++)
                {
                    Face Temp = new Face(
                        Indices[x], 
                        Indices[x + 1], 
                        Indices[x + 2], 
                        Indices[x], 
                        Indices[x + 1], 
                        Indices[x + 2], 
                        Indices[x], 
                        Indices[x + 1], 
                        Indices[x + 2]
                        );
                    Temp.MaterialID = Groups[i].ShaderIndex;
                    Temp.GroupID = i;
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
            wfo.Faces = FaceArray;
            wfo.FaceCount = FaceArray.Count;

            Wavefront.CreateWavefrontOBJFile(wfo, "O:\\test.obj");
        }

        public void ImportWavefrontObject(WavefrontObject Wavefront, BoundingBox boundingBox)
        {
            List<D3DVertex> temp = new List<D3DVertex>(Wavefront.FaceCount * 3);
            for (int Material = 0; Material < Wavefront.MaterialCount; Material++)
            {
                int[] FaceIndices = Wavefront.GetFaceIndicesUsingMaterialID(Material);
                for (int Face = 0; Face < FaceIndices.Length; Face++)
                {
                    for (int Component = 0; Component < 3; Component++)
                    {
                        D3DVertex d3DVertex = new D3DVertex();
                        d3DVertex.Position = Wavefront.Vertices[Wavefront.Faces[FaceIndices[Face]].VertexIndices[Component]];
                        d3DVertex.Texture = Wavefront.Texcoords[Wavefront.Faces[FaceIndices[Face]].TexcoordIndices[Component]];
                        d3DVertex.Normal = Wavefront.Normals[Wavefront.Faces[FaceIndices[Face]].NormalIndices[Component]];
                        temp.Add(d3DVertex);
                    }
                }
            }
            //Hashtable D3DVertexHashtable = new Hashtable(Wavefront.FaceCount * 3);
            List<short> tempIndices = new List<short>(Wavefront.FaceCount * 3);
            List<D3DVertex> D3DVertexList = new List<D3DVertex>(Wavefront.FaceCount * 3);
            short IndiceIndex = 0;
            for (int Index = 0; Index < temp.Count; Index++)
            {
                D3DVertex d3DVertex = temp[Index];
                if (!D3DVertexList.Contains(d3DVertex))
                {
                    //D3DVertexHashtable.Add(d3DVertex, d3DVertex);
                    D3DVertexList.Add(d3DVertex);
                    tempIndices.Add(IndiceIndex);
                    IndiceIndex++;
                }
                else
                {
                    tempIndices.Add((short)D3DVertexList.IndexOf(d3DVertex));
                }
            }
            this.Indices = tempIndices.ToArray();
            D3DVertex[] D3DVertices = D3DVertexList.ToArray();

            RenderDevice Device = new RenderDevice();
            Device.InitializeDevice();

            Microsoft.DirectX.Direct3D.Mesh mesh = new Microsoft.DirectX.Direct3D.Mesh(Wavefront.FaceCount, D3DVertices.Length, MeshFlags.SystemMemory, D3DVertex.Format, Device.Device);

            List<int> newAttributes = new List<int>(Wavefront.FaceCount);
            foreach (Face f in Wavefront.Faces)
            {
                newAttributes.Add(f.MaterialID);
            }
            mesh.LockAttributeBuffer(LockFlags.None);
            mesh.UnlockAttributeBuffer(newAttributes.ToArray());
            mesh.SetIndexBufferData(Indices.ToArray(), LockFlags.None);
            mesh.SetVertexBufferData(D3DVertices.ToArray(), LockFlags.None);

            int[] adj = new int[Wavefront.FaceCount * 3];
            mesh.GenerateAdjacency(0.005F, adj);
            mesh.OptimizeInPlace(MeshFlags.OptimizeAttributeSort, adj);
            IndexBuffer iBuffer = mesh.IndexBuffer;

            short[] D3DIndices;
            int IndiceCount;

            short[][] MaterialFaceIndices = new short[Wavefront.MaterialCount][];

            for (int Material = 0; Material < Wavefront.MaterialCount; Material++)
            {
                iBuffer = Microsoft.DirectX.Direct3D.Mesh.ConvertMeshSubsetToSingleStrip(mesh, Material, MeshFlags.SystemMemory, out IndiceCount);
                GraphicsStream graphics = iBuffer.Lock(0, 0, LockFlags.None);
                unsafe
                {
                    short* IndiceArray = (short*)graphics.InternalData.ToPointer();
                    D3DIndices = new short[IndiceCount];
                    for (int Index = 0; Index < IndiceCount; Index++)
                    {
                        D3DIndices[Index] = IndiceArray[Index];
                    }
                }
                MaterialFaceIndices[Material] = D3DIndices;
            }

            List<short> newIndices = new List<short>();
            Groups = new Group[MaterialFaceIndices.Length];
            for (int i = 0; i < MaterialFaceIndices.Length; i++)
            {
                Groups[i] = new Group();
                Groups[i].IndiceStart = (short)newIndices.Count;
                Groups[i].IndiceCount = (short)MaterialFaceIndices[i].Length;
                Groups[i].ShaderIndex = (short)i;
                newIndices.AddRange(MaterialFaceIndices[i]);
            }
            this.Indices = newIndices.ToArray();

            this.Vertices = new Vector3[D3DVertices.Length];
            this.Texcoords = new Vector2[D3DVertices.Length];
            this.Normals = new Vector3[D3DVertices.Length];
            for (int i = 0; i < D3DVertices.Length; i++)
            {
                Vertices[i] = D3DVertices[i].Position;
                Texcoords[i] = D3DVertices[i].Texture;
                Normals[i] = D3DVertices[i].Normal;
            }

            CalculateTangentArray(Vertices.Length, Vertices, Normals, Texcoords, mesh.NumberFaces, Wavefront.Faces.ToArray(), out Bitangents, out Tangents);
            mesh.Dispose();
        }

        private void CalculateTangentArray(
            int vertexCount, Vector3[] vertex, Vector3[] normal, Vector2[] texcoord, int triangleCount, Face[] triangles, 
            out Vector3[] Bitangent, out Vector3[] Tangent)
        {
            Vector3[] tan1 = new Vector3[vertexCount];
            Vector3[] tan2 = new Vector3[vertexCount];
            //Normals = new Vector3[vertexCount];


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
                Vector3 v1 = vertex[i1];
                Vector3 v2 = vertex[i2];
                Vector3 v3 = vertex[i3];

                //From Texture Array Copy Coordinates of Vertex's
                Vector2 w1 = texcoord[vt1];
                Vector2 w2 = texcoord[vt2];
                Vector2 w3 = texcoord[vt3];

                //Make Relative to Vertex 1
                //Vertex Coordinates
                float x1 = v2.X - v1.X;
                float x2 = v3.X - v1.X;
                float y1 = v2.Y - v1.Y;
                float y2 = v3.Y - v1.Y;
                float z1 = v2.Z - v1.Z;
                float z2 = v3.Z - v1.Z;
                //Texture Coordinates
                float s1 = w2.X - w1.X;
                float s2 = w3.X - w1.X;
                float t1 = w2.Y - w1.Y;
                float t2 = w3.Y - w1.Y;

                float r = 1.0F / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
                AngleFucker AF = new AngleFucker(v1, v2, v3);

                tan1[i1] += Vector3.Multiply(sdir, AF.Get_v1_Angle());
                tan1[i2] += Vector3.Multiply(sdir, AF.Get_v2_Angle());
                tan1[i3] += Vector3.Multiply(sdir, AF.Get_v3_Angle());

                tan2[i1] += Vector3.Multiply(tdir, AF.Get_v1_Angle());
                tan2[i2] += Vector3.Multiply(tdir, AF.Get_v2_Angle());
                tan2[i3] += Vector3.Multiply(tdir, AF.Get_v3_Angle());
            }

            Tangent = new Vector3[vertexCount];
            Bitangent = new Vector3[vertexCount];

            for (int a = 0; a < vertexCount; a++)
            {
                Vector3 n = normal[a];
               //Normals[a] = Round(normal[a]);
                Vector3 b = tan2[a];
                Vector3 t = tan1[a];

                // Gram-Schmidt orthogonalize
                Vector3 temp = Vector3.Subtract(t, Vector3.Multiply(n, Vector3.Dot(n, t)));
                temp.Normalize();

                Tangent[a] = Round(temp);

                // Calculate handedness
                float W = (Vector3.Dot(Vector3.Cross(n, t), b) < 0.0F) ? -1.0F : 1.0F;

                //Calculate Bitangent
                Bitangent[a] = Vector3.Multiply((Vector3.Cross(n, temp)), W);
                Bitangent[a] = Round(Bitangent[a]);
            }

        }

        private Vector3 Round(Vector3 vector)
        {
            vector.X = (float)Math.Round((double)vector.X, 5);
            vector.Y = (float)Math.Round((double)vector.Y, 5);
            vector.Z = (float)Math.Round((double)vector.Z, 5);
            return vector;
        }

        private class AngleFucker
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
    }
    public struct Vertex
    {
        Vector3 Position;
        Microsoft.Xna.Framework.Graphics.Color Color;
        Vector3 Normal;

        public Vertex(Vector3 pos, Microsoft.Xna.Framework.Graphics.Color col, Vector3 norm)
        {
            Position = pos;
            Color = col;
            Normal = norm;
        }

        public static int SizeInBytes { get { return 12 + 12 + 4; } }

        //Declares the elements of the custom vertex. 
        //Each vertex stores information on the current 
        //position, color, and normal.
        public static readonly Microsoft.Xna.Framework.Graphics.VertexElement[] VertexElements =
            new Microsoft.Xna.Framework.Graphics.VertexElement[] 
            {
                new Microsoft.Xna.Framework.Graphics.VertexElement(0, 0, Microsoft.Xna.Framework.Graphics.VertexElementFormat.Vector3,
                        Microsoft.Xna.Framework.Graphics.VertexElementMethod.Default,
                        Microsoft.Xna.Framework.Graphics.VertexElementUsage.Position, 0),
                new Microsoft.Xna.Framework.Graphics.VertexElement(0, sizeof(float) * 3,
                        Microsoft.Xna.Framework.Graphics.VertexElementFormat.Color, Microsoft.Xna.Framework.Graphics.VertexElementMethod.Default,
                       Microsoft.Xna.Framework.Graphics.VertexElementUsage.Color, 0),
                new Microsoft.Xna.Framework.Graphics.VertexElement(0, (sizeof(float) * 3) + sizeof(float),
                        Microsoft.Xna.Framework.Graphics.VertexElementFormat.Vector3, Microsoft.Xna.Framework.Graphics.VertexElementMethod.Default,
                       Microsoft.Xna.Framework.Graphics.VertexElementUsage.Normal, 0)     
            };
    }

    public struct Group
    {
        public short ShaderIndex;
        public short IndiceStart;
        public short IndiceCount;

        public void Serialize(Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(new byte[4]);
            bw.Write(ShaderIndex);
            bw.Write(IndiceStart);
            bw.Write(IndiceCount);
            bw.Write(new byte[62]);
        }
    }
}
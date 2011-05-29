using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Sunfish;
using Microsoft.Xna.Framework.Graphics;

namespace Sunfish.Canvas
{
    partial class MainForm
    {
        private void ImportBitmap(Surface Surface)
        {
            LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Width = (short)Surface.Description.Width; 
            LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Height = (short)Surface.Description.Height;
            LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Depth = (short)Surface.Description.Depth;
            LoadedTagMeta.Bitmaps[CurrentBitmapIndex].MIPMapCount = (short)Surface.Description.MipMapCount;
            LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Format = Surface.Format;
            if ((Surface.Description.Caps.Caps2 & Surface.SurfaceDescription.Capibilities.EdwCaps2.DDSCAPS2_CUBEMAP) == Surface.SurfaceDescription.Capibilities.EdwCaps2.DDSCAPS2_CUBEMAP)
                LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Type = H2BitmapCollection.EType.CUBEMAPS;
            else
                LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Type = H2BitmapCollection.EType.TEXTURES_2D;

            LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Flags = LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Width % 2 == 0 && LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Height % 2 == 0 ? H2BitmapCollection.BitmapData.EFlags.Power_Of_2_Dimensions : 0;

            int infoIndex = LoadedTagMeta.Bitmaps[CurrentBitmapIndex].LOD1Offset;
            LoadedTags[CurrentTagIndex].ReplaceRaw(infoIndex, Surface.GetData());

            MemoryStream memStream = new MemoryStream();
            LoadedTagMeta.Serialize(memStream, 0, H2BitmapCollection.SizeOf, 0);
            LoadedTags[CurrentTagIndex].TagStream = memStream;

            CurrentTagIndex = CurrentTagIndex;
        }

        private void AddBitmap(DirectDrawSurfaceStream Surface)
        {
            Array.Resize<H2BitmapCollection.BitmapData>(ref LoadedTagMeta.Bitmaps, LoadedTagMeta.Bitmaps.Length + 1);
            H2BitmapCollection.BitmapData bitmap = new H2BitmapCollection.BitmapData();

            bitmap.Width = (short)Surface.SurfaceDescription.dwWidth;
            bitmap.Height = (short)Surface.SurfaceDescription.dwHeight;
            bitmap.Depth = (short)Surface.SurfaceDescription.dwDepth;
            bitmap.MIPMapCount = (short)Surface.SurfaceDescription.dwMipMapCount;
            bitmap.Format = Surface.GetFormat();
            bitmap.Flags = bitmap.Width % 2 == 0 && bitmap.Height % 2 == 0 ? H2BitmapCollection.BitmapData.EFlags.Power_Of_2_Dimensions : 0;
            bitmap.LOD1Offset = LoadedTags[CurrentTagIndex].RawInfos.Length;

            LoadedTagMeta.Bitmaps[LoadedTagMeta.Bitmaps.Length - 1] = bitmap;

            LoadedTags[CurrentTagIndex].AddRaw(Surface.GetData()); 
            
            MemoryStream memStream = new MemoryStream();
            LoadedTagMeta.Serialize(memStream, 0, H2BitmapCollection.SizeOf, 0);
            LoadedTags[CurrentTagIndex].TagStream = memStream;

            CurrentTagIndex = CurrentTagIndex;
        }

        private void RemoveBitmap()
        {
            List<H2BitmapCollection.BitmapData> bitmaps = new List<H2BitmapCollection.BitmapData>(LoadedTagMeta.Bitmaps);
            bitmaps.RemoveAt(CurrentBitmapIndex);
            int rawIndex = LoadedTagMeta.Bitmaps[CurrentBitmapIndex].LOD1Offset;
            LoadedTagMeta.Bitmaps = bitmaps.ToArray();
            LoadedTags[CurrentTagIndex].RemoveRaw(rawIndex);
            foreach (H2BitmapCollection.BitmapData bitmap in LoadedTagMeta.Bitmaps)
            {
                if (bitmap.LOD1Offset > rawIndex) bitmap.LOD1Offset--;
                if (bitmap.LOD2Offset > rawIndex) bitmap.LOD2Offset--;
                if (bitmap.LOD3Offset > rawIndex) bitmap.LOD3Offset--;
            }

            MemoryStream memStream = new MemoryStream();
            LoadedTagMeta.Serialize(memStream, 0, H2BitmapCollection.SizeOf, 0);
            LoadedTags[CurrentTagIndex].TagStream = memStream;

            CurrentTagIndex = CurrentTagIndex;
        }

        private void LoadBitmapTag()
        {
            LoadedTagMeta = new H2BitmapCollection(LoadedTags[CurrentTagIndex].TagStream, 0, 0);
            RefreshBitmapInformation(); 
            //LoadBitmapStream();
        }

        private void RefreshBitmapInformation()
        {
            bitmapExplorer.Items.Clear();
            foreach (H2BitmapCollection.BitmapData bitmap in LoadedTagMeta.Bitmaps)
                LoadBitmapInformation(bitmap);
            bitmapExplorer.SelectedIndices.Clear();
            if (bitmapExplorer.Items.Count > 0)
                bitmapExplorer.SelectedIndices.Add(0);
        }

        private void LoadBitmapStream()
        {
            xnaBitmapViewer2.ClearTextures();
            int rawInfoIndex = LoadedTagMeta.Bitmaps[CurrentBitmapIndex].LOD1Offset;
            if ((rawInfoIndex & 0xC0000000) == 0x00000000)
            {
                BinaryReader binReader = new BinaryReader(LoadedTags[CurrentTagIndex].RawStream);
                LoadedTags[CurrentTagIndex].RawStream.Position = LoadedTags[CurrentTagIndex].RawInfos[rawInfoIndex].Address;
                byte[] Buffer = new byte[LoadedTags[CurrentTagIndex].RawInfos[rawInfoIndex].Length];
                binReader.Read(Buffer, 0, Buffer.Length);
                DirectDrawSurfaceStream SurfaceStream = new DirectDrawSurfaceStream();
                switch (LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Type)
                {
                    case H2BitmapCollection.EType.TEXTURES_2D:
                        xnaBitmapViewer2.LoadTexture2D(TextureLoader.LoadTexture(xnaBitmapViewer2.Device, LoadedTagMeta.Bitmaps[CurrentBitmapIndex], Buffer), LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Height, LoadedTagMeta.Bitmaps[CurrentBitmapIndex].Width);
                        break;
                    case H2BitmapCollection.EType.CUBEMAPS:
                        xnaBitmapViewer2.LoadCubemap(TextureLoader.LoadCubemap(xnaBitmapViewer2.Device, LoadedTagMeta.Bitmaps[CurrentBitmapIndex], Buffer));
                        break;
                }
            }
        }

        private void LoadBitmapInformation(H2BitmapCollection.BitmapData bitmap)
        {
            int index = bitmapExplorer.Items.Count;
            ListViewItem item = new ListViewItem(string.Format("Bitmap - {0}", index));
            item.SubItems.Add(bitmap.Format.ToString());
            item.SubItems.Add(string.Format("{0} x {1}", bitmap.Width, bitmap.Height));
            item.SubItems.Add(bitmap.MIPMapCount.ToString());
            bitmapExplorer.Items.Add(item);
        }

        private void LoadTags(string Folder)
        {
            LoadedTagMeta = null;
            LoadedTags = new List<Tag>();
            tagList.Items.Clear();
            bitmapExplorer.Items.Clear();
            string[] Files = Directory.GetFiles(Folder, "*.bitm.h2tag", SearchOption.AllDirectories);
            foreach (string s in Files)
            {
                Tag t = new Tag(s);
                LoadedTags.Add(t);
                string label = CleanFilename(t.Filename, Folder); 
                tagList.Items.Add(label);
            }
        }

        private string CleanFilename(string p, string Folder)
        {
            string s = p;
            if (Folder != string.Empty)
                s = s.Substring(Folder.Length + 1);
            if (Path.HasExtension(p))
                s = s.Remove(s.LastIndexOf('.'));
            return s;
        }

        private void Setup()
        {
            toolStripButton1.Checked = settings.Opacity;
            if (settings.TagsDirectory != null)
                LoadTags(settings.TagsDirectory);
        }
    }
}

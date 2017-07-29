using System;
using System.Configuration;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;
using System.IO;
using System.ServiceModel;
using QuadTreeApp;
using System.Drawing.Drawing2D;
using CRYBlowFish;

namespace prosnap
{
    public partial class ChooseMethod : Form
    {
        public Boolean closee = false;
        Boolean bthvertical = true;
      //  Boolean bthhori = true;
      //  Boolean bthquadtree = true;
     //   Boolean bthbox = true;
        String[] otherfiles = { "", "", "", "", "", "", "", "", "", "" };
        String[] quadfiles;
        private string zipFileName = "";
        String choosenmethod;
        String imagelocat;
        String user;
        String requser;
      //  ServiceHost Shost;
        public String returnzipfile;
        private const string myKey = "A";
        private long m_originalLength;
        public ChooseMethod()
        {

        }
        public ChooseMethod(String filename, String user, String requser)
        {
            imagelocat = filename;
            this.user = user;
            this.requser = requser;
            InitializeComponent();
            System.Drawing.Image img = System.Drawing.Image.FromFile(ConfigurationSettings.AppSettings["fileslist"] + imagelocat.ToString());
            System.Drawing.Size imgSize = GenerateImageDimensions(img.Width, img.Height, this.pictureBox1.Width, this.pictureBox1.Height);
            System.Drawing.Bitmap finalImg = new System.Drawing.Bitmap(img, imgSize.Width, imgSize.Height);
            System.Drawing.Graphics gfx = System.Drawing.Graphics.FromImage(img);
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            pictureBox1.Image = null;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.Image = finalImg;
            radioButton1.Select();
            button1.Enabled = false;
            btnencrypt.Enabled = false;
            btnsend.Enabled = false;
        }

        public System.Drawing.Size GenerateImageDimensions(int currW, int currH, int destW, int destH)
        {
            //double to hold the final multiplier to use when scaling the image
            double multiplier = 0;

            //string for holding layout
            string layout;

            //determine if it's Portrait or Landscape
            if (currH > currW) layout = "portrait";
            else layout = "landscape";

            switch (layout.ToLower())
            {
                case "portrait":
                    //calculate multiplier on heights
                    if (destH > destW)
                    {
                        multiplier = (double)destW / (double)currW;
                    }

                    else
                    {
                        multiplier = (double)destH / (double)currH;
                    }
                    break;
                case "landscape":
                    //calculate multiplier on widths
                    if (destH > destW)
                    {
                        multiplier = (double)destW / (double)currW;
                    }

                    else
                    {
                        multiplier = (double)destH / (double)currH;
                    }
                    break;
            }

            //return the new image dimensions
            return new System.Drawing.Size((int)(currW * multiplier), (int)(currH * multiplier));
        }
        private void btnVertical_Click(object sender, EventArgs e)
        {
            if (bthvertical != false)
            {
                btnHorizontal.Enabled = false;
                btnQuadTree.Enabled = false;
                btnBox.Enabled = false;
                choosenmethod = "vertical";
                callmethod();
                btnencrypt.Enabled = true;
            }
        }

        private void btnHorizontal_Click(object sender, EventArgs e)
        {
            btnVertical.Enabled = false;
            btnQuadTree.Enabled = false;
            btnBox.Enabled = false;
            choosenmethod = "horizontal";
            callmethod();
            btnencrypt.Enabled = true;
        }

        private void btnQuadTree_Click(object sender, EventArgs e)
        {

            btnHorizontal.Enabled = false;
            btnVertical.Enabled = false;
            btnBox.Enabled = false;
            choosenmethod = "quadtree";
            callmethod();
            btnencrypt.Enabled = true;
        }

        private void btnBox_Click(object sender, EventArgs e)
        {

            btnHorizontal.Enabled = false;
            btnVertical.Enabled = false;
            btnQuadTree.Enabled = false;
            choosenmethod = "box";
            callmethod();
            btnencrypt.Enabled = true;
        }

        private void callmethod()
        {
            imagelocat = (ConfigurationSettings.AppSettings["fileslist"] + imagelocat);
            int emb = 0;
            String seq = ConfigurationSettings.AppSettings[requser + "key"];
            int[] sequence = { 0, 0, 0, 0 };
            sequence[0] = Convert.ToInt32(seq.Substring(0, 1));
            sequence[1] = Convert.ToInt32(seq.Substring(2, 1));
            sequence[2] = Convert.ToInt32(seq.Substring(4, 1));
            sequence[3] = Convert.ToInt32(seq.Substring(6, 1));
            switch (choosenmethod)
            {
                case "vertical":
                    emb = sequence[0];
                    String[] filesv1 = callvertical(emb, imagelocat);
                    pictureBox1.ImageLocation = filesv1[1];
                    pictureBox1.Refresh();
                    otherfiles = filesv1;
                    break;
                case "horizontal":
                    emb = sequence[1];
                    String[] files1 = callhorizontal(emb, imagelocat);
                    pictureBox1.ImageLocation = files1[1];
                    pictureBox1.Refresh();
                    otherfiles = files1;
                    break;
                case "quadtree":
                    emb = sequence[2];
                    String[] files2 = callquadtree(emb, imagelocat);
                    pictureBox1.ImageLocation = files2[files2.Length - 2];
                    pictureBox1.Refresh();
                    quadfiles = files2;
                    break;
                case "box":
                    emb = sequence[3];
                    String[] files3 = callbox(emb, imagelocat);
                    pictureBox1.ImageLocation = files3[1];
                    pictureBox1.Refresh();
                    otherfiles = files3;
                    break;

            }

        }

        String[] callvertical(int emb, String imagelocat)
        {
            String[] files = new String[4];
            String sFileName_open = imagelocat;
            String extension = System.IO.Path.GetExtension(sFileName_open);
            String sFileName_save = sFileName_open.Substring(0, sFileName_open.Length - extension.Length);
            sFileName_save = sFileName_save.Substring(sFileName_save.LastIndexOf(('\\'))+1);
            sFileName_save = ConfigurationSettings.AppSettings["Converted"] + sFileName_save.ToString();
            files[0] = sFileName_save + ".xml";
            files[2] = sFileName_save + emb + ".xml";
            String Temp = sFileName_save + ".tif";
            files[3] = sFileName_save + emb + ".tif";
            files[3] = files[3].Substring(files[3].LastIndexOf(('\\'))+1);
            files[3] = ConfigurationSettings.AppSettings["zipfiles"] + files[3];
            sFileName_save = sFileName_save + emb + ".tif";
            RenderTargetBitmap rtb = ShredImage(sFileName_open, Temp, 1, 32);
            rtb = ShredImage(Temp, sFileName_save, 1, 64);
            files[1] = sFileName_save;
            return files;

        }

        String[] callhorizontal(int emb, string imagelocat)
        {
            String[] files = new String[5];
            String sFileName_open = imagelocat;
            String extension = System.IO.Path.GetExtension(sFileName_open);
            String sFileName_save = sFileName_open.Substring(0, sFileName_open.Length - extension.Length);
            sFileName_save = sFileName_save.Substring(sFileName_save.LastIndexOf(('\\')) + 1);
            sFileName_save = ConfigurationSettings.AppSettings["Converted"] + sFileName_save.ToString();
            files[0] = sFileName_save + ".xml";
            files[2] = sFileName_save + emb + ".xml";
            String Temp1 = sFileName_save + ".tif";
            String Temp2 = sFileName_save + "1" + ".tif";
            files[3] = sFileName_save + "1" + ".xml";
            files[4] = sFileName_save + emb + ".tif";
            files[4] = files[4].Substring(files[4].LastIndexOf(('\\')) + 1);
            files[4] = ConfigurationSettings.AppSettings["zipfiles"] + files[4];
            sFileName_save = sFileName_save + emb + ".tif";
            RenderTargetBitmap rtb = ShredImage(sFileName_open, Temp1, 16, 1);
            rtb = ShredImage(Temp1, Temp2, 32, 1);
            rtb = ShredImage(Temp2, sFileName_save, 64, 1);
            files[1] = sFileName_save;
            return files;
        }
        String[] callquadtree(int emb, string imagelocat)
        {
            String[] files = new String[10];
            QuadForm m = new QuadForm();
            files = m.doquadtree(imagelocat);
            m.Dispose();
            m = null;
            files[10] = files[9];
            files[10] = files[10].Insert(files[10].LastIndexOf('.'), emb.ToString());
            GC.Collect();
            return files;
        }
        String[] callbox(int emb, string imagelocat)
        {
            String[] files = new String[3];
            String sFileName_open = imagelocat;
            String extension = System.IO.Path.GetExtension(sFileName_open);
            String sFileName_save = sFileName_open.Substring(0, sFileName_open.Length - extension.Length);
            sFileName_save = sFileName_save.Substring(sFileName_save.LastIndexOf(('\\')) + 1);
            sFileName_save = ConfigurationSettings.AppSettings["Converted"] + sFileName_save.ToString();
            files[0] = sFileName_save + emb + ".xml";
            files[2] = sFileName_save + emb + ".tif";
            files[2] = files[2].Substring(files[2].LastIndexOf(('\\')) + 1);
            files[2] = ConfigurationSettings.AppSettings["zipfiles"] + files[2];
            sFileName_save = sFileName_save + emb + ".tif";
            RenderTargetBitmap rtb = ShredImage(sFileName_open, sFileName_save, 64, 64);
            files[1] = sFileName_save;

            return files;

        }

        void encrypt(String file)
        {
            FileStream originalStream = File.OpenRead(file); //Change to your file name  
            BlowFish alg = new BlowFish("04B915BA43FEB5B6");
            m_originalLength = originalStream.Length;
            Byte[] buffer = new byte[originalStream.Length];
            originalStream.Read(buffer, 0, buffer.Length);
            originalStream.Close();
            Byte[] cbuffer = alg.Encrypt_ECB(buffer);
            FileStream stream = new FileStream(file, FileMode.Create);
            stream.Write(cbuffer, 0, cbuffer.Length);
            stream.Close();
        }

        String zipfile1(String[] files)
        {

            zipFileName = files[files.Length - 1] + ".zip";
            java.io.FileOutputStream fos = new java.io.FileOutputStream(zipFileName);
            java.util.zip.ZipOutputStream zos = new java.util.zip.ZipOutputStream(fos);
            int l = files.Length;
            for (int i = 0; i < l - 1; i++)
            {
                if (files[i] != "")
                {
                    string sourceFile = files[i];
                    java.io.FileInputStream fis = new java.io.FileInputStream(sourceFile);
                    java.util.zip.ZipEntry ze = new java.util.zip.ZipEntry(files[i].Substring(files[i].LastIndexOf('\\') + 1));
                    zos.putNextEntry(ze);
                    sbyte[] buffer = new sbyte[1024];
                    int len;
                    while ((len = fis.read(buffer)) >= 0)
                    {
                        zos.write(buffer, 0, len);
                    }
                    zos.closeEntry();
                    fis.close();
                }
            }
            zos.close();
            fos.close();
            return zipFileName;
        }

        private RenderTargetBitmap ShredImage(string sFileNameIn, string sFileNameOut,
               int iRows, int iCols)
        {
            BitmapImage bi =
                new BitmapImage(new Uri("file://" + sFileNameIn));

            int iImageWidth = bi.PixelWidth;                // Not Width and Height.
            int iImageHeight = bi.PixelHeight;
            int iCroppedHeight = (int)(bi.PixelHeight / iRows);
            int iCroppedWidth = (int)(bi.PixelWidth / iCols);

            PointCollection pc1 = new PointCollection();    // Original.
            PointCollection pc2 = new PointCollection();    // Randomized.

            // Make list with positions of cropped bitmaps.
            Point p;
            for (int iRow = 0; iRow < iRows; iRow++)
            {
                for (int iCol = 0; iCol < iCols; iCol++)
                {
                    p = new Point(iCol * iCroppedWidth, iRow * iCroppedHeight);
                    pc1.Add(p);
                }
            }

            // Make randomized list.
            pc2 = RandomizePoints(pc1);

            // Make randomized image.
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            CroppedBitmap cb = null;
            int iCount = -1;
            foreach (Point p1 in pc1)
            {
                // Get cropped bitmap.
                cb = new CroppedBitmap(bi,
                    new Int32Rect((int)(p1.X), (int)(p1.Y),
                        iCroppedWidth, iCroppedHeight));
                // Draw.
                iCount++;
                drawingContext.DrawImage(cb,
                    new Rect((int)(pc2[iCount].X), (int)(pc2[iCount].Y),
                        iCroppedWidth, iCroppedHeight));
            }
            drawingContext.Close();

            // Store lists alongside the image as XML file.
            // Width and Height of cropped bitmaps are stored in the meta data.
            PointConverter pConv = new PointConverter();
            XList xList = new XList();
            xList.Meta.Add(iCroppedWidth.ToString());
            xList.Meta.Add(iCroppedHeight.ToString());
            for (int i = 0; i < pc1.Count; i++)
            {
                xList.DataOne.Add(pConv.ConvertToString(pc1[i]));
                xList.DataTwo.Add(pConv.ConvertToString(pc2[i]));
            }
            string sFileName_xml = System.IO.Path.ChangeExtension(sFileNameOut, ".xml");
            xList.Save(sFileName_xml);

            // Get bitmap.
            RenderTargetBitmap rtb = new RenderTargetBitmap(iImageWidth, iImageHeight,
                    96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            // Save.
            SaveBitmap(rtb, sFileNameOut);

            return rtb;
        }
        private bool SaveBitmap(RenderTargetBitmap oBitmap, string sFileName)
        {
            try
            {
                // Save to file.
                FileInfo fi = new FileInfo(sFileName);
                if (Directory.Exists(fi.DirectoryName) == true)
                {
                    if (File.Exists(sFileName) == true)
                    {
                        File.SetAttributes(sFileName, FileAttributes.Normal);
                    }
                    // Get extension and save as bitmap.
                    string sExtension = System.IO.Path.GetExtension(sFileName);
                    BitmapEncoder enc = null;
                    using (FileStream fs = new FileStream(sFileName, FileMode.Create))
                    {
                        switch (sExtension)
                        {
                            case ".bmp":
                                enc = new BmpBitmapEncoder();
                                break;
                            case ".gif":
                                enc = new GifBitmapEncoder();
                                break;
                            case ".jpg":
                                enc = new JpegBitmapEncoder();
                                break;
                            case ".png":
                                enc = new PngBitmapEncoder();
                                break;
                            case ".tif":
                                enc = new TiffBitmapEncoder();
                                break;
                            case ".wmp":
                                enc = new WmpBitmapEncoder();
                                break;
                        }

                        // Save bitmap to file.
                        enc.Frames.Add(BitmapFrame.Create(oBitmap));
                        enc.Save(fs);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        private PointCollection RandomizePoints(PointCollection inputList)
        {
            PointCollection points1 = inputList.Clone();
            PointCollection points2 = new PointCollection();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            Random r;
            byte[] seed = new byte[4];  // Int32.

            int randomIndex = 0;
            while (points1.Count > 0)
            {
                // Random index to select point.
                rng.GetBytes(seed);
                r = new Random(BitConverter.ToInt32(seed, 0));
                randomIndex = r.Next(0, points1.Count);
                // Add point to the new list.
                points2.Add(points1[randomIndex]);
                // Remove point from source list to avoid duplicates.
                points1.RemoveAt(randomIndex);
            }
            return points2;
        }

        private void btnencrypt_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            btnencrypt.Enabled = false;
            Boolean quad = false;
            if (otherfiles[1]=="")
            {
                otherfiles = quadfiles;
                quad = true;
            }
            Boolean boset = true;
            for (int i = 0; i < otherfiles.Length; i++)
            {
                if (quad == true)
                {
                    if (otherfiles[i].EndsWith(".xml"))
                        encrypt(otherfiles[i]);
                    else
                    {

                        if (otherfiles[i].StartsWith((ConfigurationSettings.AppSettings["zipfiles"].ToString() + "\\5")))
                        {
                            if (boset == true)
                            {
                                encrypt(otherfiles[i]);
                                boset = false;
                            }
                        }
                        else
                        {
                            File.Delete(otherfiles[i]);
                            if (i != otherfiles.Length - 1)
                                otherfiles[i] = "";
                        }
                    }

                }
                else
                {

                    if (i == (otherfiles.Length - 1))
                        break;
                    encrypt(otherfiles[i]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (choosenmethod == "quadtree")
            {
                returnzipfile = zipfile1(quadfiles);
            }
            else
            {
                returnzipfile = zipfile1(otherfiles);
            }
            bthvertical = false;
            btnsend.Enabled = true;
            button1.Enabled = false;
        }

        private void btnsend_Click(object sender, EventArgs e)
        {

            closee = false;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            closee = true;
            this.Close();
        }

        private void ChooseMethod_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnVertical_CheckedChanged(object sender, EventArgs e)
        {

        }

      
    }
}

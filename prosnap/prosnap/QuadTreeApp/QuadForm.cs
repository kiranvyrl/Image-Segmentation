using System;
using System.Collections.Generic;                   
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;                 
using System.IO;                                    
using QuadTreeLib;
using System.Windows.Forms;
using System.Configuration;

namespace QuadTreeApp
{
    public partial class QuadForm : Form
    {
        public String OpenFile;
        Boolean dodelete = false;
        String[] files = new String[11];
        int f = 0;
    //    int delcount = 1;
        QuadTree<Item> m_quadTree;
        public BitmapImage bi;
        QuadTreeRenderer m_renderer;
        private string sPath;
        static int countt = 0;
        int rowxcol = 0;
        public QuadForm()
        {
            InitializeComponent();
          
        }



        private void Init()
        {
            countt = 0;
            PictureBox p = new PictureBox();
            m_quadTree = new QuadTree<Item>(p.ClientRectangle);
            m_renderer = new QuadTreeRenderer(m_quadTree);
        }

        #region mouse interaction code

       // bool m_dragging = false;
       // System.Drawing.RectangleF m_selectionRect;
      //  Point m_startPoint;
     //   List<Item> m_selectedItems;



        void deletefile(String[] fi)
        {
            for (int d = 0; d < fi.Length - 1; d++)
            {
                if (fi[d] != null)
                {


                    File.Delete(fi[d].ToString());


                }
                else
                {
                    break;
                }
            }
        }

        private void CallEvent()
        {
     
            countt = countt + 1;
            Random rand = new Random(DateTime.Now.Millisecond);
            m_quadTree.Insert(new Item(new System.Drawing.Point(50, 50), rand.Next(25) + 4));
            Invalidate();
            callshuffle();

        }



        #endregion

        private void callshuffle()
        {
            rowxcol = Convert.ToInt32(Math.Pow(2.00, Convert.ToDouble((1 + countt))));
            int ro = rowxcol / 2;
            int co = rowxcol / 2;
            ShredImage(ro, co);
        }

        private void ShredImage(int iRows, int iCols)
        {

            string filename = OpenFile;
            bi = new BitmapImage(new Uri("file://" + filename));
            int iCroppedHeight;
            int iCroppedWidth;
            int iImageWidth = bi.PixelWidth;                // Not Width and Height.
            int iImageHeight = bi.PixelHeight;

            iCroppedWidth = iImageWidth / iCols;
            iCroppedHeight = iImageHeight / iRows;
            PointCollection pc1 = new PointCollection();    // Original.
            PointCollection pc2 = new PointCollection();
            PointCollection pc3 = new PointCollection(4);// Randomized.
            Point t;
            t = new Point(0, 0);
            pc3.Add(t);
            pc3.Add(t);
            pc3.Add(t);
            pc3.Add(t);
            Point p;
            if (countt != 1)
            {
                int ir = iRows / 2;
                int ic = iCols / 2;
            }
            int k = 1, mul = 1;
            int tempcount = 0;
            for (int irow = 0; irow < iRows; irow++)
            {

                for (int iCol = 0; iCol < iCols; iCol++)
                {
                    if (tempcount != rowxcol)
                    {
                        p = new Point((irow) * iCroppedWidth, (iCol) * iCroppedHeight);
                        pc1.Add(p);
                        tempcount++;
                    }
                    if (tempcount == rowxcol && countt != 1)
                    {

                        for (int i = irow; i < iRows * 2; i++)
                        {
                            for (int j = 0; j < iCols; j++)
                            {

                                p = new Point((i) * iCroppedWidth, (j) * iCroppedHeight);
                                pc1.RemoveAt(k);
                                pc1.Insert(k, p);

                                k++;
                                k++;

                                if (k == ((rowxcol * mul) + 1))
                                {
                                    irow++;
                                    break;
                                }
                            }
                            if (k == ((rowxcol * mul) + 1))
                            {
                                mul++;
                                tempcount = 0;
                                iCol--;
                                break;

                            }
                        }
                    }

                    else if (countt != 1)
                    {


                        p = new Point(0, 0);
                        pc1.Add(p);
                        tempcount++;
                    }
                    if (irow == iRows)
                    {
                        break;
                    }
                }

            }
            int cott = 0, from = 0, to = 4;
            int x = 0;
            for (int i = 0; i < (iRows * iCols); i = i + 4)
            {


                if (cott == 4)
                {
                    from = i;
                    to = i + 4;
                }

                for (int j = from; j < to; j++)
                {

                    pc3[x] = pc1[j];
                    x++;
                }
                pc3 = RandomizePoints(pc3);

                for (int j = 0; j < 4; j++)
                {
                    pc2.Add(pc3[j]);
                }
                cott = 4;
                x = 0;
           }
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
            String sFileName_xml = System.IO.Path.ChangeExtension(OpenFile, ".xml");
            sFileName_xml=sFileName_xml.Substring(sFileName_xml.LastIndexOf("\\")+1);
            sFileName_xml = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + sFileName_xml;
            files[f++] = sFileName_xml;
            xList.Save(sFileName_xml);
            if (countt == 4)
                Console.Read();
            // Get bitmap.
            RenderTargetBitmap rtb = new RenderTargetBitmap(iImageWidth, iImageHeight,
                    96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            // Save.
            SaveBitmap(rtb, OpenFile);
            bi.UriSource = null;
            cb = null;
            drawingVisual = null;
            rtb = null;

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
        private bool SaveBit(RenderTargetBitmap oBitmap, string sFileName)
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
            catch (Exception )
            {
                return false;
            }
        }
        private bool SaveBitmap(RenderTargetBitmap oBitmap, String sFileName)
        {
            sFileName = sFileName.Substring(sFileName.LastIndexOf("\\") + 1);
            sFileName = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + sFileName;
            String path = sFileName.Substring(sFileName.LastIndexOf("\\") + 1);
            path = path.Substring(0, path.LastIndexOf("."));
            if (countt > 1)
            {
                path = path.Substring(1, path.Length - 1);
            }
            sPath = ConfigurationSettings.AppSettings["zipfiles"];
            sFileName = sPath + "\\" + countt + path + ".tif";
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

                        files[f++] = sFileName;
                        OpenFile = sFileName;
                        enc = null;
                        fs.Dispose();
                        GC.Collect();
                       return true;
                    }
                }
                else
                {
                    return false;
                }
               // fi = null;
            }
            catch (Exception )
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //unshred(@"D:\image\5temp.tif", @"D:\image\5temp5.tif", @"D:\image\4temp.xml");
            //unshred(@"D:\image\5temp5.tif", @"D:\image\55temp5.tif", @"D:\image\3temp.xml");
            //unshred(@"D:\image\55temp5.tif", @"D:\image\555temp5.tif", @"D:\image\2temp.xml");
            //unshred(@"D:\image\555temp5.tif", @"D:\image\5555temp5.tif", @"D:\image\1temp.xml");
            //unshred(@"D:\image\5555temp5.tif", @"D:\image\55555temp5.tif", @"D:\image\Jellyfish.xml");
        }


        public void unshred(string sFileNameIn, string sFileNameOut, string xmlfile)
        {
            // Get shredded image from file.
            BitmapImage bi =
                new BitmapImage(new Uri("file://" + sFileNameIn));
            int iImageWidth = bi.PixelWidth;
            int iImageHeight = bi.PixelHeight;

            // Get Points from XML file.
            XList xList = new XList();
            string sFileName_xml = xmlfile;
            xList.Open(sFileName_xml);
            int iCroppedWidth = Convert.ToInt32(xList.Meta[0].Trim());
            int iCroppedHeight = Convert.ToInt32(xList.Meta[1].Trim());
            PointCollection pc1 = new PointCollection();
            PointCollection pc2 = new PointCollection();
            PointConverter pConv = new PointConverter();
            foreach (string s in xList.DataOne)
            {
                pc1.Add((Point)pConv.ConvertFromString(s));
            }
            foreach (string s in xList.DataTwo)
            {
                pc2.Add((Point)pConv.ConvertFromString(s));
            }

            // Reconstruct image.
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            CroppedBitmap cb = null;
            int iCount = -1;
            foreach (Point p2 in pc2)
            {
                iCount++;
                // Get cropped bitmap.
                cb = new CroppedBitmap(bi,
                    new Int32Rect((int)(p2.X), (int)(p2.Y),
                        iCroppedWidth, iCroppedHeight));
                // Draw.
                drawingContext.DrawImage(cb,
                    new Rect((int)(pc1[iCount].X), (int)(pc1[iCount].Y),
                        iCroppedWidth, iCroppedHeight));
            }
            drawingContext.Close();

            // Get bitmap.
            RenderTargetBitmap rtb = new RenderTargetBitmap(iImageWidth, iImageHeight,
                96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            // Save.
            SaveBit(rtb, sFileNameOut);


        }

       
        public String[] doquadtree(String pa)
        {
            Init();
            String imgloc = pa;
            OpenFile = imgloc;
            string filename = imgloc;
            bi = new BitmapImage(new Uri("file://" + filename));
            dodelete = false;
            CallEvent();
            CallEvent();
            CallEvent();
            CallEvent();
            CallEvent();
            bi = null;
            dodelete = true;
            m_quadTree = null;
          //  m_selectedItems = null;
            GC.Collect();
            bi = null;
            
            return files;
        }
        public  void doquaddelete(String[] fi)
        {
            deletefile(fi);
        }


    }
}

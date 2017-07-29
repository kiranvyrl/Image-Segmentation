using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;                                    
using System.Configuration;
using QuadTreeApp;   

namespace prosnap
{
    class unshred
    {

        static int tempcount=4;
        public  unshred(string sFileNameIn, string sFileNameOut)
        {
            // Get shredded image from file.
            BitmapImage bi =
                new BitmapImage(new Uri("file://" + sFileNameIn));
            int iImageWidth = bi.PixelWidth;
            int iImageHeight = bi.PixelHeight;

            // Get Points from XML file.
            XList xList = new XList();
            string sFileName_xml = System.IO.Path.ChangeExtension(sFileNameIn, ".xml");
            
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
            SaveBitmap(rtb, sFileNameOut);

          
        }
        public unshred(string sFileNameIn, string sFileNameOut,String[] xml)
        {
            // Get shredded image from file.
            BitmapImage bi =
                new BitmapImage(new Uri("file://" + sFileNameIn));
            int iImageWidth = bi.PixelWidth;
            int iImageHeight = bi.PixelHeight;

            // Get Points from XML file.
            XList xList = new XList();
            string sFileName_xml =Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"])+ xml[tempcount];
            tempcount--;

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
            SaveBitmap(rtb, sFileNameOut);


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
            catch(Exception )
            {
                return false;
            }
        }
    }
}

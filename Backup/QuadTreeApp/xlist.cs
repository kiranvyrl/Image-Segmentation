
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;                // File.
using System.Xml.Linq;          // XDocument. Add ref to System.Xml.Linq.dll




namespace QuadTreeApp
{
    public class XList
    {
        private List<string> _Meta;
        private List<string> _DataOne;
        private List<string> _DataTwo;


        // Constructor.
        public XList()
        {
            _Meta = new List<string>();
            _DataOne = new List<string>();
            _DataTwo = new List<string>();
        }


        // Propery - Meta.
        public List<string> Meta
        {
            get { return _Meta; }
            set { _Meta = value; }
        }


        // Propery - DataOne.
        public List<string> DataOne
        {
            get { return _DataOne; }
            set { _DataOne = value; }
        }

        // Propery - DataTwo.
        public List<string> DataTwo
        {
            get { return _DataTwo; }
            set { _DataTwo = value; }
        }


       public bool Save(string sFileName)
        {
            FileInfo fi = new FileInfo(sFileName);
            if (Directory.Exists(fi.DirectoryName) == false)
            {
                return false;
            }

            try
            {
                XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));
                XElement xContainer = new XElement("container");

                // Meta.
                XElement xMeta = new XElement("meta");
                foreach (string s in _Meta)
                {
                    xMeta.Add(new XElement("td", s));
                }

                // DataOne.
                XElement xDataOne = new XElement("dataOne");
                foreach (string s in _DataOne)
                {
                    xDataOne.Add(new XElement("td", s));
                }

                // DataTwo.
                XElement xDataTwo = new XElement("dataTwo");
                foreach (string s in _DataTwo)
                {
                    xDataTwo.Add(new XElement("td", s));
                }

                xContainer.Add(xMeta);
                xContainer.Add(xDataOne);
                xContainer.Add(xDataTwo);
                xDoc.Add(xContainer);

                if (File.Exists(sFileName))
                {
                    File.SetAttributes(sFileName, FileAttributes.Normal);
                }
                xDoc.Save(sFileName);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool Open(string sFileName)
        {
            if (File.Exists(sFileName) == false) { return false; }

            try
            {
                XDocument xDoc = XDocument.Load(sFileName);

                XElement xMeta = xDoc.Root.Element("meta");
                foreach (XElement e in xMeta.Elements())
                {
                    _Meta.Add(e.Value);
                }

                XElement xDataOne = xDoc.Root.Element("dataOne");
                foreach (XElement e in xDataOne.Elements())
                {
                    _DataOne.Add(e.Value);
                }

                XElement xDataTwo = xDoc.Root.Element("dataTwo");
                foreach (XElement e in xDataTwo.Elements())
                {
                    _DataTwo.Add(e.Value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }



    }   // end class
}       // end namespace

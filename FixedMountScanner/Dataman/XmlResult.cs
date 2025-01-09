using System;
using System.Collections.Generic;
using System.Net;
using log4net;
using System.Xml;
using System.Linq;
using System.Drawing;
using Cognex.DataMan.SDK;
using Cognex.DataMan.SDK.Utils;
using System.Threading.Tasks;
using System.Text;
using Svg;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace FixedMountScanner
{
    public class XmlResult : XmlResultBase, IDisposable
    {
        public Image Image;
        public Image OverlayImage;
        public DateTime GrabTime { get; }

        List<XmlResultBase> Child;
        public int Count
        {
            get
            {
                return Child?.Count ?? 0;
            }
        }
        public override int ImageID
        {
            get
            {
                if (Child?.Count > 0)
                    return Child.First().ImageID;
                return _imageID;
            }
        }
        public XmlResult(string xml) : base(xml)
        {
            GrabTime = DateTime.Now;
            GetReadStringFromResultXml(xml);
            GetChildBarcodes(xml);
        }
        public XmlResult()
        {
        }
        protected void GetChildBarcodes(string resultXml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(resultXml);

                XmlNode ndResult = doc.SelectSingleNode("result");
                XmlNodeList results = ndResult?.SelectNodes("result");
                if (results != null && results.Count > 0)
                {
                    Child = new List<XmlResultBase>();
                    foreach (XmlNode item in results)
                    {
                        XmlResultBase chlid = new XmlResultBase(item.OuterXml);
                        Child.Add(chlid);
                    }
                }
            }
            catch { }
        }
        public void SetImage(ImageArrivedEventArgs e)
        {
            Image = new Bitmap(e.Image);
        }
        public void SetGraphics(ImageGraphicsArrivedEventArgs e)
        {
            var graphics = e.ImageGraphics;
            XmlDocument dc = new XmlDocument();
            dc.LoadXml(graphics);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(dc.NameTable);
            nsMgr.AddNamespace("svg", dc.DocumentElement.NamespaceURI);
            var imageNd = dc.SelectSingleNode("//svg:image", nsMgr);
            if (imageNd != null)
                dc.DocumentElement.RemoveChild(imageNd);
            graphics = dc.DocumentElement.OuterXml;
            //이미지 정보가 있으면 BaseUri가 필수인데 Dataman에서 받은 이미지는 경로가 없으므로 uri가 없다
            //따라서 이미지 정보를 없애 에러가 안나도록, 에러가 나도 그래픽은 입혀 지지만, 에러 처리 때문에 100ms정도가 더 걸림
            if (Image != null)
            {
                Bitmap bitmap = new Bitmap(Image);
                var svg = SvgDocument.FromSvg<SvgDocument>(graphics);
                if (svg != null)
                {
                    svg.Height = new SvgUnit(bitmap.Height);
                    svg.Width = new SvgUnit(bitmap.Width);
                    svg.Draw(bitmap);
                }
                OverlayImage = bitmap;
            }
        }
        public void Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
                Image = null;
            }
            if (OverlayImage != null)
            {
                OverlayImage.Dispose();
                OverlayImage = null;
            }
        }
    }
    public class XmlResultBase 
    {
        protected string _status;
        protected int _imageID;
        public int ResultID { get; set; }
        public string FullString { get; set; }
        public virtual int ImageID { get { return _imageID; } }
        public bool Status
        {
            get { return _status == "GOOD READ"; }
        }
        public XmlResultBase(string xml)
        {
            GetReadStringFromResultXml(xml);
        }        
        public XmlResultBase()
        {
        }
        protected void GetReadStringFromResultXml(string resultXml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(resultXml);

                XmlNode ndResult = doc.SelectSingleNode("result");
                XmlAttribute id = ndResult?.Attributes["id"];
                XmlAttribute imageid = ndResult?.Attributes["image_id"];
                XmlNode ndGeneral = ndResult?.SelectSingleNode("general");
                XmlNode ndStatus = ndGeneral?.SelectSingleNode("status");
                XmlNode ndFullString = ndGeneral?.SelectSingleNode("full_string");

                if (ndResult != null)
                {
                    ResultID = Convert.ToInt32(id?.Value);
                    _imageID = Convert.ToInt32(imageid?.Value);
                    _status = GetXmlValue(ndStatus);
                    FullString = GetXmlValue(ndFullString);
                }
            }
            catch { }
        }
        protected string GetXmlValue(XmlNode nd)
        {
            if (nd is XmlNode)
            {
                XmlAttribute encoding = nd.Attributes["encoding"];
                if (encoding?.InnerText == "base64")
                {
                    if (!string.IsNullOrEmpty(nd.InnerText))
                    {
                        byte[] code = Convert.FromBase64String(nd.InnerText);
                        return Encoding.UTF8.GetString(code);
                    }
                }
                return nd.InnerText;
            }
            return "";
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace TeachMe.Helpers
{
    public class ImageUtils
    {
        /// <summary>
        /// Resize image
        /// </summary>
        /// <param name="imgToResize">Image - image to resize</param>
        /// <param name="size">Size - new size of image</param>
        /// <returns></returns>
        public static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }


        /// <summary>
        /// Resize image with fixed width
        /// </summary>
        /// <param name="imgToResize">Image - image to resize</param>
        /// <param name="width">Width size</param>
        /// <returns></returns>
        public static Image ResizeImageFixedWidth(Image imgToResize, int width)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = ((float)width / (float)sourceWidth);

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        /// <summary>
        /// Image to byte array
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns>byte[]</returns>
        public static byte[] ImageToByteArraybyMemoryStream(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }

        /// <summary>
        /// Upload Image to http://imgur.com/
        /// </summary>
        /// <param name="image">byte[]</param>
        /// <returns>string url</returns>
        public static string UploadImageToImgur(byte[] image)
        {
            string ClientId = "6b18f55eeee07f1";
            WebClient w = new WebClient();
            w.Headers.Add("Authorization", "Client-ID " + ClientId);
            System.Collections.Specialized.NameValueCollection Keys = new System.Collections.Specialized.NameValueCollection();
            try
            {
                Keys.Add("image", Convert.ToBase64String(image));
                byte[] responseArray = w.UploadValues("https://api.imgur.com/3/image.xml", Keys);
                dynamic result = Encoding.ASCII.GetString(responseArray);
                XDocument xml = XDocument.Parse(result);
                var i = xml.Root.Element("link").Value;
                return i;
            }
            catch (Exception s)
            {
                //MessageBox.Show("Something went wrong. " + s.Message);
                return s.Message;
            }
        }
    }
}
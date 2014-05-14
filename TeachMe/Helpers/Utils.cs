using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace TeachMe.Helpers
{
    public class Utils
    {
        
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
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
        public static string CheckRate(double lower, double upper, int rating, int totalRaters)
        {
            double toCheck = totalRaters == 0 ? 0 : Convert.ToDouble(rating) / Convert.ToDouble(totalRaters);
            //string str = toCheck > lower && toCheck <= upper ? " checked=\"checked\"" : null;
            return toCheck > lower && toCheck <= upper ? " checked=checked" : null;
        }

    }
}
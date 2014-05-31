using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TeachMe.Helpers
{
    public static class ActionLinkHelper
    {
        public static MvcHtmlString ActionLinkIcon(this HtmlHelper helper, string linkText, string actionName, string glyphiconClass)
        {
            return ActionLinkIcon(helper, linkText, actionName, null, glyphiconClass, null);
        }

        public static MvcHtmlString ActionLinkIcon(this HtmlHelper helper, string linkText, string actionName, string controllerName, string glyphiconClass)
        {
            return ActionLinkIcon(helper, linkText, actionName, controllerName, glyphiconClass, null);
        }
        public static string SimpleLink(this HtmlHelper html, string url, string text)
        {
            return String.Format("<a href=\"{0}\">{1}</a>", url, text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="iconName">awesome glyphicon name</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLinkIcon(this HtmlHelper helper, string linkText, string actionName, string controllerName, string iconName, object htmlAttributes)
        {
            // Glyphicons
            //http://getbootstrap.com/components/#glyphicons

            var a = new TagBuilder("a");

            // Get current controller
            if (string.IsNullOrEmpty(controllerName))
            {
                var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
                if (routeValues != null && routeValues.ContainsKey("controller"))
                    controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }

            // Add attributes
            a.MergeAttribute("href", "/" + controllerName + "/" + actionName);
            a.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            // Render icon
            var span = new TagBuilder("i");
            span.AddCssClass(iconName);

            a.InnerHtml = span.ToString(TagRenderMode.Normal) + "&nbsp;&nbsp;" + linkText;

            // Render tag
            return MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
        }
    }
}
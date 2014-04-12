using System.Web.Mvc;
using System.Web.Routing;

namespace TeachMe.Helpers
{
    public static class ActionLinkHelper
    {

        public static MvcHtmlString ActionLinkIcon(this HtmlHelper helper, string linkText, string actionName, string controllerName, string glyphiconClass)
        {
            return ActionLinkIcon(helper, linkText, actionName, controllerName, glyphiconClass, null);
        }

        public static MvcHtmlString ActionLinkIcon(this HtmlHelper helper, string linkText, string actionName, string controllerName, string glyphiconClass, object htmlAttributes)
        {
            //Glyphicons
            //http://getbootstrap.com/components/#glyphicons

            var a = new TagBuilder("a");

            // Add attributes
            a.MergeAttribute("href", controllerName + "/" + actionName);
            a.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var span = new TagBuilder("span");
            span.AddCssClass(glyphiconClass);

            a.InnerHtml = span.ToString(TagRenderMode.Normal) + " " + linkText;

            // Render tag
            return MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
        }
    }
}
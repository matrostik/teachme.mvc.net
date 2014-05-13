using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TeachMe.Helpers
{
       public static class GroupDropListExtensions
    {
           public static MvcHtmlString GroupDropList(this HtmlHelper helper, string name, IEnumerable<GroupDropListItem> data, string SelectedValue, string optionLabel, object htmlAttributes)
        {
            if (data == null && helper.ViewData != null)
                data = helper.ViewData.Eval(name) as IEnumerable<GroupDropListItem>;
            if (data == null) return MvcHtmlString.Empty;


            var select = new TagBuilder("select");


            if (htmlAttributes != null)
                select.MergeAttributes(new RouteValueDictionary(htmlAttributes));


            select.GenerateId(name);
            select.Attributes.Add("name", name);

            var optgroupHtml = new StringBuilder();
            var groups = data.ToList();
            foreach (var group in data)
            {
                var groupTag = new TagBuilder("optgroup");
                groupTag.Attributes.Add("label", helper.Encode( group.Name));
                var optHtml = new StringBuilder();
                foreach (var item in group.Items)
                {
                    var option = new TagBuilder("option");
                    option.Attributes.Add("value", helper.Encode(item.Value));
                    if (SelectedValue != null && item.Value == SelectedValue)
                        option.Attributes.Add("selected", "selected");
                    option.InnerHtml = helper.Encode(item.Text);
                    optHtml.AppendLine(option.ToString(TagRenderMode.Normal));
                }
                groupTag.InnerHtml = optHtml.ToString();
                optgroupHtml.AppendLine(groupTag.ToString(TagRenderMode.Normal));
            }
            // set default option
            var optionDefault = new TagBuilder("option");
            optionDefault.Attributes.Add("value", "");
            optionDefault.InnerHtml = helper.Encode(optionLabel);
            select.InnerHtml = optionDefault.ToString() + optgroupHtml.ToString();
            return MvcHtmlString.Create(select.ToString(TagRenderMode.Normal));
        }
}


    public class GroupDropListItem
    {
        public string Name { get; set; }
        public List<OptionItem> Items { get; set; }
    }


    public class OptionItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}

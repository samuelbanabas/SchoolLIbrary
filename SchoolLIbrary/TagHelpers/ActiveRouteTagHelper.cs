using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SchoolLIbrary.TagHelpers
{
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        public ActiveRouteTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            if (urlHelper.Action(Action, Controller) != null)
            {
                if (urlHelper.ActionContext.RouteData.Values["Controller"].ToString().Equals(Controller, StringComparison.OrdinalIgnoreCase) &&
                    urlHelper.ActionContext.RouteData.Values["Action"].ToString().Equals(Action, StringComparison.OrdinalIgnoreCase))
                {
                    var classAttribute = output.Attributes.FirstOrDefault(a => a.Name == "class");
                    if (classAttribute == null)
                    {
                        output.Attributes.Add("class", "active");
                    }
                    else if (classAttribute.Value.ToString().IndexOf("active") < 0)
                    {
                        output.Attributes.SetAttribute("class", classAttribute.Value.ToString() + " active");
                    }
                }
            }
        }
    }
}

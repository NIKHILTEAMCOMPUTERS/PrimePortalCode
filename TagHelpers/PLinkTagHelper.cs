using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;

namespace RMS.Client.TagHelpers
{
    public class PLinkTagHelper : TagHelper
    {
        public string id { get; set; }
        public string text { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
        public bool isbutton { get; set; } = true;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", $"/{controller}/{action}");
            output.Attributes.SetAttribute("class", !isbutton 
                ? "group py-3 px-4 inline-flex justify-center items-center gap-2 rounded-md border border-transparent font-semibold text-gray-800 hover:bg-emerald-400 hover:text-white focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:ring-offset-2 transition-all text-sm"
                : "py-2 px-5 inline-flex justify-center items-center gap-2 rounded-md border border-transparent bg-[#00cc83] text-white hover:bg-emerald-600 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:ring-offset-2 transition-all text-sm 2xl:text-base");
            var content = !isbutton ? @"<svg class=""w-5 h-5 text-emerald-400 group-hover:text-white"" xmlns=""http://www.w3.org/2000/svg"" width=""32"" height=""32"" viewBox=""0 0 32 32"">
                <path fill = ""currentColor"" d=""M16 2A14.172 14.172 0 0 0 2 16a14.172 14.172 0 0 0 14 14a14.172 14.172 0 0 0 14-14A14.172 14.172 0 0 0 16 2Zm8 15h-7v7h-2v-7H8v-2h7V8h2v7h7Z"" />
                <path fill = ""none"" d=""M24 17h-7v7h-2v-7H8v-2h7V8h2v7h7v2z"" />
            </svg>"
            : @"<svg class=""w-4 h-4"" xmlns=""http://www.w3.org/2000/svg"" width=""32"" height=""32"" viewBox=""0 0 32 32"">
                <path fill=""currentColor""
                        d=""M16 2A14.172 14.172 0 0 0 2 16a14.172 14.172 0 0 0 14 14a14.172 14.172 0 0 0 14-14A14.172 14.172 0 0 0 16 2Zm8 15h-7v7h-2v-7H8v-2h7V8h2v7h7Z"" />
                <path fill=""none"" d=""M24 17h-7v7h-2v-7H8v-2h7V8h2v7h7v2z"" />
            </svg>";

            if (!string.IsNullOrEmpty(id))
                output.Attributes.SetAttribute("id", id);

            output.PreContent.SetHtmlContent(content);
            output.Content.SetContent(text);
        }
    }
}

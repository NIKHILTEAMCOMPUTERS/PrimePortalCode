using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RMS.Client.TagHelpers
{
    public class PButtonTagHelper : TagHelper
    {
        public string text { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Content.SetContent(text);

            //<button type="button" class="py-2 px-5 inline-flex justify-center items-center gap-2 rounded-md border border-transparent bg-[#00cc83] text-white hover:bg-emerald-600 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:ring-offset-2 transition-all text-xs sm:text-sm 2xl:text-base" onclick="showFormStep(2);">Next</button>
            //<button type="button" class="py-2 px-5 border  border text-xs sm:text-sm 2xl:text-base rounded" onclick="showFormStep(1);">Prev</button>
            //<button type="submit" class="py-2 px-5 inline-flex justify-center items-center gap-2 rounded-md border border-transparent bg-[#00cc83] text-white hover:bg-emerald-600 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:ring-offset-2 transition-all text-xs sm:text-sm 2xl:text-base" onclick="handleSubmit(event)">Save</button>
        }
    }
}

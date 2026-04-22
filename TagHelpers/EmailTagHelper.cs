using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RMS.Client.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
        }
    }
}


//Button
//TextBox
//Dropdown-Singl (Searchable)
//Dropdown-Multi (Searchable)
//Checkbox
//Toastr

//DataTable
//Pageheader  => Heading, CardListView, AddButton
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Customizable.Menu.TagHelpers
{
    [HtmlTargetElement("cameltag")]
    public class CamelTagHelper : TagHelper
    {
        public string TransformText { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string result = string.Empty;
            List<string> words = TransformText.Clean().Split(' ').ToList();

            foreach (string word in words)
                result += $"<span class='cameltag-start'>"
                    + $"{word.Trim()[..1]}</span>"
                    + $"<span class='cameltag-end'>"
                    + $"{word.Trim()[1..]}</span>";

            output.TagName = null;
            output.PostContent.AppendHtml(result);
        }
    }
}

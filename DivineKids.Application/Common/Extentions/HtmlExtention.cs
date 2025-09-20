using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivineKids.Application.Common.Extentions;
public static class HtmlExtention
{
    public static string ToHtml(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        var html = text.Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");
        return html;
    }
}

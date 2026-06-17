using System;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Html;

namespace Millenium.Util
{
    public static class Util
    {
        public static void ConverterParametroEmNulo(SqlCommand cmd)
        {
            foreach (SqlParameter parameter in cmd.Parameters)
            {
                if (parameter.Value == null)
                {
                    parameter.Value = DBNull.Value;
                }
            }
        }
    }

    public static class DisableHtmlControlExtension
    {
        public static IHtmlContent DisableIf(this IHtmlContent htmlString, Func<bool> expression)
        {
            if (htmlString == null || !expression.Invoke()) return htmlString!;
            
            string html;
            using (var writer = new System.IO.StringWriter())
            {
                htmlString.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                html = writer.ToString();
            }

            const string disabled = "\"disabled\"";
            int index = html.IndexOf(">", StringComparison.Ordinal);
            if (index >= 0)
            {
                html = html.Insert(index, " disabled= " + disabled);
            }
            return new HtmlString(html);
        }
    }

    public static class RequiredHtmlControlExtension
    {
        public static IHtmlContent RequiredIf(this IHtmlContent htmlString, Func<bool> expression)
        {
            if (htmlString == null || !expression.Invoke()) return htmlString!;
            
            string html;
            using (var writer = new System.IO.StringWriter())
            {
                htmlString.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                html = writer.ToString();
            }

            const string required = "\"required\"";
            int index = html.IndexOf(">", StringComparison.Ordinal);
            if (index >= 0)
            {
                html = html.Insert(index, " required = " + required);
            }
            return new HtmlString(html);
        }
    }
}

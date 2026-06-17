using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Millenium.Util
{
    public static class MenuHelper
    {
        public static List<IMenu>? SiteMainMenu { get; set; }
        public static List<IMenu>? SiteViewApp { get; set; }

        public static IHtmlContent SiteMenuList(this IHtmlHelper helper, List<IMenu> menuMain, List<IMenu> menuView)
        {
            try
            {
                SiteMainMenu = menuMain;
                SiteViewApp = menuView;

                if (SiteMainMenu == null || SiteMainMenu.Count == 0)
                    return HtmlString.Empty;

                return new HtmlString(DMenuItems(helper));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string HtmlContentToString(IHtmlContent content)
        {
            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        private static string ToTitleCase(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            
            var lowerWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "de", "do", "da", "dos", "das", "em", "para", "o", "a", "os", "as" };
            var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i].ToLower();
                if (i > 0 && lowerWords.Contains(word))
                {
                    words[i] = word;
                }
                else
                {
                    if (word.Length > 0)
                    {
                        words[i] = char.ToUpper(word[0]) + word.Substring(1);
                    }
                }
            }
            return string.Join(" ", words);
        }

        private static string DMenuItems(IHtmlHelper helper)
        {
            try
            {
                var tagFinal = "";

                if (SiteMainMenu == null) return tagFinal;

                var mainMenu = SiteMainMenu.Where(p => p.MenuPaiId.Equals(0));

                foreach (var main in mainMenu)
                {
                    var tagLiSub = "";
                    var subMenu = SiteMainMenu.Where(p => p.MenuPaiId.Equals(main.Id));

                    foreach (var sub in subMenu)
                    {
                        var liSubFinal = new TagBuilder("li");
                        var ulSub = new TagBuilder("ul");

                        var viewApp = SiteViewApp?.Where(p => p.MenuPaiId.Equals(sub.Id)) ?? Enumerable.Empty<IMenu>();

                        foreach (var view in viewApp)
                        {
                            var liView = new TagBuilder("li");
                            if (view.Controller == "TipoCliente" && view.Action == "Index")
                            {
                                liView.InnerHtml.AppendHtml("<a href=\"/Blazor/TipoCliente\">" + ToTitleCase(view.Descricao) + "</a>");
                            }
                            else if (view.Controller == "Solicitacao" && view.Action == "Index")
                            {
                                liView.InnerHtml.AppendHtml("<a href=\"/Blazor/Solicitacao\">" + ToTitleCase(view.Descricao) + "</a>");
                            }
                            else if ((view.Controller == "Faturamento" || view.Controller == "Gerar") && view.Action == "Index")
                            {
                                liView.InnerHtml.AppendHtml("<a href=\"/Blazor/Faturamento\">" + ToTitleCase(view.Descricao) + "</a>");
                            }
                            else
                            {
                                string viewArea = "";
                                if (!string.IsNullOrEmpty(view.Route) && view.Route.Contains('_'))
                                {
                                    viewArea = view.Route.Split('_')[0];
                                }
                                var actionLinkContent = helper.ActionLink(ToTitleCase(view.Descricao), view.Action, view.Controller, new { area = viewArea });
                                liView.InnerHtml.AppendHtml(actionLinkContent);
                            }
                            ulSub.InnerHtml.AppendHtml(HtmlContentToString(liView));
                        }

                        if (sub.Controller == "TipoCliente" && sub.Action == "Index")
                        {
                            liSubFinal.InnerHtml.AppendHtml("<a href=\"/Blazor/TipoCliente\">" + ToTitleCase(sub.Descricao) + "</a>");
                        }
                        else if (sub.Controller == "Solicitacao" && sub.Action == "Index")
                        {
                            liSubFinal.InnerHtml.AppendHtml("<a href=\"/Blazor/Solicitacao\">" + ToTitleCase(sub.Descricao) + "</a>");
                        }
                        else if ((sub.Controller == "Faturamento" || sub.Controller == "Gerar") && sub.Action == "Index")
                        {
                            liSubFinal.InnerHtml.AppendHtml("<a href=\"/Blazor/Faturamento\">" + ToTitleCase(sub.Descricao) + "</a>");
                        }
                        else
                        {
                            string area = "";
                            if (!string.IsNullOrEmpty(sub.Route) && sub.Route.Contains('_'))
                            {
                                area = sub.Route.Split('_')[0];
                            }
                            var routeLinkContent = helper.ActionLink(ToTitleCase(sub.Descricao), sub.Action, sub.Controller, new { area = area });
                            liSubFinal.InnerHtml.AppendHtml(routeLinkContent);
                        }

                        if (viewApp.Any())
                        {
                            liSubFinal.InnerHtml.AppendHtml(HtmlContentToString(ulSub));
                        }

                        tagLiSub += HtmlContentToString(liSubFinal);
                    }

                    var ulFinal = new TagBuilder("ul");
                    ulFinal.InnerHtml.AppendHtml(tagLiSub);

                    var liFinal = new TagBuilder("li");

                    if (!string.IsNullOrEmpty(main.Route))
                    {
                        string mainArea = "";
                        if (main.Route.Contains('_'))
                        {
                            mainArea = main.Route.Split('_')[0];
                        }
                        var mainRouteLink = helper.ActionLink(ToTitleCase(main.Descricao), main.Action, main.Controller, new { area = mainArea });
                        liFinal.InnerHtml.AppendHtml(mainRouteLink);
                    }
                    else
                    {
                        liFinal.InnerHtml.AppendHtml("<a href='javascript:;' data-toggle='collapse' data-target='#" + main.Controller + "'>" + ToTitleCase(main.Descricao) + "<i class='fa fa-fw fa-caret-down'></i></a>");
                        ulFinal.AddCssClass("collapse");
                        ulFinal.Attributes["id"] = main.Controller;
                    }

                    if (!string.IsNullOrEmpty(tagLiSub))
                        liFinal.InnerHtml.AppendHtml(HtmlContentToString(ulFinal));
                    
                    tagFinal += HtmlContentToString(liFinal);
                }

                return tagFinal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

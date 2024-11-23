using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace WEB_253503_Timoshevich.UI.TagHelpers;


[HtmlTargetElement("pager")]
public class PagerTagHelper : TagHelper
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HtmlAttributeName("current-page")]
    public int CurrentPage { get; set; }

    [HtmlAttributeName("total-pages")]
    public int TotalPages { get; set; }

    [HtmlAttributeName("category")]
    public string? Category { get; set; }

    [HtmlAttributeName("admin")]
    public bool Admin { get; set; } = false;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Создаем список <ul> с классом "pagination"
        output.TagName = "ul";
        output.Attributes.Add("class", "pagination");

        // Добавляем кнопку "Previous", если текущая страница больше 1
        if (CurrentPage > 1)
        {
            output.Content.AppendHtml(CreatePageItem(CurrentPage - 1, "Previous"));
        }

        // Генерируем кнопки для всех страниц
        for (int i = 1; i <= TotalPages; i++)
        {
            output.Content.AppendHtml(CreatePageItem(i, i.ToString()));
        }

        // Добавляем кнопку "Next", если текущая страница меньше общего числа страниц
        if (CurrentPage < TotalPages)
        {
            output.Content.AppendHtml(CreatePageItem(CurrentPage + 1, "Next"));
        }
    }

    private TagBuilder CreatePageItem(int page, string text)
    {
        var li = new TagBuilder("li");
        li.AddCssClass("page-item");

        if (page == CurrentPage)
        {
            li.AddCssClass("active");
        }

        var a = new TagBuilder("a");
        a.AddCssClass("page-link");
        a.Attributes["href"] = GeneratePageLink(page);
        a.InnerHtml.Append(text);

        li.InnerHtml.AppendHtml(a);
        return li;
    }

    private string GeneratePageLink(int page)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null.");
        }

        var values = new RouteValueDictionary
    {
        { "pageNo", page }
    };

        if (!string.IsNullOrEmpty(Category))
        {
            values["category"] = Category;
        }

        string? url = Admin
            ? _linkGenerator.GetPathByAction(
                action: "Index",
                controller: "Admin",
                values: values,
                httpContext: httpContext)
            : _linkGenerator.GetPathByAction(
                action: "Index",
                controller: "Product",
                values: values,
                httpContext: httpContext);

        return url ?? "#";
    }
}

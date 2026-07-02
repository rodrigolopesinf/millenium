using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Millenium.Application.Interfaces;
using Millenium.Application.Services;
using Millenium.Domain.Interfaces.Repositories;
using Millenium.Domain.Interfaces.Services;
using Millenium.Domain.Services;
using Millenium.Infra.Data.Contexto;
using Millenium.Infra.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext para SQL Server
builder.Services.AddDbContext<ContextMillenium>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Millenium")));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Injeção do Cascading Authentication State no Blazor Server .NET 8
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Autenticação com Cookies (Substituto moderno do FormsAuthentication)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registro da Injeção de Dependências
builder.Services.AddScoped(typeof(IAppServiceBase<>), typeof(AppServiceBase<>));
builder.Services.AddScoped<IClienteAppService, ClienteAppService>();
builder.Services.AddScoped<IContatoAppService, ContatoAppService>();
builder.Services.AddScoped<IContatoClienteAppService, ContatoClienteAppService>();
builder.Services.AddScoped<IEnderecoAppService, EnderecoAppService>();
builder.Services.AddScoped<IFaturamentoAppService, FaturamentoAppService>();
builder.Services.AddScoped<IMenuAppService, MenuAppService>();
builder.Services.AddScoped<INivelAppService, NivelAppService>();
builder.Services.AddScoped<ISituacaoClienteAppService, SituacaoClienteAppService>();
builder.Services.AddScoped<ISolicitacaoAppService, SolicitacaoAppService>();
builder.Services.AddScoped<ITipoClienteAppService, TipoClienteAppService>();
builder.Services.AddScoped<ITipoSolicitacaoAppService, TipoSolicitacaoAppService>();
builder.Services.AddScoped<IUsuarioAppService, UsuarioAppService>();

builder.Services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IContatoService, ContatoService>();
builder.Services.AddScoped<IContatoClienteService, ContatoClienteService>();
builder.Services.AddScoped<IEnderecoService, EnderecoService>();
builder.Services.AddScoped<IFaturamentoService, FaturamentoService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IMenuUsuarioService, MenuUsuarioService>();
builder.Services.AddScoped<INivelService, NivelService>();
builder.Services.AddScoped<ISituacaoClienteService, SituacaoClienteService>();
builder.Services.AddScoped<ISolicitacaoService, SolicitacaoService>();
builder.Services.AddScoped<ITipoClienteService, TipoClienteService>();
builder.Services.AddScoped<ITipoSolicitacaoService, TipoSolicitacaoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<IContatoClienteRepository, ContatoClienteRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<IFaturamentoRepository, FaturamentoRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<INivelRepository, NivelRepository>();
builder.Services.AddScoped<ISituacaoClienteRepository, SituacaoClienteRepository>();
builder.Services.AddScoped<ISolicitacaoRepository, SolicitacaoRepository>();
builder.Services.AddScoped<ITipoClienteRepository, TipoClienteRepository>();
builder.Services.AddScoped<ITipoSolicitacaoRepository, TipoSolicitacaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IMenuUsuarioRepository, MenuUsuarioRepository>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Habilitar servir arquivos estáticos de pastas legadas diretamente
app.UseStaticFiles();

string[] legacyStaticFolders = { "Content", "css", "js", "img", "fonts" };
foreach (var folder in legacyStaticFolders)
{
    string folderPath = Path.Combine(app.Environment.ContentRootPath, folder);
    if (Directory.Exists(folderPath))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(folderPath),
            RequestPath = $"/{folder}"
        });
    }
}

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Minimal APIs para Gestão de Autenticação (Cookie-based no Blazor)
app.MapPost("/api/auth/login", async (
    HttpContext context,
    [FromForm] string username,
    [FromForm] string password,
    IUsuarioAppService usuarioApp) =>
{
    var response = new Millenium.Domain.Response.UsuarioResponse().RetornaMensagem(usuarioApp.AutenticarUsuario(username, password));
    if (!response.Existe)
    {
        return Results.Redirect("/Login?error=invalido");
    }

    var userId = response.Usuario.IdUsuario.ToString(System.Globalization.CultureInfo.InvariantCulture);
    context.Session.SetString("UsuarioLogado", userId);
    context.Session.SetString("NivelUsuarioLogado", response.Usuario.Nivel.IdNivel.ToString());
    
    var roles = response.Usuario.Nivel.Descricao;
    
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, response.Usuario.Nome),
        new Claim(ClaimTypes.Role, roles),
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim("sub", userId)
    };

    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var authProperties = new AuthenticationProperties
    {
        IsPersistent = false,
        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
    };

    await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(claimsIdentity),
        authProperties);

    return Results.Redirect("/Blazor/Dashboard");
});

app.MapGet("/api/auth/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Session.Remove("UsuarioLogado");
    context.Session.Remove("NivelUsuarioLogado");
    return Results.Redirect("/");
});

// Minimal API para Impressão de PDF (Laudo de Solicitação)
app.MapGet("/Pesquisa/Solicitacao/Imprimir", async (
    int id,
    ISolicitacaoAppService solicitacaoApp,
    IClienteAppService clienteApp) =>
{
    var solicitacao = solicitacaoApp.GetById(id);
    if (solicitacao == null) return Results.NotFound();

    if (solicitacao.Cliente == null && solicitacao.IdCliente.HasValue)
    {
        solicitacao.Cliente = clienteApp.GetById(solicitacao.IdCliente.Value);
    }

    var pdfDocument = new Document();
    var pdfFile = Path.Combine(Path.GetTempPath(), $"solicitacao_{id}.pdf");
    
    try
    {
        using (var fs = new FileStream(pdfFile, FileMode.Create))
        {
            var pdfWriter = PdfWriter.GetInstance(pdfDocument, fs);
            pdfWriter.PageEvent = new Site.Models.PDFFooter();
            pdfDocument.Open();

            FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
            var font = FontFactory.GetFont("Times-Italic", 14);

            var paragraph = new Paragraph("CONFIDENCIAL", FontFactory.GetFont("Times-Italic", 22, 1, BaseColor.RED))
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("Data da Solicitação: " + solicitacao.DataHoraCriacao.ToString("dd/MM/yyyy"), font)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            pdfDocument.Add(paragraph);

            paragraph = new Paragraph("Nº: " + solicitacao.NumeroSequencial, font)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("NOME: " + solicitacao.Nome, font)
            {
                Alignment = Element.ALIGN_JUSTIFIED
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("LOCAL DE NASCIMENTO: " + solicitacao.Local, font)
            {
                Alignment = Element.ALIGN_JUSTIFIED
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("NASCIMENTO: " + solicitacao.DataNascimento.ToString("dd/MM/yyyy"), font)
            {
                Alignment = Element.ALIGN_JUSTIFIED
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("NOME DA MÃE: " + solicitacao.NomeMae, font)
            {
                Alignment = Element.ALIGN_JUSTIFIED
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("NOME DO PAI: " + solicitacao.NomePai, font)
            {
                Alignment = Element.ALIGN_JUSTIFIED
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("IDENTIDADE: " + solicitacao.Rg, font)
            {
                Alignment = Element.ALIGN_JUSTIFIED
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("CPF: " + solicitacao.Cpf, font)
            {
                Alignment = Element.ALIGN_JUSTIFIED
            };
            pdfDocument.Add(paragraph);
            pdfDocument.Add(Chunk.NEWLINE);

            paragraph = new Paragraph("PESQUISA SOCIAL: " + solicitacao.Resposta, font)
            {
                Alignment = Element.ALIGN_JUSTIFIED
            };
            pdfDocument.Add(paragraph);

            pdfDocument.Close();
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(pdfFile);
        try { System.IO.File.Delete(pdfFile); } catch { }
        return Results.File(fileBytes, "application/pdf", "solicitacao.pdf");
    }
    catch (Exception)
    {
        if (pdfDocument.IsOpen()) pdfDocument.Close();
        try { if (System.IO.File.Exists(pdfFile)) System.IO.File.Delete(pdfFile); } catch { }
        return Results.StatusCode(500);
    }
});


app.MapRazorComponents<Site.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();

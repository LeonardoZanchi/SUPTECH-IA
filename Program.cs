using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 1) COLE AQUI a URL pública do seu bot (do Publish/Share do Typebot.io)
//    Exemplo: "https://seu-dominio-ou-typebot.io/seu-bot-id"
//    Você também pode colocar a variável de ambiente TYPEBOT_PUBLIC_URL para não precisar recompilar.
string typebotPublicUrl =
    Environment.GetEnvironmentVariable("TYPEBOT_PUBLIC_URL")
    ?? "https://typebot.co/my-typebot-x6bdxm9";

// 2) Exemplo de variáveis que você quer pré-preencher via query params
//    No Typebot, mapeie essas variáveis para ler parâmetros da URL (ex.: "nome", "email").
string nome = "Joao da Silva";
string email = "joao.silva@example.com";

// 3) Monta a URL final do iframe com query params
string iframeSrc = $"{typebotPublicUrl}?nome={Uri.EscapeDataString(nome)}&email={Uri.EscapeDataString(email)}";

app.MapGet("/", async context =>
{
    string html = $@"<!doctype html>
<html lang=""pt-br"">
<head>
  <meta charset=""utf-8"" />
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
  <title>Typebot Embed Demo</title>
  <style>
    html, body {{
      height: 100%;
      margin: 0;
      font-family: Arial, sans-serif;
      background: #f5f7fa;
    }}
    header {{
      padding: 12px 16px;
      background: #0f172a;
      color: #fff;
    }}
    .container {{
      height: calc(100% - 56px);
      padding: 0;
    }}
    .frame-wrap {{
      height: 100%;
    }}
    iframe {{
      width: 100%;
      height: 100%;
      border: 0;
      border-radius: 0;
      background: #fff;
    }}
    .info {{
      padding: 8px 16px;
      background: #e2e8f0;
      color: #0f172a;
      font-size: 14px;
    }}
    code {{
      background: #fff;
      padding: 2px 6px;
      border-radius: 4px;
    }}
  </style>
</head>
<body>
  <header>
    <strong>Typebot Embed Demo</strong>
  </header>

  <div class=""info"">
    Carregando bot de: <code>{WebUtility.HtmlEncode(typebotPublicUrl)}</code><br/>
    Variáveis passadas na URL: <code>nome={WebUtility.HtmlEncode(nome)}</code>, <code>email={WebUtility.HtmlEncode(email)}</code>
  </div>

  <div class=""container"">
    <div class=""frame-wrap"">
      <!-- Iframe do Typebot -->
      <iframe
        src=""{WebUtility.HtmlEncode(iframeSrc)}""
        allow=""microphone; camera; clipboard-read; clipboard-write; geolocation""
        loading=""lazy""
        referrerpolicy=""no-referrer-when-downgrade""
      ></iframe>
    </div>
  </div>
</body>
</html>";

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(html);
});

// Porta fixa para facilitar o teste local
app.Urls.Add("http://localhost:5177");

// Habilite HTTPS se quiser (ex.: app.Urls.Add("https://localhost:7177");) e rodar com certificado dev
app.Run();
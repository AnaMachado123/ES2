var builder = WebApplication.CreateBuilder(args);

// Razor Pages e dependências
builder.Services.AddRazorPages();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor(); // Para aceder ao contexto da requisição

// ✅ Sessão com timeout de 2 horas
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // ← mantém login ativo por 2h
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ HttpClient genérico (opcional)
builder.Services.AddHttpClient("Backend", client =>
{
    client.BaseAddress = new Uri("http://localhost:5176/"); // URL do teu backend
});

// ✅ HttpClient com JWT automático (para TarefaService)
builder.Services.AddHttpClient<front.Services.TarefaService>((provider, client) =>
{
    var accessor = provider.GetRequiredService<IHttpContextAccessor>();
    var context = accessor.HttpContext;

    var token = context?.Request.Cookies["jwt"];
    if (!string.IsNullOrEmpty(token))
    {
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    client.BaseAddress = new Uri("http://localhost:5176/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Ordem correta do middleware
app.UseSession();       // ← Tem de vir antes do Razor Pages
app.UseAuthorization();

app.MapRazorPages();

app.Run();

var builder = WebApplication.CreateBuilder(args);

// Razor Pages e dependências
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor(); // ✅ necessário para ler o cookie do contexto

// Registar o HttpClient nomeado (opcional, genérico)
builder.Services.AddHttpClient("Backend", client =>
{
    client.BaseAddress = new Uri("http://localhost:5176/"); // URL do backend
});

// ✅ Registar o TarefaService com HttpClient que envia automaticamente o token JWT
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

app.UseAuthorization();
app.UseSession(); // ✅ middleware de sessão deve vir antes de MapRazorPages

app.MapRazorPages();

app.Run();

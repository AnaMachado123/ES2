var builder = WebApplication.CreateBuilder(args);

// Razor Pages e dependências
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddAuthorization();

// Registar o HttpClient nomeado (opcional)
builder.Services.AddHttpClient("Backend", client =>
{
    client.BaseAddress = new Uri("http://localhost:5176/"); // URL do backend
});

// Registar o TarefaService com HttpClient dedicado
builder.Services.AddHttpClient<front.Services.TarefaService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5176/"); // URL da tua Web API
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
app.UseSession(); // Middleware de sessão

app.MapRazorPages();
app.Run();

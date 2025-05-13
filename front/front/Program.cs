var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient("Backend", client =>
{
    client.BaseAddress = new Uri("http://localhost:5176/");
});
builder.Services.AddAuthorization();
builder.Services.AddSession(); 

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
app.UseSession(); // ✅ middleware de sessão

app.MapRazorPages();

app.Run();

var builder = WebApplication.CreateBuilder(args);

// Registrar HttpClient con base URL de la API
builder.Services.AddHttpClient("ECommerceApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5160/"); // Cambia el puerto según donde corra tu API
});

// Agregar MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware para manejar errores y HSTS en producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Sirve archivos estáticos (css, js, imágenes)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Ruta por defecto MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

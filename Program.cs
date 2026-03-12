// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Cambiar la ruta por defecto para que inicie en Carrito/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Carritos}/{action=Index}/{id?}");

app.Run();

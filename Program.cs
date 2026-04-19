var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// MVC
builder.Services.AddControllersWithViews();

// Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Auth (needed later)
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth";
        options.AccessDeniedPath = "/";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

// MVC routes (for "/")
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Blazor hub
app.MapBlazorHub();

// Blazor fallback ONLY for /admin & /editor
app.MapFallbackToPage("/admin/{*path:nonfile}", "/_Host");
app.MapFallbackToPage("/editor/{*path:nonfile}", "/_Host");

app.Run();

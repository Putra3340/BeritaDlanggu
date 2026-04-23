using BeritaDlanggu.Models;
using BeritaDlanggu.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// MVC
builder.Services.AddControllersWithViews();

// Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 30 * 1024 * 1024;
    });

// Auth (needed later)
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Auth";
        options.AccessDeniedPath = "/";
    });

builder.Services.AddAuthorization();
builder.Services.AddDbContext<BeritaDlangguNetContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<NotificationService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

    app.UseSwagger();
    app.UseSwaggerUI();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
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

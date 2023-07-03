using Microsoft.EntityFrameworkCore;
using ProductDataManager.Infrastructure;
using ProductDataManager.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddLocalization();

builder.Services.AddDbContext<ProductContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProductConnection"), options =>
        options.MigrationsAssembly(typeof(ProductContext).Assembly.FullName));
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ProductContext>();

await context.Database.MigrateAsync();


if (!app.Environment.IsDevelopment())
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRequestLocalization(options =>
    options.AddSupportedCultures("en", "it-IT").AddSupportedUICultures("en", "it-IT").SetDefaultCulture("en"));
app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
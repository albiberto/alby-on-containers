using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductDataManager.Components;
using MudBlazor.Services;
using ProductDataManager.Components.Validators;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Infrastructure;
using ProductDataManager.Infrastructure.Interceptors;
using ProductDataManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<CategoryValidator>();
builder.Services.AddScoped<DescriptionValidator>();
builder.Services.AddSingleton<IInterceptor, AuditableInterceptor>();

builder.Services.AddDbContext<ProductContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProductConnection"), o =>
        o.MigrationsAssembly(typeof(ProductContext).Assembly.FullName));
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IDescriptionRepository, DescriptionsRepository>();

builder.Services.AddMudServices();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ProductContext>();

await context.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
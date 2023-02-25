namespace AlbyOnContainers.Library.Database;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;

public static class WebApplicationBuilderExtensions
{
    public static async Task<IHost> MigrateContextAsync<TContext>(this IHost host, Func<TContext, IServiceProvider, Task>? seeder = default, int retries = 10) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var provider = scope.ServiceProvider;

        var logger = provider.GetRequiredService<ILogger<TContext>>();
        var context = provider.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            var retry = Policy.Handle<NpgsqlException>()
                .WaitAndRetryAsync(retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, _, r, _) => logger.LogWarning(exception, "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}", nameof(TContext), exception.GetType().Name,
                        exception.Message, r, retries)
                );

            await retry.ExecuteAsync(async () => await InvokeSeederAsync(seeder, context, provider));

            logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
        }

        return host;
    }

    private static async Task InvokeSeederAsync<TContext>(Func<TContext, IServiceProvider, Task>? seeder, TContext context, IServiceProvider services) where TContext : DbContext
    {
        await context.Database.MigrateAsync();
        if (seeder is not null) await seeder(context, services);
    }
}
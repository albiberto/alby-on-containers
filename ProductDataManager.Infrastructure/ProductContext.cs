namespace ProductDataManager.Infrastructure;

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Domain.Aggregates.AttributeAggregate;
using Domain.Aggregates.CategoryAggregate;
using Domain.Aggregates.DescriptionAggregate;
using Domain.Aggregates.ProductAggregate;
using Domain.SeedWork;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

public sealed class ProductContext(DbContextOptions<ProductContext> options, IEnumerable<IInterceptor> interceptors) : ObservableDbContext(options)
{
    public DbSet<Category> Categories { get; private set; } = null!;
    public DbSet<DescriptionType> DescriptionTypes { get; private set; } = null!;
    public DbSet<DescriptionValue> DescriptionValues { get; private set; } = null!;
    public DbSet<DescriptionTypeCategory> DescriptionTypesCategories { get; private set; } = null!;
    public DbSet<AttributeType> AttributeTypes { get; private set; } = null!;
    public DbSet<Attribute> Attributes { get; private set; } = null!;
    public DbSet<Product> Products { get; private set; } = null!;
    public DbSet<ProductAttribute> ProductsAttributes { get; private set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(interceptors);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder
            .Entity<DescriptionType>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder
            .Entity<DescriptionType>()
            .Property(type => type.Name)
            .HasMaxLength(30);

        modelBuilder
            .Entity<DescriptionType>()
            .Property(type => type.Description)
            .HasMaxLength(100);
    }
}

public class ObservableDbContext : DbContext
{
    readonly SourceCache<EntityEntry, object> changes = new(entry => entry.Entity);
    
    public IObservableCache<EntityEntry, object> Changes => changes;
    
    public IReactiveProperty<bool> HasChanges { get; }

    public sealed override ChangeTracker ChangeTracker => base.ChangeTracker;

    public ObservableDbContext(DbContextOptions options) : base(options)
    {
        HasChanges = Changes
            .Connect(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToCollection()
            .Select(static entries => entries.Count > 0)
            .ToProperty(false);
        
        ChangeTracker.Tracked += (_, args) =>
        {
            if (args.Entry.State is not EntityState.Detached)
            {
                AddEntry(args.Entry);
            }
        };

        ChangeTracker.StateChanged += (_, args) =>
        {
            if (args.Entry.State is EntityState.Detached)
            {
                RemoveEntry(args);
            }
            else if (args.Entry.State is EntityState.Added)
            {
                AddEntry(args.Entry);
            }
            else if (args.Entry.State is EntityState.Deleted)
            {
                ChangeTracker.Cascade(args.Entry.Entity, node =>
                {
                    if (node.Entry.State is EntityState.Detached or EntityState.Added && node.SourceEntry is { } parent && node.InboundNavigation?.GetCollectionAccessor() is { } collection)
                    {
                        collection.Remove(parent.Entity, node.Entry.Entity);
                    }

                    return true;
                });
                
                args.Entry.CurrentValues.SetValues(args.Entry.OriginalValues);
                changes.AddOrUpdate(args.Entry);
            } 
            else
            {
                changes.AddOrUpdate(args.Entry);
            }

            NotifyParents(args.Entry);
        };

        return;

        void AddEntry(EntityEntry entry)
        {
            changes.AddOrUpdate(entry);

            if (entry.Entity is INotifyPropertyChanged propertyChanged)
            {
                propertyChanged.PropertyChanged += PropertyChanged;
            }
        }

        void RemoveEntry(EntityStateChangedEventArgs args)
        {
            changes.Remove(args.Entry);

            if (args.Entry.Entity is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= PropertyChanged;
            }
        }

        void PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is { } entity)
            {
                var entry = Entry(entity);
                
                if (entry.State is EntityState.Modified)
                {
                    if (entry.Properties.All(p => p.Metadata.GetValueComparer().Equals(p.CurrentValue, p.OriginalValue)))
                    {
                        entry.State = EntityState.Unchanged;
                    }
                }

                if (entry.State is not EntityState.Detached)
                {
                    changes.AddOrUpdate(entry);
                    NotifyParents(entry);
                }
            }
        }

        void NotifyParents(EntityEntry entry) 
            => ChangeTracker.TraverseParents(entry.Entity, node =>
            {
                changes.AddOrUpdate(node.Entry);
                return true;
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

#pragma warning disable EF1001
        optionsBuilder.ReplaceService<IChangeDetector, ChangeDetector>();
#pragma warning restore EF1001
    }

#pragma warning disable EF1001
    public class ChangeDetector(IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> logger, ILoggingOptions loggingOptions) : Microsoft.EntityFrameworkCore.ChangeTracking.Internal.ChangeDetector(logger, loggingOptions)
    {
        public override void PropertyChanged(InternalEntityEntry entry, IPropertyBase propertyBase, bool setModified)
        {
            if (propertyBase is IProperty property && property.GetValueComparer().Equals(entry.GetCurrentValue(property), entry.GetOriginalValue(property)))
            {
                setModified = false;
            }

            base.PropertyChanged(entry, propertyBase, setModified);
        }
    }
#pragma warning restore EF1001

    public override void Dispose()
    {
        Unsubscribe();
        base.Dispose();
    }

    void Unsubscribe()
    {
        HasChanges.Dispose();
        changes.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        Unsubscribe();
        return base.DisposeAsync();
    }
}

public static class ObservableExtensions
{
    public static IReactiveProperty<T> ToProperty<T>(this IObservable<T> source, T initialValue) 
        => new ConnectedProperty<T>(initialValue, source);
    
    public static IReadOnlyObservableCollection<T> ToObservableCollection<T, TKey>(this IObservable<IChangeSet<T, TKey>> source) 
        where T : notnull 
        where TKey : notnull
        => new ConnectedCollection<T, TKey>(source);
    
    sealed class ConnectedProperty<T> : ReactiveProperty<T>
    {
        readonly IDisposable subscription;

        public ConnectedProperty(T initialValue, IObservable<T> source)
            : base(initialValue)
        {
            subscription = source.Subscribe(value => Value = value);
        }
        
        protected override void Dispose(bool disposing)
        {
            subscription.Dispose();
            base.Dispose(disposing);
        }
    }
    
    sealed class ConnectedCollection<T, TKey> : IReadOnlyObservableCollection<T>
        where T : notnull 
        where TKey : notnull
    {
        readonly IDisposable subscription;
        readonly ReadOnlyObservableCollection<T> items;

        public ConnectedCollection(IObservable<IChangeSet<T, TKey>> source)
        {
            subscription = source.Bind(out items).Subscribe();
        }
        
        public IEnumerator<T> GetEnumerator()
            => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)items).GetEnumerator();

        public int Count => items.Count;

        public T this[int index] => items[index];

        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => ((INotifyCollectionChanged)items).CollectionChanged += value;
            remove => ((INotifyCollectionChanged)items).CollectionChanged -= value;
        }

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add => ((INotifyPropertyChanged)items).PropertyChanged += value;
            remove => ((INotifyPropertyChanged)items).PropertyChanged -= value;
        }

        public void Dispose() => subscription.Dispose();
    }
    
    public static IObservable<IChangeSet<TDestination, TKey>> Transform<TSource, TDestination, TKey>(this IObservable<IChangeSet<TSource, TKey>> source, Func<TSource, TDestination> create, Func<TDestination, TSource, TDestination> update)
        where TSource : notnull 
        where TDestination : notnull 
        where TKey : notnull
        => source.Scan(
                (ChangeAwareCache<TDestination, TKey>?)null,
                (cache, changes) =>
                {
                    cache ??= new ChangeAwareCache<TDestination, TKey>(changes.Count);

                    foreach (var change in changes)
                    {
                        switch (change.Reason)
                        {
                            case ChangeReason.Add:
                                cache.AddOrUpdate(create(change.Current), change.Key);
                                break;
                            case ChangeReason.Update:
                                var previous = cache.Lookup(change.Key);
                                cache.AddOrUpdate(update(previous.Value, change.Current), change.Key);
                                break;
                            case ChangeReason.Remove:
                                cache.Remove(change.Key);
                                break;
                            case ChangeReason.Refresh:
                                cache.Refresh(change.Key);
                                break;
                        }
                    }

                    return cache;
                })
            .Where(x => x is not null)
            .Select(cache => cache!.CaptureChanges());
}

public interface IReadOnlyObservableCollection<out T> : IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable;

public static class EfExtensions
{
    public static void DiscardChanges(this DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            entry.DiscardChanges();
        }
    }

    public static void DiscardChanges(this EntityEntry entry)
    {
        switch (entry.State)
        {
            case EntityState.Modified:
                entry.State = EntityState.Unchanged;
                break;
            case EntityState.Deleted:
                entry.State = EntityState.Unchanged;
                entry.Context.ChangeTracker.Cascade(entry.Entity, node =>
                {
                    if (node.Entry.State is EntityState.Deleted)
                    {
                        node.Entry.State = EntityState.Unchanged;
                        return true;
                    }
                    
                    return false;
                });

                break;
            case EntityState.Added:
                entry.State = EntityState.Detached;
                break;
        }
    }
    
    public static void Cascade(this ChangeTracker changeTracker, object rootEntity, Func<EntityEntryGraphNode, bool> callback)
    {
        if (changeTracker.CascadeDeleteTiming is CascadeTiming.Immediate)
        {
            changeTracker.TraverseDescendants(rootEntity, node => node.InboundNavigation is INavigation { ForeignKey.DeleteBehavior: DeleteBehavior.Cascade or DeleteBehavior.ClientCascade } && callback(node));
        }
    }

    public static void TraverseDescendants(this ChangeTracker changeTracker, object rootEntity, Func<EntityEntryGraphNode, bool> callback)
        => changeTracker.Traverse(rootEntity, node => node.InboundNavigation is INavigation { IsOnDependent: false } && callback(node));

    public static void TraverseParents(this ChangeTracker changeTracker, object rootEntity, Func<EntityEntryGraphNode, bool> callback)
        => changeTracker.Traverse(rootEntity, node => node.InboundNavigation is INavigation { IsOnDependent: true } && callback(node));
    
    public static void Traverse(this ChangeTracker changeTracker, object rootEntity, Func<EntityEntryGraphNode, bool> callback)
    {
        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);

        changeTracker.TrackGraph(rootEntity, (object?)null, node =>
        {
            if (!visited.Add(node.Entry.Entity))
            {
                return false;
            }
            
            return ReferenceEquals(node.Entry.Entity, rootEntity) || callback(node);
        });
    }
}

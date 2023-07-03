namespace ProductDataManager.Infrastructure.Abstract;

public abstract class Entity : Auditable
{
    public Guid? Id { get; set; }
}
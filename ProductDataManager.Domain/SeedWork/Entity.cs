namespace ProductDataManager.Domain.SeedWork;

public abstract record Entity(Guid? Id) : Auditable;

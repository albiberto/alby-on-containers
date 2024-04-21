namespace ProductDataManager.Infrastructure.Abstract;

public abstract record Entity(Guid? Id) : Auditable;

namespace Core.Domain;

public interface IRepository<T> where T : IAggregate;
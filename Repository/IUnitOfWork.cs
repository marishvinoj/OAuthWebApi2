using System.Data.Entity;

namespace Repository
{
    public interface IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        TContext Context { get; }
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}
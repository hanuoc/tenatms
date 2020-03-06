namespace TMS.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace TMS.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private TMSDbContext dbContext;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
        public TMSDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }
        public void Commit()
        {
            try
            {
				DbContext.Database.CommandTimeout = Common.Constants.CommonConstants.TimeExcuteSql;
				DbContext.SaveChanges();
            }
            catch(DbUpdateException dbex)
            {
                log.Error(Common.Constants.StringConstants.LogErrorSaveDataBase + dbex);
                var entry = dbex.Entries.Single();
                throw new DbUpdateException(entry.State.ToString());
            }
            catch(DbEntityValidationException ex)
            {
                log.Error(Common.Constants.StringConstants.LogErrorSaveDataBase + ex);
                Exception raise = ex;
                foreach(var validationErrors in ex.EntityValidationErrors)
                {
                    foreach(var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch(NullReferenceException nullex)
            {
                log.Error(Common.Constants.StringConstants.LogErrorSaveDataBase + nullex);
                throw new NullReferenceException(nullex.Message);
            }
        }
    }
}
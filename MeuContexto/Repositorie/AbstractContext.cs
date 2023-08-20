using Entities.Interfaces;
using MeuContexto.Context;
using MeuContexto.UOW;

namespace MeuContexto.Repositorie
{
    public class AbstractContext
    {               
        public IUnitOfWork AbstractContextReturn(RepositoryType repositoryType, string? connection = null, AppDbContext? appDbContext = null)
        {
            switch (repositoryType)
            {
                case RepositoryType.Dapper:
                    return new UnitOfWork();
                default:
                    return new UnitOfWork(appDbContext);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces.Services
{
    public interface IunitOfWork
    {
        Task<int> SaveChangesAsync();

        //Task BeginTransactionAsync();

        //Task CommitAsync();

        //Task RollbackAsync();
    }
}

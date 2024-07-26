using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }

        Task<int> Complete();
    }
}

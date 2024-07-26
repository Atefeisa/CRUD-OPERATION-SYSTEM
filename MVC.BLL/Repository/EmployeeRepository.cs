using Microsoft.EntityFrameworkCore;
using MVC.BLL.Interfaces;
using MVC.DAL.Data;
using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Repository
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        public EmployeeRepository( AppDbContext context ) : base ( context ) // ASK CLR To Create Object From AppDbContext
        {
            
        }

        public IEnumerable<Employee> GetByName(string name)
        {
            return _context.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower())).Include(E => E.Department);

        }
    }
}

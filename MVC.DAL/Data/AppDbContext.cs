using MVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MVC.DAL.Data
{
	public class AppDbContext :IdentityDbContext<ApplicationUser>
	{

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// modelBuilder.ApplyConfiguration(new DepartmentCofiguration());

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


			base.OnModelCreating(modelBuilder);

		}

		 public DbSet<Department> Department { get; set; }

		public DbSet<Employee> Employees { get; set; }
	


	}
}

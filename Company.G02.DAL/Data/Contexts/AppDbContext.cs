using Company.G02.DAL.Data.Configurations;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Data.Contexts
{
    public class AppDbContext:IdentityDbContext <ApplicationUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.ApplyConfiguration(new DepartmentConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee > Employees { get; set; }

      



    }
}

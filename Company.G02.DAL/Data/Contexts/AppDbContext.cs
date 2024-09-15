﻿using Company.G02.DAL.Data.Configurations;
using Company.G02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Data.Contexts
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Server= . ; DataBase =  CompanyG02 ; Trusted_Connection =true;TrustServerCertificate=true");

        //}

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

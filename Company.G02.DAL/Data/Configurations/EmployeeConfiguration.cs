﻿using Company.G02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Data.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {

            builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(E => E.Salary).HasColumnType("decimal(18,2)");
            builder.Property(E => E.DateOfCreation).HasDefaultValueSql("getdate()");


        }
    }
}
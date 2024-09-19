using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL
{
    public class UnitOfWork : IUnitOfwork
    {
        private readonly AppDbContext _context;
        private IDepartmentRepository _departmentRepository;
        private IEmployeeRepository _employeeRepository;
        public UnitOfWork(AppDbContext context)
        {
            _departmentRepository = new DepartmentRepository(context);
            _employeeRepository = new EmployeeRepository(context);
            _context = context;
        }
        public IDepartmentRepository DepartmentRepository => _departmentRepository;

        public IEmployeeRepository EmployeeRepository => _employeeRepository;

        public int SaveChange()
        {
            return _context.SaveChanges();
        }
    }
}

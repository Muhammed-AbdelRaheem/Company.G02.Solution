using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Repositories
{
    public class DepartmentRepository :GenericRepository<Department>, IDepartmentRepository
    {
        //private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context):base(context)
        {
            //_context = context;
        }

        //public IEnumerable<Department> GetAll()
        //{
        //    return _context.Departments.ToList();
        //}

        //Department IDepartmentRepository.Get(int id)
        //{
        //    return _context.Departments.Find(id);
        //}
        //int IDepartmentRepository.Add(Department entity)
        //{
        //    _context.Departments.Add(entity);

        //    return _context.SaveChanges();
        //}

        //int IDepartmentRepository.Update(Department entity)
        //{
        //    _context.Departments.Update(entity);

        //    return _context.SaveChanges();
        //}

        //int IDepartmentRepository.Delete(Department entity)
        //{
        //    _context.Departments.Remove(entity);

        //    return _context.SaveChanges();
        //}



    }
}

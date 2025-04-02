using CompanyModels.Interface;
using CompanyModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabaseBenchmark {

    public class EfCompanyBenchmarkRepository : ICompanyBenchmark {
        private readonly CompanyBenchmarkContext _context;

        public EfCompanyBenchmarkRepository(CompanyBenchmarkContext context) {
            _context = context;
        }

        public void InsertDepartment(Department department) {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        public List<Department> GetDepartments() {
            return _context.Departments.ToList();
        }

        public void UpdateDepartment(Department department) {
            _context.Departments.Update(department);
            _context.SaveChanges();
        }

        public void DeleteDepartment(int departmentNumber) {
            var department = _context.Departments.Find(departmentNumber);
            if (department != null) {
                _context.Departments.Remove(department);
                _context.SaveChanges();
            }
        }
    }

}

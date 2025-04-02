using EntityBenchmark.CompanyModels.Interface;
using EntityBenchmark.CompanyModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EntityBenchmark {
    public class EntityDatabaseBenchmark : ICompanyBenchmark {
        private readonly CompanyBenchmarkContext _context;

        public EntityDatabaseBenchmark(CompanyBenchmarkContext context) {
            _context = context;
        }

        // Department methods
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

        // Employee methods
        public void InsertEmployee(Employee employee) {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public List<Employee> GetEmployees() {
            return _context.Employees.ToList();
        }

        public void UpdateEmployee(Employee employee) {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(string ssn) {
            var employee = _context.Employees.Find(ssn);
            if (employee != null) {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        // DeptLocations methods
        public void InsertDeptLocation(DeptLocations deptLocation) {
            _context.DeptLocations.Add(deptLocation);
            _context.SaveChanges();
        }

        public List<DeptLocations> GetDeptLocations() {
            return _context.DeptLocations.ToList();
        }

        public void UpdateDeptLocation(DeptLocations deptLocation) {
            _context.DeptLocations.Update(deptLocation);
            _context.SaveChanges();
        }

        public void DeleteDeptLocation(int dnumber, string dlocation) {
            var deptLocation = _context.DeptLocations.Find(dnumber, dlocation);
            if (deptLocation != null) {
                _context.DeptLocations.Remove(deptLocation);
                _context.SaveChanges();
            }
        }

        // Project methods
        public void InsertProject(Project project) {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        public List<Project> GetProjects() {
            return _context.Projects.ToList();
        }

        public void UpdateProject(Project project) {
            _context.Projects.Update(project);
            _context.SaveChanges();
        }

        public void DeleteProject(int pnumber) {
            var project = _context.Projects.Find(pnumber);
            if (project != null) {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
        }

        // WorksOn methods
        public void InsertWorksOn(WorksOn worksOn) {
            _context.WorksOn.Add(worksOn);
            _context.SaveChanges();
        }

        public List<WorksOn> GetWorksOn() {
            return _context.WorksOn.ToList();
        }

        public void UpdateWorksOn(WorksOn worksOn) {
            _context.WorksOn.Update(worksOn);
            _context.SaveChanges();
        }

        public void DeleteWorksOn(string essn, int pno) {
            var worksOn = _context.WorksOn.Find(essn, pno);
            if (worksOn != null) {
                _context.WorksOn.Remove(worksOn);
                _context.SaveChanges();
            }
        }

        // Dependent methods
        public void InsertDependent(Dependent dependent) {
            _context.Dependents.Add(dependent);
            _context.SaveChanges();
        }

        public List<Dependent> GetDependents() {
            return _context.Dependents.ToList();
        }

        public void UpdateDependent(Dependent dependent) {
            _context.Dependents.Update(dependent);
            _context.SaveChanges();
        }

        public void DeleteDependent(string essn, string dependentName) {
            var dependent = _context.Dependents.Find(essn, dependentName);
            if (dependent != null) {
                _context.Dependents.Remove(dependent);
                _context.SaveChanges();
            }
        }
    }
}

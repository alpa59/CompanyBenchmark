using EntityBenchmark.CompanyModels.Models;
using System.Collections.Generic;

namespace EntityBenchmark.CompanyModels.Interface {
    public interface ICompanyBenchmark {
        // Department methods
        void InsertDepartment(Department department);
        List<Department> GetDepartments();
        void UpdateDepartment(Department department);
        void DeleteDepartment(int departmentNumber);

        // Employee methods
        void InsertEmployee(Employee employee);
        List<Employee> GetEmployees();
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(string ssn);

        // DeptLocations methods
        void InsertDeptLocation(DeptLocations deptLocation);
        List<DeptLocations> GetDeptLocations();
        void UpdateDeptLocation(DeptLocations deptLocation);
        void DeleteDeptLocation(int dnumber, string dlocation);

        // Project methods
        void InsertProject(Project project);
        List<Project> GetProjects();
        void UpdateProject(Project project);
        void DeleteProject(int pnumber);

        // WorksOn methods
        void InsertWorksOn(WorksOn worksOn);
        List<WorksOn> GetWorksOn();
        void UpdateWorksOn(WorksOn worksOn);
        void DeleteWorksOn(string essn, int pno);

        // Dependent methods
        void InsertDependent(Dependent dependent);
        List<Dependent> GetDependents();
        void UpdateDependent(Dependent dependent);
        void DeleteDependent(string essn, string dependentName);
    }
}
using EntityBenchmark.CompanyModels.Models;
using System.Diagnostics;

namespace EntityBenchmark {
    public class BenchmarkRunner {
        public void RunBenchmarks() {
            var context = new CompanyBenchmarkContext();
            var repository = new EntityDatabaseBenchmark(context);

            var benchmarks = new List<BenchmarkResult>();

            // Benchmark InsertDepartment
            var department = new Department { Dname = "TestDept", Mgrssn = "123456789", MgrStartDate = DateTime.Now };
            benchmarks.Add(BenchmarkMethod(() => repository.InsertDepartment(department), "InsertDepartment"));

            // Benchmark GetDepartments
            benchmarks.Add(BenchmarkMethod(() => repository.GetDepartments(), "GetDepartments"));

            // Benchmark UpdateDepartment
            department.Dname = "UpdatedDept";
            benchmarks.Add(BenchmarkMethod(() => repository.UpdateDepartment(department), "UpdateDepartment"));

            // Benchmark DeleteDepartment
            benchmarks.Add(BenchmarkMethod(() => repository.DeleteDepartment(department.Dnumber), "DeleteDepartment"));

            // Benchmark InsertEmployee
            var employee = new Employee { Fname = "John", Minit = 'D', Lname = "Doe", Ssn = "987654321", Bdate = DateTime.Now, Address = "123 Main St", Sex = 'M', Salary = 50000, Superssn = null, Dno = 10 };
            benchmarks.Add(BenchmarkMethod(() => repository.InsertEmployee(employee), "InsertEmployee"));

            // Benchmark GetEmployees
            benchmarks.Add(BenchmarkMethod(() => repository.GetEmployees(), "GetEmployees"));

            // Benchmark UpdateEmployee
            employee.Lname = "Smith";
            benchmarks.Add(BenchmarkMethod(() => repository.UpdateEmployee(employee), "UpdateEmployee"));

            // Benchmark DeleteEmployee
            benchmarks.Add(BenchmarkMethod(() => repository.DeleteEmployee(employee.Ssn), "DeleteEmployee"));

            // Benchmark InsertDeptLocation
            var deptLocation = new DeptLocations { Dnumber = 1, Dlocation = "TestLocation" };
            benchmarks.Add(BenchmarkMethod(() => repository.InsertDeptLocation(deptLocation), "InsertDeptLocation"));

            // Benchmark GetDeptLocations
            benchmarks.Add(BenchmarkMethod(() => repository.GetDeptLocations(), "GetDeptLocations"));

            // Benchmark UpdateDeptLocation
            deptLocation.Dlocation = "UpdatedLocation";
            benchmarks.Add(BenchmarkMethod(() => repository.UpdateDeptLocation(deptLocation), "UpdateDeptLocation"));

            // Benchmark DeleteDeptLocation
            benchmarks.Add(BenchmarkMethod(() => repository.DeleteDeptLocation(deptLocation.Dnumber, deptLocation.Dlocation), "DeleteDeptLocation"));

            // Benchmark InsertProject
            var project = new Project { Pname = "TestProject", Plocation = "TestLocation", Dnum = 10 };
            benchmarks.Add(BenchmarkMethod(() => repository.InsertProject(project), "InsertProject"));

            // Benchmark GetProjects
            benchmarks.Add(BenchmarkMethod(() => repository.GetProjects(), "GetProjects"));

            // Benchmark UpdateProject
            project.Pname = "UpdatedProject";
            benchmarks.Add(BenchmarkMethod(() => repository.UpdateProject(project), "UpdateProject"));

            // Benchmark DeleteProject
            benchmarks.Add(BenchmarkMethod(() => repository.DeleteProject(project.Pnumber), "DeleteProject"));

            // Benchmark InsertWorksOn
            var worksOn = new WorksOn { Essn = "987654321", Pno = 1, Hours = 40 };
            benchmarks.Add(BenchmarkMethod(() => repository.InsertWorksOn(worksOn), "InsertWorksOn"));

            // Benchmark GetWorksOn
            benchmarks.Add(BenchmarkMethod(() => repository.GetWorksOn(), "GetWorksOn"));

            // Benchmark UpdateWorksOn
            worksOn.Hours = 35;
            benchmarks.Add(BenchmarkMethod(() => repository.UpdateWorksOn(worksOn), "UpdateWorksOn"));

            // Benchmark DeleteWorksOn
            benchmarks.Add(BenchmarkMethod(() => repository.DeleteWorksOn(worksOn.Essn, worksOn.Pno), "DeleteWorksOn"));

            // Benchmark InsertDependent
            var dependent = new Dependent { Essn = "987654321", DependentName = "Jane", Sex = 'F', Bdate = DateTime.Now, Relationship = "Spouse" };
            benchmarks.Add(BenchmarkMethod(() => repository.InsertDependent(dependent), "InsertDependent"));

            // Benchmark GetDependents
            benchmarks.Add(BenchmarkMethod(() => repository.GetDependents(), "GetDependents"));

            // Benchmark UpdateDependent
            dependent.Relationship = "Child";
            benchmarks.Add(BenchmarkMethod(() => repository.UpdateDependent(dependent), "UpdateDependent"));

            // Benchmark DeleteDependent
            benchmarks.Add(BenchmarkMethod(() => repository.DeleteDependent(dependent.Essn, dependent.DependentName), "DeleteDependent"));

            // Print results to console
            foreach (var result in benchmarks) {
                Console.WriteLine($"{result.MethodName}: {result.ElapsedMilliseconds} ms");
            }

            // Write results to CSV file
            using (var writer = new StreamWriter("benchmark_results.csv")) {
                writer.WriteLine("MethodName,ElapsedMilliseconds");
                foreach (var result in benchmarks) {
                    writer.WriteLine($"{result.MethodName},{result.ElapsedMilliseconds}");
                }
            }
        }

        private BenchmarkResult BenchmarkMethod(Action method, string methodName) {
            var stopwatch = Stopwatch.StartNew();
            method();
            stopwatch.Stop();
            return new BenchmarkResult { MethodName = methodName, ElapsedMilliseconds = stopwatch.ElapsedMilliseconds };
        }

        private class BenchmarkResult {
            public string MethodName { get; set; }
            public long ElapsedMilliseconds { get; set; }
        }
    }
}

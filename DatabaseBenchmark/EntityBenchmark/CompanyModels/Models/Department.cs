using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityBenchmark.CompanyModels.Models {
    public class Department {
        [Key]
        public int Dnumber { get; set; }
        [Required, MaxLength(15)]
        public string Dname { get; set; }
        [Required]
        [ForeignKey("Manager")]
        public string Mgrssn { get; set; }
        public DateTime? MgrStartDate { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<DeptLocations> Locations { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}

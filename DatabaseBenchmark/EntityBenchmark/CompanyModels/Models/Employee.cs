using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityBenchmark.CompanyModels.Models {
    public class Employee {
        [Key]
        public string Ssn { get; set; }
        [Required, MaxLength(15)]
        public string Fname { get; set; }
        public char? Minit { get; set; }
        [Required, MaxLength(15)]
        public string Lname { get; set; }
        public DateTime? Bdate { get; set; }
        [MaxLength(30)]
        public string Address { get; set; }
        public char? Sex { get; set; }
        public decimal? Salary { get; set; }
        [ForeignKey("Supervisor")]
        public string Superssn { get; set; }
        [Required]
        public int Dno { get; set; }
        public Department Department { get; set; }
        public Employee Supervisor { get; set; }
        public ICollection<WorksOn> WorksOns { get; set; }
        public ICollection<Dependent> Dependents { get; set; }
    }
}

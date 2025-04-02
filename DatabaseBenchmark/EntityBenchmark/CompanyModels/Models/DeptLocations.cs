using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityBenchmark.CompanyModels.Models {
    public class DeptLocations {
        [Key, Column(Order = 0)]
        public int Dnumber { get; set; }
        [Key, Column(Order = 1)]
        [Required, MaxLength(15)]
        public string Dlocation { get; set; }
        public Department Department { get; set; }
    }
}

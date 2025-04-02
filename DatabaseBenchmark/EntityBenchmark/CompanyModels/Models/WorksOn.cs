using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityBenchmark.CompanyModels.Models {
    public class WorksOn {
        [Key, Column(Order = 0)]
        public string Essn { get; set; }
        [Key, Column(Order = 1)]
        public int Pno { get; set; }
        [Required]
        public decimal Hours { get; set; }
        public Employee Employee { get; set; }
        public Project Project { get; set; }
    }
}

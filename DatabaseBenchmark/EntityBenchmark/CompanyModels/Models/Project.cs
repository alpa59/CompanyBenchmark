using System.ComponentModel.DataAnnotations;

namespace EntityBenchmark.CompanyModels.Models {
    public class Project {
        [Key]
        public int Pnumber { get; set; }
        [Required, MaxLength(15)]
        public string Pname { get; set; }
        [MaxLength(15)]
        public string Plocation { get; set; }
        [Required]
        public int Dnum { get; set; }
        public Department Department { get; set; }
        public ICollection<WorksOn> WorksOns { get; set; }
    }
}

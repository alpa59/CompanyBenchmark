using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityBenchmark.CompanyModels.Models {
    public class Dependent {
        [Key, Column(Order = 0)]
        public string Essn { get; set; }
        [Key, Column(Order = 1)]
        [Required, MaxLength(15)]
        public string DependentName { get; set; }
        public char? Sex { get; set; }
        public DateTime? Bdate { get; set; }
        [MaxLength(8)]
        public string Relationship { get; set; }
        public Employee Employee { get; set; }
    }
}

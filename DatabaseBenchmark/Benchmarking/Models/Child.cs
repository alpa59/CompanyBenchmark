using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityBenchmark.Models {
    public class Child {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

    }
}
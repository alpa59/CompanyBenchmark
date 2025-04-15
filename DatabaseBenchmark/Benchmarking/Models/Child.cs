using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benchmarking.Models {
    public class Child {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }
        public int ParentId { get; set; }
        public Parent Parent { get; set; } = null!;
    }
}
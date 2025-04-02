using System;

namespace CompanyModels.Models {
    public class Dependent {
        public string Essn { get; set; }
        public string DependentName { get; set; }
        public char? Sex { get; set; }
        public DateTime Bdate { get; set; }
        public string Relationship { get; set; }
    }
}

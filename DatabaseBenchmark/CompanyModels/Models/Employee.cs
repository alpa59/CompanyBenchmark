using System;

namespace CompanyModels.Models {
    public class Employee {
        public string Fname { get; set; }
        public char? Minit { get; set; }
        public string Lname { get; set; }
        public string Ssn { get; set; }
        public DateTime Bdate { get; set; }
        public string Address { get; set; }
        public char? Sex { get; set; }
        public decimal Salary { get; set; }
        public string Superssn { get; set; }
        public int Dno { get; set; }
    }
}

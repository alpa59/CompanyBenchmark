using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyModels.Models {
    public class Department {
        public string Dname { get; set; }
        public int Dnumber { get; set; }
        public string Mgrssn { get; set; }
        public DateTime Mgrstartdate { get; set; }
    }
}

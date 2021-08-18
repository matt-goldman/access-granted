using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSW.Med_Man.MVC.Models
{
    public class Patient : BaseModel
    {
        public string firstName { get; set; }
        public string familyName { get; set; }

        public string FullName
        {
            get
            {
                return firstName + " " + familyName;
            }
        }

        public DateTime DOB { get; set; }
        public List<Prescription> prescriptions { get; set; }
        public List<Administrations> administrations { get; set; }
    }
}

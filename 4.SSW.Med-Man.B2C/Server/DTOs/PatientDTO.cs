using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSW.Med_Man.MVC.DTOs
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string FullName => string.Format("{0} {1}", GivenName, FamilyName);
        public DateTime DOB { get; set; }
        public List<PrescriptionDTO> Prescriptions { get; set; }
        public List<AdministrationDTO> Administrations { get; set; }
    }
}

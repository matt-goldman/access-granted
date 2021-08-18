using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSW.Med_Man.MVC.DTOs
{
    public class PrescriptionDTO
    {
        public int Id { get; set; }
        public PatientDTO Patient { get; set; }
        public MedicationDTO Medication { get; set; }
        public int Dose { get; set; }
    }
}

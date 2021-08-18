using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSW.Med_Man.MVC.Models
{
    public class Administrations : BaseModel
    {
        public int patientId { get; set; }
        public Patient patient { get; set; }
        public int medicationId { get; set; }
        public Medication medication { get; set; }
        public int dose { get; set; }
        public DateTime timeGiven { get; set; }
    }
}

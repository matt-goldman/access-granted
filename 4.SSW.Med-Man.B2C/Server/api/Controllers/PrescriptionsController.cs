using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSW.Med_Man.MVC.Data;
using SSW.Med_Man.MVC.DTOs;
using SSW.Med_Man.MVC.Models;

namespace SSW.Med_Man.MVC.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetPrescriptions()
        {
            List<PrescriptionDTO> prescriptions = new List<PrescriptionDTO>();

            var presList =  await _context.Prescriptions
                .Include(p => p.patient)
                .Include(p => p.medication)
                .ToListAsync();

            foreach (var pres in presList)
            {
                prescriptions.Add(new PrescriptionDTO
                {
                    Dose = pres.dose,
                    Id = pres.Id,
                    Medication = new MedicationDTO
                    {
                        Id = pres.medicationId,
                        Name = pres.medication.name
                    },
                    Patient = new PatientDTO
                    {
                        FamilyName = pres.patient.familyName,
                        GivenName = pres.patient.firstName,
                        Id = pres.patientId
                    }

                });
            }

            return prescriptions;
        }

        // GET: api/Prescriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescription(int id)
        {
            var pres = await _context.Prescriptions
                .Include(p => p.patient.FullName)
                .Include(p => p.medication.name)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pres == null)
            {
                return NotFound();
            }

            var prescription = new PrescriptionDTO
            {
                Dose = pres.dose,
                Id = pres.Id,
                Medication = new MedicationDTO
                {
                    Id = pres.medicationId,
                    Name = pres.medication.name
                },
                Patient = new PatientDTO
                {
                    FamilyName = pres.patient.familyName,
                    GivenName = pres.patient.firstName,
                    Id = pres.patientId
                }

            };

            return prescription;
        }

        // PUT: api/Prescriptions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> PutPrescription(int id, PrescriptionDTO prescription)
        {
            if (id != prescription.Id)
            {
                return BadRequest();
            }

            var pres = await  _context.Prescriptions.FirstOrDefaultAsync(p => p.Id == id);

            pres.medicationId = prescription.Medication.Id;
            pres.patientId = prescription.Patient.Id;
            pres.dose = prescription.Dose;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Prescriptions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<PrescriptionDTO>> PostPrescription(PrescriptionDTO prescription)
        {
            var dbPres = new Prescription
            {
                dose = prescription.Dose,
                patientId = prescription.Patient.Id,
                medicationId = prescription.Medication.Id
            };

            _context.Prescriptions.Add(dbPres);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrescription", new { id = dbPres.Id }, prescription);
        }

        // DELETE: api/Prescriptions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            var dto = new PrescriptionDTO
            {
                Dose = prescription.dose,
                Medication = new MedicationDTO
                {
                    Id = prescription.medicationId,
                    Name = prescription.medication.name
                },
                Id = id,
                Patient = new PatientDTO
                {
                    FamilyName = prescription.patient.familyName,
                    GivenName = prescription.patient.firstName,
                    Id = prescription.patientId,
                }
            };

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            return dto;
        }

        private bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.Id == id);
        }
    }
}

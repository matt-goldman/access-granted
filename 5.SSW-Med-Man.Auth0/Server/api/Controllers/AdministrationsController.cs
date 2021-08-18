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
    [Authorize(Roles="Nurse,Doctor")]
    public class AdministrationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdministrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Administrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdministrationDTO>>> GetAdministrations()
        {
            List<AdministrationDTO> administrations = new List<AdministrationDTO>();

            var dbMeds = await _context.Administrations
                .Include(a => a.patient)
                .Include(a => a.medication)
                .ToListAsync();

            foreach(var med in dbMeds)
            {
                administrations.Add(new AdministrationDTO
                {
                    Medication = new MedicationDTO
                    {
                        Id = med.medicationId,
                        Name = med.medication.name
                    },
                    Patient = new PatientDTO
                    {
                        Id = med.patientId,
                        GivenName = med.patient.firstName,
                        FamilyName = med.patient.familyName
                    },
                    Dose = med.dose,
                    TimeGiven = med.timeGiven
                });
            }

            return administrations;
        }

        // GET: api/Administrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdministrationDTO>> GetAdministrations(int id)
        {
            var administrations = await _context.Administrations
                .Include(a => a.patient)
                .Include(a => a.medication)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (administrations == null)
            {
                return NotFound();
            }

            var admin = new AdministrationDTO
            {
                Medication = new MedicationDTO
                {
                    Id = administrations.medicationId,
                    Name = administrations.medication.name
                },
                Patient = new PatientDTO
                {
                    Id = administrations.patientId,
                    GivenName = administrations.patient.firstName,
                    FamilyName = administrations.patient.familyName
                },
                Dose = administrations.dose,
                TimeGiven = administrations.timeGiven
            };

            return admin;
        }

        // PUT: api/Administrations/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministrations(int id, AdministrationDTO administration)
        {
            if (id != administration.Id)
            {
                return BadRequest();
            }

            var administrations = await _context.Administrations.FirstOrDefaultAsync(a => a.Id == id);

            administrations.medicationId = administration.Medication.Id;
            administrations.patientId = administration.Patient.Id;
            administrations.dose = administration.Dose;
            administrations.timeGiven = administration.TimeGiven;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministrationsExists(id))
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

        // POST: api/Administrations
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<AdministrationDTO>> PostAdministrations(AdministrationDTO administration)
        {
            var admin = new Administrations
            {
                dose = administration.Dose,
                medicationId = administration.Medication.Id,
                patientId = administration.Patient.Id,
                timeGiven = administration.TimeGiven
            };

            _context.Administrations.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdministrations", new { id = admin.Id }, administration);
        }

        // DELETE: api/Administrations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AdministrationDTO>> DeleteAdministrations(int id)
        {
            var administrations = await _context.Administrations.FindAsync(id);
            if (administrations == null)
            {
                return NotFound();
            }

            var admin = new AdministrationDTO
            {
                Id = id,
                Medication = new MedicationDTO
                {
                    Name = administrations.medication.name,
                    Id = administrations.medicationId
                },
                Dose = administrations.dose,
                Patient = new PatientDTO
                {
                    GivenName = administrations.patient.firstName,
                    FamilyName = administrations.patient.familyName,
                    Id = administrations.patientId
                },
                TimeGiven = administrations.timeGiven
            };

            _context.Administrations.Remove(administrations);
            await _context.SaveChangesAsync();

            return admin;
        }

        private bool AdministrationsExists(int id)
        {
            return _context.Administrations.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class MedicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Medications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicationDTO>>> GetMedications()
        {
            var meds =  await _context.Medications.ToListAsync();

            var medList = new List<MedicationDTO>();

            foreach(var med in meds)
            {
                medList.Add(new MedicationDTO
                {
                    Id = med.Id,
                    Name = med.name
                });
            }

            return medList;
        }

        // GET: api/Medications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicationDTO>> GetMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);

            var medDTO = new MedicationDTO
            {
                Id = medication.Id,
                Name = medication.name
            };

            if (medication == null)
            {
                return NotFound();
            }

            return medDTO;
        }

        // PUT: api/Medications/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedication(int id, MedicationDTO medication)
        {
            if (id != medication.Id)
            {
                return BadRequest();
            }

            var dbMed = await _context.Medications.FirstOrDefaultAsync(m => m.Id == id);

            dbMed.name = medication.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicationExists(id))
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

        // POST: api/Medications
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MedicationDTO>> PostMedication(MedicationDTO medication)
        {
            var med = new Medication
            {
                name = medication.Name
            };

            _context.Medications.Add(med);
            await _context.SaveChangesAsync();
            medication.Id = med.Id;

            return CreatedAtAction("GetMedication", new { id = med.Id }, medication);
        }

        // DELETE: api/Medications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MedicationDTO>> DeleteMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }
            var dto = new MedicationDTO
            {
                Id = id,
                Name = medication.name
            };

            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();

            return dto;
        }

        private bool MedicationExists(int id)
        {
            return _context.Medications.Any(e => e.Id == id);
        }
    }
}

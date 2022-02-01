using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetByID(int id)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Where(i => i.Id == id).FirstOrDefault();
            if (celestialObject == null)
            {
                return NotFound();
            }

            _context.CelestialObjects.Where(co => co.OrbitedObjectId == id).ToList().ForEach(co => co.Satellites.Add(co));
            return Ok(celestialObject);
        }

        [HttpGet("{name")]
        public IActionResult GetByName(string name)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Where(co => co.Name.Equals(name)).FirstOrDefault();
            if (celestialObject == null)
            {
                return NotFound();
            }

            _context.CelestialObjects.Where(co => co.OrbitedObjectId == celestialObject.Id).ToList().ForEach(co => co.Satellites.Add(co));
            return Ok(celestialObject);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.CelestialObjects);
        }
    }
}

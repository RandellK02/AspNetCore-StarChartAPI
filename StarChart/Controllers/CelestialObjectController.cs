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

        [HttpGet("{id:int}", Name = "GetByID")]
        public IActionResult GetByID(int id)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Where(co => co.Name.Equals(name)).FirstOrDefault();
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == celestialObject.Id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IList<CelestialObject> celestialObjects = _context.CelestialObjects.ToList();
            foreach (var co in celestialObjects)
            {
                co.Satellites = celestialObjects.Where(i => i.OrbitedObjectId == co.Id).ToList();
            }
            return Ok(_context.CelestialObjects);
        }
    }
}

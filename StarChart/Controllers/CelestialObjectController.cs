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

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
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
            IList<CelestialObject> celestialObjects = _context.CelestialObjects.Where(co => co.Name.Equals(name)).ToList();
            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }

            foreach(var co in celestialObjects)
            {
                co.Satellites = _context.CelestialObjects.Where(i => i.OrbitedObjectId == co.Id).ToList();
            }

            return Ok(celestialObjects);
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new object[] { celestialObject });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            CelestialObject tempCelestialObject = _context.CelestialObjects.Find(id);
            if (tempCelestialObject == null)
            {
                return NotFound();
            }

            tempCelestialObject.Name = celestialObject.Name;
            tempCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            tempCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            
            _context.CelestialObjects.Update(tempCelestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = name;
            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            IList<CelestialObject> celestialObjects = _context.CelestialObjects.Where(co => co.Id == id || co.OrbitedObjectId == id).ToList();
            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

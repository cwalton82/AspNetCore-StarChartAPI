using System;
using System.Linq;
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

        public CelestialObjectController(ApplicationDbContext Context)
        {
            _context = Context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int Id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(item => item.Id == Id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects.Where<CelestialObject>(item => item.OrbitedObjectId == celestialObject.Id).ToList();
            return Ok(celestialObject);

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where<CelestialObject>(item => item.Name == name).ToList();
            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }
            for (int i = 0; i < celestialObjects.Count; ++i)
            {
                celestialObjects[i].Satellites = _context.CelestialObjects.Where<CelestialObject>(item => item.OrbitedObjectId == celestialObjects[i].Id).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            for (int i = 0; i < celestialObjects.Count; ++i)
            {
                celestialObjects[i].Satellites = _context.CelestialObjects.Where<CelestialObject>(item => item.OrbitedObjectId == celestialObjects[i].Id).ToList();
            }
            return Ok(celestialObjects);
        }

    }
}

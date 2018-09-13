using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Models;
using CityInfo.API.Services;
using AutoMapper;
using CityInfo.API.Entities;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _repository;

        public CitiesController(ICityInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet(Name = "GetCities")]
        public IActionResult GetCities()
        {
            var cityEntities = _repository.GetCities();

            var results = Mapper.Map<IEnumerable<CityWithoutPointOfInterest>>(cityEntities);

            return Ok(results);
        }

        [HttpGet("{id}", Name = "GetCity")]
        public IActionResult GetCity(int id, bool includePointOfInterest = false)
        {
            var city = _repository.GetCity(id, includePointOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointOfInterest)
            {
                var resutl = Mapper.Map<CityDto>(city);
                return Ok(resutl);
            }

            var withoutPointOfInterest = Mapper.Map<CityWithoutPointOfInterest>(city);
            return Ok(withoutPointOfInterest);
        }
    }
}
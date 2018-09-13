using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();

        City GetCity(int id, bool includePointOfInterest);

        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        IEnumerable<PointOfInterest> GetPointOfInterestForCity(int cityId);

        void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest);

        bool Save();
    }
}

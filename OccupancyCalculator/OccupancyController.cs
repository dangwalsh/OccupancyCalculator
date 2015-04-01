using System.Collections.Generic;

namespace Gensler
{
    public class OccupancyController
    {
        private readonly OccupancyModel _occupancyModel;

        public OccupancyController(OccupancyModel dd)
        {
            _occupancyModel = dd;
        }

        public List<Occupancy> GetOccupancies()
        {
            return _occupancyModel.Occupancies;
        }
    }
}

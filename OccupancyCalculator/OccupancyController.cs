using System.Collections.Generic;

namespace Gensler
{
    public class OccupancyController
    {
        private OccupancyModel _documentData;

        public OccupancyController(OccupancyModel dd)
        {
            _documentData = dd;
        }

        public List<Occupancy> GetOccupancies()
        {
            return _documentData.Occupancies;
        }
    }
}

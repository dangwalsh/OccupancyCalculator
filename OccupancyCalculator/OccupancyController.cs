using System.Collections.Generic;

namespace Gensler
{
    public class OccupancyController
    {
        private readonly OccupancyModel _occupancyModel;

        public OccupancyController(OccupancyModel om)
        {
            _occupancyModel = om;
        }

        public List<Occupancy> GetOccupancies()
        {
            return _occupancyModel.Occupancies;
        }

        public void SetOccupantLoadParameters()
        {
            _occupancyModel.SetOccupantLoadParameter();
        }
    }
}

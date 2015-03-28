using System;

namespace Gensler
{
    public class Occupancy
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        private double _occupancySpaceArea;

        public double OccupancySpaceArea
        {
            get { return _occupancySpaceArea; }
            set
            {
                _occupancySpaceArea = value;
                _occupantLoad = _occupancySpaceArea/AreaPerOccupant;
            }
        }

        private int _areaPerOccupant;

        public int AreaPerOccupant
        {
            get { return _areaPerOccupant; }
            set { _areaPerOccupant = value; }
        }

        private double _occupantLoad;

        public double OccupantLoad
        {
            get { return _occupantLoad; }
        }
        
        public Occupancy(string n, int apo)
        {
            _name = n;
            _occupancySpaceArea = 0.0;
            _areaPerOccupant = apo;
            _occupantLoad = 0.0;
        }

        public Occupancy(string n, double osa, int apo, double ol)
        {
            _name = n;
            _occupancySpaceArea = osa;
            _areaPerOccupant = apo;
            _occupantLoad = ol;
        }
    }
}

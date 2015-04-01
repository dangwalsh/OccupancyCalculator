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

        private String _levelName;

        public String LevelName
        {
            get { return _levelName; }
            set { _levelName = value; }
        }
        
        
        public Occupancy(String n, Int32 apo)
        {
            _name = n;
            _occupancySpaceArea = 0.0;
            _areaPerOccupant = apo;
            _occupantLoad = 0.0;
            _levelName = "";
        }

        public Occupancy(String n, Double osa, Int32 apo, Double ol, String l)
        {
            _name = n;
            _occupancySpaceArea = osa;
            _areaPerOccupant = apo;
            _occupantLoad = ol;
            _levelName = l;
        }
    }
}

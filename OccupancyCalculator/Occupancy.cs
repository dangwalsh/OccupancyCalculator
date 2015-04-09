using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Gensler
{
    /// <summary>
    /// Occupancy data object
    /// </summary>
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

        private List<Parameter> _loadParameters;

        public List<Parameter> LoadParameters
        {
            get { return _loadParameters; }
            set { _loadParameters = value; }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n"></param>
        /// <param name="apo"></param>
        public Occupancy(String n, Int32 apo)
        {
            Name = n;
            OccupancySpaceArea = 0.0;
            AreaPerOccupant = apo;
            LevelName = "";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n"></param>
        /// <param name="osa"></param>
        /// <param name="apo"></param>
        /// <param name="ln"></param>
        /// <param name="p"></param>
        public Occupancy(String n, Double osa, Int32 apo, String ln, Parameter p)
        {
            Name = n;            
            AreaPerOccupant = apo;
            LevelName = ln;
            OccupancySpaceArea = osa;
            LoadParameters = new List<Parameter> {p};
        }
    }
}

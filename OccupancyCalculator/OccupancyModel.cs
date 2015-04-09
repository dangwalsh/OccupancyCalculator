using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;

namespace Gensler
{
    /// <summary>
    /// Occupancy data model
    /// </summary>
    public class OccupancyModel
    {
        private readonly Document _document;

        private Document Document
        {
            get { return _document; }
        }

        private readonly String _scheduleName;

        private String ScheduleName
        {
            get { return _scheduleName; }
        }

        private readonly String _loadName;

        private String LoadName
        {
            get { return _loadName; }
        }

        private readonly IEnumerable<Occupancy> _occupancyKeys;

        private IEnumerable<Occupancy> OccupancyKeys
        {
            get {  return _occupancyKeys; }
        }

        private readonly IEnumerable<Element> _levels;

        private IEnumerable<Element> Levels
        {
            get { return _levels; }
        }

        private readonly IEnumerable<Element> _rooms;

        private IEnumerable<Element> Rooms
        {
            get { return _rooms; }
        }

        private readonly List<Occupancy> _occupancies;

        public List<Occupancy> Occupancies
        {
            get { return _occupancies; }
        }

        private String _keyName;

        private String KeyName
        {
            get { return _keyName; }
            set { _keyName = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData"></param>
        public OccupancyModel(ExternalCommandData commandData)
        {
            _document = commandData.Application.ActiveUIDocument.Document;
            _scheduleName = @"* Manage Occupancy Table Per 2006 & 2009 Ibc 1004.1.2";
            _loadName = @"Occupant Load";
            _occupancyKeys = GetKeys();
            _levels = GetLevels();
            _rooms = GetRooms();
            _occupancies = GetOccupancies();
        }

        /// <summary>
        /// Iterates ViewSchedules and returns a collection of Occupancy Keys
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Occupancy> GetKeys()
        {
            var occupancyKeys = new List<Occupancy>();
            var collector = new FilteredElementCollector(Document);
            var viewSchedules = 
                collector.OfClass(typeof(TableView)).ToElements().ToList();
            var element =
                viewSchedules.FirstOrDefault(v => v.Name == ScheduleName);
            var viewSchedule = element as ViewSchedule;
            if (null == viewSchedule) return occupancyKeys;
            KeyName = viewSchedule.KeyScheduleParameterName;
            var sd = viewSchedule.GetTableData().GetSectionData(SectionType.Body);
            for (var iRow = 2; iRow < sd.NumberOfRows; iRow++)
            {
                var name = viewSchedule.GetCellText(SectionType.Body, iRow, 0);
                var sqft = viewSchedule.GetCellText(SectionType.Body, iRow, 1);
                int sqftNum;
                if (Int32.TryParse(sqft, out sqftNum))
                {
                    occupancyKeys.Add(new Occupancy(name, sqftNum));
                }
                else
                {
                    throw new Exception(@"Failed to convert to integer value.");
                }
            }
            return occupancyKeys;
        }

        /// <summary>
        /// Returns a collection of Levels as Elements
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Element> GetLevels()
        {
            var collector = new FilteredElementCollector(Document);
            return collector.OfClass(typeof(Level)).ToElements().ToList();
        }

        /// <summary>
        /// Returns a collection of SpatialElements as Elements
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Element> GetRooms()
        {
            var collector = new FilteredElementCollector(Document);
            return collector.OfClass(typeof(SpatialElement)).ToElements().ToList();
        }

        /// <summary>
        /// Iterates through Levels and Rooms and returns a List of Occupancies
        /// </summary>
        /// <returns></returns>
        private List<Occupancy> GetOccupancies()
        {
            var occupancies = new List<Occupancy>();
            foreach (var levelElement in Levels)
            {
                var level = levelElement as Level;
                if (null == level) continue;
                var roomsOnLevel = Rooms.OfType<Room>().Where(r => r.Level.Name == level.Name);
                foreach (var room in roomsOnLevel)
                {
                    var occupancyParameter =
                        room.Parameters.OfType<Parameter>()
                        .FirstOrDefault(p => p.Definition.Name == KeyName);
                    var loadParameter =
                        room.Parameters.OfType<Parameter>()
                        .FirstOrDefault(l => l.Definition.Name == LoadName);
                    if (null == occupancyParameter) continue;
                    var keyName =
                        Document.GetElement(occupancyParameter.AsElementId()).Name;
                    var existing = 
                        occupancies.FirstOrDefault(o => o.Name == keyName && o.LevelName == level.Name);
                    if (null != existing)
                    {
                        existing.OccupancySpaceArea += room.Area;
                        existing.LoadParameters.Add(loadParameter);
                    }
                    else
                    {
                        var unused =
                            OccupancyKeys.FirstOrDefault(o => o.Name == keyName);
                        if (null == unused) continue;
                        occupancies.Add(new Occupancy(
                            unused.Name, 
                            room.Area, 
                            unused.AreaPerOccupant, 
                            level.Name, 
                            loadParameter));
                    }
                }
            }
            return occupancies;
        }

        /// <summary>
        /// Sets the value of the "Occupant Load" Parameter
        /// </summary>
        public void SetOccupantLoadParameter()
        {
            var t = new Transaction(Document, @"Fill Occupant Params");
            if (t.Start() != TransactionStatus.Started) return;
            foreach (var occupancy in Occupancies)
            {
                foreach (var loadParameter in occupancy.LoadParameters)
                {
                    try
                    {
                        if (loadParameter.Set(Math.Round(occupancy.OccupantLoad))) continue;
                        throw new Exception(@"Failed to set Occupant Load value");
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show(@"Error", ex.Message);
                    }
                }
            }
            t.Commit();
        }
    }
}

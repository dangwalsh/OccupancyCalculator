using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;

namespace Gensler
{
    public class OccupancyModel
    {
        private readonly Document _document;

        private readonly String _scheduleName; 

        private String _keyName;

        private readonly List<Occupancy> _occupancyTable;

        private readonly List<Element> _levels;

        private List<Element> Levels
        {
            get { return _levels; }
        }

        private readonly List<Element> _rooms;

        private List<Element> Rooms
        {
            get { return _rooms; }
        }

        private readonly List<Occupancy> _occupancies;

        public List<Occupancy> Occupancies
        {
            get
            {
                return _occupancies;
            }
        }

        public OccupancyModel(ExternalCommandData commandData, 
            String schedName = @"* Manage Occupancy Table Per 2006 & 2009 Ibc 1004.1.2")
        {
            _document = commandData.Application.ActiveUIDocument.Document;
            _scheduleName = @"* Manage Occupancy Table Per 2006 & 2009 Ibc 1004.1.2";//schedName;
            _occupancyTable = GetKeys();
            _levels = GetLevels();
            _rooms = GetRooms();
            _occupancies = GetOccupancies();
        }

        private List<Element> GetLevels()
        {
            var collector = new FilteredElementCollector(_document);
            return collector.OfClass(typeof(Level)).ToElements().ToList();
        }

        private List<Element> GetRooms()
        {
            var collector = new FilteredElementCollector(_document);
            return collector.OfClass(typeof(SpatialElement)).ToElements().ToList();
        }

        private List<Occupancy> GetOccupancies()
        {
            var occupancies = new List<Occupancy>();
            foreach (var levelElement in Levels)
            {
                var level = levelElement as Level;
                if (null == level) continue;
                var roomsOnLevel = Rooms.OfType<Room>().Where(r => r.Level == level);
                foreach (var room in roomsOnLevel)
                {
                    var parameter =
                        room.Parameters.OfType<Parameter>()
                            .FirstOrDefault(p => p.Definition.Name == _keyName);
                    if (null == parameter) continue;
                    var keyName =
                        _document.GetElement(parameter.AsElementId()).Name;
                    //var existing =
                    //    occupancies.FirstOrDefault(o => o.Name == keyName);
                    var existing = (from o in occupancies
                        where (o.Name == keyName)
                        where (o.LevelName == level.Name)
                        select o).First();
                    if (null != existing)
                    {
                        existing.OccupancySpaceArea += room.Area;
                    }
                    else
                    {
                        var unused =
                            _occupancyTable.FirstOrDefault(o => o.Name == keyName);
                        if (null == unused) continue;
                        unused.OccupancySpaceArea += room.Area;
                        unused.LevelName = level.Name;
                        occupancies.Add(unused);
                    }
                }
            }
            return occupancies;
        }

        private List<Occupancy> GetKeys()
        {
            var occupancyTable = new List<Occupancy>();
            var collector = new FilteredElementCollector(_document);
            var viewSchedules = collector.OfClass(typeof (TableView)).ToElements().ToList();
            var element =
                viewSchedules.FirstOrDefault(v => v.Name == _scheduleName);
            var viewSchedule = element as ViewSchedule;
            if (null == viewSchedule) return occupancyTable;
            _keyName = viewSchedule.KeyScheduleParameterName;
            var sd = viewSchedule.GetTableData().GetSectionData(SectionType.Body);
            for (var iRow = 2; iRow < sd.NumberOfRows; iRow++)
            {
                var name = viewSchedule.GetCellText(SectionType.Body, iRow, 0);
                var sqft = viewSchedule.GetCellText(SectionType.Body, iRow, 1);
                int sqftNum;
                if (Int32.TryParse(sqft, out sqftNum))
                {
                    occupancyTable.Add(new Occupancy(name, sqftNum));
                }
                else
                {
                    throw new Exception(@"Failed to convert to integer value.");
                }
            }
            return occupancyTable;
        }
    }
}

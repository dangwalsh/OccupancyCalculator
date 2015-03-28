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

        private readonly List<Occupancy> _occupancyTable;
        //private readonly List<Occupancy> _occupancyTable = new List<Occupancy>()
        //{
        //    new Occupancy(@"Accessory storage areas, mechanical equipment room", 100),
        //    new Occupancy(@"Agricultural building", 15),
        //    new Occupancy(@"Aircraft hangers", 7),
        //    new Occupancy(@"Airport terminal - Bagage claim", 5)
        //};

        private readonly List<Element> _rooms;

        public List<Element> Rooms
        {
            get
            {
                return _rooms;
            }
        }

        private readonly List<Occupancy> _occupancies;

        public List<Occupancy> Occupancies
        {
            get
            {
                return _occupancies;
            }
        }
        
        public OccupancyModel(ExternalCommandData commandData)
        {
            _document = commandData.Application.ActiveUIDocument.Document;
            _occupancyTable = GetKeys();
            _rooms = GetRooms();
            _occupancies = GetOccupancies();
        }

        private List<Element> GetRooms()
        {
            FilteredElementCollector collector = new FilteredElementCollector(_document);
            return collector.OfClass(typeof(SpatialElement)).ToElements().ToList();
        }

        private List<Occupancy> GetOccupancies()
        {
            List<Occupancy> occupancies = new List<Occupancy>();
            foreach (var element in Rooms)
            {
                Room room = element as Room;
                if (null != room)
                {
                    var parameter =
                        room.Parameters.OfType<Parameter>()
                            .FirstOrDefault(p => p.Definition.Name == @"Room Occupancy Designation");
                    if (null != parameter)
                    {
                        var keyName = 
                            _document.GetElement(parameter.AsElementId()).Name;
                        var existing = 
                            occupancies.FirstOrDefault(o => o.Name == keyName);
                        if (null != existing)
                        {
                            existing.OccupancySpaceArea += room.Area;
                        }
                        else
                        {
                            var unused = 
                                _occupancyTable.FirstOrDefault(o => o.Name == keyName);
                            if (null != unused)
                            {
                                unused.OccupancySpaceArea += room.Area;
                                occupancies.Add(unused);
                            }
                        }
                    }
                }
            }
            return occupancies;
        }

        private List<Occupancy> GetKeys()
        {
            List<Occupancy> occupancyTable = new List<Occupancy>();
            FilteredElementCollector collector = new FilteredElementCollector(_document);
            List<Element> viewSchedules = collector.OfClass(typeof (ViewSchedule)).ToElements().ToList();
            var element =
                viewSchedules.FirstOrDefault(v => v.Name == @"* Manage Occupancy Table Per 2006 & 2009 Ibc 1004.1.2");
            ViewSchedule viewSchedule = element as ViewSchedule;
            if (null != viewSchedule)
            {
                var n = viewSchedule.KeyScheduleParameterName;
                var d = viewSchedule.GetTableData().GetSectionData(0);
                int last = d.LastRowNumber;
                for (int i = 0; i < last; i++)
                {
                    d.GetCellText(i, 0);
                }
            }
            return null;
        }
    }
}

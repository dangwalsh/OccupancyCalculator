using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            List<Element> viewSchedules = collector.OfClass(typeof (TableView)).ToElements().ToList();
            var element =
                viewSchedules.FirstOrDefault(v => v.Name == @"* Manage Occupancy Table Per 2006 & 2009 Ibc 1004.1.2");
            ViewSchedule viewSchedule = element as ViewSchedule;
            if (null != viewSchedule)
            {
                if (@"Room Occupancy Designation" != viewSchedule.KeyScheduleParameterName)
                {
                    throw new Exception(@"Addin is accessing the incorrect schedule key");
                }

                var sd = viewSchedule.GetTableData().GetSectionData(SectionType.Body);

                for (int iRow = 2; iRow < sd.NumberOfRows; iRow++)
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
                        throw new Exception(@"Failed to convert value to integer.");
                    }
                }
            }
            return occupancyTable;
        }
    }
}

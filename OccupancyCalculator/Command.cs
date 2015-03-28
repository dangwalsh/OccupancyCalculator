using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Gensler
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Command : IExternalCommand
    {
        /// <summary>
        /// Command entry point
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var app = commandData.Application;
                var uidoc = app.ActiveUIDocument;
                var doc = uidoc.Document;
                var mainWindow = new MainWindow(new OccupancyController(new OccupancyModel(commandData)));

                mainWindow.Show();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
        }
    }
}



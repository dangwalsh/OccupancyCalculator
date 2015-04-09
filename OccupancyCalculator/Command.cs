using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Gensler
{
    /// <summary>
    /// Revit command entry points
    /// </summary>
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
                var occupancyView = new OccupancyView(new OccupancyModel(commandData));

                occupancyView.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show(@"Error", ex.Message);
                return Result.Failed;
            }
        }
    }
}



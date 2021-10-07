using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Attributes;

namespace DATools.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class DuplicateSelectedViews : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;

            ICollection<ElementId> ids = uIDocument.Selection.GetElementIds();
            string data = "";


            TaskDialog maindialog = new TaskDialog("DATarchitets.NET - DUPLICATE SELECTED VIEWS");
            maindialog.MainInstruction = "DATarchitets.NET - DUPLICATE SELECTED VIEWS";
            maindialog.MainContent = "ESTA SEGURO QUE DESA CONTINUAR? PARA DUPLICAR TODAS LAS VISTAS , PRESIONE OK";
            maindialog.AllowCancellation = true;
            maindialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
            TaskDialogResult RESULT = maindialog.Show();

            if(RESULT == TaskDialogResult.Ok)
            {
                foreach(ElementId viewId in ids)
                {
                    View vista = doc.GetElement(viewId) as View;

                    using(Transaction trans = new Transaction(doc,"DUPLICANDO VISTAS"))
                    {
                        trans.Start();

                        ElementId newView = vista.Duplicate(ViewDuplicateOption.WithDetailing);
                        View vissta = doc.GetElement(newView) as View;
                        vissta.Name = vissta.Name + " - " + "PROTO_WORK";
                        data += vissta.Name + Environment.NewLine + vissta.Id;

                        trans.Commit();

                    }
                }
                TaskDialog.Show("INFO", data);


            }
            else
            {
                TaskDialog.Show("INFO", "OPERACIÓN CANCELADA!");
            }



          
			return Result.Succeeded;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using DATAxmTools.RebarSolidApp.ViewModel;

namespace DATAxmTools.RebarSolidApp
{
   [Transaction(TransactionMode.Manual)]
    public class SolidCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            //OBTENEMOS LA VISTA ACTUAL O ACTIVA

            var vm = new SolidViewModel(uidoc);
            vm.Solidview.ShowDialog();

            //como la Aplicaciòn funciona solo para Vistas 3D, FILTRAMOS LA VISTAS PARA QUE SEA 3D(ERROR HANDLING FILTROS,ETC..)

            

            return Result.Succeeded;
        }
    }
}

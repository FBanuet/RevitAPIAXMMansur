using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Attributes;
using DATools.UIForm;

namespace DATools.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RenamePDFCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //GLOBAL VARIABLES
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                string info = "";

                FilePathForm fpform = new FilePathForm();
                fpform.ShowDialog();

                if (fpform.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    string ruta = fpform.FPFGetfilepath();

                    string[] files = Directory.GetFiles(ruta);

                    foreach(string file in files)
                    {
                        string filename = Path.GetFileName(file);

                        string newname = filename.Remove(13, 13);

                        RenameFile(file, newname);

                        info += newname + Environment.NewLine;
                    }

                    TaskDialog.Show("AXM Info", info);
                }
                else
                {
                    TaskDialog.Show("AXM Info", "OPERACIÓN CANCELADA!");
                }

                    



                return Result.Succeeded;

            }
            catch(Exception e)
            {
                TaskDialog.Show("WARNING! ALGO SUCEDIO!", e.Message);
                return Result.Failed;
            }
        }
        
        public static void RenameFile(string bigname, string shortname)
        {
            try
            {
                string directorypath = Path.GetDirectoryName(bigname);
                if (directorypath == null)
                {
                    TaskDialog.Show("ERROR!", "DIRECTORIO NO FUE ENCONTRADO, INTENTE DE NUEVO");
                }

                var newfilewithpath = Path.Combine(directorypath, shortname);
                FileInfo finfo = new FileInfo(bigname);
                finfo.MoveTo(newfilewithpath);

            }

            catch (Exception e)
            {
                TaskDialog.Show("WARNING!", e.Message);
            }

        }
    }
}

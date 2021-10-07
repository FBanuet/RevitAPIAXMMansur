/*
 * Created by SharpDevelop.
 * User: username
 * Date: 14/07/2021
 * Time: 11:56 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.      -----------------------//-/--*/-/-/-/F12
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace CADUtils
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("1A86F7BF-B207-4E06-B8CD-216AF6FA4F53")]
	public partial class ThisApplication
	{
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
		public void CADinfo()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			Reference ele = uidoc.Selection.PickObject(ObjectType.Element);
			Element element = doc.GetElement(ele);
		
			
			Parameter par = element.get_Parameter(BuiltInParameter.IMPORT_SYMBOL_NAME);
			
			string data = par.AsString();
			TaskDialog.Show("INFO",data);
			
		}
		
		public void GetAllDocumentCadNames()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector colector = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance));
			
			string infos = "";
			
			foreach(ImportInstance cad in colector)
			{
				Parameter par = cad.get_Parameter(BuiltInParameter.IMPORT_SYMBOL_NAME);
				infos += par.AsString() + Environment.NewLine;
			}
			
			TaskDialog.Show("INFO",infos);
		}
		
		public void DeleteAllCads()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector colector = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance));
			
			string infos = "";
			IList<ElementId> elist = new List<ElementId>();
			
			foreach(ImportInstance cad in colector)
			{
				ElementId cadids = cad.Id;
				elist.Add(cadids);
				
				Parameter par = cad.get_Parameter(BuiltInParameter.IMPORT_SYMBOL_NAME);
				infos += par.AsString() + Environment.NewLine;
			}
			
			using(Transaction tr = new Transaction(doc,"BorrarCADS"))
			{
				tr.Start();
				
				doc.Delete(elist);
				
				
				tr.Commit();
			}
			
			TaskDialog.Show("INFO"," SE BORRARON LOS SIGUIENTES ELEMENTOS" + Environment.NewLine + infos);
			
		}
			
	}
}
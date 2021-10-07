/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 19/11/2020
 * Time: 12:32 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Architecture;

namespace dahyna
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("91438E58-F8E1-4FE4-9B26-D40FA1EC1F42")]
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
		public void borrarcads()
		{
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance));
			
			string info = "";
			
			IList<ElementId> elists = new List<ElementId>();
			
			foreach(ImportInstance cad in collector)
			{
				ElementId cadids = cad.Id;
				elists.Add(cadids);
				

				info += cad.Id + Environment.NewLine;
				
			}
			
			using(Transaction trans = new Transaction(doc,"DELETING"))
			{
				trans.Start();
					
				
				doc.Delete(elists);
				
					
				
					
				trans.Commit();
			}
			
			TaskDialog.Show("indo","LA INFO PAPS :" + Environment.NewLine + info);

			
			
			
			
			
		}
		public void TopoFloor()
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = this.ActiveUIDocument;
			
			TopographySurface ts = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,"seleccionar subregion")) as TopographySurface;
			Floor piso = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,"seleccione piso o losa")) as Floor;
			
			
			using (Transaction trans = new Transaction(doc,"TOPO TO FLOOR"))
			{
				trans.Start();
				SlabShapeEditor ed = piso.SlabShapeEditor;
				
				foreach(XYZ pt in ts.GetPoints())
				{
					ed.DrawPoint(pt);
				}
				
				
				
				trans.Commit();
			}
			
			
			
		}
		public void CADInfo()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			Reference ele = uidoc.Selection.PickObject(ObjectType.Element) ;
			Element el = doc.GetElement(ele);
			Options op = new Options();
			string data = "";
			
			foreach(GeometryElement gi in el.get_Geometry(op))
			{
				var gg = gi.GraphicsStyleId ;
				data += gg.ToString();
				

				        
			}
			
			TaskDialog.Show("info",data);
			
		}
		



	}
}
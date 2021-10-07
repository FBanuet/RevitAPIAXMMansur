/*
 * Created by SharpDevelop.
 * User: SMK
 * Date: 01/06/2020
 * Time: 07:55 p. m.
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

namespace GeoAutoDim
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("38113AA1-C819-45D5-9FD7-2DBEFE62715F")]
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
		public void LocationDim()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element ele = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			Location loc = ele.Location;
			
			if(loc is LocationCurve)
			{
				LocationCurve lc = loc as LocationCurve;
				Curve c = lc.Curve;
				double length = UnitUtils.ConvertFromInternalUnits(c.ApproximateLength,DisplayUnitType.DUT_METERS);
				TaskDialog.Show("Springall + mk","longitud de elemento en (m)" + " : " + length.ToString() + " m");
				if(c is Line)
				{
					Line line = c as Line;
					TaskDialog.Show("Springall + mk","LA LONGITUD RECTA EN (m) : " + UnitUtils.ConvertFromInternalUnits(line.ApproximateLength,DisplayUnitType.DUT_METERS).ToString());
				}
			}
			
		}
		
		public void EdgeDim()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			Reference myref = uidoc.Selection.PickObject(ObjectType.Edge);
			Element el = doc.GetElement(myref);
			
			GeometryObject geomObj = el.GetGeometryObjectFromReference(myref);
			
			Edge edge = geomObj as Edge;
			
			double length = UnitUtils.ConvertFromInternalUnits(edge.ApproximateLength,DisplayUnitType.DUT_METERS);
			
			
			
			TaskDialog.Show("Springall + mk","longitud de elemento en (m)" + " : " + length.ToString() + " m");
			
		}
		
		
		public void SetParameter()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			string nombres = "";
			string valores= "";
			
			foreach(Element el in new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Furniture).WhereElementIsElementType())
			{
				string tnames = el.Name;
				Parameter par = el.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_MARK);
				valores += par.AsString() + Environment.NewLine;
				nombres += tnames + Environment.NewLine;
				
			}
			
		
			TaskDialog.Show("INFO","TIPOS DE FAMILIA SON : " + nombres + Environment.NewLine + valores);
		}
		
		public void TopoTOExistingFloor()
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
		
		
		
		public void GetKeynoteTable()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			collector.OfClass(typeof(KeynoteTable));
			
			string test = "";
			
			
			foreach(KeynoteTable kt in collector)
			{
				KeyBasedTreeEntries kbe = kt.GetKeyBasedTreeEntries();
				
				string ho = "";
				foreach(KeyBasedTreeEntry k in kbe)
				{
					KeynoteEntry KE = k as KeynoteEntry;

					string key = k.Key.ToString();
					string keyvalue = k.ParentKey.ToString();
					string keytext = KE.KeynoteText.ToString();
				
					ho += key + " : " + keyvalue + " : " + keytext + Environment.NewLine;
				}
				
				test = ho;
			}
			TaskDialog.Show("INFO","CAMARA REY : " + test);
		}
		
		
		public void DeleteCADS()
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
		
		
		
		
		public void DeleteElement()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Reference refe = uidoc.Selection.PickObject(ObjectType.Element,"SELECCIONE UN ELEMENTO A BORRAR");
			
			using (Transaction trans = new Transaction(doc,"borrando"))
			{
				trans.Start();
				
				doc.Delete(refe.ElementId);
				
				
				trans.Commit();
			}
			
		}
		
		
		public void SeleccionporCategoria()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			ICollection<ElementId> elements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).ToElementIds();
			uidoc.Selection.SetElementIds(elements);
		}
		
		public void InformacionSeleccionada()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			ICollection<ElementId> selectedIDS = uidoc.Selection.GetElementIds();
			
			string s = "";
			
			foreach(ElementId eleid in selectedIDS)
			{
				Element e = doc.GetElement(eleid);
				s += e.Name + Environment.NewLine;
			}
			
			TaskDialog.Show("ELEMENTOS" , selectedIDS.Count + Environment.NewLine + s);
		}
		
		
		public void SeleccionFiltrada()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Reference refWall = uidoc.Selection.PickObject(ObjectType.Element , new GenericSelectionFilter("Floors"),"SELECCIONE ALGO");
		}
		
		
		public class GenericSelectionFilter : ISelectionFilter
		{
			static string categoryName = "";
			
			public GenericSelectionFilter(string name)
			{
				categoryName = name;
			}
			
			public bool AllowElement(Element el)
			{
				if(el.Category.Name == categoryName)
				{
					return true;
				}
				return false;
			}
			
			public bool AllowReference(Reference r , XYZ point)
			{
				return true;
			}
		}
		
		
		public class WallSelectionFilter : ISelectionFilter
		{
			public bool AllowElement(Element e)
			{
				if(e.Category.Name == "Walls")
					return true;
				
				return false;
			}
			
			public bool AllowReference(Reference r, XYZ point)
			{
				return true;
			}
		}
		
		public void GetRebarTypeName()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsElementType();
			
			string info = "";
			
			foreach(var bar in collector)
			{
				info += bar.Name + Environment.NewLine;
			}
			
			TaskDialog.Show("INFO","REBAR BAR TYPE : " + info);
		}
			
		
		
		
		
		public void GetSetParameters()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			IList<BuiltInCategory> catList = new List<BuiltInCategory>();
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			collector.OfCategory(BuiltInCategory.OST_Floors).WhereElementIsElementType();
			
			
			 string typeNames = "";
			 
			 
			 foreach(FloorType muro  in collector)
			 {
			 	string keyvalue = "";
			 	
			 	
			 	Parameter keynote = muro.get_Parameter(BuiltInParameter.KEYNOTE_PARAM);

			 	if(keynote.HasValue)
			 	{
			 		keyvalue += keynote.AsString() + Environment.NewLine;
			 		
			 		
			 	}
			 	
			 	else
			 	{
			 		keyvalue += "NO KEYNOTE ASSIGNED" + Environment.NewLine;
			 	}
			 	
			 	
			 	
			 	typeNames += muro.Name.ToString() + Environment.NewLine + "KEYNOTE VALUE : " + keyvalue +  Environment.NewLine;
			 }
			
			 
			 TaskDialog.Show("bnt","LA INFO ES : " + typeNames);
			 	
		}
		
	}
}
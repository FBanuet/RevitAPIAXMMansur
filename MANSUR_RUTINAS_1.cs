/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 24/02/2021
 * Time: 09:22 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Architecture;

namespace SELECTIONS
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("632A9C80-82F7-4F94-84AF-8F025E2BB59F")]
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
		public void IROOMS()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Selection sel = uidoc.Selection;
			ISelectionFilter isl = new RoomSelectionFilter();
			
			IList<Reference> roomrefs = sel.PickObjects(ObjectType.Element,isl,"SELECCIONE ROOMS");
			
			ICollection<ElementId> eleids = new List<ElementId>();
			
			foreach(Reference rf in roomrefs)
			{
				ElementId rid = doc.GetElement(rf).Id;
				
				eleids.Add(rid);
			}
			
			sel.SetElementIds(eleids);
		}
		
		public void IWALLS()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Selection sel = uidoc.Selection;
			ISelectionFilter isl = new WalSelectionFilter();
			
			IList<Reference> roomrefs = sel.PickObjects(ObjectType.Element,isl,"SELECCIONE WALLS");
			
			ICollection<ElementId> eleids = new List<ElementId>();
			
			foreach(Reference rf in roomrefs)
			{
				ElementId rid = doc.GetElement(rf).Id;
				
				eleids.Add(rid);
			}
			
			sel.SetElementIds(eleids);
			
		}
		
		
		
		
		public class RoomSelectionFilter : ISelectionFilter
		{
			public bool AllowElement(Element element)
			{
				if(element.Category.Name == "Rooms"){
					return true;
				}
				else{
					return false;
				}
			}
			
			public bool AllowReference(Reference rfs,XYZ point)
			{
				return false;
			}
		}
		
		public class WalSelectionFilter : ISelectionFilter
		{
			public bool AllowElement(Element element)
			{
				if(element.Category.Name == "Walls"){
					return true;
				}
				else{
					return false;
				}
			}
			
			public bool AllowReference(Reference rfs,XYZ point)
			{
				return false;
			}
		}
		public void DeleteByCategory()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			List<Room> rooms = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Rooms).Cast<Room>().ToList();
			
			using(Transaction trans = new Transaction(doc,"ROOMS DELETED"))
			{
				trans.Start();
				
				foreach(Room ro in rooms)
				{
					doc.Delete(ro.Id);
				}
				
				trans.Commit();
				
				
			}
			
			TaskDialog.Show("INFO","SE ELIMINARON LOS ELEMENTOS CON EXITO");
		}
		public void ISeleccionMuros()
		{
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			Selection sel = uidoc.Selection;
			//ReferenceArray refa = new ReferenceArray();
			
			ISelectionFilter esl = new WallSelectionFilter();
			IList<Element> eles = sel.PickElementsByRectangle(esl,"seleccione varios muros");
			
			
			ICollection<ElementId> eleid = new List<ElementId>();
			
			StringBuilder stb = new StringBuilder();
			
			foreach(Element re in eles)
			{
				eleid.Add(re.Id);
				stb.AppendLine(re.Name + " : " + re.Name);
				
			}
			
			TaskDialog.Show("INFO",stb.ToString());
			
			sel.SetElementIds(eleid);
			
		}
		
		public class WallSelectionFilter : ISelectionFilter
		{
			public bool AllowElement(Element element)
			{
				if(element.Category.Name == "Floors"){
					return true;
				}
				else{
					return false;
				}
			}
			
			public bool AllowReference(Reference rfs , XYZ point)
			{
				return false;
			}
			
		}
		public void ROOMNAMECHANGED()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector col = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Rooms);
			
			//string info = "";
			
			
			using(Transaction trans = new Transaction(doc,"ROOMS"))
			{
				trans.Start();
					
				foreach(Element el in col)
				{
					Room rom = el as Room;
						
					Parameter par = rom.get_Parameter(BuiltInParameter.ROOM_NAME);
					string read = par.AsString();
					if( read == "2ND BDRM")
					{
						par.Set("2NDBDRM-Segunda Habitación");
							
					}
					
						
				}
					
				trans.Commit();
						
						
					
				}
		
		
		}
		public void WALLOPENINGQUERY()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			Selection sel = uidoc.Selection;
			
			using(Transaction trans = new Transaction(doc,"INFOOPENINGS"))
			{
				trans.Start();
				
				Opening ope = doc.GetElement(sel.PickObject(ObjectType.Element,"SELECCIONE UN OPENING")) as Opening;
				
				GetInfoOpening(ope);
				
				
				trans.Commit();
			}
		}
		
		private void GetInfoOpening(Opening opening)
		{
			string message = "Opening";
			
			message += "\nEl id del Host del Opening es: " + opening.Host.Id.IntegerValue;
			
			if(opening.IsRectBoundary)
			{
				message += "\nEl Opening tiene un Perimetro Rectangular";
				
				IList<XYZ> boundaryRect = opening.BoundaryRect;
				
				XYZ point = opening.BoundaryRect[0];
				message += "\nMin la Coordenada del Vértice es: (" + point.X + ", " + point.Y + ", " + point.Z + ")";
				point = opening.BoundaryRect[1];
				message += "\nMin la Coordenada del Vértice es: (" + point.X + ", " + point.Y + ", " + point.Z + ")";
			}
			else
			{
				message += "\nEl Opening No tiene un Perimetro Rectangular";
				
				int curves = opening.BoundaryCurves.Size;
				message += "\nEl Numero de curvas es: " + curves;
				
				for(int i =0; i<curves; i++)
				{
					Curve curva = opening.BoundaryCurves.get_Item(i);
					message += "\nEl punto Inicial de la Curva es: " + XYZToString(curva.GetEndPoint(0));
					message += "\nEl punto Final de la Curva es: " + XYZToString(curva.GetEndPoint(1));
				}
				
			}
			
			TaskDialog.Show("INFO",message);
			
		}
		
		string XYZToString(XYZ point)
		{
			return "(" + point.X + ", " + point.Y + ", " + point.Z + ")";
		}
		
	}
}
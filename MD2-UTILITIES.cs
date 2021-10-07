/*
 * Created by SharpDevelop.
 * User: S-MK
 * Date: 27/10/2020
 * Time: 09:45 a. m.
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

namespace data
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("2876FBA3-E6C6-4F0B-A273-CE12375A66EC")]
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
		public void deleteCADS()
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
		public void floor2topo()
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
		public void FllorSP()
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = this.ActiveUIDocument;
			Floor piso = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,"SELECCIONAR LOSA")) as Floor;
			IList<Element> soportes = uidoc.Selection.PickElementsByRectangle("SELECCIONAR SOPORTES VIGAS");
			
			using (Transaction trans = new Transaction(doc,"DATARCHITECTS"))
			{
				trans.Start();
				
				piso.SlabShapeEditor.Enable();
				foreach(Element el in soportes)
				{
					LocationCurve lc = el.Location as LocationCurve;
					 
					Line linea = lc.Curve as Line;
					
					
					piso.SlabShapeEditor.PickSupport(linea);
					
				}
				
				
				
				
				trans.Commit();
			}
		}
		public void RENAMEFAMILYSYMBOL()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ICollection<Element> elementos = collector.OfClass(typeof(Family)).ToElements();
			string famnames = "";
			
			
			
			Category cat = Category.GetCategory(doc,BuiltInCategory.OST_StructuralFoundation);
			IList<Family> fams = new List<Family>();
			foreach(Element e in elementos)
			{
				Family fs = e as Family;
				
				if(fs.FamilyCategoryId == cat.Id)
				{
					fams.Add(fs);
					
				}

			}
			
			foreach(Family fam in fams)
			{
				
				using(Transaction trans = new Transaction(doc,"ABS"))
				{
					trans.Start();
					string nombre_int = fam.Name;
					fam.Name = "00 00 00-LOD-300_XX_"+nombre_int;
					
					trans.Commit();
				}
				famnames += fam.Name + Environment.NewLine;
			}
			
			
			
			TaskDialog.Show("INFO" , famnames);
			
			
		
		
		}
		
		public void getViewportInfo()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			string data = "";
			List<Element> elementos = new FilteredElementCollector(doc).OfClass(typeof(View)).ToList();
			Element el = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			Viewport vp = el as Viewport;
			Parameter par = vp.get_Parameter(BuiltInParameter.VIEW_NAME);
			
			string valor = par.AsString();
			List<View> vistas = new List<View>();
			
			foreach(Element elem in elementos)
			{
				View view = elem as View;
				if(view.Name == valor)
				{
					vistas.Add(view);
					
				}
				
			}
			
			foreach(View vv in vistas)
			{
				data += vv.Name + "  : " + vv.Id;
				uidoc.ActiveView = vv;
			}
			
			TaskDialog.Show("INFO",data);
		}
		
		
		public void crearNivel()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			
			double alturaNivel = UnitUtils.ConvertToInternalUnits(15,DisplayUnitType.DUT_METERS);
			
			using(Transaction trans = new Transaction(doc,"creamos nivel"))
			{
				trans.Start();
				
				Level nivel = Level.Create(doc,alturaNivel);
				
				nivel.Name = "NIVEL TESTING";
				
				
				
				trans.Commit();
			}
		}
		
		public void GrahamScanWalls()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			IList<Reference> eles = uidoc.Selection.PickObjects(ObjectType.Element);
			List<Location> locs = EGetLocations(eles,doc);
			
			List<XYZ> points = new List<XYZ>();
			
			
			foreach(Location loc in locs)
			{
				LocationPoint lp = loc as LocationPoint;
				points.Add(lp.Point);

			}
			
			points = points.OrderBy(pt => pt.Y).ToList();
			points = points.OrderBy(pt => Math.Atan2(pt.Y - points[0].Y, pt.X - points[0].X)).ToList();
			
			
			List<XYZ> selPts = new List<XYZ>();
			
			while(points.Count > 0){
				
				GrahamScan(ref points, ref selPts);
				
			};
			
			ElementId leleid = new FilteredElementCollector(doc).OfClass(typeof(Level))
				.First(x => x.Name == "Level 1").Id;
			
			using(Transaction trans = new Transaction(doc,"CONVEX HULL"))
			{
				trans.Start();
				
				
				XYZ prev = null;
				
				foreach(XYZ pt in selPts)
				{
					if(prev == null)
					{
						prev = selPts.Last();
					}
					Line line = Line.CreateBound(prev,pt);
					prev = pt;
					
					Wall muro = Wall.Create(doc,line,leleid,false);
					
				}
				
				trans.Commit();
				
			}
			
			
			
			
			
			
			//TaskDialog.Show("INFO",data);
		}
		
		private List<Location> EGetLocations(IList<Reference> refs , Document doc)
		{
			List<Location> locations = new List<Location>();
			
			
			foreach(Reference re in refs)
			{
				
				Element el = doc.GetElement(re);
				Location loc = el.Location;
				locations.Add(loc);
			}
			
			return locations;
		}
		
		void GrahamScan(ref List<XYZ> pts, ref List<XYZ> selPts)
		{
			if(pts.Count > 0)
			{
				var pt = pts[0];
				if(selPts.Count <= 1)
				{
					selPts.Add(pt);
					pts.RemoveAt(0);
				}
				else{
					var pt1 = selPts[selPts.Count -1];
					var pt2 = selPts[selPts.Count -2];
					XYZ vec1 = pt1 - pt2;
					XYZ vec2 = pt - pt1;
					XYZ Cross = vec1.CrossProduct(vec2);
					
					if(Cross.Z <0)
					{
						selPts.RemoveAt(selPts.Count -1);
					}
					else{
						selPts.Add(pt);
						pts.RemoveAt(0);
					}
					
				}
			}
		}
		
		public void Vistas1Info()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
			
			
			string texto = "";
			
			
			foreach(ElementId eleid in ids)
			{
				View vista = doc.GetElement(eleid) as View;
				
				string nombre = vista.Name;
				
				ViewFamilyType vft = doc.GetElement(vista.GetTypeId()) as ViewFamilyType;
				string vifa = vft.ViewFamily.ToString();
				texto += "NOMBRE DE VISTA :" + nombre + Environment.NewLine +
					"TIPO DE VISTA :"+vista.GetType() + Environment.NewLine+
					"VIEW FAMILY TYPE : " + vifa + Environment.NewLine + Environment.NewLine;
			}
			
			TaskDialog.Show("INFORMACIÓN DE VISTAS SELECCIONADAS" ,texto);
			
			
		}
		
		public void DuplicateSelectedViews()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
			string data = "";
			foreach(ElementId viewId in ids)
			{
				View vista = doc.GetElement(viewId) as View;
				
				using(Transaction trans = new Transaction(doc,"DUPLICATING VIEWS"))
				{
					trans.Start();
					
					ElementId newView = vista.Duplicate(ViewDuplicateOption.WithDetailing);
					View vissta = doc.GetElement(newView) as View;
					vissta.Name = vissta.Name+ " - " + "PROTO_WORK";
					data += vissta.Name + Environment.NewLine + vissta.Id;
					trans.Commit();

				}
	
			}
			
			TaskDialog.Show("INFO",data);
		}
		
		public void CrearVista()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			double alturaNivel = UnitUtils.ConvertToInternalUnits(10,DisplayUnitType.DUT_METERS);
			
			
			ElementId floorplanid = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().First( vf => vf.ViewFamily == ViewFamily.FloorPlan).Id;
			
			
			using(Transaction trans = new Transaction(doc,"VISTA"))
			{
				trans.Start();
				
				
				Level nov_nivel = Level.Create(doc,alturaNivel);
				nov_nivel.Name = "NIVEL FABIAN";
				
				ViewPlan vista = ViewPlan.Create(doc,floorplanid,nov_nivel.Id);
				
				vista.Name = "EL FABIÁN";
				vista.Scale = 200;
				
				
				trans.Commit();
				
				
			}
			
			
		}
		
		public void CrearPlano()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			ElementId tbid = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsElementType().First(tb => tb.Name == "A1 metric").Id;
			
			
			using (Transaction trans = new Transaction(doc,"PLANOS"))
			{
				trans.Start();
				
				ViewSheet plano = ViewSheet.Create(doc,tbid);
				
				plano.Name = "PEJ_Proyecto EJECUTIVO 01";
				plano.SheetNumber = "A100CVC3PLAN";
				
				trans.Commit();
			}
			
		}
		


		public void CreateSheetAndViewInside()
		{
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			
			ElementId viewid = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).First(vv => vv.Name == "NIVEL TESTING").Id;
			
			ElementId planoid =  new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).First(xx => xx.Name == "fabian").Id;
			
			XYZ centerpoint = new XYZ(UnitUtils.ConvertToInternalUnits(524,DisplayUnitType.DUT_MILLIMETERS),UnitUtils.ConvertToInternalUnits(420,DisplayUnitType.DUT_MILLIMETERS),0);
			
			
			using(Transaction trans = new Transaction(doc,"PLANO"))
			{
				trans.Start();
				
				Viewport.Create(doc,planoid,viewid,centerpoint);
				
				
				trans.Commit();
			}
			
		}
		
		public void DuplicateDetailActiveView()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			View ActiveView = doc.ActiveView;
			
			ElementId ops = null;
			
			using(Transaction trans = new Transaction(doc,"CUSTOM DUPLICATE"))
			{
				trans.Start();
				
				ElementId newView = ActiveView.Duplicate(ViewDuplicateOption.WithDetailing);
			
				View nuevaVista = doc.GetElement(newView) as View;
				ops = newView;
				nuevaVista.Name = nuevaVista.Name + " - " + "PROTO";
				
				
				
				trans.Commit();
			}
			
			
			View  nn = doc.GetElement(ops) as View;
			uidoc.ActiveView = nn;
			
			
			
			
			
			
		}

		
		public void PRINTTEST()
		{
			
			//variables GLOBALES
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			Selection sel = uidoc.Selection;
			ICollection<ElementId> ids = sel.GetElementIds();
			
			
			// MÉTODO DE SELECCIÓN
			
			ElementId psId = new FilteredElementCollector(doc).OfClass(typeof(PrintSetting))
				.First(x => x.Name == "TABLOIDES").Id;
			
			ViewSet vset = ViewsetAddViews(ids,doc);
			
			// TRANSACCIÓN ARRANCE
			using(Transaction trans = new Transaction(doc,"PRINTING"))
			{
				trans.Start();
				
				doc.Print(vset,true);
				
				
				
				trans.Commit();
			}
			      
			TaskDialog.Show("DATARCHITECTS","OPERACION EXITOSA");
			
			
			
		}
		
		
		// MÉTODO PRIVADO
		private ViewSet ViewsetAddViews(ICollection<ElementId> ViewIds , Document doc)
		{
			
			
			
			ViewSet vset = new ViewSet();
			
			
			
			//COLOCAR AQUI EL NOMBRE DE LA CONFIGURACIÓN DE IMPRESIÓN(PRINTSETTINGS)
			string match = "WRK-2020";
			
			
			//CICLO 
			foreach(ElementId eleid in ViewIds)
			{
				Element el = doc.GetElement(eleid);
				View view = el as View;
				
				vset.Insert(view);
			}
			
			PrintManager pm = doc.PrintManager;
			
			//EL NOMBRE TAL CUAL APARECE EN REVIT DE LA IMPRESORA 
			pm.SelectNewPrintDriver("Adobe PDF");
			
			//RANGO DE IMPREISÓN
			pm.PrintRange = PrintRange.Select;
			
			//pm.CombinedFile =  true ; QUITAR COMBINE PDF O INSTANCIAR MÉTODO DE CONFIGURACIÓN ACTUAL [EN PROCESO]

			ViewSheetSetting vss = pm.ViewSheetSetting;
			vss.CurrentViewSheetSet.Views = vset;
			
			using(Transaction trans = new Transaction(doc,"GENERATEVIEWSET"))
			{
				trans.Start();
				string SetName = "'" + match + " SHEETS";
				
				try
				{
					vss.SaveAs(SetName);
					
					
				}
				catch(Autodesk.Revit.Exceptions.InvalidOperationException)
				{
					TaskDialog.Show("WARNING!,ERROR!",SetName + "YA ESTA EN USO , USE OTRO  NOMBRE");
					trans.RollBack();
					
				}
				trans.Commit();
			}
			
			return vset;
		}
		public void ISelectionFilter()
		{
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Selection sel = uidoc.Selection;
			//ReferenceArray refa = new ReferenceArray();
			
			ISelectionFilter esl = new WallSelectionFilter();
			IList<Element> eles = sel.PickElementsByRectangle(esl,"seleccione varios obhjetos") ;
			
			StringBuilder stb = new StringBuilder();
			
			
			ICollection<ElementId> eleids = new List<ElementId>();
			
			foreach(Element e in eles)
			{
				eleids.Add(e.Id);
				
				stb.AppendLine(e.Category.Name + " : " +e.Name);
				
			}
			
			sel.SetElementIds(eleids);
			
			TaskDialog.Show("INFO",stb.ToString());
		}
		
		
		public class WallSelectionFilter : ISelectionFilter
		{
			public bool AllowElement(Element element)
			{
				if(element.Category.Name ==   " Walls"){
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
	}
}

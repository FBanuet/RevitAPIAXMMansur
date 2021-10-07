/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 18/09/2020
 * Time: 05:30 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;


namespace jajaja
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("04F6914B-72B8-426F-A3FD-BCF6CC9C86AF")]
	public partial class ThisApplication
	{
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}
		
		public void SelectCate()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			
			ICollection<ElementId> elements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming).ToElementIds();
			uidoc.Selection.SetElementIds(elements);
			
		}
		
		public void ChangeTypeName()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			string names = "";
			
			string up = "Pf-(T)-";
			string down = ".";
			
			foreach(FamilyInstance fi in new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
			        .OfCategory(BuiltInCategory.OST_PlumbingFixtures).Cast<FamilyInstance>())
			{
				

				using(Transaction trans = new Transaction(doc,"FAMS"))
				{
					trans.Start();
					
					fi.Symbol.Name = up +fi.Symbol.Name + down;
					
					trans.Commit();
					
				}
				
				names += fi.Name + Environment.NewLine;
			}
			
			
			TaskDialog.Show("info",names);
		
				
		}
		
		public void testing_rebar()
		{
			// PARÁMETROS GLOBALES
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			/// <summary>
			/// OBTENEMOS EL ELEMENTO HOST PARA HACER EL ARMADO
			/// </summary>
			Reference el = uidoc.Selection.PickObject(ObjectType.Element,"SELECCIONE EL ELEMENTO") as Reference;
			Element host = doc.GetElement(el.ElementId);
			
			
			/// <summary>
			/// OBTENEMOS LA CARA MOLDE PARA OBTENER DE AHI LOS VERTICES 
			/// </summary>
			Reference molde = uidoc.Selection.PickObject(ObjectType.Face,"SELECCIONE LA CARA");
			Element e_face = doc.GetElement(molde);
			GeometryObject geomObj = e_face.GetGeometryObjectFromReference(molde);
			Face f = geomObj as Face;
			PlanarFace pf = f as PlanarFace;
			XYZ normal = pf.ComputeNormal(new UV(pf.Origin.X,pf.Origin.Y));
			
			var xo = UnitUtils.ConvertFromInternalUnits(pf.Origin.X, DisplayUnitType.DUT_METERS);
			var yo = UnitUtils.ConvertFromInternalUnits(pf.Origin.Y, DisplayUnitType.DUT_METERS);
			var zo = UnitUtils.ConvertFromInternalUnits(pf.Origin.Z, DisplayUnitType.DUT_METERS);
			
			/// <summary>
			/// TESTING THE BUILDING CODER ALGOTIHM
			/// </summary>
			/// 
			string name = "M_T1";
			ElementId eleid = null;
			
			
			foreach(RebarShape rs in new FilteredElementCollector(doc).OfClass(typeof(RebarShape)).Cast<RebarShape>())
			{
				if(rs.Name == name)
					eleid = rs.Id;
			}
			RebarShape rebarshape = doc.GetElement(eleid) as RebarShape;
			
			TaskDialog.Show("info",rebarshape.Name);
		}
		

		public void ExporttoCad()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(ViewPlan));
			
			
			DWGExportOptions EXP = new DWGExportOptions();
			EXP.FileVersion = ACADVersion.R2013;
			bool merge = false;
			EXP.MergedViews = merge;
			
			ICollection<ElementId> vistas_ids = new List<ElementId>();
			
			foreach(Element element in collector)
			{
				ViewPlan viewplan = element as ViewPlan;
				vistas_ids.Add(viewplan.Id);
			}
			
			string folder = @"C:\Users\Admin\Music\test\TEST_2";
			string name = " DD_AHP_2020_EngineeringPlans.dwg";
			
			using(Transaction trans = new Transaction(doc,"EXPORT"))
			{
				ICollection<ElementId> vistas = FilterViewsByKey(vistas_ids,doc);
				
				doc.Export(folder,name,vistas,EXP);
				
			
			}
			

			TaskDialog.Show("info","LA OPERACION FINALIZO CON EXITO!" + Environment.NewLine +"springall + mk  tools v1.0.0.1");
		
		}
		
		public ICollection<ElementId> FilterViewsByKey(ICollection<ElementId> ids , Document doc)
		{
			ICollection<ElementId> results = new List<ElementId>();
			
			foreach(ElementId eleID in ids)
			{
				ViewPlan vp =doc.GetElement(eleID) as ViewPlan;
				ViewType vt = vp.ViewType;
				if(vt.ToString() == "EngineeringPlan")
				{
					results.Add(vp.Id);
					
				}
				
			}
			return results;
			
			
		}
			
		
		public void ChangeFamilyNameByCategory()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ICollection<Element> elementos = collector.OfClass(typeof(Family)).ToElements();
			string famnames = "";
			
			int contador = 0;
			
			Category cat = Category.GetCategory(doc,BuiltInCategory.OST_PlumbingFixtures);
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
				contador +=1;
				using(Transaction trans = new Transaction(doc,"ABS"))
				{
					trans.Start();
					string nombre_int = fam.Name;
					fam.Name = "Pf-MEP-"+"("+nombre_int+")";
					
					trans.Commit();
				}
				famnames += fam.Name + Environment.NewLine;
			}
			
			
			
			TaskDialog.Show("INFO" , famnames);
		}
		
		public void GetParameter()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element e = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			string data = "";
			foreach(BuiltInParameter bip in Enum.GetValues(typeof(BuiltInParameter)))
			{
				try
				{
					Parameter p = e.get_Parameter(bip);
					data += bip.ToString() + ": " + p.Definition.Name+": ";
					
					if(p.StorageType == StorageType.String)
						data+= "ES DE TIPO STRING    " + p.AsString();
					else if(p.StorageType == StorageType.Integer)
						data+= " ES DE TIPO INTEGER    " + p.AsInteger();
					else if(p.StorageType == StorageType.Double)
						data+= " ES DE TIPO DOBULE    " + p.AsDouble();
					else if(p.StorageType == StorageType.ElementId)
						data+= "ES UN ELEMENT ID      " + "ID" + p.AsElementId().IntegerValue;
					data += "\n";
					
					
					
				}
				catch
				{
					
				}
			}
			
			TaskDialog.Show("BI PARAMS" , data);
		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
	}
}
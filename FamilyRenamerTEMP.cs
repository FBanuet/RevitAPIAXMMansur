/*
 * Created by SharpDevelop.
 * User: S-MK
 * Date: 16/12/2020
 * Time: 12:37 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace IUYET
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("C403FF4C-6EDF-4CBE-8394-B48CB747F6EF")]
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
		public void RENOMBRARCOLUMNAS()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ICollection<Element> elementos = collector.OfClass(typeof(Family)).ToElements();
			string famnames = "";
			
			
			
			Category cat = Category.GetCategory(doc,BuiltInCategory.OST_StructuralColumns);
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
		
		public void RENOMBRARPUERTAS()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ICollection<Element> elementos = collector.OfClass(typeof(Family)).ToElements();
			string famnames = "";
			
			
			
			Category cat = Category.GetCategory(doc,BuiltInCategory.OST_Doors);
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
		
		public void RENOMBRARFLOORS()
			
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ICollection<Element> elementos = collector.OfClass(typeof(Family)).ToElements();
			string famnames = "";
			
			
			
			Category cat = Category.GetCategory(doc,BuiltInCategory.OST_Floors);
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
		public void RENOMBRARSTRUCTURALFOUNDATIONS()
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
		
		public void RENOMBRARSTRUCTURALFRAMING ()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ICollection<Element> elementos = collector.OfClass(typeof(Family)).ToElements();
			string famnames = "";
			
			
			
			Category cat = Category.GetCategory(doc,BuiltInCategory.OST_StructuralFraming);
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
		
		public void RENOMBRARMUROS()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ICollection<Element> elementos = collector.OfClass(typeof(Family)).ToElements();
			string famnames = "";
			
			
			
			Category cat = Category.GetCategory(doc,BuiltInCategory.OST_Walls);
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
	}
}
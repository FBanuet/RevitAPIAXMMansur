/*
 * Created by SharpDevelop.
 * User: S-MK
 * Date: 29/12/2020
 * Time: 12:31 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMHUB
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("FAAA0816-2149-4877-89FF-E4EDECBA02B4")]
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
		public void InfoMuro()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Wall muro  = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element)) as Wall;
			
			string data = "INFORMACIÓN DEL MURO: " + Environment.NewLine;
			
			data += "Walltype : " + muro.WallType.Name + Environment.NewLine;
			data += "Grosor Muro es :" + muro.Width + Environment.NewLine;
			data += "Orientación :" + muro.Flipped + Environment.NewLine;
			data += "Uso estructural " + muro.StructuralUsage;
			
			
			TaskDialog.Show("DATA",data);
		
			
			
		}
		
		
		private TaskDialogResult MetodoInformaciónMuro(Wall muro)
		{
			string data = "INFORMACIÓN DEL MURO: " + Environment.NewLine;
			
			data += "Walltype : " + muro.WallType.Name + Environment.NewLine;
			data += "Grosor Muro es :" + muro.Width + Environment.NewLine;
			data += "Orientación :" + muro.Flipped + Environment.NewLine;
			data += "Uso estructural " + muro.StructuralUsage;
			
			
			TaskDialogResult resultado = TaskDialog.Show("bnt info" , data);
			
			return resultado;
			
		}
		
		
		public void Testing()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Wall muro  = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element)) as Wall;
			
			MetodoInformaciónMuro(muro);
			
		}
		
		private Level LevelByName(Document doc,string levelName)
		{
			FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(Level));
			
			foreach(Level level in collector)
			{
				if(level.Name == levelName)
					return level;
				
				
			}
			return null;
		}
		
		public void GetLevel()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			string levelname = "Level 2";
			Level nivel = LevelByName(doc,levelname);
			
			string infor = nivel.Name + Environment.NewLine + nivel.Elevation+Environment.NewLine+nivel.Id;
			
			TaskDialog.Show("INFO", infor);
		
			
		}
		
		
		
		public void PlanosAgregarPrefijoyEscala()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			try
			{
				
				FilteredElementCollector planos = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));
				
				using(Transaction trans = new Transaction(doc,"SOBRESCRIBIR PLANOS"))
				{
					trans.Start();
					
					string valor = "LOS NOMBRES DE PLANOS Y SUS RESPECTIVAS ESCALAS SON :\n\n";
					foreach(Element ele in planos)
					{
						ViewSheet vs = ele as ViewSheet;
						Parameter nombre = vs.get_Parameter(BuiltInParameter.SHEET_NAME);
						Parameter escala = vs.get_Parameter(BuiltInParameter.SHEET_SCALE);
						string newName = "PEJ_"+nombre.AsString();
						
						nombre.Set(newName);
						valor += newName + " , " + escala.AsString() + Environment.NewLine + Environment.NewLine;
						
					}
					
					TaskDialog.Show("RESULTADO SCRIPT",valor);
					
					
					trans.Commit();
				}
				
			}
			catch(Exception e)
			{
				TaskDialog.Show("ERROR!",e.ToString());
			}
			
		}
		
		public void SeleccionenSeleccion()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			IList<Element> elementosSeleccionados = uidoc.Selection.PickElementsByRectangle("Seleccione Elementos");
			
			string nombre = "Los elementos seleccionados son: " + elementosSeleccionados.Count() + Environment.NewLine + Environment.NewLine;
			
			foreach(Element ele in elementosSeleccionados)
			{
				nombre += ele.Name + Environment.NewLine;
				
			}
			
			TaskDialog.Show("Elementos seleccionados",nombre);
			
			List<Element> muros = new List<Element>();
			
			foreach(Element elem in elementosSeleccionados)
			{
				if(elem is Wall)
					muros.Add(elem);
				
			}
			
			string murostt = "LOS MUROS DENTRO DE LA SELECCION SON: " + muros.Count() + Environment.NewLine+Environment.NewLine;
			
			foreach(Wall muro in muros)
				murostt += muro.Name + Environment.NewLine;
			
			if(!(muros.Count() == 0))
				TaskDialog.Show("MUROS EN SELECCION FILTRADA : ",murostt);
			else
				TaskDialog.Show("MUROS EN SELECCION FILTRADA : ","NO HAY MUROS EN SU SELECCION");
		}
		
		public void SeleccionFiltrada()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			IList<Element> elementosSeleccionados = uidoc.Selection.PickElementsByRectangle("Seleccione Elementos");
			
			string nombre = "Los elementos seleccionados son: " + elementosSeleccionados.Count() + Environment.NewLine + Environment.NewLine;
			
			foreach(Element ele in elementosSeleccionados)
			{
				nombre += ele.Name + Environment.NewLine;
				
			}
			TaskDialog.Show("Elementos seleccionados",nombre);
			
			List<ElementId> eleids = new List<ElementId>();
			
			foreach(Element elemento in elementosSeleccionados)
				if(elemento is Floor)
					eleids.Add(elemento.Id);
			
			uidoc.Selection.SetElementIds(eleids);
			
			if(!(eleids.Count() == 0))
				TaskDialog.Show("Muros en selección","El numero de muros es :" + eleids.Count());
			else
				TaskDialog.Show("Muros en selección","NULL! ERROR! NO HAY MUROS EN LA SELECCIÓN!");
		}
		
		public void ParametrosEjemplar()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element elem = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			ParameterSet parameters = elem.Parameters;
			
			string instanceparam = "Los parámetros de instancia y sus valores son:\n\n";
			
			foreach(Parameter param in parameters)
			{
				instanceparam += param.Definition.Name +":" + param.AsValueString() + "\n";
			}
			
			TaskDialog.Show("INFO",instanceparam);
			
		}
		
		public void ParametrosInstancia()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element elemento = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			// MÉTODO ParameterSet =  lista de parámetros anidada en un ObJETO DE LA CLASE ParameterSet
			ParameterSet parametros = elemento.Parameters;
			
			//CREACIÓN DE LA CLASE STRINGBUILDER (using.System.Text);
			
			StringBuilder datos = new StringBuilder().AppendLine("LOS PARAMETROS DE INSTANCIA Y SUS VALORES SON:\n\n");
			
			foreach(Parameter param in parametros)
			{
				string dato  = param.Definition.Name + " : " + param.AsValueString();
				datos.AppendLine(dato);
			}
			
			TaskDialog.Show("Parámetros",datos.ToString());
		}
		
		public void ParametroTipo()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element elem = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			ElementType tipo = doc.GetElement(elem.GetTypeId()) as ElementType;
			
			ParameterSet tipoparam = tipo.Parameters;
			
			StringBuilder datos = new StringBuilder().AppendLine("LOS PARAMETROS DE TIPO Y SUS VALORES SON:\n\n");
			
			foreach(Parameter param in tipoparam)
			{
				string dato  = param.Definition.Name + " : " + param.AsValueString();
				datos.AppendLine(dato);
			}
			
			TaskDialog.Show("Parámetros",datos.ToString());
			
			
		}
		
		public void GetTypeParameters()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element elem = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			Element tipo = doc.GetElement(elem.GetTypeId());
			
			if(tipo != null)
			{
				ParameterSet parametrosTipo = tipo.Parameters;
				
				string datosTipo = "Los parámetros de tipo y sus valores son :\n\n";
				foreach(Parameter param in parametrosTipo)
				{
					string valores = param.AsValueString();
					if(valores == null)
						valores = param.AsString();
					datosTipo += param.Definition.Name + " : " + valores + "\n";
				}
				
				TaskDialog.Show("TYPE PARAMETERS",datosTipo);
				
				
			}
			else
				TaskDialog.Show("ERROR","EL ELEMENTO SELECCIONADO NO ES UN ELEMENT TYPE ( NO CONTIENE UN TIPO DE FAMILIA!)");
			

		}
		
		public void GetOrderedParameters()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element elemento = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			//Método GetOrderedParametrs() , para obtener los párametros ordenados visibles den la Interfaz de Revit en el properties
			//Palette , por medio de la función GetOrderedParametrs() , la cual necesita como parámetro una IList<Parameter>
			
			IList<Parameter> parametros = elemento.GetOrderedParameters();
			
			//CREACIÓN DE LA CLASE STRINGBUILDER (using.System.Text);
			
			StringBuilder datos = new StringBuilder().AppendLine("LOS PARAMETROS DE INSTANCIA Y SUS VALORES SON:\n\n");
			
			foreach(Parameter param in parametros)
			{
				string dato  = param.Definition.Name + " : " + param.AsValueString();
				datos.AppendLine(dato);
			}
			
			TaskDialog.Show("Parámetros",datos.ToString());
			
		}
		
		public void UIHumanParameteros()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element element = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,"SELECCIONE UN OBJETO EN REVIT"));
			
			frmPARAMETERS formulario = new frmPARAMETERS(element);
			
			formulario.ShowDialog();
			
		}
		
		public void SetValorParametro()
		{
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			try
			{
				
				Element ele = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,"SELECCIONAR ELEMENTO!"));
				
				Parameter para = ele.get_Parameter(BuiltInParameter.KEYNOTE_PARAM);
				
				
				
				using(Transaction trans = new Transaction(doc,"PARAMETROS"))
				{
					trans.Start();
					
					para.SetValueString("05 53 00.10");
					
					
					trans.Commit();
				}
				
				TaskDialog.Show("INFO",para.StorageType.ToString());
				
			}
			catch(Exception e)
			{
				TaskDialog.Show("INFO",e.ToString());
				
			}
			
			
		}
		
		public void InterralcionParametros()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			try
			{
				
				Element el = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,"SELECCIONA UN ELEMENTO"));
				
				TaskDialogResult resultado = TaskDialog.Show("WARNING!","¿Seguro que deseas cambiar el elemento?", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No);
				
				if(resultado == TaskDialogResult.Yes)
				{
					Parameter offset = el.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET);
					Parameter area = el.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);
					
					TaskDialog.Show("ELEMENT INFO",string.Format("EL ELEMENTO SELECCIONADO TIENE POR DESFASE DE BASE {0} Y POR AREA {1}",offset.AsValueString(),
					                                             area.AsValueString()));
					string nuevoValor = "5";
					
					using(Transaction trans = new Transaction(doc,"INFO"))
					{
						
						trans.Start();
						
						offset.SetValueString(nuevoValor);
						
						
						
						trans.Commit();
						
					}
					
					string mensaje = "EL DESFASE O OFFSET QUEDA EN " + nuevoValor;
					TaskDialog.Show("ELEMENTO CAMBIADO",mensaje);
					
					mensaje = "EL ÁREA DEL MURO CAMBIADO QUEDO EN : " + area.AsValueString();
					
					TaskDialog.Show("EL ÁREA:",mensaje);
					
					
				}
				else
				{
					TaskDialog.Show("MENSAJE","CANCELADO ");
				}
				
			}
			catch(Exception e)
			{
				TaskDialog.Show("ERROR!",e.ToString());
				
			}
			
		}
		
		public void ParametrosTipoAlmacenamiento()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			try{
				
				Element ele = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
				ParameterSet parmset = ele.Parameters;
				
				string data = "";
				
				foreach(Parameter para in parmset)
				{
					data += para.Definition.Name + " = ";
					if(para.StorageType.Equals(StorageType.String))
						data += para.AsString();
					else if (para.StorageType.Equals(StorageType.Double))
						data += para.AsDouble();
					else if (para.StorageType.Equals(StorageType.Integer))
						data += para.AsInteger();
					else if (para.StorageType.Equals(StorageType.ElementId))
						data += para.AsElementId().IntegerValue;
					
					data += "\n";
				}
				
				TaskDialog.Show("DATOS",data);
				
				
				
			}
			catch(Exception e)
			{
					
				TaskDialog.Show("ERROR!",e.ToString());
				
			}
		}
	
	}
}
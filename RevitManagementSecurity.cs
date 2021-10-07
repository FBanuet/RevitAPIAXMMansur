/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 29/09/2020
 * Time: 03:57 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace RevitManagementSecurity
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("C538797A-6F11-4C25-B24A-E582EC158642")]
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
		public void DynamicModelUpdate()
		{
		}
		
		public class WallTypeUpdater :IUpdater
		{
			static AddInId m_appId;
			static UpdaterId m_updaterId;
			public WallTypeUpdater(AddInId id)
			{
				m_appId = id;
				m_updaterId = new UpdaterId(m_appId,new Guid("3b6fcf63-1ca2-4bf8-a495-c10627daf798"));
			}
			public void Execute(UpdaterData data)
			{
				Document doc = data.GetDocument();
				
				foreach(ElementId addedElemId in data.GetAddedElementIds())
				{
					WallType walltype = doc.GetElement(addedElemId) as WallType;
					string name = walltype.Name;
					doc.Delete(addedElemId);
					TaskDialog.Show("WARNING! ACCION DENEGADA" , "FAVOR DE CONTACTAR AL ADMINISTRADOR DEL MODELO " + Environment.NewLine + "TIPO :" + name + "SE HA ELIMINADO LA MODIFICACION O NUEVA FAMILIA GENERADA , PERMISO DENEGADO");
				}
				foreach(WallType et in new FilteredElementCollector(doc).OfClass(typeof(WallType)).Cast<WallType>())
				{
					WallType walltype = doc.GetElement(et.Id) as WallType;
					string wallname = et.Name;
					if(et.Name != wallname)
					{
						doc.Delete(et.Id);
					}
					else
					{
						doc.Delete(et.Id);
					}
					
					
					TaskDialog.Show("WARNING! ACCION DENEGADA" , "FAVOR DE CONTACTAR AL ADMINISTRADOR DEL MODELO " + Environment.NewLine + "TIPO :" + wallname + "SE HA ELIMINADO LA MODIFICACION O NUEVA FAMILIA GENERADA , PERMISO DENEGADO");
					
				}
			}
			
			public string GetAdditionalInformation(){return "DATARCHITECTS SECURITY BIM";}
			public ChangePriority GetChangePriority(){return ChangePriority.FloorsRoofsStructuralWalls;}
			public UpdaterId GetUpdaterId(){return m_updaterId;}
			public string GetUpdaterName(){return "DATARCHITECTS SECURITY BIM";}
			
		}
		
		/// <summary>
		/// REGISTER LOCKERS COMMAND (REGISTRANDO LOS CANDADOS Y EL COMANDO DE UNREGISTER O QUITAR REGISTRO DE CANDADOS"
		/// </summary>
		
		public void RegisterFamilyEditCommand()
		{
			WallTypeUpdater updater = new WallTypeUpdater(this.Application.ActiveAddInId);
			
			
			UpdaterRegistry.RegisterUpdater(updater);
			
			

			ElementClassFilter wallTypeFilter = new ElementClassFilter(typeof(WallType));
			
			
			UpdaterRegistry.AddTrigger(updater.GetUpdaterId(),wallTypeFilter,Element.GetChangeTypeElementAddition());
			
			
		}
		public void UnregisterUpdater()
		{
			WallTypeUpdater updater = new WallTypeUpdater(this.Application.ActiveAddInId);
			
			
			UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());
			
			
		}
		
		
		
		public void RegisterFamilyEditCommand_2()
		{
			
			FloorTypeUpdater f_updater = new FloorTypeUpdater(this.Application.ActiveAddInId);
			
			
			UpdaterRegistry.RegisterUpdater(f_updater);
			

			
			ElementClassFilter floorTypeFilter = new ElementClassFilter(typeof(FloorType));
			
			
			UpdaterRegistry.AddTrigger(f_updater.GetUpdaterId(),floorTypeFilter,Element.GetChangeTypeElementAddition());
			
		}
		public void UnregisterUpdater_2()
		{
			
			FloorTypeUpdater updater_f = new FloorTypeUpdater(this.Application.ActiveAddInId);
			
			
			UpdaterRegistry.UnregisterUpdater(updater_f.GetUpdaterId());
			
		}
		
		
		
		
		
		/// <summary>
		/// floor type locked
		/// </summary>
		
		
		
		
		
		
		public class FloorTypeUpdater :IUpdater
		{
			static AddInId m_appId;
			static UpdaterId m_updaterId;
			public FloorTypeUpdater(AddInId id)
			{
				m_appId = id;
				m_updaterId = new UpdaterId(m_appId,new Guid("871bfb00-dd3a-47ca-8d15-9df4eedfff6d"));
			}
			public void Execute(UpdaterData data)
			{
				Document doc = data.GetDocument();
				
				foreach(ElementId addedElemId in data.GetAddedElementIds())
				{
					FloorType flortype = doc.GetElement(addedElemId) as FloorType;
					string name = flortype.Name;
					doc.Delete(addedElemId);
					TaskDialog.Show("WARNING! ACCION DENEGADA" , "FAVOR DE CONTACTAR AL ADMINISTRADOR DEL MODELO " + Environment.NewLine + "TIPO :" + name + "SE HA ELIMINADO LA MODIFICACION O NUEVA FAMILIA GENERADA , PERMISO DENEGADO");
				}
			}
			
			public string GetAdditionalInformation(){return "DATARCHITECTS SECURITY BIM";}
			public ChangePriority GetChangePriority(){return ChangePriority.FloorsRoofsStructuralWalls;}
			public UpdaterId GetUpdaterId(){return m_updaterId;}
			public string GetUpdaterName(){return "DATARCHITECTS SECURITY BIM";}
			
		}
	}
}
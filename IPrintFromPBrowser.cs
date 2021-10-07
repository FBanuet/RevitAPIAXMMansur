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
		
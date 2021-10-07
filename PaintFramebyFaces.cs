public void paintFrame ()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document Documento = uidoc.Document;
			/*
			FilteredElementCollector Trabes = new FilteredElementCollector(Documento).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_StructuralFraming);
			
			string nombre = "";
			
			foreach(Element Trabe in Trabes)
			{
				nombre += Trabe.Name;
			}
			
			TaskDialog.Show("Resultado",nombre);
			
			*/
			

			//LINQ

			List<Element> trabes = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
			.OfCategory(BuiltInCategory.OST_StructuralFraming)
			.Where(el => el.Name == "TRABES_PORTANTES_ITISA")
			.OrderBy(el => el.Id)
			.ToList();

			Element trabe = Documento.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			List<Face> caras = new List<Face>();
			
			foreach (GeometryInstance geometryObject in trabe.get_Geometry(new Options ()))
    		{
       		 if (geometryObject != null)
        		{
            		GeometryElement geo = geometryObject.GetInstanceGeometry();
            		
            		foreach(GeometryObject abc in geo)
            		{
            			Solid Solido = abc as Solid;
            			foreach(Face carass in Solido.Faces)
            			{
            				caras.Add(carass);
            			}
            		}
            		
        		}
    		}
			
    		string tex = "";
    		
    		List<double> Areas = new List<double>();
    		
    		foreach (Face cara in  caras)
    		{
    			Areas.Add(UnitUtils.ConvertFromInternalUnits(cara.Area, DisplayUnitType.DUT_SQUARE_METERS));
    			if(Documento.IsPainted(trabe.Id,cara))
    			
    			{
    				var area = UnitUtils.ConvertFromInternalUnits(cara.Area, DisplayUnitType.DUT_SQUARE_METERS);
    				tex += "La cara esta pintada" + " " + area.ToString() + Environment.NewLine;
    			}
    			
    			else
    			{
    				var area = UnitUtils.ConvertFromInternalUnits(cara.Area, DisplayUnitType.DUT_SQUARE_METERS);
    				tex += "La cara NO esta pintada" + " " + area.ToString() + Environment.NewLine;
    			}
    		}
    		
    		string Resultado = Areas.Sum().ToString();
    		
    		TaskDialog.Show ("Resultados", Resultado);
		}
	}
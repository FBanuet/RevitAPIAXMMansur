public void ISelectionFilter()
		{
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			//ReferenceArray refa = new ReferenceArray();
			
			ISelectionFilter esl = new WallSelectionFilter();
			IList<Reference> eles = uidoc.Selection.PickObjects(ObjectType.Element,esl,"seleccione varios muros");
			
			StringBuilder stb = new StringBuilder();
			
			foreach(Reference re in eles)
			{
				Element e = doc.GetElement(re);
				stb.AppendLine(e.Category.Name + " : " +e.Name);
				
			}
			
			TaskDialog.Show("INFO",stb.ToString());
		}
		
		
		public class WallSelectionFilter : ISelectionFilter
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
			
			public bool AllowReference(Reference rfs , XYZ point)
			{
				return false;
			}
			
		}
	}
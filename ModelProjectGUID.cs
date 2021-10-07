		public void ModelProjectGuid()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			string projectGuid = doc.GetCloudModelPath().GetProjectGUID().ToString();
			string Modelgid = doc.GetCloudModelPath().GetModelGUID().ToString();
			
			TaskDialog.Show("INFO","PROJECT GUID" +Environment.NewLine + projectGuid + Environment.NewLine + "MODEL GUID: " + Environment.NewLine + Modelgid);
		}
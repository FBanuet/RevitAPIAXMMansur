

clr.AddReference("DSCoreNodes")
import DSCore as DS

import System.Drawing
import System.Windows.Forms

from System.Drawing import  Point , Size , Graphics, Bitmap, Image, Font, FontStyle, Icon, Color, Region , Rectangle , ContentAlignment
from System.Windows.Forms import Button,Label,ListView,Form,CheckState


clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import FilteredElementCollector,BuiltInParameter,RevitLinkInstance,Level,ElementLevelFilter,Document,BuiltInCategory,Category

from Autodesk.Revit.DB.Plumbing import Pipe


clr.AddReference("RevitAPIUI")
from Autodesk.Revit.UI.Selection import Selection,ObjectType,ISelectionFilter
from Autodesk.Revit.UI import TaskDialog



#from Autodesk.Revit.UI import *

clr.AddReference("RevitServices")
import RevitServices
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager
doc = DocumentManager.Instance.CurrentDBDocument
uiapp = DocumentManager.Instance.CurrentUIApplication
app = uiapp.Application
uidoc = DocumentManager.Instance.CurrentUIApplication.ActiveUIDocument
doc = DocumentManager.Instance.CurrentDBDocument





class CustomISelectionFilter(ISelectionFilter):

	def __init__(self, categoryIds):
		self.CategoryIds = categoryIds
		
	def AllowElement(self, e):
		
		if e.Category.Id in self.CategoryIds:
			return True
		else:
			return False
	def AllowReference(self, ref, point):
		return true


def __interface__():

	
	
	categories = [BuiltInCategory.OST_Topography]
	
		
	catergoryIds = [Category.GetCategory(doc,c).Id for c in categories]
	TaskDialog.Show("Hi!","Select one or more topographies in Revit and click 'Finish'")
	try:	
		elements = [doc.GetElement(ref) for ref in uidoc.Selection.PickObjects(ObjectType.Element,CustomISelectionFilter(catergoryIds))]	
	except:
		return []
	

	return elements

x = __interface__()
OUT = x

import clr

import sys
sys.path.append(r'C:\Program Files (x86)\IronPython 2.7\Lib')

clr.AddReference('ProtoGeometry')
import Autodesk.DesignScript.Geometry as DG

clr.AddReference("DSCoreNodes")
import DSCore as DS

clr.AddReference("RevitNodes")
import Revit.Elements as DR

import Revit
clr.ImportExtensions(Revit.Elements)
clr.ImportExtensions(Revit.GeometryConversion)
#
clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *
from Autodesk.Revit.DB.Mechanical import *
from Autodesk.Revit.DB.Structure import *
clr.AddReference("RevitAPIUI")
from Autodesk.Revit.UI import *

clr.AddReference("RevitServices")
import RevitServices
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager
doc = DocumentManager.Instance.CurrentDBDocument
uiapp = DocumentManager.Instance.CurrentUIApplication
app = uiapp.Application
uidoc = DocumentManager.Instance.CurrentUIApplication.ActiveUIDocument
doc = DocumentManager.Instance.CurrentDBDocument

import System
from System import Array as CArray
clr.AddReference("System")
from System.Collections.Generic import List as CList
from System.Collections.Generic import Dictionary

import time
import math
import copy
import string
import datetime
import random
import os
import shutil


def Visualize(geometryOrList):

	def Convert(geometry):

		lst =[]

		if geometry is None:
			lst.append(None)

		elif geometry.GetType() == Solid:

			edges = [e.AsCurve().ToProtoType() for e in  geometry.Edges]
			lst.append(edges)

		elif isinstance(geometry, Face):

			for loop in geometry.EdgeLoops:
				for edge in loop:
					lst.append(edge.AsCurve().ToProtoType())

		elif geometry.GetType() == BoundingBoxXYZ:

			minp,maxp = geometry.Min, geometry.Max

			p1 = minp
			p2 = XYZ(minp.X, maxp.Y, minp.Z)
			p3 = XYZ(maxp.X, maxp.Y, minp.Z)
			p4 = XYZ(maxp.X, minp.Y, minp.Z)

			points = [p1,p2,p3,p4]

			curves = CList[Curve]([Line.CreateBound(points[i-1],points[i]) for i in range(len(points))])
			curveLoop = CurveLoop.Create(curves)
			curveLoopList = CList[CurveLoop]([curveLoop])

			solid = GeometryCreationUtilities.CreateExtrusionGeometry(curveLoopList, XYZ.BasisZ, math.fabs(maxp.Z - minp.Z) )

			edges = [e.AsCurve().ToProtoType() for e in  solid.Edges]

			lst.append(edges)

		elif geometry.GetType() == XYZ:

			lst.append(geometry.ToPoint())

		elif isinstance(geometry, Curve):

			lst.append(geometry.ToProtoType())

		return lst

	def Go_into(geometryOrList, lst):

		if hasattr(geometryOrList, "__iter__"):
			sublist = []
			for gol in geometryOrList:
				res = Go_into(gol,sublist)
				sublist.append(res)
			return sublist
		else:
			geometryList = Convert(geometryOrList)
			return geometryList

	lst = Go_into(geometryOrList,[])
	return lst


def RebuildLoop(curves,minAngle,tolerance):


	def Compare(curve1,curve2):

		v1 = curve1.ComputeDerivatives(1,True).BasisX
		v2= curve2.ComputeDerivatives(0,True).BasisX
		p11 = curve1.Evaluate(0, True)
		p12 = curve1.Evaluate(1, True)

		pmid = curve1.Evaluate(0.5, True)

		p21 = curve2.Evaluate(0, True)
		p22 = curve2.Evaluate(1, True)

		if v1.AngleTo(v2) < math.radians(minAngle) and p12.DistanceTo(p21)< tolerance/304.8:
			return p11,p22,pmid
		else:
			return None
	i = 0
	while i< len(curves):
		com = Compare(curves[i-1],curves[i])
		if com is None:
			i += 1
		else:
			if com[0] == Arc:
				curves[i-1] = Arc.Create(com[0],com[1],com[2])
			else:
				curves[i-1] = Line.CreateBound(com[0],com[1])
			del curves[i]
	return curves


def BakeGeometryAsFamilyInstance(solids,name,familyTemplatePath,color):

	#familyPath = "C:\ProgramData\Autodesk\RVT 2019\Family Templates\Russian\Метрическая система, типовая модель"

	class NewFolder():

		def __init__(self):

			dataPath = os.getenv('APPDATA')

			dirPath = dataPath + "//Dynamograph"
			try:
				os.mkdir(dirPath)
			except:
				pass

			self.Path = dirPath

		def Delete(self):

			shutil.rmtree(self.Path)

	geoElements = []

	class FamOpt1(IFamilyLoadOptions):
		def __init__(self):
			pass
		def OnFamilyFound(self,familyInUse, overwriteParameterValues):
			return True
		def OnSharedFamilyFound(self,familyInUse, source, overwriteParameterValues):
			return True

	folder = NewFolder()
	folderPath = folder.Path

	saveAsOpt = SaveAsOptions()
	saveAsOpt.OverwriteExistingFile = True

	elements = FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_MassForm).WhereElementIsNotElementType().ToElements()
	names = []
	for e in elements:
		try:
			type = doc.GetElement(e.GetTypeId())
			names.append(type.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_NAME).AsString())
		except: pass

	TransactionManager.Instance.ForceCloseTransaction()
	famDoc = doc.Application.NewFamilyDocument(familyTemplatePath)
	TransactionManager.Instance.EnsureInTransaction(famDoc)
	materialId = Material.Create(famDoc,name)
	material = famDoc.GetElement(materialId)
	material.Color = color
	#Color(random.random()*255,random.random()*255,random.random()*255)

	for solid in solids:
		try:
			freeForm = FreeFormElement.Create(famDoc, solid)
			freeForm.get_Parameter(BuiltInParameter.MATERIAL_ID_PARAM).Set(materialId)
		except:
			pass
	TransactionManager.Instance.ForceCloseTransaction()

	famDoc.SaveAs(folderPath +"\\"+name+"_1.rfa", saveAsOpt)
	family1 = famDoc.LoadFamily(doc, FamOpt1() )
	famDoc.Close(False)

	symbols = family1.GetFamilySymbolIds().GetEnumerator()
	symbols.MoveNext()
	symbol1 = doc.GetElement(symbols.Current)

	t1 = TransactionManager.Instance
	t1.EnsureInTransaction(doc)
	if not symbol1.IsActive:
		symbol1.Activate()

	familyInstance = doc.Create.NewFamilyInstance(XYZ(0,0,0), symbol1, StructuralType.NonStructural)

	TransactionManager.ForceCloseTransaction(t1)

	folder.Delete()

	return familyInstance

def GetHorizontalLoopsBySolid(solid,up):

	faces = list(solid.Faces)

	if up:
		filteredFaces = [f for f in faces if f.ComputeNormal(UV(0,0)).Normalize().Z > 0.99]
	else:
		filteredFaces = [f for f in faces if f.ComputeNormal(UV(0,0)).Normalize().Z < -0.99]

	filteredFaces.sort(key = lambda x:x.Area)
	filteredFace = filteredFaces[-1]

	loops = list(filteredFace.GetEdgesAsCurveLoops())
	loops = GetExternalAndInternalLoops(loops)
	return loops

def GetExternalAndInternalLoops(loops):

	xx,yy,zz = [],[],[]
	for l in loops:
		for c in l:
			for p in c.Tessellate():
				xx.append(p.X)
				yy.append(p.Y)
				zz.append(p.Z)

	point = XYZ(min(xx),min(yy),min(zz))

	minDist = 9999999
	index = 0
	for i,loop in enumerate(loops):
		for curve in loop:
			d = curve.Distance(point)
			if d < minDist:
				minDist = d
				index = i

	outLoop = loops.pop(index)
	return outLoop,loops


def BooleanOperationWithTolerance(solid1, solid2, bop, steps, x, y, z):

	for i in range(steps):

		if i == 0:
			v = XYZ(0,0,0)
		else:
			v = XYZ(random.random()*x, random.random()*y, random.random()*z)

		transform = Transform.CreateTranslation(v)
		transformedSolid1 = SolidUtils.CreateTransformed(solid1, transform)
		try:
			result = BooleanOperationsUtils.ExecuteBooleanOperation(transformedSolid1, solid2, bop)
		except:
			continue
		if result.Volume > 0:
			return result
		else:
			continue


def TopographySolidUnion(solids,lengthOfGroup,numberOfAttempts):

	def TryToJoin(solids):
		solid0 = solids.pop(0)#
		bop = BooleanOperationsType.Union
		report = []
		for s in solids:
			result = BooleanOperationWithTolerance(solid0, s, bop, 100 , 0, 0, 10/304.8)
			if result is None:
				report.append(s)
			else:
				solid0 = result
		return [solid0]+report

	lengthOfGroup = 100
	numberOfAttempts = 10

	for j in range(numberOfAttempts):

		joinedSolids = []
		groups = []
		for i in range(0, len(solids), lengthOfGroup):
			group = solids[i:i + lengthOfGroup]
			result = TryToJoin(group)
			joinedSolids.extend(result)
		solids = joinedSolids

		if len(solids) < lengthOfGroup:
			solids = TryToJoin(solids)
			break

	return solids


def GroupAndSortCurves(curves):

	groups = []
	k = 0.1/304.8

	while len(curves) !=0:

		groups.append([curves.pop(0)])
		oldLen = 0

		while oldLen != len(curves):

			oldLen = len(curves)

			i = 0
			while i<len(curves):

				pg0 = groups[-1][0].GetEndPoint(0)
				pg1 = groups[-1][-1].GetEndPoint(1)

				p0 = curves[i].GetEndPoint(0)
				p1 = curves[i].GetEndPoint(1)

				if pg0.DistanceTo(p0) < k:
					groups[-1].insert(0,curves.pop(i).CreateReversed())

				elif pg1.DistanceTo(p0) < k:
					groups[-1].append(curves.pop(i))

				elif pg1.DistanceTo(p1) < k:
					groups[-1].append(curves.pop(i).CreateReversed())

				elif pg0.DistanceTo(p1) < k:
					groups[-1].insert(0,curves.pop(i))

				else:
					i+=1


	return groups



def ExtrudeTopography(topography, deep):#, limitingSolid):

	opt = Options()
	geometryList  = topography.get_Geometry(opt)

	mesh = None
	for g in geometryList:
		if g.GetType() == Mesh:
			mesh = g
			break
	if mesh is None:
		return None

	triangles = mesh.NumTriangles
	solidOpt = SolidCurveIntersectionOptions()

	solids,xCoordinates = [],[]

	for i in range(mesh.NumTriangles):

		triangle = mesh.get_Triangle(i)
		points = [triangle.Vertex[0],triangle.Vertex[1],triangle.Vertex[2]]

		try:
			lines = CList[Curve]()
			for j in range(len(points)):
				lines.Add(Line.CreateBound(points[j-1],points[j]))
		except:
			continue

		loop = CurveLoop.Create(lines)
		loopList = CList[CurveLoop]([loop])
		try:
			solid = GeometryCreationUtilities.CreateExtrusionGeometry(loopList,XYZ(0,0,-1),deep)
		except:
			continue

		solids.append(solid)
		xCoordinates.append(sum([p.X for p in points])/3)


	sorting = DS.List.SortByKey(solids,xCoordinates)
	sortedSolids = list(sorting["sorted list"])
	return sortedSolids


def GetOutCurvesByByTopography(topography):

	opt = Options()
	geometryList  = topography.get_Geometry(opt)

	mesh = None
	for g in geometryList:
		if g.GetType() == Mesh:
			mesh = g
			break
	if mesh is None:
		return None

	triangles = mesh.NumTriangles
	solidOpt = SolidCurveIntersectionOptions()

	lines = []
	for i in range(mesh.NumTriangles):

		triangle = mesh.get_Triangle(i)
		points = [triangle.Vertex[0],triangle.Vertex[1],triangle.Vertex[2]]

		for j in range(len(points)):

			try:
				line = Line.CreateBound(points[j-1],points[j])
			except:
				continue

			lines.append(line)

	# Delete lines that have adjacent lines

	i = 0
	while i<len(lines)-1:
		j = i+1
		lonely = True
		p1 = lines[i].Evaluate(0.5,True)
		while j<len(lines):
			p2 = lines[j].Evaluate(0.5,True)
			if p1.DistanceTo(p2)<1/304.8:
				lonely = False
				del lines[j]
				break
			else:
				j+=1
		if lonely:
			i+=1
		else:
			del lines[i]

	return lines



def __maincode__():

	settings = IN[0]

	if len(settings.Topographies)== 0:
		return "You did not select any topography or cancel the operation"


	opt = Options()
	topographies = settings.Topographies
	borderOffset = settings.BorderOffset
	minimumDeep = settings.MinimumDeep

	# Step1. Determine the boundary values of heights

	boxesMinZ,boxesMaxZ = [],[]
	for topography in topographies:
		box = topography.get_BoundingBox(None)
		boxesMaxZ.append(box.Max.Z)
		boxesMinZ.append(box.Min.Z)

	globalMinZ, globalMaxZ = min(boxesMinZ),max(boxesMaxZ)
	globalDiff = globalMaxZ - globalMinZ

	# Step2. Press out the outer contour of the relief to get a prism. Then intersect all the prisms with each other to get an intersection prism

	xx = []
	prismSolids = []
	for topography in topographies:

		unsortedCurves = GetOutCurvesByByTopography(topography) #Obtaining outer topography curves
		sortedCurves = GroupAndSortCurves(unsortedCurves) #Sorting of outer curves with the formation of closed contours
		outLoop = GetExternalAndInternalLoops(sortedCurves)[0] #Get only the outer contour#
		outPoints = [l.GetEndPoint(0) for l in outLoop]
		outNullPoints = [XYZ(p.X,p.Y,globalMaxZ) for p in outPoints]

		#return Visualize(outNullPoints)

		outNullLines = []
		for j in range(len(outNullPoints)):
			outNullLines.append(Line.CreateBound(outNullPoints[j-1],outNullPoints[j]))

		#return Visualize(outNullLines)

		loopList = CList[CurveLoop]([CurveLoop.Create(CList[Curve](outNullLines))])
		prismSolid = GeometryCreationUtilities.CreateExtrusionGeometry(loopList,XYZ(0,0,-1),globalDiff+minimumDeep)
		prismSolids.append(prismSolid)

	finishPrismSolid = prismSolids.pop(0)
	x = 0
	for solid in prismSolids:
		res = BooleanOperationWithTolerance(finishPrismSolid,solid,BooleanOperationsType.Intersect,10,0,0,1/304.8)
		if res is not None:
			finishPrismSolid = res
			x+=1

	#return Visualize(finishPrismSolid)
	lines = list(GetHorizontalLoopsBySolid(finishPrismSolid,True)[0])
	lines = RebuildLoop(lines,5,1) #Curve optimization reduces the chance of offset errors
	loop = CurveLoop.Create(CList[Curve](lines))

# Step3 Find only those points that lie within the general contour. They will determine the order of the layers

	sciOpt = SolidCurveIntersectionOptions()
	topographyMaxZList = []
	for topography in topographies:

		points = topography.GetPoints()
		zList = []

		for point in points:

			line = Line.CreateBound(point,point.Add(XYZ(1,1,1)))
			intRes = finishPrismSolid.IntersectWithCurve(line,sciOpt)
			if len(list(intRes))!=0:
				zList.append(point.Z)

		topographyMaxZList.append(max(zList))

	sortedData = list(zip(topographyMaxZList,topographies))
	sortedData.sort(key = lambda x:-x[0])
	topographies = [x[1] for x in sortedData]

# Step 4 But each topography layer must have its own limiting prism, and the higher the layer, the narrower its prism will be. This is done so that subtracting the layers does not result in thin plates at the edges of the solids

	offsetedFinishPrismSolids = []
	stepx = borderOffset/len(topographies)
	offsetsX = [borderOffset -(stepx*i) for i in range(len(topographies))]
	stepZ = float(1)/len(topographies)
	offsetsZ = [(stepZ*i) for i in range(len(topographies))]

	for i,solid in enumerate(topographies):

		offsetLoop = CurveLoop.CreateViaOffset(loop,-offsetsX[i],XYZ(0,0,1))
		loopList = CList[CurveLoop]([offsetLoop])
		offsetedFinishPrismSolid = GeometryCreationUtilities.CreateExtrusionGeometry(loopList,XYZ(0,0,-1),globalDiff+minimumDeep+offsetsZ[i])
		offsetedFinishPrismSolids.append(offsetedFinishPrismSolid)

	#return Visualize(offsetedFinishPrismSolids)

	# Step 5. Extrude the contours of the topographies and try to unite everything into a single solid.

	extrTopoList = []
	for i,topography in enumerate(topographies):

		trianglePrisms = []
		topoSolids = ExtrudeTopography(topography, globalDiff + minimumDeep + 5)
		for solid in topoSolids:
			res = BooleanOperationWithTolerance(offsetedFinishPrismSolids[i],solid,BooleanOperationsType.Intersect,20,0,0,5/304.8)
			if res is not None:
				trianglePrisms.append(res)

		unitedTrianglePrisms = TopographySolidUnion(trianglePrisms,settings.NumberOfAttemptsToJoin, settings.LengthOfGroupToJoin)
		extrTopoList.append(unitedTrianglePrisms)

	#return Visualize(extrTopoList)

	# Step 6. Subtract from the layer all the layers that lie below.

	for i in range(len(extrTopoList)-1):
		for j in range(len(extrTopoList[i])):
			for ii in range(i+1,len(extrTopoList)):
				for jj in  range(len(extrTopoList[ii])):
					res = BooleanOperationWithTolerance(extrTopoList[i][j],extrTopoList[ii][jj],BooleanOperationsType.Difference,10,0,0,10/304.8)
					if res is not None:
						extrTopoList[i][j] = res

	#return Visualize(extrTopoList)

	#Step 7. Creating FamilyInstances

	elements = []
	for i in range(len(extrTopoList)):

		name = time.strftime("%Y_%b_%d_%H_%M_%S_")+str(i)
		path = settings.FamilyTemplatePath
		color = Color(random.random()*255,random.random()*255,random.random()*255)
		element = BakeGeometryAsFamilyInstance(extrTopoList[i],name,path,color)
		elements.append(element)

	return elements


timeStart = time.time()
TransactionManager.Instance.EnsureInTransaction(doc)
report = __maincode__()
TransactionManager.Instance.TransactionTaskDone()
sumTime = time.time() - timeStart
OUT = "Time: " + str(sumTime),report

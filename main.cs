 System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Attributes;

namespace DATools
{


    public class Main : IExternalApplication
    {


        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "AXM MNSR";

            string panelName = "Annotation";
            string panel2Name = " Views";
            string panel3Name = " Sheets";
            string panel4name = "Navisworks";
            string panel5Name = "Selección";


            application.CreateRibbonTab(tabName);

            RibbonPanel rpanel = application.CreateRibbonPanel(tabName, panelName);
            RibbonPanel rpanel2 = application.CreateRibbonPanel(tabName, panel2Name);
            RibbonPanel rpanel3 = application.CreateRibbonPanel(tabName, panel3Name);
            RibbonPanel rpanel4 = application.CreateRibbonPanel(tabName, panel4name);
            RibbonPanel rpanel5 = application.CreateRibbonPanel(tabName, panel5Name);

            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyPath = assembly.Location;


            PushButton tagWallLayerbtn = rpanel.AddItem(new PushButtonData("TagWallLayers", "Tag wall Layers", assemblyPath, "DATools.Commands.TagWallLayersCommand")) as PushButton;
            tagWallLayerbtn.ToolTip = "TAG ALL THE LAYERS INSIDE A WALL WITHIN THE INFO INSIDE IT";
            tagWallLayerbtn.LargeImage = GetResourceImage(assembly, "DATools.Resources.show32.png");
            tagWallLayerbtn.Image = GetResourceImage(assembly, "DATools.Resources.show16.png");


            PushButton OpenAciteViewportView = rpanel2.AddItem(new PushButtonData("ActivarViewport", "Open Viewport View", assemblyPath, "DATools.Commands.OpenViewViewportCommand")) as PushButton;
            OpenAciteViewportView.ToolTip = "Click in the Desire Viewport an activate/Open the View inside it";
            OpenAciteViewportView.LargeImage = GetResourceImage(assembly, "DATools.Resources.VIEW32.png");
            OpenAciteViewportView.Image = GetResourceImage(assembly, "DATools.Resources.VIEW16.png");

            PushButton DuplicateDetailActiveView = rpanel2.AddItem(new PushButtonData("Duplicar Vista Activa", "Duplicar Vista Act", assemblyPath, "DATools.Commands.DuplicateDetailingActiveView")) as PushButton;
            DuplicateDetailActiveView.ToolTip = "DUPLIQUE LA VISTA ACTUAL CON DETALLE";
            DuplicateDetailActiveView.LargeImage = GetResourceImage(assembly, "DATools.Resources.SHEET32.png");
            DuplicateDetailActiveView.Image = GetResourceImage(assembly, "DATools.Resources.SHEET16.png");

            PushButton PlaceViewOnSheet = rpanel2.AddItem(new PushButtonData("PlaceViewOnSheet", "Colocar Vista Plano", assemblyPath, "DATools.Commands.PlaceViewOnSheet")) as PushButton;
            PlaceViewOnSheet.ToolTip = "COLOQUE LA VISTA ACTUAL EN EL PLANO DESEADO";
            PlaceViewOnSheet.LargeImage = GetResourceImage(assembly, "DATools.Resources.LAYOUT32.png");
            PlaceViewOnSheet.Image = GetResourceImage(assembly, "DATools.Resources.LAYOUT16.png");

            PushButton DuplicateSelectedViews = rpanel2.AddItem(new PushButtonData("DuplicateSelectedViews", "DuplicateSelectedViews", assemblyPath, "DATools.Commands.DuplicateSelectedViews")) as PushButton;
            DuplicateSelectedViews.ToolTip = "DUPLICA A DETALLE LAS VISTAS SELECCIONADAS EN EL PROJECT BROWSER";
            DuplicateSelectedViews.LargeImage = GetResourceImage(assembly, "DATools.Resources.Duplicate32.png");
            DuplicateSelectedViews.Image = GetResourceImage(assembly, "DATools.Resources.Duplicate16.png");



            PushButton CustomPrint = rpanel3.AddItem(new PushButtonData("PrintSelectedSheets", "CustomPrint ", assemblyPath, "DATools.Commands.PrintPDFCommand")) as PushButton;
            CustomPrint.ToolTip = "EXPORTA A PDF LOS SHEETS SELECCIONADOS PREVIAMENTE EN EL PROJECT BROWSER";
            CustomPrint.LargeImage = GetResourceImage(assembly, "DATools.Resources.plano32.png");
            CustomPrint.Image = GetResourceImage(assembly, "DATools.Resources.plano16.png");

            PushButton RenamePDF = rpanel3.AddItem(new PushButtonData("RenamePDF", "RenombrarPDF ", assemblyPath, "DATools.Commands.RenamePDFCommand")) as PushButton;
            RenamePDF.ToolTip = "RENOMBRA LOS ARCHIVOS PDF DADO EL DIRECTORIO DIRECTO ABSOLUTO";
            RenamePDF.LargeImage = GetResourceImage(assembly, "DATools.Resources.pdf32.png");
            RenamePDF.Image = GetResourceImage(assembly, "DATools.Resources.pdf16.png");


            PushButton TypeSelector = rpanel5.AddItem(new PushButtonData("Selector PRO AXM", "AXM PRO Selector", assemblyPath, "DATools.Commands.SelectByTypeCommand")) as PushButton;
            TypeSelector.ToolTip = "GENERE CONJUNTOS DE SELECCIÓN Y FILTROS POR TIPO O CATEGORIA Y TIPO DE ELEMENTO YA SEA MODELO, 2D O SOLO DATUM CON ESTA HERRAMIENTA";
            TypeSelector.LargeImage = GetResourceImage(assembly, "DATools.Resources.seleccion32.png");
            TypeSelector.Image = GetResourceImage(assembly, "DATools.Resources.seleccion16.png");


            ///////
            ///NWCWEx


            PushButtonData all = new PushButtonData("AXM2NWC", "ALL MODELS", assemblyPath, "DATools.Commands.NavisWorksExporterCommand");
            all.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            all.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            all.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");


            PushButtonData d2 = new PushButtonData("AXM5_6", "5-6 MODELS", assemblyPath, "DATools.Commands._5_6_NWC");
            d2.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            d2.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            d2.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");

           

            PushButtonData d3 = new PushButtonData("AXM7_8", "7-8 MODELS", assemblyPath, "DATools.Commands._7_8_NWC");
            d3.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            d3.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            d3.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");

            

            PushButtonData d4 = new PushButtonData("AXM3_4", "3_4 MODELS", assemblyPath, "DATools.Commands._3_4_NWC");
            d4.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            d4.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            d4.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");


            

            PushButtonData d5 = new PushButtonData("AXMBLD1", "BLD_1MODEL", assemblyPath, "DATools.Commands.BLD1_NWC");
            d5.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            d5.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            d5.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");


            

            PushButtonData d6 = new PushButtonData("AXMBLD2", "BLD_2MODEL", assemblyPath, "DATools.Commands.BLD1_NWC");
            d6.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            d6.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            d6.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");


            

            PushButtonData d7 = new PushButtonData("AXMBLD9", "BLD_9MODEL", assemblyPath, "DATools.Commands.BLD9");
            d7.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            d7.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            d7.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");

            

            PushButtonData d8 = new PushButtonData("AXMBLDLS", "BLD_LSMODEL", assemblyPath, "DATools.Commands.BLDLS");
            d8.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            d8.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            d8.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");


            PushButtonData d9 = new PushButtonData("AXMBLDRD", "BLD_RDMODEL", assemblyPath, "DATools.Commands.BLDRD");
            d9.ToolTip = "EXPORTA LAS VISTAS 3D PREVIAMENTE CONFIGURADAS DE LAS ESPECIALIDADES MEP-EST-ARQ A NWC EN LA RUTA SELECCIONADA";
            d9.LargeImage = GetResourceImage(assembly, "DATools.Resources.navislogo32.png");
            d9.Image = GetResourceImage(assembly, "DATools.Resources.navislogo16.png");



            SplitButtonData sb1 = new SplitButtonData("AxmNWCExporter", "NWCbyMODEL");

            SplitButton sb = rpanel4.AddItem(sb1) as SplitButton;
            sb.AddPushButton(all);
            sb.AddPushButton(d2);
            sb.AddPushButton(d3);
            sb.AddPushButton(d4);
            sb.AddPushButton(d5);
            sb.AddPushButton(d6);
            sb.AddPushButton(d7);
            sb.AddPushButton(d8);
            sb.AddPushButton(d9);


            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public ImageSource GetResourceImage(Assembly assembly,string imageName)
        {
            try
            {
                Stream resource = assembly.GetManifestResourceStream(imageName);
                return BitmapFrame.Create(resource);

            }
            catch
            {
                return null;
            }
        }
        

        
    }
}

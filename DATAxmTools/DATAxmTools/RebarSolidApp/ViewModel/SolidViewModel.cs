using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATAxmTools.RebarSolidApp.ViewModelBase;
using RebarSolidApp.View;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;

namespace DATAxmTools.RebarSolidApp.ViewModel
{
    public class SolidViewModel : ViewModelBase.ViewModelBase
    {

        
        private Document Doc { get; }
        private UIDocument UIDoc { get; }
        private SolidView solidView;
        public SolidView Solidview

        {
            get {
                if(solidView == null)
                {
                    solidView = new SolidView() { DataContext = this };
                }
                return solidView;}
            set
            {
                solidView = value;
                OnPropertyChange(nameof(Solidview));
            }
        }

        private bool isCheckedSolid;

        public bool IsCheckedSolid
        {
            get { return isCheckedSolid; }

            set
            {
                isCheckedSolid = value;
                OnPropertyChange(nameof(IsCheckedSolid));
            }
        }

        private bool isCheckedUnobscured;

        private bool IsCheckedUnobscured
        {
            get { return isCheckedUnobscured; }

            set
            {
                isCheckedUnobscured = value;
                OnPropertyChange(nameof(IsCheckedUnobscured));
            }
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }

            set
            {
                selectedIndex = value;
                OnPropertyChange(nameof(SelectedIndex));
            }
        }
        
        public RelayCommand<object> ButtonRun { get; set; }

        public SolidViewModel(UIDocument uidoc)
        {
            UIDoc = uidoc;
            Doc = uidoc.Document;
            
            ButtonRun = new RelayCommand<object>(p => true, p => ButtonRunAction());
            //SetUnobscuredInView
        }


        private void ButtonRunAction()
        {
            this.Solidview.Close();

            

            try
            {

            
                Autodesk.Revit.DB.View vistaActual = Doc.ActiveView;
                if (vistaActual.ViewType == ViewType.ThreeD)
                {
                    //COLLECCIONAR TODOS LOS REBAR ELEMENTS EN LA VISTA ACTUAL

                    View3D vista3D = vistaActual as View3D;


                    IEnumerable<Rebar> rebars = null;

                    if(SelectedIndex == 0)
                    {
                        rebars = new FilteredElementCollector(Doc).OfClass(typeof(Rebar)).Cast<Rebar>();
                    }
                    else
                    {
                        try
                        {
                            rebars = UIDoc.Selection.PickObjects(ObjectType.Element, new RebarFilter(), "Seleccione los elementos Rebar").Select(x => Doc.GetElement(x)).Cast<Rebar>();
                        }
                        catch
                        { }



                    }
                    if (rebars == null) return;


                    using (Transaction trans = new Transaction(Doc, "Solid Rebar"))
                    {
                        trans.Start();

                        foreach (Rebar rb in rebars)
                        {
                            rb.SetSolidInView(vista3D, IsCheckedSolid);
                            rb.SetUnobscuredInView(vista3D, IsCheckedUnobscured);
                        }

                        trans.Commit();
                    }
                }
                else
                {
                    TaskDialog.Show("SOLID REBAR", "LA VISTA UNICAMENTE PUEDE SER UN THREEDEE VIEW");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
    }

    public class RebarFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return (elem.Category != null && elem is Rebar);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}

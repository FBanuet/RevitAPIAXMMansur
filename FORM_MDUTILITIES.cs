/*
 * Created by SharpDevelop.
 * User: S-MK
 * Date: 04/01/2021
 * Time: 02:37 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using Autodesk.Revit.DB;


namespace BMHUB
{
	/// <summary>
	/// Description of frmPARAMETERS.
	/// </summary>
	public partial class frmPARAMETERS : System.Windows.Forms.Form
	{
		public frmPARAMETERS(Element elem)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			string nombreElemento = elem.Name;
			
			this.txtElemento.Text = nombreElemento;
			
			string texto = "";
			
			foreach(Parameter param in elem.Parameters)
			{
				texto += param.Definition.Name + " = " + param.AsValueString() + Environment.NewLine;
			}
			
			
			this.rtbParametros.Text = texto;
		}
	}
}

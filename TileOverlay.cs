/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 28/11/2009
 * Time: 1:41 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ET2Solver
{
	/// <summary>
	/// Description of UserControl1.
	/// </summary>
	public partial class TileOverlay : Label
	{
		public TileOverlay()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			
		}

		/*
		override protected CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x20;
				return cp;
			}
		}
		*/
	
	}
}

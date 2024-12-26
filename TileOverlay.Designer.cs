/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 28/11/2009
 * Time: 1:41 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Drawing;

namespace ET2Solver
{
	partial class TileOverlay
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.Name = "TileOverlay";
		}
		
		/*
		override protected void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			System.Drawing.Graphics g = this.CreateGraphics();
			g.Clear(this.BackColor);
			System.Drawing.Rectangle r = new System.Drawing.Rectangle(0, 0, this.Width, this.Height);
			g.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Aqua)), r);
			g.Dispose();
		}
		*/

		/*
		override protected void OnMove(System.EventArgs e)
		{
			RecreateHandle();
		}
		*/
	}
}

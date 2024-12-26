/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 8/11/2009
 * Time: 10:34 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using CAF;

namespace ET2Solver
{
	internal sealed class Program
	{
		public static MainForm TheMainForm = null;

		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new MainForm());

			// initialise CAF
			CAF_Application.init();
			TheMainForm = new MainForm();
			CAF_Application.stats.reset();
			Application.Run(TheMainForm);
			
			// 4-Nov-2010 cannot successfully use TheMainForm.applyConfiguration() on startup! :(
		}
		
	}
}

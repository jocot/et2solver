/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 14/06/2010
 * Time: 4:53 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace ET2Solver
{
	/// <summary>
	/// A class to log and visualise application function flowchart / execution logic
	/// </summary>
	public class AppFlowLog
	{
		public int max_log_size = 1000;

		private int indent = 0;
		private List<string> log = new List<string>();

		public AppFlowLog()
		{
		}

		public void open(string text)
		{
			this.comment(text);
			this.indent++;
		}

		public void close(string text)
		{
			if ( this.indent > 0 )
			{
				this.indent--;
			}
			this.comment(text);
		}

		public void comment(string text)
		{
			if ( this.max_log_size > this.log.Count )
			{
				text = String.Format("{0}{1}", new String('.', this.indent*2), text);
				this.log.Add(text);
			}
		}

		public string getLog()
		{
			return String.Join("\r\n", this.log.ToArray());
		}
	}
}

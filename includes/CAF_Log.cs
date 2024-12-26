/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 23/10/2010
 * Time: 2:56 AM
 * 
 * Application Logging module
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CAF
{
	[Flags]
	public enum LogType
	{
		DEBUG = 0x01, 
		INFO = 0x02, 
		WARNING = 0x04, 
		ERROR = 0x08
	};

	public class CAF_Log_Entry
	{
		public LogType type = 0;
		public string text = "";
		public DateTime timestamp;
	}
	
	public class CAF_Log
	{
		private List<CAF_Log_Entry> log = new List<CAF_Log_Entry>();
		
		// textbox logging options
		private TextBox textControl;
		private LogType textControlFlags;
		private bool textControlTimestamp;
		
		public CAF_Log()
		{
		}
		
		public void add(LogType type, string text)
		{
			CAF_Log_Entry log = new CAF_Log_Entry();
			log.type = type;
			log.timestamp = DateTime.Now;
			log.text = text;
			this.log.Add(log);
			
			if ( this.textControl != null && ( this.textControlFlags & log.type ) != 0 )
			{
				string timestamp;
				if ( this.textControlTimestamp )
				{
					timestamp = log.timestamp.ToString("dd-MM-yyyy hh:mm:ss tt : ");
				}
				else
				{
					timestamp = "";
				}
				string output = String.Format("{0}{1} : {2}\r\n", timestamp, type, text);
				this.textControl.AppendText(output);
				this.textControl.Update();
			}
		}
		
		public string getAsText(bool includeTimestamps)
		{
			return this.getAsText(includeTimestamps, LogType.DEBUG | LogType.ERROR | LogType.INFO | LogType.WARNING);
		}
		
		public string getAsText(bool includeTimestamps, LogType logTypes)
		{
			List<string> output = new List<string>();
			string timestamp;
			foreach ( CAF_Log_Entry logrec in this.log )
			{
				if ( includeTimestamps )
				{
					timestamp = logrec.timestamp.ToString("dd-MM-yyyy hh:mm:ss tt : ");
				}
				else
				{
					timestamp = "";
				}
				output.Add(String.Format("{0}{1} : {2}", timestamp, logrec.type, logrec.text));
			}
			return String.Join("\r\n", output.ToArray());
		}
		
		public void setTextControl(TextBox textObject, bool includeTimestamps, LogType logTypes)
		{
			this.textControl = textObject;
			this.textControlTimestamp = includeTimestamps;
			this.textControlFlags = logTypes;
		}
		
	}
}

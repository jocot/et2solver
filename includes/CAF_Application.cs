/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 23/10/2010
 * Time: 2:52 AM
 * 
 * Custom Application Framework
 * 
 * Provides:
 * application level configuration load/save & runtime read/write access
 * application statistics module
 * application logging
 * 
 */
using System;

namespace CAF
{
	public static class CAF_Application
	{
		public static CAF_Config config = null;
		public static CAF_Log log = null;
		public static CAF_Stats stats = null;
		
		public static void init()
		{
			CAF_Application.config = new CAF_Config();
			CAF_Application.log = new CAF_Log();
			CAF_Application.stats = new CAF_Stats();
		}

	}
}

/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 4/12/2009
 * Time: 11:50 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace ET2Solver
{
	/// <summary>
	/// Description of TileInfo.
	/// </summary>
	public class TileInfo
	{
		public int id = 0;
		public string pattern = "";
		public string[] patterns = new string[4];
		public string title = "";
		public string left = "";
		public string up = "";
		public string right = "";
		public string down = "";
		
		public TileInfo(int id, string pattern)
		{
			this.id = id;
			this.pattern = pattern;
			this.title = id.ToString() + "_" + pattern;
			this.left = pattern.Substring(0, 1);
			this.up = pattern.Substring(1, 1);
			this.right = pattern.Substring(2, 1);
			this.down = pattern.Substring(3, 1);
			this.generatePatterns(pattern);
		}
		
		public void generatePatterns(string startingPattern)
		{
			//Program.TheMainForm.log("generatePatterns(" + startingPattern + ")");
	        // creates all pattern rotations, in clockwise sequence
	        // LTRB -> BLTR -> RBLT -> TRBL
	        this.patterns = new string[4];
	        string pattern = startingPattern;
	        this.patterns[0] = pattern;
	        for ( int i = 2; i <= 4; i++ )
	        {
	        	pattern = pattern.Substring(pattern.Length-1) + pattern.Substring(0, 3);
	        	this.patterns[i-1] = pattern;
	        	//Program.TheMainForm.log("generate pattern " + i + ": " + pattern);
	        }

		}
		
	}
	
}

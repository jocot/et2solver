/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 14/12/2009
 * Time: 2:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET2Solver
{
	/// <summary>
	/// Description of PatternStats.
	/// </summary>
	public class PatternStats
	{
		// orientation stats - LURD
		// pattern, numLeft, numTop, numRight, numDown, totalEdges, totalTiles, numUniqueTiles
		public List<string> orientation_stats = new List<string>();

		// list of tiles that include each pattern
		public Dictionary<string, List<string>> pattern_tiles = new Dictionary<string, List<string>>();

		// list of tile patterns
		public List<string> patterns = new List<string>();
		
		public string patternIncludeFilter = "";
		
		public PatternStats()
		{
		}
		
		public void clear()
		{
			this.patterns = new List<string>();
			this.pattern_tiles = new Dictionary<string, List<string>>();
			this.orientation_stats = new List<string>();
		}
		
		public void calcOrientationStats()
		{
			this.orientation_stats.Add("pattern, numLeft, numTop, numRight, numDown, totalEdges, totalTiles, numUniqueTiles");

			for ( int pid = 1; pid <= 22; pid++ )
			{
				string letter = Char.ConvertFromUtf32(64 + pid).ToUpper().ToString();
				
				// only extract specified patterns
				if ( this.patternIncludeFilter != "" )
				{
					if ( !Regex.Match(letter, this.patternIncludeFilter).Success )
					{
						continue;
					}
				}
				
				// initialise unique tile list
				this.pattern_tiles.Add(letter, new List<string>());

				// numLeft, numTop, numRight, numDown, totalEdges, totalTiles, numUniqueTiles
				int[] stats = {0,0,0,0,0,0,0};
				foreach ( string pattern in this.patterns )
				{
					bool matched = false;
					for ( int p = 0; p < 4; p++ )
					{
						if ( pattern[p].ToString().ToUpper() == letter )
						{
							matched = true;
							stats[p] += 1;
						}
					}
					if ( matched )
					{
						// totalTiles
						stats[5]++;
						
						// numUniqueTiles
						if ( this.addAllRotations(letter, pattern.Trim()) )
						{
							stats[6]++;
						}
						else
						{
//							Program.TheMainForm.loadsavelog(pattern.Trim() + " not unique in list " + letter);
						}
					}
				}
				// totalEdges
				stats[4] = stats[0] + stats[1] + stats[2] + stats[3];
				
				// add as csv
				this.orientation_stats.Add(letter + "," + String.Join(",", new List<int>(stats).ConvertAll<string>(delegate(int i){ return i.ToString(); }).ToArray()));
			}
		}
		
		public string getStatsCSV()
		{
			this.calcOrientationStats();
			return String.Join("\r\n", this.orientation_stats.ToArray());
		}
		
		// compare tile patterns, rotates to see if they are the same
		public bool equalTile(string tileA, string tileB)
		{
			string test = tileB.Clone().ToString();
	        for ( int r = 1; r <= 4; r++ )
	        {
	        	if ( r > 1 )
	        	{
		        	test = test.Substring(test.Length-1) + test.Substring(0, 3);
	        	}
				if ( test == tileA )
				{
					return true;
				}
	        }
	        return false;
		}
		
		// return true if all patterns added without duplicates
		public bool addAllRotations(string letter, string tile)
		{
			bool rv = true;
			string pattern = tile.Clone().ToString();
			int numAdded = 0;
	        for ( int r = 1; r <= 4; r++ )
	        {
	        	if ( r > 1 )
	        	{
		        	pattern = pattern.Substring(pattern.Length-1) + pattern.Substring(0, 3);
	        	}
	        	if ( !this.pattern_tiles[letter].Contains(pattern) )
	        	{
		        	this.pattern_tiles[letter].Add(pattern);
		        	numAdded++;
	        	}
	        }
	        
//	        Program.TheMainForm.loadsavelog("added " + numAdded + " rotations for " + letter + " of " + tile);
			rv = numAdded == 4;
	        return rv;
		}
		
	}
}

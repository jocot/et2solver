/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 3/12/2009
 * Time: 6:32 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace ET2Solver
{
	/// <summary>
	/// Description of TileSearch.
	/// </summary>
	public class TileSearch
	{
		// string[] = {tileId, pattern}
		public ArrayList tileset = new ArrayList();
		public System.Collections.Generic.List<string> tileset_list = new System.Collections.Generic.List<string>();
		
		// [pos] = ArrayList(string[tileId, pattern])
		public ArrayList searchOptions = new ArrayList();
		public Hashtable results = new Hashtable();
		public Hashtable includeFilters = new Hashtable();
		public Hashtable excludeFilters = new Hashtable();
		
		// if overwrite = true, then will prompt before overwriting
		// if overwrite = false, then will append to existing files
		public bool save_overwrite = false;
		public int save_pos = 0;
		public string save_filename = "";
		public string[] tiles;
		public System.Collections.Generic.List<int> tileIds = new System.Collections.Generic.List<int>();
		
		public TileSearch()
		{
		}
		
		public void newSearch()
		{
			this.searchOptions = new ArrayList();
			this.results = new Hashtable();
			this.includeFilters = new Hashtable();
			this.excludeFilters = new Hashtable();
		}

		public void loadTileset(string id)
		{
			string sourcefile = "tilesets\\" + id + ".txt";
			string[] lines;
			try
			{
				lines = System.IO.File.ReadAllLines(sourcefile);
			}
			catch
			{
				return;
			}
			
			this.tileset = new ArrayList();
			for ( int i = 0; i < lines.Length; i++ )
			{
				string pattern = lines[i].Trim();
				if ( pattern.Length == 4 )
				{
					this.tileset.Add(new TileInfo(i+1, pattern));
					this.tileset_list.Add(pattern);
					// add all tile rotations
			        for ( int r = 2; r <= 4; r++ )
			        {
			        	pattern = pattern.Substring(pattern.Length-1) + pattern.Substring(0, 3);
			        	this.tileset.Add(new TileInfo(i+1, pattern));
						this.tileset_list.Add(pattern);
			        }
				}
			}
			
		}
		
		public int addTileToList(int pos, TileInfo tile)
		{
			// add a tile to the result list in the specified position (if doesnt already exist)
			if ( !this.results.ContainsKey(pos) )
			{
				this.results.Add(pos, new ArrayList());
			}
			ArrayList poslist = (ArrayList)this.results[pos];
			
			// store tile in current export list if save list enabled
			if ( this.save_pos > 0 )
			{
				this.storeTile(pos, tile);
			}

			// abort adding if tile already exists
			foreach ( TileInfo stile in poslist )
			{
				if ( stile.title == tile.title )
				{
					return 0;
				}
			}
			poslist.Add(tile);

			return 1;
		}

		public int addTiles(params string[] matches)
		{
			// pos@searchPattern
			// 1@[F-V]{4}
			int numAdded = 0;
			int numSearched = 0;
			foreach ( string searchtile in matches )
			{
				string[] tileinfo = searchtile.Split('@');
				int pos = Convert.ToInt16(tileinfo[0]);
				string match = tileinfo[1];
				foreach ( TileInfo rtile in this.tileset )
				{
					numSearched++;
		            Regex reg = new Regex("^" + match, RegexOptions.IgnoreCase);
		            if ( reg.IsMatch(rtile.pattern) && this.isIncluded(pos, rtile.pattern) && !this.isExcluded(pos, rtile.pattern) )
		            {
		            	numAdded += this.addTileToList(pos, rtile);
		            }
				}
			}
			/*
			if ( numAdded > 0 )
			{
				string logtext = "addTiles(" + String.Join(",", matches) + ") added " + numAdded.ToString() + " / " + numSearched.ToString() + " matching tiles";
				Program.TheMainForm.loadsavelog(logtext);
			}
			*/
			return numAdded;
		}
		
		public void includeFilter(int pos, string pattern)
		{
			this.includeFilters.Add(pos, pattern);
		}
		
		public void excludeFilter(int pos, string pattern)
		{
			this.excludeFilters.Add(pos, pattern);
		}
		
		// check if tile matches inclusion filter
		// only return false if filter exists and it does not match
		public bool isIncluded(int pos, string pattern)
		{
			if ( this.includeFilters != null && this.includeFilters.ContainsKey(pos) )
			{
				string filter = (string)this.includeFilters[pos];
	            Regex reg = new Regex(filter, RegexOptions.IgnoreCase);
	            if ( reg.IsMatch(pattern) )
	            {
	            	return true;
	            }
	            else
	            {
	            	return false;
	            }
			}
			else
			{
				return true;
			}
		}

		// check if tile matches exclusion filter
		// only return true if filter exists and it matches
		public bool isExcluded(int pos, string pattern)
		{
			if ( this.excludeFilters != null && this.excludeFilters.ContainsKey(pos) )
			{
				string filter = (string)this.excludeFilters[pos];
	            Regex reg = new Regex(filter, RegexOptions.IgnoreCase);
	            if ( reg.IsMatch(pattern) )
	            {
	            	return true;
	            }
			}
			return false;
		}
		
		public ArrayList findTiles(string search)
		{
			ArrayList rv = new ArrayList();
			foreach ( TileInfo rtile in this.tileset )
			{
	            Regex reg = new Regex("^" + search, RegexOptions.IgnoreCase);
	            if ( reg.IsMatch(rtile.pattern) )
	            {
	            	rv.Add(rtile);
	            }
			}
			return rv;
		}
		
		// findAdjacent(3, "1@L,2@U");
		public int findAdjacent(int pos, string match)
		{
			int numFound = 0;
			string[] matches = match.Split(',');
			// matches contains max of 2 patterns
			if ( matches.Length == 1 )
			{
				// check if source pos exists
				string[] tileinfo = matches[0].ToString().Split('@');
				string[] stile = matches[0].ToString().Split('@');
				int spos = Convert.ToInt16(stile[0]);
				string smatch = stile[1];
				// check if source position exist in list
				if ( this.results.ContainsKey(spos) )
				{
					ArrayList tilelist = (ArrayList)this.results[spos];
					//Program.TheMainForm.loadsavelog("pos" + spos.ToString() + " contains " + tilelist.Count.ToString() + " tile(s)");
					
					// search the list
					foreach ( TileInfo tile in tilelist )
					{
						string search = this.getAdjacentSearchPattern(tile, smatch);
						numFound += this.addTiles(pos.ToString() + "@" + search);
					}
				}
				
			}
			else if ( matches.Length == 2 )
			{
				string[] stileA = matches[0].ToString().Split('@');
				int sposA = Convert.ToInt16(stileA[0]);
				string smatchA = stileA[1];

				string[] stileB = matches[1].ToString().Split('@');
				int sposB = Convert.ToInt16(stileB[0]);
				string smatchB = stileB[1];

				// check if source positions exist in list
				if ( this.results.ContainsKey(sposA) && this.results.ContainsKey(sposB) )
				{
					ArrayList tilelist = this.getPairedList(sposA, sposB);
					//Program.TheMainForm.loadsavelog("found " + tilelist.Count.ToString() + " unique pairs");
					
					// search the list
					foreach ( ArrayList tilepair in tilelist )
					{
						TileInfo tileA = (TileInfo)tilepair[0];
						TileInfo tileB = (TileInfo)tilepair[1];
						string search = this.getAdjacentPairSearchPattern(tileA, tileB, smatchA, smatchB);
						//Program.TheMainForm.loadsavelog("tileA: " + tileA.title + ", tileB: " + tileB.title + ", search pattern: " + search);

						// save tiles for list export
						if ( this.save_pos > 0 )
						{
							this.storeTile(sposA, tileA);
							this.storeTile(sposB, tileB);
						}
						numFound += this.addTiles(pos.ToString() + "@" + search);
					}
				}
			}
			Program.TheMainForm.loadsavelog("findAdjacent(" + pos.ToString() + ", " + match + ") = " + numFound.ToString());
			return numFound;
		}
		
		public string getAdjacentSearchPattern(TileInfo tile, string match)
		{
			string rv = "";
			string[] rmatch = new string[4];
			rmatch[0] = ".";
			rmatch[1] = ".";
			rmatch[2] = ".";
			rmatch[3] = ".";

            // tile match
            if ( match == "U" )
            {
                // adjacent match = down
                rmatch[3] = tile.pattern[1].ToString();
            }
    		else if ( match == "D" )
    		{
	            // adjacent match = up
	            rmatch[1] = tile.pattern[3].ToString();
    		}
    		else if ( match == "L" )
    		{
                // adjacent match = right
                rmatch[2] = tile.pattern[0].ToString();
    		}
			else if ( match == "R" )
			{
                // adjacent match = left
                rmatch[0] = tile.pattern[2].ToString();
			}

			rv = String.Join("", rmatch);
			return rv;
		}
		
		public string getAdjacentPairSearchPattern(TileInfo tileA, TileInfo tileB, string matchA, string matchB)
		{
			string rv = "";
			string[] rmatch = new string[4];
			rmatch[0] = ".";
			rmatch[1] = ".";
			rmatch[2] = ".";
			rmatch[3] = ".";

            // tileA match
            if ( matchA == "U" )
            {
                // adjacent match = down
                rmatch[3] = tileA.pattern[1].ToString();
            }
    		else if ( matchA == "D" )
    		{
	            // adjacent match = up
	            rmatch[1] = tileA.pattern[3].ToString();
    		}
    		else if ( matchA == "L" )
    		{
                // adjacent match = right
                rmatch[2] = tileA.pattern[0].ToString();
    		}
			else if ( matchA == "R" )
			{
                // adjacent match = left
                rmatch[0] = tileA.pattern[2].ToString();
			}

            // tileB match
            if ( matchB == "U" )
            {
                // adjacent match = down
                rmatch[3] = tileB.pattern[1].ToString();
            }
    		else if ( matchB == "D" )
    		{
	            // adjacent match = up
	            rmatch[1] = tileB.pattern[3].ToString();
    		}
    		else if ( matchB == "L" )
    		{
                // adjacent match = right
                rmatch[2] = tileB.pattern[0].ToString();
    		}
			else if ( matchB == "R" )
			{
                // adjacent match = left
                rmatch[0] = tileB.pattern[2].ToString();
			}
			rv = String.Join("", rmatch);
			return rv;
		}

		// return all unique combinations of tiles from 2 positions as a list of pairs
		public ArrayList getPairedList(int pos1, int pos2)
		{
			ArrayList rv = new ArrayList();
			ArrayList uniquePairs = new ArrayList();
			ArrayList pos1list = (ArrayList)this.results[pos1];
			ArrayList pos2list = (ArrayList)this.results[pos2];
			foreach ( TileInfo tileA in pos1list )
			{
				foreach ( TileInfo tileB in pos2list )
				{
					if ( !uniquePairs.Contains(tileA.title + tileB.title) )
					{
						ArrayList pair = new ArrayList();
						pair.Add(tileA);
						pair.Add(tileB);
						rv.Add(pair);
						uniquePairs.Add(tileA.title + tileB.title);
					}
				}
			}
			return rv;
		}
		
		public string getResultCount()
		{
			string rv = "";
			foreach ( int pos in this.results.Keys )
			{
				ArrayList poslist = (ArrayList)this.results[pos];
				int numTiles = poslist.Count;
				rv += "pos: " + pos.ToString() + ", numtiles: " + numTiles.ToString() + "\r\n";
			}
			return rv;
		}
		
		public string getPosListAsString(int pos)
		{
			string rv = "";
			if ( this.results.ContainsKey(pos) )
			{
				ArrayList poslist = (ArrayList)this.results[pos];
				foreach ( TileInfo tile in poslist )
				{
					rv += tile.title + "\r\n";
				}
			}
			return rv;
		}
		
		public int countUniqueTiles()
		{
			System.Collections.Generic.List<int> uniqueTiles = new System.Collections.Generic.List<int>();
			// count how many unique tiles in result set
			foreach ( int pos in this.results.Keys )
			{
				ArrayList poslist = (ArrayList)this.results[pos];
				foreach ( TileInfo tile in poslist )
				{
					if ( !uniqueTiles.Contains(tile.id) )
					{
						uniqueTiles.Add(tile.id);
					}
				}
			}
			return uniqueTiles.Count;
		}
		
		public void addSearch(string method, int pos, string match)
		{
			string[] searchCriteria = new string[3];
			searchCriteria[0] = method;
			searchCriteria[1] = pos.ToString();
			searchCriteria[2] = match;
			this.searchOptions.Add(searchCriteria);
		}
		
		public void doSearch()
		{
			foreach ( string[] searchCriteria in this.searchOptions )
			{
				string method = searchCriteria[0];
				int pos = Convert.ToInt16(searchCriteria[1]);
				string match = searchCriteria[2];
				int numFound = 0;
				switch ( method )
				{
					case "match":
						ArrayList tilelist = this.findTiles(match);
						foreach ( TileInfo tile in tilelist )
						{
							numFound += this.addTileToList(pos, tile);
						}
						break;
					case "match_adjacent":
						numFound = this.findAdjacent(pos, match);
						break;
				}
				if ( numFound == 0 )
				{
					// no tiles found, exit search
					break;
				}
			}
		}
		
		// define save list criteria - saves when pos is generated
		// first tile from other pos not referenced in addSearch(pos, match) will be saved as-is
		public bool saveListAt(int pos, string filename)
		{
			this.save_pos = pos;
			this.save_filename = "tilelists\\" + filename + ".txt";
			this.tiles = new string[pos];
			this.tileIds = new System.Collections.Generic.List<int>();
			
			// erase list if exists
			if ( System.IO.File.Exists(this.save_filename) )
			{
				if ( this.save_overwrite )
				{
					System.Windows.Forms.DialogResult confirm = System.Windows.Forms.MessageBox.Show("Overwrite " + this.save_filename + " ?", "Overwrite File ?", System.Windows.Forms.MessageBoxButtons.YesNo);
					if ( confirm.Equals(System.Windows.Forms.DialogResult.Yes) )
					{
						System.IO.File.Delete(this.save_filename);
					}
					else
					{
						return false;
					}
				}
			}
			return true;
		}
		
		// saves tile list to file (appends)
		// pattern, pattern, pattern, pattern
		public void saveList()
		{
			// abort if not a unique tile list
			System.Collections.Generic.List<int> uniqueTiles = new System.Collections.Generic.List<int>();
			foreach ( int tileId in this.tileIds )
			{
				if ( uniqueTiles.Contains(tileId) )
				{
//					this.tiles[0] = "DUPE";
					return;
				}
				else
				{
					uniqueTiles.Add(tileId);
				}
			}

			string line = String.Join(",", this.tiles);
			System.IO.File.AppendAllText(this.save_filename, line + "\r\n");
		}
		
		public void storeTile(int pos, TileInfo tile)
		{
			if ( this.tiles.Length >= pos )
			{
				this.tiles[pos-1] = tile.pattern;
			}
			if ( this.tileIds.Count >= pos )
			{
				this.tileIds[pos-1] = tile.id;
			}
			else
			{
				this.tileIds.Add(tile.id);
			}
			
			// save list if pos matches this.save_pos and list contains 4 unique tiles
			if ( pos == this.save_pos )
			{
				this.saveList();
			}
		}
	
		
	}
}

/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 23/10/2010
 * Time: 12:18 AM
 * 
 * Class to manage the tile queue & pre-sorted tile lists
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAF;

namespace ET2Solver
{
	public class TileQueue
	{
		// solver object
		public Solver3 solver = null;
		
		// pre-saved tile lists
		public Dictionary<string, List<string>> tileList = new Dictionary<string, List<string>>();

		// tile queue - cellId, list of tile patterns
		public Dictionary<int, List<string>> queueList = new Dictionary<int, List<string>>();
		public int[] queueProgress;

		/* TILE DATA */

		// primary tileset, contains single original rotation of all tiles
		// tile pattern 0,1,2,3 = L,T,R,B
		public SortedDictionary<int, string> tileSet = new SortedDictionary<int, string>();
		
		// pattern, tileCount - tile cache used by solver to determine used/available tiles
		// allows working with boards that have duplicate tiles
		//public SortedDictionary<string, int> tileCache = new SortedDictionary<string, int>();
		// disabled - was not correctly accounting for used tiles, as other rotations were not decreased accordingly
		
		// tileId, tileCount - tile cache used by solver to determine used/available tiles
		// allows working with boards that have duplicate tiles
		public SortedDictionary<int, int> tileCache = new SortedDictionary<int, int>();

		// tile cache, contains all rotations of all tiles
		// pattern, tileId (from tileSet)
		public SortedDictionary<string, List<int>> tileCacheIds = new SortedDictionary<string, List<int>>();
		
		// cellId, pattern
		public SortedDictionary<int, string> hintTiles = new SortedDictionary<int, string>();
		// list of hint tiles all rotations (used to check if hint tile has been used)
		public List<string> hintTileCache = new List<string>();

		public TileQueue()
		{
		}
		
		public TileQueue(Solver3 solver)
		{
			this.solver = solver;
		}
		
		public void init()
		{
			this.initQueues();
	
			// load pre-saved tile lists
			if ( !this.loadTileLists() )
			{
				// build pre-saved tile lists if they dont exist
				this.buildTileLists();
				this.loadTileLists();
			}
		}
		
		public void queueClear(int cellId)
		{
//			Program.TheMainForm.timer.start("queueClear");
			if ( cellId > 0 )
			{
				this.queueList[cellId] = new List<string>();
				if ( this.queueProgress.Length >= cellId )
				{
					this.queueProgress[cellId-1] = 0;
				}
			}
//			Program.TheMainForm.timer.stop("queueClear");
		}
		
		public void initQueues()
		{
			this.queueList.Clear();
			this.queueProgress = new int[this.solver.boardSize()];
			for ( int i = 1; i <= this.solver.boardSize(); i++ )
			{
				this.queueClear(i);
			}
			
			//this.queueData = new List<string>();
			//this.queueFilename = "";
			//this.modelData = "";
		}

		public bool loadTileLists()
		{
			// load pre-sorted tile list queues
			int numQueuesLoaded = 0;
			int numTiles = 0;
			this.tileList.Clear();

			string dirpath = "tilelists\\" + Board.title;
			string[] filenames = new string[0];
			try
			{
				filenames = System.IO.Directory.GetFiles(dirpath);
			}
			catch
			{
				// path doesn't exist
				return false;
			}
			foreach (string filename in filenames)
			{
				// exclude hint lists
				string basename = System.IO.Path.GetFileNameWithoutExtension(filename);
				if ( !Regex.IsMatch(basename, "^hints") )
				{
					this.tileList.Add(basename, new List<string>());
					string[] tiles = System.IO.File.ReadAllLines(filename);
					foreach ( string tile in tiles )
					{
						// exclude hint tiles
						if ( !this.hintTileCache.Contains(tile) )
					    {
							this.tileList[basename].Add(tile);
					    }
					}
					numTiles += this.tileList[basename].Count;
					if ( this.tileList[basename].Count > 0 )
					{
						numQueuesLoaded++;
					}
					//this.logt("loaded tile list " + filename + " with " + this.tileList[basename].Count + " tile(s)", 3);
				}
			}
			CAF_Application.log.add(LogType.INFO, "loaded " + numTiles + " tile(s) from " + numQueuesLoaded + " tile lists");
			return numQueuesLoaded > 0;
		}

		public void buildTileLists()
		{
			// generates pre-saved tilelists for all valid adjacent pattern sets
			
			// build corner lists
			// corner_tl ^--..
			this.buildTileListBySearch("corner_tl", "^--..");
			// corner_tr ^.--.
			this.buildTileListBySearch("corner_tr", "^.--.");
			// corner_bl ^-..-
			this.buildTileListBySearch("corner_bl", "^-..-");
			// corner_br ^..--
			this.buildTileListBySearch("corner_br", "^..--");

			// build edge lists - need L/U adj for right col & bottom row..
			/*
			string[] edge_patterns = Board.getEdgePatternList();
			foreach ( string p in edge_patterns )
			{
				this.buildTileListBySearch("edge_left_" + p, "-[A-Z]{3}");
			}
			*/
			
			// edge_left -[A-Z]{3}
			this.buildTileListBySearch("edge_left", "-[^-]{3}");
			// edge_top  [A-Z]-[A-Z]{2}
			this.buildTileListBySearch("edge_top", "[^-]-[^-]{2}");
			// edge_right [A-Z]{2}-[A-Z]
			this.buildTileListBySearch("edge_right", "[^-]{2}-[^-]");
			// edge_bottom [A-Z]{3}-
			this.buildTileListBySearch("edge_bottom", "[^-]{3}-");

			// internal
			this.buildTileListBySearch("internal", "[^-]{4}");

			// left_
			// top_
			foreach ( string pid in Program.TheMainForm.board.patterns )
			{
				this.buildTileListBySearch("left_" + pid, pid + "[^-]{3}");
				this.buildTileListBySearch("top_" + pid, "[^-]" + pid + "[^-]{2}");
			}

			// build tile list for each adjacent pattern set
			// use adj lists for tile list AB, AC, AF, FF, FG, FH etc
			List<string> patternList = this.getAdjacentPatternList();
			foreach ( string patterns in patternList )
			{
				if ( patterns.Length == 2 )
				{
					string listId = "adj_" + patterns;
					string a = patterns[0].ToString();
					string b = patterns[1].ToString();
					string search;
					
					// find tiles that match the adjacent pattern using the following regex: AB|B..A
//					search = a + b + "|" + b + ".." + a;
					// exclude corners from adj lists as they are defined separately
					search = "^" + a + b + "[^\\-]{2}";
					this.buildTileListBySearch(listId, search);
				}
			}
		}
		
		public void buildTileListBySearch(string listId, string search)
		{
			List<string> tilelist = new List<string>();
			foreach ( string tilepattern in this.tileCacheIds.Keys )
			{
	            if ( Regex.IsMatch(tilepattern, search, RegexOptions.IgnoreCase) )
	            {
	            	tilelist.Add(tilepattern);
	            }
			}
			if ( tilelist.Count > 0 )
			{
				this.saveTilelist(listId, String.Join("\r\n", tilelist.ToArray()) + "\r\n");
				Program.TheMainForm.log("created tile list " + listId + " with " + tilelist.Count + " tile(s)");
			}
		}
		
		public List<string> getAdjacentPatternList()
		{
			// generate sorted list of unique valid adjacent pattern pairs from tileset
			// eg. --, -A, AA, AB, AZ, ZA, AB, etc
			HashSet<string> adjpatterns = new HashSet<string>();
			List<string> pairs = new List<string>();
//			string borderRegex = Board.getEdgePatternRegex();
			string borderRegex = Board.edge_pattern_regex;
			foreach ( string tile in this.tileSet.Values )
			{
				// ABCD -> AB, BC, CD, DA
				pairs.Add(tile.Substring(0, 2));
				pairs.Add(tile.Substring(1, 2));
				pairs.Add(tile.Substring(2, 2));
				pairs.Add(tile[3].ToString() + tile[0].ToString());
				foreach ( string pair in pairs )
				{
					// only allow valid border edge combinations (eg. exlucde inner/border pairs)
					if ( Regex.IsMatch(pair, "-") )
					{
						if ( Regex.IsMatch(pair, "-" + borderRegex) || Regex.IsMatch(pair, borderRegex + "-") )
						{
							adjpatterns.Add(pair);
						}
					}
					else
					{
						adjpatterns.Add(pair);
					}
				}
			}
			// sort patterns and return as list
			string[] patternArray = new string[adjpatterns.Count];
			adjpatterns.CopyTo(patternArray);
			List<string> patternList = new List<string>(patternArray);
			patternList.Sort();
			return patternList;
		}
		
		public void saveTilelist(string id, string data)
		{
			string dirpath = "tilelists\\" + Board.title;
			string filename = System.IO.Path.Combine(dirpath, id + ".txt");
			System.IO.File.WriteAllText(filename, data);
		}

		public int loadQueue(int cellId)
		{
//			this.timer.start("loadQueueForCellId");
//			Program.TheMainForm.timer.start("loadQueueForCellId");
			string qid = "";

			// corner_tl
			if ( cellId == 1 )
			{
				qid = "corner_tl";
			}
			// corner_tr
			else if ( cellId == this.solver.numCols )
			{
				qid = "corner_tr";
			}
			// corner_bl
			else if ( cellId == this.solver.boardSize() - (this.solver.numCols - 1) )
			{
				qid = "corner_bl";
			}
			// corner_br
			else if ( cellId == this.solver.boardSize() )
			{
				qid = "corner_br";
			}
			// edge_top
			else if ( cellId > 1 && cellId < this.solver.numCols )
			{
				qid = "edge_top";
			}
			// edge_bottom
			else if ( cellId > this.solver.boardSize() - (this.solver.numCols - 1) && cellId < this.solver.boardSize() )
			{
				qid = "edge_bottom";
			}
			// edge_left
			else if ( cellId % this.solver.numCols == 1 )
			{
				qid = "edge_left";
			}
			// edge_right
			else if ( cellId % this.solver.numCols == 0 )
			{
				qid = "edge_right";
			}
			else
			{
				// use cell specific lists if they exist
				qid = "cell_" + cellId;
			}
			if ( !this.tileList.ContainsKey(qid) )
			{
				// use adj lists for internal tile list
				// load adjacent tilelist for rows_right_down - direction = left-top
				string tileSearch = this.solver.getTileListMatchString(cellId);
				qid = "internal";
				// "" = inner tile
				// use any internal
				if ( tileSearch == "" )
				{
					qid = "internal";
				}
				else if ( tileSearch.Length == 2 )
				{
					// left-up adjacent search
					qid = "adj_" + tileSearch;
				}
				else
				{
					qid = tileSearch;
				}
			}
			if ( qid.Length > 0 && this.tileList.ContainsKey(qid) )
			{
//				this.queueList[cellId].InsertRange(0, this.tileList[qid]);
//				this.queueList[cellId] = this.tileList[qid];
				
				// apply cell filter to queue if enabled
				if ( this.solver.cellFilters.ContainsKey(cellId) )
				{
					this.loadQueueViaCellFilter(cellId, qid);
				}
				else
				{
					this.queueList[cellId] = new List<string>(this.tileList[qid]);
				}
			}
//			Program.TheMainForm.timer.stop("loadQueueForCellId");
//			this.timer.stop("loadQueueForCellId");
			return this.queueList[cellId].Count;
		}
		
		// loads queue list with tiles that match cell filter if enabled
		public void loadQueueViaCellFilter(int cellId, string qid)
		{
			this.queueList[cellId] = new List<string>();
			int i = 0;
			foreach ( string tile in this.tileList[qid] )
			{
				i++;
				if ( this.solver.checkCellFilter(cellId, tile) )
				{
					this.queueList[cellId].Add(tile);
				}
			}
			//this.logt("loaded " + this.queueList[cellId].Count + " / " + i + " tiles in queue for cellId " + cellId, 3);
		}

		public string fetchTile(int cellId)
		{
			// fetch tile from queue only if it fits in the specified cell
			// check if queue loaded for cellId
				// load new queue if none loaded
			// fetch next tile from queue if available
			// check if tile is hint or already used
			// if tile is hint or used then re-fetch
			if ( cellId == 0 )
			{
				return null;
			}

			/*
			if ( CAF_Application.config.contains("solver3_debug_on_fetch_cell") && cellId == Convert.ToInt16(CAF_Application.config.getValue("solver3_debug_on_fetch_cell")) )
			{
				System.Diagnostics.Debugger.Break();
			}
			*/
			
			string tile = null;
			if ( this.queueList.ContainsKey(cellId) && this.queueList[cellId].Count == 0 )
			{
				// load queue if not loaded (find matching tiles)
				int queueSize = this.loadQueue(cellId);
				if ( queueSize > 0 )
				{
					this.queueProgress[cellId-1] = 1;
					tile = this.queueList[cellId][this.queueProgress[cellId-1]-1];
				}
			}
			else if ( this.queueProgress.Length >= cellId )
			{
				if ( this.queueProgress[cellId-1] < this.queueList[cellId].Count )
				{
					this.queueProgress[cellId-1]++;
					if ( this.queueProgress[cellId-1] > 0 && this.queueProgress[cellId-1] <= this.queueList[cellId].Count )
					{
						tile = this.queueList[cellId][this.queueProgress[cellId-1]-1];
					}
				}
				else
				{
					// end of queue for current cell
					// skip cell if enabled
					if ( CAF_Application.config.contains("max_empty_cells") && Convert.ToInt16(CAF_Application.config.getValue("max_empty_cells")) > 0 )
					{
						return "EMPTY";
					}
				}
			}

			// check if tile is valid
			if ( tile != null )
			{
				// re-fetch if tile is used or a hint tile
				if ( this.hintTileCache.Contains(tile) || this.numAvailTiles(tile) < 1 )
				{
					tile = this.fetchTile(cellId);
				}

				// re-fetch if does not pass cell filter
				if ( tile != null && ( !this.solver.checkCellFilter(cellId, tile) || !this.solver.checkCellFilter(0, tile) ) )
				{
					tile = this.fetchTile(cellId);
				}
			}

			CAF_Application.stats.inc("numTilesFetched");
			return tile;
		}
		

		/* TILE functions */

		public bool loadTileset(string filename)
		{
			this.tileSet.Clear();
			
			// load from file
			// try filename directly if exists
			// else try in tilesets folder (auto add .txt extension)
			string sourcefile;
			string[] lines;
			try
			{
				if ( System.IO.File.Exists(filename) )
				{
					sourcefile = filename;
				}
				else if ( System.IO.File.Exists("tilesets\\" + filename + ".txt") )
				{
					sourcefile = "tilesets\\" + filename + ".txt";
				}
				else
				{
					// file not found
					return false;
				}
				lines = System.IO.File.ReadAllLines(sourcefile);
			}
			catch
			{
				return false;
			}
			
			string pattern = "";
			int tileId = 0;
			foreach ( string line in lines )
			{
				pattern = line.Trim().ToUpper();
				if ( pattern.Length == 4 && pattern[0] != '#' && pattern[0] != ';' )
				{
					tileId++;
					this.tileSet.Add(tileId, pattern);
				}
			}

			// create tile cache
			this.prepareTileset();

			return this.tileSet.Count > 0;
		}
		
		public bool setTileset(string[] tiles)
		{
			this.tileSet.Clear();
			// set tileset from string array
			int tileId = 0;
			foreach ( string tile in tiles )
			{
				string pattern = tile.Trim().ToUpper();
				if ( pattern.Length == 4 && pattern[0] != '#' && pattern[0] != ';' )
				{
					tileId++;
					this.tileSet.Add(tileId, pattern);
				}
			}
			// create tile cache
			this.prepareTileset();
			return this.tileSet.Count > 0;
		}
		
		private void prepareTileset()
		{
			// create tileCache & tileCacheIds from tileSet
			this.tileCache.Clear();
			this.tileCacheIds.Clear();
			foreach ( int tileId in this.tileSet.Keys )
			{
				string sourceTile = this.tileSet[tileId];
				// create tileCache
				if ( !this.tileCache.ContainsKey(tileId) )
			    {
					this.tileCache.Add(tileId, 1);
			    }
				else
				{
					this.tileCache[tileId]++;
				}

				// iterate through all rotations
				string[] patterns = this.getAllRotations(sourceTile);
				foreach ( string tile in patterns )
				{
					// create tileCacheIds
					if ( this.tileCacheIds.ContainsKey(tile) )
					{
						this.tileCacheIds[tile].Add(tileId);
					}
					else
					{
						this.tileCacheIds.Add(tile, new List<int>(new int[]{tileId}));
					}
				}
			}
		}
		
		// col, row, pattern
		public bool setHintTiles(string[] hints)
		{
			try
			{
				this.hintTiles.Clear();
				this.hintTileCache.Clear();
				
				// add hints to board
				foreach ( string line in hints )
				{
					List<string> data = new List<string>(line.Trim().Split(','));
					if ( data.Count == 4 && data[0][0] != ';' && data[0][0] != '#' )
					{
						int col = Convert.ToInt16(data[0]);
						int row = Convert.ToInt16(data[1]);
						string pattern = data[2].Trim().ToUpper();
						int cellId = (row - 1) * this.solver.numCols + col;
						this.hintTiles.Add(cellId, pattern);
						this.hintTileCache.AddRange(this.getAllRotations(pattern));
						
						// place on solver board
						this.solver.placeTile(cellId, pattern);
					}
				}
			}
			catch
			{
				return false;
			}
			this.solver.setAdjacentHintFilters();
			return true;
		}
		
		public int numAvailTiles(string pattern)
		{
			int rv = 0;
			if ( !this.tileCacheIds.ContainsKey(pattern) )
			{
				return rv;
			}
			foreach ( int tileId in this.tileCacheIds[pattern] )
			{
				if ( this.tileCache.ContainsKey(tileId) )
				{
					rv += this.tileCache[tileId];
				}
			}
			return rv;
		}
		
		public void markUsedTile(string pattern)
		{
			foreach ( int tileId in this.tileCacheIds[pattern] )
			{
				if ( this.tileCache.ContainsKey(tileId) && this.tileCache[tileId] > 0 )
				{
					this.tileCache[tileId]--;
					break;
				}
			}
		}
		
		public void freeTile(string pattern)
		{
			foreach ( int tileId in this.tileCacheIds[pattern] )
			{
				if ( this.tileCache.ContainsKey(tileId) )
				{
					this.tileCache[tileId]++;
					break;
				}
			}
		}
		
		public string[] getAllRotations(string startingPattern)
		{
	        // creates all pattern rotations, in clockwise sequence
	        // LTRB -> BLTR -> RBLT -> TRBL
	        string[] patterns = new string[4];
	        patterns[0] = startingPattern;
	        string pattern = startingPattern;
	        for ( int i = 2; i <= 4; i++ )
	        {
	        	pattern = pattern.Substring(pattern.Length-1) + pattern.Substring(0, 3);
	        	patterns[i-1] = pattern;
	        }
	        return patterns;
		}
		
	}
}

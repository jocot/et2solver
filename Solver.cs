/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 9/12/2009
 * Time: 5:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

/*
27-May-2010 - remove hint tiles from solve path so max tiles / max placed don't exceed numTiles + solve_path.Count
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Forms;
//using System.Threading;
using et2;
using CAF;

namespace ET2Solver
{
	/// <summary>
	/// Description of Solver.
	/// </summary>
	public class Solver
	{
		// debug level = 0 = off, 1 = low verbosity, 2 = medium verbosity, 3 = high verbosity
		public int debug_level = 1;
		
		public AppFlowLog appflow = new AppFlowLog();
		
		public PatternStats ps = new PatternStats();
		public SortedDictionary<string, string> uniquePatternOrientations = new SortedDictionary<string, string>();
		
		// solution pattern orientation to match for testing
		public string solution_pattern_orientation_hash = "2c4cdfa98465278028713ba4c0d2b745f3ec866e";	// 200168
		
		// how often to update stats (number of ticks)
		public Int64 update_frequency = 10000000;
		public Int64 last_update_ticks = 0;
		public Int64 last_update_iteration = 0;
		
		// autosave after x iterations
		public bool autosave_enable_iteration = false;
		public int autosave_iteration = 0;
		public Int64 lastSaveIteration = 0;
		
		// auto stop on iteration limit
		public bool autostop_on_iteration = false;
		// max number of iterations before restarting / stopping
		public Int64 maxIterations = 0;
		public int iterationScoreProximity = 0;
		
		public bool single_step = false;
		
		// run multiple jobs
		public bool run_multiple = false;
		public int num_runs = 1;
		public int last_run = 0;
		
		public int numPasses = 0;
		public int lastPass = 0;
		public int maxPasses = 10;
		
		// TODO maxIterationsPerCell - if current cell path doesn't achieve steady progress then go to next cell in queue after maxIterationsPerCell
		
		// auto score trigger
		public bool auto_score_trigger_enabled = false;
		public int auto_score_trigger = 0;
		// next trigger is used for auto increment
		public int auto_score_next_trigger = 0;
		// auto pause when score reached
		public bool auto_score_pause = false;
		// auto save queue when score reached
		public bool auto_score_save = false;
		// auto increment next score trigger when queue saved
		public bool auto_score_increment = false;
		
		// tileset for source tile data
		public Dictionary<int, TileInfo> tileset = new Dictionary<int, TileInfo>();
		
		// for balanced tilesets (does 0 rotating)
		public bool fixedRotation = false;

		// all tiles in all rotations - for finding tileIds
		public Dictionary<string, int> alltiles = new Dictionary<string, int>();
		// all tiles in all rotations - for searching (randomisable)
		public List<string> searchtileset = new List<string>();
		
		// cell filters for restricting tiles to certain cells
		public Dictionary<int, string> cellFilters = new Dictionary<int, string>();
		
		// cellPos, TileInfo
		public Dictionary<int, int> hintTiles = new Dictionary<int, int>();

		// board for tile placement, pattern rotations etc
		public SortedDictionary<int, TileInfo> board = new SortedDictionary<int, TileInfo>();
		
		public Dictionary<char, int> borderPatternCount = new Dictionary<char, int>();
		
		// used tile ids
//		public List<int> usedTiles = new List<int>();
		public HashSet<int> usedTiles = new HashSet<int>();

		// queue
		public Dictionary<int, List<TileInfo>> queueList = new Dictionary<int, List<TileInfo>>();
//		public List<int> queueProgress = new List<int>(new int[Board.max_tiles]);
		public int[] queueProgress = new int[Board.max_tiles];
		
		// pre-saved tile lists
		public Dictionary<string, List<TileInfo>> tileList = new Dictionary<string, List<TileInfo>>();
		
		public bool storeQueueInMemory = false;
		public string queueFilename = "";
		public List<string> queueData = new List<string>();
		public string modelData = "";
		public string modelFilename = "";
		
		// solver settings

		// random seed generator
		public bool useRandom = false;
		public bool useRandomTileset = false;
		public bool useRandomTileQueue = false;
		public bool randomSeeds = false;
		public int start_seed = 0;
		public int seed_step = 1;
		public Random r = new Random();
		public Int32 seed;

		// max number of cells to backtrack before restarting / stopping
		public int backtrackDepthLimit = 0;
		public int backtrackMinIterations = 0;
		public int backtrackMinScore = 0; 
		public int backtrackStaleIterations = 0;
		public Int64 backtrackLastTriggeredIteration = 0;

		public bool backtrack_empty_cell = false;
		public int first_empty_cell = 0;
		public List<int> empty_cells = new List<int>();
		public int max_empty_cells = 0;
		
		// solve path
		public PathBuilder pb = new PathBuilder();
		public List<int> solve_path = new List<int>();
		// id of current cell in solve path
		public int solve_path_id = 0;

		// solve stats
		public Int64 numIterations = 0;
		public Int64 numStaleIterations = 0;
		public Int64 numLastStaleIterations = 0;
		public Int64 maxStaleIterations = 0;
		public Int64 numBorderIterations = 0;
		public Int64 numBorderSlipMisses = 0;
		public int mostTilesPlaced = 0;
		public Int64 bestIteration = 0;
		public int overallMostTilesPlaced = 0;
		public List<string> solveResults = new List<string>();
		public int bestScore = 0;
		
		public Dictionary<int, Int64> scoreStats = new Dictionary<int, Int64>(Board.max_tiles);
		public List<string> iterationLog = new List<string>();
		public int numQueueJumps = 0;
		
		public List<string> solutionStats = new List<string>();
		public Dictionary<int, HashSet<string>> tileDistribution = new Dictionary<int, HashSet<string>>();
		public Dictionary<int, HashSet<char>> patternDistribution = new Dictionary<int, HashSet<char>>();

		// colour,[numHighest,highestCount]
		public Dictionary<string, int[]> bgcolour_stats = new Dictionary<string, int[]>();
		public SortedDictionary<int, int> num_bgcolours = new SortedDictionary<int, int>();
		
		// pause/resume
		public bool isPaused = false;
		public bool isResumed = false;
		public bool isStopped = false;
		public bool isNextRun = false;
		
		// track number of unique vs total solutions (store hash of solutions)
		public bool saveSolutionImages = false;
		public bool trackNumUniqueSolutions = true;
		public List<string> uniqueSolutions = new List<string>();
		public List<string> totalSolutions = new List<string>();
		
		// iteration interval to measure speed
		public int speedInterval = 1000;
		
		// extract & compare pattern orientation after solved (use on borders to compare orientation of inner edges
		public bool extractPatternCountAfterSolving = false;

		// enable/disable auto saving of results
		public bool saveResultsEnabled = false;
		
		public bool useRegionRestrictions = false;
		public int numRegionSkips = 0;
		
		// wild card patterns that will match anything if nothing else found
//		public List<string> wildcards = new List<string>();
//		public string wildcard = "N";
		
		public string solve_method = "";
		public bool method_right_down = false;
		
		public bool enforce_fillable_cells = false;
		public bool backtrack_trigger_enabled = false;
		public bool backtrack_skip_cell = false;
		public bool backtrack_pause = false;
		public bool backtrack_queue_jump = false;
		public bool backtrack_back_pedal = false;
		public Int64 num_overall_tiles_placed = 0;

		public double time_elapsed = 0;
		public double end_time = 0;
		public double start_time = 0;
		
		public SpeedTimer timer = new SpeedTimer();
		
		// count number of searches, number of hints (1+ results), number of misses (0 results)
		public Dictionary<string, int[]> searchStats = new Dictionary<string, int[]>();

		// tile stats - tileId, numPlaced
		public Dictionary<int, int> tileStats = new Dictionary<int, int>();
		
		// store magic square csv data (from isMagicInnerSquareBitValues())
		public List<string> magicSquareData = new List<string>();
		
		// solver3
		public Solver3 solver3 = new Solver3();
		public Int64 solverProgressCount = 0;

		public Solver()
		{
			this.pb.defineMethods();
		}
		
		public int randomNumber()
		{
	        // 2-pass random generator to generate more balanced variety of random numbers... 
	        // 1st pass determines number range
	        // 2nd pass gets random number from that range
	        List<int> negnums = new List<int>();
	        negnums.Add(-1024);
	        negnums.Add(-65536);
	        negnums.Add(0);
	        negnums.Add(-16384);
	        negnums.Add(-2147483647);
	        negnums.Add(-32768);
	        negnums.Add(0);
	        negnums.Add(-8388608);
	        negnums.Add(0);
	        negnums.Add(-1048576);
	        negnums.Add(-256);
	        negnums.Add(0);
	        negnums.Add(-4194304);
	        negnums.Add(-8192);
	        negnums.Add(0);
	        negnums.Add(-16777216);
	        int minvalue = negnums[this.randomNumber(negnums.Count)];

	        List<int> posnums = new List<int>();
	        posnums.Add(256);
	        posnums.Add(4194304);
	        posnums.Add(8192);
	        posnums.Add(1048576);
	        posnums.Add(16384);
	        posnums.Add(2147483647);
	        posnums.Add(1024);
	        posnums.Add(32768);
	        posnums.Add(65536);
	        posnums.Add(8388608);
	        posnums.Add(16777216);
	        posnums.Add(64);
	        int maxvalue = posnums[this.randomNumber(posnums.Count)];

			string sticks = DateTime.Now.Ticks.ToString();
			int nticks = Convert.ToInt32(sticks.Substring(sticks.Length-12, 9));
			Random r = new Random(nticks);
	        return r.Next(minvalue, maxvalue);
		}
		
		public int randomNumber(int max)
		{
			string sticks = DateTime.Now.Ticks.ToString();
			int nticks = Convert.ToInt32(sticks.Substring(sticks.Length-12, 9));
			Random r = new Random(nticks);
			return r.Next(0, max);
		}
		
		public int getSeed()
		{
//			int unixtime = (int)(DateTime.UtcNow - new DateTime(1970,1,1,0,0,0)).TotalSeconds;
//			this.seed = unixtime;
//			r = new Random(unixtime);

			// generate a random number as a seed instead of just using the timestamp
			// use ticks to get > 1 random numbers / second
			/*
			string sticks = DateTime.Now.Ticks.ToString();
			int nticks = Convert.ToInt32(sticks.Substring(sticks.Length-12, 9));
			Random r = new Random(nticks);
			this.seed = r.Next(-2147483647, 2147483647);
			*/
			this.seed = this.randomNumber();
			Program.TheMainForm.inputS1StartSeed.Text = this.seed.ToString();
			Program.TheMainForm.inputS1StartSeed.Update();
			return this.seed;
		}
		
		public string getSeedInfo()
		{
			string seedinfo = "";
			if ( this.useRandom && this.seed > 0 )
			{
				seedinfo = "seed" + this.seed.ToString() + "-";
			}
			return seedinfo;
		}
		
		public void randomiseStringList(List<string> list)
		{
			for ( int i = 0; i < list.Count; i++ )
			{
				int pos = this.r.Next(list.Count);
				string a = list[i];
				string b = list[pos];
				list[i] = b;
				list[pos] = a;
			}
		}
		
		public void randomiseIntList(List<int> list)
		{
			for ( int i = 0; i < list.Count; i++ )
			{
				int pos = this.r.Next(list.Count);
				int a = list[i];
				int b = list[pos];
				list[i] = b;
				list[pos] = a;
			}
		}
		
		public object[] randomiseList(object[] list)
		{
			for ( int i = 0; i < list.Length; i++ )
			{
				int pos = this.r.Next(list.Length);
				object a = list[i];
				object b = list[pos];
				list[i] = b;
				list[pos] = a;
			}
			return list;
		}
		
		public void randomiseTileset()
		{
			this.randomiseStringList(this.searchtileset);
		}

		public void randomiseQueueList(int cellId)
		{
//			Program.TheMainForm.timer.start("randomiseQueueList");
			for ( int i = 0; i < this.queueList[cellId].Count; i++ )
			{
				int pos = this.r.Next(this.queueList[cellId].Count);
				TileInfo a = this.queueList[cellId][i];
				TileInfo b = this.queueList[cellId][pos];
				this.queueList[cellId][i] = b;
				this.queueList[cellId][pos] = a;
			}
//			Program.TheMainForm.timer.stop("randomiseQueueList");
		}
		
		public void loadTileset()
		{
			this.tileset = new Dictionary<int, TileInfo>();
			this.alltiles = new Dictionary<string, int>();
			this.searchtileset = new List<string>();
			foreach ( Tile tile in Program.TheMainForm.board.tileset )
			{
				if ( tile != null )
				{
					this.tileset.Add(tile.id, new TileInfo(tile.id, tile.patterns[0]));
					foreach ( string pattern in tile.patterns )
					{
						if ( !this.alltiles.ContainsKey(pattern) )
						{
							this.alltiles.Add(pattern, tile.id);
							this.searchtileset.Add(pattern);
						}
						
						if ( this.fixedRotation )
						{
							break;
						}
					}
				}
			}
			
			this.logt(Board.title + " loaded with " + this.tileset.Count + " (" + Board.max_tiles + ") / " + this.alltiles.Count + " / " + this.searchtileset.Count+ " (" + Board.max_tiles * 4 + ") tiles", 1);
			
			// create tilelist path for tileset if doesnt exist (to store hints etc)
			string dirpath = "tilelists\\" + Board.title;
			if ( !System.IO.File.Exists(dirpath) )
			{
				System.IO.Directory.CreateDirectory(dirpath);
			}
		}

		public void loadQueueList()
		{
			if ( Board.title == "" )
			{
				return;
			}
			string[] filenames = System.IO.Directory.GetFiles("queues");
			Array.Sort(filenames);
			Array.Reverse(filenames);
			string id;
			Program.TheMainForm.selQS1.Items.Clear();
			for ( int i = 0; i < filenames.Length; i++ )
			{
				string filename = filenames[i];
				if ( System.IO.File.Exists(filename) )
				{
					id = System.IO.Path.GetFileNameWithoutExtension(filename);
					// only show queues relating to current tileset
					if ( id.Length >= Board.title.Length && id.Substring(0, Board.title.Length + 1) == Board.title + "-" )
					{
						// remove tileset from queue name
						id = id.Substring(Board.title.Length + 1);
						Program.TheMainForm.selQS1.Items.Add(id);
					}
				}
			}
		}
		
		public void queueClear(int cellId)
		{
//			Program.TheMainForm.timer.start("queueClear");
			if ( cellId > 0 )
			{
				this.queueList[cellId] = new List<TileInfo>();
				if ( this.queueProgress.Length >= cellId )
				{
					this.queueProgress[cellId-1] = 0;
				}
			}
//			Program.TheMainForm.timer.stop("queueClear");
		}
		
		public void initQueues()
		{
			this.queueProgress = new int[Board.max_tiles];
			for ( int i = 1; i <= Board.max_tiles; i++ )
			{
				this.queueClear(i);
			}
			
			this.queueData = new List<string>();
			this.queueFilename = "";
			this.modelData = "";
		}
		
		public void logl(string text, int level)
		{
//			Program.TheMainForm.timer.start("logl");
			if ( this.debug_level > 0 && this.debug_level >= level )
			{
				if ( !text.Contains("\r\n") )
				{
					text += "\r\n";
				}
				//Program.TheMainForm.logS1.Text += text;
				Program.TheMainForm.logS1.AppendText(text);
				Program.TheMainForm.logS1.Update();
			}
//			Program.TheMainForm.timer.stop("logl");
		}
		
		public void logt(string text, int level)
		{
//			Program.TheMainForm.timer.start("logt");
			string timestamp = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
			this.logl(timestamp + " : " + text, level);
//			Program.TheMainForm.timer.stop("logt");
		}

		public void log(string text)
		{
//			Program.TheMainForm.timer.start("log");
			this.logl(text, 3);
//			Program.TheMainForm.timer.start("log");
		}
		
		public void placeTile(int cellId, TileInfo tile)
		{
//			this.timer.start("placeTile");
//			Program.TheMainForm.timer.start("placeTile");
			// remove any previous cell if they exist
			this.removeTile(cellId);
			
			// place on board
//			this.logt("placing tile " + tile.pattern + " onto cell " + cellId.ToString(), 3);
			if ( this.board.ContainsKey(cellId) )
			{
				this.board[cellId] = tile;
			}
			else
			{
				this.board.Add(cellId, tile);
			}
			if ( !this.usedTiles.Contains(tile.id) )
			{
				this.usedTiles.Add(tile.id);
			}
			this.num_overall_tiles_placed++;
			
			// track tile usage stats
			//this.tileStats[tile.id]++;
			
			// remove from empty cells if exists
			if ( this.empty_cells.Contains(cellId) )
			{
				this.empty_cells.Remove(cellId);
			}
			
			// count number of times scores are reached
			if ( this.scoreStats.ContainsKey(this.board.Count) )
			{
				this.scoreStats[this.board.Count]++;
			}
			else
			{
				this.scoreStats.Add(this.board.Count, 1);
			}
			
			if ( this.backtrackMinScore > 0 && this.board.Count <= this.backtrackMinScore )
			{
				// reset stale iterations if score reaches min threshold
				this.resetStaleIterations();
			}
//			Program.TheMainForm.timer.stop("placeTile");
//			this.timer.stop("placeTile");
		}
		
		public bool updateable()
		{
//			Program.TheMainForm.timer.start("updateable");
			if ( DateTime.Now.Ticks >= this.last_update_ticks + this.update_frequency )
			{
				this.last_update_ticks = DateTime.Now.Ticks;
				this.last_update_iteration = this.numIterations;
//				Program.TheMainForm.timer.stop("updateable");
				return true;
			}
			else
			{
//				Program.TheMainForm.timer.stop("updateable");
				return false;
			}
		}

		public void updateStats(bool force)
		{
			// multithreading support
//			ReaderWriterLockSlim trw = new ReaderWriterLockSlim();
			
			if ( force || this.updateable() )
			{
				System.Windows.Forms.Application.DoEvents();
//				trw.EnterWriteLock();
				int cellId = this.getCellId();
				int pathLen = this.solve_path.Count;
//				int maxTiles = this.solve_path.Count + this.hintTiles.Count;
				int maxTiles = Board.max_tiles;
				Program.TheMainForm.labelS1NumIterations.Text = this.numIterations.ToString();
				Program.TheMainForm.labelCellPath.Text = (this.solve_path_id + 1).ToString() + " / " + pathLen.ToString();
				Program.TheMainForm.labelS1TilesPlaced.Text = this.board.Count + " / " + maxTiles;
				Program.TheMainForm.labelS1MaxTilesPlaced.Text = this.overallMostTilesPlaced.ToString() + " / " + maxTiles;
				Program.TheMainForm.labelS1QueueProgress.Text = this.getQueueProgress();
				this.updateQueueStats();
				
				// refresh display
				Program.TheMainForm.labelS1NumIterations.Update();
				Program.TheMainForm.labelCellPath.Update();
				Program.TheMainForm.labelS1TilesPlaced.Update();
				Program.TheMainForm.labelS1MaxTilesPlaced.Update();
				Program.TheMainForm.labelS1QueueProgress.Update();
//				trw.ExitWriteLock();

			}
		}
		
		public void updateQueueStats()
		{
			int cellId = this.getCellId();
			if ( cellId > 0 )
			{
				if (this.queueProgress.Length > cellId) {
					Program.TheMainForm.labelQueueProgress.Text = this.queueProgress[cellId-1].ToString();
					if ( this.queueList.ContainsKey(cellId) ) {
						Program.TheMainForm.labelS2QueueSize.Text = this.queueList[cellId].Count.ToString();
					}
					Program.TheMainForm.labelQueueProgress.Update();
					Program.TheMainForm.labelS2QueueSize.Update();
				}
			}
		}

		public int getCellId()
		{
			this.appflow.comment("getCellId()");
			if ( this.solve_path.Count > this.solve_path_id )
			{
				return this.solve_path[this.solve_path_id];
			}
			else
			{
				return 0;
			}
		}
		
		public int getTileId(string pattern)
		{
			int tileId = 0;
			if ( this.alltiles.ContainsKey(pattern) )
			{
				tileId = this.alltiles[pattern];
			}
			return tileId;
		}
		
		public string getPatternAtColRow(int col, int row)
		{
			int pos = (row - 1) * Board.num_cols + col;
			if ( !this.board.ContainsKey(pos) )
			{
				return "";
			}
			else
			{
				return this.board[pos].pattern.ToString().Clone().ToString();
			}
		}
		
		public int getCellIdFromColRow(int col, int row)
		{
			return (row - 1) * Board.num_cols + col;
		}

		public TileInfo getTileFromColRow(int col, int row)
		{
//			Program.TheMainForm.timer.start("getTileFromColRow");
			int pos = (row - 1) * Board.num_cols + col;
			if ( !this.board.ContainsKey(pos) )
			{
				//Program.TheMainForm.log("getTileFromColRow(" + col + "," + row + ")=" + pos + ",N/A");
				return null;
			}
			TileInfo tile = new TileInfo(this.board[pos].id, this.board[pos].pattern);
			//Program.TheMainForm.log("getTileFromColRow(" + col + "," + row + ")=" + pos + "," + tile.title());
//			Program.TheMainForm.timer.stop("getTileFromColRow");
			return tile;
		}
		
		public string getTileMatchString(int pos)
		{
			int[] colrow = Board.getColRowFromPos(pos);
			return this.getTileMatchStringB(colrow[0], colrow[1]);
		}

		public string getTileMatchStringB(int col, int row)
		{
		    string match = "";
		    string pattern = "";
//		    string edgeTile = Board.getEdgePatternRegex();
//		    string internalTile = Board.getInternalPatternRegex();
		    string edgeTile = Board.edge_pattern_regex;
		    string internalTile = Board.internal_pattern_regex;
		    
		    // left
		    if ( col > 1 )
		    {
		        pattern = this.getPatternAtColRow(col - 1, row);
		        if ( pattern != "" )
		        {
		            match += pattern[2];
		        }
		        else if ( row == 1 || row == Board.num_rows )
		        {
		            match += edgeTile;
		        }
		        else
		        {
		            match += internalTile;
		        }
		    }
		    else
		    {
		        match += "-";
		    }
		
		    // top
		    if ( row > 1 )
		    {
		        pattern = this.getPatternAtColRow(col, row - 1);
		        if ( pattern != "" )
		        {
		            match += pattern[3];
		        }
		        else if ( col == 1 || col == Board.num_cols )
		        {
		            match += edgeTile;
		        }
		        else
		        {
		            match += internalTile;
		        }
		    }
		    else
		    {
		        match += "-";
		    }
		
		    // right
		    if ( col < Board.num_cols )
		    {
		        pattern = this.getPatternAtColRow(col + 1, row);
		        if ( pattern != "" )
		        {
		            match += pattern[0];
		        }
		        else if ( row == 1 || row == Board.num_rows )
		        {
		            match += edgeTile;
		        }
		        else
		        {
		            match += internalTile;
		        }
		    }
		    else
		    {
		        match += "-";
		    }
		
		    // bottom
		    if ( row < Board.num_rows )
		    {
		        pattern = this.getPatternAtColRow(col, row + 1);
		        if ( pattern != "" )
		        {
		            match += pattern[1];
		        }
		        else if ( col == 1 || col == Board.num_cols )
		        {
		            match += edgeTile;
		        }
		        else
		        {
		            match += internalTile;
		        }
		    }
		    else
		    {
		        match += "-";
		    }
		    return match;
		}

		public string getTileMatchString(int col, int row)
		{
//			Program.TheMainForm.timer.start("getTileMatchString");
		    string match = "";
		    string pattern = "";
//		    string edgeTile = Board.getEdgePatternRegex();
//		    string internalTile = Board.getInternalPatternRegex();
		    string edgeTile = Board.edge_pattern_regex;
		    string internalTile = Board.internal_pattern_regex;
		    TileInfo matchTile = null;

		    // left
		    if ( col > 1 )
		    {
		        matchTile = this.getTileFromColRow(col - 1, row);
		        if ( matchTile != null )
		        {
		            pattern = matchTile.pattern;
		            match += pattern[2];
		        }
		        else if ( row == 1 || row == Board.num_rows )
		        {
		            match += edgeTile;
		        }
		        else
		        {
		            match += internalTile;
		        }
		    }
		    else
		    {
		        match += "-";
		    }
		
		    // top
		    if ( row > 1 )
		    {
		        matchTile = this.getTileFromColRow(col, row - 1);
		        if ( matchTile != null )
		        {
		            pattern = matchTile.pattern;
		            match += pattern[3];
		        }
		        else if ( col == 1 || col == Board.num_cols )
		        {
		            match += edgeTile;
		        }
		        else
		        {
		            match += internalTile;
		        }
		    }
		    else
		    {
		        match += "-";
		    }
		
		    // right
		    if ( col < Board.num_cols )
		    {
		        matchTile = this.getTileFromColRow(col + 1, row);
		        if ( matchTile != null )
		        {
		            pattern = matchTile.pattern;
		            match += pattern[0];
		        }
		        else if ( row == 1 || row == Board.num_rows )
		        {
		            match += edgeTile;
		        }
		        else
		        {
		            match += internalTile;
		        }
		    }
		    else
		    {
		        match += "-";
		    }
		
		    // bottom
		    if ( row < Board.num_rows )
		    {
		        matchTile = this.getTileFromColRow(col, row + 1);
		        if ( matchTile != null )
		        {
		            pattern = matchTile.pattern;
		            match += pattern[1];
		        }
		        else if ( col == 1 || col == Board.num_cols )
		        {
		            match += edgeTile;
		        }
		        else
		        {
		            match += internalTile;
		        }
		    }
		    else
		    {
		        match += "-";
		    }
//			Program.TheMainForm.timer.stop("getTileMatchString");
		    return match;
		}
		
		// counts inner patterns on border tiles to use for inner solving by check distribution of edge inner patterns
		// avoids need to iterate through border tiles
		// intended result is faster solving by eliminating border iterations
		// count how many border queue iterations are performed in normal solve to determine % reduction..
		public void generateBorderInnerPatternCount()
		{
			this.borderPatternCount = new Dictionary<char, int>();
			foreach ( TileInfo tile in this.tileset.Values )
			{
            	// check if edge tile
            	if ( Regex.Matches(tile.pattern, "-").Count == 1 )
            	{
            		foreach ( char p in tile.pattern )
            		{
//            			if ( Regex.IsMatch(p.ToString(), Board.getInternalPatternRegex()) )
            			if ( Regex.IsMatch(p.ToString(), Board.internal_pattern_regex) )
        			    {
		            		if ( !this.borderPatternCount.ContainsKey(p) )
		            		{
		            			this.borderPatternCount.Add(p, 1);
		            		}
		            		else
		            		{
		            			this.borderPatternCount[p]++;
		            		}
        			    }
            		}
            	}
			}
		}
		
		public bool validBorderSlip(char pattern)
		{
			// check if pattern will create valid border pattern distribution
			// if true then update border pattern list
			// only concentrates on inner patterns therefore border patterns are slipped to make this work
			// and reduce the number of border iterations...
			// restore borderPatternCount when border touching tiles are removed..
			if ( this.borderPatternCount.ContainsKey(pattern) && this.borderPatternCount[pattern] > 0 )
			{
				this.borderPatternCount[pattern]--;
				return true;
			}
			else
			{
				this.numBorderSlipMisses++;
				return false;
			}
		}
		
		public void restoreBorderPattern(params char[] patterns)
		{
			foreach ( char p in patterns )
			{
				if ( this.borderPatternCount.ContainsKey(p) )
				{
					this.borderPatternCount[p]++;
				}
			}
		}
		
		public bool validPlacement(string pattern, int cellId)
		{
//			Program.TheMainForm.timer.start("validPlacement");
			bool rv = false;
			List<char> borderPatterns = new List<char>();

//		    if ( this.solve_method == "rows_right_down" )
			if ( this.method_right_down )
		    {
		    	// check if matches leftTile.right && aboveTile.bottom
		    	// top row = L
		    	if ( cellId > 1 )
		    	{
			    	if ( cellId <= Board.num_cols )
			    	{
			    		if ( this.board.ContainsKey(cellId-1) && pattern[0].ToString() != this.board[cellId-1].right )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
			    	// left col = U
			    	else if ( cellId % Board.num_cols == 1 )
			    	{
			    		if ( this.board.ContainsKey(cellId-Board.num_cols) && pattern[1].ToString() != this.board[cellId-Board.num_cols].down )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
			    	// everything else excluding top row & left col = LU
			    	else
			    	{
			    		if ( ( this.board.ContainsKey(cellId-1) && pattern[0].ToString() != this.board[cellId-1].right ) || ( this.board.ContainsKey(cellId-Board.num_cols) && pattern[1].ToString() != this.board[cellId-Board.num_cols].down ) )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
		    	}
		    	rv = true;
		    }
			else if ( this.solve_method == "inner" )
			{
		    	// check validBorderSlip
		    	// TL = left (top is handled below)
		    	if ( cellId == Board.num_cols + 2 )
		    	{
		    		if ( this.validBorderSlip(pattern[0]) )
		    		{
		    			borderPatterns.Add(pattern[0]);
		    		}
		    		else
		    		{
		    			goto Exit_Fail;
		    		}
		    	}

				// check if matches leftTile.right && aboveTile.bottom
		    	// top row = L
		    	if ( cellId > Board.num_cols + 2 )
		    	{
			    	// TR = right (top is handled below)
			    	if ( cellId == Board.num_cols * 2 - 1 )
			    	{
			    		if ( this.validBorderSlip(pattern[2]) )
			    		{
			    			borderPatterns.Add(pattern[2]);
			    		}
			    		else
			    		{
			    			goto Exit_Fail;
			    		}
			    	}
			    	// BL = bottom (left is handled below)
			    	if ( cellId == Board.num_cols * (Board.num_rows - 2) + 2 )
			    	{
			    		if ( this.validBorderSlip(pattern[3]) )
			    		{
			    			borderPatterns.Add(pattern[3]);
			    		}
			    		else
			    		{
			    			goto Exit_Fail;
			    		}
			    	}
			    	// BR = bottom (right is handled below)
			    	if ( cellId == Board.num_cols * (Board.num_rows - 1) - 1 )
			    	{
			    		if ( this.validBorderSlip(pattern[3]) )
			    		{
			    			borderPatterns.Add(pattern[3]);
			    		}
			    		else
			    		{
			    			goto Exit_Fail;
			    		}
			    	}
			    	
			    	if ( cellId <= Board.num_cols * 2 )
			    	{
		    			// check if top is valid border slip
			    		if ( pattern[0].ToString() != this.board[cellId-1].right || !this.validBorderSlip(pattern[1]) )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
			    	// left col = U
			    	else if ( cellId % Board.num_cols == 2 )
			    	{
		    			// check if left is valid border slip
			    		if ( pattern[1].ToString() != this.board[cellId-Board.num_cols].down || !this.validBorderSlip(pattern[0]) )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
			    	// right col
			    	else if ( cellId % Board.num_cols == Board.num_cols - 1 )
			    	{
			    		// check L && if right is valid border slip
			    		if ( pattern[0].ToString() != this.board[cellId-1].right || !this.validBorderSlip(pattern[2]) )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
			    	// bottom row
			    	else if ( cellId > Board.num_cols * Board.num_rows - Board.num_cols )
			    	{
			    		// check LU && if bottom is valid border slip
			    		if ( pattern[0].ToString() != this.board[cellId-1].right || pattern[1].ToString() != this.board[cellId-Board.num_cols].down || !this.validBorderSlip(pattern[3]) )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
			    	// all inners excluding top row & left col check LU
			    	else
			    	{
			    		if ( pattern[0].ToString() != this.board[cellId-1].right || pattern[1].ToString() != this.board[cellId-Board.num_cols].down )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
		    	}
		    	rv = true;
			}
		    else
		    {
				int[] colrow = Board.getColRowFromPos(cellId);
				string search = this.getTileMatchString(colrow[0], colrow[1]);
	//            if ( Regex.IsMatch(pattern, "^" + search, RegexOptions.IgnoreCase) && this.checkCellFilter(cellId, pattern) )
	            if ( Regex.IsMatch(pattern, "^" + search, RegexOptions.IgnoreCase) )
				{
					rv = true;
				}
		    }
			
		   	Exit_Fail:
		    	// restore border pattern count if failed with partial validBorderSlip() match (TL,TR,BL,BR)
		    	if ( !rv && this.solve_method == "inner" )
			    {
		    		this.restoreBorderPattern(borderPatterns.ToArray());
		    	}
//				Program.TheMainForm.timer.stop("validPlacement");
				return rv;
		}

		public void buildCachedTileQueue(int cellId)
		{
			int[] colrow = Board.getColRowFromPos(cellId);
			string search = this.getTileMatchString(colrow[0], colrow[1]);
			
			// store found tiles in cache to be re-used
			string qid = "search_" + search;
			if ( !this.tileList.ContainsKey(qid) )
			{
				// build new list and cache in memory
				this.tileList.Add(qid, new List<TileInfo>());
				
				bool validSearch = false;
				// TODO ? use border/edge/internal list to reduce search space & improve speed
				foreach ( string tilepattern in this.searchtileset )
				{
		            // check if tile matches, and if it passes cell filter (if enabled)
//		            bool tileFound = Regex.IsMatch(tilepattern, "^" + search, RegexOptions.IgnoreCase) && this.checkCellFilter(cellId, tilepattern);
		            bool tileFound = Regex.IsMatch(tilepattern, "^" + search, RegexOptions.IgnoreCase);

					// filter tileset if wildcard filter enabled
					bool passesFilter = this.checkCellFilter(0, tilepattern);
		            
					if ( tileFound && passesFilter )
					{
		            	validSearch = true;
	
		            	// skip if tile already exists in region (with restrictions enabled)
		            	if ( this.useRegionRestrictions )
		            	{
							// get patterns near cell in local region - quarter, list of patterns
				    		Dictionary<int, Dictionary<string, int>> regionPatterns = new Dictionary<int, Dictionary<string, int>>();
				
		            		int quarter = 0;
	//            			this.logt("cell " + cellId.ToString() + " - trying " + tilepattern, 2);
		            		foreach ( char c in tilepattern )
		            		{
		            			// ignore edges
		            			if ( c != '-' )
		            			{
		    						int regionId = this.pb.getRegionId(cellId, quarter);
						        	if ( !regionPatterns.ContainsKey(regionId) )
						        	{
						        		regionPatterns.Add(regionId, this.getRegionPatterns(regionId));
						        	}
	//		            			this.logt("cell " + cellId.ToString() + " - quarter " + quarter.ToString() + ", regionId: " + regionId.ToString() + ", patterns: " + String.Join(",", new List<string>(regionPatterns[regionId].Keys).ToArray()), 2);
			            			if ( regionPatterns.ContainsKey(regionId) && regionPatterns[regionId].ContainsKey(c.ToString()) && regionPatterns[regionId][c.ToString()] > 1 )
			            			{
			            				this.numRegionSkips++;
	//		            				this.logt("region restriction on cell " + cellId.ToString() + ", skipping tile: " + tilepattern, 2);
			            				tileFound = false;
			            				break;
			            			}
		            				// add current pattern to avoid duplicates (ie. for tiles that have double patterns)
		            				if ( regionPatterns[regionId].ContainsKey(c.ToString()) )
		            				{
			            				regionPatterns[regionId][c.ToString()]++;
		            				}
		            				else
		            				{
			            				regionPatterns[regionId].Add(c.ToString(), 1);
		            				}
		            			}
		            			quarter++;
		            		}
		            	}
	
	        		 	int tileId = this.alltiles[tilepattern];
		            	this.tileList[qid].Add(new TileInfo(tileId, tilepattern));

		            	// skip if tile is used or if it's a hint tile
		            	/*
	        		 	bool isUsedOrHint = this.usedTiles.Contains(tileId) || this.hintTiles.ContainsValue(tileId);
		            	if ( !isUsedOrHint )
		            	{
			            	this.tileList[qid].Add(new TileInfo(tileId, tilepattern));
		            	}
		            	*/
					}
				}

				// count num searches/num invalid/hits/misses
				if ( !this.searchStats.ContainsKey(search) )
				{
					this.searchStats.Add(search, new int[4]);
				}
				// num searches
				this.searchStats[search][0]++;
				// num invalid searches
				if ( !validSearch )
				{
					this.searchStats[search][1]++;
				}
				if ( this.queueList[cellId].Count > 0 )
				{
					// num hits
					this.searchStats[search][2]++;
				}
				else
				{
					// num misses
					this.searchStats[search][3]++;
				}
			}
			
			// clear previous queue
			this.queueClear(cellId);

			// load queue from tile cache if exists
			if ( this.tileList.ContainsKey(qid) )
			{
				this.queueList[cellId] = new List<TileInfo>(this.tileList[qid]);
			}
			
//			this.log("buildCachedTileQueue(path: " + (this.solve_path_id+1).ToString() + ", cellId: " + cellId.ToString() + ", search: " + search + " = " + this.queueList[cellId].Count.ToString());
		}

		public void buildTileQueue(int cellId)
		{
//			Program.TheMainForm.timer.start("buildTileQueue");

//			Program.TheMainForm.timer.start("buildTileQueue.init");
			int[] colrow = Board.getColRowFromPos(cellId);
			string search = this.getTileMatchString(colrow[0], colrow[1]);
			this.queueClear(cellId);
//			Program.TheMainForm.timer.stop("buildTileQueue.init");
			
//			this.log("buildTileQueue(path: " + (this.solve_path_id+1).ToString() + ", cell:" + cellId.ToString() + ", search: " + search);
//			this.logl("searching " + this.searchtileset.Count + " tiles", 1);
//			Program.TheMainForm.timer.start("foreach.searchtileset");
			
			bool validSearch = false;
			foreach ( string tilepattern in this.searchtileset )
			{
	            // check if tile matches, and if it passes cell filter (if enabled)
//	            Program.TheMainForm.timer.start("Regex");
	            bool tileFound = Regex.IsMatch(tilepattern, "^" + search, RegexOptions.IgnoreCase) && this.checkCellFilter(cellId, tilepattern);
//	            Program.TheMainForm.timer.stop("Regex");
	            if ( tileFound )
				{
	            	validSearch = true;

	            	// skip if tile already exists in region (with restrictions enabled)
	            	if ( this.useRegionRestrictions )
	            	{
						// get patterns near cell in local region - quarter, list of patterns
			    		Dictionary<int, Dictionary<string, int>> regionPatterns = new Dictionary<int, Dictionary<string, int>>();
			
	            		int quarter = 0;
//            			this.logt("cell " + cellId.ToString() + " - trying " + tilepattern, 2);
	            		foreach ( char c in tilepattern )
	            		{
	            			// ignore edges
	            			if ( c != '-' )
	            			{
	    						int regionId = this.pb.getRegionId(cellId, quarter);
					        	if ( !regionPatterns.ContainsKey(regionId) )
					        	{
					        		regionPatterns.Add(regionId, this.getRegionPatterns(regionId));
					        	}
//		            			this.logt("cell " + cellId.ToString() + " - quarter " + quarter.ToString() + ", regionId: " + regionId.ToString() + ", patterns: " + String.Join(",", new List<string>(regionPatterns[regionId].Keys).ToArray()), 2);
		            			if ( regionPatterns.ContainsKey(regionId) && regionPatterns[regionId].ContainsKey(c.ToString()) && regionPatterns[regionId][c.ToString()] > 1 )
		            			{
		            				this.numRegionSkips++;
//		            				this.logt("region restriction on cell " + cellId.ToString() + ", skipping tile: " + tilepattern, 2);
		            				tileFound = false;
		            				break;
		            			}
	            				// add current pattern to avoid duplicates (ie. for tiles that have double patterns)
	            				if ( regionPatterns[regionId].ContainsKey(c.ToString()) )
	            				{
		            				regionPatterns[regionId][c.ToString()]++;
	            				}
	            				else
	            				{
		            				regionPatterns[regionId].Add(c.ToString(), 1);
	            				}
	            			}
	            			quarter++;
	            		}
	            	}

	            	// skip if tile is used or if it's a hint tile
        		 	int tileId = this.alltiles[tilepattern];
//        		 	Program.TheMainForm.timer.start("isUsedOrHint");
					bool isUsedOrHint = this.usedTiles.Contains(tileId) || this.hintTiles.ContainsValue(tileId);
//        		 	Program.TheMainForm.timer.stop("isUsedOrHint");
	            	if ( !isUsedOrHint )
	            	{
//	            			this.logt("adding tile: " + tilepattern + " to queue for cell " + cellId.ToString(), 2);
//	            		Program.TheMainForm.timer.start("queueList.Add");
		            	this.queueList[cellId].Add(new TileInfo(tileId, tilepattern));
//		            	Program.TheMainForm.timer.stop("queueList.Add");
	            	}
				}
			}
//			Program.TheMainForm.timer.stop("foreach.searchtileset");
			
			// count num searches/num invalid/hits/misses
			if ( !this.searchStats.ContainsKey(search) )
			{
				this.searchStats.Add(search, new int[4]);
			}
			// num searches
			this.searchStats[search][0]++;
			// num invalid searches
			if ( !validSearch )
			{
				this.searchStats[search][1]++;
			}
			if ( this.queueList[cellId].Count > 0 )
			{
				// num hits
				this.searchStats[search][2]++;
			}
			else
			{
				// num misses
				this.searchStats[search][3]++;
			}
			
//			this.log("buildTileQueue(path: " + (this.solve_path_id+1).ToString() + ", cellId: " + cellId.ToString() + ", search: " + search + " = " + this.queueList[cellId].Count.ToString());
//			Program.TheMainForm.timer.stop("buildTileQueue");
		}
		
		public string getTileListMatchString(int cellId)
		{
//			Program.TheMainForm.timer.start("getTileListMatchString");
			int[] colrow = Board.getColRowFromPos(cellId);
			int col = colrow[0];
			int row = colrow[1];
			string match = "";
			string p = "";
//			System.Text.StringBuilder match = new System.Text.StringBuilder();

			// return id of tile list to queue tiles from
//		    if ( this.solve_method == "rows_right_down" )
			if ( this.method_right_down )
		    {
		    	// get adj pattern from left cell
		    	if ( col == 1 )
		    	{
		    		match = "-";
//		    		match.Append("-");
		    	}
		    	else
		    	{
		    		p = this.getPatternAtColRow(col - 1, row);
		    		if ( p.Length == 4 )
		    		{
		    			match = p[2].ToString();
		    		}
		    		else
		    		{
		    			// left cell is blank, so lets match on top cell
		    			match = "top_";
		    		}
//		    		match.Append(this.getPatternAtColRow(col - 1, row)[2]);
		    	}
		    	
		    	// get adj pattern from above cell
		    	if ( row == 1 )
		    	{
		    		match += "-";
//		    		match.Append("-");
		    	}
		    	else
		    	{
		    		p = this.getPatternAtColRow(col, row - 1);
		    		if ( p.Length == 4 )
		    		{
			    		match += p[3].ToString();
		    		}
		    		else
		    		{
		    			// top cell is blank, so lets match on left cell if available
		    			if ( match.Length == 1 )
		    			{
			    			match = "left_" + match;
		    			}
		    			else
		    			{
		    				// no tiles in left or top so return blank
		    				match = "";
		    			}
		    		}
//		    		match.Append(this.getPatternAtColRow(col, row - 1)[3]);
		    	}
		    }
			else if ( this.solve_method == "inner" )
			{
				if ( col == 2 && row == 2 )
				{
					match = "";
				}
		    	// row 2 - check left cell only
		    	if ( row == 2 && col > 2 )
		    	{
		    		p = this.getPatternAtColRow(col - 1, row);
		    		if ( p.Length == 4 )
		    		{
			    		match = "left_" + p[2].ToString();
		    		}
		    	}
		    	
		    	// col 2 - check top cell only
		    	else if ( col == 2 && row > 2 )
		    	{
		    		p = this.getPatternAtColRow(col, row - 1);
		    		if ( p.Length == 4 )
		    		{
			    		match = "top_" + p[3].ToString();
		    		}
		    	}
		    	
		    	else if ( col > 2 && row > 2 )
		    	{
		    		p = this.getPatternAtColRow(col - 1, row);
		    		if ( p.Length == 4 )
		    		{
			    		match = p[2].ToString();
		    		}
		    		p = this.getPatternAtColRow(col, row - 1);
		    		if ( p.Length == 4 )
		    		{
			    		match += p[3].ToString();
		    		}
		    	}
			}
//			Program.TheMainForm.timer.stop("getTileListMatchString");
		    return match;
//			return match.ToString();
		}
		
		public int loadTileQueue(int cellId)
		{
//			Program.TheMainForm.timer.start("loadTileQueue");

			// select pre-defined queue if available (for rows_right_down method)
//			if ( this.solve_method == "rows_right_down" )
			if ( this.method_right_down || this.solve_method == "inner" )
			{
				this.loadQueueForCellId(cellId);
				this.queueProgress[cellId-1] = 0;
			}
			else
			{
				// else search for matching tiles every time.. (slower)
//				this.buildTileQueue(cellId);
				// try cached tile search to improve speed
				this.buildCachedTileQueue(cellId);
			}
			
			if ( this.useRandomTileQueue )
			{
				this.randomiseQueueList(cellId);
			}
//			Program.TheMainForm.timer.stop("loadTileQueue");
			return this.queueList[cellId].Count;
		}
		
		public void loadQueueForCellId(int cellId)
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
			else if ( cellId == Board.num_cols )
			{
				qid = "corner_tr";
			}
			// corner_bl
			else if ( cellId == Board.max_tiles - (Board.num_cols - 1) )
			{
				qid = "corner_bl";
			}
			// corner_br
			else if ( cellId == Board.max_tiles )
			{
				qid = "corner_br";
			}
			// edge_top
			else if ( cellId > 1 && cellId < Board.num_cols )
			{
				qid = "edge_top";
			}
			// edge_bottom
			else if ( cellId > Board.max_tiles - (Board.num_cols - 1) && cellId < Board.max_tiles )
			{
				qid = "edge_bottom";
			}
			// edge_left
			else if ( cellId % Board.num_cols == 1 )
			{
				qid = "edge_left";
			}
			// edge_right
			else if ( cellId % Board.num_cols == 0 )
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
				string tileSearch = this.getTileListMatchString(cellId);
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
				if ( Program.TheMainForm.cbCellFilter.Checked && this.cellFilters.ContainsKey(cellId) )
				{
					this.loadQueueViaCellFilter(cellId, qid);
				}
				else
				{
					this.queueList[cellId] = new List<TileInfo>(this.tileList[qid]);
				}
			}
//			Program.TheMainForm.timer.stop("loadQueueForCellId");
//			this.timer.stop("loadQueueForCellId");
		}
		
		// loads queue list with tiles that match cell filter if enabled
		public void loadQueueViaCellFilter(int cellId, string qid)
		{
			this.queueList[cellId] = new List<TileInfo>();
			int i = 0;
			foreach ( TileInfo tile in this.tileList[qid] )
			{
				i++;
				if ( this.checkCellFilter(cellId, tile.pattern) )
				{
					this.queueList[cellId].Add(tile);
				}
			}
			this.logt("loaded " + this.queueList[cellId].Count + " / " + i + " tiles in queue for cellId " + cellId, 3);
		}

		public void _oldloadQueueForCellId(int cellId, string qid)
		{
//			Program.TheMainForm.timer.start("loadQueueForCellId");
			
			// use corner lists
			// use edge lists
			// use adj lists for internal tile list

			if ( this.tileList.ContainsKey(qid) )
			{
	//			this.queueList[cellId].InsertRange(0, this.tileList[qid]);
				foreach ( TileInfo tile in this.tileList[qid] )
				{
					// filter edge tiles where appropriate
//					string match = "....";
					bool isValid = true;
					// top left corner
					if ( cellId == 1 )
					{
//						match = "--..";
						isValid = tile.pattern.Substring(0,2) == "--" && !tile.pattern.Substring(2,2).Contains("-");
					}
					// top right corner
					else if ( cellId == Board.num_cols )
					{
//						match = ".--.";
						isValid = tile.pattern.Substring(1,2) == "--" && tile.pattern[0] != '-' && tile.pattern[3] != '-';
					}
					// bottom left corner
					else if ( cellId == 241 )
					{
//						match = "-..-";
						isValid = !tile.pattern.Substring(1,2).Contains("-") && tile.pattern[0] == '-' && tile.pattern[3] == '-';
					}
					// bottom right corner
					else if ( cellId == Board.max_tiles )
					{
//						match = "..--";
						isValid = !tile.pattern.Substring(0,2).Contains("-") && tile.pattern.Substring(2,2) == "--";
					}
					// left col
					else if ( cellId % Board.num_cols == 1 )
					{
//						match = "-...";
						isValid = tile.pattern[0] == '-' && !tile.pattern.Substring(1,3).Contains("-");
					}
					// right col
					else if ( cellId % Board.num_cols == 0 )
					{
//						match = "..-.";
						isValid = tile.pattern[2] == '-' && !tile.pattern.Substring(0,2).Contains("-") && tile.pattern[3] != '-';
					}
					// top row
					else if ( cellId > 1 && cellId < Board.num_cols )
					{
//						match = ".-..";
						isValid = tile.pattern[1] == '-' && !tile.pattern.Substring(2,2).Contains("-") && tile.pattern[0] != '-';
					}
					// bottom row
					else if ( cellId > Board.max_tiles - (Board.num_cols - 1) && cellId < Board.max_tiles )
					{
//						match = "...-";
						isValid = !tile.pattern.Substring(0,3).Contains("-") && tile.pattern[3] == '-';
					}
//					if ( Regex.IsMatch(tile.pattern, match) )
					if ( isValid )
					{
						this.queueList[cellId].Add(tile);
					}
				}
			}
//			Program.TheMainForm.timer.stop("loadQueueForCellId");
		}
		
		public bool isBorderCell(int cellId)
		{
			return cellId <= Board.num_cols || cellId % Board.num_cols == 0 || cellId % Board.num_cols == 1 || cellId >= Board.num_cols * (Board.num_rows - 1);
		}
		
		// iterate once backward
		public void stepBack()
		{
			this.logt("stepBack()", 1);
			if ( !this.queuePrev() )
			{
				this.prevCell();
			}
			else
			{
				int cellId = this.getCellId();
				TileInfo tile = this.fetchFromQueue(cellId);
				if ( tile != null && this.validPlacement(tile.pattern, cellId) )
				{
					// place tile on board
					this.placeTile(cellId, tile);
				}
			}
			this.dumpInfo();
			this.updateStats(true);
			Program.TheMainForm.board.setTileset();
			this.setModel();
			this.enableControls();
		}
		
		// iterate once forward
		public void stepForward()
		{
			this.logt("stepForward()", 1);
			this.single_step = true;
			this.resume();
			this.dumpInfo();
			this.enableControls();
		}
		
		// step back in queue
		public bool queuePrev()
		{
			bool queueLoaded = false;
			int cellId = this.getCellId();
			if ( cellId == 0 )
			{
				return false;
			}
			this.numIterations--;
			this.numStaleIterations--;

			// count border iterations
			if ( this.isBorderCell(cellId) )
			{
				this.numBorderIterations--;
			}

			if ( this.queueList.ContainsKey(cellId) && this.queueList[cellId].Count > 0 )
			{
				if ( this.queueProgress.Length >= cellId && this.queueProgress[cellId-1] > 0 )
				{
					this.queueProgress[cellId-1]--;
					queueLoaded = true;
				}
			}
			
			return queueLoaded;
		}

		public bool queueNext(int cellId)
		{
//			this.timer.start("queueNext");
			this.appflow.open(String.Format("queueNext({0})", cellId));
			bool queueLoaded = false;
			
			if ( cellId == 0 )
			{
//				this.timer.stop("queueNext");
				return false;
			}

//			Program.TheMainForm.timer.start("queueNext");
			this.numIterations++;
			
			this.numStaleIterations++;
//			this.updateIterationStatus();
			
			// count border iterations
			if ( this.isBorderCell(cellId) )
			{
				this.numBorderIterations++;
			}

			if ( this.queueList.ContainsKey(cellId) && this.queueList[cellId].Count == 0 )
			{
				// load queue if not loaded (find matching tiles)
				int queueSize = this.loadTileQueue(cellId);
				if ( queueSize > 0 )
				{
					this.queueProgress[cellId-1] = 1;
					this.logt("loaded queue for path: " + (this.solve_path_id+1).ToString() + ", cellId: " + cellId.ToString() + " = " + queueSize.ToString() + " item(s)", 3);
					queueLoaded = true;
				}
			}
			else if ( this.queueProgress.Length >= cellId )
			{
				if ( this.queueProgress[cellId-1] < this.queueList[cellId].Count )
				{
					this.queueProgress[cellId-1]++;
					queueLoaded = true;
				}
			}
			
//			Program.TheMainForm.timer.stop("queueNext");
			this.appflow.close(String.Format("queueNext({0})", cellId));
//			this.timer.stop("queueNext");
			return queueLoaded;
		}
		
		public TileInfo fetchFromQueue(int cellId)
		{
//			this.timer.start("fetchFromQueue");
//			Program.TheMainForm.timer.start("fetchFromQueue");
			TileInfo tile = null;
			if ( this.queueProgress[cellId-1] > 0 && this.queueProgress[cellId-1] <= this.queueList[cellId].Count )
			{
				tile = this.queueList[cellId][this.queueProgress[cellId-1]-1];
				/*
				if ( cellId == 137 && tile != null && this.debug_level > 0 )
				{
					System.Diagnostics.Debugger.Break();
				}
				*/
				if ( !this.checkCellFilter(cellId, tile.pattern) || !this.checkCellFilter(0, tile.pattern) )
				{
					return null;
				}
	
				// filter out used tiles
				if ( this.usedTiles.Contains(tile.id) )
				{
//					Program.TheMainForm.timer.stop("fetchFromQueue");
//					this.timer.stop("fetchFromQueue");
					return null;
				}
				
				// filter out hint tiles unless they are in the correct spot
				if ( this.hintTiles.ContainsValue(tile.id) )
				{
//					Program.TheMainForm.timer.stop("fetchFromQueue");
//					this.timer.stop("fetchFromQueue");
					return null;
				}
			}
//			Program.TheMainForm.timer.stop("fetchFromQueue");
//			this.timer.stop("fetchFromQueue");
			return tile;
		}
		
		public bool nextCell()
		{
			if ( this.numPasses == 1 && this.solve_path_id + 1 < this.solve_path.Count )
			{
				this.solve_path_id++;
//				this.log("nextCell()::" + (this.solve_path_id+1).ToString());
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public void restoreBorderPatternForCell(int cellId, string pattern)
		{
	    	// TL = left (top is handled below)
	    	if ( cellId == Board.num_cols + 2 )
	    	{
	    		this.restoreBorderPattern(pattern[0]);
	    	}

			// check if matches leftTile.right && aboveTile.bottom
	    	// top row = L
	    	if ( cellId > Board.num_cols + 2 )
	    	{
		    	// TR = right (top is handled below)
		    	if ( cellId == Board.num_cols * 2 - 1 )
		    	{
		    		this.restoreBorderPattern(pattern[2]);
		    	}
		    	// BL = bottom (left is handled below)
		    	if ( cellId == Board.num_cols * (Board.num_rows - 2) + 2 )
		    	{
		    		this.restoreBorderPattern(pattern[3]);
		    	}
		    	// BR = bottom (right is handled below)
		    	if ( cellId == Board.num_cols * (Board.num_rows - 1) - 1 )
		    	{
		    		this.restoreBorderPattern(pattern[3]);
		    	}
		    	
		    	// top row = T
		    	if ( cellId <= Board.num_cols * 2 )
		    	{
		    		this.restoreBorderPattern(pattern[1]);
		    	}
		    	// left col = L
		    	else if ( cellId % Board.num_cols == 2 )
		    	{
		    		this.restoreBorderPattern(pattern[0]);
		    	}
		    	// right col = R
		    	else if ( cellId % Board.num_cols == Board.num_cols - 1 )
		    	{
		    		this.restoreBorderPattern(pattern[2]);
		    	}
		    	// bottom row = B
		    	else if ( cellId > Board.num_cols * Board.num_rows - Board.num_cols )
		    	{
		    		this.restoreBorderPattern(pattern[3]);
		    	}
	    	}
		}
		
		public void removeTile(int cellId)
		{
//			Program.TheMainForm.timer.start("removeTile");
//			this.timer.start("removeTile");
			if ( this.board.ContainsKey(cellId) )
			{
				int tileId = this.board[cellId].id;
				
				// restore border pattern if border touching cell
				if ( this.solve_method == "inner" )
				{
					this.restoreBorderPatternForCell(cellId, this.board[cellId].pattern);
				}
				this.board.Remove(cellId);
				this.usedTiles.Remove(tileId);
			}
//			this.timer.stop("removeTile");
//			Program.TheMainForm.timer.stop("removeTile");
		}
		
		public bool prevCell()
		{
//			Program.TheMainForm.timer.start("prevCell");

//			this.log("prevCell(FROM)::" + (this.solve_path_id+1).ToString());
			// go to previous cell if available
			// returns false if no more available cells
			if ( this.solve_path_id > 0 )
			{
				int cellId = this.getCellId();
				// clear queue for current cell
				this.queueClear(cellId);
				// remove current tile from board
				this.removeTile(cellId);

				this.solve_path_id--;
//				this.log("prevCell(OK)::" + (this.solve_path_id+1).ToString());

//				Program.TheMainForm.timer.stop("prevCell");
				return true;
			}
			else
			{
//				this.log("prevCell(START)");
//				Program.TheMainForm.timer.stop("prevCell");
				return false;
			}
		}
		
		public void enableControls()
		{
			Program.TheMainForm.grS1IterationControl.Enabled = true;

			Program.TheMainForm.cbS1UseRandomSeed.Enabled = true;
			if ( Program.TheMainForm.cbS1UseRandomSeed.Checked )
			{
				Program.TheMainForm.grS1RandomTileQueue.Enabled = true;
			}

			Program.TheMainForm.cbS1EnableScoreTrigger.Enabled = true;
			if ( Program.TheMainForm.cbS1EnableScoreTrigger.Checked )
			{
				Program.TheMainForm.grS1AutoScoreTrigger.Enabled = true;
			}
			
			Program.TheMainForm.cbS1EnableBacktrackLimit.Enabled = true;
			if ( Program.TheMainForm.cbS1EnableBacktrackLimit.Checked )
			{
				Program.TheMainForm.grS1BacktrackOptions.Enabled = true;
			}

			Program.TheMainForm.cbS1EnforceFillableCells.Enabled = true;
			Program.TheMainForm.inputS1SolvePath.Enabled = true;
			Program.TheMainForm.selS1SolveMethod.Enabled = true;
			Program.TheMainForm.inputS1CurrentSolveMethod.Enabled = true;

			Program.TheMainForm.btStartS1.Enabled = true;
			Program.TheMainForm.btPauseS1.Enabled = false;
			Program.TheMainForm.btResumeS1.Enabled = true;
			Program.TheMainForm.btS1Reset.Enabled = true;
//			Program.TheMainForm.tabSolver1.Update();
		}
		
		public void disableControls()
		{
			Program.TheMainForm.grS1IterationControl.Enabled = false;
			Program.TheMainForm.grS1RandomTileQueue.Enabled = false;
			Program.TheMainForm.grS1AutoScoreTrigger.Enabled = false;
			Program.TheMainForm.grS1BacktrackOptions.Enabled = false;
			Program.TheMainForm.cbS1EnforceFillableCells.Enabled = false;
			Program.TheMainForm.cbS1EnableBacktrackLimit.Enabled = false;
			Program.TheMainForm.cbS1UseRandomSeed.Enabled = false;
			Program.TheMainForm.cbS1EnableScoreTrigger.Enabled = false;
			Program.TheMainForm.inputS1SolvePath.Enabled = false;
			Program.TheMainForm.selS1SolveMethod.Enabled = false;
			Program.TheMainForm.inputS1CurrentSolveMethod.Enabled = false;

			Program.TheMainForm.btStartS1.Enabled = false;
			Program.TheMainForm.btPauseS1.Enabled = true;
			Program.TheMainForm.btResumeS1.Enabled = false;
			Program.TheMainForm.btS1Reset.Enabled = false;
//			Program.TheMainForm.tabSolver1.Update();
		}

		// start solve loop
		private void solve()
		{
			CAF_Application.log.add(LogType.INFO, "solve()");
			                        
			this.appflow.open("solve()");
			if ( !this.isResumed )
			{
				if ( !this.reset() )
				{
					return;
				}

//				Program.TheMainForm.timer.start("solve");
				if ( !this.run_multiple )
				{
					if ( this.useRandom )
					{
						this.logt("*** " + Board.title + " : START SEED " + this.seed.ToString() + ", " + Program.TheMainForm.inputS1CurrentSolveMethod.Text + ", ITERATION LIMIT = " + this.maxIterations.ToString(), 1);
					}
					else
					{
						this.logt("*** " + Board.title + " : START " + Program.TheMainForm.inputS1CurrentSolveMethod.Text + ", ITERATION LIMIT = " + this.maxIterations.ToString(), 1);
					}
				}
			}

			this.disableControls();
			this.setupIterationTrigger();
			this.setupAutoScoreAction();
			this.setupBacktrackTrigger();
			
			// reset tile stats
			if ( CAF_Application.config.getValue("reset_tilestats_each_iteration") == "1" )
			{
				this.resetTileStats();
			}
	
			// set path filter
			this.pb.pathFilter(this.pb.selected_filter);
			this.pb.excludeCells(this.hintTiles.Keys);
			this.solve_path = this.pb.path;

			// activate tileswap on start if specified
			if ( CAF_Application.config.getValue("tileswap_on_start") == "1" )
			{
				this.runTileswap();
				return;
			}

			// if path longer than number of tiles, reduce to size..
			// handle hintTiles ?
			// this.hintTiles.Count
			/*
			if ( this.solve_path.Count > this.alltiles.Count / 4 )
			{
				this.solve_path.RemoveRange(this.board.Count, this.solve_path.Count - this.alltiles.Count / 4);
			}
			*/
			
			if ( this.solve_path.Count == 0 )
			{
				System.Windows.Forms.MessageBox.Show("Blank solve path, please enter/choose a path.", "Blank Solve Path", System.Windows.Forms.MessageBoxButtons.OKCancel);
				this.enableControls();
				this.pause();
				return;
			}
			
			bool done = false;
			bool backtrack = false;
			int cellId = 0;
//			Program.TheMainForm.timer.start("iteration");
			this.start_time = DateTime.Now.Ticks;
			while ( !done )
			{
				this.appflow.comment("*** solve_loop_start");
//				Program.TheMainForm.timer.start("solve_loop");
//				Program.TheMainForm.timer.start("solve_trigger-cell");
				if ( this.isPaused || this.isStopped )
				{
					break;
				}
				
				if ( this.isNextRun )
				{
					// if useRandom then just reset queue & change seed instead of re-loading tilelist etc
					if ( this.useRandom )
					{
						this.logt("HIGHEST SCORE: " + this.mostTilesPlaced.ToString() + " / " + this.solve_path.Count + this.hintTiles.Count + " @ ITERATION " + this.bestIteration + ", SEED : " + this.seed + ", RUN : " + this.last_run + 1, 1);
						this.mostTilesPlaced = 0;
						this.solve_path_id = 0;
						this.board = new SortedDictionary<int, TileInfo>();
						this.usedTiles = new HashSet<int>();
						this.initQueues();
						this.setupRandomGenerator();
						this.maxIterations += Convert.ToInt64(Program.TheMainForm.inputS1MaxIterations.Text);
						this.isNextRun = false;
						this.last_run++;
						if ( this.num_runs > 0 && this.last_run >= this.num_runs )
						{
							this.pause();
							break;
						}
					}
					//break;
				}
				
				// handle iteration trigger if enabled
				this.iterationTrigger();

				// back track jump if enabled
				if ( this.backtrack_empty_cell )
				{
					this.queueJumpStart();
				}
					
				// get cell from solve_path
				cellId = this.getCellId();
				/*
				if ( cellId == 32 )
				{
					System.Diagnostics.Debugger.Break();
				}
				*/
				
				// check if empty internal cell due to path filter, if so, skip to next cell
//				string cellmatch = this.getTileMatchString(cellId);
//				this.logt("cell " + cellId + ", match: " + cellmatch, 1);
				
//				Program.TheMainForm.timer.stop("solve_trigger-cell");
				// fetch next tile from queue
//				Program.TheMainForm.timer.start("solve_queueplace");
				if ( this.queueNext(cellId) )
				{
					backtrack = false;
					// fetch tile from queue
					this.removeTile(cellId);
					TileInfo tile = this.fetchFromQueue(cellId);
					if ( tile != null && this.validPlacement(tile.pattern, cellId) )
					{
						// place tile on board
						this.placeTile(cellId, tile);
						
						// update max score
						this.updateMaxScore();

						// handle auto score trigger
						this.autoscoreTrigger();

						// check if any unfillable cells if enabled
						bool validCheckpoint = true;
						if ( this.checkUnfillableCells() )
						{
//							if ( !this.validateCheckpoint(cellId) )
							{
								// remove tile
								this.removeTile(cellId);
								validCheckpoint = false;
							}
						}

						if ( validCheckpoint )
						{
							if ( this.board.Count == this.solve_path.Count + this.hintTiles.Count || this.board.Count == Board.max_tiles )
							{
								this.handleSolved();
								
								// check on_solve events
								bool stop = false;
								switch ( CAF_Application.config.getValue("on_solve") )
								{
									case "pause":
										stop = true;
										this.pause();
										break;
									case "next_iteration":
										stop = true;
										done = true;
										break;
									case "jump_start":
                                        this.logt("on_solve: jump_start", 1);
                                        this.queueJumpStart();
										break;
									case "back_pedal":
										this.logt("on_solve: backpedal", 1);
                                        this.queueBackPedal();
										break;
								}
								if ( stop )
								{
									// exit loop/iteration
									break;
								}
								// else continue processing for more solutions
								// don't backtrack yet as need to exhaust current queue
							}
							else if ( !this.nextCell() )
							{
								this.logt("*** ENDED PASS @ SEED " + this.seed + ", ITERATION " + this.numIterations.ToString() + ", HASH = " + this.getModelHash(), 1);
								
								// log numQueueJumps
								this.logt("numQueueJumps: " + this.numQueueJumps, 2);
								// log numStaleIterations
								this.logt("numStaleIterations: " + this.numLastStaleIterations, 2);
							}
						}
					}
				}
				else
				{
//					this.log("queue(" + cellId.ToString() + ") returned empty tile - backtracking");
					backtrack = true;
				}
//				Program.TheMainForm.timer.stop("solve_queueplace");
//				Program.TheMainForm.timer.start("solve_backtrack");
				if ( backtrack )
				{

					// backtrack limit trigger
					if ( this.checkBacktrackTrigger() )
					{
						this.backtrackLastTriggeredIteration = this.numIterations;
						if ( this.backtrack_skip_cell  )
						{
							if ( !this.empty_cells.Contains(cellId) )
							{
								this.empty_cells.Add(cellId);
							}
							if ( this.first_empty_cell == 0 && this.solve_path_id > 0 )
							{
								// set first empty cell to cell preceding first empty cell (to advance queue upon backtrack)
								this.first_empty_cell = this.solve_path_id-1;
							}
							this.log("");
							this.logt("backtrack trigger - skipping cell " + (this.solve_path_id+1).ToString() + ". cellId " + cellId.ToString(), 2);
							this.queueClear(cellId);
							// end of tile skip cycle
							if ( !this.nextCell() )
							{
								// log score in iteration log
								// iteration,score
								this.iterationLog.Add(String.Format("{0},{1}", this.numIterations.ToString(), this.board.Count));

								// set flag to backtrack to first empty cell upon next iteration
								this.backtrack_empty_cell = true;

								if ( CAF_Application.config.getValue("tileswap_on_skip_cell") == "1" )
								{
									// activate tileswap
									this.runTileswap();
								}
								
								// pause at end of cycle if enabled
								if ( Program.TheMainForm.cbPauseEndCycle.Checked )
								{
									this.pause();
									break;
								}
							}
						}
						else if ( this.backtrack_pause )
						{
							this.pause();
							break;
						}
						else if ( this.backtrack_queue_jump )
						{
							// queue jumper!
							this.queueJumpStart();
						}
						else if ( this.backtrack_back_pedal )
						{
							// back pedal queue
							this.queueBackPedal();
						}
					}
					else
					{
						// abort if backtrack limit reached
						int backtrackDepth = this.mostTilesPlaced - this.board.Count;
						if ( this.backtrackDepthLimit > 0 && backtrackDepth >= this.backtrackDepthLimit )
						{
							this.logt("*** ABORTING at backtrack depth " + backtrackDepth, 1);
							this.updateStats(true);
							done = true;
							break;
						}

						// normal backtrack action
//						this.log("");
//						this.log("backtrack from " + (this.solve_path_id+1).ToString() + ". cellId " + cellId.ToString());
	
						// backtrack
						if ( !this.prevCell() )
						{
							this.logt("no more backtrack options - exiting", 1);
							done = true;
							break;
						}
						else
						{
							// reset stale iterations if score drops below min threshold
							if ( this.backtrackMinScore > 0 && this.board.Count <= this.backtrackMinScore )
							{
								this.resetStaleIterations();
							}
							
//							cellId = this.getCellId();
//							this.log("backtrack to " + (this.solve_path_id+1).ToString() + ". cellId " + cellId.ToString());
						}

						// handle auto score trigger (for pause action only)
						if ( !this.auto_score_save )
						{
							this.autoscoreTrigger();
						}

					}

				}
//				Program.TheMainForm.timer.stop("solve_backtrack");
//				Program.TheMainForm.timer.start("solve_updateStats");
				// update stats / score
//				new Thread(this.updateStats).Start();
				this.updateStats(false);
				
				if ( this.single_step )
				{
					this.single_step = false;
					this.isPaused = true;
					break;
				}
//				Program.TheMainForm.timer.stop("solve_updateStats");
//				Program.TheMainForm.timer.stop("solve_loop");
				this.appflow.comment("*** solve_loop_end");
			}
			
			int maxCells = this.solve_path.Count + this.hintTiles.Count;
			this.logt("HIGHEST SCORE: " + this.mostTilesPlaced.ToString() + " / " + maxCells.ToString() + " @ ITERATION " + this.bestIteration + ", SEED : " + this.seed + ", RUN : " + this.last_run + 1, 1);
			
			if ( this.isPaused )
			{
				this.updateStats(true);
				this.logt("*** PAUSED " + this.getSeedInfo() + " @ " + this.numIterations.ToString(), 1);
				Program.TheMainForm.board.setTileset();
				this.setModel();
				this.logStats();
				this.dumpInfo();
				Program.TheMainForm.tabSolver1.Update();
				return;
			}

			Program.TheMainForm.labelSpeed.Text = "STOPPED";
			this.logt("*** STOPPED " + this.getSeedInfo() + " after " + String.Format("{0:#,#}", this.numIterations) + " iterations", 1);
			// log iteration speed
//				double elapsed = Program.TheMainForm.timer.elapsed("iteration");
//				double speed = this.speedInterval / elapsed;
//				double speed = this.num_overall_tiles_placed / Program.TheMainForm.timer.elapsed("solve");
//				this.logt(String.Format("placed {0:#,#} tile(s) @ {1:#,#.0} tiles / second", Program.TheMainForm.timer.stats["placeTile"].num_iterations, speed), 1);

			// update stats / score
			this.logStats();
			this.updateStats(true);
			
			if ( this.isNextRun )
			{
				this.isNextRun = false;
			}

			if ( !this.run_multiple )
			{
				this.enableControls();
				this.saveResults();
				this.saveLog();
			}

//			Program.TheMainForm.timer.stop("solve");
//			this.logt(Program.TheMainForm.timer.results("solve"), 1);
			this.logl("", 1);
			
			this.logl(Program.TheMainForm.timer.getStats(), 2);

			this.appflow.close("solve()");
			if ( !this.run_multiple )
			{
				// log application flow
				this.logl(this.appflow.getLog(), 3);
			}
		}
		
		public void logStats()
		{
			this.end_time = DateTime.Now.Ticks;
			this.time_elapsed = (this.end_time - this.start_time) / 10000000.0;
			double qspeed = this.numIterations / this.time_elapsed;
			double tspeed = this.num_overall_tiles_placed / this.time_elapsed;

			this.logt(String.Format("Found {0:#,#} unique / {1:#,#} solution(s)", this.uniqueSolutions.Count, this.solutionStats.Count), 1);
			this.logt(String.Format("queued {0:#,#} tile(s) @ {1:#,#.0} tiles / second", this.numIterations, qspeed), 1);
			this.logt(String.Format("placed {0:#,#} tile(s) @ {1:#,#.0} tiles / second", this.num_overall_tiles_placed, tspeed), 1);

			this.logt(String.Format("numQueueJumps: {0:#,#}", this.numQueueJumps), 2);
			this.logt(String.Format("numStaleIterations: {0:#,#}", this.numLastStaleIterations), 2);
			this.logt(String.Format("number of border iterations: {0:#,#}", this.numBorderIterations), 1);
			this.logt(String.Format("number of border slip misses: {0:#,#}", this.numBorderSlipMisses), 1);
			
//			this.logt(this.timer.getStats(), 1);
		}
		
		public bool checkBacktrackTrigger()
		{
			bool backtrackTriggerEnabled = false;
			bool failedCriteria = false;
			if ( this.backtrack_trigger_enabled )
			{
				// check if backtrack trigger warranted
				// score only checked for first instance of backtrack trigger
//				if ( this.backtrackMinScore > 0 && this.backtrackLastTriggeredIteration == 0 && this.board.Count < this.backtrackMinScore )
				if ( this.backtrackMinScore > 0 )
				{
					if ( this.board.Count >= this.backtrackMinScore )
					{
						backtrackTriggerEnabled = true;
					}
					else
					{
						failedCriteria = true;
					}
				}
				if ( this.backtrackMinIterations > 0 )
				{
					if ( this.numIterations >= this.backtrackMinIterations )
					{
						backtrackTriggerEnabled = true;
					}
					else
					{
						failedCriteria = true;
					}
					/*
					if ( this.backtrackMinIterations > 0 && this.numIterations - this.backtrackLastTriggeredIteration >= this.backtrackMinIterations )
					{
						backtrackTriggerEnabled = true;
					}
					*/
				}
				if ( this.backtrackStaleIterations > 0 )
				{
					if ( this.numStaleIterations >= this.backtrackStaleIterations )
					{
						backtrackTriggerEnabled = true;
					}
					else
					{
						failedCriteria = true;
					}
				}
				// dont skip cell if max empty cells reached
				if ( this.backtrack_skip_cell && this.empty_cells.Count >= this.max_empty_cells )
				{
					failedCriteria = true;
				}
			}
			if ( backtrackTriggerEnabled && !failedCriteria )
			{
				this.logt("backtrack trigger enabled @ iteration " + this.numIterations.ToString() + ", backtrack depth: " + (this.mostTilesPlaced - this.board.Count).ToString(), 3);
				this.resetStaleIterations();
			}
			return backtrackTriggerEnabled && !failedCriteria;
		}
		
		public void start()
		{
			Program.TheMainForm.labelSpeed.Text = "STARTED";
			Program.TheMainForm.timer = new Timer();
			this.setDebugLevel();
			this.loadQueueList();
			CAF_Application.config.setConfiguration(Program.TheMainForm.tbConfig.Lines);
			
			// clear log / stats
			if ( !this.run_multiple )
			{
				Program.TheMainForm.logS1.Clear();
				this.clearSolutions();
			}

			// run solver multiple times using random seeds or queue jump, etc
			this.num_runs = 1;
			this.last_run = 0;
			
			this.uniqueSolutions = new List<string>();
			this.totalSolutions = new List<string>();
			this.uniquePatternOrientations = new SortedDictionary<string, string>();
			this.solutionStats = new List<string>();
			this.iterationLog = new List<string>();
			this.scoreStats = new Dictionary<int, long>(Board.max_tiles);
			this.maxStaleIterations = 0;
			this.searchStats = new Dictionary<string, int[]>();
			
			// reset tile stats
			this.resetTileStats();

			// reset tile distribution table
			this.tileDistribution.Clear();
			for ( int cellId = 1; cellId <= Board.max_tiles; cellId++ )
			{
				this.tileDistribution.Add(cellId, new HashSet<string>());
			}
			
			this.patternDistribution.Clear();
			
			this.appflow = new AppFlowLog();
			this.checkRandomSettings();
			this.setupIterationTrigger();
            this.run_multiple = num_runs != 1 && this.useRandom;

            if ( this.run_multiple )
			{
				if ( this.num_runs == 0 )
				{
					bool done = false;
					int i = 0;
					while ( !done )
					{
						i++;
						this.solve();
						this.last_run = i;
						if ( this.isPaused || this.isStopped )
						{
							break;
						}
					}
				}
				else
				{
					for ( Int32 i = 1; i <= this.num_runs; i++ )
					{
						this.solve();
						this.last_run = i;
						if ( this.isPaused || this.isStopped )
						{
							break;
						}
					}
				}
			}
			else
			{
				this.solve();
			}
			this.enableControls();
			this.saveResults(true);
			this.saveLog();
		}

		public void pause()
		{
			this.isPaused = true;
			this.isResumed = false;
			this.updateStats(true);
			this.enableControls();
			Program.TheMainForm.labelSpeed.Text = "PAUSED";
		}
		
		public void stop()
		{
			this.isStopped = true;
			this.isPaused = false;
			this.isResumed = false;
			this.enableControls();
			// log application flow
			this.logl(this.appflow.getLog(), 3);
			Program.TheMainForm.labelSpeed.Text = "STOPPED";
			
			if ( this.solver3 != null )
			{
				CAF_Application.config.setValue("runtime_is_stopped", "1");
				Application.DoEvents();
			}
		}
		
		public void resume()
		{
			this.isPaused = false;
			this.isResumed = true;
			this.isStopped = false;
			this.logt("*** RESUMED " + this.getSeedInfo() + " @ " + this.numIterations.ToString(), 1);
			Program.TheMainForm.labelSpeed.Text = "RESUMED";

            if ( this.run_multiple )
			{
				bool firstResumed = true;

				if ( this.num_runs == 0 )
				{
					bool done = false;
					int i = 0;
					this.logt("RESUMING UNLIMITED RUN, SEED " + this.seed.ToString() + " @ iteration " + this.numIterations, 1);
					while ( !done )
					{
						i++;
						this.solve();
						this.last_run = i;
						this.isResumed = false;
						if ( this.isPaused || this.isStopped )
						{
							break;
						}
					}
				}
				else
				{
					for ( Int32 i = this.last_run; i <= this.num_runs - this.last_run; i++ )
					{
						if ( firstResumed )
						{
							this.logt("RESUMING RUN #" + i.ToString() + ", SEED " + this.seed.ToString() + " @ iteration " + this.numIterations, 1);
							firstResumed = false;
						}
						this.solve();
						this.last_run = i;
						// revert back to normal operation
						this.isResumed = false;
						if ( this.isPaused || this.isStopped )
						{
							break;
						}
					}
				}
			}
			else
			{
				this.solve();
			}
		}
		
		public string getQueueProgress()
		{
			int queueUsed = 0;
			int queueSize = 0;
			if ( this.queueProgress.Length == this.queueList.Count )
			{
				foreach ( int cellId in this.queueList.Keys )
				{
					queueUsed += this.queueProgress[cellId-1];
					queueSize += this.queueList[cellId].Count;
				}
			}
			else
			{
//				this.logl("Queue progress size / queueList length mismatch. queueProgress.Count: " + this.queueProgress.Length.ToString() + ", queueList.Count: " + this.queueList.Count.ToString(), 1);
			}
			string rv = queueUsed.ToString() + " / " + queueSize.ToString();
			return rv;
		}
		
		public void dumpStats()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : SOLVE STATS");
			stats.Add("tileset: " + Board.title);
			stats.Add("method: " + Program.TheMainForm.inputS1CurrentSolveMethod.Text);
			if ( this.useRandom )
			{
				stats.Add("seed: " + this.seed.ToString());
			}
			else
			{
				stats.Add("seed: none");
			}
			stats.Add("run #: " + this.last_run + 1);
			stats.Add("iteration #: " + this.numIterations.ToString());
			stats.Add("score: " + this.calculateScore());
			stats.Add("board.Count: " + this.board.Count.ToString());
			stats.Add("usedTiles.Count: " + this.usedTiles.Count.ToString());
			stats.Add("mostTilesPlaced: " + this.mostTilesPlaced.ToString());
			stats.Add("hintTiles.Count: " + this.hintTiles.Count.ToString());
			stats.Add("countUnfillableCells(): " + this.countUnfillableCells().ToString());
			stats.Add("solve_path.Count: " + this.solve_path.Count.ToString());
			stats.Add("queueList.Count: " + this.queueList.Count.ToString());
			stats.Add("queueProgress: " + this.getQueueProgress());
			stats.Add("region skips: " + this.numRegionSkips.ToString());
			stats.Add("numQueueJumps: " + this.numQueueJumps.ToString());
			stats.Add("numLastStaleIterations: " + this.numLastStaleIterations.ToString());
			stats.Add("numStaleIterations: " + this.numStaleIterations.ToString());
			stats.Add("maxStaleIterations: " + this.maxStaleIterations.ToString());

			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}
		
		public string getScores()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			
			stats.Add(timestamp + " : Score Stats @ " + this.numIterations + " iteration(s)");
			stats.Add(timestamp + " : board " + Board.title);
			if ( this.useRandom && this.seed > 0 )
			{
				stats.Add(timestamp + " : solve seed = " + this.seed);
			}
			else
			{
				stats.Add(timestamp + " : solve seed = N/A");
			}
			stats.Add(timestamp + " : solve method = " + this.solve_method);
			for ( int score = Board.max_tiles; score > 0; score-- )
			{
				if ( this.scoreStats.ContainsKey(score) )
				{
					stats.Add(score.ToString() + ": " + this.scoreStats[score]);
				}
			}
			return String.Join("\r\n", stats.ToArray());
		}

		public void dumpScores()
		{
			Program.TheMainForm.logS1Stats.Text = this.getScores();
			Program.TheMainForm.logS1Stats.Update();
		}
		
		public string getSearchStats()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			
			stats.Add(timestamp + " : Search Stats @ " + this.numIterations + " iteration(s)");
			stats.Add(timestamp + " : board " + Board.title);
			if ( this.useRandom && this.seed > 0 )
			{
				stats.Add(timestamp + " : solve seed = " + this.seed);
			}
			else
			{
				stats.Add(timestamp + " : solve seed = N/A");
			}
			stats.Add(timestamp + " : solve method = " + this.solve_method);
			stats.Add(timestamp + " : num search strings = " + this.searchStats.Count);
			stats.Add("searchString,numSearches,numInvalidSearches,numHits,numMisses");
			foreach ( string search in this.searchStats.Keys )
			{
				stats.Add(String.Format("{0},{1},{2},{3},{4}", search, this.searchStats[search][0], this.searchStats[search][1], this.searchStats[search][2], this.searchStats[search][3]));
			}
			return String.Join("\r\n", stats.ToArray());
		}

		public void dumpSearchStats()
		{
			Program.TheMainForm.logS1Stats.Text = this.getSearchStats();
			Program.TheMainForm.logS1Stats.Update();
		}
		
		public string getTileStats()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			
			stats.Add(timestamp + " : Tile Stats @ " + this.numIterations + " iteration(s)");
			stats.Add(timestamp + " : board " + Board.title);
			if ( this.useRandom && this.seed > 0 )
			{
				stats.Add(timestamp + " : solve seed = " + this.seed);
			}
			else
			{
				stats.Add(timestamp + " : solve seed = N/A");
			}
			stats.Add(timestamp + " : solve method = " + this.solve_method);
			int numUsed = 0;
			for ( int tileId = 1; tileId <= Board.max_tiles; tileId++ )
			{
				if ( this.tileStats[tileId] > 0 )
				{
					numUsed++;
				}
			}
			stats.Add(timestamp + " : num tiles used = " + numUsed);
			stats.Add("tileId,numTimesUsed");
			for ( int tileId = 1; tileId <= Board.max_tiles; tileId++ )
			{
				stats.Add(String.Format("{0},{1}", tileId.ToString(), this.tileStats[tileId]));
			}
			return String.Join("\r\n", stats.ToArray());
		}

		public void dumpTileStats()
		{
			Program.TheMainForm.logS1Stats.Text = this.getTileStats();
			Program.TheMainForm.logS1Stats.Update();
		}

		public void dumpIterationLog()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : iteration log:\r\n" + String.Join("\r\n", this.iterationLog.ToArray()));
			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
			
		}
		
		public void dumpSolutionStats()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : solution stats:\r\n" + String.Join("\r\n", this.solutionStats.ToArray()));
			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}

		public void dumpQueue(bool dumpTiles)
		{
//			this.generateStats();
			List<string> stats = new List<string>();
			// path / queue dump
			// show queue stats in solve_path order
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : PATH / QUEUE DUMP");
			stats.Add("cells excluded from path: " + this.pb.getExcludedCellsAsString());

			for ( int pid = 1; pid <= this.solve_path.Count; pid++ )
			{
				int cellId = this.solve_path[pid-1];
				if ( this.queueProgress.Length >= cellId && this.queueList.ContainsKey(cellId) )
				{
					List<string> line = new List<string>();
					List<string> queueTiles = new List<string>();
					foreach ( TileInfo tile in this.queueList[cellId] )
					{
						queueTiles.Add(tile.pattern);
					}
					line.Add("path: " + pid.ToString());
					line.Add("cell: " + cellId.ToString());
					line.Add("queue: " + this.queueProgress[cellId-1].ToString() + " / " + this.queueList[cellId].Count.ToString());
					if ( dumpTiles )
					{
						string[] tilequeue = queueTiles.ToArray();
						int qp = this.queueProgress[cellId-1];
						if ( qp > 0 )
						{
							tilequeue[qp-1] = ">" + tilequeue[qp-1];
						}
						line.Add(String.Join(", ", tilequeue));
					}
					stats.Add(String.Join(", ", line.ToArray()));
				}
			}
			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}

		public void dumpUsedTiles()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : Used Tiles");
			foreach ( int tileId in this.usedTiles )
			{
				TileInfo tile = this.tileset[tileId];
				stats.Add(tile.title);
			}
			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}
		
		public void dumpBoard()
		{
//			this.generateStats();
			List<string> stats = new List<string>();
			// board dump
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : BOARD DUMP");
			foreach ( int cellId in this.board.Keys )
			{
				TileInfo tile = this.board[cellId];
				stats.Add("cell: " + cellId.ToString() + ", tile: " + tile.title);
			}
			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}
		
		public string getModelHash()
		{
			List<string> data = new List<string>();
			foreach ( int cellId in this.board.Keys )
			{
				TileInfo tile = this.board[cellId];
				data.Add(tile.pattern);
			}
			string rv = Utils.getSHA1(String.Join("\r\n", data.ToArray()));
			return rv;
		}
		
		public void dumpTileset()
		{
			// dump tileset
			List<string> dump = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			dump.Add(timestamp + " : TILESET DUMP - SEED " + this.seed.ToString());
			foreach ( string pattern in this.searchtileset )
			{
				dump.Add("pattern: " + pattern + ", tileId: " + this.alltiles[pattern].ToString());
			}
			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", dump.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}
		
		public string saveModel()
		{
			string filename = this.getModelFilename();
			string model = this.getModel();
			if ( this.storeQueueInMemory )
			{
				this.modelFilename = "models\\" + filename;
				this.modelData = model;
			}
			else
			{
				System.IO.File.WriteAllText("models\\" + filename, model);
				this.log("saved model with " + this.board.Count.ToString() + " tile(s) to: " + filename);
//				Program.TheMainForm.board.saveModelImage(filename);
			}
			return filename;
		}
		
		public void saveSolution()
		{
			string dirpath = "solutions\\" + Board.title;
			if ( !System.IO.File.Exists(dirpath) )
			{
				System.IO.Directory.CreateDirectory(dirpath);
			}
			string model = this.getModel();
			string numTiles = (this.usedTiles.Count + this.hintTiles.Count).ToString();
			string filename = System.IO.Path.Combine(dirpath, this.solve_method + "_" + numTiles + "_" + this.getModelHash() + ".txt");
			System.IO.File.WriteAllText(filename, model);
			this.log("saved solution with " + this.board.Count.ToString() + " tile(s) to: " + filename);
		}
		
		public string getModelFilename()
		{
			return Board.title + "-solve1-seed" + this.seed + "-" + Program.TheMainForm.inputS1CurrentSolveMethod.Text + "-it" + this.numIterations + "-" + this.board.Count + "tiles.txt";
		}
		
		public string getModel()
		{
			List<string> model = new List<string>();
			for ( int cellId = 1; cellId <= Board.max_tiles; cellId++ )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				if ( this.board.ContainsKey(cellId) )
				{
					TileInfo tile = this.board[cellId];
					List<string> line = new List<string>();
					// v1 col,row,tileId,rotation
					/*
					line.Add(colrow[0].ToString());
					line.Add(colrow[1].ToString());
					line.Add(tile.id.ToString());
					line.Add(this.getTileRotationByPattern(tile.pattern).ToString());
					*/

					// v2 format col,row,pattern
					line.Add(colrow[0].ToString());
					line.Add(colrow[1].ToString());
					line.Add(tile.pattern);
					
//					this.log(String.Join(",", line.ToArray()));
					model.Add(String.Join(",", line.ToArray()));
				}
			}
			return String.Join("\r\n", model.ToArray());
		}

		public void setModel()
		{
			Program.TheMainForm.board.setLayout();
//			HACK stop overriding newly created random board
//			Program.TheMainForm.board.loadTileSet(Board.title);
			Program.TheMainForm.textModel.Text = this.getModel();
			Program.TheMainForm.textModel.Update();
			Program.TheMainForm.board.setModel();
		}

		public int getTileRotationByPattern(string pattern)
		{
			int i = 0;
			foreach ( var tile in this.alltiles )
			{
				if ( tile.Key == pattern )
				{
					return i % 4 + 1;
				}
				i++;
			}
			this.logt("getTileRotationByPattern(" + pattern + ") - rotation not found", 1);
			return 1;
		}
		
		public void setupIterationTrigger()
		{
			// number of runs
            if ( Program.TheMainForm.inputS1RunLength.Text != "" )
            {
            	this.num_runs = Convert.ToInt32(Program.TheMainForm.inputS1RunLength.Text);
            }

			// auto save interval
			if ( Program.TheMainForm.cbS1AutoSaveByInterval.Checked && Program.TheMainForm.inputS1AutoSaveIterations.Text != "" )
			{
				this.autosave_enable_iteration = true;
				this.autosave_iteration = Convert.ToInt32(Program.TheMainForm.inputS1AutoSaveIterations.Text);
				if ( !this.isResumed )
				{
					this.lastSaveIteration = 0;
				}
			}
			else
			{
				this.autosave_enable_iteration = false;
			}
			
			// auto pause/stop interval
			if ( Program.TheMainForm.cbS1IterationLimit.Checked && Program.TheMainForm.inputS1MaxIterations.Text != "" )
			{
				this.maxIterations = Convert.ToInt64(Program.TheMainForm.inputS1MaxIterations.Text);
			}
			else
			{
				this.maxIterations = 0;
			}
			
			// score proximity
			if ( Program.TheMainForm.cbS1IterationLimitScoreProximity.Checked && Program.TheMainForm.inputS1IterationScoreProximity.Text != "" )
			{
				this.iterationScoreProximity = Convert.ToInt16(Program.TheMainForm.inputS1IterationScoreProximity.Text);
			}
			else
			{
				this.iterationScoreProximity = 0;
			}
			
		}
		
		public void iterationTrigger()
		{
			if ( this.autosave_enable_iteration && this.autosave_iteration > 0 )
			{
				// auto save every xx iterations
				if ( this.lastSaveIteration == 0 )
				{
					this.lastSaveIteration = this.numIterations;
				}
				if ( this.numIterations - this.lastSaveIteration >= this.autosave_iteration )
				{
					this.lastSaveIteration = this.numIterations;
					this.logt("*** AUTO SAVE @ ITERATION " + this.numIterations.ToString(), 1);
					this.saveQueue();
				}
			}
			
			// auto stop/pause
			if ( this.maxIterations > 0 && this.numIterations >= this.maxIterations )
			{
				if ( this.iterationScoreProximity > 0 && this.mostTilesPlaced - this.board.Count <= this.iterationScoreProximity )
				{
					// run for 10% more iterations if score remains near high score
					int extendedIterations = (int)(this.maxIterations * 0.10);
					this.maxIterations += extendedIterations;
					// TODO - restore original max iterations from user input OR display max iterations in text label
					Program.TheMainForm.inputS1MaxIterations.Text = this.maxIterations.ToString();
					this.logt("extending iterations by " + extendedIterations.ToString() + ", score of " + this.board.Count.ToString() + " within " + this.iterationScoreProximity.ToString() + " of high score: " + this.mostTilesPlaced, 2);
				}
				else if ( this.run_multiple )
				{
					this.isNextRun = true;
				}
				else
				{
					this.pause();
				}
			}
		}
			
		public void autoscoreTrigger()
		{
			if ( CAF_Application.config.contains("tileswap_on_num_tiles") )
			{
				if ( this.board.Count >= Convert.ToInt16(CAF_Application.config.getValue("tileswap_on_num_tiles")) )
				{
					this.runTileswap();
				}
			}
			
			// auto save based on score/# tiles placed
			if ( this.auto_score_trigger_enabled && this.board.Count == this.auto_score_next_trigger )
			{
				this.logt("Score Triggered (" + this.board.Count.ToString() + " tiles placed) @ iteration " + this.numIterations.ToString() + ", seed " + this.seed, 2);
			
				// autosave queue when new score reached
				if ( this.auto_score_save )
				{
					System.Media.SystemSounds.Beep.Play();
					this.saveQueue();
					this.logt("*** AUTO SAVE @ iteration " + this.numIterations.ToString() + " - SCORE = " + this.board.Count.ToString() + ", hash = " + this.getModelHash(), 1);
				}
				
				// auto pause when trigger score reached
				if ( this.auto_score_pause )
				{
					System.Media.SystemSounds.Beep.Play();
					this.logt("AUTO PAUSE @ iteration " + this.numIterations.ToString() + ", SCORE = " + this.board.Count.ToString() + ", hash = " + this.getModelHash(), 1);
					this.pause();
				}

				// increase next auto save score trigger if enabled
				if ( this.auto_score_increment )
				{
					this.auto_score_next_trigger++;
					Program.TheMainForm.labelS1NextScoreTrigger.Text = this.auto_score_next_trigger.ToString();
					Program.TheMainForm.labelS1NextScoreTrigger.Update();
				}

				// check for magic border
				if ( Program.TheMainForm.cbShowMagicBorders.Checked )
				{
					List<string> tiles = new List<string>();
					foreach ( TileInfo tile in this.board.Values )
					{
						if ( tile != null )
						{
							tiles.Add(tile.pattern);
						}
					}
					if ( this.isMagicBorder(tiles.ToArray()) )
					{
						this.logt("Magic Border @ iteration " + this.numIterations, 1);
						System.Media.SystemSounds.Beep.Play();
	//					this.pause();
					}
				}
	
			}
		}

		public void saveQueue()
		{
			string solvemethod = "custom";
			if ( this.solve_method != "" )
			{
				solvemethod = this.solve_method;
			}
			
			List<string> data = new List<string>();
			List<string> line;
			
			// solver method
			data.Add(solvemethod);
			
			// save board
			data.Add(this.saveModel());

			// solve_path_id, solvePath
			line = new List<string>();
			line.Add(this.solve_path_id.ToString());
			foreach ( int cellId in this.solve_path )
			{
				line.Add(cellId.ToString());
			}
			data.Add(String.Join(",", line.ToArray()));

			// cellId, queueProgress, queueSize, queueItems
			for ( int cellId = 1; cellId <= Board.max_tiles; cellId++ )
			{
				line = new List<string>();
				line.Add(cellId.ToString());
				line.Add(this.queueProgress[cellId-1].ToString());
				line.Add(this.queueList[cellId].Count.ToString());
				List<string> queueList = new List<string>();
				foreach ( TileInfo tile in this.queueList[cellId] )
				{
					queueList.Add(tile.pattern);
				}
				if ( queueList.Count > 0 )
				{
					line.Add(String.Join(",", queueList.ToArray()));
				}
				data.Add(String.Join(",", line.ToArray()));
			}
			
			// numIterations, mostTilesPlaced
			data.Add(this.numIterations.ToString());
			data.Add(this.mostTilesPlaced.ToString());
			
			string seedinfo = this.getSeedInfo();
			
//			string filename = "queues\\" + Board.title + "-" + Program.TheMainForm.getTimestamp() + "-S1-" + Program.TheMainForm.labelS1SolveMethod.Text + "-" + seedinfo + this.numIterations + "i-" + this.board.Count.ToString() + "t.txt";
			// tileset-s1-method-seed-score-iteration-datetime.txt
			string filename = "queues\\" + Board.title + "-S1-" + Program.TheMainForm.inputS1CurrentSolveMethod.Text + "-" + seedinfo + this.board.Count.ToString() + "t-" + this.numIterations + "i-" + Program.TheMainForm.getTimestamp() + ".txt";
			
			if ( this.storeQueueInMemory )
			{
				this.queueFilename = filename;
				this.queueData = data;
			}
			else
			{
				System.IO.File.WriteAllText(filename, String.Join("\r\n", data.ToArray()));
				this.logt("saved queue to " + filename, 1);
				this.loadQueueList();
			}
		}

		public void loadQueue(string qid)
		{
			if ( qid == "" )
			{
				System.Windows.Forms.MessageBox.Show("Please select a queue to load.");
				return;
			}
			string filename = "queues\\" + Board.title + "-" + qid + ".txt";
			List<string> lines;
			try
			{
				lines = new List<string>(System.IO.File.ReadAllLines(filename));
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("Could not queue from: " + filename);
				return;
			}
			
			// solver method
			this.solve_method = lines[0];
			Program.TheMainForm.inputS1CurrentSolveMethod.Text = this.solve_method;
			lines.RemoveAt(0);
			
			// load model
			string modelfile = lines[0];
			lines.RemoveAt(0);
			if ( !Program.TheMainForm.board.loadModel(modelfile) )
			{
				System.Windows.Forms.MessageBox.Show("Could not load model: " + modelfile);
				return;
			}
			this.loadBoardFromModel();
			
			// re-apply hints
			this.applyHints();
			
			// solve path
			this.pb.clear();
			List<string> solvepath = new List<string>(lines[0].Split(','));
			List<int> isolvepath = new List<int>();
			int isolve_path_id = Convert.ToInt16(solvepath[0]);
			solvepath.RemoveAt(0);
			foreach ( string cellId in solvepath )
			{
				isolvepath.Add(Convert.ToInt16(cellId));
			}
			this.pb.addCellsToPath(isolvepath.ToArray());
			Program.TheMainForm.inputS1SolvePath.Text = String.Join(",", isolvepath.ConvertAll<string>(delegate(int i){ return i.ToString(); }).ToArray());

			lines.RemoveAt(0);
			this.log("loaded solve path with " + solvepath.Count.ToString() + " cell(s)");
			
			// queue
			int iline = 0;
			foreach ( string line in lines.GetRange(0, Board.max_tiles) )
			{
				iline++;
				List<string> data = new List<string>(line.Split(','));
				int cellId = Convert.ToInt16(data[0]);
				int queueProgress = Convert.ToInt16(data[1]);
				int queueSize = Convert.ToInt16(data[2]);
				data.RemoveRange(0, 3);
				// clear previous queue
				this.queueClear(cellId);
				this.queueProgress[cellId-1] = queueProgress;

				if ( queueSize != data.Count )
				{
					this.logt("error loading queue from " + filename + " - incorrect queue size for cell " + cellId.ToString(), 1);
					return;
				}
				else
				{
					// remove duplicate tiles from queue list (from previous bug)
					List<string> uniqueTiles = new List<string>();
					for ( int i = 0; i < queueSize; i++ )
					{
						string pattern = data[i];
						if ( !uniqueTiles.Contains(pattern) )
						{
							uniqueTiles.Add(pattern);
							this.queueList[cellId].Add(new TileInfo(this.getTileId(pattern), pattern));
						}
					}
				}

			}
	        lines.RemoveRange(0, Board.max_tiles);
			
			// numIterations, mostTilesPlaced
			this.numIterations = Convert.ToInt64(lines[0]);
			this.mostTilesPlaced = Convert.ToInt16(lines[1]);
			
			// reset autosave score if set (increase by 1)
			if ( this.auto_score_trigger_enabled )
			{
				this.auto_score_trigger = this.mostTilesPlaced;
				this.auto_score_next_trigger = this.mostTilesPlaced + 1;
				Program.TheMainForm.inputS1ScoreTrigger.Text = this.auto_score_trigger.ToString();
				Program.TheMainForm.labelS1NextScoreTrigger.Text = this.auto_score_next_trigger.ToString();
			}

			Program.TheMainForm.labelS1NumIterations.Text = this.numIterations.ToString();
			Program.TheMainForm.labelS1NumIterations.Update();
			this.pause();
			this.solve_path_id = isolve_path_id;
			this.updateStats(true);
	
			this.log("loaded queue from " + filename);
			this.log("solve_path_id " + (this.solve_path_id+1).ToString());
			this.log("cellId " + this.getCellId().ToString());
			this.loadQueueList();
			this.loadTileLists();
		}
		
		public void loadBoardFromModel()
		{
			string model = Program.TheMainForm.textModel.Text;
			string[] lines = Program.TheMainForm.textModel.Text.Split('\n');
			this.board = new SortedDictionary<int, TileInfo>();
//			this.usedTiles = new List<int>();
			this.usedTiles = new HashSet<int>();
			int i = 0;
			foreach ( string line in lines )
			{
				string[] parts = line.Trim().Split(',');
				if ( parts.Length >= 3 )
				{
					// v1 col,row,tileId,rotation
					int col = 1;
					int row = 1;
					int pos = 1;
					int tileId = 1;
					int rotation = 1;

					if ( Program.TheMainForm.isNumeric(parts[0]) && Program.TheMainForm.isNumeric(parts[1]) )
					{
						col = Convert.ToInt16(parts[0]);
						row = Convert.ToInt16(parts[1]);
						pos = (row - 1) * Board.num_cols + col;
						if ( pos < 1 || pos > Board.max_tiles )
						{
							throw new Exception("Invalid tile position " + pos + " [" + col + "," + row + "] on line: " + i + 1);
						}
					}
					else
					{
						throw new Exception("Invalid line " + i + 1 + " : " + line);
					}

					// v1 format col,row,tileId,rotation
					if ( Program.TheMainForm.isNumeric(parts[2]) )
					{
						tileId = Convert.ToInt16(parts[2]);
						if ( parts.Length >= 4 )
						{
							rotation = Convert.ToInt16(parts[3]);
						}
					}
					else
					{
						// v2 col,row,pattern
						string pattern = parts[2];
						tileId = this.getTileId(pattern);
						rotation = this.getTileRotationByPattern(pattern);
						TileInfo tile = new TileInfo(tileId, this.tileset[tileId].patterns[rotation-1]);
						this.placeTile(pos, tile);
					}

					i++;
				}
			}
		}

		public int countSurroundingTiles(int col, int row)
		{
			int numSurrounding = 0;
			
			// check left
			if ( col > 1 )
			{
				int leftpos = (row - 1) * Board.num_cols + col - 1;
				if ( this.board.ContainsKey(leftpos) )
				{
					numSurrounding ++;
				}
			}

			// check top
			if ( row > 1 )
			{
				int toppos = (row - 1 - 1) * Board.num_cols + col;
				if ( this.board.ContainsKey(toppos) )
				{
					numSurrounding ++;
				}
			}

			// check right
			if ( col < Board.num_cols )
			{
				int rightpos = (row - 1) * Board.num_cols + col + 1;
				if ( this.board.ContainsKey(rightpos) )
				{
					numSurrounding ++;
				}
			}

			// check bottom
			if ( row < Board.num_rows )
			{
				int bottompos = (row - 1 + 1) * Board.num_cols + col;
				if ( this.board.ContainsKey(bottompos) )
				{
					numSurrounding ++;
				}
			}
			return numSurrounding;
		}
		
		public bool isEdgePos(int col, int row)
		{
			if ( col == 1 || col == Board.num_cols || row == 1 || row == Board.num_rows )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public int[] countMatchingTiles(int col, int row, string match)
		{
			// returns numUniqueFree & numUniqueUsed tiles that match the pattern
			List<int> freeTiles = new List<int>();
			List<int> usedTiles = new List<int>();
				
		    int[] rv = new int[2];
			foreach ( var tile in this.alltiles )
			{
	            if ( Regex.IsMatch(tile.Key, "^" + match, RegexOptions.IgnoreCase) )
				{
	            	int tileId = tile.Value;
	            	if ( this.usedTiles.Contains(tileId) )
	            	{
	            		if ( !usedTiles.Contains(tileId) )
	            		{
		            		usedTiles.Add(tileId);
	            		}
	            	}
	                else
	                {
	                	if ( !freeTiles.Contains(tileId) )
	                	{
			            	freeTiles.Add(tileId);
	                	}
	                }
	            }
			}

		    // return [numFree, numUsed]
		    rv[0] = freeTiles.Count;
		    rv[1] = usedTiles.Count;
		    return rv;
		}
		
		// checks if cell is in checkpoint list and then checks if cell is fillable
		public bool validateCheckpoint(int cellId)
		{
			if ( this.pb.checkpoints.ContainsKey(cellId) )
			{
				foreach ( int checkCell in this.pb.checkpoints[cellId] )
				{
					if ( !this.checkIfCellIsFillable(checkCell) )
					{
						return false;
					}
				}
			}
			return true;
		}
		
		public bool checkIfCellIsFillable(int cellId)
		{
			int[] colrow = Board.getColRowFromPos(cellId);
			string match = this.getTileMatchString(colrow[0], colrow[1]);
			int[] numTiles = this.countMatchingTiles(colrow[0], colrow[1], match);
			if ( numTiles[0] == 0 )
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		
		public int countUnfillableCells()
		{
			// checks to see if there are any unfillable cells, returns number of cells unfillable
			// useful to backtrack as soon as a unsolvable position is encountered
			
			// TODO - check for swappable tiles where numUsed > 0 ?
			int numUnfillable = 0;
			for ( int cellId = 1; cellId <= Board.max_tiles; cellId++ )
			{
				// search for blank tiles and check intersecting tiles where applicable
				if ( !this.board.ContainsKey(cellId) )
				{
					int[] colrow = Board.getColRowFromPos(cellId);
					string match = this.getTileMatchString(colrow[0], colrow[1]);
					if ( this.countSurroundingTiles(colrow[0], colrow[1]) > 1 || (this.countSurroundingTiles(colrow[0], colrow[1]) > 0 && this.isEdgePos(colrow[0], colrow[1])) )
					{
						int[] numTiles = this.countMatchingTiles(colrow[0], colrow[1], match);
						if ( numTiles[0] == 0 )
						{
							numUnfillable++;
						}
						//Program.TheMainForm.log("countIntersectingTiles(" + colrow[0] + "," + colrow[1] + "), numFree = " + numTiles[0] + ", numUsed = " + numTiles[1]);
					}
					else
					{
						//Program.TheMainForm.log("countIntersectingTiles(" + colrow[0] + "," + colrow[1] + ") didn't match ? " + match);
					}
				}
			}
			return numUnfillable;
		}
		
		public void setSolveMethod(string method, int pathId)
		{
			this.solve_path = new List<int>();
			this.solve_path_id = pathId;

			if ( CAF_Application.config.contains("solve_method") )
			{
				this.solve_method = CAF_Application.config.getValue("solve_method");
				Program.TheMainForm.selS1SolveMethod.Text = this.solve_method;
			}

			if ( CAF_Application.config.contains("solve_path") )
			{
				string spath = CAF_Application.config.getValue("solve_path");
				Program.TheMainForm.inputS1SolvePath.Text = spath;
				this.pb.path = new List<string>(spath.Split(',')).ConvertAll<int>(
				delegate(string s)
				{
					if ( s.Length > 0 )
					{
			          	return Convert.ToInt16(s); 
					}
					else
					{
						return 0;
					}
				});
			}
			else if ( method != "Custom" )
			{
				this.pb.clear();
				this.pb.buildPath(method);
			}
			else
			{
				this.pb.path = new List<string>(Program.TheMainForm.inputS1SolvePath.Text.Split(',')).ConvertAll<int>(
				delegate(string s)
				{
					if ( s.Length > 0 )
					{
			          	return Convert.ToInt16(s); 
					}
					else
					{
						return 0;
					}
				});
			}
			this.solve_method = method;
			if ( method == "rows_right_down" )
			{
				this.method_right_down = true;
			}
			else
			{
				this.method_right_down = false;
			}
			this.solve_path = this.pb.path;

			Program.TheMainForm.inputS1SolvePath.Text = String.Join(",", this.pb.path.ConvertAll<string>(delegate(int i) { return i.ToString(); }).ToArray());
//			this.logl("path length: " + this.pb.path.Count, 1);
//			this.logl("excluded cells: " + this.pb.getExcludedCellsAsString(), 1);
		}
		
		public void loadHints(string hid)
		{
			// col,row,tileId,searchPattern 
			// 8,9,139,LHII

			string listpath = System.IO.Path.Combine("tilelists\\" + Board.title, "hints_" + hid + ".txt");
			if ( System.IO.File.Exists(listpath) )
			{
				List<string> list = new List<string>(System.IO.File.ReadAllLines(listpath));
				// update hints text
				Program.TheMainForm.textHints.Text = String.Join("\r\n", list.ToArray());
			}
		}
		
		public void applyHints()
		{
			// re-apply hints that have been loaded
			List<string> list = new List<string>(Program.TheMainForm.textHints.Text.Trim().Split('\n'));
			this.hintTiles = new Dictionary<int, int>();
			foreach ( string line in list )
			{
				List<string> data = new List<string>(line.Trim().Split(','));
				if ( data.Count == 4 && data[0][0] != ';' && data[0][0] != '#' )
				{
					int col = Convert.ToInt16(data[0]);
					int row = Convert.ToInt16(data[1]);
					int tileId = Convert.ToInt16(data[2]);
					string pattern = data[3];
					int cellId = (row - 1) * Board.num_cols + col;
					this.hintTiles.Add(cellId, tileId);
					
					// place on solver board
					TileInfo tile = new TileInfo(tileId, pattern);
					this.placeTile(cellId, tile);
					
					// place on interactive board
					Tile itile = Program.TheMainForm.board.getTileByPattern(pattern);
					Program.TheMainForm.board.drawTile(col, row, itile);
				}
			}
			this.logt("using " + this.hintTiles.Count + " hint tile(s)", 1);
		}
		
		// set cell filters for cells adjacent to hint tiles
		public void setAdjacentHintFilters()
		{
			int numAdded = 0;
			foreach ( int cellId in this.hintTiles.Keys )
			{
				// left_cell.right = hint.left - col 2+
				if ( cellId % Board.num_cols != 1 )
				{
					this.addCellFilter(cellId - 1, "^.." + this.board[cellId].left);
					numAdded++;
				}
				// above_cell.bottom = hint.top - row 2+
				if ( cellId > Board.num_cols )
				{
					this.addCellFilter(cellId - Board.num_cols, "^..." + this.board[cellId].up);
					numAdded++;
				}
				// right_cell.left = hint.right - col < Board.num_cols
				if ( cellId % Board.num_cols != 0 )
				{
					this.addCellFilter(cellId + 1, "^" + this.board[cellId].right);
					numAdded++;
				}
				// below_cell.top = hint.bottom - row < Board.num_rows
				if ( cellId <= Board.max_tiles - Board.num_cols )
				{
					this.addCellFilter(cellId + Board.num_cols, "^." + this.board[cellId].down);
					numAdded++;
				}
			}
			
			// auto enable cell filters
			if ( numAdded > 0 )
			{
				Program.TheMainForm.cbCellFilter.Checked = true;
				//int i = 0;
				//foreach ( int cellId in this.cellFilters.Keys )
				//{
				//	i++;
				//	this.logt("cell filter " + i + " for adjacent hint cellId " + cellId + " = " + this.cellFilters[cellId], 1);
				//}
			}
		}
		
		// merge cell filters using | regex operator if adding multiple filters per cell
		// tile must match all filters that have been added/merged
		public void addCellFilter(int cellId, string pattern)
		{
			if ( this.cellFilters.ContainsKey(cellId) )
			{
				this.cellFilters[cellId] += "~" + pattern;
			}
			else
			{
				this.cellFilters[cellId] = pattern;
			}
		}
		
		public void saveHints(string hid)
		{
			string filename = System.IO.Path.Combine("tilelists\\" + Board.title, "hints_" + hid + ".txt");
			if ( Program.TheMainForm.textHints.Text.Trim() != "" )
			{
				System.IO.File.WriteAllText(filename, Program.TheMainForm.textHints.Text.Trim());
			}
			Program.TheMainForm.loadHintList();
		}
		
		public void setBoardAsHints()
		{
			List<string> hints = new List<string>();
			foreach ( int cellId in this.board.Keys )
			{
				List<string> line = new List<string>();
				TileInfo tile = this.board[cellId];
				int[] colrow = Board.getColRowFromPos(cellId);
				line.Add(colrow[0].ToString());
				line.Add(colrow[1].ToString());
				line.Add(tile.id.ToString());
				line.Add(tile.pattern);
				hints.Add(String.Join(",", line.ToArray()));
			}
			Program.TheMainForm.textHints.Text = String.Join("\r\n", hints.ToArray());
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
					this.tileList.Add(basename, new List<TileInfo>());
					string[] tiles = System.IO.File.ReadAllLines(filename);
					foreach ( string tile in tiles )
					{
						if ( this.alltiles.ContainsKey(tile) )
						{
							int tileId = this.alltiles[tile];
							// exclude hint tiles
							if ( !this.hintTiles.ContainsValue(tileId) )
						    {
								this.tileList[basename].Add(new TileInfo(tileId, tile));
						    }
						}
					}
					numTiles += this.tileList[basename].Count;
					if ( this.tileList[basename].Count > 0 )
					{
						numQueuesLoaded++;
					}
					this.logt("loaded tile list " + filename + " with " + this.tileList[basename].Count + " tile(s)", 3);
				}
			}
			this.logt("loaded " + numTiles + " tile(s) from " + numQueuesLoaded + " tile lists", 1);
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
				this.buildTileListBySearch("edge_left_" + p, "-[^-]{3}");
			}
			*/
			
			// edge_left -[^-]{3}
			this.buildTileListBySearch("edge_left", "-[^-]{3}");
			// edge_top  [^-]-[^-]{2}
			this.buildTileListBySearch("edge_top", "[^-]-[^-]{2}");
			// edge_right [^-]{2}-[^-]
			this.buildTileListBySearch("edge_right", "[^-]{2}-[^-]");
			// edge_bottom [^-]{3}-
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
			foreach ( string tilepattern in this.alltiles.Keys )
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
			foreach ( TileInfo tile in this.tileset.Values )
			{
				// ABCD -> AB, BC, CD, DA
				pairs.Add(tile.pattern.Substring(0, 2));
				pairs.Add(tile.pattern.Substring(1, 2));
				pairs.Add(tile.pattern.Substring(2, 2));
				pairs.Add(tile.pattern[3].ToString() + tile.pattern[0].ToString());
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

		public bool reset()
		{
			this.time_elapsed = 0;
			this.end_time = 0;
			this.start_time = 0;

			this.clearScores();
			this.scoreStats = new Dictionary<int, Int64>(Board.max_tiles);
			this.numQueueJumps = 0;
			this.num_overall_tiles_placed = 0;
			this.num_bgcolours.Clear();
			this.bgcolour_stats.Clear();
			
			this.backtrack_empty_cell = false;
			this.first_empty_cell = 0;
			this.max_empty_cells = Convert.ToInt16(Program.TheMainForm.inputS1MaxEmptyCells.Text);
			this.empty_cells.Clear();
			
			// clear board / used tiles
			Program.TheMainForm.board.setLayout();
			Program.TheMainForm.board.loadTileSet(Board.title);
			this.board = new SortedDictionary<int, TileInfo>();
//			this.usedTiles = new List<int>();
			this.usedTiles = new HashSet<int>();
			this.loadTileset();
			
			// generate border inner pattern count
			if ( this.solve_method == "inner" )
			{
				this.generateBorderInnerPatternCount();
			}

			// load pre-saved tile lists
			if ( !this.loadTileLists() )
			{
				// build pre-saved tile lists if they dont exist
				this.buildTileLists();
				this.loadTileLists();
			}
			
			// load cell filters
			this.loadCellFilters();
			// re-apply hints
			this.applyHints();
			// auto apply cell filters based on hint tiles
			this.setAdjacentHintFilters();
			
			// initialise queues
			this.initQueues();
			
			// initialise random seed generator if enabled
			if ( Program.TheMainForm.cbS1UseRandomSeed.Checked )
			{
				this.checkRandomSettings();
				if ( this.useRandom )
				{
					this.setupRandomGenerator();
				}
			}
			
			// initialise solver method / path
			this.loadSolvePath();
			
			// load the magical restrictions :) (pleeease work!)
			if ( Program.TheMainForm.cbS1UseRegionRestrictions.Checked )
			{
				this.pb.loadRegions();
				if ( this.pb.regions.Count > 0 )
				{
					this.useRegionRestrictions = true;
				}
			}
			else
			{
				this.useRegionRestrictions = false;
			}

			Program.TheMainForm.labelS1NumIterations.Text = "0";
			this.numPasses = 1;
			this.lastPass = 0;
			this.numIterations = 0;
			this.numStaleIterations = 0;
			this.numLastStaleIterations = 0;
			this.numBorderIterations = 0;
			this.numBorderSlipMisses = 0;
			this.auto_score_next_trigger = 0;
			this.mostTilesPlaced = 0;
			this.bestIteration = 0;
			this.lastSaveIteration = 0;
			this.numRegionSkips = 0;
			this.backtrackLastTriggeredIteration = 0;
			Program.TheMainForm.labelSpeed.Text = "0";
			return true;
		}
		
		public void updateMaxScore()
		{
			if ( this.board.Count > this.mostTilesPlaced )
			{
				this.resetStaleIterations();
				this.mostTilesPlaced = this.board.Count;
				this.bestIteration = this.numIterations;
				// store queue in memory
				this.storeQueue();
//				HACK this.addResult();
			}
			if ( this.board.Count > this.overallMostTilesPlaced )
			{
				this.overallMostTilesPlaced = this.board.Count;
			}
		}
		
		public bool checkUnfillableCells()
		{
			if ( this.enforce_fillable_cells )
			{
				// check for unfillable empty cells to ensure solvable solution
				return this.countUnfillableCells() > 0;
			}
			else
			{
				// dont check for unfillable empty cells
				return false;
			}
		}
		
		public void setupAutoScoreAction()
		{
			this.auto_score_trigger_enabled = false;
			this.auto_score_pause = false;
			this.auto_score_save = false;
			this.auto_score_increment = false;
			if ( Program.TheMainForm.cbS1EnableScoreTrigger.Checked && Program.TheMainForm.inputS1ScoreTrigger.Text != "" )
			{
				// auto score trigger
				this.auto_score_trigger_enabled = true;
				this.auto_score_trigger = Convert.ToInt16(Program.TheMainForm.inputS1ScoreTrigger.Text);
				this.auto_score_next_trigger = this.auto_score_trigger;
				Program.TheMainForm.labelS1NextScoreTrigger.Text = this.auto_score_next_trigger.ToString();
				
				// auto pause
				if ( Program.TheMainForm.cbS1PauseOnScore.Checked )
				{
					this.auto_score_pause = true;
				}
				
				// auto save
				if ( Program.TheMainForm.cbS1ScoreAutoSave.Checked )
				{
					this.auto_score_save = true;
				}

				if ( Program.TheMainForm.cbS1AutoIncrementScore.Checked )
				{
					this.auto_score_increment = true;
				}
			}
		}
		
		public void checkRandomSettings()
		{
			this.useRandom = false;
			this.randomSeeds = false;
			this.useRandomTileQueue = false;
			this.useRandomTileset = false;
			this.start_seed = 0;
			this.seed_step = 1;
			
            if ( Program.TheMainForm.cbS1UseRandomSeed.Checked && ( Program.TheMainForm.cbS1UseRandomQueues.Checked || Program.TheMainForm.cbS1UseRandomTileset.Checked ) )
            {
            	this.useRandom = true;
                if ( Program.TheMainForm.inputS1StartSeed.Text != "" && Program.TheMainForm.inputS1SeedStep.Text != "" )
                {
                	this.start_seed = Convert.ToInt32(Program.TheMainForm.inputS1StartSeed.Text);
                	this.seed_step = Convert.ToInt32(Program.TheMainForm.inputS1SeedStep.Text);
                }
                else
                {
                	Program.TheMainForm.inputS1StartSeed.Text = this.start_seed.ToString();
                	Program.TheMainForm.inputS1SeedStep.Text = this.seed_step.ToString();
                }
                if ( Program.TheMainForm.cbS1RandomSeeds.Checked )
                {
                	this.randomSeeds = true;
                }
                
                if ( Program.TheMainForm.cbS1UseRandomQueues.Checked )
                {
					this.useRandomTileQueue = true;
                }
                if ( Program.TheMainForm.cbS1UseRandomTileset.Checked )
                {
					this.useRandomTileset = true;
				}
            }
            else
            {
            	Program.TheMainForm.grS1RandomTileQueue.Enabled = false;
            	Program.TheMainForm.labelS1Seed.Text = "";
            }
		}
		
		public void setupRandomGenerator()
		{
			if ( this.run_multiple )
			{
				if ( this.randomSeeds )
				{
					this.getSeed();
				}
				else
				{
					if ( this.last_run == 0 )
					{
						this.seed = this.start_seed;
					}
					else
					{
						this.seed += this.seed_step;
					}
				}

				this.logt("STARTING " + Program.TheMainForm.inputS1CurrentSolveMethod.Text + " - RUN #" + (this.last_run+1).ToString() + " / " + this.num_runs.ToString() + ", SEED " + this.seed.ToString() + ", ITERATION LIMIT = " + this.maxIterations.ToString(), 1);
			}
			else
			{
				if ( this.randomSeeds )
				{
					this.getSeed();
				}
				else
				{
					this.seed = this.start_seed;
				}
			}
			
			this.r = new Random(this.seed);
			
			if ( this.useRandomTileset )
			{
				this.loadTileset();
				this.randomiseTileset();
			}
			
			Program.TheMainForm.labelS1Seed.Text = this.seed.ToString();
		}
		
		public void loadSolvePath()
		{
			if ( Program.TheMainForm.inputS1SolvePath.Text.Trim() != "" )
			{
				this.pb.clear();
				this.pb.addCellsToPath(new List<string>(Program.TheMainForm.inputS1SolvePath.Text.Split(',')).ConvertAll<int>(delegate(string s) { return Convert.ToInt16(s); }).ToArray());
				this.solve_path_id = 0;
			}
		}
		
		public void setDebugLevel()
		{
			switch ( Program.TheMainForm.selDebugLevel.Text )
			{
				case "Off":
					this.debug_level = 0;
					break;
				case "Low":
					this.debug_level = 1;
					break;
				case "Medium":
					this.debug_level = 2;
					break;
				case "High":
					this.debug_level = 3;
					break;
			}
		}
		
		// add solve score result to memory
		public void addResult()
		{
			// tileset, method, seed, iteration, numTilesPlaced, pathlength, hash, filename
			// TODO add bestscore (ie. which seeds produced highest score quickest/most often)
			List<string> result = new List<string>();
			result.Add(Board.title);
			result.Add(Program.TheMainForm.inputS1CurrentSolveMethod.Text);
			result.Add(this.seed.ToString());
			result.Add(this.numIterations.ToString());
			result.Add(this.mostTilesPlaced.ToString());
			result.Add(this.solve_path.Count.ToString());
			string hash = this.getModelHash();
			result.Add(hash);
			result.Add(this.getModelFilename());
			this.solveResults.Add(String.Join(",", result.ToArray()));
			this.logt("added result: " + String.Join(",", result.ToArray()), 3);
			
			// track total vs unique solutions
			if ( this.trackNumUniqueSolutions )
			{
				if ( !this.uniqueSolutions.Contains(hash) )
				{
					this.uniqueSolutions.Add(hash);
				}
				this.totalSolutions.Add(hash);
				Program.TheMainForm.labelSolutions.Text = this.uniqueSolutions.Count + " / " + this.totalSolutions.Count;
			}
		}
		
		// save score results
		public void saveResults()
		{
			this.saveResults(false);
		}

		public void saveResults(bool force)
		{
			if ( this.solveResults.Count == 0 )
			{
				return;
			}
			if ( !force && !this.saveResultsEnabled )
			{
				return;
			}
			// save best queue if enabled
			if ( this.auto_score_save )
			{
				this.saveBestQueue();
			}
			
			// save unique pattern orientations
			if ( this.extractPatternCountAfterSolving )
			{
				this.savePatternOrientations();
			}

			string filename = "logs\\results.txt";
			if ( !System.IO.File.Exists(filename) )
			{
				string header = "tileset,method,seed,iteration,numTilesPlaced,pathlength,hash,filename";
				System.IO.File.WriteAllText(filename, header + "\r\n");
			}
			string data = String.Join("\r\n", this.solveResults.ToArray());
			if ( !data.EndsWith("\r\n") )
			{
				data += "\r\n";
			}
			System.IO.File.AppendAllText(filename, data);
			this.logt("saved " + this.solveResults.Count.ToString() + " results to " + filename, 1);
			
			// save stats
			filename = "stats\\" + Board.title + ".txt";
			// log
			System.IO.File.AppendAllText(filename, Program.TheMainForm.logS1.Text);
			// scores
			System.IO.File.AppendAllText(filename, this.getScores());
			this.logt("saved solve stats to " + filename, 1);
			
			// clear results after saving
			this.solveResults = new List<string>();
		}
		
		public void saveLog()
		{
			string filename = "logs\\" + DateTime.Now.ToString("yyyy-MMdd") + ".txt";
			System.IO.File.AppendAllText(filename, Program.TheMainForm.logS1.Text);
		}
		
		public bool checkCellFilter(int cellId, string tilepattern)
		{
			bool rv = true;
			if ( Program.TheMainForm.cbCellFilter.Checked )
			{
//				Program.TheMainForm.timer.start("checkCellFilter");
				if ( this.cellFilters.ContainsKey(cellId) )
				{
	//	            this.logt("checkCellFilter(" + cellId + "," + tilepattern + " == " + this.cellFilters[cellId], 1);
					/*
					if ( cellId == 137 )
					{
						bool fm = Utils.multipleRegexMatch(tilepattern, this.cellFilters[cellId].Split('~'));
						if ( fm )
						{
							System.Diagnostics.Debugger.Log(0, "info", "MATCH tile: " + tilepattern + ", filter: " + this.cellFilters[cellId] + "\r\n");
						}
						else
						{
							System.Diagnostics.Debugger.Log(0, "info", "FAIL tile: " + tilepattern + ", filter: " + this.cellFilters[cellId] + "\r\n");
						}
					}
					*/
		            //if ( !Regex.IsMatch(tilepattern, this.cellFilters[cellId], RegexOptions.IgnoreCase)  )
		            if ( !Utils.multipleRegexMatch(tilepattern, this.cellFilters[cellId].Split('~')) )
		            {
	//		            this.logt("checkCellFilter(" + cellId + "," + tilepattern + " == " + this.cellFilters[cellId] + " OK", 1);
		            	rv = false;
		            }
				}
				// wildcard filter if no specific filter for cell exists
				else if ( this.cellFilters.ContainsKey(0) )
				{
		            //if ( !Regex.IsMatch(tilepattern, this.cellFilters[0], RegexOptions.IgnoreCase)  )
		            if ( !Utils.multipleRegexMatch(tilepattern, this.cellFilters[0].Split('~')) )
		            {
		            	rv = false;
		            }
				}
//				Program.TheMainForm.timer.stop("checkCellFilter");
			}
			return rv;
		}
		
		public void loadCellFilters()
		{
			this.cellFilters = new Dictionary<int, string>();
			int i = 0;
			if ( Program.TheMainForm.cbCellFilter.Checked && Program.TheMainForm.inputCellFilter.Text != "" )
			{
				string[] lines = Program.TheMainForm.inputCellFilter.Text.Split('\n');
				foreach ( string line in lines )
				{
					// ignore comments
					if ( !line.StartsWith("#" ) )
					{
						string[] parts = line.Split(',');
						if ( parts.Length == 2 )
						{
							i++;
							int cellId = Convert.ToInt16(parts[0]);
							string pattern = parts[1].Trim();
							this.cellFilters.Add(cellId, pattern);
							this.logt("manual cell filter " + i + ". " + cellId + ", pattern: [" + pattern + "]", 2);
						}
					}
				}
			}
		}
		
		public void storeQueue()
		{
			this.storeQueueInMemory = true;
			this.saveQueue();
			this.storeQueueInMemory = false;
		}
		
		public void saveBestQueue()
		{
			if ( this.queueData.Count > 0 && this.queueFilename !=  "" )
			{
				System.IO.File.WriteAllText(this.modelFilename, this.modelData);
				this.logt("saved model to: " + this.modelFilename, 1);
			
				System.IO.File.WriteAllText(this.queueFilename, String.Join("\r\n", this.queueData.ToArray()));
				this.logt("saved queue to: " + this.queueFilename, 1);
				this.loadQueueList();
				
				// clear once saved
				this.queueFilename = "";
				this.queueData = new List<string>();
			}
		}
		
		public void updateIterationStatus()
		{
//			if ( this.numIterations % this.speedInterval == 0 )
			if ( this.updateable() )
			{
//				Program.TheMainForm.timer.stop("iteration");
				double elapsed = Program.TheMainForm.timer.elapsed("iteration");
				Int64 numIterationsPeriod = this.numIterations - this.last_update_iteration;
				double speed = numIterationsPeriod / elapsed;
				Program.TheMainForm.labelSpeed.Text = String.Format("{0:0}", speed);
				Program.TheMainForm.labelSpeed.Update();
//				Program.TheMainForm.timer.start("iteration");
			}
		}		
		
		public void setupBacktrackTrigger()
		{
			this.backtrackDepthLimit = 0;
			this.backtrackMinIterations = 0;
			this.backtrackMinScore = 0; 
			this.backtrackStaleIterations = 0;
			
			this.enforce_fillable_cells = Program.TheMainForm.cbS1EnforceFillableCells.Checked;
			this.backtrack_skip_cell = Program.TheMainForm.rbS1BacktrackSkipCell.Checked;
			this.backtrack_trigger_enabled = Program.TheMainForm.cbS1EnableBacktrackLimit.Checked;
			this.backtrack_pause = Program.TheMainForm.rbS1BacktrackPause.Checked;
			this.backtrack_back_pedal = Program.TheMainForm.rbBacktrackQueueBackPedal.Checked;
			this.backtrack_queue_jump = Program.TheMainForm.rbBacktrackQueueJump.Checked;

			if ( Program.TheMainForm.cbS1BacktrackIterationTrigger.Checked && Program.TheMainForm.inputS1BacktrackMinIterations.Text != "" )
			{
				this.backtrackMinIterations = Convert.ToInt32(Program.TheMainForm.inputS1BacktrackMinIterations.Text);
			}
			if ( Program.TheMainForm.cbS1BacktrackMinScoreTrigger.Checked && Program.TheMainForm.inputS1BacktrackMinScore.Text != "" )
			{
				this.backtrackMinScore = Convert.ToInt32(Program.TheMainForm.inputS1BacktrackMinScore.Text);
			}
			if ( Program.TheMainForm.cbS1BacktrackStaleIterationTrigger.Checked && Program.TheMainForm.inputS1BacktrackStaleIterations.Text != "" )
			{
				this.backtrackStaleIterations = Convert.ToInt32(Program.TheMainForm.inputS1BacktrackStaleIterations.Text);
			}
			if ( Program.TheMainForm.cbS1BacktrackDepthLimit.Checked && Program.TheMainForm.inputS1BacktrackDepthLimit.Text != "" )
			{
				this.backtrackDepthLimit = Convert.ToInt32(Program.TheMainForm.inputS1BacktrackDepthLimit.Text);
			}
		}
		
		public void extractPatternCount()
		{
			// get pattern orientations
//			this.ps.patternIncludeFilter = Board.getInternalPatternRegex();
			this.ps.patternIncludeFilter = Board.internal_pattern_regex;
			// get all 4 rotations
			for ( int r = 1; r <= 4; r++ )
			{
				this.ps.clear();
				foreach ( TileInfo tile in this.board.Values )
				{
					ps.patterns.Add(tile.patterns[r-1]);
				}
				string porient = this.ps.getStatsCSV();
				
				// get hash of orientations
				string hash = Utils.getSHA1(porient);
				this.logt("pattern orientation set R" + r + ", length: " + porient.Length + ", hash: " + hash, 1);
	
				// store pattern orientations if none of the rotations
				if ( !this.uniquePatternOrientations.ContainsKey(hash) )
				{
					this.uniquePatternOrientations.Add(hash, porient);
					Program.TheMainForm.labelUniquePatternOrientations.Text = this.uniquePatternOrientations.Count.ToString();
					Program.TheMainForm.labelUniquePatternOrientations.Update();
				}
				
				// check if it matches the solution
				if ( hash == this.solution_pattern_orientation_hash )
				{
					System.Media.SystemSounds.Asterisk.Play();
					this.logt("*** SOLUTION PATTERN ORIENTATION MATCH @ iteration " + this.numIterations, 1);
					this.pause();
				}
			}
			
		}
		
		public void savePatternOrientations()
		{
			string path = "pattern-orientations\\" + Board.title  + "\\";
			if ( !System.IO.File.Exists(path) )
			{
				System.IO.Directory.CreateDirectory(path);
			}
			foreach ( string hash in this.uniquePatternOrientations.Keys )
			{
				string filename = path + Program.TheMainForm.inputS1CurrentSolveMethod.Text + "_" + hash + ".txt";
				System.IO.File.WriteAllText(filename, this.uniquePatternOrientations[hash]);
			}
		}
		
		public void handleSolved()
		{
//			System.Media.SystemSounds.Asterisk.Play();
			string numTiles = (this.usedTiles.Count + this.hintTiles.Count).ToString();
			string info = this.solve_method + "_" + numTiles;
			this.logt("*** SOLVED " + Board.title + " via " + info + ", SEED = " + this.seed + " @ ITERATION " + this.numIterations.ToString() + ", HASH = " + this.getModelHash(), 1);
			// log numQueueJumps
			this.logt("numQueueJumps: " + this.numQueueJumps, 2);
			// log numStaleIterations
			this.logt("numStaleIterations: " + this.numLastStaleIterations, 2);
			this.addResult();
			this.solutionStats.Add(this.numIterations.ToString());

			// save solution if not tracking tile distribution
			if ( Program.TheMainForm.cbSaveSolutions.Checked )
			{
				this.saveSolution();
			}
			
			if ( !this.trackNumUniqueSolutions )
			{
				this.totalSolutions.Add(this.numIterations.ToString());
				Program.TheMainForm.labelSolutions.Text = this.totalSolutions.Count.ToString();
			}
			
			// track tile & pattern distribution
			if ( Program.TheMainForm.cbTrackTileDistribution.Checked )
			{
				this.trackTileDistribution();
				this.trackPatternDistribution();
			}
			
			// track tile usage stats
			this.trackTileUsageStats();

			// track bgcolour stats
			if ( Program.TheMainForm.cbBGColourStats.Checked )
			{
				this.trackBGColourStats(false);
			}
			
			// count border triplets
			if ( Program.TheMainForm.cbCountBorderTriplets.Checked )
			{
				// check all border rotations
				string left = this.getBorderEdge("left");
				string top = this.getBorderEdge("top");
				string right = this.getBorderEdge("right");
				string bottom = this.getBorderEdge("bottom");
				string border1 = left + top + right + bottom;
				string border2 = top + right + bottom + left;
				string border3 = right + bottom + left + top;
				string border4 = bottom + left + top + right;
				
				List<int> numTriplets = new List<int>();
				numTriplets.Add(this.numTripletPatterns(border1));
				numTriplets.Add(this.numTripletPatterns(border2));
				numTriplets.Add(this.numTripletPatterns(border3));
				numTriplets.Add(this.numTripletPatterns(border4));
				if ( numTriplets.Max() >= 10 )
				{
					this.logl("paused when numTriplets = " + numTriplets.Max(), 1);
					this.pause();
				}
			}

			// extract pattern orientations
			if ( this.extractPatternCountAfterSolving )
			{
				this.extractPatternCount();
			}
			
			if ( this.saveResultsEnabled )
			{
				this.saveQueue();
			}
			
			// tileswap test
			if ( CAF_Application.config.getValue("tileswap_on_solution") == "1" )
			{
				// activate tileswap
				this.runTileswap();
			}
			
		}
		
		// get list of patterns contained within a region for a cell/quarter
		public Dictionary<string, int> getRegionPatterns(int regionId)
		{
			// pattern - count - can't have more than 2 patterns in 1 region (2 quarters = 1 square)
			Dictionary<string, int> patterns = new Dictionary<string, int>();
//			string rv = "";
			if ( this.pb.regions.ContainsKey(regionId) )
			{
				Dictionary<int, List<int>> regionData = this.pb.regions[regionId];
				foreach ( int regionCellId in regionData.Keys )
				{
					foreach ( int rquarter in regionData[regionCellId] )
					{
						if ( this.board.ContainsKey(regionCellId) )
						{
							if ( this.board[regionCellId].pattern.Length > rquarter )
							{
								string pattern = this.board[regionCellId].pattern[rquarter].ToString();
								if ( !patterns.ContainsKey(pattern) )
								{
									patterns.Add(pattern, 1);
								}
								else
								{
									patterns[pattern]++;
								}
							}
							else
							{
								this.logt("warning: invalid quarter " + rquarter.ToString() + " specified in region " + regionId + ", cellId: " + regionCellId.ToString(), 1);
							}
						}
					}
				}
			}
//			return rv;
			return patterns;
		}
		
		// jumps the queue from the beginning or from first empty cell if enabled
		// if method = rows_right_down then jump to first inner cell
		public void queueJumpStart()
		{
			bool jumped = false;
			int maxPathId = this.solve_path_id;
			int startId = 0;
//			if ( this.solve_method == "rows_right_down" )
			if ( this.backtrack_empty_cell )
			{
				this.solve_path_id = this.first_empty_cell;
				this.empty_cells.Clear();
				startId = this.first_empty_cell + 1;
				int cellId = this.solve_path[this.solve_path_id];
				string queueProgress = "queue: " + this.queueProgress[cellId-1].ToString() + " / " + this.queueList[cellId].Count.ToString();
				this.logt("QUEUE JUMP (empty_cell) at iteration " + this.numIterations.ToString() + " to pathId " + this.solve_path_id.ToString() + ", cellId " + cellId + ", " + queueProgress, 1);
			}
			else if ( this.method_right_down )
			{
				startId = Board.num_cols + 1;
			}
			for ( int i = startId; i <= maxPathId; i++ )
			{
				int cellId = this.solve_path[i];
				if ( !this.backtrack_empty_cell && !jumped && cellId > 0 )
				{
					if ( this.queueProgress[cellId-1] < this.queueList[cellId].Count )
					{
						// jump to here
						this.solve_path_id = i;
						jumped = true;
						string queueProgress = "queue: " + this.queueProgress[cellId-1].ToString() + " / " + this.queueList[cellId].Count.ToString();
						this.logt("QUEUE JUMP at iteration " + this.numIterations.ToString() + " to pathId " + i.ToString() + ", cellId " + cellId + ", " + queueProgress, 2);
						this.numQueueJumps++;
					}
				}
				else
				{
					// clear queues / tiles after newly jumped position
					this.queueClear(cellId);
					this.removeTile(cellId);
					this.resetStaleIterations();
				}
			}
			if ( this.backtrack_empty_cell )
			{
				this.first_empty_cell = 0;
				this.backtrack_empty_cell = false;
			}
		}
		
		// back pedals to the first queue where pos 1 & size > 1 (jumps back a few steps)
		// numStaleIterations should be reset if backtrack drops below threshold..
		public void queueBackPedal()
		{
			bool jumped = false;
			int maxPathId = this.solve_path_id;
			// if this.backtrackMinScore then start queue back pedal from path/position of this.backtrackMinScore
			// allow for hint tiles ?
//			if ( this.backtrackMinScore > 0 && this.backtrackMinScore >= this.hintTiles.Count )
			if ( this.backtrackMinScore > 0 )
			{
				maxPathId = this.backtrackMinScore;
			}
			for ( int i = maxPathId; i >= 0; i-- )
			{
				if (i >= this.solve_path.Count - 1) {
					break;
				}
				int cellId = this.solve_path[i];
				if ( !jumped && cellId > 0 )
				{
//					if ( this.queueProgress[cellId-1] == 1 && this.queueProgress[cellId-1] < this.queueList[cellId].Count )
					// by jumping to queues at 1st pos we are missing out potential solutions.. hence make it more granular
					if ( this.queueProgress[cellId-1] < this.queueList[cellId].Count )
					{
						// jump to here
						string queueProgress = "queue: " + this.queueProgress[cellId-1].ToString() + " / " + this.queueList[cellId].Count.ToString();
						this.logt("QUEUE BACK PEDAL - numTiles=" + this.board.Count + ", from pathId " + this.solve_path_id + ", iteration " + this.numIterations.ToString() + " to pathId " + i.ToString() + ", cellId " + cellId + ", " + queueProgress, 2);
						this.solve_path_id = i;
						this.numQueueJumps++;
						jumped = true;
						break;
					}
				}
			}
			if ( jumped )
			{
				// clear queues / tiles after newly jumped position
				for ( int i = this.solve_path_id + 1; i < this.solve_path.Count; i++ )
				{
					int cellId = this.solve_path[i];
					this.queueClear(cellId);
					this.removeTile(cellId);
				}
				this.resetStaleIterations();
			}
		}
		
		public void resetStaleIterations()
		{
//			this.iterationLog.Add(this.numIterations.ToString() + "," + this.numStaleIterations.ToString() + "," + this.numLastStaleIterations.ToString() + ",resetStaleIterations");
			if ( this.numStaleIterations > this.maxStaleIterations )
			{
				this.maxStaleIterations = this.numStaleIterations;
			}
			this.numLastStaleIterations = this.numStaleIterations;
			this.numStaleIterations = 0;
		}
		
		public void clearScores()
		{
			this.overallMostTilesPlaced = 0;
			Program.TheMainForm.labelS1MaxTilesPlaced.Text = "0";
			Program.TheMainForm.labelS1MaxTilesPlaced.Update();
		}
		
		public void clearSolutions()
		{
			this.uniqueSolutions = new List<string>();
			this.totalSolutions = new List<string>();
			Program.TheMainForm.labelSolutions.Text = this.uniqueSolutions.Count + " / " + this.totalSolutions.Count;
		}
		
		public void trackTileDistribution()
		{
			// track where unique tiles appear in each cell of a partial solution
			foreach ( int cellId in this.board.Keys )
			{
				if ( !this.tileDistribution[cellId].Contains(this.board[cellId].pattern) )
				{
					this.tileDistribution[cellId].Add(this.board[cellId].pattern);
				}
			}
		}
		
		public void dumpTileDistribution()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : Tile Distribution Stats");
			stats.Add(this.uniqueSolutions.Count + " solution(s)");
			foreach ( int cellId in this.tileDistribution.Keys )
			{
				int numTiles = this.tileDistribution[cellId].Count;
				if ( numTiles > 0 )
				{
					stats.Add(cellId + " : " + numTiles + " = " + String.Join(",", new List<string>(this.tileDistribution[cellId]).ToArray()));
				}
			}
			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}
		
		public void trackBGColourStats(bool includePartials)
		{
			string tiles = "";
			if ( includePartials )
			{
				// get board tile dump
				List<string> dump = new List<string>();
				foreach ( int cellId in this.board.Keys )
				{
					TileInfo tile = this.board[cellId];
					dump.Add(tile.pattern);
				}
				tiles = String.Join("", dump.ToArray());
			}
			else
			{
				tiles = this.getWholePatterns();
			}
			
			// count bgcolours
			int numBGColours = 0;
			Dictionary<string, string> bgcolours = Board.getBGColourList("all");
			Dictionary<string, int> bgcolour_count = new Dictionary<string, int>();
			foreach ( string bgcolour in bgcolours.Keys )
			{
				string patterns = bgcolours[bgcolour];
				int count = Regex.Matches(tiles, "[" + patterns + "]").Count;
				if ( count > 0 )
				{
					numBGColours++;
				}
				// count individual colours
				if ( bgcolour_count.ContainsKey(bgcolour) )
			    {
					bgcolour_count[bgcolour] += count;
			    }
				else
				{
					bgcolour_count.Add(bgcolour, count);
				}
			}
			
			// determine bgcolour with highest count
			string max_bgcolour = bgcolour_count.OrderBy(key => key.Value).Last().Key;
			if ( this.bgcolour_stats.ContainsKey(max_bgcolour) )
			{
				// update number of times this bgcolour has the highest count
				this.bgcolour_stats[max_bgcolour][0]++;
			}
			else
			{
				this.bgcolour_stats.Add(max_bgcolour, new int[]{1, bgcolour_count[max_bgcolour]});
			}
			// update max count for all bgcolours
			foreach ( string bgcolour in bgcolours.Keys )
			{
				// update max count for this bgcolour
				if ( !this.bgcolour_stats.ContainsKey(bgcolour) )
				{
					this.bgcolour_stats.Add(bgcolour, new int[]{0, bgcolour_count[bgcolour]});
				}
				else if ( bgcolour_count[bgcolour] > this.bgcolour_stats[bgcolour][1] )
				{
					this.bgcolour_stats[bgcolour][1] = bgcolour_count[bgcolour];
				}
			}
			
			// update numbgcolours count
			if ( this.num_bgcolours.ContainsKey(numBGColours) )
			{
				this.num_bgcolours[numBGColours]++;
			}
			else
			{
				this.num_bgcolours.Add(numBGColours, 1);
			}
			if ( Program.TheMainForm.cbPauseNumBGColours.Checked )
			{
				if ( Program.TheMainForm.inputS1NumBGColours.Text == numBGColours.ToString() )
				{
					this.logl("paused when numBGColours = " + numBGColours, 1);
					this.pause();
				}
			}
		}
		
		public void dumpBGColourStats()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : BGColour Stats");
			stats.Add("BGColour,numHighest,maxCount");
			foreach ( string bgcolour in this.bgcolour_stats.Keys )
			{
				stats.Add(String.Concat(bgcolour, ",", String.Join(",", Utils.IntListToStringList(this.bgcolour_stats[bgcolour]))));
			}
			stats.Add("");
			
			// num bgcolours & num solutions
			stats.Add("numBGColours,numSolutions");
			foreach ( int numBGColours in this.num_bgcolours.Keys )
			{
				stats.Add(String.Concat(numBGColours, ",", this.num_bgcolours[numBGColours]));
			}

			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}
		
		
		// 16x16 / 480 squares returns cols 1-31, rows 1-16. Max is col=15, row=16
		private int[] getColRowFromPos(int pos)
		{
			int num_pos = (Board.num_cols * (Board.num_rows - 1)) + ((Board.num_cols-1) * Board.num_rows);
			int rowsize = (Board.num_cols * 2) - 1;
			int rowpos = pos % rowsize;
			int row;
			if ( pos % rowsize == 0 )
			{
				row = pos / rowsize;
			}
			else
			{
				row = (pos / rowsize) + 1;
			}
			int col = pos - ((row-1) * rowsize);
			if ( col == 0 )
			{
				col = Board.num_cols;
			}
			return new int[]{col, row};
		}

		public void trackPatternDistribution()
		{
			// track how many times each pattern has been used in each location
			// eg. 16x16 = 480 patterns
			int num_pos = (Board.num_cols * (Board.num_rows - 1)) + ((Board.num_cols-1) * Board.num_rows);
			for ( int pos = 1; pos <= num_pos; pos++ )
			{
				int[] colrow = this.getColRowFromPos(pos);
				int col = colrow[0];
				int row = colrow[1];
				int tileId = 0;
				int offset = 0;
				char pattern;
				offset = (row-1) * Board.num_cols;
				if ( col < Board.num_cols )
				{
					// left tile
					// col = R
					tileId = offset + col;
					if ( this.board.ContainsKey(tileId) )
					{
						pattern = this.board[tileId].pattern[2];
					}
					else
					{
						pattern = (char)0;
					}

					// right tile
					// col+1 = L
					tileId = offset + col+1;
					
					// if pattern on 2nd tile matches then log pattern stats
					if ( pattern != (char)0 && this.board.ContainsKey(tileId) && this.board[tileId].pattern[0] == pattern )
					{
						if ( !this.patternDistribution.ContainsKey(pos) )
						{
							this.patternDistribution.Add(pos, new HashSet<char>());
						}
						this.patternDistribution[pos].Add(pattern);
					}
				}
				else
				{
					// squares num_cols+1 to (num_cols + num_cols-1) (16-31)
					col -= Board.num_cols - 1;
					// top tile D
					tileId = offset + col;
					if ( this.board.ContainsKey(tileId) )
					{
						pattern = this.board[tileId].pattern[3];
					}
					else
					{
						pattern = (char)0;
					}

					// bottom tile
					// row+1 = U
					tileId = offset + col + Board.num_cols;

					// if pattern on 2nd tile matches then log pattern stats
					if ( pattern != (char)0 && this.board.ContainsKey(tileId) && this.board[tileId].pattern[1] == pattern )
					{
						if ( !this.patternDistribution.ContainsKey(pos) )
						{
							this.patternDistribution.Add(pos, new HashSet<char>());
						}
						this.patternDistribution[pos].Add(pattern);
					}
				}
			}
		}
		
		public void dumpPatternDistribution()
		{
			List<string> stats = new List<string>();
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			stats.Add(timestamp + " : Pattern Distribution Stats");
			foreach ( int pos in this.patternDistribution.Keys )
			{
				int numPatterns = this.patternDistribution[pos].Count;
				if ( numPatterns > 0 )
				{
					// sort patterns
					List<char> pch = new List<char>(this.patternDistribution[pos].ToArray());
					pch.Sort();
					// show which are excluded
					stats.Add(pos + " : " + numPatterns + " = " + new String(pch.ToArray()));
				}
			}
			Program.TheMainForm.logS1Stats.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.logS1Stats.Update();
		}
		
		public void dumpInfo()
		{
			switch ( Program.TheMainForm.selDumpOptions.Text )
			{
				case "board":
					this.dumpBoard();
					break;
				case "bgcolour stats":
					this.dumpBGColourStats();
					break;
				case "pattern distribution":
					this.dumpPatternDistribution();
					break;
				case "queue":
					this.dumpQueue(false);
					break;
				case "queue detail":
					this.dumpQueue(true);
					break;
				case "scores":
					this.dumpScores();
					break;
				case "search stats":
					this.dumpSearchStats();
					break;
				case "iteration log":
					this.dumpIterationLog();
					break;
				case "solution stats":
					this.dumpSolutionStats();
					break;
				case "stats":
					this.dumpStats();
					break;
				case "tile distribution":
					this.dumpTileDistribution();
					break;
				case "tile stats":
					this.dumpTileStats();
					break;
				case "tileset":
					this.dumpTileset();
					break;
				case "used tiles":
					this.dumpUsedTiles();
					break;
			}
		}
		
		public bool isMagicSquare(string[] tiles)
		{
			// calc sum of all cols & rows
			// also calc mod16 sum
			if ( tiles.Length != Board.max_tiles )
			{
				this.logt("isMagicSquare() - Please supply a complete board before checking if magic square.", 1);
				return false;
			}
			
			// calc row sums
			int row_sum = 0;
			for ( int row = 1; row <= Board.num_rows; row++ )
			{
//				Dictionary<int, int[]> rows = new Dictionary<int, int[]>();
				int[] data = new int[Board.num_cols];
				for ( int col = 1; col <= Board.num_cols; col++ )
				{
					int pos = (row - 1) * Board.num_cols + col;
					string tile = tiles[pos-1];
					data[col-1] = Numerology.num_value(tile);
				}
//				rows.Add(row, data);
				if ( row == 1 )
				{
					row_sum = data.Sum();
				}
				else if ( row_sum != data.Sum() )
				{
					return false;
				}
			}

			// calc col sums
			int col_sum = 0;
			for ( int col = 1; col <= Board.num_cols; col++ )
			{
//				Dictionary<int, int[]> cols = new Dictionary<int, int[]>();
				int[] data = new int[Board.num_rows];
				for ( int row = 1; row <= Board.num_rows; row++ )
				{
					int pos = (row - 1) * Board.num_cols + col;
					string tile = tiles[pos-1];
					data[row-1] = Numerology.num_value(tile);
				}
//				cols.Add(col data);
				if ( col == 1 )
				{
					col_sum = data.Sum();
				}
				else if ( col_sum != data.Sum() )
				{
					return false;
				}
			}
			return row_sum == col_sum;
		}
		
		public bool isMagicInnerSquare(string[] tiles)
		{
			// calc sum of all cols & rows excluding border
			// also calc mod16 sum
			
			// calc row sums
			int row_sum = 0;
			for ( int row = 1; row <= Board.num_rows; row++ )
			{
//				Dictionary<int, int[]> rows = new Dictionary<int, int[]>();
				int[] data = new int[Board.num_cols];
				for ( int col = 1; col <= Board.num_cols; col++ )
				{
					int pos = (row - 1) * Board.num_cols + col;
//					string tile = Regex.Replace(tiles[pos-1], Board.getEdgePatternRegex(), "");
					string tile = Regex.Replace(tiles[pos-1], Board.edge_pattern_regex, "");
					data[col-1] = Numerology.num_value(tile);
				}
//				rows.Add(row, data);
				if ( row == 1 )
				{
					row_sum = data.Sum();
				}
				else if ( row_sum != data.Sum() )
				{
					return false;
				}
			}

			// calc col sums
			int col_sum = 0;
			for ( int col = 1; col <= Board.num_cols; col++ )
			{
//				Dictionary<int, int[]> cols = new Dictionary<int, int[]>();
				int[] data = new int[Board.num_rows];
				for ( int row = 1; row <= Board.num_rows; row++ )
				{
					int pos = (row - 1) * Board.num_cols + col;
//					string tile = Regex.Replace(tiles[pos-1], Board.getEdgePatternRegex(), "");
					string tile = Regex.Replace(tiles[pos-1], Board.edge_pattern_regex, "");
					data[row-1] = Numerology.num_value(tile);
				}
//				cols.Add(col data);
				if ( col == 1 )
				{
					col_sum = data.Sum();
				}
				else if ( col_sum != data.Sum() )
				{
					return false;
				}
			}
			return row_sum == col_sum;
		}
		
		// check if inner tiles form a magic square using the tile bit values
		public bool isMagicInnerSquareBitValues(string[] tiles)
		{
			// calc bit values for inner tiles only (extract inner tiles excluding border tiles)
			List<string> innertiles = new List<string>();
			for ( int row = 2; row <= Board.num_rows - 1; row++ )
			{
				for ( int col = 2; col <= Board.num_cols - 1; col++ )
				{
					int pos = (row - 1) * Board.num_cols + col;
					innertiles.Add(tiles[pos-1]);
				}
			}
			Dictionary<string, int> bitValues = this.getTileIndex(innertiles.ToArray());
			this.magicSquareData.Clear();

			// calc row sums
			int row_sum = 0;
			for ( int row = 1; row <= Board.num_rows - 2; row++ )
			{
//				Dictionary<int, int[]> rows = new Dictionary<int, int[]>();
				int[] data = new int[Board.num_cols];
				for ( int col = 1; col <= Board.num_cols - 2; col++ )
				{
					int pos = (row - 1) * (Board.num_cols - 2) + col;
					int bitValue = bitValues[innertiles[pos-1]];
					data[col-1] = bitValue;
				}
//				rows.Add(row, data);
				if ( row == 1 )
				{
					row_sum = data.Sum();
				}
				else if ( row_sum != data.Sum() )
				{
					return false;
				}
				// save rows to magic square table
				this.magicSquareData.Add(String.Join(",", Utils.IntListToStringList(data)));
			}

			// calc col sums
			int col_sum = 0;
			for ( int col = 1; col <= Board.num_cols - 2; col++ )
			{
//				Dictionary<int, int[]> cols = new Dictionary<int, int[]>();
				int[] data = new int[Board.num_rows];
				for ( int row = 1; row <= Board.num_rows - 2; row++ )
				{
					int pos = (row - 1) * (Board.num_cols-2) + col;
					int bitValue = bitValues[innertiles[pos-1]];
					data[row-1] = bitValue;
				}
//				cols.Add(col data);
				if ( col == 1 )
				{
					col_sum = data.Sum();
				}
				else if ( col_sum != data.Sum() )
				{
					return false;
				}
			}
			return row_sum == col_sum;
		}
		
		// returns tile index based on bit value tileId -> bitIndex (sorted by bitvalue)
		public Dictionary<string, int> getTileIndex(string[] tiles)
		{
			Dictionary<string, int> results = new Dictionary<string, int>();
			SortedDictionary<int, string> index = new SortedDictionary<int, string>();
			foreach ( string tile in tiles )
			{
				if ( tile.Trim().Length == 4 )
				{
					int bitvalue = 0;

					// convert LTRB to 20bit integer (well inside a 32bit int)
					int a = Convert.ToByte(tile[0]);
					int b = Convert.ToByte(tile[1]);
					int c = Convert.ToByte(tile[2]);
					int d = Convert.ToByte(tile[3]);
					// convert edges (-) to 0
					// convert A-Z -> 1-26 (-64)
					if ( tile[0] == '-' )
					{
						a = 0;
					}
					else if ( a > 64 )
					{
						a -= 64;
					}
					if ( tile[1] == '-' )
					{
						b = 0;
					}
					else if ( b > 64 )
					{
						b -= 64;
					}
					if ( tile[2] == '-' )
					{
						c = 0;
					}
					else if ( c > 64 )
					{
						c -= 64;
					}
					if ( tile[3] == '-' )
					{
						d = 0;
					}
					else if ( d > 64 )
					{
						d -= 64;
					}
					
					// try convert 22 to 17 (-5 by excluding edges
					/*
					if ( a > 5 )
					{
						a -= 5;
					}
					if ( b > 5 )
					{
						b -= 5;
					}
					if ( c > 5 )
					{
						c -= 5;
					}
					if ( d > 5 )
					{
						d -= 5;
					}
					*/
					
					bitvalue = bitvalue ^ a;
					bitvalue = bitvalue << 5;
					bitvalue = bitvalue ^ b;
					bitvalue = bitvalue << 5;
					bitvalue = bitvalue ^ c;
					bitvalue = bitvalue << 5;
					bitvalue = bitvalue ^ d;
					index.Add(bitvalue, tile);
				}
			}
			int indexId = 0;
			foreach ( string tile in index.Values )
			{
				indexId++;
				results.Add(tile, indexId);
			}
			return results;
		}
			
		public void runCmd()
		{
			List<string> cmdlist = new List<string>(Program.TheMainForm.inputCmd.Text.Split('\r','\n'));
			foreach ( string icmd in cmdlist )
			{
				string[] args = icmd.Trim().Split(' ');
				string cmd = args[0].ToLower();
				if ( !cmd.StartsWith("//") && !cmd.StartsWith("#") )
				{
					if ( cmd == "+" )
					{
						this.stepForward();
					}
					else if ( cmd == "-" )
					{
						this.stepBack();
					}
					else if ( cmd == "solve" )
					{
						this.logt("solve()", 1);
						this.solve();
					}
					else if ( cmd == "solver3" )
					{
						CAF_Application.config.deleteValue("runtime_is_stopped");
						// setup log window output
						//CAF_Application.log.setTextControl(Program.TheMainForm.logwindow, true, LogType.INFO);
						CAF_Application.log.setTextControl(Program.TheMainForm.logS1, true, LogType.INFO);
						
						this.solver3 = new Solver3();
						this.solver3.numCols = Board.num_cols;
						this.solver3.numRows = Board.num_rows;
						this.solver3.initSolver();
						this.solver3.queue.setTileset(new List<string>(this.getTileSetV3().Values).ToArray());
						this.solver3.queue.init();
						this.solver3.setProgressCallback(this.updateSolverProgress);
		
						this.setSolveMethod(Program.TheMainForm.inputS1CurrentSolveMethod.Text, 0);
						this.solver3.pb.path = this.pb.path;
						this.solver3.solve_path = this.solver3.pb.path;
						this.solver3.solve_start_id = 0;
						
						this.solver3.solve();
						CAF_Application.log.add(LogType.INFO, CAF_Application.stats.getAsText() + "\r\n");
						CAF_Application.log.add(LogType.INFO, this.solver3.status + "\r\n");
						this.solver3.updateProgress();
					}
					else if ( cmd == "reset_stats" )
					{
						CAF_Application.stats.reset();
					}
					else if ( cmd == "buildtilelists" )
					{
						this.logt("buildTileLists()", 1);
						this.buildTileLists();
					}
					else if ( cmd == "is_magic_border" )
					{
						this.logt("is_magic_border " + Board.title, 1);
						List<string> borderTiles = new List<string>();
						for ( int i = 0; i < Board.max_tiles; i++ )
						{
							int tileId = Program.TheMainForm.board.tilepos[i];
							if ( tileId > 0 && Program.TheMainForm.board.tileset[tileId-1] != null )
							{
								string tile = Program.TheMainForm.board.tileset[tileId-1].pattern;
								if ( Regex.IsMatch(tile, "-" ) )
							    {
									borderTiles.Add(tile);
							    }
							}
						}
						if ( this.isMagicBorder(borderTiles.ToArray()) )
						{
							this.logt(Board.title + " has magic border", 1);
						}
						else
						{
							this.logt(Board.title + " does NOT have magic border", 1);
						}
					}
					else if ( cmd == "is_magic_square" )
					{
						this.logt("is_magic_square " + Board.title, 1);
						if ( Board.max_tiles != Program.TheMainForm.board.tileset.Count() )
						{
							this.logt("Board size " + Program.TheMainForm.board.tileset.Count() + " != " + Board.max_tiles, 1);
							return;
						}
						string[] tiles = new string[Board.max_tiles];
						for ( int i = 0; i < Board.max_tiles; i++ )
						{
							int tileId = Program.TheMainForm.board.tilepos[i];
							tiles[i] = Program.TheMainForm.board.tileset[tileId-1].pattern;
						}
						if ( this.isMagicSquare(tiles) )
						{
							this.logt("Magic Square test : " + Board.title + " = YES", 1);
						}
						else
						{
							this.logt("Magic Square test : " + Board.title + " = NO", 1);
						}
					}
					else if ( cmd == "is_magic_inner_square" )
					{
						if ( Board.max_tiles != Program.TheMainForm.board.tileset.Count() )
						{
							this.logt("Board size " + Program.TheMainForm.board.tileset.Count() + " != " + Board.max_tiles, 1);
							return;
						}
						string[] tiles = new string[Board.max_tiles];

						for ( int i = 0; i < Board.max_tiles; i++ )
						{
							int tileId = Program.TheMainForm.board.tilepos[i];
							if ( tileId > 0 )
							{
								tiles[i] = Program.TheMainForm.board.tileset[tileId-1].pattern;
							}
						}
						if ( this.isMagicInnerSquareBitValues(tiles) )
						{
							this.logt("Magic Inner Square test : " + Board.title + " = YES", 1);
							// log magic square data
							Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", this.magicSquareData.ToArray());
						}
						else
						{
							this.logt("Magic Inner Square test : " + Board.title + " = NO", 1);
						}
					}
					else if ( cmd == "export_inner_2x2" )
					{
						// export 49 inner 2x2s
						// 18,19,34,35
						// 20,21,36,37
						List<string> output = new List<string>();
						for ( int row = 2; row <= Board.num_rows - 2; row+=2 )
						{
							for ( int col = 2; col <= Board.num_cols - 2; col+=2 )
							{
								int cellId = Board.getCellIdFromColRow(col,row);
								int cell1 = cellId;
								int cell2 = cellId + 1;
								int cell3 = cellId + Board.num_cols;
								int cell4 = cellId + Board.num_cols + 1;
								List<string> line = new List<string>();
								line.Add(Program.TheMainForm.board.tileset[cell1-1].pattern);
								line.Add(Program.TheMainForm.board.tileset[cell2-1].pattern);
								line.Add(Program.TheMainForm.board.tileset[cell3-1].pattern);
								line.Add(Program.TheMainForm.board.tileset[cell4-1].pattern);
								output.Add(String.Join(",", line.ToArray()));
							}
						}
						Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", output.ToArray());
					}
					else if ( cmd == "get_tile_ids" )
					{
						// get tile ids from tile patterns in tbSLog1
						// outputs tile ids in tbSLog2
						int numFound = 0;
						if ( Program.TheMainForm.tbSLog1.Lines.Count() > 0 )
						{
							Program.TheMainForm.tbSLog2.Clear();
							foreach ( string spattern in Program.TheMainForm.tbSLog1.Lines )
							{
								string pattern = spattern.Trim();
								int tileId = this.getTileId(pattern);
								if ( tileId > 0 )
								{
									numFound++;
									Program.TheMainForm.tbSLog2.AppendText(tileId.ToString() + "\r\n");
								}
							}
						}
						this.logl("found " + numFound + " / " + Program.TheMainForm.tbSLog1.Lines.Count() + " tile ids", 1);
					}
					else if ( cmd == "get_tile_patterns" )
					{
						// get tile patterns from tile ids in tbSLog1
						// outputs tile patterns in tbSLog2
						int numFound = 0;
						if ( Program.TheMainForm.tbSLog1.Lines.Count() > 0 )
						{
							Program.TheMainForm.tbSLog2.Clear();
							foreach ( string tileId in Program.TheMainForm.tbSLog1.Lines )
							{
								if ( tileId.Trim() != "" && Convert.ToInt16(tileId.Trim()) > 0 )
								{
									numFound++;
									string spattern = this.tileset[Convert.ToInt16(tileId)].pattern;
									Program.TheMainForm.tbSLog2.AppendText(spattern + "\r\n");
								}
							}
						}
						this.logl("found " + numFound + " / " + Program.TheMainForm.tbSLog1.Lines.Count() + " tiles", 1);
					}
					else if ( cmd == "num_value" )
					{
						string input = String.Join(" ", new List<string>(args).GetRange(1, args.Length-1).ToArray());
						this.logl("num_value(" + input + ")", 1);
						this.logl(Numerology.num_value(input).ToString(), 1);
					}
					else if ( cmd == "num_reduce" && args.Length == 3 )
					{
						int max = Convert.ToInt16(args[1]);
						int num = Convert.ToInt16(args[2]);
						this.logl("num_reduce(" + max + ", " + num + ")", 1);
						this.logl(Numerology.num_reduce(max, num).ToString(), 1);
					}
					else if ( cmd == "highlight_cells" && args.Length == 2 )
					{
						string[] cells = args[1].Split(',');
						foreach ( string sCellId in cells )
						{
							Program.TheMainForm.board.highlightCell(Convert.ToInt16(sCellId), Color.Yellow);
						}
						this.logl("highlight_cells(" + args[1] + ")", 1);
					}
					else if ( cmd == "num_list_border_tiles" )
					{
						// calculate numerology values for border tiles
						List<string> borderTiles = new List<string>();
						foreach ( TileInfo tile in this.tileset.Values )
						{
							if ( Regex.IsMatch(tile.pattern, "-" ) )
						    {
							    borderTiles.Add(tile.pattern);
						    }
						}
						this.logl("num_list_border_tiles(" + Board.title + ")", 1);
						this.logl(String.Join("\r\n", Utils.IntListToStringList(Numerology.num_list(borderTiles.ToArray()))), 1);
					}
					else if ( cmd == "num_list_internal_tiles" )
					{
						// calculate numerology values for internal tiles
						List<string> internalTiles = new List<string>();
						foreach ( TileInfo tile in this.tileset.Values )
						{
							if ( !Regex.IsMatch(tile.pattern, "-" ) )
						    {
							    internalTiles.Add(tile.pattern);
						    }
						}
						this.logl("num_list_internal_tiles(" + Board.title + ")", 1);
						this.logl(String.Join("\r\n", Utils.IntListToStringList(Numerology.num_list(internalTiles.ToArray()))), 1);
					}
					else if ( cmd == "reduce_sum_border_tiles" && args.Length == 2 )
					{
						// calculate reduced mod sum of border tiles
						int max = Convert.ToInt16(args[1]);
						List<string> borderTiles = new List<string>();
						foreach ( TileInfo stile in this.tileset.Values )
						{
							if ( Regex.IsMatch(stile.pattern, "-" ) )
						    {
								// filter out internal patterns
//								string tile = Regex.Replace(stile.pattern, Board.getInternalPatternRegex(), "");
								string tile = Regex.Replace(stile.pattern, Board.internal_pattern_regex, "");
							    borderTiles.Add(tile);
						    }
						}
						this.logl("reduce_sum_border_tiles(" + max + ", " + Board.title + ")", 1);
						this.logl(Numerology.reduce_sum(max, Numerology.num_list(borderTiles.ToArray())).ToString(), 1);
					}
					else if ( cmd == "reduce_sum_internal_tiles" && args.Length == 2 )
					{
						// calculate reduced mod sum of internal tiles
						int max = Convert.ToInt16(args[1]);
						List<string> internalTiles = new List<string>();
						foreach ( TileInfo stile in this.tileset.Values )
						{
							// filter out border patterns
//							string tile = Regex.Replace(stile.pattern, Board.getEdgePatternRegex(), "");
							string tile = Regex.Replace(stile.pattern, Board.edge_pattern_regex, "");
						    internalTiles.Add(tile);
						}
						this.logl("reduce_sum_internal_tiles(" + max + ", " + Board.title + ")", 1);
						this.logl(Numerology.reduce_sum(max, Numerology.num_list(internalTiles.ToArray())).ToString(), 1);
					}
					else if ( cmd == "pattern_stats" )
					{
						// search tileset and report number of unique tiles per pattern, then number of other patterns across chosen pattern
						// produces CSV output
						// Pattern,A,B,C,D,E,F
						// A,24,1,2,3,4,5
						Dictionary<char, Dictionary<char, int>> patternStats = new Dictionary<char, Dictionary<char, int>>();
						int numPatterns = Board.num_edges + Board.num_inner1 + Board.num_inner2 + Board.num_inner3;
						for ( int i = 0; i < numPatterns; i++ )
						{
							char p = Convert.ToChar(Convert.ToInt16('A') + i);
							patternStats.Add(p, new Dictionary<char, int>());

							// pre-create sub patterns
							for ( int ci = 0; ci < numPatterns; ci++ )
							{
								char ch = Convert.ToChar(Convert.ToInt16('A') + ci);
								patternStats[p].Add(ch, 0);
							}

							foreach ( TileInfo stile in this.tileset.Values )
							{
								if ( Regex.IsMatch(stile.pattern, p.ToString(), RegexOptions.IgnoreCase) )
								{
									foreach ( char ch in stile.pattern )
									{
										// exclude borders
										if ( ch != '-' )
										{
											if ( patternStats[p].ContainsKey(ch) )
											{
												patternStats[p][ch]++;
											}
											else
											{
												// we shouldn't be here unless we've mucked up the pattern list as they should be pre-created
												patternStats[p].Add(ch, 1);
											}
										}
									}
								}
							}
						}
						this.logl("pattern_stats", 1);
						List<string> stats = new List<string>();
						// CSV header
						stats.Add("pattern," + String.Join(",", Utils.CharListToStringList(patternStats.Keys.ToArray())));
						foreach ( char p in patternStats.Keys )
						{
							List<string> pstats = new List<string>();
							pstats.Add(p.ToString());
							// sort keys alphabetically
							List<char> keys = new List<char>();
							keys.AddRange(patternStats[p].Keys.ToArray());
							keys.Sort();
							foreach ( char ch in keys )
							{
								// CSV pattern count value
								pstats.Add(String.Format(patternStats[p][ch].ToString()));
							}
							stats.Add(String.Join(",", pstats.ToArray()));
						}
						this.logl(String.Join("\r\n", stats.ToArray()), 1);
					}
					else if ( cmd == "primes" && args.Length == 2 )
					{
						int max = Convert.ToInt16(args[1]);
						for ( int i = 2; i <= max; i++ )
						{
							if ( Numerology.IsPrime(i) )
							{
								this.logl(i + " is prime", 1);
							}
						}
					}
					else if ( cmd == "isprime" && args.Length == 2 )
					{
						int number = Convert.ToInt16(args[1]);
						if ( Numerology.IsPrime(number) )
						{
							this.logl(number + " is prime", 1);
						}
						else
						{
							this.logl(number + " is not prime", 1);
						}
					}
					else if ( icmd.Trim() != "" )
					{
						this.logt("unknown command: " + icmd, 1);
					}
				}
			}
		}
		
		// calculate the sum of the border tile numeric values
		public int calcSumBorderTiles(string[] tiles)
		{
			List<string> borderTiles = new List<string>();
			foreach ( string stile in tiles )
			{
				if ( Regex.IsMatch(stile, "-" ) )
			    {
					// filter out internal patterns from border tiles
//					string tile = Regex.Replace(stile, Board.getInternalPatternRegex(), "");
					string tile = Regex.Replace(stile, Board.internal_pattern_regex, "");
					borderTiles.Add(tile);
			    }
			}
			int[] borderTileValues = Numerology.num_list(borderTiles.ToArray());
			return borderTileValues.Sum();
		}
		
		// calculate the sum of the internal tile numeric values
		public int calcSumInternalTiles(string[] tiles)
		{
			List<string> internalTiles = new List<string>();
			foreach ( string stile in tiles )
			{
				// filter out border patterns from internal tiles
//				string tile = Regex.Replace(stile, Board.getEdgePatternRegex(), "");
				string tile = Regex.Replace(stile, Board.edge_pattern_regex, "");
			    internalTiles.Add(tile);
			}
			int[] internalTileValues = Numerology.num_list(internalTiles.ToArray());
			return internalTileValues.Sum();
		}
		
		// check if border is magic square (row 1 sum = row 16 sum = col 1 sum = col 16 sum)
		
		public bool isMagicBorder(string[] tiles)
		{
			int borderSize = 2 * ((Board.num_cols - 1)+(Board.num_rows - 1));
			if ( tiles.Length < borderSize )
			{
				this.logt("Board size " + tiles.Length + " < " + borderSize, 1);
				return false;
			}
			List<int> rowTop = new List<int>();
			List<int> rowBottom = new List<int>();
			List<int> colLeft = new List<int>();
			List<int> colRight = new List<int>();
			int value = 0;
			foreach ( string tile in tiles )
			{
				// filters out internal pieces from border
				
				// top row
				if ( Regex.IsMatch(tile, "^--|^[^-]-|^[^-]--" ) )
				{
					// only count left/right patterns
					value = Numerology.num_value(String.Concat(tile[0], tile[2]));
					rowTop.Add(value);
				}
				// bottom row
				if ( Regex.IsMatch(tile, "^-[^-]{2}-|^[^-]{3}-|^[^-]{2}--" ) )
				{
					// only count left/right patterns
					value = Numerology.num_value(String.Concat(tile[0], tile[2]));
					rowBottom.Add(value);
				}
				// left col
				if ( Regex.IsMatch(tile, "^--|^-[^-]{3}|^-[^-]{2}-" ) )
				{
					// only count top/bottom patterns
					value = Numerology.num_value(String.Concat(tile[1], tile[3]));
					colLeft.Add(value);
				}
				// right col
				if ( Regex.IsMatch(tile, "^[^-]--|^[^-]{2}-[^-]|^[^-]{2}--" ) )
				{
					// only count top/bottom patterns
					value = Numerology.num_value(String.Concat(tile[1], tile[3]));
					colRight.Add(value);
				}
			}
//			this.logt("border size: " + tiles.Length, 1);
//			this.logt("rowTop size: " + rowTop.Count, 1);
//			this.logt("rowBottom size: " + rowBottom.Count, 1);
//			this.logt("colLeft size: " + colLeft.Count, 1);
//			this.logt("colRight size: " + colRight.Count, 1);
//			this.logt("rowTop sum: " + rowTop.Sum(), 1);
//			this.logt("rowBottom sum: " + rowBottom.Sum(), 1);
//			this.logt("colLeft sum: " + colLeft.Sum(), 1);
//			this.logt("colRight sum: " + colRight.Sum(), 1);
			return ( rowTop.Sum() == rowBottom.Sum() && colLeft.Sum() == colRight.Sum() && rowTop.Sum() == colLeft.Sum() );
		}
		
		// build cell filter from selected solve path
		public void buildCellFilter()
		{
			this.cellFilters.Clear();
			List<string> filter = new List<string>();
			foreach ( int cellId in this.pb.path )
			{
//				string match = this.getTileMatchString(cellId);
				int[] colrow = Board.getColRowFromPos(cellId);
				string match = Program.TheMainForm.board.getTileMatchString(colrow[0], colrow[1]);
				this.addCellFilter(cellId, match);
				filter.Add(cellId + "," + match);
			}
			Program.TheMainForm.inputCellFilter.Text = String.Join("\r\n", filter.ToArray());
			Program.TheMainForm.cbCellFilter.Checked = true;
		}
		
		public void selectTiles()
		{
			List<string> selectedTiles = new List<string>();
			foreach ( int tileId in Program.TheMainForm.board.tilepos )
			{
				if ( tileId > 0 )
				{
					Tile tile = Program.TheMainForm.board.tileset[tileId-1];
					// count points the tile is worth in current position
					selectedTiles.Add(tile.pattern);
				}
			}
			//this.logl(String.Join("\r\n", selectedTiles.ToArray()), 1);
		}
		
		public void resetTileStats()
		{
			// reset tile stats
			this.tileStats = new Dictionary<int, int>();
			for ( int tileId = 1; tileId <= Board.max_tiles; tileId++ )
			{
				this.tileStats.Add(tileId, 0);
			}
			
		}
		
		public int calculateScore()
		{
			// col1-15 - check right & down
			// row 1-15 - check right & down
			// col16 = check down only
			// row16 = check right only
			// 16,16 = skip
			int score = 0;
			string match = "";
			TileInfo rightTile;
			TileInfo belowTile;
			foreach ( int cellId in this.board.Keys )
			{
				TileInfo tile = this.board[cellId];
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				if ( col < Board.num_cols && row < Board.num_rows )
				{
					match = "RD";
				}
				else if ( col == Board.num_cols && row < Board.num_rows )
				{
					match = "D";
				}
				else if ( row == Board.num_rows && col < Board.num_cols )
				{
					match = "R";
				}
				else
				{
					match = "";
				}
				switch ( match )
				{
					case "RD":
						rightTile = this.getTileFromColRow(col+1, row);
						if ( rightTile != null && tile.right == rightTile.left )
						{
							score += 1;
						}
						
						belowTile = this.getTileFromColRow(col, row+1);
						if ( belowTile != null && tile.down == belowTile.up )
						{
							score += 1;
						}
						break;
					case "D":
						belowTile = this.getTileFromColRow(col, row+1);
						if ( belowTile != null && tile.down == belowTile.up )
						{
							score += 1;
						}
						break;
					case "R":
						rightTile = this.getTileFromColRow(col+1, row);
						if ( rightTile != null && tile.right == rightTile.left )
						{
							score += 1;
						}
						break;
				}
			}
			return score;
		}
		
		public string getWholePatterns()
		{
			// count bgcolours from matching pieces
			// col1-15 - check right & down
			// row 1-15 - check right & down
			// col16 = check down only
			// row16 = check right only
			// 16,16 = skip
			string patterns = "";
			string match = "";
			TileInfo rightTile;
			TileInfo belowTile;
			foreach ( int cellId in this.board.Keys )
			{
				TileInfo tile = this.board[cellId];
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				if ( col < Board.num_cols && row < Board.num_rows )
				{
					match = "RD";
				}
				else if ( col == Board.num_cols && row < Board.num_rows )
				{
					match = "D";
				}
				else if ( row == Board.num_rows && col < Board.num_cols )
				{
					match = "R";
				}
				else
				{
					match = "";
				}
				switch ( match )
				{
					case "RD":
						rightTile = this.getTileFromColRow(col+1, row);
						if ( rightTile != null && tile.right == rightTile.left )
						{
							patterns += tile.right;
						}
						
						belowTile = this.getTileFromColRow(col, row+1);
						if ( belowTile != null && tile.down == belowTile.up )
						{
							patterns += tile.down;
						}
						break;
					case "D":
						belowTile = this.getTileFromColRow(col, row+1);
						if ( belowTile != null && tile.down == belowTile.up )
						{
							patterns += tile.down;
						}
						break;
					case "R":
						rightTile = this.getTileFromColRow(col+1, row);
						if ( rightTile != null && tile.right == rightTile.left )
						{
							patterns += tile.right;
						}
						break;
				}
			}
			return patterns;
		}
		
		// returns a string of patterns for a given border edge (left/top/right/bottom)
		public string getBorderEdge(string location)
		{
//			left = col 1 U/D
//			top = row 1 L/R
//			right = col Board.num_cols U/D
//			bottom = row Board.num_rows L/R
			string rv = "";
			if ( location == "left" || location == "right" )
			{
				int col = 1;
				if ( location == "right" )
				{
					col = Board.num_cols;
				}
				for ( int row = 1; row < Board.num_rows; row++ )
				{
					TileInfo tileAbove = this.getTileFromColRow(col, row);
					TileInfo tileBelow = this.getTileFromColRow(col, row+1);
					if ( tileAbove != null && tileBelow != null && tileAbove.down == tileBelow.up )
					{
						rv += tileAbove.down;
					}
					else
					{
						rv += ".";
					}
				}
			}

			if ( location == "top" || location == "bottom" )
			{
				int row = 1;
				if ( location == "bottom" )
				{
					row = Board.num_rows;
				}
				for ( int col = 1; col < Board.num_cols; col++ )
				{
					TileInfo tileLeft = this.getTileFromColRow(col, row);
					TileInfo tileRight = this.getTileFromColRow(col+1, row);
					if ( tileLeft != null && tileRight != null && tileLeft.right == tileRight.left )
					{
						rv += tileLeft.right;
					}
					else
					{
						rv += ".";
					}
				}
			}
			return rv;
		}
		
		// count number of triplet patterns
		// eg. AAABBBCCCDDDEEE or BBBDDDCCCAAAEEE etc
		public int numTripletPatterns(string patterns)
		{
			int rv = 0;
			int i = 0;
			while ( i < patterns.Length - 2 )
			{
				string p1 = patterns.Substring(i, 2);
				string p2 = patterns.Substring(i+1, 2);
				if ( p1 == p2 )
			    {
					// triplet match found, jump 3 digits
			    	rv++;
			    	i += 3;
				}
				else
				{
					// no match, move to next digit
					i++;
				}
		    }
			return rv;
		}
		
		public void trackTileUsageStats()
		{
			foreach ( int cellId in this.board.Keys )
			{
				TileInfo tile = this.board[cellId];
				if ( this.tileStats.ContainsKey(tile.id) )
				{
					this.tileStats[tile.id]++;
				}
				else
				{
					this.tileStats.Add(tile.id, 1);
				}
			}
		}
		
		// return a copy of the tileset in the new v3 format (without using TileInfo)
		public SortedDictionary<int, string> getTileSetV3()
		{
			SortedDictionary<int, string> tileset = new SortedDictionary<int, string>();
			foreach ( int tileId in this.tileset.Keys )
			{
				tileset.Add(tileId, this.tileset[tileId].pattern);
			}
			return tileset;
		}
		
		// returns a region with min/max col/row that encapsulates the boards contents
		public CellRegion getBoardRegion()
		{
			int topLeftCellCol = Board.num_cols;
			int topLeftCellRow = Board.num_cols;
			int bottomRightCellCol = 0;
			int bottomRightCellRow = 0;
			foreach ( int cellId in this.board.Keys )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				// use a bit of reverse psychology to get the min/max col/row from all tiles on the board
				if ( col < topLeftCellCol )
				{
					topLeftCellCol = col;
				}
				if ( col > bottomRightCellCol )
				{
					bottomRightCellCol = col;
				}
				if ( row < topLeftCellRow )
				{
					topLeftCellRow = row;
				}
				if ( row > bottomRightCellRow )
				{
					bottomRightCellRow = row;
				}
			}
			CellRegion region = new CellRegion(topLeftCellCol, topLeftCellRow, bottomRightCellCol, bottomRightCellRow);
			return region;
		}
		
		public void runTileswap()
		{
			TileSwap ts = new TileSwap(this.board, this.getTileSetV3());
			CellRegion region = this.getBoardRegion();
			ts.getOpportunities(region);
			Solution bestop = ts.getBestOpportunity();
			this.logt("best opportunity score: " + bestop.score, 1);
		}

		public bool updateSolverProgress(string text)
		{
			this.solverProgressCount++;
			if ( this.solverProgressCount % 100000 == 0 )
			{
				this.solver3.calcScore();
				Program.TheMainForm.statusInstructions.Text = String.Format("{0}, max score: {1}", text, this.solver3.maxScore);
				Application.DoEvents();
				
				if ( CAF_Application.config.contains("runtime_is_stopped" ) )
				{
					return false;
				}
			}
			return true;
		}

	}
}

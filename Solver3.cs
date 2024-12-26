/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 18/07/2010
 * Time: 11:54 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using et2;
using CAF;

namespace ET2Solver
{
	/// <summary>
	/// Solver v3
	/// aims to be faster (2M-30M tiles placed / sec)
	/// start with basic backtracker, built for speed, then see how adding features affects speed...
	/// run in separate thread
	/// </summary>
	/// 
	public class Solver3
	{
		/* EVENT DATA */
		public delegate bool SolverEvent(string text);
		public SolverEvent progressCallback;	// callback function to update progress during getOpportunities()
		public string progressText = "";
		
		/* BOARD DATA */
		
		// board size
		public int numCols = 16;
		public int numRows = 16;
		
		// cellId, tile pattern (from TileCache)
		public SortedDictionary<int, string> board = new SortedDictionary<int, string>();
		
		/* SOLVER DATA */
		
		// solve path / method
		public List<int> solve_path = new List<int>();
		public int solve_start_id = 0;
		public string solve_method = "rows_right_down";
		//public string solve_method = "spiral_in_cw"; 
		//public string solve_method = "spiral_out_cw"; 
		public PathBuilder pb = new PathBuilder();
		// id of current cell in solve path
		public int solve_path_id = 0;
		
		// list of cells that have been skipped so far
		public Stack<int> empty_cells = new Stack<int>();
		// max number of empty cells to allow
		public int max_empty_cells = 0;
		
		// solver status: INIT | READY | RUNNING | PAUSED | SOLVED | INCOMPLETE
		public string status = "INIT";
		
		// queue object
		public TileQueue queue;

		// score of current solution
		public int score = 0;
		// best score from all current solutions
		public int maxScore = 0;
		
		public int max_solutions = 0;
		public Int64 max_num_tiles_placed = 0;
		
		public Int64 numTilesPlaced = 0;
		public Int64 max_num_tiles_placed_session = 0;
		
		public Int64 numIterations = 0;
		
		// stored solutions
		public List<Solution> solutions = new List<Solution>();
		
		// cell filters for restricting tiles to certain cells
		public Dictionary<int, string> cellFilters = new Dictionary<int, string>();
		// contains cellId & LRUD to make it easy to count scores for filtered searches
		public HashSet<string> cellScores = new HashSet<string>();
		
		public Solver3()
		{
			this.queue = new TileQueue(this);
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
		    if ( this.solve_method == "rows_right_down" )
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

		/* BOARD functions */
		
		public string getPatternAtColRow(int col, int row)
		{
			int pos = (row - 1) * this.numCols + col;
			if ( !this.board.ContainsKey(pos) )
			{
				return "";
			}
			else
			{
				return this.board[pos].Clone().ToString();
			}
		}
		
		// max number of tiles that the board can accommodate
		public int boardSize()
		{
			return this.numCols * this.numRows;
		}
		
		public SortedDictionary<int, string> copyBoard()
		{
			SortedDictionary<int, string> rv = new SortedDictionary<int, string>();
			foreach ( int cellId in this.board.Keys )
			{
				rv.Add(cellId, this.board[cellId]);
			}
			return rv;
		}
		
		// return text dump of current solution from the board
		public string dumpSolution()
		{
			List<string> model = new List<string>();
			foreach ( int cellId in this.board.Keys )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				string tile = this.board[cellId];
				// v2 format col,row,pattern
				model.Add(String.Format("{0},{1},{2}", colrow[0], colrow[1], tile));
			}
			return String.Join("\r\n", model.ToArray());
		}

		// return an SHA1 has of the current solution
		public string getSolutionHash()
		{
			return Utils.getSHA1(this.dumpSolution());
		}
		
		public void saveSolution()
		{
			string dirpath = "solutions\\" + Board.title;
			if ( !System.IO.File.Exists(dirpath) )
			{
				System.IO.Directory.CreateDirectory(dirpath);
			}
			string model = this.dumpSolution();
			string filename = System.IO.Path.Combine(dirpath, this.score + "_" + this.getSolutionHash() + ".txt");
			System.IO.File.WriteAllText(filename, model);
			CAF_Application.log.add(LogType.INFO, "saved solution to: " + filename);
		}
		
		// iterate board tiles calculate score
		// check against filter if used (ie. for adjacent tiles)
		// LTRB
		public int calcScore()
		{
			int score = 0;
			foreach ( int cellId in this.board.Keys )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				
				string tile = this.board[cellId];
				// left
				if ( col > 1 && col <= this.numCols && row <= this.numRows )
				{
					int leftCellId = Board.getCellIdFromColRow(col - 1, row);
					if ( this.board.ContainsKey(leftCellId) && this.board[leftCellId] != null )
					{
						string leftTile = this.board[leftCellId];
						if ( tile != null && tile[0] == leftTile[2] )
						{
							score++;
						}
					}
					else if ( this.cellScores.Contains(cellId + "L") )
					{
						// check score against filter
						//score++;
					}
				}
				// above
				if ( row > 1 && row <= this.numRows && col <= this.numCols )
				{
					int aboveCellId = Board.getCellIdFromColRow(col, row - 1);
					if ( this.board.ContainsKey(aboveCellId) && this.board[aboveCellId] != null )
					{
						string aboveTile = this.board[aboveCellId];
						if ( tile != null && tile[1] == aboveTile[3] )
						{
							score++;
						}
					}
					else if ( this.cellScores.Contains(cellId + "U") )
					{
						// check score against filter
						//score++;
					}
				}
			}
			if ( score > this.maxScore )
			{
				this.maxScore = score;
			}
			return score;
		}
		
		// check if a tile fits into the specified cellId on the board
		// LTRB
		public bool validPlacement(int cellId, string pattern)
		{
			bool rv = false;

		    if ( this.solve_method == "rows_right_down" )
		    {
		    	// check if matches leftTile.right && aboveTile.bottom
		    	// top row = L
		    	if ( cellId > 1 )
		    	{
			    	if ( cellId <= Board.num_cols )
			    	{
			    		if ( this.board.ContainsKey(cellId-1) && pattern[0] != this.board[cellId-1][2] )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
			    	// left col = U
			    	else if ( cellId % Board.num_cols == 1 )
			    	{
			    		if ( this.board.ContainsKey(cellId-Board.num_cols) && pattern[1] != this.board[cellId-Board.num_cols][3] )
			    		{
				    		goto Exit_Fail;
			    		}
			    	}
			    	// everything else excluding top row & left col = LU
			    	else
			    	{
			    		if ( ( this.board.ContainsKey(cellId-1) && pattern[0] != this.board[cellId-1][2] ) || ( this.board.ContainsKey(cellId-Board.num_cols) && pattern[1] != this.board[cellId-Board.num_cols][3] ) )
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
				return rv;
		}

		public string getTileFromColRow(int col, int row)
		{
			int pos = (row - 1) * this.numCols + col;
			if ( this.board.ContainsKey(pos) )
			{
				return this.board[pos];
			}
			else
			{
				return null;
			}
		}
		
		public string getTileMatchString(int col, int row)
		{
//			Program.TheMainForm.timer.start("getTileMatchString");
		    string match = "";
//		    string edgeTile = Board.getEdgePatternRegex();
//		    string internalTile = Board.getInternalPatternRegex();
		    string edgeTile = Board.edge_pattern_regex;
		    string internalTile = Board.internal_pattern_regex;
		    string matchTile = null;

		    // left
		    if ( col > 1 )
		    {
		        matchTile = this.getTileFromColRow(col - 1, row);
		        if ( matchTile != null )
		        {
		            match += matchTile[2];
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
		            match += matchTile[3];
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
		            match += matchTile[0];
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
		            match += matchTile[1];
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
		
		public void placeTile(int cellId, string pattern)
		{
			if ( this.board.ContainsKey(cellId) )
			{
				// remove existing tile
				this.removeTile(cellId);
			}
			// store in board
			this.board.Add(cellId, pattern);
			// mark tile as used
			this.queue.markUsedTile(pattern);
			
			this.numTilesPlaced++;
			CAF_Application.stats.inc("numTilesPlaced");
		}
		
		public void removeTile(int cellId)
		{
			if ( this.board.ContainsKey(cellId) )
			{
				string pattern = this.board[cellId];
				this.queue.freeTile(pattern);
				this.board.Remove(cellId);
			}
		}
		
		/* SOLVER functions */
		
		// set filters that must be matched for tiles to be placed (ie. to fit within existing partial etc
		// cellId, regex pattern
		public void setCellFilters(string[] filters)
		{
			this.cellFilters.Clear();
			int i = 0;
			foreach ( string line in filters )
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
						this.addCellFilter(cellId, pattern);
						//this.logt("manual cell filter " + i + ". " + cellId + ", pattern: [" + pattern + "]", 2);
					}
				}
			}
		}
		
		// set cell filters for cells adjacent to hint tiles
		public void setAdjacentHintFilters()
		{
			int numAdded = 0;
			foreach ( int cellId in this.queue.hintTiles.Keys )
			{
				// left_cell.right = hint.left - col 2+
				if ( cellId % this.numCols != 1 )
				{
					this.addCellFilter(cellId - 1, "^.." + this.board[cellId][0]);
					numAdded++;
				}
				// above_cell.bottom = hint.top - row 2+
				if ( cellId > this.numCols )
				{
					this.addCellFilter(cellId - this.numCols, "^..." + this.board[cellId][1]);
					numAdded++;
				}
				// right_cell.left = hint.right - col < this.numCols
				if ( cellId % this.numCols != 0 )
				{
					this.addCellFilter(cellId + 1, "^" + this.board[cellId][2]);
					numAdded++;
				}
				// below_cell.top = hint.bottom - row < this.numRows
				if ( cellId <= this.boardSize() - this.numCols )
				{
					this.addCellFilter(cellId + this.numCols, "^." + this.board[cellId][3]);
					numAdded++;
				}
			}
			
			/*
			if ( numAdded > 0 )
			{
				int i = 0;
				foreach ( int cellId in this.cellFilters.Keys )
				{
					i++;
					//this.logt("cell filter " + i + " for adjacent hint cellId " + cellId + " = " + this.cellFilters[cellId], 1);
				}
			}
			*/
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
		
		public bool checkCellFilter(int cellId, string tilepattern)
		{
			bool rv = true;
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
			return rv;
		}
		
		// initialise solver, queues etc
		public void initSolver()
		{
			this.numIterations = 0;
			this.maxScore = 0;
			this.numTilesPlaced = 0;

			this.empty_cells = new Stack<int>();
			if ( CAF_Application.config.contains("max_empty_cells") )
			{
				this.max_empty_cells = Convert.ToInt16(CAF_Application.config.getValue("max_empty_cells"));
			}
			else
			{
				this.max_empty_cells = 0;
			}
			
			// solve path starting position
			this.solve_path_id = this.solve_start_id;

			// set board size for path builder
			this.board.Clear();
			
			// set standard solve path if applicable
			if ( this.solve_method.ToLower() != "custom" )
			{
				this.pb.clear();
				this.pb.buildPath(this.solve_method);
				this.solve_path = this.pb.path;
			}

			// exclude hint cells from solve path
			this.pb.excludeCells(this.queue.hintTiles.Keys);

			// initialise queues
			this.queue.init();

			this.solutions.Clear();
			this.status = "READY";
		}
		
		// returns cellId 1 - board size
		public int nextCell()
		{
			if ( this.solve_path_id + 1 < this.solve_path.Count )
			{
				this.solve_path_id++;
				return this.solve_path[this.solve_path_id];
			}
			else
			{
				// cellId = 0 means no more cells
				return 0;
			}
		}
		
		// returns cellId 1 - board size
		public int prevCell()
		{
			if ( this.solve_path_id > 0 )
			{
				int cellId = this.solve_path[this.solve_path_id];
				this.queue.queueClear(cellId);
				this.removeTile(cellId);
				this.solve_path_id--;
				return this.solve_path[this.solve_path_id];
			}
			else
			{
				// cellId = 0 means no more cells
				return 0;
			}
		}
		
		public void solve()
		{
			CAF_Application.stats.inc("solver3_solve()");
			                          
			bool done = false;
			// get first cell
			int cellId = this.solve_path[this.solve_path_id];
			this.status = "RUNNING";
			
			// check if any debug triggers
			int debugOnCell = 0;
			if ( CAF_Application.config.contains("solver3_debug_break_on_cell") )
			{
				debugOnCell = Convert.ToInt16(CAF_Application.config.getValue("solver3_debug_break_on_cell"));
			}
			
			this.max_solutions = 0;
			if ( CAF_Application.config.contains("solver3_max_solutions") )
			{
				this.max_solutions = Convert.ToInt32(CAF_Application.config.getValue("solver3_max_solutions"));
			}
			
			this.max_num_tiles_placed = 0;
			if ( CAF_Application.config.contains("solver3_max_num_tiles_placed") )
			{
				this.max_num_tiles_placed = Convert.ToInt32(CAF_Application.config.getValue("solver3_max_num_tiles_placed"));
			}
			
			this.max_num_tiles_placed_session = 0;
			if ( CAF_Application.config.contains("solver3_max_num_tiles_placed_session") )
			{
				this.max_num_tiles_placed_session = Convert.ToInt32(CAF_Application.config.getValue("solver3_max_num_tiles_placed_session"));
			}

			while ( !done )
			{
				this.numIterations++;
				if ( this.progressCallback != null )
				{
					if ( !this.updateProgress() )
					{
						this.status = "USER_STOPPED";
						done = true;
						break;
					}
				}
				
				if ( this.max_solutions > 0 && this.solutions.Count >= this.max_solutions )
				{
					// max solutions reached - exit
					CAF_Application.log.add(LogType.INFO, this.max_solutions + " maximum solutions reached - aborting");
					this.status = "MAX_SOLUTIONS_REACHED=" + this.max_solutions;
					done = true;
					break;
				}
				
				// get next tile from queue
				this.removeTile(cellId);
				string tile = this.queue.fetchTile(cellId);
				
				/*
				if ( debugOnCell > 0 && debugOnCell == cellId )
				{
					System.Diagnostics.Debugger.Break();
				}
				*/

				// skip cell if queue empty (max_empty_cells > 0)
				if ( tile == "EMPTY" )
				{
					if ( this.empty_cells.Count < this.max_empty_cells )
					{
						this.empty_cells.Push(cellId);
						cellId = this.nextCell();
					}
					else
					{
						// backtrack if max_empty_cells reached
						cellId = this.prevCell();
						
						// backtrack again if prev cell was empty
						while ( cellId > 0 && this.empty_cells.Count > 0 )
						{
							if ( cellId == this.empty_cells.Peek() )
							{
								this.empty_cells.Pop();
								cellId = this.prevCell();
							}
							else
							{
								break;
							}
						}
					}
					
					// fetch tile from next/prev cell
					tile = this.queue.fetchTile(cellId);
				}
				
				if ( tile != null && tile != "EMPTY" )
				{
					// check if tile is a valid placement
					if ( this.validPlacement(cellId, tile) )
					{
						/*
						if ( debugOnCell > 0 && debugOnCell == cellId )
						{
							System.Diagnostics.Debugger.Break();
						}
						*/
	
						// place tile
						this.placeTile(cellId, tile);
						
						CAF_Application.stats.inc("solver3_score_" + this.board.Count);
	
						// check if solved
						if ( this.board.Count == this.solve_path.Count + this.queue.hintTiles.Count )
						{
							this.status = "SOLVED";
							this.onSolutionFound();
							// keep fetching from same cell to exhaust queue
						}
						else
						{
							// if doing tileswap - check if minimum tile count reached
							if ( CAF_Application.config.contains("solution_mintiles_store") )
							{
								if ( this.board.Count >= Convert.ToInt16(CAF_Application.config.getValue("solution_mintiles_store")) )
								{
									CAF_Application.stats.inc("numSolutions");
									this.score = this.calcScore();
									this.solutions.Add(new Solution(this.score, this.copyBoard()));
								}
							}
							
							// get next cell if available, else re-fetch from same cell to exhaust queue
							int nextCellId = this.nextCell();
							if ( nextCellId > 0 )
							{
								cellId = nextCellId;
							}
						}
						
						// exit if max num tiles placed has been reached
						if ( this.max_num_tiles_placed > 0 )
						{
							if ( CAF_Application.stats.get("numTilesPlaced") >= this.max_num_tiles_placed )
							{
								this.status = "NUM_MAX_TILES_PLACED=" + this.max_num_tiles_placed;
								done = true;
								break;
							}
						}
						else if ( this.max_num_tiles_placed_session > 0 && this.numTilesPlaced >= this.max_num_tiles_placed_session )
						{
							this.status = "NUM_MAX_TILES_PLACED_SESSION=" + this.max_num_tiles_placed_session;
							done = true;
							break;
						}
						
					}
					else
					{
						// tile doesn't match/fit this cell - fetch next in queue
						tile = null;
						CAF_Application.stats.inc("numTileFetchesMissed");
					}
				}
				else
				{
					if ( debugOnCell > 0 && debugOnCell == cellId )
					{
						System.Diagnostics.Debugger.Break();
					}

					// backtrack - queue empty
					cellId = this.prevCell();
					if ( cellId == 0 )
					{
						this.status = "FINISHED";
						done = true;
						break;
					}
					else
					{
						// check if cell is empty - then go back another cell and reduce empty_cells
						while ( !this.board.ContainsKey(cellId) && this.empty_cells.Count > 0 )
						{
							this.empty_cells.Pop();
							cellId = this.prevCell();
							if ( cellId == 0 )
							{
								this.status = "FINISHED";
								done = true;
								break;
							}
						}
					}
				}
			}
			
			//CAF_Application.log.add(LogType.INFO, "Solver3 Status: " + this.status);
			//CAF_Application.log.add(LogType.INFO, "Solver3 Score: " + this.calcScore());
			//CAF_Application.log.add(LogType.INFO, CAF_Application.stats.getAsText() + "\r\n");
		}
		
		/* EVENTS */
		
		public void onSolutionFound()
		{
			CAF_Application.stats.inc("solver3_onSolutionFound()");
			
			bool storedInMemory = false;
			
			// if doing tileswap - check if minimum tile count reached - store in memory
			if ( CAF_Application.config.contains("solution_mintiles_store") )
			{
				if ( this.board.Count >= Convert.ToInt16(CAF_Application.config.getValue("solution_mintiles_store")) )
				{
					CAF_Application.stats.inc("numSolutions");
					this.score = this.calcScore();
					this.solutions.Add(new Solution(this.score, this.copyBoard()));
					storedInMemory = true;
				}
			}
			else
			{
				this.score = this.calcScore();
				CAF_Application.stats.inc("numSolutions");
			}

			// store solution in memory if applicable
			if ( CAF_Application.config.contains("solution_minscore_store") && !storedInMemory )
			{
				this.score = this.calcScore();
				if ( this.score >= Convert.ToInt16(CAF_Application.config.getValue("solution_minscore_store")) )
				{
					CAF_Application.stats.inc("numSolutions");
					this.solutions.Add(new Solution(this.score, this.copyBoard()));
					storedInMemory = true;
				}
			}
			
			// save solution to file if applicable
			if ( CAF_Application.config.contains("solution_minscore_save") )
			{
				this.score = this.calcScore();
				if ( this.score >= Convert.ToInt16(CAF_Application.config.getValue("solution_minscore_save")) )
				{
					this.saveSolution();
				}
			}
			
		}
		
		// saveState() - complete dump of config, queue, board, etc use for start/pause/stop/resume etc

		public void setProgressCallback(SolverEvent func)
		{
			this.progressCallback = func;
		}
		
		public void resetStats()
		{
			CAF_Application.stats.del("solver3_");
			CAF_Application.stats.set("numTilesFetched", 0);
			CAF_Application.stats.set("numTilesPlaced", 0);
			CAF_Application.stats.set("numSolutions", 0);
			CAF_Application.stats.set("numTileFetchesMissed", 0);
		}
		
		public bool updateProgress()
		{
			this.progressText = String.Format("NumTilesFetched: {0}, NumTilesPlaced: {1}, NumSolutions: {2}", CAF_Application.stats.get("numTilesFetched"), CAF_Application.stats.get("numTilesPlaced"), CAF_Application.stats.get("numSolutions"));
			bool keepGoing = this.progressCallback(this.progressText);
			return keepGoing;
		}
		
	}
}

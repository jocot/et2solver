/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 20/10/2010
 * Time: 12:52 AM
 * 
 * class that encapsulates the Intelligent Tile Swap Algorithm for Eternity II solver
 * 
 * objective: given a partial solution, find and rank opportunities to swap tiles that will improve the current score
 * 
 * save board state so we can check if we get stuck - ie. we return to a previous state
 *  
 * future feature maybe to find opportunities from existing tiles if empty cells don't provide anything worthwhile..
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using CAF;

namespace ET2Solver
{
	/// <summary>
	/// Description of TileSwap.
	/// </summary>
	public class TileSwap
	{
		// min region with/height
		public int minRegionWidth = 2;
		public int minRegionHeight = 2;
		// max region with/height
		public int maxRegionWidth = 3;
		public int maxRegionHeight = 3;
		
		public SortedDictionary<int, string> tileSet = new SortedDictionary<int, string>();
		public SortedDictionary<int, TileInfo> board = new SortedDictionary<int, TileInfo>();
		public List<int> empty_cells = new List<int>();
		
		// score efficiency ratio score / numTiles - higher ratio means better score using less tiles which is more beneficial..
		public int score = 0;
		public int numTiles = 0;
		public double scoreEfficiency = 0;
		
		// pattern, cellId - allows easy way to find where usedTiles are placed on the board (all rotations)
		public SortedDictionary<string, int> usedTiles = new SortedDictionary<string, int>();
		// list of unique used tile Ids to accurately count how many used tiles were used...
		public HashSet<int> uniqueUsedTiles = new HashSet<int>();
		
		// potential cell regions to explore
		public List<CellRegion> regions = new List<CellRegion>();
		
		// contains cellId & LRUD to make it easy to count scores for filtered searches
		public HashSet<string> cellScores = new HashSet<string>();

		// stored opportunities
		public List<Solution> opportunities = new List<Solution>();
		
		public int numRegions = 0;
		public int totalRegions = 0;
		public int numOpportunities = 0;
		public int totalOpportunities = 0;
		public int potentialOpportunities = 0;
		
		public Solver3 solver = null;
		
		public delegate void TileSwapEvent();
		public TileSwapEvent progressCallback;	// callback function to update progress during getOpportunities()
		
		public int bestOppGain = -1;
		public bool improvedEfficiency = false;
		public int numZeroGainStored = 0;

		public TileSwap()
		{
			this.init();
		}
		
		// using Solver v1 data structure
		public TileSwap(SortedDictionary<int, TileInfo> currentBoard, SortedDictionary<int, string> tileset)
		{
			this.init();
			
			// make a copy of the current board
			this.board = this.copyBoard(currentBoard);
			
			// make a copy of the tileset
			this.tileSet = new SortedDictionary<int, string>(tileset);
		}
		
		// using Solver v3 data structure
		public TileSwap(SortedDictionary<int, string> currentBoard, SortedDictionary<int, string> tileset)
		{
			this.init();

			// make a copy of the current board
			//this.board = this.copyBoard(currentBoard);
			
			// make a copy of the tileset
			this.tileSet = new SortedDictionary<int, string>(tileset);
		}
		
		private void init()
		{
			this.solver = new Solver3();

			// config
			if ( CAF_Application.config.contains("tileswap_min_region_width") )
			{
				this.minRegionWidth = Convert.ToInt16(CAF_Application.config.getValue("tileswap_min_region_width"));
			}
			if ( CAF_Application.config.contains("tileswap_min_region_height") )
			{
				this.minRegionHeight = Convert.ToInt16(CAF_Application.config.getValue("tileswap_min_region_height"));
			}
			if ( CAF_Application.config.contains("tileswap_max_region_width") )
			{
				this.maxRegionWidth = Convert.ToInt16(CAF_Application.config.getValue("tileswap_max_region_width"));
			}
			if ( CAF_Application.config.contains("tileswap_max_region_height") )
			{
				this.maxRegionHeight = Convert.ToInt16(CAF_Application.config.getValue("tileswap_max_region_height"));
			}
		}
		
		// copy v1 board to v3 format
		public SortedDictionary<int, TileInfo> copyBoard(SortedDictionary<int, TileInfo> sourceBoard)
		{
			SortedDictionary<int, TileInfo> boardCopy = new SortedDictionary<int, TileInfo>();
			foreach ( int cellId in sourceBoard.Keys )
			{
				boardCopy.Add(cellId, sourceBoard[cellId]);
			}
			return boardCopy;
		}
		
		// copy v3 board
		public SortedDictionary<int, string> copyBoard(SortedDictionary<int, string> sourceBoard)
		{
			SortedDictionary<int, string> boardCopy = new SortedDictionary<int, string>();
			foreach ( int cellId in sourceBoard.Keys )
			{
				boardCopy.Add(cellId, sourceBoard[cellId]);
			}
			return boardCopy;
		}
		
		/*
		 * get tile swap opportunities
		*/
		public void getOpportunities(CellRegion region)
		{
			CAF_Application.log.add(LogType.INFO, "TileSwap :: getOpportunities()");
			this.bestOppGain = -1;
			this.locateEmptyCells(region);
			this.defineRegions(region);
			this.findOpportunities();
		}
		
		public void locateEmptyCells(CellRegion region)
		{
			this.empty_cells.Clear();
			
			// check partial if specified, else check whole board
			if ( region.isDefined() )
			{
				// partial
				for ( int row = region.topLeftCellRow; row <= region.bottomRightCellRow; row++ )
				{
					for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol; col++ )
					{
						int cellId = Board.getCellIdFromColRow(col, row);
						if ( !this.board.ContainsKey(cellId) || this.board[cellId] == null )
						{
							this.empty_cells.Add(cellId);
						}
					}
				}
			}
			else
			{
				// whole board
				for ( int cellId = 1; cellId <= Board.max_tiles; cellId++ )
				{
					if ( !this.board.ContainsKey(cellId) || this.board[cellId] == null )
					{
						this.empty_cells.Add(cellId);
					}
				}
			}
			CAF_Application.log.add(LogType.INFO, "found " + this.empty_cells.Count + " empty cells");
		}
		
		// define regions based on list of empty cells
		// will search up to a configurable size eg. 2x2, 3x2, 3x3 etc
		// multiple locations for each region
		// eg. 2x2 has 4 positions, top left, top right, bottom left, bottom right
		// 3x2 = 6, 3x3 = 9, 4x3 = 12, 4x4 = 16
		//
		// region data:
		// regionId, topLeftCell, bottomRightCell, numEmptyCells, regionScore
		public void defineRegions(CellRegion parent_region)
		{
			// handle square regions only 2x2, 3x3, etc
			if ( CAF_Application.config.getValue("tileswap_include_rectangle_regions") == "1" )
			{
				this.defineAllRegions(parent_region);
			}
			else
			{
				// handle rectangle + square regions 2x1, 2x2, 3x2, 3x3 etc
				this.defineSquareRegions(parent_region);
			}
		}
		
		private void defineSquareRegions(CellRegion parent_region)
		{
			this.regions.Clear();
			List<string> uniqueRegions = new List<string>();

			int regionHeight = this.minRegionHeight;
			int regionWidth = this.minRegionWidth;
			
			// iterate through square region sizes from 2x2, 3x3, 4x4 etc
			foreach ( int cellId in this.empty_cells )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int cellCol = colrow[0];
				int cellRow = colrow[1];
				while ( regionWidth <= this.maxRegionWidth && regionHeight <= this.maxRegionHeight )
				{
					for ( int refRow = 1; refRow <= regionHeight; refRow++ )
					{
						for ( int refCol = 1; refCol <= regionWidth; refCol++ )
						{
							// only add valid regions (that are contained within board boundary / parent region)
							int[] regionCoOrds = this.getValidRegion(cellCol, cellRow, regionWidth, regionHeight, refCol, refRow, parent_region);
							if ( regionCoOrds.Length > 0 )
							{
								string regionParams = String.Format("{0}_{1}_{2}_{3}", regionCoOrds[0], regionCoOrds[1], regionCoOrds[2], regionCoOrds[3]);
								// exclude duplicate regions
								if ( !uniqueRegions.Contains(regionParams) )
								{
									uniqueRegions.Add(regionParams);
									// store region
									CellRegion region = new CellRegion(this.regions.Count, regionCoOrds[0], regionCoOrds[1], regionCoOrds[2], regionCoOrds[3]);
									region.numEmptyCells = this.calcRegionNumEmptyCells(region);
									region.score = this.calcRegionScore(region);
									region.maxScore = this.calcRegionMaxScore(region);
									this.regions.Add(region);
									
									CAF_Application.log.add(LogType.DEBUG, String.Format("region {0} = {1}", this.regions.Count, regionParams));
								}
							}
						}
					}
					// increase region size
					regionWidth++;
					regionHeight++;
				}
			}
			
			CAF_Application.log.add(LogType.INFO, "found " + this.regions.Count + " empty cell region(s)");
		}
		
		public void defineAllRegions(CellRegion parent_region)
		{
			this.regions.Clear();
			List<string> uniqueRegions = new List<string>();

			// iterate through variable region sizes from 2x2, 2x3, 2x4, 3x2, 3x3 -> maxRegionWidth,maxRegionHeight
			foreach ( int cellId in this.empty_cells )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int cellCol = colrow[0];
				int cellRow = colrow[1];
				for ( int regionHeight = this.minRegionHeight; regionHeight <= this.maxRegionHeight; regionHeight++ )
				{
					for ( int regionWidth = this.minRegionWidth; regionWidth <= this.maxRegionWidth; regionWidth++ )
					{
						for ( int refRow = 1; refRow <= regionHeight; refRow++ )
						{
							for ( int refCol = 1; refCol <= regionWidth; refCol++ )
							{
								// only add valid regions (that are contained within board boundary)
								int[] regionCoOrds = this.getValidRegion(cellCol, cellRow, regionWidth, regionHeight, refCol, refRow, parent_region);
								if ( regionCoOrds.Length > 0 )
								{
									string regionParams = String.Format("{0}_{1}_{2}_{3}", regionCoOrds[0], regionCoOrds[1], regionCoOrds[2], regionCoOrds[3]);
									// exclude duplicate regions
									if ( !uniqueRegions.Contains(regionParams) )
									{
										uniqueRegions.Add(regionParams);
										// store region
										CellRegion region = new CellRegion(this.regions.Count, regionCoOrds[0], regionCoOrds[1], regionCoOrds[2], regionCoOrds[3]);
										region.numEmptyCells = this.calcRegionNumEmptyCells(region);
										region.score = this.calcRegionScore(region);
										region.maxScore = this.calcRegionMaxScore(region);
										this.regions.Add(region);
										
										CAF_Application.log.add(LogType.DEBUG, String.Format("region {0} = {1}", this.regions.Count, regionParams));
									}
								}
							}
						}
					}
				}
			}
			
			CAF_Application.log.add(LogType.INFO, "found " + this.regions.Count + " empty cell region(s)");
		}
		
		public void identifyUsedTiles()
		{
			this.usedTiles.Clear();
			this.uniqueUsedTiles.Clear();
			
			TileQueue q = new TileQueue();
			// iterate through tiles in board
			foreach ( int cellId in this.board.Keys )
			{
				TileInfo tile = this.board[cellId];
				this.uniqueUsedTiles.Add(tile.id);
				// get all rotations
				foreach ( string pattern in q.getAllRotations(tile.pattern) )
				{
					if ( !this.usedTiles.ContainsKey(pattern) )
					{
						this.usedTiles.Add(pattern, cellId);
					}
				}
			}
		}
		
		public void calcScore()
		{
			// create region that fills the board
			CellRegion region = new CellRegion(1, 1, Board.num_cols, Board.num_rows);
			this.score = this.calcRegionScore(region);
			this.numTiles = this.board.Count;
			this.scoreEfficiency = (double)this.score / (double)this.numTiles;
		}
		
		/*
		 * scan regions and find & store prospective tile swap opportunities
		*/
		public void findOpportunities()
		{
			// init solver
			this.solver.numCols = Board.num_cols;
			this.solver.numRows = Board.num_rows;
			
			// save copy of board
			SortedDictionary<int, TileInfo> backupBoard = this.copyBoard(this.board);
			
			// calculate current board score & efficiency
			this.calcScore();
			
			this.opportunities.Clear();
			
			// iterate through regions
			this.totalRegions += this.regions.Count;

			foreach ( CellRegion region in this.regions )
			{
				this.numRegions++;
				
				if ( CAF_Application.config.contains("runtime_is_stopped" ) )
				{
					return;
				}

				if ( CAF_Application.config.contains("tileswap_region_end") && this.numRegions > Convert.ToInt16(CAF_Application.config.getValue("tileswap_region_end")) )
				{
					return;
				}
				else if ( CAF_Application.config.contains("tileswap_region_start") && this.numRegions < Convert.ToInt16(CAF_Application.config.getValue("tileswap_region_start")) )
				{
					continue;
				}
				
				if ( this.progressCallback != null )
				{
					this.progressCallback();
				}
				
				// set max number of empty cells same as current region (to try get better scoring efficiency)
				//CAF_Application.config.setValue("max_empty_cells", Convert.ToString(region.numEmptyCells));
				if ( region.numEmptyCells > 0 )
				{
					CAF_Application.config.setValue("max_empty_cells", Convert.ToString(region.numEmptyCells - 1));
				}

				// set minimum score to find (try to beat current score for the region)
				// these are using score not numTiles... (currently takes longer to calculate score for each solution = slower
				//CAF_Application.config.setValue("solution_minscore_store", Convert.ToString(region.score - 4));
				//CAF_Application.config.setValue("solution_minscore_store", Convert.ToString(region.score + 1));

				int minNumTiles = region.numTiles() - 1;
				CAF_Application.config.setValue("solution_mintiles_store", Convert.ToString(minNumTiles));
				// estimate minimum number of tiles needed to reach score by / 4 (best possible scenario) 
					// saves having the solver to calculate score after each placeTile()
					// may help find a more efficient score/tile ratio...
				
				//CAF_Application.config.setValue("solution_mintiles_store", Convert.ToString(Math.Floor((double)region.score / 4.0)));

				// clear tiles within region
				this.clearRegion(region);
				
				// count the number of tiles around the perimeter of the region
				// used by analyseOpportunities() to add to opportunity score
				region.perimeterTileCells = this.getPerimeterTileCells(region);
				
				// make list of remaining used tiles (all rotations) - after region has been cleared
				this.identifyUsedTiles();

				// get region perimeter search strings
				string[] filters = this.getRegionFilter(region);
				
				// initialise solver
				this.solver.initSolver();
				
				if ( CAF_Application.config.getValue("tileswap_include_used_tiles") == "all" )
				{
					// use all tiles during tileswap - more opportunities to sift through...
					// exclude used tiles that are adjacent to the region as they will negatively impact the resulting score...
					string[] tiles = this.getUsedTiles(region);
					this.solver.queue.setTileset(tiles);
					CAF_Application.log.add(LogType.INFO, "tileswap using " + tiles.Length + " used tiles (excluding those adjacent to region)");
				}
				else if ( CAF_Application.config.getValue("tileswap_include_used_tiles") == "adjacent_to_empty" )
				{
					// only include used tiles that are adjacent to other empty cells and are not adjacent to the selected region
					// also include available tiles
					string[] tiles = this.getTilesWithAdjacentToEmpty(region);
					this.solver.queue.setTileset(tiles);
					CAF_Application.log.add(LogType.INFO, "tileswap using " + tiles.Length + " available + adjacent to empty tiles");
				}
				else
				{
					// only use available tiles - less opportunities, but scores should be higher
					List<string> availableTiles = new List<string>();
					foreach ( string tile in this.tileSet.Values )
					{
						if ( !this.usedTiles.ContainsKey(tile) )
						{
							availableTiles.Add(tile);
						}
					}
					CAF_Application.log.add(LogType.INFO, "tileswap using " + availableTiles.Count + " available tiles");
					this.solver.queue.setTileset(availableTiles.ToArray());
				}

				// set region as solve path
				this.solver.pb.setRegionAsPath(region);
				this.solver.solve_path = this.solver.pb.path;
				this.solver.solve_start_id = 0;

				// apply filters to solver
				this.solver.cellScores = this.cellScores;
				this.solver.setCellFilters(filters);
				
				// find best score using all tiles (if used tile used, then score is deducted from tile removal
				this.solver.solve();

				// analyse & store the best solution opportunities
				if ( !this.analyseOpportunities(region, solver.solutions) )
				{
					break;
				}
				
				// restore board before processing next region
				this.board = this.copyBoard(backupBoard);
			}
			
			// log best opportunities
			if ( CAF_Application.config.getValue("log_best_opportunities") == "1" && this.opportunities.Count > 0 )
			{
				CAF_Application.log.add(LogType.INFO, "RegionID,Region,RegionScore,NetScore,Gain,OpportunityScore,Eff,UsedScore,MaxPotentialScore,RegionEmpty,NumUsedTiles,NumTilesOnBoard");
				foreach ( Solution opportunity in this.opportunities )
				{
					CAF_Application.log.add(LogType.INFO, String.Format("{0},{1}:{2}-{3}:{4},{5},{6},{7},{8},{9:.000},{10},{11},{12},{13},{14}", 
						opportunity.region.regionId, opportunity.region.topLeftCellCol, opportunity.region.topLeftCellRow, opportunity.region.bottomRightCellCol, opportunity.region.bottomRightCellRow,
                        opportunity.region.score, opportunity.netscore, opportunity.gainLoss, opportunity.score, opportunity.scoreEfficiency, opportunity.usedScore, opportunity.region.maxScore, opportunity.region.numEmptyCells, opportunity.usedTiles.Count, opportunity.board.Count));
				}
			}
		}
		
		// return text dump of current board
		public string getBoardDump()
		{
			List<string> model = new List<string>();
			foreach ( int cellId in this.board.Keys )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				TileInfo tile = this.board[cellId];
				// v2 format col,row,pattern
				model.Add(String.Format("{0},{1},{2}", colrow[0], colrow[1], tile.pattern));
			}
			return String.Join("\r\n", model.ToArray());
		}
		
		public bool analyseOpportunities(CellRegion region, List<Solution> opportunities)
		{
			this.totalOpportunities += opportunities.Count;
			foreach ( Solution opportunity in opportunities )
			{
				this.numOpportunities++;
				if ( this.progressCallback != null )
				{
					this.progressCallback();
				}

				opportunity.region = region;

				// count used tiles & calculate their score
				opportunity.usedTiles = new List<string>();
				List<string> matchingVertices = new List<string>();
				int perimeterScore = 0;
				foreach ( int cellId in opportunity.board.Keys )
				{
					string tile = opportunity.board[cellId];
					if ( this.usedTiles.ContainsKey(tile) )
					{
						// calculate existing score of used tiles
						int usedTileCellId = this.usedTiles[tile];
						matchingVertices.AddRange(this.getTileMatchingVertices(usedTileCellId));
						opportunity.usedTiles.Add(tile);
						
						CAF_Application.log.add(LogType.DEBUG, "OppID: " + opportunity.id + ", RegionID: " + opportunity.region.regionId + ", used tile: " + tile + ", cellID: " + usedTileCellId);
					}

					// add perimeter score if tile adjacent to perimeter tile
					if ( region.perimeterTileCells.ContainsKey(cellId) )
					{
						perimeterScore += region.perimeterTileCells[cellId];
					}
				}
				
				opportunity.usedScore = new HashSet<string>(matchingVertices).Count / 2;
				opportunity.netscore = opportunity.score + perimeterScore - opportunity.usedScore;
				opportunity.gainLoss = opportunity.netscore - region.score;
				
				// compare score efficiency
				int minGain = 0;
				if ( CAF_Application.config.contains("tileswap_min_gain") )
				{
					minGain = Convert.ToInt16(CAF_Application.config.getValue("tileswap_min_gain"));
				}

				int oppScore = this.score + opportunity.gainLoss;
				int oppNumTiles = this.numTiles + opportunity.board.Count - region.numTiles();
				double oppScoreEfficiency = (double)oppScore / (double)oppNumTiles;
				opportunity.scoreEfficiency = oppScoreEfficiency;
				
				// track best gain
				if ( opportunity.gainLoss > this.bestOppGain )
				{
					this.bestOppGain = opportunity.gainLoss;
				}
				
				// check if score efficiency has improved
				if ( opportunity.gainLoss >= minGain && oppScoreEfficiency > this.scoreEfficiency )
				{
					this.improvedEfficiency = true;
				}
				else
				{
					this.improvedEfficiency = false;
				}
				
				// stop if max opportunities reached
				if ( CAF_Application.config.contains("tileswap_max_opportunities") )
				{
					if ( Convert.ToInt16(CAF_Application.config.getValue("tileswap_max_opportunities")) > 0 && Convert.ToInt16(CAF_Application.config.getValue("tileswap_max_opportunities")) <= this.opportunities.Count )
					{
						return false;
					}
				}
				
				// store limited number of zero gain opportunities
				int maxZeroGainStore = 0;
				if ( opportunity.gainLoss == 0 )
				{
					if ( CAF_Application.config.contains("tileswap_max_zerogain_store") )
					{
						maxZeroGainStore = Convert.ToInt16(CAF_Application.config.getValue("tileswap_max_zerogain_store"));
					}
					if ( this.numZeroGainStored < maxZeroGainStore )
					{
						this.numZeroGainStored++;
					}
					else
					{
						// don't store any more opportunities with zero gain
						continue;
					}
				}
				
				// store eligible opportunities
				if ( opportunity.gainLoss >= minGain )
				{
					this.potentialOpportunities++;
					opportunity.id = this.opportunities.Count;
					this.opportunities.Add(opportunity);
					
					// save best opportunities to files
					if ( CAF_Application.config.getValue("save_best_opportunities") == "1" )
					{
						string dirpath = "opportunities\\" + Board.title;
						if ( !System.IO.File.Exists(dirpath) )
						{
							System.IO.Directory.CreateDirectory(dirpath);
						}
						string filename = String.Format("{0}\\{1}_{2}_{3}_{4}.txt", dirpath, this.board.Count, opportunity.gainLoss, opportunity.region.regionId, opportunity.GetHashCode());
						// dump opportunity, region & board info to file
						System.IO.File.WriteAllText(filename, String.Join("\r\n", opportunity.getAsText()) + "\r\n" + "Full Board\r\n" + this.getBoardDump());
					}
					
				}
			}
			return true;
		}
		
		// iterate through all opportunities to find the one that produces the best score
		public Solution getBestOpportunity()
		{
			Solution bestOpportunity = new Solution();
			foreach ( Solution opportunity in this.opportunities )
			{
				if ( opportunity.score > bestOpportunity.score )
				{
					bestOpportunity = opportunity;
				}
			}
			return bestOpportunity;
		}
		
		// calculate the score value of a single tile on the board
		public int calcTileScore(int cellId)
		{
			int score = 0;
			
			if ( this.board[cellId] != null )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				
				TileInfo tile = this.board[cellId];
				
				// left
				int leftCellId = Board.getCellIdFromColRow(col - 1, row);
				if ( this.board.ContainsKey(leftCellId) && this.board[leftCellId] != null )
				{
					TileInfo leftTile = this.board[leftCellId];
					if ( tile != null && tile.left == leftTile.right )
					{
						score++;
					}
				}
				
				// above
				int aboveCellId = Board.getCellIdFromColRow(col, row - 1);
				if ( this.board.ContainsKey(aboveCellId) && this.board[aboveCellId] != null )
				{
					TileInfo aboveTile = this.board[aboveCellId];
					if ( tile != null && tile.up == aboveTile.down )
					{
						score++;
					}
				}

				// right
				int rightCellId = Board.getCellIdFromColRow(col - 1, row);
				if ( this.board.ContainsKey(rightCellId) && this.board[rightCellId] != null )
				{
					TileInfo rightTile = this.board[rightCellId];
					if ( tile != null && tile.right == rightTile.left )
					{
						score++;
					}
				}
				
				// below
				int belowCellId = Board.getCellIdFromColRow(col, row - 1);
				if ( this.board.ContainsKey(belowCellId) && this.board[belowCellId] != null )
				{
					TileInfo belowTile = this.board[belowCellId];
					if ( tile != null && tile.down == belowTile.up )
					{
						score++;
					}
				}
			}
			return score;
		}
		
		// return pairs of cellId, [UDLR] which refer to matching vertices
		// used to count distinct score of multiple tiles (without overlapping)
		// eg. cellId 1, R = cellId 2, L
		public HashSet<string> getTileMatchingVertices(int cellId)
		{
			HashSet<string> matches = new HashSet<string>();
			
			if ( this.board[cellId] != null )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				
				TileInfo tile = this.board[cellId];
				
				// left
				int leftCellId = Board.getCellIdFromColRow(col - 1, row);
				if ( this.board.ContainsKey(leftCellId) && this.board[leftCellId] != null )
				{
					TileInfo leftTile = this.board[leftCellId];
					if ( tile.left == leftTile.right )
					{
						matches.Add(String.Format("{0},L", cellId));
						matches.Add(String.Format("{0},R", leftCellId));
					}
				}
				
				// above
				int aboveCellId = Board.getCellIdFromColRow(col, row - 1);
				if ( this.board.ContainsKey(aboveCellId) && this.board[aboveCellId] != null )
				{
					TileInfo aboveTile = this.board[aboveCellId];
					if ( tile.up == aboveTile.down )
					{
						matches.Add(String.Format("{0},U", cellId));
						matches.Add(String.Format("{0},D", aboveCellId));
					}
				}

				// right
				int rightCellId = Board.getCellIdFromColRow(col + 1, row);
				if ( this.board.ContainsKey(rightCellId) && this.board[rightCellId] != null )
				{
					TileInfo rightTile = this.board[rightCellId];
					if ( tile.right == rightTile.left )
					{
						matches.Add(String.Format("{0},R", cellId));
						matches.Add(String.Format("{0},L", rightCellId));
					}
				}
				
				// below
				int belowCellId = Board.getCellIdFromColRow(col, row + 1);
				if ( this.board.ContainsKey(belowCellId) && this.board[belowCellId] != null )
				{
					TileInfo belowTile = this.board[belowCellId];
					if ( tile.down == belowTile.up )
					{
						matches.Add(String.Format("{0},D", cellId));
						matches.Add(String.Format("{0},U", belowCellId));
					}
				}
			}
			return matches;
		}
		
		// check that region doesn't exceed board boundarry based on perspective of reference col/row
		public int[] getValidRegion(int cellCol, int cellRow, int regionWidth, int regionHeight, int refCol, int refRow, CellRegion parent_region)
		{
			int minCol = cellCol - (refCol - 1);
			int maxCol = cellCol + (regionWidth - refCol);
			int minRow = cellRow - (refRow - 1);
			int maxRow = cellRow + (regionHeight - refRow);
			// check perimeter boundary
//			if ( minCol < 1 || maxCol > Board.num_cols || minRow < 1 || maxRow > Board.num_rows )
			if ( minCol < parent_region.topLeftCellCol || maxCol > parent_region.bottomRightCellCol || minRow < parent_region.topLeftCellRow || maxRow > parent_region.bottomRightCellRow )
			{
				// invalid - return no boundary info
				return new int[0];
			}
			// valid - return boundary info top left cell pos & bottom right cell pos
			return new int[]{minCol,minRow,maxCol,maxRow};
		}
		
		// calculate the number of empty cells within the region
		// updates region.numEmptyCells
		public int calcRegionNumEmptyCells(CellRegion region)
		{
			int numEmptyCells = 0;
			for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol; col++ )
			{
				for ( int row = region.topLeftCellRow; row <= region.bottomRightCellRow; row++ )
				{
					int cellId = Board.getCellIdFromColRow(col, row);
					if ( !this.board.ContainsKey(cellId) || this.board[cellId] == null )
					{
						numEmptyCells++;
					}
				}
			}
			return numEmptyCells;
		}
		
		public int calcRegionMaxScore(CellRegion region)
		{
			// trace perimeter and deduct adjacent empty sides from max score
			// perimeter = 1st col, last col, 1st row, last row
			// exclude cells around border edges as they cannot be matched
			int regionWidth = region.bottomRightCellCol - region.topLeftCellCol + 1;
			int regionHeight = region.bottomRightCellRow - region.topLeftCellRow + 1;
			int maxScore = regionHeight * (regionWidth * 2) + regionWidth + regionHeight;
			int deductPoints = 0;
			int cellId = 0;
			int row = 0;
			for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol; col++ )
			{
				// first row
				row = region.topLeftCellRow;
				if ( col == region.topLeftCellCol )
				{
					// top left corner = left/top
					// left
					cellId = Board.getCellIdFromColRow(col - 1, row);
					if ( cellId > 0 && col > 1 && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
					// top
					cellId = Board.getCellIdFromColRow(col, row - 1);
					if ( cellId > 0 && row > 1 && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
				}
				else if ( col == region.topLeftCellCol + regionWidth - 1 )
				{
					// top right corner = top/right
					// right
					cellId = Board.getCellIdFromColRow(col + 1, row);
					if ( cellId > 0 && col < Board.num_cols && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
					// top
					cellId = Board.getCellIdFromColRow(col, row - 1);
					if ( cellId > 0 && row > 1 && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
				}
				else
				{
					// top edge = top
					cellId = Board.getCellIdFromColRow(col, row - 1);
					if ( cellId > 0 && row > 1 && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
				}

				// last row
				row = region.bottomRightCellRow;
				if ( col == region.topLeftCellCol )
				{
					// bottom left corner = left/bottom
					// left
					cellId = Board.getCellIdFromColRow(col - 1, row);
					if ( cellId > 0 && col > 1 && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
					// bottom
					cellId = Board.getCellIdFromColRow(col, row + 1);
					if ( cellId > 0 && row < Board.num_rows && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
				}
				else if ( col == region.topLeftCellCol + regionWidth - 1 )
				{
					// bottom right corner = right/bottom
					// right
					cellId = Board.getCellIdFromColRow(col + 1, row);
					if ( cellId > 0 && col < Board.num_cols && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
					// bottom
					cellId = Board.getCellIdFromColRow(col, row + 1);
					if ( cellId > 0 && row < Board.num_rows && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
				}
				else
				{
					// bottom row = bottom
					cellId = Board.getCellIdFromColRow(col, row + 1);
					if ( cellId > 0 && row < Board.num_rows && !this.board.ContainsKey(cellId) )
					{
						deductPoints++;
					}
				}
			}
			
			// left / right columns (excluding corner pieces)
			for ( int irow = region.topLeftCellRow + 1; irow < region.bottomRightCellRow; irow++ )
			{
				// left col = left
				cellId = Board.getCellIdFromColRow(region.topLeftCellCol, irow);
				if ( cellId > 0 && region.topLeftCellCol > 1 && !this.board.ContainsKey(cellId) )
				{
					deductPoints++;
				}
				// right col = right
				cellId = Board.getCellIdFromColRow(region.bottomRightCellCol, irow);
				if ( cellId > 0 && region.bottomRightCellCol < Board.num_cols && !this.board.ContainsKey(cellId) )
				{
					deductPoints++;
				}
			}
			
			return maxScore - deductPoints;
		}

		// calculate the actual score for the tiles within the region
		// only count top/left scores as it goes past col/row (to avoid duplicate score for same squares)
		public int calcRegionScore(CellRegion region)
		{
			int score = 0;
			for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol + 1; col++ )
			{
				for ( int row = region.topLeftCellRow; row <= region.bottomRightCellRow + 1; row++ )
				{
					// calc actual score
					int cellId = Board.getCellIdFromColRow(col, row);
					TileInfo tile = null;
					if ( this.board.ContainsKey(cellId) && this.board[cellId] != null )
					{
						tile = this.board[cellId];
					}
					// left
					if ( col > 1 && col <= Board.num_cols && row <= region.bottomRightCellRow )
					{
						int leftCellId = Board.getCellIdFromColRow(col - 1, row);
						if ( this.board.ContainsKey(leftCellId) && this.board[leftCellId] != null )
						{
							TileInfo leftTile = this.board[leftCellId];
							if ( tile != null && tile.left == leftTile.right )
							{
								score++;
							}
						}
					}
					// above
					if ( row > 1 && row <= Board.num_rows && col <= region.bottomRightCellCol )
					{
						int aboveCellId = Board.getCellIdFromColRow(col, row - 1);
						if ( this.board.ContainsKey(aboveCellId) && this.board[aboveCellId] != null )
						{
							TileInfo aboveTile = this.board[aboveCellId];
							if ( tile != null && tile.up == aboveTile.down )
							{
								score++;
							}
						}
					}
				}
			}
			return score;
		}
		
		// returns search filter for selected region
		public string[] getRegionFilter(CellRegion region)
		{
			this.cellScores.Clear();

			List<string> filters = new List<string>();
			for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol + 1; col++ )
			{
				for ( int row = region.topLeftCellRow; row <= region.bottomRightCellRow + 1; row++ )
				{
					int cellId = Board.getCellIdFromColRow(col, row);
					// left
					if ( col > 1 && col <= Board.num_cols )
					{
						int leftCellId = Board.getCellIdFromColRow(col - 1, row);
						if ( this.board.ContainsKey(leftCellId) && this.board[leftCellId] != null )
						{
							TileInfo leftTile = this.board[leftCellId];
							filters.Add(String.Format("{0}, ^{1}...", cellId, leftTile.right));

							// add to cell scores
							this.cellScores.Add(cellId + "L");
						}
					}
					// above
					if ( row > 1 && row <= Board.num_rows )
					{
						int aboveCellId = Board.getCellIdFromColRow(col, row - 1);
						if ( this.board.ContainsKey(aboveCellId) && this.board[aboveCellId] != null )
						{
							TileInfo aboveTile = this.board[aboveCellId];
							filters.Add(String.Format("{0}, ^.{1}..", cellId, aboveTile.down));

							// add to cell scores
							this.cellScores.Add(cellId + "U");
						}
					}
					// right
					if ( col > 0 && col < Board.num_cols )
					{
						int rightCellId = Board.getCellIdFromColRow(col + 1, row);
						if ( this.board.ContainsKey(rightCellId) && this.board[rightCellId] != null )
						{
							TileInfo rightTile = this.board[rightCellId];
							filters.Add(String.Format("{0}, ^..{1}.", cellId, rightTile.left));

							// add to cell scores
							this.cellScores.Add(cellId + "R");
						}
					}
					// below
					if ( row > 0 && row < Board.num_rows )
					{
						int belowCellId = Board.getCellIdFromColRow(col, row + 1);
						if ( this.board.ContainsKey(belowCellId) && this.board[belowCellId] != null )
						{
							TileInfo belowTile = this.board[belowCellId];
							filters.Add(String.Format("{0}, ^...{1}", cellId, belowTile.up));

							// add to cell scores
							this.cellScores.Add(cellId + "D");
						}
					}
				}
			}
			return filters.ToArray();
		}
		
		// returns a list of used tiles, excluding those adjacent to the selected region
		public string[] getUsedTiles(CellRegion region)
		{
			List<string> tiles = new List<string>();
			// increase size of excluded region so that tiles adjacent to it are also excluded
			CellRegion excludeRegion = new CellRegion(region.topLeftCellCol - 1, region.topLeftCellRow - 1, region.bottomRightCellCol + 1, region.bottomRightCellRow + 1);
			foreach ( int cellId in this.board.Keys )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				if ( !excludeRegion.intersects(col, row) )
				{
					tiles.Add(this.board[cellId].pattern);
				}
			}
			return tiles.ToArray();
		}
		
		// returns a list of available tiles including used tiles that are adjacent to empty cells (that are not in or adjacent to the selected region)
		public string[] getTilesWithAdjacentToEmpty(CellRegion region)
		{
			List<string> tiles = new List<string>();

			// get available tiles
			foreach ( string tile in this.tileSet.Values )
			{
				if ( !this.usedTiles.ContainsKey(tile) )
				{
					tiles.Add(tile);
				}
			}
			
			// track tiles that have been added, ie. only add each tile once
			List<int> adjacentTiles = new List<int>();
			
			// increase size of excluded region so that tiles adjacent to it are also excluded
			CellRegion excludeRegion = new CellRegion(region.topLeftCellCol - 1, region.topLeftCellRow - 1, region.bottomRightCellCol + 1, region.bottomRightCellRow + 1);
			
			foreach ( int cellId in this.empty_cells )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				// find adjacent tiles that are not inside the excluded region
				// left
				if ( col > 1 && !excludeRegion.intersects(col - 1, row) )
				{
					int leftCellId = Board.getCellIdFromColRow(col - 1, row);
					if ( !adjacentTiles.Contains(leftCellId) && this.board.ContainsKey(leftCellId) )
					{
						tiles.Add(this.board[leftCellId].pattern);
						adjacentTiles.Add(leftCellId);
					}
				}
				// above
				if ( row > 1 && !excludeRegion.intersects(col, row - 1) )
				{
					int aboveCellId = Board.getCellIdFromColRow(col, row - 1);
					if ( !adjacentTiles.Contains(aboveCellId) && this.board.ContainsKey(aboveCellId) )
					{
						tiles.Add(this.board[aboveCellId].pattern);
						adjacentTiles.Add(aboveCellId);
					}
				}
				// right
				if ( col < Board.num_cols && !excludeRegion.intersects(col + 1, row) )
				{
					int rightCellId = Board.getCellIdFromColRow(col + 1, row);
					if ( !adjacentTiles.Contains(rightCellId) && this.board.ContainsKey(rightCellId) )
					{
						tiles.Add(this.board[rightCellId].pattern);
						adjacentTiles.Add(rightCellId);
					}
				}
				// below
				if ( row < Board.num_rows && !excludeRegion.intersects(col, row + 1) )
				{
					int belowCellId = Board.getCellIdFromColRow(col, row + 1);
					if ( !adjacentTiles.Contains(belowCellId) && this.board.ContainsKey(belowCellId) )
					{
						tiles.Add(this.board[belowCellId].pattern);
						adjacentTiles.Add(belowCellId);
					}
				}
			}
			return tiles.ToArray();
		}
		
		// remove tiles from within selected region
		public void clearRegion(CellRegion region)
		{
			for ( int row = region.topLeftCellRow; row <= region.bottomRightCellRow; row++ )
			{
				for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol; col++ )
				{
					int cellId = Board.getCellIdFromColRow(col, row);
					if ( this.board.ContainsKey(cellId) )
					{
						this.board.Remove(cellId);
					}
				}
			}
		}
		
		public void setProgressCallback(TileSwapEvent func)
		{
			this.progressCallback = func;
		}
		
		// return a list of cells around the perimeter of the region that contain adjacent tiles
		// used to check the perimeter score of opportunities that are found
		public SortedDictionary<int, int> getPerimeterTileCells(CellRegion region)
		{
			SortedDictionary<int, int> perimeterTileCells = new SortedDictionary<int, int>();
			int adjCellId = 0;
			int row = 0;
			int regionWidth = region.bottomRightCellCol - region.topLeftCellCol + 1;
			int regionHeight = region.bottomRightCellRow - region.topLeftCellRow + 1;
			for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol; col++ )
			{
				// first row
				row = region.topLeftCellRow;
				int cellId = Board.getCellIdFromColRow(col, row);
				if ( col == region.topLeftCellCol )
				{
					// top left corner = left/top
					// left
					adjCellId = Board.getCellIdFromColRow(col - 1, row);
					if ( adjCellId > 0 && col > 1 && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
					// top
					adjCellId = Board.getCellIdFromColRow(col, row - 1);
					if ( adjCellId > 0 && row > 1 && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
				}
				else if ( col == region.topLeftCellCol + regionWidth - 1 )
				{
					// top right corner = top/right
					// right
					adjCellId = Board.getCellIdFromColRow(col + 1, row);
					if ( adjCellId > 0 && col < Board.num_cols && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
					// top
					adjCellId = Board.getCellIdFromColRow(col, row - 1);
					if ( adjCellId > 0 && row > 1 && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
				}
				else
				{
					// top edge = top
					adjCellId = Board.getCellIdFromColRow(col, row - 1);
					if ( adjCellId > 0 && row > 1 && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
				}

				// last row
				row = region.bottomRightCellRow;
				cellId = Board.getCellIdFromColRow(col, row);
				if ( col == region.topLeftCellCol )
				{
					// bottom left corner = left/bottom
					// left
					adjCellId = Board.getCellIdFromColRow(col - 1, row);
					if ( adjCellId > 0 && col > 1 && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
					// bottom
					adjCellId = Board.getCellIdFromColRow(col, row + 1);
					if ( adjCellId > 0 && row < Board.num_rows && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
				}
				else if ( col == region.topLeftCellCol + regionWidth - 1 )
				{
					// bottom right corner = right/bottom
					// right
					adjCellId = Board.getCellIdFromColRow(col + 1, row);
					if ( adjCellId > 0 && col < Board.num_cols && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
					// bottom
					adjCellId = Board.getCellIdFromColRow(col, row + 1);
					if ( adjCellId > 0 && row < Board.num_rows && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
				}
				else
				{
					// bottom row = bottom
					adjCellId = Board.getCellIdFromColRow(col, row + 1);
					if ( adjCellId > 0 && row < Board.num_rows && this.board.ContainsKey(adjCellId) )
					{
						this.dictIncValue(perimeterTileCells, cellId);
					}
				}
			}
			
			// left / right columns (excluding corner pieces)
			for ( int irow = region.topLeftCellRow + 1; irow < region.bottomRightCellRow; irow++ )
			{
				// left col = left
				int cellId = Board.getCellIdFromColRow(region.topLeftCellCol, irow);
				adjCellId = Board.getCellIdFromColRow(region.topLeftCellCol - 1, irow);
				if ( adjCellId > 0 && region.topLeftCellCol > 1 && this.board.ContainsKey(adjCellId) )
				{
					this.dictIncValue(perimeterTileCells, cellId);
				}

				// right col = right
				cellId = Board.getCellIdFromColRow(region.topLeftCellCol, irow);
				adjCellId = Board.getCellIdFromColRow(region.bottomRightCellCol, irow);
				if ( adjCellId > 0 && region.bottomRightCellCol < Board.num_cols && this.board.ContainsKey(adjCellId) )
				{
					this.dictIncValue(perimeterTileCells, cellId);
				}
			} 
			
			return perimeterTileCells;
		}
	
		// increase value of dictionary variable, if doesn't exist then create & set to 1
		public void dictIncValue(SortedDictionary<int, int> dict, int key)
		{
			if ( dict.ContainsKey(key) )
			{
				dict[key]++;
			}
			else
			{
				dict[key] = 1;
			}
		}
		
	}
}

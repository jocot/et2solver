/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 3/12/2009
 * Time: 5:57 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET2Solver
{
	/// <summary>
	/// Description of Solver.
	/// </summary>
	public class Solver2x2
	{
		// debug level = 0 = off, 1 = low verbosity, 2 = medium verbosity, 3 = high verbosity
		public int debug_level = 1;
		public int debug_default_level = 1;

		public TileSearch ts;
			
		// save generated lists to file
		public bool saveToFile = false;
		
		// tileset for solving
		public List<int> usedTiles = new List<int>();
		public Hashtable board = new Hashtable();
		
		// tileId, cellpos - position of where the hint tile exists in 2x2 cell (1,2,3,4)
		public Dictionary<int, int> hintTiles = new Dictionary<int, int>();
		// hint patterns
		public Dictionary<int, string> hintPatterns = new Dictionary<int, string>();
		
		// source lists
		public bool lists_loaded = false;
		public Hashtable tilelists = new Hashtable();
		
		// queue data
		// queue list for each of the 64 cells
		public Dictionary<int, List<string>> queueList = new Dictionary<int, List<string>>();
//		public Hashtable queueList = new Hashtable();
		// progress of current queue
		public List<int> queueProgress = new List<int>(new int[64]);

		// solve path - list of cellIds (1-64)
		public List<int> solve_path = new List<int>();
		// path id of current cell
		public int currentPathId = 0;
		
		// auto save settings
		public Int64 lastSaveIteration = 0;
		// autosave after x iterations
		public int autoSaveInterval = 100000;
		// autosave when tiles placed reaches this (auto increments by 4)
		public int autoSaveMaxTilesPlaced = 108;

		// solve stats
		public Int32 maxQueueSize = 0;
		public Int64 numIterations = 0;
		public Int64 iterationSize = 0;
		public int mostTilesPlaced = 0;
		
		// pause/resume
		public bool isPaused = false;
		public bool isResumed = false;

		// random seed generator
		public bool useRandom = false;
		public bool randomSeeds = false;
		public int start_seed = 0;
		public int seed_step = 1;
		public Random r = new Random();
		public Int32 seed;

		// max number of iterations before restarting / stopping
		public int maxIterations = 0;

		// run multiple jobs
		public bool run_multiple = false;
		public int num_runs = 1;
		public int last_run = 0;
		
		public Solver2x2()
		{
			Program.TheMainForm.selSolver2Method.Items.Add("border_bl_br_tr_tl");
			Program.TheMainForm.selSolver2Method.Items.Add("border_tl_tr_br_bl");
			Program.TheMainForm.selSolver2Method.Items.Add("border_x");
			Program.TheMainForm.selSolver2Method.Items.Add("rows_right_up");
			Program.TheMainForm.selSolver2Method.Items.Add("cols_up_right");
			Program.TheMainForm.selSolver2Method.Items.Add("spiral_in_bl_br_tr_tl");
			Program.TheMainForm.selSolver2Method.Items.Add("spiral_out_bl_br_tr_tl");
			Program.TheMainForm.selSolver2Method.SelectedIndex = 0;
			Program.TheMainForm.selSolver2Method.Update();
			
			this.loadQueueList();
		}
		
		/* 
		 generate 3x3 for each internal tile
		 * positions: BL = r1, TL = r2, TR = r3, BR = r4 (rotate grid)
		 loc = corner, edge, internal
		*/
		public void generateList(string loc)
		{
			if ( Board.title == "" )
			{
				System.Windows.Forms.MessageBox.Show("Please load a tileset");
				return;
			}
			Program.TheMainForm.timer.start("generateList");
			this.log("generateList(" + Board.title + ":" + loc + ")");

			TileSearch ts = new TileSearch();
			ts.loadTileset(Board.title);
			ArrayList tilelist;
			int numSearched = 0;

			switch ( loc )
			{
				case "2x2_corner_tl":
					/*
					 * 2x2 grid (top left corner)
					 * 1,2
					 * 3,4
					*/
					ts.save_overwrite = false;
					tilelist = ts.findTiles("--[A-V]{2}");
					foreach ( TileInfo tile in tilelist )
					{
						numSearched++;
						this.log("tile " + numSearched.ToString() + " / " + tilelist.Count.ToString() + " : " + tile.title);
						ts.newSearch();
						if ( this.saveToFile )
						{
							if ( !ts.saveListAt(4, Board.title + "_" + loc) )
							{
								break;
							}
						}
						ts.addTileToList(1, tile);
						ts.includeFilter(2, "[A-V]-[A-V]{2}");
						ts.includeFilter(3, "-[A-V]{3}");
						ts.excludeFilter(4, "-");
						ts.addSearch("match_adjacent", 2, "1@R");
						ts.addSearch("match_adjacent", 3, "1@D");
						ts.addSearch("match_adjacent", 4, "3@R,2@D");
						ts.doSearch();
						this.log(ts.countUniqueTiles().ToString() + " unique tiles");
						this.log("");
					}
					break;

				case "2x2_edge_left":
					/*
					 * 2x2 grid (on left edge of board)
					 * 1,2
					 * 3,4
					*/
					ts.save_overwrite = false;
					tilelist = ts.findTiles("-[A-V]{3}");
					foreach ( TileInfo tile in tilelist )
					{
						numSearched++;
						this.log("tile " + numSearched.ToString() + " / " + tilelist.Count.ToString() + " : " + tile.title);
						ts.newSearch();
						if ( this.saveToFile )
						{
							if ( !ts.saveListAt(4, Board.title + "_" + loc) )
							{
								break;
							}
						}
						ts.addTileToList(1, tile);
						ts.excludeFilter(2, "-");
						ts.includeFilter(3, "^-[A-V]{3}");
						ts.excludeFilter(4, "-");
						ts.addSearch("match_adjacent", 2, "1@R");
						ts.addSearch("match_adjacent", 3, "1@D");
						ts.addSearch("match_adjacent", 4, "3@R,2@D");
						ts.doSearch();
						this.log(ts.countUniqueTiles().ToString() + " unique tiles");
						this.log("");
						if ( !this.saveToFile && numSearched >= 10 )
//						if ( numSearched >= 10 )
						{
							break;
						}
					}
					break;

				case "2x2_internal":
					/*
					 * 2x2 grid
					 * 1,2
					 * 3,4
					*/
					tilelist = ts.findTiles("[F-V]{4}");
					foreach ( TileInfo tile in tilelist )
					{
						numSearched++;
						this.log("tile " + numSearched.ToString() + " / " + tilelist.Count.ToString() + " : " + tile.title);
						ts.newSearch();
						if ( this.saveToFile )
						{
							if ( !ts.saveListAt(4, Board.title + "_" + loc + "_tile_" + tile.title) )
							{
								break;
							}
						}
						ts.addTileToList(1, tile);
						ts.excludeFilter(2, "-");
						ts.excludeFilter(3, "-");
						ts.excludeFilter(4, "-");
						ts.addSearch("match_adjacent", 2, "1@R");
						ts.addSearch("match_adjacent", 3, "1@D");
						ts.addSearch("match_adjacent", 4, "3@R,2@D");
						ts.doSearch();
						this.log(ts.countUniqueTiles().ToString() + " unique tiles");
						this.log("");
						if ( !this.saveToFile && numSearched >= 10 )
//						if ( numSearched >= 10 )
						{
							break;
						}
					}
					break;

				case "2x2_internal_139":
					/*
					 * 2x2 grid for hint piece tile 139
					 * 2,1
					 * 4,3
					*/
					tilelist = ts.findTiles("LHII");
					foreach ( TileInfo tile in tilelist )
					{
						numSearched++;
						this.log("tile " + numSearched.ToString() + " / " + tilelist.Count.ToString() + " : " + tile.title);
						ts.newSearch();
						if ( this.saveToFile )
						{
							if ( !ts.saveListAt(4, Board.title + "_internal_tile_" + tile.title) )
							{
								break;
							}
						}
						ts.addTileToList(1, tile);
						ts.excludeFilter(2, "-");
						ts.excludeFilter(3, "-");
						ts.excludeFilter(4, "-");
						ts.addSearch("match_adjacent", 2, "1@L");
						ts.addSearch("match_adjacent", 3, "1@D");
						ts.addSearch("match_adjacent", 4, "2@D,3@L");
						ts.doSearch();
						this.log(ts.countUniqueTiles().ToString() + " unique tiles");
						this.log("");
					}
					break;

				case "3x3_corner":
					/*
					 * 3x3 grid (bottom left)
					 * 5,7,9
					 * 3,1,8
					 * 2,4,6
					*/
					tilelist = ts.findTiles("[F-V]{4}");
					foreach ( TileInfo tile in tilelist )
					{
						numSearched++;
						this.log("tile " + numSearched.ToString() + " / " + tilelist.Count.ToString() + " : " + tile.title);
						ts.newSearch();
						ts.addTileToList(1, tile);
						ts.includeFilter(2, "^-");
						ts.includeFilter(3, "^-");
						ts.includeFilter(4, "-$");
						ts.includeFilter(5, "^-");
						ts.includeFilter(6, "-$");
						ts.excludeFilter(7, "-");
						ts.excludeFilter(8, "-");
						ts.excludeFilter(9, "-");
						ts.addSearch("match", 2, "-..-");
						ts.addSearch("match_adjacent", 3, "1@L,2@U");
						ts.addSearch("match_adjacent", 4, "1@D,2@R");
						ts.addSearch("match_adjacent", 5, "3@U");
						ts.addSearch("match_adjacent", 6, "4@R");
						ts.addSearch("match_adjacent", 7, "1@U,5@R");
						ts.addSearch("match_adjacent", 8, "1@R,6@U");
						ts.addSearch("match_adjacent", 9, "7@R,8@U");
						ts.doSearch();
						this.log("");
						
						//this.log(ts.getPosListAsString(9));
						if ( numSearched >= 1 )
						{
							break;
						}
					}
					break;

				case "3x3_internal":
					/*
					 * 3x3 grid (internal)
					 * 8,5,7
					 * 2,1,6
					 * 4,3,9
					*/
					tilelist = ts.findTiles("[F-V]{4}");
					foreach ( TileInfo tile in tilelist )
					{
						numSearched++;
						this.log("tile " + numSearched.ToString() + " / " + tilelist.Count.ToString() + " : " + tile.title);
						ts.newSearch();
						ts.addTileToList(1, tile);
						ts.excludeFilter(2, "-");
						ts.excludeFilter(3, "-");
						ts.excludeFilter(4, "-");
						ts.excludeFilter(5, "-");
						ts.excludeFilter(6, "-");
						ts.excludeFilter(7, "-");
						ts.excludeFilter(8, "-");
						ts.excludeFilter(9, "-");
						ts.addSearch("match_adjacent", 2, "1@L");
						ts.addSearch("match_adjacent", 3, "1@D");
						ts.addSearch("match_adjacent", 4, "2@D,3@L");
						ts.addSearch("match_adjacent", 5, "1@U");
						ts.addSearch("match_adjacent", 6, "1@R");
						ts.addSearch("match_adjacent", 7, "5@R,6@U");
						ts.addSearch("match_adjacent", 8, "2@U,5@L");
						ts.addSearch("match_adjacent", 9, "3@R,6@D");
						ts.doSearch();
						break;
					}
					break;

			}
			//this.log(ts.getPosListAsString(9));
			//this.log(ts.getResultCount());
			this.log("");
			Program.TheMainForm.timer.stop("generateList");
			this.log(Program.TheMainForm.timer.results("generateList"));
			this.log("");
		}
		
		public int getTileId(string pattern)
		{
			int tileId = 0;
			int pos = this.ts.tileset_list.IndexOf(pattern);
//			Console.WriteLine(pos.ToString() + "." + pattern);
			if ( pos > -1 )
			{
				/*
				 * 1-4 = 1
				 * 5-8 = 2
				 * 9-12 = 3
				 * 13-16 = 4
				 * tileId = floor(pos/4) + 1
				*/
				tileId = (int)Math.Floor(pos / 4.0) + 1;
			}
			return tileId;
		}
		
		public int getTileRotationByPattern(string pattern)
		{
			int pos = this.ts.tileset_list.IndexOf(pattern);
			return pos % 4 + 1;
		}

		public bool setSolvePath()
		{
			// set solve path / method
			int inner = 0;
			int outer = 0;
			int length = 0;
			this.solve_path = new System.Collections.Generic.List<int>();
			Program.TheMainForm.labelSolver2Method.Text = Program.TheMainForm.selSolver2Method.Text;
			switch ( Program.TheMainForm.selSolver2Method.Text )
			{
				case "border_bl_br_tr_tl":
					// 28 2x2 cells around the border, starting from bottom left
					this.addRowsToPath("right", 8);
					this.addColsToPath("up", 8);
					this.addRowsToPath("left", 1);
					this.addColsToPath("down", 1);
					break;
				case "border_tl_tr_br_bl":
					// 28 2x2 cells around the border, starting from top left
					this.addRowsToPath("right", 1);
					this.addColsToPath("down", 8);
					this.addRowsToPath("left", 8);
					this.addColsToPath("up", 1);
					break;
				case "border_x":
					// border from X formation inwards, starting with corners, edges, inner corners etc
					this.addCellsToPath(57,49,58,64,63,56,8,16,7,1,2,9,41,59,62,48,24,6,3,17,60,61,40,32,5,4,25,33);
					break;
				case "rows_right_up":
					// by row, from bottom to top (left-right)
					this.addRowsToPath("right", 8,7,6,5,4,3,2,1);
					break;
				case "cols_up_right":
					// by column, from left to right (bottom-top)
					this.addColsToPath("up", 1,2,3,4,5,6,7,8);
					break;
				case "spiral_in_bl_br_tr_tl":
					// spiral from outside border inwards, from bottom left, going right/up, then left/down
					inner = 1;
					outer = 8;
					for ( int i = 1; i <= 4; i++ )
					{
						this.addRowsToPath("right", outer);
						this.addColsToPath("up", outer);
						this.addRowsToPath("left", inner);
						this.addColsToPath("down", inner);
//						Program.TheMainForm.log(outer.ToString() + "-" + inner.ToString());
						outer--;
						inner++;
					}
					break;
				case "spiral_out_bl_br_tr_tl":
					// spiral outwards from center, from bottom left, going right/up, then left/down
					// row: 4,5 to 5,5
					// col: 5,5 to 5,4
					// row: 5,4 to 4,4
					// col: 4,4 to 4,5
					
					// row: 3,6 to 6,6
					// col: 6,6 to 6,3
					// row: 6,3 to 3,3
					// col: 3,3 to 3,6

					inner = 4;
					outer = 5;
					length = 2;
					for ( int i = 1; i <= 4; i++ )
					{
//						this.addRowRange("right", inner, outer, length);
						this.addRowRange("right", 4, outer, length - (4 - inner));
						this.addColRange("up", outer, outer, length);
						this.addRowRange("left", outer, inner, length);
						this.addColRange("down", inner, inner, length);
						// always start from col 4 (below hint tile
						// reason: so that it's adjacent to a previous tile when forming next ring
						if ( inner < 4 )
						{
							this.addRowRange("right", inner, outer, 4 - inner);
						}
						Program.TheMainForm.log(outer.ToString() + "-" + inner.ToString());
						outer++;
						inner--;
						length += 2;
					}
					break;
				default:
					System.Windows.Forms.MessageBox.Show("Please choose a solve method.");
					return false;
			}
			this.logPath();
			return true;
		}
		
		public ArrayList getColRowBy2x2CellId(int cellId)
		{
			// returns arraylist of 4 x [col,row] for the cells referenced by the specified 2x2 cellId
			// TL = 1, TR = 8, BL = 57, BR = 64
			ArrayList rv = new ArrayList();
			int[] colrow;

			int row = (int)Math.Ceiling((double)cellId / 8d);
			int col = ((cellId - (row-1) * 8) - 1) * 2 + 1;
			row = (row - 1) * 2 + 1;
//			System.Console.WriteLine(cellId.ToString() + " = " + col.ToString() + "," + row.ToString());

			// tile 1 TL
			colrow = new int[2];
			colrow[0] = col;
			colrow[1] = row;
			rv.Add(colrow);

			// tile 2 TR
			colrow = new int[2];
			colrow[0] = col + 1;
			colrow[1] = row;
			rv.Add(colrow);

			// tile 3 BL
			colrow = new int[2];
			colrow[0] = col;
			colrow[1] = row + 1;
			rv.Add(colrow);

			// tile 4 BR
			colrow = new int[2];
			colrow[0] = col + 1;
			colrow[1] = row + 1;
			rv.Add(colrow);

			return rv;
		}
		
		public int get2x2CellIdByColRow(int col, int row)
		{
			// col = 1-8, row = 1-8
			int cellId = (row - 1) * 8 + col;
			return cellId;
		}
		
		private void logPath()
		{
			this.log("log path: " + Program.TheMainForm.selSolver2Method.Text + ", length = " + this.solve_path.Count.ToString());
			List<string> line = new List<string>();
			foreach ( int cellId in this.solve_path )
			{
				line.Add(cellId.ToString());
			}
			this.log(String.Join(",", line.ToArray()));
		}
		
		public void addCellsToPath(params int[] cells)
		{
			// skips adding duplicates (to allow spiral path to be added easily by row/col
			foreach ( int cellId in cells )
			{
//				if ( !this.solve_path.Contains(cellId) )
				if ( cellId >= 1 && cellId <= 64 && !this.solve_path.Contains(cellId) )
				{
					this.solve_path.Add(cellId);
				}
			}
		}
		
		public void addColsToPath(string direction, params int[] cols)
		{
			switch ( direction )
			{
				case "up":
					foreach ( int col in cols )
					{
						for ( int row = 8; row >= 1; row-- )
						{
							int cellId = this.get2x2CellIdByColRow(col, row);
							this.addCellsToPath(cellId);
						}
					}
					break;
				case "down":
					foreach ( int col in cols )
					{
						for ( int row = 1; row <= 8; row++ )
						{
							int cellId = this.get2x2CellIdByColRow(col, row);
							this.addCellsToPath(cellId);
						}
					}
					break;
			}
		}
		
		public void addRowsToPath(string direction, params int[] rows)
		{
			switch ( direction )
			{
				case "right":
					foreach ( int row in rows )
					{
						for ( int col = 1; col <= 8; col++ )
						{
							int cellId = this.get2x2CellIdByColRow(col, row);
							this.addCellsToPath(cellId);
						}
					}
					break;
				case "left":
					foreach ( int row in rows )
					{
						for ( int col = 8; col >= 1; col-- )
						{
							int cellId = this.get2x2CellIdByColRow(col, row);
							this.addCellsToPath(cellId);
						}
					}
					break;
			}
		}

		public void addRowRange(string direction, int col, int row, int length)
		{
			switch ( direction )
			{
				case "right":
					for ( int c = col; c < col + length; c++ )
					{
						int cellId = this.get2x2CellIdByColRow(c, row);
						this.addCellsToPath(cellId);
					}
					break;
				case "left":
					for ( int c = col; c > col - length; c-- )
					{
						int cellId = this.get2x2CellIdByColRow(c, row);
						this.addCellsToPath(cellId);
					}
					break;
			}
		}
		
		public void addColRange(string direction, int col, int row, int length)
		{
			switch ( direction )
			{
				case "up":
					for ( int r = row; r > row - length; r-- )
					{
						int cellId = this.get2x2CellIdByColRow(col, r);
						this.addCellsToPath(cellId);
					}
					break;
				case "down":
					for ( int r = row; r < row + length; r++ )
					{
						int cellId = this.get2x2CellIdByColRow(col, r);
						this.addCellsToPath(cellId);
					}
					break;
			}
		}
		
		/*
		public List<int> cellRange(int start, int end)
		{
			List<int> rv = new List<int>();
			if ( start < end )
			{
				// iterate forwards
				for ( int i = start; i <= end; i++ )
				{
					rv.Add(i);
				}
			}
			else if ( start > end )
			{
				// iterate backwards
				for ( int i = start; i >= end; i-- )
				{
					rv.Add(i);
				}
			}
			return rv;
		}
		*/
		
		public void updateStats()
		{
			Program.TheMainForm.labelNumIterations.Text = this.numIterations.ToString();

			int cellId = this.getCellId();
			Program.TheMainForm.labelS2CellNum.Text = this.currentPathId.ToString();
			Program.TheMainForm.labelS2PathLength.Text = this.solve_path.Count.ToString();
			Program.TheMainForm.labelS2CellId.Text = cellId.ToString();
			
			Program.TheMainForm.labelTilesPlaced.Text = this.usedTiles.Count.ToString();
			Program.TheMainForm.labelS2MostTilesPlaced.Text = this.mostTilesPlaced.ToString();
			
			// refresh display
			Program.TheMainForm.labelNumIterations.Update();
			Program.TheMainForm.labelS2CellNum.Update();
			Program.TheMainForm.labelS2PathLength.Update();
			Program.TheMainForm.labelS2CellId.Update();
			Program.TheMainForm.labelTilesPlaced.Update();
		}
		
		public void updateQueueStats()
		{
			int cellId = this.getCellId();
			Program.TheMainForm.labelQueueProgress.Text = this.queueProgress[cellId-1].ToString();
			Program.TheMainForm.labelS2QueueSize.Text = this.queueSize(cellId).ToString();
			Program.TheMainForm.labelQueueProgress.Update();
			Program.TheMainForm.labelS2QueueSize.Update();
		}
		
		public string getListId(int cellId)
		{
			// select appropriate list based on cellId
			// corner = 1, 8, 57, 64
			// edge = col 1, col 15, row 1, row 15 (2x2 positions)
			// else = internal
			ArrayList lcolrow = this.getColRowBy2x2CellId(cellId);
			/*
			foreach ( int[] cr in lcolrow )
			{
				this.log("col/row : " + cr[0].ToString() + "," + cr[1].ToString());
			}
			*/
			int[] colrow = (int[])lcolrow[0];
			string listId = "internal";
			if ( cellId == 1 )
			{
				listId = "corner_tl";
			}
			else if ( cellId == 8 )
			{
				listId = "corner_tr";
			}
			else if ( cellId == 57 )
			{
				listId = "corner_bl";
			}
			else if ( cellId == 64 )
			{
				listId = "corner_br";
			}
			else if ( colrow[0] == 1 )
			{
				listId = "edge_left";
			}
			else if ( colrow[0] == 15 )
			{
				listId = "edge_right";
			}
			else if ( colrow[1] == 1 )
			{
				listId = "edge_top";
			}
			else if ( colrow[1] == 15 )
			{
				listId = "edge_bottom";
			}
//			this.log("cellId: " + cellId.ToString() + " | col/row : " + colrow[0].ToString() + "," + colrow[1].ToString() + " | list : " + listId);
			return listId;
		}

		public List<string> loadList(string filename)
		{
			string listpath = System.IO.Path.Combine("tilelists\\" + Board.title, filename + ".txt");
			if ( System.IO.File.Exists(listpath) )
			{
				List<string> list = new List<string>(System.IO.File.ReadAllLines(listpath));
				this.log("list: " + filename + " loaded with " + list.Count.ToString() + " items.");
				return list;
			}
			else
			{
				return new List<string>();
			}
		}
		
		public void loadHints()
		{
//				# cellId,cellPos,tileId,searchPattern 
//				36,2,139,....,LHII,....,....
				this.tilelists.Add("hints", this.loadList("2x2_hints"));
				List<string> hintlist = (List<string>)this.tilelists["hints"];
				foreach ( string hint in hintlist )
				{
					string[] hintdata = hint.Split(',');
					if ( hintdata.Length == 7 )
					{
						int cellId = Convert.ToInt16(hintdata[0]);
						int cellPos = Convert.ToInt16(hintdata[1]);
						int tileId = Convert.ToInt16(hintdata[2]);
						string pattern = hintdata[3] + "," + hintdata[4] + "," + hintdata[5] + "," + hintdata[6];
						this.logt("loaded hint tile for cell " + cellId.ToString() + "," + cellPos.ToString() + ", tile " + tileId.ToString() + pattern, 1);
						this.hintPatterns.Add(cellId, pattern);
						this.hintTiles.Add(tileId, cellPos);
					}
				}
		}

		public bool loadLists()
		{
			if ( Board.title == "" )
			{
				System.Windows.Forms.MessageBox.Show("Please load a tileset");
				return false;
			}

			Program.TheMainForm.timer.start("loadLists");
			
			try
			{
				// load 1024 tileset
				this.ts = new TileSearch();
				this.ts.loadTileset(Board.title);
				
				// load 2x2 hints
				this.loadHints();

				// load 2x2 lists
				this.tilelists.Add("corner_bl", this.loadList("2x2_corner_bl"));
				this.tilelists.Add("corner_br", this.loadList("2x2_corner_br"));
				this.tilelists.Add("corner_tl", this.loadList("2x2_corner_tl"));
				this.tilelists.Add("corner_tr", this.loadList("2x2_corner_tr"));
				this.tilelists.Add("edge_left", this.loadList("2x2_edge_left"));
				this.tilelists.Add("edge_right", this.loadList("2x2_edge_right"));
				this.tilelists.Add("edge_top", this.loadList("2x2_edge_top"));
				this.tilelists.Add("edge_bottom", this.loadList("2x2_edge_bottom"));
//				this.tilelists.Add("internal", this.loadList("2x2_internal"));
			}
			catch (Exception e)
			{
				Program.TheMainForm.log("Exception: " + e.Message + ", source: " + e.StackTrace);
				Program.TheMainForm.log("Could not load lists...");
				return false;
			}
			Program.TheMainForm.timer.stop("loadLists");
			this.log(Program.TheMainForm.timer.results("loadLists"));
			this.lists_loaded = true;
			return true;
		}
		
		public List<string> getMatchLocations(int cellId)
		{
			ArrayList lcolrow = this.getColRowBy2x2CellId(cellId);
			int[] colrow = (int[])lcolrow[0];
			List<string> matches = new List<string>();
			if ( colrow[0] > 1 )
			{
				matches.Add("L");
			}
			if ( colrow[0] < 15 )
			{
				matches.Add("R");
			}
			if ( colrow[1] > 1 )
			{
				matches.Add("U");
			}
			if ( colrow[1] < 15 )
			{
				matches.Add("D");
			}
			return matches;
		}
		
		public string getSearchPattern(int cellId)
		{
			// search = LTRB,LTRB,LTRB,LTRB
			string[] search = new string[19];
			for ( int i = 1; i <= search.Length; i++ )
			{
				if ( i == 5 || i == 10 || i == 15 )
				{
					search[i-1] = ",";
				}
				else
				{
					search[i-1] = ".";
				}
			}
			
			// get match locations
			List<string> matches = this.getMatchLocations(cellId);
			
			// check for adjacent tiles
			// search = LTRB,LTRB,LTRB,LTRB
			ArrayList tiles = new ArrayList();
			TileInfo tile;
			foreach ( string loc in matches )
			{
				if ( loc == "L" )
				{
					// 1L,3L = tiles 2R,4R
					if ( this.board.ContainsKey(cellId-1) )
					{
						tiles = (ArrayList)this.board[cellId-1];
						if ( tiles.Count == 4 )
						{
							// 1L - 2R
							tile = (TileInfo)tiles[1];
							search[0] = tile.right;
							// 3L - 4R
							tile = (TileInfo)tiles[3];
							search[10] = tile.right;
						}
						else
						{
							this.log("getSearchPattern(" + (cellId-1).ToString() + ") = " + tiles.Count.ToString());
						}
					}
				}
				else if ( loc == "U" )
				{
					// 1U,2U = tiles 3D,4D
					if ( this.board.ContainsKey(cellId-8) )
					{
						tiles = (ArrayList)this.board[cellId-8];
						if ( tiles.Count == 4 )
						{
							// 1U - 3D
							tile = (TileInfo)tiles[2];
							search[1] = tile.down;
							// 2U - 4D
							tile = (TileInfo)tiles[3];
							search[6] = tile.down;
						}
						else
						{
							this.log("getSearchPattern(" + (cellId-1).ToString() + ") = " + tiles.Count.ToString());
						}
					}
				}
				else if ( loc == "R" )
				{
					// 2R,4R = tiles 1L,3L
					if ( this.board.ContainsKey(cellId+1) )
					{
						tiles = (ArrayList)this.board[cellId+1];
						if ( tiles.Count == 4 )
						{
							// 2R - 1L
							tile = (TileInfo)tiles[0];
							search[7] = tile.left;
							// 4R - 3L
							tile = (TileInfo)tiles[2];
							search[17] = tile.left;
						}
						else
						{
							this.log("getSearchPattern(" + (cellId-1).ToString() + ") = " + tiles.Count.ToString());
						}
					}
				}
				else if ( loc == "D" )
				{
					// 3D,4D = tiles 1U,2U
					if ( this.board.ContainsKey(cellId+8) )
					{
						tiles = (ArrayList)this.board[cellId+8];
						if ( tiles.Count == 4 )
						{
							// 3D - 1U
							tile = (TileInfo)tiles[0];
							search[13] = tile.up;
							// 4D - 2U
							tile = (TileInfo)tiles[1];
							search[18] = tile.up;
						}
						else
						{
							this.log("getSearchPattern(" + (cellId-1).ToString() + ") = " + tiles.Count.ToString());
						}
					}
				}
			}
			
			// load hint tiles if available
			string hintpattern = this.getHintPattern(cellId);
			if ( hintpattern != "" )
			{
				for ( int i = 0; i < hintpattern.Length; i++ )
				{
					if ( hintpattern[i] != '.' )
					{
						search[i] = hintpattern[i].ToString();
					}
				}
			}
			
			return String.Concat(search);
		}
		
		public void findMatchingList(int cellId)
		{
			Program.TheMainForm.timer.start("findMatchingList");
			List<string> results = new List<string>();
			string search = this.getSearchPattern(cellId);
			string listId = this.getListId(cellId);
			List<string> tilelist;
			
			this.log("findMatchingList(" + this.currentPathId.ToString() + ". cellId: " + cellId.ToString() + "):" + search);
			// use sql
			if ( listId == "internal" )
			{
				List<string> lsearch = new List<string>(search.Split(','));
				string sql = "SELECT tileA,tileB,tileC,tileD"
					+ " FROM internal"
					+ " WHERE tileA REGEXP '" + lsearch[0] + "' AND tileB REGEXP '" + lsearch[1] + "' AND tileC REGEXP '" + lsearch[2] + "' AND tileD REGEXP '" + lsearch[3] + "'";
				System.Windows.Forms.Application.DoEvents();
				tilelist = Program.TheMainForm.executeSQL(sql);
				this.queueList[cellId].AddRange(tilelist);
			}
			else
			{
				tilelist = (List<string>)this.tilelists[listId];
				foreach ( string tiles in tilelist )
				{
					System.Windows.Forms.Application.DoEvents();
		            Regex reg = new Regex("^" + search, RegexOptions.IgnoreCase);
		            if ( reg.IsMatch(tiles) )
					{
		            	this.queueList[cellId].Add(tiles);
					}
		            // limit queue size if specified
		            if ( this.maxQueueSize > 0 && this.queueList[cellId].Count >= this.maxQueueSize )
		            {
		            	break;
		            }
				}
			}
			
			// randomise queue list if enabled
			if ( this.useRandom )
			{
				this.randomiseQueueList(cellId);
			}
			
			this.iterationSize += this.queueList[cellId].Count;
			this.log("findMatchingList(" + this.currentPathId.ToString() + ". cellId: " + cellId.ToString() + "):" + search + " = " + this.queueList[cellId].Count.ToString() + " / " + tilelist.Count.ToString());
			Program.TheMainForm.timer.stop("findMatchingList");
			this.log(Program.TheMainForm.timer.results("findMatchingList"));
		}
		
		public void queueClear(int cellId)
		{
			this.queueList[cellId] = new List<string>();
			this.queueProgress[cellId-1] = -1;
		}
		
		public void initQueues()
		{
			for ( int i = 1; i <= 64; i++ )
			{
				this.queueClear(i);
			}
		}
		
		public int loadTileQueue(int cellId)
		{
			this.queueClear(cellId);
			this.findMatchingList(cellId);
			return this.queueSize(cellId);
		}
		
		public bool queueNext(int cellId)
		{
			bool queueLoaded = false;
			this.numIterations++;
			if ( this.queueList[cellId].Count == 0 )
			{
				// load queue if not loaded (find matching tiles)
				int queueSize = this.loadTileQueue(cellId);
				if ( queueSize > 0 )
				{
					this.queueProgress[cellId-1] = 0;
					this.log("loaded queue for " + this.currentPathId.ToString() + ". cellId: " + cellId.ToString() + " = " + queueSize.ToString() + " item(s)");
					queueLoaded = true;
				}
			}
			else
			{
				if ( this.queueProgress[cellId-1] < this.queueSize(cellId) - 1 )
				{
					this.queueProgress[cellId-1]++;
					queueLoaded = true;
				}
			}
			this.updateQueueStats();
			return queueLoaded;
		}
		
		public string fetchFromQueue(int cellId)
		{
			string stile = "";
			if ( this.queueProgress[cellId-1] < this.queueSize(cellId) - 1 )
			{
				stile = this.queueList[cellId][this.queueProgress[cellId-1]];
				string[] ltiles = stile.Split(',');
				int pos = 0;
				foreach ( string tile in ltiles )
				{
					pos++;
					System.Windows.Forms.Application.DoEvents();

					// filter out used tiles
					int tileId = this.getTileId(tile);
					if ( this.usedTiles.Contains(tileId) )
					{
						return "";
					}
					
					// filter out hint tiles unless they are in the correct spot
					if ( this.hintTiles.ContainsKey(tileId) && this.hintTiles[tileId] != pos )
					{
						return "";
					}
				}
			}
			return stile;
		}
		
		public void placeTile(int cellId, string tile)
		{
			// place on board
//			this.log("placeTile(" + cellId.ToString() + ". " + tile + ") in cellId " + cellId.ToString());

			ArrayList lcolrow = this.getColRowBy2x2CellId(cellId);
			int[] colrow = (int[])lcolrow[0];
			string[] tiles = tile.Split(',');
			ArrayList atiles = new ArrayList();
			foreach ( string stile in tiles )
			{
				int tileId = this.getTileId(stile);
				TileInfo itile = new TileInfo(tileId, stile); 
				atiles.Add(itile);
				// update usedTiles
				this.usedTiles.Add(tileId);
			}
			if ( this.board.ContainsKey(cellId) )
			{
				this.board[cellId] = atiles;
			}
			else
			{
				this.board.Add(cellId, atiles);
			}
		}
		
		public void removeUsedTiles(int cellId)
		{
			if ( this.board.ContainsKey(cellId) )
			{
				ArrayList atiles = (ArrayList)this.board[cellId];
				foreach ( TileInfo tile in atiles )
				{
					this.usedTiles.Remove(tile.id);
				}
				this.board.Remove(cellId);
			}
		}
		
		public bool nextCell()
		{
			if ( this.currentPathId + 1 < this.solve_path.Count )
			{
				this.currentPathId++;
//				this.log("nextCell()::" + this.currentPathId.ToString());
				return true;
			}
			else
			{
				// end of the path reached
//				this.log("nextCell()::end");
				return false;
			}
		}
		
		public bool prevCell()
		{
//			this.log("prevCell(FROM)::" + this.currentPathId.ToString());
			// go to previous cell if available
			// returns false if no more available cells
			if ( this.currentPathId > 0 )
			{
				this.currentPathId--;
//				this.log("prevCell(OK)::" + this.currentPathId.ToString());
				
				// remove tiles from board
				int cellId = this.getCellId();
				this.removeUsedTiles(cellId);

				return true;
			}
			else
			{
//				this.log("prevCell(START)");
				return false;
			}
		}
		
		public int getCellId()
		{
			return this.solve_path[this.currentPathId];
		}
		
		// start solving
		public void solve()
		{
			Program.TheMainForm.btSolvePuzzle.Enabled = false;
			Program.TheMainForm.btPause.Enabled = true;
			Program.TheMainForm.btResume.Enabled = false;

			if ( !this.isResumed )
			{
				Program.TheMainForm.timer.start("solve");
	
				Program.TheMainForm.labelNumIterations.Text = "0";
	
				if ( !this.setSolvePath() )
				{
					return;
				}
				
				// check if tile lists loaded
				if ( !this.lists_loaded )
				{
					System.Windows.Forms.MessageBox.Show("Load a Dataset.");
					return;
				}
				
				// clear board / used tiles
				this.board = new Hashtable();
				this.usedTiles = new List<int>();
				
				// initialise queues
				this.initQueues();
				
				// initialise random seed generator if enabled
				if ( Program.TheMainForm.cbS2UseRandomSeed.Checked && Program.TheMainForm.inputS2StartSeed.Text != "" )
				{
					this.checkRandomSettings();
					if ( this.useRandom )
					{
						this.setupRandomGenerator();
					}
				}

				this.currentPathId = 0;
				this.numIterations = 0;
				this.iterationSize = 0;
				this.lastSaveIteration = 0;
				this.mostTilesPlaced = 0;
				
				this.log("*** START " + Program.TheMainForm.labelSolver2Method.Text);
			}

			this.maxQueueSize = Convert.ToInt32(Program.TheMainForm.textSolver2MaxQueueSize.Text);
			bool done = false;
			bool backtrack = false;
			int cellId = 0;
			bool solved = false;
			while ( !done )
			{
				System.Windows.Forms.Application.DoEvents();
				if ( this.isPaused )
				{
					this.updateStats();
					this.updateQueueStats();
					this.log("*** PAUSED @ " + this.numIterations.ToString());
					this.setModel();
					Program.TheMainForm.tabSolver2.Refresh();
					return;
				}
				
				// check autosave
				this.autoSave();
				
				// get cell from solve_path
				cellId = this.getCellId();
				
				// fetch next 2x2 tile from queue
				if ( this.queueNext(cellId) )
				{
					backtrack = false;
					// fetch 2x2 tile from queue
					string tile = this.fetchFromQueue(cellId);
					if ( tile != "" )
					{
						// place 2x2 tile on board
						this.placeTile(this.getCellId(), tile);
						this.log("");
						
						// go to next cell in path
						if ( !this.nextCell() )
						{
							// no more cells to fill, SOLVED!
							solved = true;
							done = true;
							break;
						}
					}
				}
				else
				{
					this.log("queue(" + cellId.ToString() + ") returned empty tile - backtracking");
					backtrack = true;
				}
				if ( backtrack )
				{
					this.log("");
					this.log("backtrack from " + this.currentPathId.ToString() + ". cellId " + cellId.ToString());

					// if queue empty / nothing found, clear list queue for current cell
					this.queueClear(cellId);
					
					// backtrack
					if ( !this.prevCell() )
					{
						// no more options - exit
						done = true;
						break;
					}
					else
					{
						cellId = this.getCellId();
						this.log("backtrack to " + this.currentPathId.ToString() + ". cellId " + cellId.ToString());
					}

				}
				 
				// update stats / score
				this.updateStats();
			}

			this.log("*** STOPPED @ " + this.numIterations.ToString());
			Program.TheMainForm.btSolvePuzzle.Enabled = true;
			Program.TheMainForm.btPause.Enabled = false;
			Program.TheMainForm.btResume.Enabled = true;

			// update stats / score
			this.updateStats();
			
			// save model
			if ( solved )
			{
				this.saveModel();
				System.Media.SystemSounds.Asterisk.Play();
			}
			
			this.log(this.numIterations.ToString() + " / " + this.iterationSize.ToString() + " iterations");
			
			Program.TheMainForm.timer.stop("solve");
			this.log(Program.TheMainForm.timer.results("solve"));
		}
		
		public void pause()
		{
			this.isPaused = true;
			this.isResumed = false;
			Program.TheMainForm.btSolvePuzzle.Enabled = true;
			Program.TheMainForm.btPause.Enabled = false;
			Program.TheMainForm.btResume.Enabled = true;
			this.updateStats();
		}
		
		public void resume()
		{
			this.isPaused = false;
			this.isResumed = true;

			Program.TheMainForm.btSolvePuzzle.Enabled = false;
			Program.TheMainForm.btPause.Enabled = true;
			Program.TheMainForm.btResume.Enabled = false;
			this.log("*** RESUMED @ " + this.numIterations.ToString());
			this.solve();
		}
		
		public string getModel()
		{
			List<string> model = new List<string>();
			for ( int cellId = 1; cellId <= 64; cellId++ )
			{
				ArrayList lcolrow = this.getColRowBy2x2CellId(cellId);
				if ( this.board.ContainsKey(cellId) )
				{
					ArrayList atiles = (ArrayList)this.board[cellId];
					int i = 0;
					foreach ( TileInfo tile in atiles )
					{
						List<string> line = new List<string>();
						int[] colrow = (int[])lcolrow[i];
						line.Add(colrow[0].ToString());
						line.Add(colrow[1].ToString());
						line.Add(tile.id.ToString());
						line.Add(getTileRotationByPattern(tile.pattern).ToString());
//						this.log(String.Join(",", line.ToArray()));
						model.Add(String.Join(",", line.ToArray()));
						i++;
					}
				}
			}
			return String.Join("\r\n", model.ToArray());
		}
		
		public void saveModel()
		{
			string filename = "models\\" + Board.title + "-solve-" + Program.TheMainForm.selSolver2Method.Text + "-" + this.numIterations + ".txt";
			string model = this.getModel();
			System.IO.File.WriteAllText(filename, model);
			this.log("saved model with " + this.usedTiles.Count.ToString() + " tile(s) to: " + filename);
		}
		
		public void setModel()
		{
			Program.TheMainForm.board.clear();
			Program.TheMainForm.board.loadTileSet(Board.title);
			Program.TheMainForm.textModel.Text = this.getModel();
			Program.TheMainForm.board.setModel();
		}
		
		public void generateStats()
		{
			List<string> stats = new List<string>();
			for ( int cellId = 1; cellId <= 64; cellId++ )
			{
				stats.Add("cell: " + cellId.ToString() + ", queue progress: " + this.queueProgress[cellId-1].ToString() + " / " + this.queueSize(cellId).ToString());
			}
			
			// show queue stats in solve_path order
			this.log(String.Join("\r\n", stats.ToArray()));
		}
		
		public void saveQueue()
		{
			if ( Program.TheMainForm.labelSolver2Method.Text == "" )
			{
				return;
			}
			
			List<string> data = new List<string>();
			List<string> line;
			
			// solver method
			data.Add(Program.TheMainForm.labelSolver2Method.Text);
			
			// save board
			for ( int cellId = 1; cellId <= 64; cellId++ )
			{
				line = new List<string>();
				if ( this.board.ContainsKey(cellId) )
			    {
					ArrayList atiles = (ArrayList)this.board[cellId];
					foreach ( TileInfo tile in atiles )
					{
						line.Add(tile.pattern);
					}
			    }
				else
				{
					line.Add(",,,");
				}
				data.Add(String.Join(",", line.ToArray()));
			}

			// currentPathId, solvePath
			line = new List<string>();
			line.Add(this.currentPathId.ToString());
			foreach ( int cellId in this.solve_path )
			{
				line.Add(cellId.ToString());
			}
			data.Add(String.Join(",", line.ToArray()));

			// cellId, queueProgress, queueSize, queueItems
			for ( int cellId = 1; cellId <= 64; cellId++ )
			{
				line = new List<string>();
				line.Add(cellId.ToString());
				line.Add(this.queueProgress[cellId-1].ToString());
				line.Add(this.queueSize(cellId).ToString());
				line.Add(String.Join(",", this.queueList[cellId].ToArray()));
				data.Add(String.Join(",", line.ToArray()));
			}
			
			// numIterations, iterationSize, mostTilesPlaced
			data.Add(this.numIterations.ToString());
			data.Add(this.iterationSize.ToString());
			data.Add(this.mostTilesPlaced.ToString());
			
			string filename = "queues\\" + Board.title + "-" + Program.TheMainForm.getTimestamp() + "-" + Program.TheMainForm.labelSolver2Method.Text + "-" + this.numIterations + "i-" + this.usedTiles.Count.ToString() + "t.txt";
			System.IO.File.WriteAllText(filename, String.Join("\r\n", data.ToArray()));
			this.log("saved queue to " + filename);
			this.loadQueueList();
		}
		
		public void loadQueue(string qid)
		{
			string filename = "queues\\" + Board.title + "-" + qid + ".txt";
			List<string> lines = new List<string>(System.IO.File.ReadAllLines(filename));
			
			// solver method
			Program.TheMainForm.selSolver2Method.Text = lines[0];
			lines.RemoveAt(0);
			
			// place tiles on solver board
			this.board = new Hashtable();
			this.usedTiles = new List<int>();
			List<string> cells = lines.GetRange(0, 64);
			lines.RemoveRange(0, 64);
			for ( int cellId = 1; cellId <= 64; cellId++ )
			{
				string tile = cells[cellId-1];
				if ( tile != ",,," )
				{
//					this.log("cellId: " + cellId.ToString() + ", " + tile);
					this.placeTile(cellId, tile);
				}
			}
			
			// solve path
			List<string> solvepath = new List<string>(lines[0].Split(','));
			this.currentPathId = Convert.ToInt16(solvepath[0]);
			solvepath.RemoveAt(0);
			this.solve_path = new List<int>();
			foreach ( string cellId in solvepath )
			{
				this.solve_path.Add(Convert.ToInt16(cellId));
			}
			lines.RemoveAt(0);
			this.log("loaded solve path with " + solvepath.Count.ToString() + " cell(s)");
			
			// queue
			foreach ( string line in lines.GetRange(0, 64) )
			{
				List<string> data = new List<string>(line.Split(','));
				int cellId = Convert.ToInt16(data[0]);
				int queueProgress = Convert.ToInt16(data[1]);
				int queueSize = Convert.ToInt16(data[2]);
				data.RemoveRange(0, 3);
				this.queueClear(cellId);
				this.queueProgress[cellId-1] = queueProgress;

				// split queue list into groups of 4 tiles
				if ( queueSize != data.Count / 4 )
				{
					this.log("error loading queue from " + filename + " - incorrect queue size for cell " + cellId.ToString());
					return;
				}
				else
				{
					for ( int i = 0; i < queueSize; i++ )
					{
						string tiles = String.Join(",", data.GetRange(0, 4).ToArray());
						this.queueList[cellId].Add(tiles);
					}
				}

			}
	        lines.RemoveRange(0, 64);
			
			// numIterations, iterationSize, mostTilesPlaced
			this.numIterations = Convert.ToInt64(lines[0]);
			this.iterationSize = Convert.ToInt64(lines[1]);
			this.mostTilesPlaced = Convert.ToInt16(lines[2]);

			this.updateStats();
			this.updateQueueStats();
			this.setModel();
			this.pause();
		
			this.log("loaded queue from " + filename);
			this.log("currentPathId " + this.currentPathId.ToString());
			this.log("cellId " + this.getCellId().ToString());
			this.loadQueueList();
		}
		
		public void loadQueueList()
		{
			string[] filenames = System.IO.Directory.GetFiles("queues");
			Array.Sort(filenames);
			Array.Reverse(filenames);
			string id;
			Program.TheMainForm.selSolver2QueueList.Items.Clear();
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
						Program.TheMainForm.selSolver2QueueList.Items.Add(id);
					}
				}
			}
			if ( Program.TheMainForm.selSolver2QueueList.Items.Count > 0 )
			{
				Program.TheMainForm.selSolver2QueueList.SelectedIndex = 0;
			}
		}
		
		public int queueSize(int cellId)
		{
			if ( this.queueList.ContainsKey(cellId) )
			{
				return this.queueList[cellId].Count;
			}
			else
			{
				return -1;
			}
		}
		
		public string getHintPattern(int cellId)
		{
			if ( this.hintPatterns.ContainsKey(cellId) )
			{
				return this.hintPatterns[cellId];
			}
			else
			{
				return "";
			}
		}
		
		public void autoSave()
		{
			// auto save every xx iterations
			if ( this.lastSaveIteration == 0 )
			{
				this.lastSaveIteration = this.numIterations;
			}
			if ( this.numIterations - this.lastSaveIteration >= this.autoSaveInterval )
			{
				this.lastSaveIteration = this.numIterations;
				this.log("*** AUTO SAVE - ITERATION INTERVAL @ " + this.numIterations.ToString());
				this.saveQueue();
			}

			// autosave queue when new score reached
			if ( this.usedTiles.Count > this.mostTilesPlaced )
			{
				this.mostTilesPlaced = this.usedTiles.Count;
				if ( this.mostTilesPlaced >= this.autoSaveMaxTilesPlaced )
				{
					this.saveQueue();
					// increase bar for next occurrence
					this.autoSaveMaxTilesPlaced = this.mostTilesPlaced + 4;
					this.log("*** AUTO SAVE - NEW SCORE - " + this.mostTilesPlaced.ToString() + ", new threshold = " + this.autoSaveMaxTilesPlaced.ToString());
				}
			}
			
		}
		
		public void logl(string text, int level)
		{
			if ( this.debug_level > 0 && this.debug_level >= level )
			{
				if ( !text.Contains("\r\n") )
				{
					text += "\r\n";
				}
				Program.TheMainForm.textSolver2Log.Text += text;
				Program.TheMainForm.textSolver2Log.Update();
			}
		}
		
		public void logt(string text, int level)
		{
			string timestamp = DateTime.Now.ToString("hh:mm:ss tt");
			this.logl(timestamp + " : " + text, level);
		}

		public void log(string text)
		{
			this.logl(text, this.debug_default_level);
		}

		public int getSeed()
		{
			int unixtime = (int)(DateTime.UtcNow - new DateTime(1970,1,1,0,0,0)).TotalSeconds;
			this.seed = unixtime;
			return this.seed;
		}
		public void checkRandomSettings()
		{
			this.useRandom = false;
			this.randomSeeds = false;
			this.start_seed = 0;
			this.seed_step = 1;
			
            if ( Program.TheMainForm.cbS2UseRandomSeed.Checked )
            {
            	this.useRandom = true;
                if ( Program.TheMainForm.inputS2StartSeed.Text != "" && Program.TheMainForm.inputS2SeedStep.Text != "" )
                {
                	this.start_seed = Convert.ToInt32(Program.TheMainForm.inputS2StartSeed.Text);
                	this.seed_step = Convert.ToInt32(Program.TheMainForm.inputS2SeedStep.Text);
                }
                else
                {
                	Program.TheMainForm.inputS2StartSeed.Text = this.start_seed.ToString();
                	Program.TheMainForm.inputS2SeedStep.Text = this.seed_step.ToString();
                }
                if ( Program.TheMainForm.cbS2RandomSeeds.Checked )
                {
                	this.randomSeeds = true;
                }
            }
		}
		
		public void setupRandomGenerator()
		{
			if ( this.run_multiple )
			{
				if ( this.randomSeeds )
				{
					this.seed = this.getSeed();
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

				this.logt("STARTING RUN #" + (this.last_run+1).ToString() + " / " + this.num_runs.ToString() + ", SEED " + this.seed.ToString() + ", ITERATION LIMIT = " + this.maxIterations.ToString(), 1);
			}
			else
			{
				if ( this.randomSeeds )
				{
					this.seed = this.getSeed();
				}
				else
				{
					this.seed = this.start_seed;
				}
			}
			this.r = new Random(this.seed);
			Program.TheMainForm.labelS2Seed.Text = this.seed.ToString();
		}
		
		public void randomiseQueueList(int cellId)
		{
			for ( int i = 0; i < this.queueList[cellId].Count; i++ )
			{
				int pos = this.r.Next(this.queueList[cellId].Count);
				string a = this.queueList[cellId][i];
				string b = this.queueList[cellId][pos];
				this.queueList[cellId][i] = b;
				this.queueList[cellId][pos] = a;
			}
			if ( this.queueList[cellId].Count > 0 )
			{
				this.logt("randomise queue using seed " + this.seed.ToString() + " for cellId: " + cellId.ToString() + " (" + this.queueList[cellId].Count.ToString() + " items)", 1);
			}
		}

	}
}

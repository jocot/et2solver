/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 9/12/2009
 * Time: 9:05 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET2Solver
{
	/// <summary>
	/// Builds cell paths for solver
	/// </summary>
	public class PathBuilder
	{
		public List<int> path = new List<int>();
		private List<int> excludedCells = new List<int>();
		public string pathString = "";
		
		// checkpoints to trigger enforce fillable check
		public Dictionary<int, int[]> checkpoints = new Dictionary<int, int[]>();
		
		public string selected_filter = "";
		
		// regions for region specific restrictions (no duplicate tiles etc)
		public Dictionary<int, Dictionary<int, List<int>>> regions = new Dictionary<int, Dictionary<int, List<int>>>();
		// map of which region applies to a cellId (auto generated) - cellId, regionId
		public Dictionary<string, int> cellRegions = new Dictionary<string, int>();

		public PathBuilder()
		{
		}
		
		public void clear()
		{
			this.path = new List<int>();
			this.excludedCells = new List<int>();
			this.pathString = "";
		}
		
		public void defineMethods()
		{
			// paths
			Program.TheMainForm.selS1SolveMethod.Items.Clear();
			Program.TheMainForm.selS1SolveMethod.Items.Add("Custom");
			Program.TheMainForm.selS1SolveMethod.Items.Add("rows_right_down");
			Program.TheMainForm.selS1SolveMethod.Items.Add("rows_right_up");
			Program.TheMainForm.selS1SolveMethod.Items.Add("rows_left_down");
			Program.TheMainForm.selS1SolveMethod.Items.Add("rows_left_up");
			Program.TheMainForm.selS1SolveMethod.Items.Add("border_clockwise");
			Program.TheMainForm.selS1SolveMethod.Items.Add("border_bl_br_tr_tl");
			Program.TheMainForm.selS1SolveMethod.Items.Add("border_tl_tr_br_bl");
			Program.TheMainForm.selS1SolveMethod.Items.Add("border_x");
			Program.TheMainForm.selS1SolveMethod.Items.Add("cols_down_right");
			Program.TheMainForm.selS1SolveMethod.Items.Add("cols_up_right");
			Program.TheMainForm.selS1SolveMethod.Items.Add("cols_down_left");
			Program.TheMainForm.selS1SolveMethod.Items.Add("cols_up_left");
			Program.TheMainForm.selS1SolveMethod.Items.Add("inner");
			Program.TheMainForm.selS1SolveMethod.Items.Add("inner_144");
			Program.TheMainForm.selS1SolveMethod.Items.Add("inner_52");
			Program.TheMainForm.selS1SolveMethod.Items.Add("spiral_in_lbrt");
			Program.TheMainForm.selS1SolveMethod.Items.Add("spiral_in_trbl");
			Program.TheMainForm.selS1SolveMethod.Items.Add("spiral_out_bl_br_tr_tl");
			Program.TheMainForm.selS1SolveMethod.Items.Add("10x10_border_rows");

			// path filters
			Program.TheMainForm.selS1PathFilter.Items.Clear();
			Program.TheMainForm.selS1PathFilter.Items.Add("none");
			Program.TheMainForm.selS1PathFilter.Items.Add("4x5");
			Program.TheMainForm.selS1PathFilter.Items.Add("checkers_x");
			Program.TheMainForm.selS1PathFilter.Items.Add("center_untouched");
			Program.TheMainForm.selS1PathFilter.Items.Add("center_cross_4_2x2");
			Program.TheMainForm.selS1PathFilter.Items.Add("center_square_4_2x2");
			Program.TheMainForm.selS1PathFilter.Items.Add("exclude_center_8x6");
			Program.TheMainForm.selS1PathFilter.Items.Add("all_hints_untouched");

		}
		
		public int getCellPos(int col, int row)
		{
			int pos = (row - 1) * Board.num_cols + col;
			return pos;
		}
		
		public List<int> buildPath(string method)
		{
			this.path = new List<int>();
			int inner = 0;
			int outer = 0;
			int length = 0;
			switch ( method )
			{
				case "border_clockwise":
					this.addRowsToPath("right", 1);
					this.addColsToPath("down", Board.num_cols);
					this.addRowsToPath("left", Board.num_rows);
					this.addColsToPath("up", 1);
					break;
				case "border_bl_br_tr_tl":
					// 60 around the border, starting from bottom left
					// add corners first
					this.addCellsToPath(241, 256);
					this.addRowsToPath("right", Board.num_rows);
					this.addCellsToPath(16);
					this.addColsToPath("up", Board.num_cols);
					this.addCellsToPath(1);
					this.addRowsToPath("left", 1);
					this.addColsToPath("down", 1);
					break;
				case "border_tl_tr_br_bl":
					// 28 2x2 cells around the border, starting from top left
					// add corners first
					this.addCellsToPath(1, 16);
					this.addRowsToPath("right", 1);
					this.addCellsToPath(256);
					this.addColsToPath("down", Board.num_cols);
					this.addCellsToPath(241);
					this.addRowsToPath("left", Board.num_rows);
					this.addColsToPath("up", 1);
					break;
				case "border_x":
					// border from X formation inwards, starting with corners, edges, inner corners etc
					this.addCellsToPath(225,226,227,228,193,194,195,196,229,230,231,232,253,254,255,256,249,250,251,252,221,222,223,224,29,30,31,32,61,62,63,64,25,26,27,28,1,2,3,4,5,6,7,8,33,34,35,36,161,162,163,164,233,234,235,236,245,246,247,248,189,190,191,192,93,94,95,96,21,22,23,24,9,10,11,12,65,66,67,68,237,238,239,240,241,242,243,244,157,158,159,160,125,126,127,128,17,18,19,20,13,14,15,16,97,98,99,100,129,130,131,132);
					break;
				case "rows_right_up":
					// by row, from bottom to top (left-right)
					this.addRowsToPath("right", this.cellRange(Board.num_rows, 1).ToArray());
					break;
				case "rows_right_down":
					// by row, from top down (left-right)
					this.addRowsToPath("right", this.cellRange(1, Board.num_rows).ToArray());
					break;
				case "rows_left_up":
					// by row, from bottom to top (right-left)
					this.addRowsToPath("left", this.cellRange(Board.num_rows, 1).ToArray());
					break;
				case "rows_left_down":
					// by row, from top down (right-left)
					this.addRowsToPath("left", this.cellRange(1, Board.num_rows).ToArray());
					break;
				case "cols_up_right":
					// by column, from left to right (bottom-top)
					this.addColsToPath("up", this.cellRange(1, Board.num_cols).ToArray());
					break;
				case "cols_down_right":
					// by column, from left to right top down
					this.addColsToPath("down", this.cellRange(1, Board.num_cols).ToArray());
					break;
				case "cols_up_left":
					// by column, from right to left (bottom-top)
					this.addColsToPath("up", this.cellRange(Board.num_cols, 1).ToArray());
					break;
				case "cols_down_left":
					// by column, from right to left top down
					this.addColsToPath("down", this.cellRange(Board.num_cols, 1).ToArray());
					break;
				case "spiral_in_lbrt":
					// spiral from outside border inwards, from bottom left, going right/up, then left/down
					outer = Board.num_rows;
					for ( inner = 1; inner <= Board.num_rows; inner++ )
					{
						this.addColsToPath("down", inner);
						this.addRowsToPath("right", outer);
						this.addColsToPath("up", outer);
						this.addRowsToPath("left", inner);
//						Program.TheMainForm.log(outer.ToString() + "-" + inner.ToString());
						outer--;
					}
					break;
				case "spiral_in_trbl":
					// spiral from outside border inwards
					outer = Board.num_rows;
					for ( inner = 1; inner <= Board.num_rows; inner++ )
					{
						this.addRowsToPath("right", inner);
						this.addColsToPath("down", outer);
						this.addRowsToPath("left", outer);
						this.addColsToPath("up", inner);
//						Program.TheMainForm.log(outer.ToString() + "-" + inner.ToString());
						outer--;
					}
					break;
				case "spiral_out_lbrt":
					// spiral outwards from center, from bottom left, going right/up, then left/down
					// row: 8,10 to 10,10
					// col: 10,10 to 10,8
					// row: 10,8 to 8,8
					// col: 8,8 to 8,10
					
					// row: 6,12 to 12,12
					// col: 12,12 to 12,6
					// row: 12,6 to 6,6
					// col: 6,6 to 6,12

					inner = Board.max_tiles / 2;
					outer = Board.max_tiles / 2 + 1;
					length = Board.max_tiles / 4;
					for ( int i = 1; i <= Board.max_tiles / 2; i++ )
					{
//						this.addRowRange("right", inner, outer, length);
						this.addRowRange("right", Board.max_tiles / 2, outer, length - (Board.max_tiles / 2 - inner));
						this.addColRange("up", outer, outer, length);
						this.addRowRange("left", outer, inner, length);
						this.addColRange("down", inner, inner, length);
						// always start from col 8 (below hint tile
						// reason: so that it's adjacent to a previous tile when forming next ring
						if ( inner < Board.max_tiles / 2 )
						{
							this.addRowRange("right", inner, outer, Board.max_tiles / 2 - inner);
						}
						Program.TheMainForm.log(outer.ToString() + "-" + inner.ToString());
						outer++;
						inner--;
						length += Board.max_tiles / 4;
//						System.Diagnostics.Debug.WriteLine("debug!");
					}
					break;
				case "inner":
					// inner pieces, excluding border
					this.addRowsToPath("right", this.cellRange(1, Board.num_rows).ToArray());
					this.removeRowsFromPath(1, Board.num_rows);
					this.removeColsFromPath(1, Board.num_cols);
					break;
				case "inner_144":
					this.addCellsToPath(36,37,38,39,40,41,42,43,44,45,51,52,53,54,55,56,57,58,59,60,61,62,67,68,69,70,71,72,73,74,75,76,77,78,83,84,85,86,87,88,89,90,91,92,93,94,99,100,101,102,103,104,105,106,107,108,109,110,115,116,117,118,119,120,121,122,123,124,125,126,131,132,133,134,135,137,138,139,140,141,142,147,148,149,150,151,152,153,154,155,156,157,158,163,164,165,166,167,168,169,170,171,172,173,174,179,180,181,182,183,184,185,186,187,188,189,190,195,196,197,198,199,200,201,202,203,204,205,206,212,213,214,215,216,217,218,219,220,221);
					break;
				case "inner_52":
					this.addCellsToPath(19,20,21,22,23,24,25,26,27,28,29,30,31,47,63,79,95,111,127,143,159,175,191,207,223,239,238,237,236,235,234,233,232,231,230,229,228,227,226,210,194,178,162,146,130,114,98,82,66,50,34,18);
					break;
				case "10x10_border_rows":
					this.addCellsToPath(1,2,3,4,5,6,7,8,9,10,20,30,40,50,60,70,80,90,100,99,98,97,96,95,94,93,92,91,81,71,61,51,41,31,21,11,12,13,14,15,16,17,18,19,22,23,24,25,26,27,28,29,32,33,34,35,36,37,38,39,42,43,44,45,46,47,48,49,52,53,54,55,56,57,58,59,62,63,64,65,66,67,68,69,72,73,74,75,76,77,78,79,82,83,84,85,86,87,88,89);
					break;
			}
			this.pathString = this.getPathAsString();
			return this.path;
		}
		
		// assumes rows_right_down method to iterate through region
		public void setRegionAsPath(CellRegion region)
		{
			this.path.Clear();
			for ( int row = region.topLeftCellRow; row <= region.bottomRightCellRow; row++ )
			{
				for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol; col++ )
				{
					this.addCellsToPath(this.getCellIdFromColRow(col, row));
				}
			}
		}
		
		public void pathFilter(string id)
		{
			int[] cells;
			switch ( id )
			{
				case "checkers_x":
					int cellId = 0;
					int offset = 0;
					cells = new int[0];
					for ( int row = Board.num_rows; row >= 1; row-- )
					{
						if ( row != 1 && row != Board.max_tiles )
						{
							cellId = this.getCellPos(1, row);
							cells = new int[]{cellId + offset};
							this.excludeCells(cells);
							cellId = this.getCellPos(Board.num_cols, row);
							cells = new int[]{cellId - offset};
							this.excludeCells(cells);
						}
						offset++;
					}
					break;
				case "4x5":
					cells = new int[]{19,21,28,30,52,61,83,85,92,94,147,149,156,158,180,189,211,213,220,222};
					this.excludeCells(cells);
					break;
				case "center_untouched":
					cells = new int[]{120,135,137,152};
					this.excludeCells(cells);
					break;
				case "exclude_center_8x6":
					cells = new int[]{85,86,87,88,89,90,91,92,101,102,103,104,105,106,107,108,117,118,119,120,121,122,123,124,133,134,135,136,137,138,139,140,149,150,151,152,153,154,155,156,165,166,167,168,169,170,171,172};
					this.excludeCells(cells);
					break;
				case "center_cross_4_2x2":
					cells = new int[]{88,89,104,105,122,123,138,139,152,153,168,169,118,119,134,135};
					this.excludeCells(cells);
					break;
				case "center_square_4_2x2":
					cells = new int[]{86,87,102,103,90,91,106,107,154,155,170,171,150,151,166,167};
					this.excludeCells(cells);
					break;
				case "all_hints_untouched":
					cells = new int[]{120,135,137,152,19,34,36,51,30,45,47,62,195,210,212,227,206,221,223,238};
					this.excludeCells(cells);
					
					// setup checkpoints if enforce fillable is enabled
					// trigger cell, cell to check
					// for left-right, top-down fill method
					if ( Program.TheMainForm.cbS1EnforceFillableCells.Checked )
					{
						// checkpoints for cells surrounding 5 hint pieces
						this.checkpoints = new Dictionary<int, int[]>();
						this.checkpoints.Add(18, new int[]{19});
						this.checkpoints.Add(20, new int[]{19});
						this.checkpoints.Add(29, new int[]{30});
						this.checkpoints.Add(31, new int[]{30});
						this.checkpoints.Add(33, new int[]{34});
						this.checkpoints.Add(37, new int[]{36});
						this.checkpoints.Add(44, new int[]{45});
						this.checkpoints.Add(48, new int[]{47});
						this.checkpoints.Add(50, new int[]{34,51});
						this.checkpoints.Add(52, new int[]{36,51});
						this.checkpoints.Add(61, new int[]{45,62});
						this.checkpoints.Add(63, new int[]{47,62});
						this.checkpoints.Add(67, new int[]{51});
						this.checkpoints.Add(78, new int[]{62});
						this.checkpoints.Add(119, new int[]{120});
						this.checkpoints.Add(121, new int[]{120});
						this.checkpoints.Add(134, new int[]{135});
						this.checkpoints.Add(138, new int[]{137});
						this.checkpoints.Add(151, new int[]{135,152});
						this.checkpoints.Add(153, new int[]{137,152});
						this.checkpoints.Add(168, new int[]{152});
						this.checkpoints.Add(194, new int[]{195});
						this.checkpoints.Add(196, new int[]{195});
						this.checkpoints.Add(205, new int[]{206});
						this.checkpoints.Add(207, new int[]{206});
						this.checkpoints.Add(209, new int[]{210});
						this.checkpoints.Add(213, new int[]{212});
						this.checkpoints.Add(220, new int[]{221});
						this.checkpoints.Add(224, new int[]{223});
						this.checkpoints.Add(226, new int[]{210,227});
						this.checkpoints.Add(228, new int[]{227,212});
						this.checkpoints.Add(237, new int[]{221,238});
						this.checkpoints.Add(239, new int[]{223,238});
						this.checkpoints.Add(243, new int[]{227});
						this.checkpoints.Add(254, new int[]{238});
					}
					break;
			}
		}
		
		private string getPathAsString()
		{
			return String.Join(",", this.path.ConvertAll<string>(delegate(int i) { return i.ToString(); }).ToArray());
		}
		
		public string getExcludedCellsAsString()
		{
			return String.Join(",", this.excludedCells.ConvertAll<string>(delegate(int i) { return i.ToString(); }).ToArray());
		}
		
		public void addCellsToPath(params int[] cells)
		{
			// skips adding duplicates (to allow spiral path to be added easily by row/col
//			Program.TheMainForm.log("excluded length: " + this.excludedCells.Count + ", path length: " + this.path.Count);
			foreach ( int cellId in cells )
			{
				if ( cellId >= 1 && cellId <= Board.max_tiles && !this.path.Contains(cellId) && !this.excludedCells.Contains(cellId) )
				{
					this.path.Add(cellId);
				}
			}
//			Program.TheMainForm.log("excluded length: " + this.excludedCells.Count + ", path length: " + this.path.Count);
		}
		
		public void addColsToPath(string direction, params int[] cols)
		{
			switch ( direction )
			{
				case "up":
					foreach ( int col in cols )
					{
						for ( int row = Board.num_rows; row >= 1; row-- )
						{
							int cellId = this.getCellPos(col, row);
							this.addCellsToPath(cellId);
						}
					}
					break;
				case "down":
					foreach ( int col in cols )
					{
						for ( int row = 1; row <= Board.num_rows; row++ )
						{
							int cellId = this.getCellPos(col, row);
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
						for ( int col = 1; col <= Board.num_cols; col++ )
						{
							int cellId = this.getCellPos(col, row);
							this.addCellsToPath(cellId);
						}
					}
					break;
				case "left":
					foreach ( int row in rows )
					{
						for ( int col = Board.num_cols; col >= 1; col-- )
						{
							int cellId = this.getCellPos(col, row);
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
						int cellId = this.getCellPos(c, row);
						this.addCellsToPath(cellId);
					}
					break;
				case "left":
					for ( int c = col; c > col - length; c-- )
					{
						int cellId = this.getCellPos(c, row);
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
						int cellId = this.getCellPos(col, r);
						this.addCellsToPath(cellId);
					}
					break;
				case "down":
					for ( int r = row; r < row + length; r++ )
					{
						int cellId = this.getCellPos(col, r);
						this.addCellsToPath(cellId);
					}
					break;
			}
		}
		
		public void removeColsFromPath(params int[] cols)
		{
			List<int> cells = new List<int>();
			foreach ( int col in cols )
			{
				for ( int row = 1; row <= Board.num_rows; row++ )
				{
					cells.Add(this.getCellPos(col, row));
				}
			}
			this.excludeCells(cells);
		}

		public void removeRowsFromPath(params int[] rows)
		{
			List<int> cells = new List<int>();
			foreach ( int row in rows )
			{
				for ( int col = 1; col <= Board.num_cols; col++ )
				{
					cells.Add(this.getCellPos(col, row));
				}
			}
			this.excludeCells(cells);
		}

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
		
		public void excludeCells(ICollection<int> cells)
		{
			this.excludedCells.AddRange(cells);
			// remove from path if exists
			foreach ( int cell in cells )
			{
				if ( this.path.Contains(cell) )
				{
					this.path.Remove(cell);
				}
			}
		}
		
		// load regions from regions.txt
		// define restrictive regions that limit duplicate tiles from being placed (ie. sudoku style)
		// use cellId and quarter position to define regions
		public void loadRegions()
		{
			// regions contains: regionId, regionData
			this.regions = new Dictionary<int, Dictionary<int, List<int>>>();
			this.cellRegions = new Dictionary<string, int>();

//			string filename = "regions\\regions.txt";
			string filename = "regions\\regions-border5x3.txt";
			if ( !System.IO.File.Exists(filename) )
			{
				System.Windows.Forms.MessageBox.Show("Could not load " + filename);
				return;
			}
			string[] lines = System.IO.File.ReadAllLines(filename);

			int lineNum = 0;
			foreach ( string line in lines )
			{
				lineNum++;
				if ( !Regex.Match(line, "\\s*#").Success && line.Trim() != "" )
				{
					string[] parts = line.Split('\t');
					// regionData contains: cellId, quarters
					int regionId = Convert.ToInt16(parts[0]);
					int cellId = Convert.ToInt16(parts[1]);
					List<int> quarters = new List<string>(parts[2].Split(',')).ConvertAll<int>(delegate(string s){ return Convert.ToInt16(s); });

					if ( this.regions.ContainsKey(regionId) )
					{
						if ( this.regions[regionId].ContainsKey(cellId) )
						{
							// overwrite region data if cellId exists more than once for the same region
							this.regions[regionId][cellId] = quarters;
							Program.TheMainForm.solver.logt("warning: region " + regionId.ToString() + " contains duplicate cellId " + cellId.ToString() + " on line " + lineNum.ToString() + " in " + filename, 1);
						}
						else
						{
							this.regions[regionId].Add(cellId, quarters);
							// map cellId to region
							this.mapCellToRegion(regionId, cellId, quarters);
						}
					}
					else
					{
						Dictionary<int, List<int>> regionData = new Dictionary<int, List<int>>();
						regionData.Add(cellId, quarters);
						regions.Add(regionId, regionData);
						// map cellId to region
						this.mapCellToRegion(regionId, cellId, quarters);
					}
				}
			}
			Program.TheMainForm.solver.logt("loaded " + regions.Count.ToString() + " regions from " + filename, 1);
		}
		
		public void mapCellToRegion(int regionId, int cellId, List<int> quarters)
		{
			foreach ( int quarter in quarters )
			{
				string id = cellId.ToString() + "." + quarter.ToString();
				if ( this.cellRegions.ContainsKey(id) )
				{
					this.cellRegions[id] = regionId;
				}
				else
				{
					this.cellRegions.Add(id, regionId);
				}
			}
		}
		
		public int getRegionId(int cellId, int quarter)
		{
			string id = cellId.ToString() + "." + quarter.ToString();
			int regionId = -1;
			if ( this.cellRegions.ContainsKey(id) )
			{
				regionId = this.cellRegions[id];
			}
			return regionId;
		}
		
		public int getCellIdFromColRow(int col, int row)
		{
			if ( col >= 1 && col <= Board.num_cols && row >= 1 && row <= Board.num_rows )
			{
				return (row - 1) * Board.num_cols + col;
			}
			else
			{
				// invalid col/row
				return 0;
			}
		}
		
	}
}

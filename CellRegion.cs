/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 20/10/2010
 * Time: 1:22 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace ET2Solver
{
	/// <summary>
	/// regionId, topLeftCell, bottomRightCell, numEmptyCells, score
	/// </summary>
	public class CellRegion
	{
		public int regionId = 0;
		public int topLeftCellCol = 0;
		public int topLeftCellRow = 0;
		public int bottomRightCellCol = 0;
		public int bottomRightCellRow = 0;
		public int numEmptyCells = 0;
		public int score = 0;
		public int maxScore = 0;
		
		// used to calculate perimeter score
		// cellId, score (corners = 2 if they have 2 adjacent tiles, edges = 1)
		public SortedDictionary<int, int> perimeterTileCells = new SortedDictionary<int, int>();
		
		public CellRegion()
		{
		}

		public CellRegion(int topLeftCellCol, int topLeftCellRow, int bottomRightCellCol, int bottomRightCellRow)
		{
			this.topLeftCellCol = topLeftCellCol;
			this.topLeftCellRow = topLeftCellRow;
			this.bottomRightCellCol = bottomRightCellCol;
			this.bottomRightCellRow = bottomRightCellRow;
		}
		
		public CellRegion(int regionId, int topLeftCellCol, int topLeftCellRow, int bottomRightCellCol, int bottomRightCellRow)
		{
			this.regionId = regionId;
			this.topLeftCellCol = topLeftCellCol;
			this.topLeftCellRow = topLeftCellRow;
			this.bottomRightCellCol = bottomRightCellCol;
			this.bottomRightCellRow = bottomRightCellRow;
		}
		
		public bool isDefined()
		{
			if ( this.topLeftCellCol > 0 && this.topLeftCellRow > 0 && this.bottomRightCellCol > 0 && this.bottomRightCellRow > 0 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public string[] getAsText()
		{
			List<string> text = new List<string>();
			text.Add(String.Format("Region ID: {0}", this.regionId));
			text.Add(String.Format("Co-Ords: {0},{1} to {2},{3}", this.topLeftCellCol, this.topLeftCellRow, this.bottomRightCellCol, this.bottomRightCellRow));
			text.Add(String.Format("Score: {0} / {1}", this.score, this.maxScore));
			text.Add(String.Format("Number of empty cells: {0}", this.numEmptyCells));
			return text.ToArray();
		}
		
		// number of cells within this region
		public int numCells()
		{
			int width = this.bottomRightCellCol - this.topLeftCellCol + 1;
			int height = this.bottomRightCellRow - this.topLeftCellRow + 1;
			return width * height;
		}
		
		// number of tiles contained in this region
		public int numTiles()
		{
			return this.numCells() - this.numEmptyCells;
		}
		
		// return true if col/row intersects with this region
		public bool intersects(int col, int row)
		{
			// check if intersects with column
			bool colIntersect = col >= this.topLeftCellCol && col <= this.bottomRightCellCol;
			// check if intersects with row
			bool rowIntersect = row >= this.topLeftCellRow && row <= this.bottomRightCellRow;
			return colIntersect && rowIntersect;
		}
		
	}
}

/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 20/10/2010
 * Time: 1:14 AM
 * 
 * Store full and partial solutions.
 * 
 */
using System;
using System.Collections.Generic;

namespace ET2Solver
{
	public class Solution
	{
		public int id = 0;
		
		// score of this solution/opportunity
		public int score = 0;
		
		// score efficiency (score / numTiles)
		public double scoreEfficiency = 0;

		// net score after used tiles have been accounted for
		public int netscore = 0;
		
		// score of used tiles required to fulfill this opportunity (deduct from score to calculate net score gain/loss)
		public int usedScore = 0;
		public int gainLoss = 0;
		
		// number of unique used tiles
		//public int numUsedTiles = 0;
		
		// list of used tiles from existing board that are contained within this opportunity/solution
		public List<string> usedTiles = new List<string>();
		
		// cellId, tile pattern
		public SortedDictionary<int, string> board = new SortedDictionary<int, string>();
		
		public CellRegion region;

		public Solution()
		{
		}

		public Solution(int score, SortedDictionary<int, string> board)
		{
			this.score = score;
			this.board = board;
		}
		
		public string[] getAsText()
		{
			List<string> text = new List<string>();
			text.Add(String.Format("Opportunity ID: {0}", this.id));
			text.Add(String.Format("Score Gain/Loss: {0}", this.gainLoss));
			text.Add(String.Format("Score: {0}", this.score));
			text.Add(String.Format("Net Score: {0}", this.netscore));
			text.Add(String.Format("Score from used tiles: {0}", this.usedScore));
			text.Add(String.Format("Used Tiles: {0}", String.Join(",", this.usedTiles.ToArray())));
			text.Add("Board Dump");
			foreach ( int cellId in this.board.Keys )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				text.Add(String.Format("{0},{1},{2}", colrow[0], colrow[1], this.board[cellId]));
			}
			// region info, params & dump
			text.AddRange(this.region.getAsText());
			return text.ToArray();
		}
	}
}

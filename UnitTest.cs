/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 27/10/2010
 * Time: 2:04 AM
 * 
 * A class to test important functions of ET2Solver
 * 
 */
using System;
using System.Collections.Generic;
using CAF;

namespace ET2Solver
{
	public class UnitTest
	{
		public UnitTest()
		{
		}
		
		public void run(string test)
		{
			switch ( test )
			{
				case "TileSwap":
					// TileSwap module
					// important functions to test
					
					// appears solutions/opportunities contain mismatched tiles
					// check Solver 3 ?
					// String objects (ie, tile pattern) copies are copied via value (therefore no reference issues)
					
					// tiles in opportunity list don't match the tiles that are actually placed on the board??
					
					// opportunity scoring analyzer is incorrect
					// mingain = -10
					// region_start = 6
					// region_end = 6
					// opportunityId = 90 - reported gain = -4, actual gain = 0, netscore = 5, actual score = 9
					
					/*
					private void defineSquareRegions()
					private void init()
					public HashSet<string> getTileMatchingVertices(int cellId)
					public int calcRegionMaxScore(CellRegion region)
					public int calcRegionNumEmptyCells(CellRegion region)
					public int calcRegionScore(CellRegion region)
					public int calcTileScore(int cellId)
					public int[] getValidRegion(int cellCol, int cellRow, int regionWidth, int regionHeight, int refCol, int refRow)
					public Solution getBestOpportunity()
					public SortedDictionary<int, int> getPerimeterTileCells(CellRegion region)
					public SortedDictionary<int, string> copyBoard(SortedDictionary<int, string> sourceBoard)
					public SortedDictionary<int, TileInfo> copyBoard(SortedDictionary<int, TileInfo> sourceBoard)
					public string getBoardDump()
					public string[] getRegionFilter(CellRegion region)
					public string[] getTilesWithAdjacentToEmpty(CellRegion region)
					public string[] getUsedTiles(CellRegion region)
					public TileSwap()
					public TileSwap(SortedDictionary<int, string> currentBoard, SortedDictionary<int, string> tileset)
					public TileSwap(SortedDictionary<int, TileInfo> currentBoard, SortedDictionary<int, string> tileset)
					public void analyseOpportunities(CellRegion region, List<Solution> opportunities)
					public void calcScore()
					public void clearRegion(CellRegion region)
					public void defineAllRegions()
					public void defineRegions()
					public void dictIncValue(SortedDictionary<int, int> dict, int key)
					public void findOpportunities()
					public void getOpportunities(CellRegion region)
					public void identifyUsedTiles()
					public void locateEmptyCells(CellRegion region)
					public void setProgressCallback(TileSwapEvent func)
					*/
					// test copyboard copies all contents
					// modify copy and check if source is unchanged
					break;
			}
		}
		
		public void runAll()
		{
			List<string> tests = new List<string>();
			tests.Add("TileSwap");
			foreach ( string test in tests )
			{
				this.run(test);
			}
		}
		
	}
}

/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 24/10/2010
 * Time: 1:16 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using CAF;

namespace ET2Solver
{
	/// <summary>
	/// Description of TileSwapUI.
	/// </summary>
	public partial class TileSwapUI : Form
	{
		public TileSwap ts = new TileSwap();
		
		public int progressCount = 0;
		public Int64 solverProgressCount = 0;
		
		// backup of board tileset
		private Tile[] tileset = new Tile[0];
		// backup of board model
		private int[] tilepos = new int[0];
		
		public TileSwapUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.init();
		}
		
		void BtCloseClick(object sender, EventArgs e)
		{
			this.ts.progressCallback = null;
			this.ts = null;
			this.Close();
		}
		
		void BtStartClick(object sender, EventArgs e)
		{
			this.findSwaps();
		}
		
		public void init()
		{
			if ( CAF_Application.config.contains("tileswap_region_start") )
			{
				this.inputStartRegion.Text = CAF_Application.config.getValue("tileswap_region_start");
			}
			if ( CAF_Application.config.contains("tileswap_region_end") )
			{
				this.inputEndRegion.Text = CAF_Application.config.getValue("tileswap_region_end");
			}
			
			// setup log window output
			CAF_Application.log.setTextControl(Program.TheMainForm.logwindow, true, LogType.INFO);
			
			// setup datagrid cell click callback
			this.dataGridView1.CellMouseDoubleClick += this.cellClick;
		}
		
		public void findSwaps()
		{
			CAF_Application.config.deleteValue("runtime_is_stopped");
			this.tileswap_status.Text = "Finding opportunities...please wait...";
			this.tileswap_status.Update();
			
			if ( this.cbRegions.Checked )
			{
				if ( this.inputStartRegion.Text != "" )
				{
					CAF_Application.config.setValue("tileswap_region_start", this.inputStartRegion.Text);
				}
				else
				{
					CAF_Application.config.setValue("tileswap_region_start", "1");
				}
				if ( this.inputEndRegion.Text != "" )
				{
					CAF_Application.config.setValue("tileswap_region_end", this.inputEndRegion.Text);
				}
				else
				{
					CAF_Application.config.deleteValue("tileswap_region_end");
				}
			}
			else
			{
				CAF_Application.config.deleteValue("tileswap_region_start");
				CAF_Application.config.deleteValue("tileswap_region_end");
			}

			this.dataSet1.Clear();
			this.dataSet1.Tables.Clear();
			this.dataSet1.Tables.Add("opportunities");
			this.dataSet1.Tables["opportunities"].Columns.Add("OppID");
			this.dataSet1.Tables["opportunities"].Columns.Add("RegionID");
			this.dataSet1.Tables["opportunities"].Columns.Add("CellTL");
			this.dataSet1.Tables["opportunities"].Columns.Add("CellBR");
			this.dataSet1.Tables["opportunities"].Columns.Add("RegionScore");
			this.dataSet1.Tables["opportunities"].Columns.Add("UsedScore");
			this.dataSet1.Tables["opportunities"].Columns.Add("OppNetScore");
			this.dataSet1.Tables["opportunities"].Columns.Add("RegionTiles");
			this.dataSet1.Tables["opportunities"].Columns.Add("OppTiles");
			this.dataSet1.Tables["opportunities"].Columns.Add("Gain");
			this.dataSet1.Tables["opportunities"].Columns.Add("ImpEff");
			this.dataSet1.Tables["opportunities"].Columns.Add("Board");
			this.dataGridView1.DataSource = this.dataSet1.Tables["opportunities"];
			this.dataGridView1.Update();

			SortedDictionary<int, TileInfo> board = Program.TheMainForm.board.getBoardData();
			// backup tileset
			this.tileset = Program.TheMainForm.board.copyTileSet(Program.TheMainForm.board.tileset);
			// backup model
			this.tilepos = new List<int>(Program.TheMainForm.board.tilepos).ToArray();
			
			this.ts = new TileSwap(board, Program.TheMainForm.board.getTileSetV3());
			this.ts.setProgressCallback(this.updateProgress);
			this.ts.solver.setProgressCallback(this.updateSolverProgress);

			//CellRegion region = this.getBoardRegion();
//			CellRegion region = new CellRegion(1, 1, Board.num_cols, Board.num_rows);
//			CellRegion region = new CellRegion(8, 1, 16, 9);

			// define tileswap board perimeter
			int parent_region_x1 = 1;
			int parent_region_y1 = 1;
			int parent_region_x2 = Board.num_cols;
			int parent_region_y2 = Board.num_rows;
			if ( CAF_Application.config.contains("tileswap_parent_region_x1") )
			{
				parent_region_x1 = Convert.ToInt16(CAF_Application.config.getValue("tileswap_parent_region_x1"));
			}
			if ( CAF_Application.config.contains("tileswap_parent_region_y1") )
			{
				parent_region_y1 = Convert.ToInt16(CAF_Application.config.getValue("tileswap_parent_region_y1"));
			}
			if ( CAF_Application.config.contains("tileswap_parent_region_x2") )
			{
				parent_region_x2 = Convert.ToInt16(CAF_Application.config.getValue("tileswap_parent_region_x2"));
			}
			if ( CAF_Application.config.contains("tileswap_parent_region_y2") )
			{
				parent_region_y2 = Convert.ToInt16(CAF_Application.config.getValue("tileswap_parent_region_y2"));
			}
			CellRegion region = new CellRegion(parent_region_x1, parent_region_y1, parent_region_x2, parent_region_y2);
			
			this.ts.getOpportunities(region);
			//Solution bestop = ts.getBestOpportunity();

			// reset stats
			this.ts.solver.resetStats();

			foreach ( Solution opportunity in this.ts.opportunities )
			{
				//this.dataSet1.Tables["opportunities"].Rows.Add(new string[]{"a","b","c"});
				List<string> row = new List<string>();
				row.Add(opportunity.id.ToString());
				row.Add(opportunity.region.regionId.ToString());
				row.Add(opportunity.region.topLeftCellCol + "," + opportunity.region.topLeftCellRow);
				row.Add(opportunity.region.bottomRightCellCol + "," + opportunity.region.bottomRightCellRow);
				row.Add(opportunity.region.score.ToString());
				row.Add(opportunity.usedScore.ToString());
				row.Add(opportunity.netscore.ToString());
				row.Add(opportunity.region.numTiles().ToString());
				row.Add(opportunity.board.Count.ToString());
				row.Add(opportunity.gainLoss.ToString());
				// check if efficiency improved (same/higher score with less tiles)
				if ( opportunity.scoreEfficiency > this.ts.scoreEfficiency )
				{
					row.Add("+");
				}
				else if ( opportunity.scoreEfficiency == this.ts.scoreEfficiency )
				{
					row.Add("=");
				}
				else if ( opportunity.scoreEfficiency < this.ts.scoreEfficiency )
				{
					row.Add("-");
				}
				row.Add(String.Join(",", new List<string>(opportunity.board.Values).ToArray()));
				this.dataSet1.Tables["opportunities"].Rows.Add(row.ToArray());
			}
			this.dataGridView1.Sort(this.dataGridView1.Columns["Gain"], System.ComponentModel.ListSortDirection.Descending);
			this.dataGridView1.Update();
			
			this.tileswap_status.Text = String.Format("Found {0} regions, {1} opportunities, {2} eligible opportunities, best gain: {3}, improved efficiency: {4}", this.ts.totalRegions, this.ts.totalOpportunities, this.ts.potentialOpportunities, this.ts.bestOppGain, this.ts.improvedEfficiency);
			
			// log opportunity / num solutions ratio
			// this.ts.potentialOpportunities / CAF_Application.stats.get("numSolutions")
			
			this.tileswap_status.Refresh();
			
			this.tileswap_solver_status.Text = this.ts.solver.progressText;
			this.tileswap_solver_status.Refresh();
			
			CAF_Application.log.add(LogType.INFO, CAF_Application.stats.getAsText() + "\r\n");
			CAF_Application.log.add(LogType.INFO, this.tileswap_solver_status.Text + "\r\n");
			CAF_Application.log.add(LogType.INFO, this.tileswap_status.Text + "\r\n");
		}
		
		private void cellClick(Object sender, DataGridViewCellMouseEventArgs e)
		{
			//CAF_Application.log.add(LogType.INFO, "cell click on col: " + e.ColumnIndex + ", row: " + e.RowIndex);

			int selectedRow = e.RowIndex;
			if ( selectedRow > -1 && this.ts.opportunities.Count > selectedRow )
			{
				// restore board tileset from backup
				Program.TheMainForm.board.tileset = Program.TheMainForm.board.copyTileSet(this.tileset);
				// restore board model from backup
				Program.TheMainForm.board.tilepos = new List<int>(this.tilepos).ToArray();
				
				// row clicked - use opportunity, place tiles on board
				// get opportunity id from datagrid (as it may be re-sorted)
				int oppId = Convert.ToInt16(this.dataGridView1.Rows[selectedRow].Cells[0].Value);
				Solution opportunity = this.ts.opportunities[oppId];

				CAF_Application.log.add(LogType.INFO, "clicked: " + oppId + "\r\n");

				this.tileswap_status.Text = "Applying opportunity " + opportunity.id;
				this.tileswap_status.Update();

				// clear region
				Program.TheMainForm.board.clearRegion(opportunity.region);

				foreach ( int cellId in opportunity.board.Keys )
				{
					int[] colrow = Board.getColRowFromPos(cellId);
					string tile = opportunity.board[cellId];
					Tile itile = Program.TheMainForm.board.getTileByPattern(tile);

					// remove old tile if exists
					Tile oldtile = Program.TheMainForm.board.getTileFromColRow(colrow[0], colrow[1]);
					if ( oldtile != null )
					{
						Program.TheMainForm.board.removeTile(colrow[0], colrow[1]);
					}
					
					// draw tile on board
					Program.TheMainForm.board.drawTile(colrow[0], colrow[1], itile);
				}
				Program.TheMainForm.board.updateScore();
				Program.TheMainForm.board.refresh();
				this.tileswap_status.Text = "Applied opportunity " + opportunity.id;
				this.tileswap_status.Update();
				
				// play sound when opportunity applied
				System.Media.SystemSounds.Beep.Play();
			}
		}
		
		public void updateProgress()
		{
			this.progressCount++;
			if ( this.progressCount < 1000 || this.progressCount % 1000 == 0 )
			{
				this.tileswap_status.Text = String.Format("Processing region {0} / {1}, opportunity {2} / {3}, eligible opportunities: {4}, best gain: {5}, improved efficiency: {6}", this.ts.numRegions, this.ts.totalRegions, this.ts.numOpportunities, this.ts.totalOpportunities, this.ts.potentialOpportunities, this.ts.bestOppGain, this.ts.improvedEfficiency);
				this.tileswap_status.Refresh();
				Application.DoEvents();
			}
		}

		public bool updateSolverProgress(string text)
		{
			this.solverProgressCount++;
			if ( this.solverProgressCount % 10000 == 0 )
			{
				this.tileswap_solver_status.Text = text;
				this.tileswap_solver_status.Refresh();
				
				// update overall progress as well
				this.tileswap_status.Text = String.Format("Processing region {0} / {1}, opportunity {2} / {3}, eligible opportunities: {4}, best gain: {5}, improved efficiency: {6}", this.ts.numRegions, this.ts.totalRegions, this.ts.numOpportunities, this.ts.totalOpportunities, this.ts.potentialOpportunities, this.ts.bestOppGain, this.ts.improvedEfficiency);
				this.tileswap_status.Refresh();
				
				Application.DoEvents();
				
				if ( CAF_Application.config.contains("runtime_is_stopped" ) )
				{
					return false;
				}
			}
			return true;
		}

		void BtStopClick(object sender, EventArgs e)
		{
			CAF_Application.config.setValue("runtime_is_stopped", "1");
			Application.DoEvents();
		}
	}
}

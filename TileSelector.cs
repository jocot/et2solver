/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 25/11/2009
 * Time: 8:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ET2Solver
{
	/// <summary>
	/// Description of TileSelector.
	/// </summary>
	public partial class TileSelector : Form
	{
		public string loaded_tileset = "";
	
		public TileSelector()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
		
		public void createImagelist()
		{
			this.listTilesetImages.Items.Clear();
    		this.listTilesetImages.AutoSize = false;
    		this.listTilesetImages.LargeImageList = new System.Windows.Forms.ImageList();
    		this.listTilesetImages.LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.listTilesetImages.LargeImageList.ImageSize = new Size(Program.TheMainForm.board.cellWidth, Program.TheMainForm.board.cellHeight);
		}
		
		public void create2x2Imagelist()
		{
			this.listTilesetImages.Items.Clear();
    		this.listTilesetImages.AutoSize = false;
    		this.listTilesetImages.LargeImageList = new System.Windows.Forms.ImageList();
    		this.listTilesetImages.LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.listTilesetImages.LargeImageList.ImageSize = new Size(Program.TheMainForm.board.cellWidth*2, Program.TheMainForm.board.cellHeight*2);
		}
		
		public void loadImages(string tileset)
		{
			if ( tileset == this.loaded_tileset || tileset == "" )
			{
				return;
			}
			this.createImagelist();

			foreach ( Tile tile in Program.TheMainForm.board.tileset )
			{
				TileGraphics gtile = new TileGraphics(tile.pattern.Clone().ToString());
				this.listTilesetImages.Items.Add(tile.title(), tile.id.ToString());
				this.listTilesetImages.LargeImageList.Images.Add(tile.id.ToString(), gtile.image);
			}
			this.loaded_tileset = tileset;
		}
		
		void BtCloseClick(object sender, EventArgs e)
		{
			this.Hide();
		}
		
		void ListTilesetImagesSelectedIndexChanged(object sender, EventArgs e)
		{
			if ( this.listTilesetImages.SelectedIndices.Count > 0 )
			{
				string tileinfo = this.listTilesetImages.SelectedItems[0].Text;
				if ( tileinfo.Length != 19 )
				{
					// extract single tileId & rotation
					string[] parts = tileinfo.Split(' ');
					// tileId,rotation,pattern,col,row
					int tileId = System.Convert.ToInt16(parts[0]);
					int rotation = System.Convert.ToInt16(parts[1]);
					string pattern = parts[2];
					this.textTileDump.AppendText(pattern + "\r\n");
				}
				else
				{
					// extract 2x2 tiles & rotation
					this.textTileDump.AppendText(tileinfo + "\r\n");
				}
			}
		}
		
		void BtClearDumpClick(object sender, EventArgs e)
		{
			this.textTileDump.Clear();
		}
		
		void BtImportClick(object sender, EventArgs e)
		{
			Program.TheMainForm.rbImportTiles.Checked = true;
			this.Hide();
		}
		
		void BtCopyClick(object sender, EventArgs e)
		{
			this.textTileDump.SelectAll();
			this.textTileDump.Copy();
		}
		
		void BtSearch2x2Click(object sender, EventArgs e)
		{
			Program.TheMainForm.initSolver(false);
		
			bool tileGraphicsSetting = Program.TheMainForm.useTileGraphics;
			Program.TheMainForm.useTileGraphics = true;
			List<string> lsearch = new List<string>(this.inputSearch.Text.Split(','));
			if ( lsearch.Count == 4 )
			{
				// load filter
				string[] filter = this.input2x2Filter.Text.Split('\n');
				string whereFilter = "";
				foreach ( string fpattern in filter )
				{
					string[] tileFilters = fpattern.Trim().Split(',');
					if ( tileFilters.Length >= 1 && tileFilters[0] != "" )
					{
						whereFilter += " AND tileA NOT REGEXP '" + tileFilters[0] + "'";
					}
					if ( tileFilters.Length >= 2 && tileFilters[1] != ""  )
					{
						whereFilter += " AND tileB NOT REGEXP '" + tileFilters[1] + "'";
					}
					if ( tileFilters.Length >= 3 && tileFilters[2] != ""  )
					{
						whereFilter += " AND tileC NOT REGEXP '" + tileFilters[2] + "'";
					}
					if ( tileFilters.Length >= 4 && tileFilters[3] != ""  )
					{
						whereFilter += " AND tileD NOT REGEXP '" + tileFilters[3] + "'";
					}
				}

				// count results
				string sql = "SELECT COUNT(*)"
					+ " FROM internal"
					+ " WHERE tileA REGEXP '" + lsearch[0] + "' AND tileB REGEXP '" + lsearch[1] + "' AND tileC REGEXP '" + lsearch[2] + "' AND tileD REGEXP '" + lsearch[3] + "'"
					+ whereFilter;
				List<string> results = Program.TheMainForm.executeSQL(sql);
				string count = "0";
				if ( results.Count > 0 )
				{
					count = Program.TheMainForm.executeSQL(sql)[0];
				}

				sql = "SELECT tileA,tileB,tileC,tileD"
					+ " FROM internal"
					+ " WHERE tileA REGEXP '" + lsearch[0] + "' AND tileB REGEXP '" + lsearch[1] + "' AND tileC REGEXP '" + lsearch[2] + "' AND tileD REGEXP '" + lsearch[3] + "'"
					+ whereFilter
					+ " LIMIT 0, 256";
				Program.TheMainForm.log("searching 2x2s for:" + this.inputSearch.Text);
				results = Program.TheMainForm.executeSQL(sql);
				this.searchResults.Text = String.Join("\r\n", results.ToArray());
				
				// draw results to listview
				this.create2x2Imagelist();
				int numExcluded = 0;
				foreach ( string tiles2x2 in results )
				{
					List<string> tiles = new List<string>(tiles2x2.Split(','));
					bool excluded = false;
					
					// load filter
					/*
					string[] filter = this.input2x2Filter.Text.Split('\n');
					foreach ( string fpattern in filter )
					{
						if ( Regex.Match(tiles2x2, fpattern.Trim(), RegexOptions.IgnoreCase).Success )
						{
							excluded = true;
							numExcluded++;
							break;
						}
					}
					*/
					
					if ( tiles.Count == 4 && !excluded )
					{
						// create tileA image and palce onto blank 2x2 canvas
						Tile tileA = Program.TheMainForm.board.getTileByPattern(tiles[0]);
						Image image = tileA.gtile.createBlankTile(Program.TheMainForm.board.cellWidth*2, Program.TheMainForm.board.cellHeight*2);
						Graphics g = Graphics.FromImage(image);
						g.DrawImage(tileA.gtile.image, 0, 0, Program.TheMainForm.board.cellWidth, Program.TheMainForm.board.cellHeight);
//						this.searchResults.AppendText("canvas width: " + image.Width.ToString() + ", height: " + image.Height.ToString() + "\r\n");
//						this.searchResults.AppendText("image width: " + tile.gtile.image.Width.ToString() + ", height: " + tile.gtile.image.Height.ToString() + "\r\n");

						// create tileB image
						Rectangle sr = new Rectangle(0, 0, Program.TheMainForm.board.cellWidth+16, Program.TheMainForm.board.cellHeight+16);
						Rectangle dr = new Rectangle(50, 0, Program.TheMainForm.board.cellWidth, Program.TheMainForm.board.cellHeight);
						Tile tileB = Program.TheMainForm.board.getTileByPattern(tiles[1]);
						g.DrawImage(tileB.gtile.image, 50, 0, Program.TheMainForm.board.cellWidth, Program.TheMainForm.board.cellHeight);
						
						// create tileC image
						Tile tileC = Program.TheMainForm.board.getTileByPattern(tiles[2]);
						dr = new Rectangle(0, 50, Program.TheMainForm.board.cellWidth, Program.TheMainForm.board.cellHeight);
						g.DrawImage(tileC.gtile.image, dr, sr, GraphicsUnit.Pixel);

						// create tileD image
						Tile tileD = Program.TheMainForm.board.getTileByPattern(tiles[3]);
						dr = new Rectangle(50, 50, Program.TheMainForm.board.cellWidth, Program.TheMainForm.board.cellHeight);
						g.DrawImage(tileD.gtile.image, dr, sr, GraphicsUnit.Pixel);

						// exclude used tiles - cant as used tile is within results when searching....
						/*
						if ( !Program.TheMainForm.solver.usedTiles.Contains(tileA.id) &&
							!Program.TheMainForm.solver.usedTiles.Contains(tileB.id) &&
							!Program.TheMainForm.solver.usedTiles.Contains(tileC.id) &&
							!Program.TheMainForm.solver.usedTiles.Contains(tileD.id) )
						{
						*/
						this.listTilesetImages.Items.Add(tiles2x2, tiles2x2);
						this.listTilesetImages.LargeImageList.Images.Add(tiles2x2, image);
					}
				}
				if ( numExcluded > 0 )
				{
					this.labelSearchStatus.Text = results.Count.ToString() + " / " + count + " results, " + numExcluded + " excluded via filter";
				}
				else
				{
					this.labelSearchStatus.Text = results.Count.ToString() + " / " + count + " results";
				}

			}
			else
			{
				this.searchResults.Text = "Enter 4 tile regex parameters in the format: ABCD,ABCD,[F-J],[K-V]";
			}
			Program.TheMainForm.useTileGraphics = tileGraphicsSetting;
		}
		
		void BtViewTilesetClick(object sender, EventArgs e)
		{
			this.loaded_tileset = "";
			this.createImagelist();
			this.loadImages(Board.title);
		}
		
		void BtSearchTilesetClick(object sender, EventArgs e)
		{
			bool tileGraphicsSetting = Program.TheMainForm.useTileGraphics;
			Program.TheMainForm.useTileGraphics = true;
			
			List<string> lsearch = new List<string>(this.inputSearch.Text.Split('\n'));
			List<string> results = new List<string>();
			int totalTiles = 0;
			HashSet<int> totalUniqueTiles = new HashSet<int>();
			Dictionary<string, int> patternCount = new Dictionary<string, int>();
			if ( lsearch.Count > 0 )
			{
    			if ( this.cbImportTiles.Checked )
    			{
    				this.textTileDump.Clear();
    			}
				this.createImagelist();
				foreach ( string rsearch in lsearch )
				{
					string search = rsearch.Trim();
					if ( search.Length > 0 )
					{
						List<string> tresults = new List<string>();
						HashSet<int> uniqueTiles = new HashSet<int>();
						int numTiles = 0;
						
					    for ( int i = 1; i <= Program.TheMainForm.board.tileset.Length; i++ )
					    {
				        	Tile tile = Program.TheMainForm.board.tileset[i-1];
				        	
				        	// count patterns if checked
				        	if ( this.cbCountPatterns.Checked )
				        	{
				        		int numPatterns = Regex.Matches(tile.pattern, search, RegexOptions.IgnoreCase).Count;
				        		if ( patternCount.ContainsKey(search) )
				        		{
				        			patternCount[search] += numPatterns;
				        		}
				        		else
				        		{
				        			patternCount.Add(search, numPatterns);
				        		}
				        	}
				        	
					        for ( int r = 1; r <= 4; r++ )
					        {
					        	string pattern = tile.patterns[r-1];
					        	if ( Regex.IsMatch(pattern, search, RegexOptions.IgnoreCase) )
					        	{
					        		numTiles++;
									totalTiles++;
					        		if ( this.cbUniquesOnly.Checked )
					        		{
					        			if ( !uniqueTiles.Contains(tile.id) )
					        			{
						        			uniqueTiles.Add(tile.id);
					        				if ( !this.cbCountOnly.Checked )
					        				{
								        		tresults.Add(tile.id + ". " + pattern);
								        		this.addImageResult(new Tile(tile.id, pattern, r));
					        				}
					        			}
					        			if ( !totalUniqueTiles.Contains(tile.id) && this.cbImportTiles.Checked )
					        			{
					        				this.textTileDump.AppendText(pattern + "\r\n");
					        			}
					        		}
					        		else
					        		{
										if ( !this.cbCountOnly.Checked )
				        				{
							        		tresults.Add(tile.id + ". " + pattern);
							        		this.addImageResult(new Tile(tile.id, pattern, r));
				        				}
						        		if ( !uniqueTiles.Contains(tile.id) )
						        		{
						        			uniqueTiles.Add(tile.id);
						        		}
					        			if ( this.cbImportTiles.Checked )
					        			{
					        				this.textTileDump.AppendText(pattern + "\r\n");
					        			}
					        		}
					        		if ( !totalUniqueTiles.Contains(tile.id) )
					        		{
					        			totalUniqueTiles.Add(tile.id);
					        		}
					        	}
					        }
					    }
						if ( !this.cbUniquesOnly.Checked )
						{
						    results.Add(String.Format("search: {0}, {1} tile(s) found", search, numTiles));
						}
					    results.Add(String.Format("search: {0}, {1} unique tile(s) found", search, uniqueTiles.Count));
					    results.AddRange(tresults);
					}
					else
					{
					    results.Add("");
					}
				}
				foreach ( string key in patternCount.Keys )
				{
					results.Add(String.Format("search: {0}, {1} pattern(s) found", key, patternCount[key]));
				}
			    results.Add(String.Format("{0} total tile(s) found", totalTiles));
			    results.Add(String.Format("{0} total unique tile(s) found", totalUniqueTiles.Count));
				this.searchResults.Text = String.Join("\r\n", results.ToArray());
			}
			else
			{
				this.searchResults.Text = "Enter regex tile search parameters in the format: [A-V]{2}";
			}

			Program.TheMainForm.useTileGraphics = tileGraphicsSetting;
		}
		
		void BtCopySearchResultsClick(object sender, EventArgs e)
		{
			this.searchResults.SelectAll();
			this.searchResults.Copy();
		}
		
		public void addImageResult(Tile tile)
		{
			TileGraphics gtile = new TileGraphics(tile.pattern.Clone().ToString());
			this.listTilesetImages.Items.Add(tile.title(), tile.id.ToString());
			this.listTilesetImages.LargeImageList.Images.Add(tile.id.ToString(), gtile.image);
		}
		
		
		void BtCopyExcludeFilterClick(object sender, EventArgs e)
		{
			this.input2x2Filter.SelectAll();
			this.input2x2Filter.Copy();
		}
		
		void BtClearExcludeFilterClick(object sender, EventArgs e)
		{
			this.input2x2Filter.Clear();
		}
		
		
		void BtClearInputSearchClick(object sender, EventArgs e)
		{
			this.inputSearch.Clear();
			this.searchResults.Clear();
			this.input2x2Filter.Clear();
		}
	}
}

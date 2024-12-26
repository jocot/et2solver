/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 23/11/2009
 * Time: 3:57 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using CAF;

namespace ET2Solver
{
	/// <summary>
	/// manages drawing tiles on the board
	/// </summary>
	public class Board
	{
		// size of board cells
		// TODO - workout why it is not creating images at the correct size??
		public int cellWidth = 50;
		public int cellHeight = 50;

		// board title = colsxrowsxnumEdgesxnuminner_seed
		public static string title = "";

	    // board dimensions
	    public static int num_cols = 16;
	    public static int num_rows = 16;
	    public static int max_tiles = 16*16;
	    public static string boardfilename = "board_16x16.jpg";
	    public static int num_edges = 5;
	    public static int num_inner1 = 5;
	    public static int num_inner1h = 240;
	    public static int num_inner2 = 12;
	    public static int num_inner2h = 600;
	    public static int num_inner3 = 0;
	    public static int num_inner3h = 0;

	    // pattern regex strings
	    public static string edge_pattern_regex = "";
	    public static string internal_pattern_regex = "";

	    // board layouts
	    public static List<string> layouts = new List<string>();

	    public bool isCancel = false;

		// fixed rotation - only use 256 pieces instead of full 1024 (requires balanced tileset)
		public bool fixedRotation = false;

		public Image image;
		public Image blankBoardImage;
		private Graphics gboard;
		public bool redraw = false;

		// matching stats / overlays
		public System.Collections.Hashtable matchingTileList = new Hashtable();
		public System.Collections.ArrayList swappableTileList = new ArrayList();
		public System.Collections.ArrayList overlays = new ArrayList();
		public System.Collections.ArrayList swappableOverlays = new ArrayList();
		public System.Drawing.Point selectedOverlay = new Point(0,0);

		// search string used for interactive search
		public string searchMatch = "";

		// stats
		public System.Collections.ArrayList patternStats = new ArrayList();
		public string log_stats = "";

		// tileset
		public static Int64 seed;
		public bool patternImagesLoaded = false;
		public List<string> patterns = new List<string>();
		public Hashtable patternImages = new Hashtable();
		public Tile[] tileset = new Tile[0];
		public static int numUniqueTiles = 0;
		public List<string> originalTileset = new List<string>();

		// model
		public int[] tilepos = new int[0];
		public int score = 0;
		public int maxScore = 0;

		// save image of model when saving
		public bool saveSolutionImages = false;

		public Board()
		{
			this.patterns.Clear();
			string[] filenames = System.IO.Directory.GetFiles(CAF_Application.config.imagePath() + "\\patterns-diagonal", "*.png");
			foreach ( string filename in filenames )
			{
				string pattern = System.IO.Path.GetFileNameWithoutExtension(filename);
				this.patterns.Add(pattern.ToUpper());
			}
		}

		public static void defineRegexStrings(Tile[] tilelist)
		{
			// define regex patterns from analysing tileset

			// edge patterns (get from 1 & 3 / top & bottom of ^-... search)
			Board.edge_pattern_regex = "[";
			Dictionary<string, int[]> searches = new Dictionary<string, int[]>();
			searches.Add("^-[^-]{3}|^[^-]{2}-[^-]", new int[]{1,3});
			searches.Add("^[^-]-[^-]{2}|^[^-]{3}-", new int[]{0,2});
			foreach ( Tile tile in tilelist )
			{
				foreach ( string search in searches.Keys )
				{
		            if ( Regex.IsMatch(tile.pattern, search, RegexOptions.IgnoreCase) )
		            {
						int[] offsets = searches[search];
	            		foreach ( int i in offsets )
	            		{
	            			char ch = tile.pattern[i];
			            	if ( !Board.internal_pattern_regex.Contains(ch) )
			            	{
			            		Board.internal_pattern_regex += ch;
	            			}
		            	}
		            }
				}
			}
			if ( Board.edge_pattern_regex.Length == 1 )
			{
				Board.edge_pattern_regex += "^-";
			}
			Board.edge_pattern_regex += "]";

			// internal patterns (get from [^-]{4} search)
			Board.internal_pattern_regex = "[";
			string isearch = "[^-]{4}";
			foreach ( Tile tile in tilelist )
			{
	            if ( Regex.IsMatch(tile.pattern, isearch, RegexOptions.IgnoreCase) )
	            {
	            	for ( int i = 0; i <= 3; i++ )
	            	{
		            	if ( !Board.internal_pattern_regex.Contains(tile.pattern[i]) )
		            	{
		            		Board.internal_pattern_regex += tile.pattern[i];
		            	}
	            	}
	            }
			}
			if ( Board.internal_pattern_regex.Length == 1 )
			{
				Board.internal_pattern_regex += "^-";
			}
			Board.internal_pattern_regex += "]";
		}

		public void setLayout()
		{
			bool validLayout = false;
			if ( Program.TheMainForm.selBoardLayout.SelectedIndex > -1 )
			{
				// cols,rows,numEdges,numInner1,numInner1Halves,numInner2,numInner2Halves,numInner3,numInner3Halves
				// 16x16x5x5x240x12x600x0x0 title
				string[] layout = Program.TheMainForm.selBoardLayout.Text.Split(new char[]{' '}, 2);
				string[] layoutParams = layout[0].Split('x');
				if ( layoutParams.Length >= 5 )
				{
					num_cols = System.Convert.ToInt16(layoutParams[0]);
					num_rows = System.Convert.ToInt16(layoutParams[1]);
					num_edges = System.Convert.ToInt16(layoutParams[2]);
					num_inner1 = System.Convert.ToInt16(layoutParams[3]);
					num_inner1h = System.Convert.ToInt16(layoutParams[4]);
					num_inner2 = System.Convert.ToInt16(layoutParams[5]);
					num_inner2h = System.Convert.ToInt16(layoutParams[6]);
					num_inner3 = System.Convert.ToInt16(layoutParams[7]);
					num_inner3h = System.Convert.ToInt16(layoutParams[8]);
					// maximum of 26 edge + inner patterns (A-Z)
					int numInternals = Board.num_inner1 + Board.num_inner2 + Board.num_inner3;
//					if ( num_cols <= 16 && num_rows <= 16 && num_edges + numInternals <= 26 )
					if ( num_cols <= 16 && num_rows <= 16 && num_edges + numInternals > 1 )
					{
						validLayout = true;
					}
				}
			}
			if ( !validLayout )
			{
				System.Windows.Forms.MessageBox.Show("Invalid board size specified. Using default 16x16.");
				// default board layout
				num_cols = 16;
				num_rows = 16;
				num_edges = 5;
				num_inner1 = 5;
				num_inner1h = 240;
				num_inner2 = 12;
				num_inner2h = 600;
				num_inner3 = 0;
				num_inner3h = 0;
			}
			max_tiles = num_cols * num_rows;
			Board.boardfilename = "board_" + num_cols + "x" + num_rows + ".jpg";
			if ( !System.IO.File.Exists(CAF_Application.config.imagePath() + "\\boards\\" + Board.boardfilename) )
			{
				System.Windows.Forms.MessageBox.Show("Could not load board image file: " + CAF_Application.config.imagePath() + "\\boards\\" + Board.boardfilename);
				return;
			}
			this.blankBoardImage = new Bitmap(CAF_Application.config.imagePath() + "\\boards\\" + Board.boardfilename);
			Program.TheMainForm.pb_board.Load(CAF_Application.config.imagePath() + "\\boards\\" + Board.boardfilename);
			this.gboard = Graphics.FromImage(Program.TheMainForm.pb_board.Image);
			this.clear();
			if ( Program.TheMainForm.solver != null && Program.TheMainForm.inputS1CurrentSolveMethod.Text != "" )
			{
				Program.TheMainForm.solver.setSolveMethod(Program.TheMainForm.inputS1CurrentSolveMethod.Text, Program.TheMainForm.solver.solve_path_id);
			}
		}

		public void clear()
		{
			// clears the model - keeps the tileset
			// load backup copy of board image for reclipping when tiles are removed
			Program.TheMainForm.textModel.Text = "";
			Program.TheMainForm.updateStatusTilesUsed(0);
			this.tilepos = new int[Board.max_tiles];
			Program.TheMainForm.searchFreeResultsImages.Items.Clear();
			Program.TheMainForm.searchUsedResultsImages.Items.Clear();
			this.clearOverlays();
			this.redraw = true;
		}

		public void refresh()
		{
			// refresh board
			Program.TheMainForm.pb_board.Refresh();
		}

		public Point getXYFromColRow(int col, int row)
		{
		    if ( row > Board.num_rows )
		    {
		        row = Board.num_rows;
		    }
		    if ( row < 1 )
		    {
		        row = 1;
		    }
		    if ( col > Board.num_cols )
		    {
		        col = Board.num_cols;
		    }
		    if ( col < 1 )
		    {
		        col = 1;
		    }
		    int x = (col-1) * (this.cellWidth + 1);
		    int y = (row-1) * (this.cellHeight + 1);
		    return new Point(x,y);
		}

		// create a tileset of images
		public bool loadTileSet(string id)
		{
			this.setLayout();
			this.loadPatternImages();
			string sourcefile = "tilesets\\" + id + ".txt";
			string[] lines = null;
			string pattern = "";
			int totalScore = 0;
			this.maxScore = 0;
			//if ( System.IO.File.Exists(sourcefile) )
			try
			{
				Program.TheMainForm.log("Loading tiles from tileset file " + sourcefile);
				lines = System.IO.File.ReadAllLines(sourcefile);
			}
			catch
			{
				Program.TheMainForm.log("Error - could not open tileset " + sourcefile);
				return false;
			}

			Program.TheMainForm.textTileset.Text = "";
			this.tileset = new Tile[lines.Length];
			this.originalTileset = new List<string>();
			string tileset = "";
			// disable tile graphics to speed up loading
			Program.TheMainForm.useTileGraphics = false;
			for ( int i = 0; i < lines.Length; i++ )
			{
				pattern = lines[i].Trim();
				if ( pattern.Length == 4 )
				{
					this.tileset[i] = new Tile(i+1, pattern, 1);
					totalScore += pattern.Replace("-", "").Length;
					this.originalTileset.Add(pattern);
					tileset += pattern + "\r\n";
				}
			}
			if ( tileset.Length == 0 )
			{
				return false;
			}
			this.tilepos = new int[Board.max_tiles];
			Program.TheMainForm.useTileGraphics = true;
			Program.TheMainForm.textTileset.Text = tileset;
			// save backup of tileset for this.compareBoardToTileset()
			Program.TheMainForm.textTileset.Update();
			this.maxScore = totalScore / 2;
			Program.TheMainForm.updateStatusScore(0);
			Board.title = id;
			Program.TheMainForm.selectTileSetListId();
			Program.TheMainForm.log("Created " + lines.Length + " tiles from tileset file " + sourcefile);
//			Program.TheMainForm.loadModelList();

			Board.defineRegexStrings(this.tileset);
			this.setModel();
			this.redraw = true;
			this.refresh();
			return true;
		}

		public void setTileset()
		{
			this.setLayout();
			this.loadPatternImages();
			string[] lines = Program.TheMainForm.textTileset.Text.Trim().Split('\n');
			string pattern = "";
			int totalScore = 0;
			this.maxScore = 0;
			this.tileset = new Tile[lines.Length];
			// disable tile graphics to speed up loading
			Program.TheMainForm.useTileGraphics = false;
			for ( int i = 0; i < lines.Length; i++ )
			{
				pattern = lines[i].Trim();
				if ( pattern.Length == 4 )
				{
					this.tileset[i] = new Tile(i+1, pattern, 1);
					totalScore += pattern.Replace("-", "").Length;
					//Program.TheMainForm.log(pattern);
				}
			}
			this.tilepos = new int[Board.max_tiles];
			Program.TheMainForm.useTileGraphics = true;
			Program.TheMainForm.textTileset.Update();
			this.maxScore = totalScore / 2;
			Program.TheMainForm.updateStatusScore(0);
			Program.TheMainForm.log("Loaded " + lines.Length + " tiles from tileset");
//			Program.TheMainForm.loadModelList();
			this.redraw = true;
		}

		public void saveTileset()
		{
			string id = Program.TheMainForm.textSaveTilesetName.Text;
			if ( id.Trim() == "" )
			{
				System.Windows.Forms.MessageBox.Show("Enter a name for the tileset before saving.");
				return;
			}
			string filename = "tilesets\\" + id + ".txt";
			string tileset = Program.TheMainForm.textTileset.Text;
			try
			{
				if ( System.IO.File.Exists(filename) )
				{
					System.Windows.Forms.DialogResult confirm = System.Windows.Forms.MessageBox.Show("Overwrite " + filename + " ?", "Save Tileset", System.Windows.Forms.MessageBoxButtons.YesNo);
					if ( confirm.Equals(System.Windows.Forms.DialogResult.Yes) )
					{
						System.IO.File.WriteAllText(filename, tileset);
						Program.TheMainForm.log("saved tileset to " + filename);
					}
				}
				else
				{
					System.IO.File.WriteAllText(filename, tileset);
					Program.TheMainForm.log("saved tileset to " + filename);
				}
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("Error saving tileset " + filename);
			}
		}

		public void getModel()
		{
			// gets current model from board placements and saves to model text box
			string model = "";
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				string line = "";
				if ( this.tilepos[i] > 0 )
				{
					// v1 col,row,tileId,rotation
					/*
					line += this.tileset[this.tilepos[i]-1].col;
					line += "," + this.tileset[this.tilepos[i]-1].row;
					line += "," + this.tilepos[i];
					line += "," + this.tileset[this.tilepos[i]-1].rotation;
					model += line + "\r\n";
					*/

					// v2 format col,row,pattern
//					line += this.tileset[this.tilepos[i]-1].col;
//					line += "," + this.tileset[this.tilepos[i]-1].row;
					// get col,row from board rather than tile as it seems to get mucked up during tileswap etc
					int[] colrow = Board.getColRowFromPos(i + 1);
					line += colrow[0];
					line += "," + colrow[1];
					line += "," + this.tileset[this.tilepos[i]-1].pattern;
					model += line + "\r\n";
				}
			}
			Program.TheMainForm.textModel.Text = model;
			Program.TheMainForm.textModel.Update();
			Program.TheMainForm.model_name = "new";
//			Program.TheMainForm.selectModelListId();
		}

		public void setModelData(string[] lines)
		{
			Tile tile = null;
			int i = 0;

			// disable tile graphics for faster loading
			Program.TheMainForm.useTileGraphics = false;

			// use solver for v2 models
			Solver s = new Solver();
			s.loadTileset();

			Array.Sort(lines);
			this.tilepos = new int[Board.max_tiles];
			List<string> model = new List<string>();

			try
			{
				foreach ( string line in lines )
				{
					string[] parts = line.Trim().Split(',');
					if ( parts.Length >= 3 )
					{
						int col = 0;
						int row = 0;
						int tileId;
						int rotation = 1;

						// v1 format col,row,tileId,rotation
						if ( Program.TheMainForm.isNumeric(parts[0]) && Program.TheMainForm.isNumeric(parts[1]) && Program.TheMainForm.isNumeric(parts[2]) && Program.TheMainForm.isNumeric(parts[3]) )
						{
							col = Convert.ToInt16(parts[0]);
							row = Convert.ToInt16(parts[1]);
							tileId = Convert.ToInt16(parts[2]);
							// skip duplicate tiles only for unique tilesets
							if ( Board.numUniqueTiles != Board.max_tiles || ( Board.numUniqueTiles == Board.max_tiles && !this.isTileUsed(tileId) ) )
							{
								int pos = (row - 1) * Board.num_cols + col;
								if ( pos > this.tilepos.Length )
								{
									throw new Exception("Invalid tile position " + pos + " [" + col + "," + row + "] on line: " + i + 1);
								}
								else
								{
									this.tilepos[pos-1] = tileId;
									tile = this.tileset[tileId-1];
								}
								if ( parts.Length >= 4 )
								{
									rotation = Convert.ToInt16(parts[3]);
								}
							}
							else
							{
								Program.TheMainForm.log("Skipping duplicate tile: " + tileId + " on line " + i);
								tile = null;
							}
						}
						else if ( Program.TheMainForm.isNumeric(parts[0]) && Program.TheMainForm.isNumeric(parts[1]) )
						{
							// v2 format col,row,pattern
							col = Convert.ToInt16(parts[0]);
							row = Convert.ToInt16(parts[1]);
							if ( col > 0 && row > 0 )
							{
								string pattern = parts[2];
								tileId = s.getTileId(pattern);
								if ( Board.numUniqueTiles != Board.max_tiles || ( Board.numUniqueTiles == Board.max_tiles && !this.isTileUsed(tileId) ) )
								{
									rotation = s.getTileRotationByPattern(pattern);
									int pos = (row - 1) * Board.num_cols + col;
									this.tilepos[pos-1] = tileId;
									if ( pos > this.tilepos.Length )
									{
										throw new Exception("Invalid tile position " + pos + " [" + col + "," + row + "] on line: " + i + 1);
									}
									else
									{
										this.tilepos[pos-1] = tileId;
										tile = this.tileset[tileId-1];
									}
								}
								else
								{
									Program.TheMainForm.log("Skipping duplicate tile: " + tileId + " on line " + i);
									tile = null;
								}
							}
						}

						if ( tile != null )
						{
							if ( !this.fixedRotation )
							{
								tile.rotate(rotation);
							}
							tile.moveTo(col, row);
							//this.drawTile(col, row, tile);

							// rewrite model in v2 format
							model.Add(col.ToString() + "," + row.ToString() + "," + tile.pattern);

						}

						i++;
					}

					Program.TheMainForm.updateStatusTilesUsed(i);
				}
				Program.TheMainForm.log("Set model with " + i.ToString() + " tiles");
				this.updateScore();
				Program.TheMainForm.useTileGraphics = true;
				Program.TheMainForm.textModel.Text = String.Join("\r\n", model.ToArray());
				Program.TheMainForm.textModel.Update();

			}
			catch (Exception e)
			{
				Program.TheMainForm.log("Exception: " + e.Message + ", source: " + e.StackTrace);
				Program.TheMainForm.log("Error setting model at line " + i);
			}
		}

		public bool loadModel(string id)
		{
			this.setLayout();
			if ( this.tileset.Length == 0 )
			{
				this.loadTileSet(Board.title);
			}

			string sourcefile = id;
			if ( !System.IO.File.Exists(sourcefile) )
			{
				sourcefile = "models\\" + Board.title + "-" + id + ".txt";
				if ( !System.IO.File.Exists(sourcefile) )
				{
					sourcefile = "models\\" + id;
				}
			}
			//if ( System.IO.File.Exists(sourcefile) )
			try
			{
				string[] lines = System.IO.File.ReadAllLines(sourcefile);
				Program.TheMainForm.log("Read " + lines.Length + " lines from model file " + sourcefile);
				this.setModelData(lines);
				Program.TheMainForm.model_name = id;
//				Program.TheMainForm.selectModelListId();
				this.redraw = true;
				this.refresh();
				return true;
			}
			catch (Exception e)
			{
				Program.TheMainForm.log("Exception: " + e.Message + ", source: " + e.StackTrace);
				Program.TheMainForm.log("Error loading model " + sourcefile);
				this.redraw = true;
				this.refresh();
				return false;
			}
		}

		public void setModel()
		{
			if ( this.tileset.Length == 0 )
			{
				System.Windows.Forms.MessageBox.Show("Load a tileset first.");
				return;
			}

			// disable tile graphics for faster loading
			Program.TheMainForm.useTileGraphics = false;
			try
			{
				string[] lines = Program.TheMainForm.textModel.Text.Split('\n');
				this.setModelData(lines);
			}
			catch (Exception e)
			{
				Program.TheMainForm.log("Exception: " + e.Message + ", source: " + e.StackTrace);
				Program.TheMainForm.log("Error setting up model");
			}
			Program.TheMainForm.useTileGraphics = true;
			this.refresh();
			this.redraw = true;

			Program.TheMainForm.model_name = "new";
//			Program.TheMainForm.selectModelListId();
			this.drawTiles();
		}

		public void saveModel()
		{
			// use file save dialog
			System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			saveFileDialog1.FileName = Board.title + "-";
//			saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			saveFileDialog1.Filter = Board.title + " models|" + Board.title + "-*.txt|All files (*.*)|*.*";
			saveFileDialog1.FilterIndex = 1;
			saveFileDialog1.RestoreDirectory = true;
			saveFileDialog1.InitialDirectory = "models";
			saveFileDialog1.Title = "Save Model";
//			saveFileDialog1.ShowDialog();

			bool isFileSelected = false;
			if ( saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName != "" )
			{
				isFileSelected = true;
			}
			if ( !isFileSelected )
			{
				System.Windows.Forms.MessageBox.Show("No destination model filename entered");
				return;
			}
			string filename = saveFileDialog1.FileName;

			/*
			string id = Program.TheMainForm.textSaveModelName.Text;
			if ( id.Trim() == "" )
			{
				System.Windows.Forms.MessageBox.Show("Enter a name for the model before saving.");
				return;
			}
			string filename = "models\\" + Board.title + "-" + id + ".txt";
			*/

			string model = Program.TheMainForm.textModel.Text;

			try
			{
				// overwrite prompt not needed when using saveFileDialog
				/*
				if ( System.IO.File.Exists(filename) )
				{
					System.Windows.Forms.DialogResult confirm = System.Windows.Forms.MessageBox.Show("Overwrite " + filename + " ?", "Save Model", System.Windows.Forms.MessageBoxButtons.YesNo);
					if ( confirm.Equals(System.Windows.Forms.DialogResult.Yes) )
					{
						System.IO.File.WriteAllText(filename, model);
						Program.TheMainForm.log("saved model to " + filename);
					}
				}
				else
				{
					System.IO.File.WriteAllText(filename, model);
					Program.TheMainForm.log("saved model to " + filename);
				}
				*/
				System.IO.File.WriteAllText(filename, model);
				Program.TheMainForm.log("saved model to " + filename);
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("Error saving model " + filename);
			}

			// save image
			this.saveModelImage(filename);
		}

		private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for(int i=0; i<codecs.Length; i++)
                if(codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

		public void saveModelImage(string filename)
		{
			// 2-Aug-2010 no idea why this doesnt work, "generic exception error" not very helpful!
			if ( true || !this.saveSolutionImages )
			{
				return;
			}

//				Program.TheMainForm.board.drawTiles();

			string imagepath = "images-models\\";
			if ( !System.IO.File.Exists(imagepath) )
			{
				System.IO.Directory.CreateDirectory(imagepath);
			}
			string imagefilename = imagepath + System.IO.Path.GetFileNameWithoutExtension(filename) + ".jpg";

			// set encoder parameters
			EncoderParameters encoderParams = new EncoderParameters();
			encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);

			// encode image to memory stream
			System.IO.MemoryStream mss = new System.IO.MemoryStream();
			ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

			/*
			Program.TheMainForm.log("RawFormat: " + Program.TheMainForm.pb_board.Image.RawFormat.ToString());
			Program.TheMainForm.log("Bmp: " + System.Drawing.Imaging.ImageFormat.Bmp.Guid.ToString());
			Program.TheMainForm.log("Gif: " + System.Drawing.Imaging.ImageFormat.Gif.Guid.ToString());
			Program.TheMainForm.log("Jpeg: " + System.Drawing.Imaging.ImageFormat.Jpeg.Guid.ToString());
			Program.TheMainForm.log("Png: " + System.Drawing.Imaging.ImageFormat.Png.Guid.ToString());
			Program.TheMainForm.log("MemoryBmp: " + System.Drawing.Imaging.ImageFormat.MemoryBmp.Guid.ToString());
			return;
			*/
//			Program.TheMainForm.pb_board.Image.Save(mss, jpegCodec, encoderParams);

			/*
			this.gboard.Save();
			this.gboard.Dispose();
			Program.TheMainForm.pb_board.Image.Save(imagefilename, System.Drawing.Imaging.ImageFormat.Jpeg);
			*/

			// save memory stream to file
			System.IO.FileStream fs = new System.IO.FileStream(imagefilename, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
			byte[] matriz = mss.ToArray();
			fs.Write(matriz, 0, matriz.Length);
			mss.Close();
			fs.Close();

			Program.TheMainForm.log("saved model image to: " + imagefilename);
		}


		public void drawTile(int col, int row, Tile tile)
		{
			// remove previous tile if exists
			// FIXME - causes slow graphic updates!
			/*
			Tile oldtile = this.getTileFromColRow(col, row);
			if ( oldtile != null )
			{
				this.removeTile(col, row);
			}
			*/
			if ( tile == null )
			{
				return;
			}
			tile.moveTo(col, row);
			// draw tile onto board
			Point point = this.getXYFromColRow(col, row);
			//Program.TheMainForm.log("drawing tile " + tile.title() + " at pos: " + point.X + "," + point.Y);
			if ( tile.image == null )
			{
				tile.updateImage();
			}
			if ( tile.image != null && this.gboard != null )
			{
				this.gboard.DrawImage(tile.image, point.X, point.Y);
			}

			// update tilepos (used tiles)
			int pos = (row - 1) * Board.num_cols + col;
			this.tilepos[pos-1] = tile.id;
		}

		public void drawTileId(int col, int row, int tileId, int rotation)
		{
			// draw tile onto board
			Tile tile = this.tileset[tileId-1];
			if ( !this.fixedRotation )
			{
				tile.rotate(rotation);
			}
			tile.moveTo(col, row);
			Point point = this.getXYFromColRow(col, row);
			//Program.TheMainForm.log("drawing tile " + tile.title() + " at pos: " + point.X + "," + point.Y);
			this.gboard.DrawImage(tile.image, point.X, point.Y);

			// update tilepos (used tiles)
			int pos = (row - 1) * Board.num_cols + col;
			this.tilepos[pos-1] = tileId;
		}

		public void test()
		{
			//System.Threading.Thread t = new System.Threading.Thread(this.gboard.test);
			//t.Start();
			//this.gboard.test();
		}

		public void updateScore()
		{
			// col1-15 - check right & down
			// row 1-15 - check right & down
			// col16 = check down only
			// row16 = check right only
			// 16,16 = skip
			int tileId;
			int[] colrow;
			int col;
			int row;
			string match = "";
			int score = 0;
			Tile tile;
			Tile rightTile;
			Tile belowTile;
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				tileId = this.tilepos[i];
				if ( tileId == 0 )
				{
					continue;
				}
				colrow = Board.getColRowFromPos(i+1);
				col = colrow[0];
				row = colrow[1];
				tile = this.tileset[tileId-1];
				//Program.TheMainForm.log(col + "," + row + " - tileId: " + tileId + ", r: " + tile.rotation + ", pattern: " + tile.pattern);
				if ( col != Board.num_cols && row != Board.num_rows )
				{
					match = "RD";
				}
				else if ( col == Board.num_cols && row != Board.num_rows )
				{
					match = "D";
				}
				else if ( row == Board.num_rows && col != Board.num_cols )
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
						if ( rightTile != null && tile.matchRight(rightTile) )
						{
							score += 1;
						}
						belowTile = this.getTileFromColRow(col, row+1);
						if ( belowTile != null && tile.matchDown(belowTile) )
						{
							score += 1;
						}
						break;
					case "D":
						belowTile = this.getTileFromColRow(col, row+1);
						if ( belowTile != null && tile.matchDown(belowTile) )
						{
							score += 1;
						}
						break;
					case "R":
						rightTile = this.getTileFromColRow(col+1, row);
						if ( rightTile != null && tile.matchRight(rightTile) )
						{
							score += 1;
						}
						break;
				}
			}
			this.score = score;
			Program.TheMainForm.updateStatusScore(score);
		}

		public Tile getTileFromColRow(int col, int row)
		{
			int pos = (row - 1) * Board.num_cols + col;
			if ( this.tilepos.Length < pos )
			{
				//Program.TheMainForm.log("getTileFromColRow(" + col + "," + row + ")=" + pos + ",N/A");
				return null;
			}
			int tileId = this.tilepos[pos-1];
			if ( tileId > 0 )
			{
				Tile tile = this.tileset[tileId-1];
				//Program.TheMainForm.log("getTileFromColRow(" + col + "," + row + ")=" + pos + "," + tile.title());
				return tile;
			}
			else
			{
				return null;
			}
		}

		public static int[] getColRowFromPos(int pos)
		{
			Program.TheMainForm.timer.start("getColRowFromPos");
			// return col,row for board/cell position (1+)
			int[] colrow = new int[2];
			int row = (int)Math.Ceiling((double)pos / (double)num_rows);
			int col = pos - (row-1) * num_cols;
			/*
			if ( col == 0 || row == 0 )
			{
				System.Diagnostics.Debugger.Break();
			}
			*/
			colrow[0] = col;
			colrow[1] = row;
			Program.TheMainForm.timer.stop("getColRowFromPos");
			return colrow;
		}

		public int[] getColRowFromXY(int x, int y)
		{
			int[] colrow = new int[2];
			int col = (int)Math.Floor((double)x / (this.cellWidth + 1) + 1);
		    int row = (int)Math.Floor((double)y / (this.cellHeight + 1) + 1);
			colrow[0] = col;
			colrow[1] = row;
			return colrow;
		}

		public int getTileIdFromColRow(int col, int row)
		{
			int tileId = 0;
			int pos = (row - 1) * Board.num_cols + col;
			if ( this.tilepos.Length >= pos )
			{
				tileId = this.tilepos[pos-1];
			}
			return tileId;
		}

		public void rotateTile(int col, int row)
		{
			Tile tile = this.getTileFromColRow(col, row);
			if ( tile != null )
			{
				if ( !this.fixedRotation )
				{
					tile.rotate(0);
				}
				this.updateScore();
				this.drawTile(col, row, tile);
			}

		}

		public void removeTile(int col, int row)
		{
			int pos = (row - 1) * Board.num_cols + col;
			if ( this.tilepos.Length >= pos )
			{
				int tileId = this.tilepos[pos-1];
				if ( tileId > 0 && this.tileset[tileId-1] != null )
				{
					this.tileset[tileId-1].moveTo(0,0);

					this.tilepos[pos-1] = 0;

					// update score/tile count
					this.updateScore();
					this.updateTileCount();

					// redraw cell
					this.clearCell(col, row);
				}
			}
		}

		public void clearCell(int col, int row)
		{
			Point p = this.getXYFromColRow(col, row);
			Rectangle srcRect = new Rectangle(p.X, p.Y, this.cellWidth, this.cellHeight);
			Rectangle destRect = new Rectangle(p.X, p.Y, this.cellWidth, this.cellHeight);
			this.gboard.DrawImage(this.blankBoardImage, srcRect, destRect, System.Drawing.GraphicsUnit.Pixel);
			Program.TheMainForm.pb_board.Update();
		}

		public void updateTileCount()
		{
			int numTiles = 0;
			foreach ( int tileId in this.tilepos )
			{
				if ( tileId > 0 )
				{
					numTiles++;
				}
			}
			Program.TheMainForm.updateStatusTilesUsed(numTiles);
		}

		public void drawTiles()
		{
			if ( this.redraw )
			{
				//Program.TheMainForm.timer.start("drawtiles");
				foreach ( Tile tile in this.tileset )
				{
					if ( tile != null )
					{
						if ( tile.col > 0 && tile.col <= Board.num_cols && tile.row > 0 && tile.row <= Board.num_rows)
						{
							System.Windows.Forms.Application.DoEvents();
							this.drawTile(tile.col, tile.row, tile);
						}
					}
				}
				this.refresh();
				//Program.TheMainForm.timer.stop("drawtiles");
				//Program.TheMainForm.log(Program.TheMainForm.timer.results("drawtiles"));
				this.redraw = false;
			}
			else
			{
				this.refresh();
			}
		}

		public void loadPatternImages(bool force = false)
		{
			if (force || !this.patternImagesLoaded)
			{
				Image patternImage;
				string pattern;
				this.patternImages = new Hashtable();
				string[] filenames = System.IO.Directory.GetFiles(CAF_Application.config.imagePath() + "\\patterns-diagonal", "*.png");
				foreach ( string filename in filenames )
				{
					pattern = System.IO.Path.GetFileNameWithoutExtension(filename);
					//Program.TheMainForm.log("loading pattern: " + filename);
					patternImage = System.Drawing.Image.FromFile(filename);
					if ( patternImage != null )
					{
						this.patternImages.Add(pattern.ToUpper(), patternImage);
						//Program.TheMainForm.log("loaded pattern: " + pattern + ", " + patternImage.ToString());
					}
				}
				this.patternImagesLoaded = true;
			}
		}

		public string getTileMatchString(int col, int row)
		{
		    string match = "";
//		    string edgeTile = Board.getEdgePatternRegex();
//		    string internalTile = Board.getInternalPatternRegex();
		    string edgeTile = Board.edge_pattern_regex;
		    string internalTile = Board.internal_pattern_regex;
		    this.searchMatch = "";
		    Tile matchTile = null;
		    // left
		    if ( col > 1 )
		    {
		        matchTile = this.getTileFromColRow(col - 1, row);
		        if ( matchTile != null )
		        {
		            match += matchTile.pattern[2];
		            this.searchMatch += matchTile.pattern[2];
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
		            match += matchTile.pattern[3];
		            this.searchMatch += matchTile.pattern[3];
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
		            match += matchTile.pattern[0];
		            this.searchMatch += matchTile.pattern[0];
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
		            match += matchTile.pattern[1];
		            this.searchMatch += matchTile.pattern[1];
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

		public bool isTileUsed(int tileId)
		{
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				if ( this.tilepos[i] == tileId )
				{
					return true;
				}
			}
			return false;
		}

		public int[] countMatchingTiles(int col, int row, string match)
		{
			// returns numUniqueFree & numUniqueUsed tiles that match the pattern
			System.Collections.ArrayList freeTiles = new ArrayList();
			System.Collections.ArrayList usedTiles = new ArrayList();

		    int[] rv = new int[2];
		    int i = 1;
		    int r = 0;
		    for ( i = 1; i <= this.tileset.Length; i++ )
		    {
		        for ( r = 1; r <= 4; r++ )
		        {
		        	string pattern = this.tileset[i-1].patterns[r-1];
		            Regex reg = new Regex("^" + match, RegexOptions.IgnoreCase);
		            if ( reg.IsMatch(pattern) )
		            {
		            	if ( this.isTileUsed(i) )
		            	{
		            		if ( !usedTiles.Contains(i) )
		            		{
			            		usedTiles.Add(i);
		            		}
		            	}
		                else
		                {
		                	if ( !freeTiles.Contains(i) )
		                	{
				            	freeTiles.Add(i);
		                	}
		                }
		            }
		            if ( this.fixedRotation )
		            {
		            	break;
		            }
		        }
		    }

		    // update matching tile list
		    int[] key = {col, row};
		    ArrayList arTiles = new ArrayList();
		    arTiles.Add(freeTiles);
		    arTiles.Add(usedTiles);
		    if ( !this.matchingTileList.ContainsKey(key) )
		    {
			    this.matchingTileList.Add(key, arTiles);
		    }

		    // return [numFree, numUsed]
		    rv[0] = freeTiles.Count;
		    rv[1] = usedTiles.Count;
		    return rv;
		}

		public void clearImageResults()
		{
			Program.TheMainForm.searchFreeResultsImages.Items.Clear();
    		Program.TheMainForm.searchFreeResultsImages.AutoSize = false;
    		Program.TheMainForm.searchFreeResultsImages.LargeImageList = new System.Windows.Forms.ImageList();
    		Program.TheMainForm.searchFreeResultsImages.LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			Program.TheMainForm.searchFreeResultsImages.LargeImageList.ImageSize = new Size(this.cellWidth, this.cellHeight);

			Program.TheMainForm.searchUsedResultsImages.Items.Clear();
    		Program.TheMainForm.searchUsedResultsImages.AutoSize = false;
    		Program.TheMainForm.searchUsedResultsImages.LargeImageList = new System.Windows.Forms.ImageList();
    		Program.TheMainForm.searchUsedResultsImages.LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			Program.TheMainForm.searchUsedResultsImages.LargeImageList.ImageSize = new Size(this.cellWidth, this.cellHeight);
		}

		public void searchTiles(string match)
		{
			Program.TheMainForm.timer.start("searchTiles(" + match + ")");
			Program.TheMainForm.searchFreeResultsImages.Items.Clear();
			Program.TheMainForm.searchUsedResultsImages.Items.Clear();
		    int i = 1;
		    int r = 0;

			// define match criteria
        	int numMatch = 0;
    		List<string> matches = new List<string>();
    		if ( Program.TheMainForm.rbSearchMatch2.Checked )
    		{
    			numMatch = 2;
    		}
    		else if ( Program.TheMainForm.rbSearchMatch3.Checked )
    		{
    			numMatch = 3;
    		}
    		else if ( Program.TheMainForm.rbSearchMatch4.Checked )
    		{
    			numMatch = 4;
    		}

    		// initialise image view
    		this.clearImageResults();

        	List<int> uniqueFreeTiles = new List<int>();
        	List<int> uniqueUsedTiles = new List<int>();
		    for ( i = 1; i <= this.tileset.Length; i++ )
		    {
		        for ( r = 1; r <= 4; r++ )
		        {
		        	string pattern = this.tileset[i-1].patterns[r-1];
		        	//Program.TheMainForm.log(pattern);

		        	bool found = false;
		        	int numFound = 0;
		        	if ( Program.TheMainForm.rbSearchRegex.Checked )
		        	{
		            	try
		            	{
				            Regex reg = new Regex("^" + match, RegexOptions.IgnoreCase);
				            if ( reg.IsMatch(pattern) )
				            {
				            	found = true;
				            }
		            	}
		            	catch
		            	{
				        }
		        	}
		        	else if ( Program.TheMainForm.rbSearchMatchAny.Checked )
		        	{
		            	try
		            	{
				            Regex reg = new Regex("[" + match + "]", RegexOptions.IgnoreCase);
				            if ( reg.IsMatch(pattern) )
				            {
				            	found = true;
				            }
				        }
				        catch
				        {
				        }
		        	}
//			       	if ( Program.TheMainForm.rbSearchMatch2.Checked || Program.TheMainForm.rbSearchMatch3.Checked || Program.TheMainForm.rbSearchMatch4.Checked || Program.TheMainForm.rbSearchMatchAll.Checked )
		        	else
		        	{
			        	if ( Program.TheMainForm.rbSearchMatchAll.Checked )
			        	{
				    		matches = new List<string>();
			        		foreach ( char ch in match.ToCharArray() )
			        		{
			        			if ( !matches.Contains(ch.ToString().ToUpper()) )
			        			{
			        				matches.Add(ch.ToString().ToUpper());
			        			}
			        		}
			        		numMatch = matches.Count;
			        	}

		        		numFound = 0;
			        	matches = new List<string>();
			            foreach ( char p in match.ToCharArray() )
			            {
			            	try
			            	{
					            Regex reg = new Regex(p.ToString().ToUpper(), RegexOptions.IgnoreCase);
					            if ( reg.IsMatch(pattern) )
				                {
				                	if ( !matches.Contains(p.ToString().ToUpper()) )
				                	{
				                		matches.Add(p.ToString().ToUpper());
					                    numFound++;
				                	}
				                }
			            	}
			            	catch
			            	{
			            	}
			            }
			            found = numFound == numMatch;
		        	}

		        	if ( found )
					{
		            	string title = "";
		            	// search all tiles, only return uniques in the free list...
	            		if ( Program.TheMainForm.cbSearchUniques.Checked )
	            		{
	            	    	title = String.Format("{0:000}", i) + " " + r + " " + pattern + " " + this.tileset[i-1].col + "," + this.tileset[i-1].row;

	            	    	if ( this.isTileUsed(i) )
	            	    	{
			            		if ( !uniqueUsedTiles.Contains(i) )
			            		{
		            	    		// used tiles
									TileGraphics gtile = new TileGraphics(pattern);
						    		Program.TheMainForm.searchUsedResultsImages.Items.Add(title, title);
						    		Program.TheMainForm.searchUsedResultsImages.LargeImageList.Images.Add(title, gtile.image);
			            			uniqueUsedTiles.Add(i);
			            		}
	            	    	}
	            	    	else
	            	    	{
	            	    		// free tiles
		            	    	if ( !uniqueFreeTiles.Contains(i) )
			            		{
			            			// update graphic image results
									TileGraphics gtile = new TileGraphics(pattern);
						    		Program.TheMainForm.searchFreeResultsImages.Items.Add(title, title);
						    		Program.TheMainForm.searchFreeResultsImages.LargeImageList.Images.Add(title, gtile.image);
			            			uniqueFreeTiles.Add(i);
			            		}
	            	    	}
	            		}

		            	// used tiles
		            	else if ( this.isTileUsed(i) )
	            	    {
	            	    	title = String.Format("{0:000}", i) + " " + r + " " + pattern + " " + this.tileset[i-1].col + "," + this.tileset[i-1].row;
	            			// update graphic image results
							TileGraphics gtile = new TileGraphics(pattern);
				    		Program.TheMainForm.searchUsedResultsImages.Items.Add(title, title);
				    		Program.TheMainForm.searchUsedResultsImages.LargeImageList.Images.Add(title, gtile.image);

		            		if ( !uniqueUsedTiles.Contains(i) )
		            		{
		            			uniqueUsedTiles.Add(i);
		            		}
	            	    }
		            	else
		            	{
		            		// free tiles
	            	    	title = String.Format("{0:000}", i) + " " + r + " " + pattern + " ";
	            			// update graphic image results
							TileGraphics gtile = new TileGraphics(pattern);
				    		Program.TheMainForm.searchFreeResultsImages.Items.Add(title, title);
				    		Program.TheMainForm.searchFreeResultsImages.LargeImageList.Images.Add(title, gtile.image);
		            		if ( !uniqueFreeTiles.Contains(i) )
		            		{
		            			uniqueFreeTiles.Add(i);
		            		}
		            	}
		            }
		        }
		        if ( this.fixedRotation )
		        {
		        	break;
		        }
		    }

		    // dump search results into stats tab
		    if ( Program.TheMainForm.rbDumpTiles.Checked )
		    {
		    	this.dumpSearchResults(match);
		    }

		    Program.TheMainForm.tabResultsFree.Text = "Free (" + Program.TheMainForm.searchFreeResultsImages.Items.Count + ")";
		    Program.TheMainForm.tabResultsUsed.Text = "Used (" + Program.TheMainForm.searchUsedResultsImages.Items.Count + ")";
		    Program.TheMainForm.labelNumSearchResultsFree.Text = Program.TheMainForm.searchFreeResultsImages.Items.Count + " free, " + uniqueFreeTiles.Count + " unique";
		    Program.TheMainForm.labelNumSearchResultsUsed.Text = Program.TheMainForm.searchUsedResultsImages.Items.Count + " used, " + uniqueUsedTiles.Count + " unique";
		    Program.TheMainForm.Update();
			Program.TheMainForm.timer.stop("searchTiles(" + match + ")");
			Program.TheMainForm.log(Program.TheMainForm.timer.results("searchTiles(" + match + ")"));
		}

		public void dumpSearchResults(string search)
		{
			Program.TheMainForm.tbSLog1.Clear();
			Program.TheMainForm.tbSLog1.Text += "// Search Results : " + search + " = " + Program.TheMainForm.searchFreeResultsImages.Items.Count + " Free Tiles" + "\r\n";
			foreach ( System.Windows.Forms.ListViewItem tileinfo in Program.TheMainForm.searchFreeResultsImages.Items )
			{
				// extract tileId & rotation
				string[] parts = tileinfo.Text.Split(' ');
				// tileId,rotation,pattern,col,row
				int tileId = System.Convert.ToInt16(parts[0]);
				int rotation = System.Convert.ToInt16(parts[1]);
				string pattern = parts[2];
				Program.TheMainForm.tbSLog1.Text += pattern + "\r\n";
			}
			Program.TheMainForm.tbSLog1.Text += "// Search Results : " + search + " = " + Program.TheMainForm.searchUsedResultsImages.Items.Count + " Used Tiles" + "\r\n";
			foreach (  System.Windows.Forms.ListViewItem tileinfo in Program.TheMainForm.searchUsedResultsImages.Items )
			{
				// extract tileId & rotation
				string[] parts = tileinfo.Text.Split(' ');
				// tileId,rotation,pattern,col,row
				int tileId = System.Convert.ToInt16(parts[0]);
				int rotation = System.Convert.ToInt16(parts[1]);
				string pattern = parts[2];
				Program.TheMainForm.tbSLog1.Text += pattern + "\r\n";
			}

		}

		public int countSurroundingTiles(int col, int row)
		{
			int numSurrounding = 0;

			// check left
			if ( col > 1 )
			{
				int leftpos = (row - 1) * Board.num_cols + col - 1;
				numSurrounding += Convert.ToInt16(this.tilepos[leftpos-1] > 0);
			}

			// check top
			if ( row > 1 )
			{
				int toppos = (row - 1 - 1) * Board.num_cols + col;
				numSurrounding += Convert.ToInt16(this.tilepos[toppos-1] > 0);
			}

			// check right
			if ( col < Board.num_cols )
			{
				int rightpos = (row - 1) * Board.num_cols + col + 1;
				numSurrounding += Convert.ToInt16(this.tilepos[rightpos-1] > 0);
			}

			// check bottom
			if ( row < Board.num_rows )
			{
				int bottompos = (row - 1 + 1) * Board.num_cols + col;
				numSurrounding += Convert.ToInt16(this.tilepos[bottompos-1] > 0);
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

		public bool isSwappable(int col, int row, int tileId)
		{
	    	// iterate through matchingTileList and see if exists in any other cell where numFree = 1 or numUsed = 1
			int[] key = {col, row};
			foreach ( int[] checkKey in this.matchingTileList.Keys )
			{
				if ( key[0] != checkKey[0] && key[1] != checkKey[1] )
				{
					ArrayList tiles = (ArrayList)this.matchingTileList[checkKey];
					ArrayList freeTiles = (ArrayList)tiles[0];
					ArrayList usedTiles = (ArrayList)tiles[1];
					if ( freeTiles.Count == 1 && freeTiles.Contains(tileId) )
					{
						//Program.TheMainForm.log("isSwappable(" + col + "," + row + ",#" + tileId + ") - FREE");
						return true;
					}
					else if ( usedTiles.Count == 1 && usedTiles.Contains(tileId) )
					{
						//Program.TheMainForm.log("isSwappable(" + col + "," + row + ",#" + tileId + ") - USED");
						return true;
					}
				}
			}
			return false;
		}

		public void updateTileStats()
		{
			Program.TheMainForm.dspNumFillableCells.Text = "0";
			Program.TheMainForm.dspNumFillableTiles.Text = "0";
			Program.TheMainForm.dspNumSwappableCells.Text = "0";
			Program.TheMainForm.dspNumSwappableTiles.Text = "0";
			Program.TheMainForm.dspNumScarceCells.Text = "0";
			Program.TheMainForm.dspNumScarceTiles.Text = "0";
			Program.TheMainForm.dspNumInvalidCells.Text = "0";
			int numFillableCells = 0;
			int numFillableTiles = 0;
			int numSwappableCells = 0;
			int numSwappableTiles = 0;
			int numScarceCells = 0;
			int numScarceTiles = 0;
			int numInvalidCells = 0;
			Color colour;
			this.swappableTileList = new ArrayList();

			// iterate through this.matchingTileList
			foreach ( int[] key in this.matchingTileList.Keys )
			{
				ArrayList tiles = (ArrayList)this.matchingTileList[key];
				ArrayList freeTiles = (ArrayList)tiles[0];
				ArrayList usedTiles = (ArrayList)tiles[1];
				int numFree = freeTiles.Count;
				int numUsed = usedTiles.Count;

				// fillable
				if ( numFree > 1 )
				{
					numFillableCells += 1;
					numFillableTiles += numFree;
					colour = System.Drawing.Color.LightGreen;
				}
				else if ( numFree == 1 && !this.isSwappable(key[0], key[1], (int)freeTiles[0]) )
				{
					//Program.TheMainForm.log("fillable: " + freeTiles[0]);
					numFillableCells += 1;
					numFillableTiles += numFree;
					colour = System.Drawing.Color.LightGreen;
				}
				// swappable
				else if ( numFree == 0 && numUsed > 1 )
				{
					colour = System.Drawing.Color.Yellow;
					numSwappableCells += 1;
					numSwappableTiles += numUsed;
					this.swappableTileList.AddRange(usedTiles);
				}
				else if ( numFree == 0 && numUsed == 1 && !this.isSwappable(key[0], key[1], (int)usedTiles[0]) )
				{
					colour = System.Drawing.Color.Yellow;
					numSwappableCells += 1;
					numSwappableTiles += numUsed;
					this.swappableTileList.AddRange(usedTiles);
				}
				// scarcely swappable
				else if ( numFree == 1 && this.isSwappable(key[0], key[1], (int)freeTiles[0]) )
				{
					Program.TheMainForm.log("scarcely swappable: " + freeTiles[0]);
					colour = System.Drawing.Color.Orange;
					numScarceCells += 1;
					numScarceTiles += 1;
					this.swappableTileList.AddRange(freeTiles);
				}
				else if ( numUsed == 1 && this.isSwappable(key[0], key[1], (int)usedTiles[0]) )
				{
					colour = System.Drawing.Color.Orange;
					numScarceCells += 1;
					numScarceTiles += 1;
					this.swappableTileList.AddRange(usedTiles);
				}
				// blocked
				else if ( numFree + numUsed == 0 )
				{
					colour = System.Drawing.Color.Red;
					numInvalidCells += 1;
				}
				else
				{
					// pink - if we get here we've missed something :)
					colour = System.Drawing.Color.Pink;
				}

				// create cell overlay
				this.createCellOverlay(key[0], key[1], numFree + "\n" + numUsed, colour);
			}

			// update listSwappableTiles - DISABLED
			/*
			this.swappableTileList.Sort();
			Program.TheMainForm.labelSwappableTiles.Text = this.swappableTileList.Count + " Tiles";
			foreach ( int tileId in this.swappableTileList )
			{
				if ( !Program.TheMainForm.listSwappableTiles.Items.Contains(tileId) )
				{
					Program.TheMainForm.listSwappableTiles.Items.Add(tileId);
				}
			}
			*/

			Program.TheMainForm.dspNumFillableCells.Text = numFillableCells.ToString();
			Program.TheMainForm.dspNumFillableTiles.Text = numFillableTiles.ToString();
			Program.TheMainForm.dspNumSwappableCells.Text = numSwappableCells.ToString();
			Program.TheMainForm.dspNumSwappableTiles.Text = numSwappableTiles.ToString();
			Program.TheMainForm.dspNumScarceCells.Text = numScarceCells.ToString();
			Program.TheMainForm.dspNumScarceTiles.Text = numScarceTiles.ToString();
			Program.TheMainForm.dspNumInvalidCells.Text = numInvalidCells.ToString();
		}

		public void highlightSwappableTile(int tileId)
		{
			this.clearSwappableOverlays();
			if ( tileId > 0 )
			{
				int col = this.tileset[tileId-1].col;
				int row = this.tileset[tileId-1].row;
				this.createCellOverlay(col, row, tileId.ToString(), Color.LightBlue);
			}
		}

		public void highlightCell(int cellId, Color colour)
		{
			if ( cellId > 0 )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				int col = colrow[0];
				int row = colrow[1];
				this.createCellOverlay(col, row, cellId.ToString(), colour);
			}
		}

		public void countIntersectingTiles()
		{
			this.clearOverlays();
			this.matchingTileList = new Hashtable();
// DISABLED
//			Program.TheMainForm.listSwappableTiles.Items.Clear();
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				// search for blank tiles and count intersecting tiles where applicable
				if ( this.tilepos[i] == 0 )
				{
					int[] colrow = Board.getColRowFromPos(i+1);
					string match = this.getTileMatchString(colrow[0], colrow[1]);
					if ( this.countSurroundingTiles(colrow[0], colrow[1]) > 1 || (this.countSurroundingTiles(colrow[0], colrow[1]) > 0 && this.isEdgePos(colrow[0], colrow[1])) )
					{
						int[] numTiles = this.countMatchingTiles(colrow[0], colrow[1], match);
						//Program.TheMainForm.log("countIntersectingTiles(" + colrow[0] + "," + colrow[1] + "), numFree = " + numTiles[0] + ", numUsed = " + numTiles[1]);
					}
					else
					{
						//Program.TheMainForm.log("countIntersectingTiles(" + colrow[0] + "," + colrow[1] + ") didn't match ? " + match);
					}
				}
			}
			this.updateTileStats();
		}

		public void createCellOverlay(int col, int row, string text, Color bgColour)
		{
			System.Windows.Forms.Label overlay = new TileOverlay();
			Program.TheMainForm.tabBoard.Controls.Add(overlay);
			//overlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			overlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			overlay.ForeColor = System.Drawing.Color.Black;
			int xoffset = Program.TheMainForm.pb_board.Left;
			int yoffset = Program.TheMainForm.pb_board.Top;
			Point cellPos = this.getXYFromColRow(col, row);
			overlay.Location = new Point(cellPos.X + xoffset + 1, cellPos.Y + yoffset + 1);
			overlay.Size = new System.Drawing.Size(this.cellWidth, this.cellHeight);
			overlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			overlay.Text = text;
			overlay.BackColor = bgColour;
			overlay.BringToFront();
			overlay.Show();

			// click to hide cell overlay
			overlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.overlayCellRemove);

			// swappable overlays are light blue - handle them differently
			if ( bgColour.Equals(Color.LightBlue) )
			{
				this.swappableOverlays.Add(overlay);
			}
			else
			{
				overlay.Name = col + "," + row;
//				overlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.overlayCellClicked);
//				overlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.overlayCellReleased);
				if ( !bgColour.Equals(Color.Red) )
				{
					overlay.MouseHover += new System.EventHandler(this.overlayCellClicked);
					overlay.MouseLeave += new System.EventHandler(this.overlayCellReleased);
				}
				this.overlays.Add(overlay);
			}
		}

//		public void overlayCellClicked(object sender, System.Windows.Forms.MouseEventArgs e)
		public void overlayCellClicked(object sender, System.EventArgs e)
		{
			System.Windows.Forms.Label label = (System.Windows.Forms.Label)sender;
			string[] cellPos = label.Name.Split(',');
			this.clearSwappableOverlays();

			// highlight all used tells that match the selected cell
			foreach ( int[] key in this.matchingTileList.Keys )
			{
				if ( key[0] == Convert.ToInt16(cellPos[0]) && key[1] == Convert.ToInt16(cellPos[1]) )
				{
					ArrayList tiles = (ArrayList)this.matchingTileList[key];
					ArrayList usedTiles = (ArrayList)tiles[1];
					foreach ( int tileId in usedTiles )
					{
						int col = this.tileset[tileId-1].col;
						int row = this.tileset[tileId-1].row;
						this.createCellOverlay(col, row, tileId.ToString(), Color.LightBlue);
					}
				}
			}
		}

//		public void overlayCellReleased(object sender, System.Windows.Forms.MouseEventArgs e)
		public void overlayCellReleased(object sender, System.EventArgs e)
		{
			this.clearSwappableOverlays();
		}

		public void overlayCellRemove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.Label overlay = (System.Windows.Forms.Label)sender;
			overlay.Dispose();
		}

		public void clearOverlays()
		{
			this.clearSwappableOverlays();
			for ( int i = 0; i < this.overlays.Count; i++ )
			{
				System.Windows.Forms.Label overlay = (System.Windows.Forms.Label)this.overlays[i];
				overlay.Dispose();
			}
			this.overlays = new ArrayList();
		}

		public void clearSwappableOverlays()
		{
			for ( int i = 0; i < this.swappableOverlays.Count; i++ )
			{
				System.Windows.Forms.Label overlay = (System.Windows.Forms.Label)this.swappableOverlays[i];
				overlay.Dispose();
			}
			this.swappableOverlays = new ArrayList();
		}

		public void clearPatternStats()
		{
			this.log_stats = "";
			for ( int i = 0; i < this.patternStats.Count; i++ )
			{
				System.Windows.Forms.Label obj = (System.Windows.Forms.Label)this.patternStats[i];
				obj.Dispose();
			}
			Program.TheMainForm.panel1.Controls.Clear();
			Program.TheMainForm.panel1.Refresh();
			this.patternStats = new ArrayList();
		}

		public ArrayList calcPatternStats()
		{
			ArrayList stats = new ArrayList();
			foreach ( string letter in this.patterns )
			{
				int[] patternStat = {0,0,0,0};
				for ( int tileId = 0; tileId < this.tileset.Length; tileId++ )
				{
					if ( this.tileset[tileId] != null )
					{
						string pattern = this.tileset[tileId].pattern;
						for ( int p = 0; p < 4; p++ )
						{
							if ( pattern[p].ToString().ToUpper() == letter )
							{
								patternStat[p] += 1;
							}
						}
					}
				}
				stats.Add(patternStat);
			}
			return stats;
		}

		public void showPatternStats()
		{
			this.clearPatternStats();
			ArrayList stats = this.calcPatternStats();
			// xy : 10,10
			int xoffset = 10;
			int yoffset = 10;
			int x = 0;
			int y = 0;
			string text = "";
			string logtext = "";
			Color bgColour = Color.Transparent;

			// column headings
			// pattern id
			x = xoffset;
			y = yoffset;
			this.createStatBox("ID", false, "", x, y, bgColour);

			// pattern image
			x += 32 + 1;
			this.createStatBox("P", false, "", x, y, bgColour);

			// pattern stats - left
			x += 32 + 1;
			text = "L";
			this.createStatBox(text, true, "", x, y, bgColour);

			// pattern stats - top
			x += 32 + 1;
			text = "T";
			this.createStatBox(text, true, "", x, y, bgColour);

			// pattern stats - right
			x += 32 + 1;
			text = "R";
			this.createStatBox(text, true, "", x, y, bgColour);

			// pattern stats - bottom
			x += 32 + 1;
			text = "B";
			this.createStatBox(text, true, "", x, y, bgColour);

			// pattern stats - total
			x += 32 + 1;
			text = "#";
			this.createStatBox(text, true, "", x, y, bgColour);

			this.log_stats += "id,pattern,left,up,right,down,total" + "\r\n";

			string[] filenames = System.IO.Directory.GetFiles(CAF_Application.config.imagePath() + "\\patterns-flat", "*.png");
			int i = 0;
			foreach ( string filename in filenames )
			{
				i++;
				string pattern = System.IO.Path.GetFileNameWithoutExtension(filename);
				logtext = "";
				int[] pstats = (int[])stats[i-1];

				// check if balanced
				if ( pstats[0] != pstats[2] || pstats[1] != pstats[3] )
				{
					bgColour = Color.LightYellow;
				}
				else
				{
					bgColour = Color.Transparent;
				}

				// pattern id
				x = xoffset;
				y = yoffset + i * 32;
				this.createStatBox(pattern.ToUpper(), false, "", x, y, bgColour);

				// pattern image
				x += 32 + 1;
				this.createStatBox("", false, filename, x, y, bgColour);

				// pattern stats - left
				x += 32 + 1;
				text = pstats[0].ToString();
				this.createStatBox(text, true, "", x, y, bgColour);

				// pattern stats - top
				x += 32 + 1;
				text = pstats[1].ToString();
				this.createStatBox(text, true, "", x, y, bgColour);

				// pattern stats - right
				x += 32 + 1;
				text = pstats[2].ToString();
				this.createStatBox(text, true, "", x, y, bgColour);

				// pattern stats - bottom
				x += 32 + 1;
				text = pstats[3].ToString();
				this.createStatBox(text, true, "", x, y, bgColour);

				// pattern stats - total
				x += 32 + 1;
				int total = pstats[0] + pstats[1] + pstats[2] + pstats[3];
				text = total.ToString();
				this.createStatBox(text, true, "", x, y, bgColour);

				logtext = pattern.ToUpper() + "," + pstats[0].ToString() + "," + pstats[1].ToString() + "," + pstats[2].ToString() + "," + pstats[3].ToString() + "," + total.ToString();
				this.log_stats += logtext + "\r\n";
			}
			Program.TheMainForm.tbSLog1.Text = this.log_stats;
			Program.TheMainForm.tbSLog1.Update();
		}

		public void createStatBox(string text, bool is3D, string bgImage, int x, int y, Color bgColour)
		{
			System.Windows.Forms.Label label = new System.Windows.Forms.Label();
			label.Text = text;
			if ( is3D )
			{
				label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			}
			if ( bgImage != "" )
			{
				if ( System.IO.File.Exists(bgImage) )
				{
					//Program.TheMainForm.log("loading image: " + bgImage);
					label.Image = new Bitmap(bgImage);
					label.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
				}
			}
			label.Location = new Point(x, y);
			label.Size = new System.Drawing.Size(32, 32);
			label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label.ForeColor = System.Drawing.Color.Black;
			label.BackColor = bgColour;
			label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//			Program.TheMainForm.tabStats.Controls.Add(label);
			Program.TheMainForm.panel1.Controls.Add(label);
			label.BringToFront();
			label.Show();
		}

		public void updateStatistics()
		{
			// count unique tiles
			System.Collections.Generic.List<string> uniqueTiles = new System.Collections.Generic.List<string>();
			for ( int i = 0; i < this.tileset.Length; i++ )
			{
				for ( int r = 0; r < 4; r++ )
				{
					string pattern = this.tileset[i].patterns[r];
					if ( !uniqueTiles.Contains(pattern) )
					{
						uniqueTiles.Add(pattern);
					}
				}
			}
			Board.numUniqueTiles = uniqueTiles.Count;
			string logtext = "Tileset " + Board.title + " contains " + uniqueTiles.Count.ToString() + " unique tiles";
//			Program.TheMainForm.log(logtext);
			Program.TheMainForm.tbSLog1.Text += logtext + "\r\n";
		}

		public void createUniqueBoards()
		{
			string filename = "logs\\unique-boards.txt";
			Program.TheMainForm.timer.start("createUniqueBoards");
			Int32 seed = 0;
			Int32 numBoards = 1;
			try
			{
				seed = Convert.ToInt32(Program.TheMainForm.inputSeedStart.Text);
				numBoards = Convert.ToInt32(Program.TheMainForm.inputNumBoards.Text);
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("Please enter a valid seed and # of boards to generate.");
				return;
			}
			RandomBoard rb = new RandomBoard();

			List<string> logtext = new List<string>();
			logtext.Add("Generating " + numBoards.ToString() + " " + Board.getLayout() + " boards from seed " + seed.ToString());
			Program.TheMainForm.textLoadSaveLog.Text = String.Join("\r\n", logtext.ToArray());
			Program.TheMainForm.textLoadSaveLog.Update();

			Program.TheMainForm.initSolver(false);
			this.isCancel = false;
			for ( Int32 i = seed; i < seed + numBoards; i++ )
			{
				string board = rb.createNewTileset(i);
				if ( board == "" )
				{
					break;
				}
				string[] tiles = board.Split('\n');
				int numUnique = this.countNumUniqueTiles(tiles);
				string line = rb.result;

				/*
				bool isMagicBorder = Program.TheMainForm.solver.isMagicBorder(tiles);
				if ( isMagicBorder )
				{
					line += ", magic Border = TRUE";
				}
				*/

				// check if magic square
				/*
				if ( Program.TheMainForm.solver.isMagicSquare(tiles) )
				{
					line += ", isMagicSquare = YES";
					System.Media.SystemSounds.Beep.Play();
				}
				else
				{
					line += ", isMagicSquare = NO";
				}
				*/

				// check if sum(border pieces) is prime
				/*
				bool isPrime = true;
				int sumBorder = Program.TheMainForm.solver.calcSumBorderTiles(tiles);
				if ( Numerology.IsPrime(sumBorder) )
				{
					line += ", borderSum = " + sumBorder + " is PRIME";
				}
				else
				{
					isPrime = false;
				}
				// check if sum(internal pieces) is prime
				int sumInternal = Program.TheMainForm.solver.calcSumInternalTiles(tiles);
				if ( Numerology.IsPrime(sumInternal) )
				{
					line += ", internalSum = " + sumInternal + " is PRIME";
				}
				else
				{
					isPrime = false;
				}
				if ( isPrime )
				{
					System.Media.SystemSounds.Beep.Play();
				}
				*/

				bool isMagicBorder = false;

				if ( numUnique == Board.max_tiles * 4 && Program.TheMainForm.cbShowUniqueBoards.Checked )
				{
					if ( Program.TheMainForm.cbShowMagicBorders.Checked )
					{
						// check if inner square is magic square using bit values (only for unique boards)
						isMagicBorder = Program.TheMainForm.solver.isMagicInnerSquareBitValues(tiles);
						if ( isMagicBorder )
						{
							System.Media.SystemSounds.Beep.Play();
							line += ", isMagicInnerSquareBitValues = YES";
							logtext.Add(line);
							// log to file
							System.IO.File.AppendAllText(filename, line + "\r\n");
						}
					}
					else
					{
						logtext.Add(line);
						// log to file
						System.IO.File.AppendAllText(filename, line + "\r\n");
					}
				}
				else if ( Program.TheMainForm.cbShowNonUniqueBoards.Checked )
				{
					if ( Program.TheMainForm.cbShowMagicBorders.Checked )
					{
						// check if inner square is magic square using bit values (only for unique boards)
						isMagicBorder = Program.TheMainForm.solver.isMagicInnerSquareBitValues(tiles);
						if ( isMagicBorder )
						{
							System.Media.SystemSounds.Beep.Play();
							line += ", isMagicInnerSquareBitValues = YES";
							logtext.Add(line);
							// log to file
							System.IO.File.AppendAllText(filename, line + "\r\n");
						}
					}
					else
					{
						logtext.Add(line);
						// log to file
						System.IO.File.AppendAllText(filename, line + "\r\n");
					}
				}

				if ( i % 100 == 0 )
				{
					System.Windows.Forms.Application.DoEvents();
				}
				if ( this.isCancel )
				{
					logtext.Add("Cancelled.");
					break;
				}
			}
			this.isCancel = false;

			Program.TheMainForm.timer.stop("createUniqueBoards");
			logtext.Add(Program.TheMainForm.timer.results("createUniqueBoards"));
			Program.TheMainForm.textLoadSaveLog.Text = String.Join("\r\n", logtext.ToArray()) + "\r\n";
			Program.TheMainForm.textLoadSaveLog.Update();
		}

		public int countNumUniqueTiles(string[] tiles)
		{
			// rotates all patterns in a string of patterns and counts how many unique tiles there are
			System.Collections.Generic.List<string> uniqueTiles = new System.Collections.Generic.List<string>();
			for ( int i = 0; i < tiles.Length; i++ )
			{
				string pattern = tiles[i].Trim();
				if ( pattern.Length > 0 )
				{
			        for ( int r = 1; r <= 4; r++ )
			        {
			        	if ( r > 1 )
			        	{
				        	pattern = pattern.Substring(pattern.Length-1) + pattern.Substring(0, 3);
			        	}
						if ( !uniqueTiles.Contains(pattern) )
						{
							uniqueTiles.Add(pattern);
						}
			        }
				}
			}
			return uniqueTiles.Count;
		}

		public string rotateTileset(string[] tiles, int rotation)
		{
			// rotates all pieces in a tileset then extracts 1 of 4 rotations (where each piece is individually rotated to the same position)
			// each piece is rotated in alphanumeric order to extract r1-r4 based upon the alphanumeric value
			string rv = "";
			System.Collections.Generic.List<string> newtiles = new System.Collections.Generic.List<string>();
			System.Collections.Generic.List<string> patterns;
			for ( int i = 0; i < tiles.Length; i++ )
			{
				string pattern = tiles[i].Trim();
				if ( pattern.Length >= 4 )
				{
					patterns = new System.Collections.Generic.List<string>();
					patterns.Add(pattern);
			        for ( int r = 1; r <= 4; r++ )
			        {
			        	if ( r > 1 )
			        	{
				        	pattern = pattern.Substring(pattern.Length-1) + pattern.Substring(0, 3);
							patterns.Add(pattern);
			        	}
			        }
			        // sort rotations in alphanumeric order to get r1-r4
			        patterns.Sort(String.CompareOrdinal);
			        newtiles.Add(patterns[rotation-1]);
				}
			}
			// sort the tileset
			newtiles.Sort(String.CompareOrdinal);
			foreach ( string line in newtiles )
			{
		        rv += line + "\r\n";
			}
			return rv;
		}

		public void setBoardAsHint()
		{
			List<string> hints = new List<string>();
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				List<string> line = new List<string>();
				if ( this.tilepos[i] > 0 )
				{
					line.Add(this.tileset[this.tilepos[i]-1].col.ToString());
					line.Add(this.tileset[this.tilepos[i]-1].row.ToString());
					line.Add(this.tilepos[i].ToString());
					line.Add(this.tileset[this.tilepos[i]-1].pattern);
					hints.Add(String.Join(",", line.ToArray()));
				}
			}
			Program.TheMainForm.textHints.Text = String.Join("\r\n", hints.ToArray());
		}

		public Tile getTileByPattern(string pattern)
		{
			foreach ( Tile tile in this.tileset )
			{
				int r = 0;
				foreach ( string p in tile.patterns )
				{
					r++;
					if ( p.ToUpper() == pattern.ToUpper() )
					{
						if ( !this.fixedRotation )
						{
							tile.rotate(r);
						}
						return tile;
					}
				}
			}
			return null;
		}

		public void dumpTiles()
		{
			List<string> tiles = new List<string>();
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				if ( this.tilepos[i] > 0 )
				{
					tiles.Add(this.tileset[this.tilepos[i]-1].pattern);
				}
			}
			Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", tiles.ToArray());
		}

		public SortedDictionary<int, TileInfo> getBoardData()
		{
			SortedDictionary<int, TileInfo> board = new SortedDictionary<int, TileInfo>();
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				if ( this.tilepos[i] > 0 )
				{
					TileInfo tile = new TileInfo(this.tileset[this.tilepos[i]-1].id, this.tileset[this.tilepos[i]-1].pattern);
					board.Add(i+1, tile);
				}
			}
			return board;
		}

		public void shuffle()
		{
			Program.TheMainForm.initSolver(false);
			this.tileset = (Tile[])Program.TheMainForm.solver.randomiseList(this.tileset);
			string tiles = "";
			foreach ( Tile tile in this.tileset )
			{
				tiles += tile.pattern + "\r\n";
			}
			Program.TheMainForm.textTileset.Text = tiles;
			Program.TheMainForm.textTileset.Update();
			Program.TheMainForm.createSolutionModel();
			this.setModel();
		}

		/*
		public static string getPatternRegex()
		{
			int minPID = Convert.ToInt16('A');
			char b = Convert.ToChar(minPID + Board.num_edges + Board.num_inner1 + Board.num_inner2 + Board.num_inner3);
			return "[A-" + b + "]";
		}
		*/

		/*
		public static string getEdgePatternRegex()
		{
			int minPID = Convert.ToInt16('A');
			char b = Convert.ToChar(minPID + Board.num_edges - 1);
			return "[A-" + b + "]";
		}

		public static string getInternalPatternRegex()
		{
			int minPID = Convert.ToInt16('A');
			char a = Convert.ToChar(minPID + Board.num_edges);
			char b = Convert.ToChar(minPID + Board.num_edges + Board.num_inner1 + Board.num_inner2 + Board.num_inner3 - 1);
			return "[" + a + "-" + b + "]";
		}
		*/

		public static string[] getEdgePatternList()
		{
			/*
			string[] plist = new string[Board.num_edges];
			int minPID = Convert.ToInt16('A');
			for ( int i = 0; i < Board.num_edges; i++ )
			{
				char a = Convert.ToChar(minPID + i);
				plist[i] = a.ToString();
			}
			return plist;
			*/

			List<string> plist = new List<string>();
			foreach ( char ch in Board.edge_pattern_regex )
			{
				if ( ch != '[' && ch != ']' )
				{
					plist.Add(ch.ToString());
				}
			}
			return plist.ToArray();
		}

		public static string[] getInnerPatternList()
		{
			/*
			string[] plist = new string[Board.num_inner1 + Board.num_inner2 + Board.num_inner3];
			int minPID = Convert.ToInt16('A');
			for ( int i = 0; i < Board.num_inner1 + Board.num_inner2 + Board.num_inner3; i++ )
			{
				char a = Convert.ToChar(minPID + Board.num_edges + i);
				plist[i] = a.ToString();
			}
			return plist;
			*/

			List<string> plist = new List<string>();
			foreach ( char ch in Board.internal_pattern_regex )
			{
				if ( ch != '[' && ch != ']' )
				{
					plist.Add(ch.ToString());
				}
			}
			return plist.ToArray();
		}

		public static void setTitle()
		{
			Board.title = Board.num_cols + "x" + Board.num_rows + "x" + Board.num_edges + "x" + (Board.num_inner1 + Board.num_inner2 + Board.num_inner3) + "_" + Board.seed;
		}

		public static string getLayout()
		{
			return Board.num_cols + "x" + Board.num_rows + "x" + Board.num_edges + "x" + (Board.num_inner1 + Board.num_inner2 + Board.num_inner3);
		}

		public void removeSelectedTiles()
		{
			foreach ( int cellId in Program.TheMainForm.solver.pb.path )
			{
				int[] colrow = Board.getColRowFromPos(cellId);
				this.removeTile(colrow[0], colrow[1]);
			}
		}

		public void dumpAsTileset()
		{
			List<string> tiles = new List<string>();
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				if ( this.tilepos[i] > 0 )
				{
					tiles.Add(this.tileset[this.tilepos[i]-1].pattern);
				}
				else
				{
					tiles.Add("");
				}
			}
			Program.TheMainForm.tbBoardLog.Text = String.Join("\r\n", tiles.ToArray());
		}

		public void compareBoardToTileset()
		{
			// compares tileset (solution) with current board/model
			// outputs matches/differences
			// Cell,Solution,Board,IsMatch
			// 1,--AC,--BD,no
			// count total number of matches
			List<string> results = new List<string>();
			int numMatches = 0;
			results.Add("Cell,Solution,Board,IsMatch");
			for ( int i = 0; i < this.tilepos.Length; i++ )
			{
				string isMatch = "no";
				string originalTile = this.originalTileset[i];
				string currentTile = "";
				if ( this.tilepos[i] > 0 )
				{
					currentTile = this.tileset[this.tilepos[i]-1].pattern;
				}
				if ( originalTile != "" && originalTile == currentTile )
				{
					isMatch = "yes";
					numMatches++;
				}
				results.Add(String.Format("{0},{1},{2},{3}", i+1, originalTile, currentTile, isMatch));
			}
			results.Add(numMatches + " / " + this.originalTileset.Count + " matches found");
			Program.TheMainForm.tbBoardLog.Text = String.Join("\r\n", results.ToArray());
		}

		// search all tiles all rotations for regex string and return results
		public List<string> getTileSearchResults(string search, bool onlyUnique)
		{
			List<string> tilelist = new List<string>();
		    for ( int i = 1; i <= this.tileset.Length; i++ )
		    {
		        for ( int r = 1; r <= 4; r++ )
		        {
		        	string pattern = this.tileset[i-1].patterns[r-1];
		        	if ( Regex.IsMatch(pattern, search, RegexOptions.IgnoreCase) )
		            {
		            	tilelist.Add(pattern);
			            if ( onlyUnique )
			            {
			            	// only get 1st rotation that matches
			            	break;
			            }
		            }
		        }
			}
			return tilelist;
		}

		public void showIndividualPatternStats()
		{
			List<string> stats = new List<string>();
			stats.Add("pattern,numSingle,numAdj,numOpp,numTriple");
			foreach ( string letter in this.patterns )
			{
				int[] patternStat = {0,0,0,0};
				// search for internal tiles for this pattern left aligned, then count numSingle,numAdjacent,numOpposite
//				List<string> tilelist = this.getTileSearchResults(String.Format("^{0}[^-]{{3}}", letter), true);
				List<string> tilelist = this.getTileSearchResults(String.Format("^{0}", letter), true);
				foreach ( string tile in tilelist )
				{
					// numSingle
					if ( Regex.Matches(tile, letter).Count == 1 )
					{
						patternStat[0]++;
					}
					// numAdj - LT, TR, RB, LB
					if ( Regex.IsMatch(tile, String.Format("^[{0}][{0}]|^.[{0}][{0}]|^..[{0}][{0}]|^[{0}]..[{0}]", letter)) )
					{
						patternStat[1]++;
					}
					// numOpp - LR, TB
					if ( Regex.IsMatch(tile, String.Format("^[{0}].[{0}]|^.[{0}].[{0}]", letter)) )
					{
						patternStat[2]++;
					}
					// numTriple
					if ( Regex.Matches(tile, letter).Count == 3 )
					{
						patternStat[3]++;
					}
				}
				string line = letter + "," + String.Join(",", et2.Utils.IntListToStringList(patternStat));
				stats.Add(line);
			}
			Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.tbSLog1.Update();
		}

		public static Dictionary<string, string> getBGColourList(string type)
		{
			Dictionary<string, string> rlist = new Dictionary<string, string>();
			switch ( type )
			{
				case "all":
					// bgcolours from all  tiles
					rlist.Add("Blue","BSTV");
				    rlist.Add("Green","DJL");
				    rlist.Add("Lightblue","NP");
				    rlist.Add("Orange","AR");
				    rlist.Add("Pink","CFU");
				    rlist.Add("Purple","GI");
				    rlist.Add("Red","EKM");
				    rlist.Add("Yellow","HOQ");
					break;
				case "internal":
			    	// bgcolours from internal tiles
			    	rlist.Add("Blue","STV");
				    rlist.Add("Green","JL");
				    rlist.Add("Lightblue","NP");
				    rlist.Add("Orange","R");
				    rlist.Add("Pink","FU");
				    rlist.Add("Purple","GI");
				    rlist.Add("Red","KM");
				    rlist.Add("Yellow","HOQ");
					break;
			}

			return rlist;
		}

		public static string getPatternBGColour(char pattern)
		{
			Dictionary<string, string> bgColourList = Board.getBGColourList("all");
			foreach ( string colour in bgColourList.Keys )
			{
				if ( bgColourList[colour].Contains(pattern.ToString()) )
				{
					return colour;
				}
			}
			return "";
		}

		public static string getPatternShape(char pattern)
		{
			Dictionary<string, string> shapeList = Board.getShapeList();
			foreach ( string shape in shapeList.Keys )
			{
				if ( shapeList[shape].Contains(pattern.ToString()) )
				{
					return shape;
				}
			}
			return "";
		}

		public static Dictionary<string, string> getShapeList()
		{
			Dictionary<string, string> rlist = new Dictionary<string, string>();
	    	rlist.Add("Castle","NQU");
	    	rlist.Add("CrossInCircle","IKV");
	    	rlist.Add("PointedCross","GLP");
	    	rlist.Add("RoundCross","FJS");
	    	rlist.Add("SquareRing","OT");
	    	rlist.Add("Star","HMR");
			return rlist;
		}

		public void showTileAnalysis()
		{
			// analyse internal tiles and summarise how the bgcolours,shapes,patterns are matched
			// eg. count how many are quad (all same colour), triple (3 same), 2 adj, 2 opp, 1 adj, 1 opp, 4 unique colours etc
			List<string> output = new List<string>();
			List<string> tilelist = this.getTileSearchResults("^[^-]{4}", true);
			output.Add("scanning " + tilelist.Count + " tile(s)");

			// by bgcolour
			Dictionary<string, string> bgcolours = Board.getBGColourList("internal");
			Dictionary<string, int> stats = new Dictionary<string, int>();
			output.Add("by bgcolour,count");
			stats.Add("quad",0);
			stats.Add("triple",0);
			stats.Add("2 adj",0);
			stats.Add("2 opp",0);
			stats.Add("1 adj",0);
			stats.Add("1 opp",0);
			stats.Add("4 unique",0);

			foreach ( string tile in tilelist )
			{
				int numAdj = 0;
				int numOpp = 0;
				int numInd = 0;
				foreach ( string colour in bgcolours.Keys )
				{
					string patterns = bgcolours[colour];
					// quad
					if ( Regex.Matches(tile, String.Format("[{0}]", patterns)).Count == 4 )
					{
						stats["quad"]++;
						break;
					}

					// triple
					else if ( Regex.Matches(tile, String.Format("[{0}]", patterns)).Count == 3 )
					{
						stats["triple"]++;
						break;
					}

					// count adjacents
					numAdj += Regex.Matches(tile, String.Format("[{0}][{0}]|^[{0}]..[{0}]", patterns)).Count;

					// count opposites
					if ( Regex.IsMatch(tile, String.Format("^[{0}].[{0}]", patterns)) )
					{
						numOpp++;
					}
					if ( Regex.IsMatch(tile, String.Format("^.[{0}].[{0}]", patterns)) )
					{
						numOpp++;
					}

					// count individual colours/patterns/shapes
					if ( Regex.IsMatch(tile, String.Format("[{0}]", patterns)) )
					{
						numInd++;
					}
				}
				if ( numAdj == 2 )
				{
					stats["2 adj"]++;
				}
				if ( numOpp == 2 )
				{
					stats["2 opp"]++;
				}
				if ( numAdj == 1 )
				{
					stats["1 adj"]++;
				}
				if ( numOpp == 1 )
				{
					stats["1 opp"]++;
				}
				if ( numInd == 4 )
				{
					stats["4 unique"]++;
				}
			}
			foreach ( string key in stats.Keys )
			{
				output.Add(String.Concat(key,",",stats[key]));
			}
			output.Add("total," + new List<int>(stats.Values).Sum());
			output.Add("");

			// by shape
			Dictionary<string, string> styles = Board.getShapeList();
			stats.Clear();
			output.Add("by shape,count");
			stats.Add("quad",0);
			stats.Add("triple",0);
			stats.Add("2 adj",0);
			stats.Add("2 opp",0);
			stats.Add("1 adj",0);
			stats.Add("1 opp",0);
			stats.Add("4 unique",0);

			foreach ( string tile in tilelist )
			{
				int numAdj = 0;
				int numOpp = 0;
				int numInd = 0;
				foreach ( string shape in styles.Keys )
				{
					string patterns = styles[shape];
					// quad
					if ( Regex.Matches(tile, String.Format("[{0}]", patterns)).Count == 4 )
					{
						stats["quad"]++;
						break;
					}

					// triple
					else if ( Regex.Matches(tile, String.Format("[{0}]", patterns)).Count == 3 )
					{
						stats["triple"]++;
						break;
					}

					// count adjacents
					numAdj += Regex.Matches(tile, String.Format("[{0}][{0}]|^[{0}]..[{0}]", patterns)).Count;

					// count opposites
					if ( Regex.IsMatch(tile, String.Format("^[{0}].[{0}]", patterns)) )
					{
						numOpp++;
					}
					if ( Regex.IsMatch(tile, String.Format("^.[{0}].[{0}]", patterns)) )
					{
						numOpp++;
					}

					// count individual colours/patterns/shapes
					if ( Regex.IsMatch(tile, String.Format("[{0}]", patterns)) )
					{
						numInd++;
					}
				}
				if ( numAdj == 2 )
				{
					stats["2 adj"]++;
				}
				if ( numOpp == 2 )
				{
					stats["2 opp"]++;
				}
				if ( numAdj == 1 )
				{
					stats["1 adj"]++;
				}
				if ( numOpp == 1 )
				{
					stats["1 opp"]++;
				}
				if ( numInd == 4 )
				{
					stats["4 unique"]++;
				}
			}
			foreach ( string key in stats.Keys )
			{
				output.Add(String.Concat(key,",",stats[key]));
			}
			output.Add("total," + new List<int>(stats.Values).Sum());
			output.Add("");

			// by pattern A-Z
			stats.Clear();
			output.Add("by pattern,count");
			stats.Add("quad",0);
			stats.Add("triple",0);
			stats.Add("2 adj",0);
			stats.Add("2 opp",0);
			stats.Add("1 adj",0);
			stats.Add("1 opp",0);
			stats.Add("4 unique",0);

			foreach ( string tile in tilelist )
			{
				int numAdj = 0;
				int numOpp = 0;
				int numInd = 0;
				for ( int i = 1; i <= 26; i++ )
				{
					string pattern = Char.ConvertFromUtf32(64 + i).ToUpper().ToString();
					// quad
					if ( Regex.Matches(tile, String.Format("[{0}]", pattern)).Count == 4 )
					{
						stats["quad"]++;
						break;
					}

					// triple
					else if ( Regex.Matches(tile, String.Format("[{0}]", pattern)).Count == 3 )
					{
						stats["triple"]++;
						break;
					}

					// count adjacents
					numAdj += Regex.Matches(tile, String.Format("[{0}][{0}]|^[{0}]..[{0}]", pattern)).Count;

					// count opposites
					if ( Regex.IsMatch(tile, String.Format("^[{0}].[{0}]", pattern)) )
					{
						numOpp++;
					}
					if ( Regex.IsMatch(tile, String.Format("^.[{0}].[{0}]", pattern)) )
					{
						numOpp++;
					}

					// count individual colours/patterns/shapes
					if ( Regex.IsMatch(tile, String.Format("[{0}]", pattern)) )
					{
						numInd++;
					}
				}
				if ( numAdj == 2 )
				{
					stats["2 adj"]++;
				}
				if ( numOpp == 2 )
				{
					stats["2 opp"]++;
				}
				if ( numAdj == 1 )
				{
					stats["1 adj"]++;
				}
				if ( numOpp == 1 )
				{
					stats["1 opp"]++;
				}
				if ( numInd == 4 )
				{
					stats["4 unique"]++;
				}
			}
			foreach ( string key in stats.Keys )
			{
				output.Add(String.Concat(key,",",stats[key]));
			}
			output.Add("total," + new List<int>(stats.Values).Sum());
			output.Add("");

			Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", output.ToArray());
			Program.TheMainForm.tbSLog1.Update();
		}

		public void showBGColourStats(string type)
		{
			List<string> stats = new List<string>();
			HashSet<string> uniquePairs = new HashSet<string>();
			bool isSame = false;

			stats.Add("bgcolour,patterns,numTiles,numSquares,numBorder,numSingle,numAdj,numOpp,numTriple");
			Dictionary<string, string> bgcolours = Board.getBGColourList(type);
			Dictionary<string, string> internal_bgcolours = Board.getBGColourList("internal");
			foreach ( string colour in bgcolours.Keys )
			{
				string patterns = bgcolours[colour];
				string ipatterns = internal_bgcolours[colour];
				int[] bgstat = {0,0,0,0};
				// count number of borders
				List<string> btilelist = this.getTileSearchResults(String.Format("^-.[{0}]", ipatterns), true);
				int numBorder = btilelist.Count;
				List<string> tilelist;
				if ( type == "internal" )
				{
					tilelist = this.getTileSearchResults(String.Format("^[{0}][^-]{{3}}", patterns), true);
				}
				else
				{
					tilelist = this.getTileSearchResults(String.Format("^[{0}]", patterns), true);
				}
				int numSquares = 0;
				foreach ( string tile in tilelist )
				{
					numSquares += Regex.Matches(tile, "[" + patterns + "]").Count;

					// numSingle
					if ( Regex.Matches(tile, "[" + patterns + "]").Count == 1 )
					{
						bgstat[0]++;
					}
					// numAdj - LT, TR, RB, LB
					if ( Regex.IsMatch(tile, String.Format("^[{0}][{0}]|^.[{0}][{0}]|^..[{0}][{0}]|^[{0}]..[{0}]", patterns)) )
					{
						bgstat[1]++;
					}
					// numOpp - LR, TB
					if ( Regex.IsMatch(tile, String.Format("^[{0}].[{0}]|^.[{0}].[{0}]", patterns)) )
					{
						bgstat[2]++;
					}
					// numTriple
					if ( Regex.Matches(tile, "[" + patterns + "]").Count == 3 )
					{
						bgstat[3]++;
					}
				}
				if ( type == "internal" )
				{
					numSquares = (numSquares + numBorder) / 2;
				}
				else
				{
					numSquares = numSquares / 2;
				}
				string line = colour + "," + patterns + "," + tilelist.Count + "," + numSquares + "," + numBorder + "," + String.Join(",", et2.Utils.IntListToStringList(bgstat));
				stats.Add(line);
			}
			stats.Add("");

			// count colour associations
			uniquePairs.Clear();
			stats.Add("bgcolourA,bgcolourB,numTiles,numSquaresA,numSquaresB,numSingleB,numAdjB,numOppB,numTripleB,isMatch");
			int numMatches = 0; // number of colours that share same number of squares on interior tiles
			foreach ( string colourA in bgcolours.Keys )
			{
				foreach ( string colourB in bgcolours.Keys )
				{
					if ( colourA != colourB )
					{
						string patternsA = bgcolours[colourA];
						string patternsB = bgcolours[colourB];

						// exclude duplicate pairs, eg. keep Blue,Orange but discard Orange,Blue
						if ( !uniquePairs.Contains(patternsA + patternsB) && !uniquePairs.Contains(patternsB + patternsA) )
						{
							uniquePairs.Add(patternsA + patternsB);

							List<string> tilelist = this.getTileSearchResults(String.Format("^[{0}].*[{1}]|[{1}].*[{0}]", patternsA, patternsB), true);
							Dictionary<string, int> statline = new Dictionary<string, int>();
							statline.Add("numTiles", tilelist.Count);
							statline.Add("numSquaresA", 0);
							statline.Add("numSquaresB", 0);
							statline.Add("numSingleB", 0);
							statline.Add("numAdjB", 0);
							statline.Add("numOppB", 0);
							statline.Add("numTripleB", 0);
							statline.Add("isMatch", 0);
							foreach ( string tile in tilelist )
							{
								statline["numSquaresA"] += Regex.Matches(tile, "[" + patternsA + "]").Count;
								statline["numSquaresB"] += Regex.Matches(tile, "[" + patternsB + "]").Count;

								// numSingle
								if ( Regex.Matches(tile, "[" + patternsB + "]").Count == 1 )
								{
									statline["numSingleB"]++;
								}
								// numAdj - LT, TR, RB, LB
								if ( Regex.IsMatch(tile, String.Format("^[{0}][{0}]|^.[{0}][{0}]|^..[{0}][{0}]|^[{0}]..[{0}]", patternsB)) )
								{
									statline["numAdjB"]++;
								}
								// numOpp - LR, TB
								if ( Regex.IsMatch(tile, String.Format("^[{0}].[{0}]|^.[{0}].[{0}]", patternsB)) )
								{
									statline["numOppB"]++;
								}
								// numTriple
								if ( Regex.Matches(tile, "[" + patternsB + "]").Count == 3 )
								{
									statline["numTripleB"]++;
								}
							}
							if ( statline["numSquaresA"] == statline["numSquaresB"] )
							{
								statline["isMatch"] = 1;
							}
							numMatches += statline["isMatch"];
							stats.Add(String.Concat(colourA,",",colourB,",",String.Join(",", et2.Utils.IntListToStringList(new List<int>(statline.Values).ToArray()))));
						}
					}
				}
			}
			stats.Add("number of matches," + numMatches);
			stats.Add("");

			// count numAdj,numOpp between bgcolours
			uniquePairs.Clear();
			stats.Add("bgcolourA,bgcolourB,numTiles,numSquaresA,numSquaresB,numAdj,numOpp,numTriple,numQuad");
			foreach ( string colourA in bgcolours.Keys )
			{
				foreach ( string colourB in bgcolours.Keys )
				{
					if ( colourA == colourB )
					{
						isSame = true;
					}
					else
					{
						isSame = false;
					}
					string patternsA = bgcolours[colourA];
					string patternsB = bgcolours[colourB];

					// exclude duplicate pairs, eg. keep Blue,Orange but discard Orange,Blue
					if ( !isSame && !uniquePairs.Contains(patternsA + patternsB) && !uniquePairs.Contains(patternsB + patternsA) )
					{
						uniquePairs.Add(patternsA + patternsB);

						List<string> tilelist = this.getTileSearchResults(String.Format("^[{0}].*[{1}]", patternsA, patternsB), true);
						Dictionary<string, int> statline = new Dictionary<string, int>();
						statline.Add("numTiles", 0);
						statline.Add("numSquaresA", 0);
						statline.Add("numSquaresB", 0);
						statline.Add("numAdj", 0);
						statline.Add("numOpp", 0);
						statline.Add("numTriple", 0);
						statline.Add("numQuad", 0);
						foreach ( string tile in tilelist )
						{
							statline["numTiles"]++;
							statline["numSquaresA"] += Regex.Matches(tile, "[" + patternsA + "]").Count;
							statline["numSquaresB"] += Regex.Matches(tile, "[" + patternsB + "]").Count;

							// numQuad
							if ( Regex.Matches(tile, "[" + patternsA + patternsB + "]").Count == 4 )
							{
								statline["numQuad"]++;
							}
							// numTriple
							else if ( Regex.Matches(tile, "[" + patternsA + patternsB + "]").Count == 3 )
							{
								statline["numTriple"]++;
							}
							else
							{
								// numAdj - LT, TR, RB, LB
								if ( Regex.IsMatch(tile, String.Format("[{0}][{1}]|[{0}]..[{1}]", patternsA, patternsB)) )
								{
									statline["numAdj"]++;
								}
								// numOpp - LR, TB
								if ( Regex.IsMatch(tile, String.Format("^[{0}].[{1}]|^.[{0}].[{1}]", patternsA, patternsB)) )
								{
									statline["numOpp"]++;
								}
							}
						}
						stats.Add(String.Concat(colourA,",",colourB,",",String.Join(",", et2.Utils.IntListToStringList(new List<int>(statline.Values).ToArray()))));
					}
				}
			}
			stats.Add("");

			// count number of related bgcolour squares
			stats.Add("bgcolour," + String.Join(",", new List<string>(bgcolours.Keys).ToArray()));
			foreach ( string colourA in bgcolours.Keys )
			{
				string patternsA = bgcolours[colourA];
				Dictionary<string, int> statline = new Dictionary<string, int>();
				List<string> tilelist = this.getTileSearchResults(String.Format("^[{0}]", patternsA), true);
				foreach ( string colourB in bgcolours.Keys )
				{
					string patternsB = bgcolours[colourB];
					statline.Add(colourB, 0);
					foreach ( string tile in tilelist )
					{
						statline[colourB] += Regex.Matches(tile, "[" + patternsB + "]").Count;
					}
				}
				stats.Add(String.Concat(colourA,",",String.Join(",", et2.Utils.IntListToStringList(new List<int>(statline.Values).ToArray()))));
			}

			stats.Add("");

			// count number of related bgcolour squares for each pair of colours...
			uniquePairs.Clear();
			stats.Add("bgcolourA,bgColourB," + String.Join(",", new List<string>(bgcolours.Keys).ToArray()));
			foreach ( string colourA in bgcolours.Keys )
			{
				string patternsA = bgcolours[colourA];
				foreach ( string colourB in bgcolours.Keys )
				{
					string patternsB = bgcolours[colourB];
					// exclude duplicate pairs, eg. keep Blue,Orange but discard Orange,Blue, discard Blue/Blue
					if ( colourA != colourB && !uniquePairs.Contains(patternsA + patternsB) && !uniquePairs.Contains(patternsB + patternsA) )
					{
						List<string> tilelist = this.getTileSearchResults(String.Format("^[{0}].*[{1}]|[{1}].*[{0}]", patternsA, patternsB), true);
						uniquePairs.Add(patternsA + patternsB);
						Dictionary<string, int> statline = new Dictionary<string, int>();

						foreach ( string colourC in bgcolours.Keys )
						{
							string patternsC = bgcolours[colourC];
							statline.Add(colourC, 0);
							foreach ( string tile in tilelist )
							{
								statline[colourC] += Regex.Matches(tile, "[" + patternsC + "]").Count;
							}
						}
						stats.Add(String.Concat(colourA,",",colourB,",",String.Join(",", et2.Utils.IntListToStringList(new List<int>(statline.Values).ToArray()))));
					}
				}
			}

			Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.tbSLog1.Update();
		}

		public void showShapeStats()
		{
			List<string> stats = new List<string>();
			HashSet<string> uniquePairs = new HashSet<string>();
			bool isSame = false;

			stats.Add("shape,patterns,numTiles,numSquares,numBorder,numSingle,numAdj,numOpp,numTriple");
			Dictionary<string, string> styles = Board.getShapeList();
			foreach ( string shape in styles.Keys )
			{
				string patterns = styles[shape];
				int[] stat = {0,0,0,0};
				// count number of borders
				List<string> btilelist = this.getTileSearchResults(String.Format("^-.[{0}]", patterns), true);
				int numBorder = btilelist.Count;
				List<string> tilelist = this.getTileSearchResults(String.Format("^[{0}][^-]{{3}}", patterns), true);
				int numSquares = 0;
				foreach ( string tile in tilelist )
				{
					numSquares += Regex.Matches(tile, "[" + patterns + "]").Count;

					// numSingle
					if ( Regex.Matches(tile, "[" + patterns + "]").Count == 1 )
					{
						stat[0]++;
					}
					// numAdj - LT, TR, RB, LB
					if ( Regex.IsMatch(tile, String.Format("^[{0}][{0}]|^.[{0}][{0}]|^..[{0}][{0}]|^[{0}]..[{0}]", patterns)) )
					{
						stat[1]++;
					}
					// numOpp - LR, TB
					if ( Regex.IsMatch(tile, String.Format("^[{0}].[{0}]|^.[{0}].[{0}]", patterns)) )
					{
						stat[2]++;
					}
					// numTriple
					if ( Regex.Matches(tile, "[" + patterns + "]").Count == 3 )
					{
						stat[3]++;
					}
				}
				numSquares = (numSquares + numBorder) / 2;
				string line = shape + "," + patterns + "," + tilelist.Count + "," + numSquares + "," + numBorder + "," + String.Join(",", et2.Utils.IntListToStringList(stat));
				stats.Add(line);
			}
			stats.Add("");

			// count shape associations
			stats.Add("shapeA,shapeB,numTiles,numSquaresA,numSquaresB,numSingleB,numAdjB,numOppB,numTripleB");
			foreach ( string shapeA in styles.Keys )
			{
				foreach ( string shapeB in styles.Keys )
				{
					if ( shapeA != shapeB )
					{
						string patternsA = styles[shapeA];
						string patternsB = styles[shapeB];
						List<string> tilelist = this.getTileSearchResults(String.Format("^[{0}].*[{1}]", patternsA, patternsB), true);
						Dictionary<string, int> statline = new Dictionary<string, int>();
						statline.Add("numTiles", tilelist.Count);
						statline.Add("numSquaresA", 0);
						statline.Add("numSquaresB", 0);
						statline.Add("numSingleB", 0);
						statline.Add("numAdjB", 0);
						statline.Add("numOppB", 0);
						statline.Add("numTripleB", 0);
						foreach ( string tile in tilelist )
						{
							statline["numSquaresA"] += Regex.Matches(tile, "[" + patternsA + "]").Count;
							statline["numSquaresB"] += Regex.Matches(tile, "[" + patternsB + "]").Count;

							// numSingle
							if ( Regex.Matches(tile, "[" + patternsB + "]").Count == 1 )
							{
								statline["numSingleB"]++;
							}
							// numAdj - LT, TR, RB, LB
							if ( Regex.IsMatch(tile, String.Format("^[{0}][{0}]|^.[{0}][{0}]|^..[{0}][{0}]|^[{0}]..[{0}]", patternsB)) )
							{
								statline["numAdjB"]++;
							}
							// numOpp - LR, TB
							if ( Regex.IsMatch(tile, String.Format("^[{0}].[{0}]|^.[{0}].[{0}]", patternsB)) )
							{
								statline["numOppB"]++;
							}
							// numTriple
							if ( Regex.Matches(tile, "[" + patternsB + "]").Count == 3 )
							{
								statline["numTripleB"]++;
							}
						}
						stats.Add(String.Concat(shapeA,",",shapeB,",",String.Join(",", et2.Utils.IntListToStringList(new List<int>(statline.Values).ToArray()))));
					}
				}
			}
			stats.Add("");

			// count numAdj,numOpp between shapes
			uniquePairs.Clear();
			stats.Add("shapeA,shapeB,numTiles,numSquaresA,numSquaresB,numAdj,numOpp,numTriple,numQuad");
			foreach ( string shapeA in styles.Keys )
			{
				foreach ( string shapeB in styles.Keys )
				{
					if ( shapeA == shapeB )
					{
						isSame = true;
					}
					else
					{
						isSame = false;
					}
					string patternsA = styles[shapeA];
					string patternsB = styles[shapeB];

					// exclude duplicate pairs, eg. keep Castle,Star but discard Star,Castle
					if ( !isSame && !uniquePairs.Contains(patternsA + patternsB) && !uniquePairs.Contains(patternsB + patternsA) )
					{
						uniquePairs.Add(patternsA + patternsB);

						List<string> tilelist = this.getTileSearchResults(String.Format("^[{0}].*[{1}]", patternsA, patternsB), true);
						Dictionary<string, int> statline = new Dictionary<string, int>();
						statline.Add("numTiles", 0);
						statline.Add("numSquaresA", 0);
						statline.Add("numSquaresB", 0);
						statline.Add("numAdj", 0);
						statline.Add("numOpp", 0);
						statline.Add("numTriple", 0);
						statline.Add("numQuad", 0);
						foreach ( string tile in tilelist )
						{
							// exclude border tiles
							if ( !Regex.IsMatch(tile, "-") )
							{
								statline["numTiles"]++;
								statline["numSquaresA"] += Regex.Matches(tile, "[" + patternsA + "]").Count;
								statline["numSquaresB"] += Regex.Matches(tile, "[" + patternsB + "]").Count;

								// numQuad
								if ( Regex.Matches(tile, "[" + patternsA + patternsB + "]").Count == 4 )
								{
									statline["numQuad"]++;
								}
								// numTriple
								else if ( Regex.Matches(tile, "[" + patternsA + patternsB + "]").Count == 3 )
								{
									statline["numTriple"]++;
								}
								else
								{
									// numAdj - LT, TR, RB, LB
									if ( Regex.IsMatch(tile, String.Format("[{0}][{1}]|[{0}]..[{1}]", patternsA, patternsB)) )
									{
										statline["numAdj"]++;
									}
									// numOpp - LR, TB
									if ( Regex.IsMatch(tile, String.Format("^[{0}].[{1}]|^.[{0}].[{1}]", patternsA, patternsB)) )
									{
										statline["numOpp"]++;
									}
								}
							}
						}
						stats.Add(String.Concat(shapeA,",",shapeB,",",String.Join(",", et2.Utils.IntListToStringList(new List<int>(statline.Values).ToArray()))));
					}
				}
			}

			Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", stats.ToArray());
			Program.TheMainForm.tbSLog1.Update();
		}

		public void showBGColourCount(bool distinct)
		{
			Dictionary<string, int> bgCount = new Dictionary<string, int>();
			List<string> output = new List<string>();
			if ( distinct )
			{
				output.Add("tile_bgcolour_distinct_count");
				// get distinct bgcolours from each tile (ie. LHII = green, yellow, purple)
				foreach ( Tile tile in this.tileset )
				{
					HashSet<string> bgTitleSet = new HashSet<string>();
					for ( int p = 0; p < 4; p++ )
					{
						char pattern = tile.pattern[p];
						string bgcolour = Board.getPatternBGColour(pattern);
						if ( bgcolour != "" )
						{
							bgTitleSet.Add(bgcolour);
						}
					}
					List<string> bgTitleList = new List<string>(bgTitleSet);
					bgTitleList.Sort();
					string bgTitle = String.Join(",", bgTitleList.ToArray());
					if ( bgCount.ContainsKey(bgTitle) )
					{
						bgCount[bgTitle]++;
					}
					else
					{
						bgCount.Add(bgTitle, 1);
					}
				}
			}
			else
			{
				output.Add("tile_bgcolour_count");
				// get all bgcolours from each tile (ie. LHII = green, yellow, purple, purple)
				foreach ( Tile tile in this.tileset )
				{
					List<string> bgTitleList = new List<string>();
					for ( int p = 0; p < 4; p++ )
					{
						char pattern = tile.pattern[p];
						string bgcolour = Board.getPatternBGColour(pattern);
						if ( bgcolour != "" )
						{
							bgTitleList.Add(bgcolour);
						}
					}
					bgTitleList.Sort();
					string bgTitle = String.Join(",", bgTitleList.ToArray());
					if ( bgCount.ContainsKey(bgTitle) )
					{
						bgCount[bgTitle]++;
					}
					else
					{
						bgCount.Add(bgTitle, 1);
					}
				}
			}

			output.Add("numTiles:bgColours");
			foreach ( string bgTitle in bgCount.Keys )
			{
				output.Add(String.Format("{0}:{1}", bgCount[bgTitle], bgTitle));
			}

			Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", output.ToArray());
			Program.TheMainForm.tbSLog1.Update();
		}

		public void showShapeCount(bool distinct)
		{
			Dictionary<string, int> shapeCount = new Dictionary<string, int>();
			List<string> output = new List<string>();
			if ( distinct )
			{
				output.Add("tile_shape_distinct_count");
				// get distinct shapes from each tile (ie. LHII = pointedCross,star,crossInCircle)
				foreach ( Tile tile in this.tileset )
				{
					HashSet<string> titleSet = new HashSet<string>();
					for ( int p = 0; p < 4; p++ )
					{
						char pattern = tile.pattern[p];
						string shape = Board.getPatternShape(pattern);
						if ( shape != "" )
						{
							titleSet.Add(shape);
						}
					}
					List<string> titleList = new List<string>(titleSet);
					titleList.Sort();
					string title = String.Join(",", titleList.ToArray());
					if ( shapeCount.ContainsKey(title) )
					{
						shapeCount[title]++;
					}
					else
					{
						shapeCount.Add(title, 1);
					}
				}
			}
			else
			{
				output.Add("tile_shape_count");
				// get all shapes from each tile (ie. LHII = pointedCross,star,crossInCircle,crossInCircle)
				foreach ( Tile tile in this.tileset )
				{
					List<string> titleList = new List<string>();
					for ( int p = 0; p < 4; p++ )
					{
						char pattern = tile.pattern[p];
						string shape = Board.getPatternShape(pattern);
						if ( shape != "" )
						{
							titleList.Add(shape);
						}
					}
					titleList.Sort();
					string title = String.Join(",", titleList.ToArray());
					if ( shapeCount.ContainsKey(title) )
					{
						shapeCount[title]++;
					}
					else
					{
						shapeCount.Add(title, 1);
					}
				}
			}

			output.Add("numTiles:shapes");
			foreach ( string title in shapeCount.Keys )
			{
				output.Add(String.Format("{0}:{1}", shapeCount[title], title));
			}

			Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", output.ToArray());
			Program.TheMainForm.tbSLog1.Update();
		}

		public void showTilePatternCount(bool distinct)
		{
			Dictionary<string, int> pCount = new Dictionary<string, int>();
			List<string> output = new List<string>();
			if ( distinct )
			{
				output.Add("tile_pattern_distinct_count");
				// get distinct patterns from each tile (eg. HIIL = HIL)
				foreach ( Tile tile in this.tileset )
				{
					HashSet<string> pset = new HashSet<string>();
					for ( int p = 0; p < 4; p++ )
					{
						char pattern = tile.pattern[p];
						if ( pattern != '-' )
						{
							pset.Add(pattern.ToString());
						}
					}
					List<string> plist = new List<string>(pset);
					plist.Sort();
					string pTitle = String.Join(",", plist.ToArray());
					if ( pCount.ContainsKey(pTitle) )
					{
						pCount[pTitle]++;
					}
					else
					{
						pCount.Add(pTitle, 1);
					}
				}
			}
			else
			{
				output.Add("tile_pattern_count");
				// get all patterns from each tile (eg. HIIL)
				foreach ( Tile tile in this.tileset )
				{
					List<string> plist = new List<string>();
					for ( int p = 0; p < 4; p++ )
					{
						char pattern = tile.pattern[p];
						if ( pattern != '-' )
						{
							plist.Add(pattern.ToString());
						}
					}
					plist.Sort();
					string pTitle = String.Join(",", plist.ToArray());
					if ( pCount.ContainsKey(pTitle) )
					{
						pCount[pTitle]++;
					}
					else
					{
						pCount.Add(pTitle, 1);
					}
				}
			}

			output.Add("numTiles:patterns");
			foreach ( string pTitle in pCount.Keys )
			{
				output.Add(String.Format("{0}:{1}", pCount[pTitle], pTitle));
			}

			Program.TheMainForm.tbSLog1.Text = String.Join("\r\n", output.ToArray());
			Program.TheMainForm.tbSLog1.Update();
		}

		public static int getCellIdFromColRow(int col, int row)
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

		// return a copy of the tileset in the new v3 format (without using TileInfo)
		public SortedDictionary<int, string> getTileSetV3()
		{
			SortedDictionary<int, string> tileset = new SortedDictionary<int, string>();
			for ( int i = 0; i < this.tileset.Length; i++ )
			{
				tileset.Add(this.tileset[i].id, this.tileset[i].pattern.Clone().ToString());
			}
			return tileset;
		}

		// return a copy of the tileset
		public Tile[] copyTileSet(Tile[] source)
		{
			Tile[] tileset = new Tile[source.Length];
			for ( int i = 0; i < source.Length; i++ )
			{
				tileset[i] = new Tile(source[i].id, source[i].pattern.Clone().ToString(), 1);
			}
			return tileset;
		}

		public void clearRegion(CellRegion region)
		{
			for ( int row = region.topLeftCellRow; row <= region.bottomRightCellRow; row++ )
			{
				for ( int col = region.topLeftCellCol; col <= region.bottomRightCellCol; col++ )
				{
					this.removeTile(col, row);
				}
			}
		}

	}
}

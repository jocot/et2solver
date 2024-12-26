/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 2/08/2010
 * Time: 10:05 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.IO;
using CAF;

namespace ET2Solver
{
	/// <summary>
	/// Description of BoardDesigner.
	/// </summary>
	public class BoardDesigner
	{
		public Image blankBoardImage;
		private Graphics gboard;

		// board size - derived from image filename
		public int num_cols = 16;
		public int num_rows = 16;
		public int num_pos = 480;

		// 16x16 = 480 squares
		public bool isInitialised = false;
		public char[] board_squares;

		// 16x16 = 256 tiles
		public Dictionary<int, char[]> board_tiles;

		// pattern count - count,x,y
		private Dictionary<char, int[]> patternCount = new Dictionary<char, int[]>();
		private Dictionary<char, System.Windows.Forms.Label> patternCountLabels = new Dictionary<char, System.Windows.Forms.Label>();
		private char currentPattern = 'A';

		// tesselation shapes
		private int shapeSize = 6;
		private string shapeDesign = "66344";
		private string shapePatterns = "FFFFFF";
		private string hoverShapeDesign = "";
		private int hoverShapePos = -1;
		private int shapeRotation = 0;
		private bool shapeFlipped = false;

		private Dictionary<int, int[]> boardPosList = new Dictionary<int, int[]>();

		// interactive actions
		private int current_pos = -1;
		private string action = "";
		private int action_step = 0;

		private int select_start_cell = 0;
		private int select_end_cell = 0;

		public BoardDesigner()
		{
			this.generateBoardPosList();
			this.getBoardImageFilenames();
			this.drawPatternPanel();
			this.setupCommands();
//			Program.TheMainForm.selDesignSize.Text = Program.TheMainForm.selDesignSize.Items[0].ToString();
			Program.TheMainForm.selDesignSize.Text = "board_16x16";
			this.newDesign();
			Program.TheMainForm.tabDesign.MouseWheel += new System.Windows.Forms.MouseEventHandler(Program.TheMainForm.Pb_designMouseWheel);
		}

		private void getBoardImageFilenames()
		{
			// get board filenames: images\board_*.jpg
			Program.TheMainForm.selDesignSize.Items.Clear();
			string[] filenames = System.IO.Directory.GetFiles(CAF_Application.config.imagePath() + "\\boards", "*.jpg");
			Array.Sort(filenames);
			foreach ( string filename in filenames )
			{
				string name = System.IO.Path.GetFileNameWithoutExtension(filename);
				if ( Regex.IsMatch(name, "^board_") )
				{
					Program.TheMainForm.selDesignSize.Items.Add(name);
				}
			}
		}

		private void selectDesignLayout()
		{
			// select board layout & draw board background
			string boardImageFilename = Program.TheMainForm.selDesignSize.Text + ".jpg";
			if ( File.Exists(CAF_Application.config.imagePath() + "\\boards\\" + boardImageFilename) )
			{
				this.blankBoardImage = new Bitmap(CAF_Application.config.imagePath() + "\\boards\\" + boardImageFilename);
				Program.TheMainForm.pb_design.Load(CAF_Application.config.imagePath() + "\\boards\\" + boardImageFilename);
				this.gboard = Graphics.FromImage(Program.TheMainForm.pb_board.Image);

				string name = Program.TheMainForm.selDesignSize.Text;
				string[] parts = name.Split('_', 'x');
				this.num_cols = Convert.ToInt16(parts[1]);
				this.num_rows = Convert.ToInt16(parts[2]);
				this.num_pos = (this.num_cols * (this.num_rows - 1)) + ((this.num_cols-1) * this.num_rows);
				this.board_squares = new char[this.num_pos];
				this.board_tiles = new Dictionary<int, char[]>(this.num_cols * this.num_rows);
			}
		}

		public void newDesign()
		{
			this.selectDesignLayout();
			this.drawPatternPanel();
			this.gboard = Graphics.FromImage(Program.TheMainForm.pb_design.Image);
			this.generateBoardPosList();
			this.isInitialised = true;
		}

		private Dictionary<string, string> getPatternList()
		{
			Dictionary<string, string> rlist = new Dictionary<string, string>();
		    rlist.Add("Orange","AR");
	    	rlist.Add("Blue","BSTV");
		    rlist.Add("Pink","CFU");
	    	rlist.Add("Green","DJL");
		    rlist.Add("Red","EKM");
		    rlist.Add("Purple","GI");
		    rlist.Add("Yellow","HOQ");
		    rlist.Add("Lightblue","NP");
		    rlist.Add("Custom1","WXYZ");
		    rlist.Add("Custom2","1234");
		    rlist.Add("Custom3","5678");
		    rlist.Add("Custom4","9");
			return rlist;
		}

		private void drawPatternPanel()
		{
			int x = 2;
			int y = 2;
			Program.TheMainForm.patternPanel.Controls.Clear();
			Dictionary<string, string> patternList = this.getPatternList();
			foreach ( string key in patternList.Keys )
			{
				foreach ( char p in patternList[key] )
				{
					this.drawTextBox(p.ToString() + (Convert.ToInt16(p)-64), true, x, y, 32, 16, 8F, Color.Black, Color.White);
					this.updatePatternCount(p, 0, x, y+16);
					this.drawSquare(p.ToString(), x+32, y);
					x += 69;
				}
				x = 2;
				y += 35;
			}
		}

		private System.Windows.Forms.Label drawTextBox(string text, bool is3D, int x, int y, int width, int height, float fontsize, Color foreColour, Color bgColour)
		{
			System.Windows.Forms.Label label = new System.Windows.Forms.Label();
			label.Text = text;
			if ( is3D )
			{
				label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			}
			label.Location = new Point(x, y);
			label.Size = new System.Drawing.Size(width, height);
			label.Font = new System.Drawing.Font("Microsoft Sans Serif", fontsize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label.ForeColor = foreColour;
			label.BackColor = bgColour;
			label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			Program.TheMainForm.patternPanel.Controls.Add(label);
			label.BringToFront();
			label.Show();
			return label;
		}

		private void drawSquare(string pattern, int x, int y)
		{
			System.Windows.Forms.Label label = new System.Windows.Forms.Label();
			string filename = CAF_Application.config.imagePath() + "\\patterns-flat\\" + pattern + ".png";
			if ( filename != "" )
			{
				if ( System.IO.File.Exists(filename) )
				{
					//Program.TheMainForm.log("loading image: " + bgImage);
					label.Image = new Bitmap(filename);
					label.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
				}
			}
			label.Location = new Point(x, y);
			label.Size = new System.Drawing.Size(32, 32);
			label.BackColor = Color.Transparent;
			Program.TheMainForm.patternPanel.Controls.Add(label);
			label.BringToFront();
			label.Show();

			// use text field to identify pattern when clicked
			// minimise font so it's barely noticable
			label.Font = new System.Drawing.Font("Microsoft Sans Serif", 1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label.Text = pattern;
			label.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			label.MouseUp += new System.Windows.Forms.MouseEventHandler(this.patternSelect);
		}

		private void drawPatternAtXY(char pattern, int x, int y)
		{
			if ( pattern == '-' )
			{
				return;
			}
//			Program.TheMainForm.tbDesignLog.AppendText(this.currentPattern + "\r\n");

			// draw pattern on board (has black border around it, and is a little small)
			Image bitmap = new Bitmap(50, 50, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			Image sbitmap = new Bitmap(CAF_Application.config.imagePath() + "\\patterns-flat\\" + pattern + ".png");
			Graphics gimage = Graphics.FromImage(bitmap);
//			// move image to the right
			gimage.TranslateTransform((float)bitmap.Width/2, 0);
//			// rotate 45 degrees
			gimage.RotateTransform(45F);
			// scale up so it fills the size correctly
			gimage.ScaleTransform(1.2F, 1.2F);
//			// update moved/rotated image
			gimage.DrawImage(sbitmap, 0, 0, 32, 32);
			// draw as polygon (correct size)
			Point[] polygonPoints = new Point[5];
			polygonPoints[0] = new Point(x+bitmap.Width/2, y-bitmap.Height/2);
			polygonPoints[1] = new Point(x+bitmap.Width, y);
			polygonPoints[2] = new Point(x+bitmap.Width/2, y+bitmap.Height/2);
			polygonPoints[3] = new Point(x+0, y);
			polygonPoints[4] = new Point(x+bitmap.Width/2, y-bitmap.Height/2);
			TextureBrush tb = new TextureBrush(bitmap);
			tb.TranslateTransform(x,y-bitmap.Height/2);
			this.gboard.FillPolygon(tb, polygonPoints);

			// draw as polygon to board (original size as pattern image 109x109)
//			Point[] polygonPoints = new Point[5];
//			Image bitmap = new Bitmap(109, 109, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
//			Graphics gimage = Graphics.FromImage(bitmap);
//			Image sbitmap = new Bitmap("patterns\\" + pattern + ".gif");
//			TextureBrush stb = new TextureBrush(sbitmap);
//			polygonPoints[0] = new Point(sbitmap.Width/2, 0);
//			polygonPoints[1] = new Point(sbitmap.Width, sbitmap.Height/2);
//			polygonPoints[2] = new Point(sbitmap.Width/2, sbitmap.Height);
//			polygonPoints[3] = new Point(0, sbitmap.Height/2);
//			polygonPoints[4] = new Point(sbitmap.Width/2, 0);
//			gimage.FillPolygon(stb, polygonPoints);
//			polygonPoints[0] = new Point(x+sbitmap.Width/2, y+0);
//			polygonPoints[1] = new Point(x+sbitmap.Width, y+sbitmap.Height/2);
//			polygonPoints[2] = new Point(x+sbitmap.Width/2, y+sbitmap.Height);
//			polygonPoints[3] = new Point(x+0, y+sbitmap.Height/2);
//			polygonPoints[4] = new Point(x+sbitmap.Width/2, y+0);
//			TextureBrush tb = new TextureBrush(bitmap);
//			tb.TranslateTransform(x,y);
//			this.gboard.FillPolygon(tb, polygonPoints);

			Program.TheMainForm.pb_design.Refresh();
		}

		public void handleBoardClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
//			Program.TheMainForm.tbDesignLog.AppendText("mouse click XY @ " + e.X + "," + e.Y + "\r\n");
			this.current_pos = this.getBoardPos(e.X, e.Y);
			switch ( e.Button.ToString() )
			{
				case "Left":
					if ( this.action == "fill_line" && this.current_pos > 0 )
					{
						this.fillLine();
					}
					else if ( this.action == "draw_shapes" )
					{
						this.placeShape();
					}
					else
					{
						// left click = place currently selected pattern
						// auto update pattern counters
						if ( this.current_pos > 0 )
						{
							char pattern = this.currentPattern;
							this.drawPatternAtPos(pattern, this.current_pos);
						}
					}
					break;
				case "Right":
					if ( this.action == "draw_shapes" )
					{
						this.action = "";
						this.removeShapeHover();
					}
					// right click = remove pattern from board
					// auto update pattern counters
					else if ( this.current_pos > 0 )
					{
						this.removePatternAtPos(this.current_pos);
					}
					break;
				case "Middle":
					if ( this.action == "draw_shapes" )
					{
						// flip shape
						this.flipShapeHover();
					}
					else if ( this.action != "" )
					{
						this.action = "";
						this.action_step = 0;
						Program.TheMainForm.statusInstructions.Text = "";
					}
					break;
			}
		}

		public void handleMouseMovement(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// highlight selected shape if cmd = draw_shapes
			// remove shape from previous hovering position
			// redraw placed tiles beneath selected regions
			if ( this.action == "draw_shapes" )
			{
				int pos = this.getBoardPos(e.X, e.Y);
				if ( pos != this.current_pos )
				{
					this.removeShapeHover();
					this.current_pos = pos;
					this.drawShapeHover();
				}
			}
		}

		public void handleMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( this.action == "draw_shapes" )
			{
				this.rotateShape(e.Delta > 0);
			}
		}

		public void patternSelect(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.Label label = (System.Windows.Forms.Label)sender;
			// deselect previously selected pattern
			if ( this.patternCountLabels.ContainsKey(this.currentPattern) )
			{
				this.patternCountLabels[this.currentPattern].BackColor = Color.White;
			}
			if ( this.patternCountLabels.ContainsKey(label.Text[0]) )
			{
				this.currentPattern = label.Text[0];
				this.patternCountLabels[label.Text[0]].BackColor = Color.LemonChiffon;
			}

			// update patterns in tesselated shape drawing
			if ( this.action == "draw_shapes" )
			{
				this.selectShape();
			}
		}

		private void updatePatternCount(char pattern, int count, int x, int y)
		{
			if ( pattern == '-' )
			{
				return;
			}
			// update counter
			if ( this.patternCount.ContainsKey(pattern) )
			{
				this.patternCount[pattern][0] = count;
				this.patternCount[pattern][1] = x;
				this.patternCount[pattern][2] = y;
			}
			else
			{
				this.patternCount.Add(pattern, new int[]{count, x, y});
			}
			// update label
			Color bgcolour = Color.White;
			if ( this.currentPattern == pattern )
			{
				bgcolour = Color.LemonChiffon;
			}
			System.Windows.Forms.Label label = this.drawTextBox(count.ToString(), true, x, y, 32, 16, 8F, Color.Gray, bgcolour);
			if ( this.patternCountLabels.ContainsKey(pattern) )
			{
				Program.TheMainForm.patternPanel.Controls.Remove(this.patternCountLabels[pattern]);
				this.patternCountLabels[pattern] = label;
			}
			else
			{
				this.patternCountLabels.Add(pattern, label);
			}
		}

		private void addPatternCount(char pattern)
		{
			if ( this.patternCount.ContainsKey(pattern) )
			{
				int count = this.patternCount[pattern][0];
				int x = this.patternCount[pattern][1];
				int y = this.patternCount[pattern][2];
				this.updatePatternCount(pattern, count+1, x, y);
			}
		}

		private void removePatternCount(char pattern)
		{
			if ( this.patternCount.ContainsKey(pattern) )
			{
				int count = this.patternCount[pattern][0];
				if ( count > 0 )
				{
					count--;
				}
				int x = this.patternCount[pattern][1];
				int y = this.patternCount[pattern][2];
				this.updatePatternCount(pattern, count, x, y);
			}
		}

		public void importDesign()
		{
			if ( !this.isInitialised )
			{
				this.newDesign();
			}
			foreach ( string line in Program.TheMainForm.tbDesignLog.Lines )
			{
				string[] parts = line.Split(',');
				if ( !Regex.IsMatch(line, "^;") && parts.Length == 2 )
				{
					int pos = Convert.ToInt16(parts[0]);
					char pattern = Convert.ToChar(parts[1]);
					this.drawPatternAtPos(pattern, pos);
				}
			}
		}

		public void exportDesign()
		{
			if ( !this.isInitialised )
			{
				return;
			}
			// export format: pos,pattern
			List<string> export = new List<string>();
			for ( int i = 0; i < this.board_squares.Length; i++ )
			{
				if ( this.board_squares[i] != (char)0 )
				{
					char pattern = this.board_squares[i];
					export.Add(String.Format("{0},{1}", i+1, pattern));
				}
			}
			Program.TheMainForm.tbDesignLog.Text = String.Join("\r\n", export.ToArray());
		}

		// checks if mouse pointer is within diamond at x,y
		private bool isBoardPosAtXY(int x, int y, int mx, int my)
		{
			int size = 50;
			GraphicsPath gp = new GraphicsPath();
			Point[] poly = new Point[5];
			poly[0] = new Point(x+size/2, y-size/2);
			poly[1] = new Point(x+size, y);
			poly[2] = new Point(x+size/2, y+size/2);
			poly[3] = new Point(x+0, y);
			poly[4] = new Point(x+size/2, y-size/2);
			gp.AddPolygon(poly);
			System.Drawing.Region r = new Region(gp);
			Rectangle rect = new Rectangle(mx,my,1,1);
			r.Intersect(rect);
			RectangleF intersection = r.GetBounds(this.gboard);
			if ( intersection.Width > 0 && intersection.Height > 0 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private int getBoardPos(int x, int y)
		{
			// iterate through nearest diamond pos to see which one mouse clicked inside of
			foreach ( int pos in this.boardPosList.Keys )
			{
				int[] colrow = this.boardPosList[pos];
				if ( this.isBoardPosAtXY(colrow[0], colrow[1], x, y) )
				{
					return pos;
				}
			}
			return -1;
		}

		private int[] getBoardXYForPos(int pos)
		{
			if ( this.boardPosList.ContainsKey(pos) )
			{
				int[] colrow = this.boardPosList[pos];
				return colrow;
			}
			else
			{
				return new int[]{0,0};
			}
		}

		private void drawPatternAtPos(char pattern, int pos)
		{
			if ( pattern == '-' )
			{
				return;
			}
			if ( this.boardPosList.ContainsKey(pos) )
			{
				int[] colrow = this.boardPosList[pos];
				// remove previous pattern if exists
				this.removePatternAtPos(pos);
				// add new pattern count
				this.board_squares[pos-1] = pattern;
				this.addPatternCount(pattern);
				this.drawPatternAtXY(pattern, colrow[0], colrow[1]);
				Program.TheMainForm.statusInstructions.Text = pos + " = " + pattern;
			}
		}

		private void generateBoardPosList()
		{
			// diamond pos begins at left point
			this.boardPosList.Clear();
			int x = 0;
			int y = 0;
			int size = 50;
			int rowsize = (this.num_cols * 2) - 1;
			for ( int i = 1; i <= this.num_pos; i++ )
			{
				int rowpos = i % rowsize;
				int row;
				if ( i % rowsize == 0 )
				{
					row = i / rowsize;
				}
				else
				{
					row = (i / rowsize) + 1;
				}
				int col = i - ((row-1) * rowsize);
				if ( rowpos >= 1 && rowpos <= this.num_cols - 1 )
				{
					// 15 per row
					x = size/2 + ((col-1) * size);
					y = size/2 + ((row-1) * size);
				}
				else
				{
					// this.num_cols per row
					// reset column # for 2nd row style. 16 = 1 on 2nd row style..
					if ( col > this.num_cols - 1 )
					{
						col -= this.num_cols - 1;
					}
					else
					{
						// col must be 0 which = this.num_cols
						col = this.num_cols;
					}
					x = ((col-1) * size);
					y = size + ((row-1) * size);
				}
				if ( col > 0 )
				{
					x += col - 1;
				}
				if ( row > 1 )
				{
					y += row - 1;
				}
				this.boardPosList.Add(i, new int[]{x, y});
			}
		}

		private void removePatternAtPos(int pos)
		{
			if ( this.board_squares.Length >= pos && this.board_squares[pos-1] != (char)0 )
			{
				this.removePatternCount(this.board_squares[pos-1]);
				this.board_squares[pos-1] = (char)0;
				// redraw board
				this.redrawCell(pos);
			}
		}

		private void redrawCell(int pos)
		{
			// redraw polygon from blank board backup copy
			int[] colrow = this.getBoardXYForPos(pos);
			int x = colrow[0];
			int y = colrow[1];
			int width = 50;
			int height = 50;
			Point[] polygonPoints = new Point[5];
			polygonPoints[0] = new Point(x+width/2, y-height/2);
			polygonPoints[1] = new Point(x+width, y);
			polygonPoints[2] = new Point(x+width/2, y+height/2);
			polygonPoints[3] = new Point(x+0, y);
			polygonPoints[4] = new Point(x+width/2, y-height/2);
			TextureBrush tb = new TextureBrush(this.blankBoardImage);
			this.gboard.FillPolygon(tb, polygonPoints);
			Program.TheMainForm.pb_design.Refresh();
		}

		public void exportFilter(string translationMethod)
		{
			// row 1
			// cells  1-15 -> tiles 1-16 LR
			// cells 16-31 -> tiles 1-16 D + 17-32 U

			// row 2
			// cells 32-47 -> tiles 17-32 LR
			// cells 48-63 -> tiles 17-32 D + 33-48 U
			List<string> export = new List<string>();
			this.board_tiles = new Dictionary<int, char[]>(this.num_cols * this.num_rows);
			for ( int pos = 1; pos <= this.num_pos; pos++ )
			{
				if ( this.board_squares[pos-1] != (char)0 )
				{
					int[] colrow = this.getColRowFromPos(pos);
					int col = colrow[0];
					int row = colrow[1];
					int tileId = 0;
					int offset = 0;
					char pattern;
					pattern = this.board_squares[pos-1];
					offset = (row-1) * this.num_cols;
					if ( col < this.num_cols )
					{
						// tiles 1 - this.num_cols LR

						// left tile
						// col = R
						tileId = offset + col;
						if ( !this.board_tiles.ContainsKey(tileId) )
						{
							// initialise blank tile as .... (for regex filter)
							this.board_tiles.Add(tileId, "....".ToCharArray());
						}
						this.setBorderFilter(tileId, col, row);
						// pattern goes into R cell
						this.board_tiles[tileId][2] = pattern;

						// right tile
						// col+1 = L
						tileId = offset + col+1;
						if ( !this.board_tiles.ContainsKey(tileId) )
						{
							// initialise blank tile as .... (for regex filter)
							this.board_tiles.Add(tileId, "....".ToCharArray());
						}
						this.setBorderFilter(tileId, col+1, row);
						// pattern goes into L cell
						this.board_tiles[tileId][0] = pattern;
					}
					else
					{
						// squares num_cols+1 to (num_cols + num_cols-1) (16-31)
						col -= this.num_cols - 1;
						// top tile
						tileId = offset + col;
						if ( !this.board_tiles.ContainsKey(tileId) )
						{
							// initialise blank tile as .... (for regex filter)
							this.board_tiles.Add(tileId, "....".ToCharArray());
						}
						// row = D
						this.setBorderFilter(tileId, col, row);
						// pattern goes into D cell
						this.board_tiles[tileId][3] = pattern;

						// bottom tile
						// row+1 = U
						tileId = offset + col + this.num_cols;
						if ( !this.board_tiles.ContainsKey(tileId) )
						{
							// initialise blank tile as .... (for regex filter)
							this.board_tiles.Add(tileId, "....".ToCharArray());
						}
						this.setBorderFilter(tileId, col, row+1);
						// pattern goes into U cell
						this.board_tiles[tileId][1] = pattern;
					}
				}
			}
			// export sorted by tileId
			List<int> keys = new List<int>(this.board_tiles.Keys);
			keys.Sort();
			foreach ( int tileId in keys )
			{
				export.Add(String.Format("{0},^{1}", tileId, new String(this.board_tiles[tileId])));
			}

			string filter = String.Join("\r\n", export.ToArray());
			switch ( translationMethod )
			{
				case "bgcolour":
					Dictionary<string, string> bgcolours = Board.getBGColourList("all");
					foreach ( string bgcolour in bgcolours.Keys )
					{
						string tpattern = "[" + bgcolours[bgcolour] + "]";
						filter = Regex.Replace(filter, tpattern, tpattern, RegexOptions.IgnoreCase);
					}
					break;
				case "shape":
					Dictionary<string, string> shapes = Board.getShapeList();
					foreach ( string shape in shapes.Keys )
					{
						string tpattern = "[" + shapes[shape] + "]";
						filter = Regex.Replace(filter, tpattern, tpattern, RegexOptions.IgnoreCase);
					}
					break;
			}
			Program.TheMainForm.tbDesignLog.Text = filter;
			Program.TheMainForm.cbCellFilter.Checked = true;
			Program.TheMainForm.inputCellFilter.Text = filter;
		}

		private int[] getColRowFromPos(int pos)
		{
			// 16x16 / 480 squares returns cols 1-31, rows 1-16. Max is col=15, row=16
			int rowsize = (this.num_cols * 2) - 1;
			int rowpos = pos % rowsize;
			int row;
			if ( pos % rowsize == 0 )
			{
				row = pos / rowsize;
			}
			else
			{
				row = (pos / rowsize) + 1;
			}
			int col = pos - ((row-1) * rowsize);
			if ( col == 0 )
			{
				col = this.num_cols;
			}
			return new int[]{col, row};
		}

		private void setBorderFilter(int tileId, int col, int row)
		{
			if ( col == 1 )
			{
				// left border tile
				this.board_tiles[tileId][0] = '-';
			}
			else if ( col == this.num_cols )
			{
				// right border tile
				this.board_tiles[tileId][2] = '-';
			}
			if ( row == 1 )
			{
				// top border tile
				this.board_tiles[tileId][1] = '-';
			}
			else if ( row == this.num_rows )
			{
				// bottom border tile
				this.board_tiles[tileId][3] = '-';
			}
		}

		public void exportTileset()
		{
			List<string> export = new List<string>();
			this.board_tiles = new Dictionary<int, char[]>(this.num_cols * this.num_rows);
			for ( int pos = 1; pos <= this.num_pos; pos++ )
			{
				if ( this.board_squares[pos-1] != (char)0 )
				{
					int[] colrow = this.getColRowFromPos(pos);
					int col = colrow[0];
					int row = colrow[1];
					int tileId = 0;
					int offset = 0;
					char pattern;
					pattern = this.board_squares[pos-1];
					offset = (row-1) * this.num_cols;
					if ( col < this.num_cols )
					{
						// tiles 1 - this.num_cols LR

						// left tile
						// col = R
						tileId = offset + col;
						if ( !this.board_tiles.ContainsKey(tileId) )
						{
							// initialise blank tile as .... (for regex filter)
							this.board_tiles.Add(tileId, "....".ToCharArray());
						}
						this.setBorderFilter(tileId, col, row);
						// pattern goes into R cell
						this.board_tiles[tileId][2] = pattern;

						// right tile
						// col+1 = L
						tileId = offset + col+1;
						if ( !this.board_tiles.ContainsKey(tileId) )
						{
							// initialise blank tile as .... (for regex filter)
							this.board_tiles.Add(tileId, "....".ToCharArray());
						}
						this.setBorderFilter(tileId, col+1, row);
						// pattern goes into L cell
						this.board_tiles[tileId][0] = pattern;
					}
					else
					{
						// squares num_cols+1 to (num_cols + num_cols-1) (16-31)
						col -= this.num_cols - 1;
						// top tile
						tileId = offset + col;
						if ( !this.board_tiles.ContainsKey(tileId) )
						{
							// initialise blank tile as .... (for regex filter)
							this.board_tiles.Add(tileId, "....".ToCharArray());
						}
						// row = D
						this.setBorderFilter(tileId, col, row);
						// pattern goes into D cell
						this.board_tiles[tileId][3] = pattern;

						// bottom tile
						// row+1 = U
						tileId = offset + col + this.num_cols;
						if ( !this.board_tiles.ContainsKey(tileId) )
						{
							// initialise blank tile as .... (for regex filter)
							this.board_tiles.Add(tileId, "....".ToCharArray());
						}
						this.setBorderFilter(tileId, col, row+1);
						// pattern goes into U cell
						this.board_tiles[tileId][1] = pattern;
					}
				}
			}
			// export sorted by tileId
			List<int> keys = new List<int>(this.board_tiles.Keys);
			keys.Sort();
			foreach ( int tileId in keys )
			{
				export.Add(String.Format("{1}", tileId, new String(this.board_tiles[tileId])));
			}

			string tiles = String.Join("\r\n", export.ToArray());
			Program.TheMainForm.tbDesignLog.Text = tiles;
		}

		public void importTileset()
		{

		}

		// direction = 7,8,9,4,6,1,2,3 (nw,n,ne,w,e,sw,s,se)
		// performs a move using the above directions on the board, returns new pos #
		public int movePos(int startPos, string direction)
		{
			if ( !this.boardPosList.ContainsKey(startPos) )
			{
				return -1;
			}
			int newpos = -1;
			switch ( direction )
			{
				case "7":
					newpos = startPos - this.num_cols;
					break;
				case "8":
					newpos = startPos - ((this.num_cols*2)-1);
					break;
				case "9":
					newpos = startPos - (this.num_cols - 1);
					break;
				case "4":
					newpos = startPos - 1;
					break;
				case "6":
					newpos = startPos + 1;
					break;
				case "1":
					newpos = startPos + this.num_cols - 1;
					break;
				case "2":
					newpos = startPos + (this.num_cols*2)-1;
					break;
				case "3":
					newpos = startPos + this.num_cols;
					break;
			}
			if ( this.boardPosList.ContainsKey(newpos) )
			{
				return newpos;
			}
			else
			{
				return -1;
			}
		}

		public void drawLine(string direction, int start_pos, int end_pos)
		{
			// default to start_pos to end_pos
			int pos = start_pos;
			int dest = end_pos;
			if ( end_pos < start_pos )
			{
				pos = end_pos;
				dest = start_pos;
			}
			switch ( direction )
			{
				case "horizontal":
					while ( pos <= dest )
					{
						this.drawPatternAtPos(this.currentPattern, pos);
						pos++;
					}
					break;
				case "vertical":
					while ( pos <= dest )
					{
						this.drawPatternAtPos(this.currentPattern, pos);
						pos += (this.num_cols*2)-1;
					}
					break;
				case "diagonal":
					int cell_diff = Math.Abs(this.select_start_cell - this.select_end_cell);
					// if cell_diff is mod this.num_cols then its diagonal_down_right
					if ( cell_diff % this.num_cols == 0 )
					{
						while ( pos <= dest )
						{
							this.drawPatternAtPos(this.currentPattern, pos);
							pos += this.num_cols;
						}
					}
					// if cell_diff is mod this.num_cols - 1 then its diagonal_down_left
					else if ( cell_diff % (this.num_cols - 1) == 0 )
					{
						while ( pos <= dest )
						{
							this.drawPatternAtPos(this.currentPattern, pos);
							pos += this.num_cols - 1;
						}
					}
					break;
			}
		}

		public void fillLine()
		{
			switch ( this.action_step )
			{
				case 0:
					this.action = "fill_line";
					this.action_step = 1;
					Program.TheMainForm.statusInstructions.Text = "Fill Line. 1 / 2. Select FROM Cell.";
					break;
				case 1:
					// select start cell
					this.select_start_cell = this.current_pos;
					Program.TheMainForm.statusInstructions.Text = "Fill Line. 2 / 2. Select END cell.";
					this.action_step++;
					break;
				case 2:
					// get start cell
					int[] start_colrow = this.getColRowFromPos(this.select_start_cell);
					int start_col = start_colrow[0];
					int start_row = start_colrow[1];

					// select end cell
					this.select_end_cell = this.current_pos;
					int[] end_colrow = this.getColRowFromPos(this.select_end_cell);
					int end_col = end_colrow[0];
					int end_row = end_colrow[1];

					bool validLine = false;
					if ( this.select_start_cell == this.select_end_cell )
					{
						// same cell selected, abort
						this.action_step = 0;
						this.action = "";
						Program.TheMainForm.statusInstructions.Text = "Fill Line aborted - same cell selected.";
						break;
					}
					// check if horizontal line
					if ( start_col >= 1 && start_col <= this.num_cols - 1 && end_col >= 1 && end_col <= this.num_cols - 1 && start_row == end_row )
					{
						// 1st row
//						System.Windows.Forms.MessageBox.Show("horizontal line");
						this.drawLine("horizontal", this.select_start_cell, this.select_end_cell);
						validLine = true;
					}
					else if ( start_col > this.num_cols - 1 && end_col > this.num_cols - 1 && start_row == end_row )
					{
						// 2nd row
//						System.Windows.Forms.MessageBox.Show("horizontal line");
						this.drawLine("horizontal", this.select_start_cell, this.select_end_cell);
						validLine = true;
					}
					// check if vertical line
					else if ( start_col == end_col )
					{
//						System.Windows.Forms.MessageBox.Show("vertical line");
						this.drawLine("vertical", this.select_start_cell, this.select_end_cell);
						validLine = true;
					}
					else
					{
						// check if diagonal line
						// if cell_diff is mod this.num_cols then its diagonal_down_right
						// if cell_diff is mod this.num_cols - 1 then its diagonal_down_left
						int cell_diff = Math.Abs(this.select_start_cell - this.select_end_cell);
						if ( cell_diff % this.num_cols == 0 )
						{
//							System.Windows.Forms.MessageBox.Show("diagonal line - top-left to bottom-right");
							this.drawLine("diagonal", this.select_start_cell, this.select_end_cell);
							validLine = true;
						}
						else if ( cell_diff % (this.num_cols - 1) == 0 )
						{
//							System.Windows.Forms.MessageBox.Show("diagonal line - top-right to bottom-left");
							this.drawLine("diagonal", this.select_start_cell, this.select_end_cell);
							validLine = true;
						}
					}
					if ( validLine )
					{
						Program.TheMainForm.statusInstructions.Text = String.Format("Line Filled from cell {0} to {1}", this.select_start_cell, this.select_end_cell);
						this.action_step = 0;
						this.action = "";
					}
					else
					{
						Program.TheMainForm.statusInstructions.Text = "Fill Line. 2 / 2. Select END cell. No valid horizontal,vertical or diagonal line selected.";
					}
					break;
			}
		}

		private void setupCommands()
		{
			Program.TheMainForm.lbDesignCmd.Items.AddRange(new object[]
				{
               		"export_path",
               		"export_path_bgcolours",
               		"export_path_shapes",
               		"import_board",
               		"draw_shapes",
               		"tesselate"
				});
		}

		public void runCmd(string cmd)
		{
			List<string> export = new List<string>();
			switch ( cmd )
			{
				case "export_path":
				case "export_path_bgcolours":
					if ( cmd == "export_path" )
					{
						this.exportFilter("");
					}
					else if ( cmd == "export_path_bgcolours" )
					{
						this.exportFilter("bgcolour");
					}
					else if ( cmd == "export_path_shapes" )
					{
						this.exportFilter("shape");
					}
					foreach ( int pathId in this.board_tiles.Keys )
					{
						export.Add(pathId.ToString());
					}
					if ( Program.TheMainForm.solver == null )
					{
						Program.TheMainForm.initSolver(false);
					}
					Program.TheMainForm.selS1SolveMethod.Text = "Custom";
					Program.TheMainForm.solver.setSolveMethod("Custom", 0);
					Program.TheMainForm.inputS1SolvePath.Text = String.Join(",", export.ToArray());
					break;
				case "import_board":
					this.importBoard();
					break;
				case "draw_shapes":
					this.action = "draw_shapes";
					this.selectShape();
					break;
				case "tesselate":
					this.tesselate();
					break;
			}
		}

		private void importBoard()
		{
			if ( !this.isInitialised )
			{
				this.newDesign();
			}
			// gets patterns from board, just gets first pattern, doesnt check for valid joins etc
			int num_pos = (Board.num_cols * (Board.num_rows - 1)) + ((Board.num_cols-1) * Board.num_rows);
			for ( int pos = 1; pos <= num_pos; pos++ )
			{
				int[] colrow = this.getColRowFromPos(pos);
				int col = colrow[0];
				int row = colrow[1];
				int cellId = 0;
				int tileId = 0;
				int offset = 0;
				char pattern;
				offset = (row-1) * Board.num_cols;
				if ( col < Board.num_cols )
				{
					// check left tile
					// col = R
					cellId = offset + col;
					if ( Program.TheMainForm.board.tilepos[cellId-1] > 0 )
					{
						tileId = Program.TheMainForm.board.tilepos[cellId-1];
						pattern = Program.TheMainForm.board.tileset[tileId-1].pattern[2];
						this.drawPatternAtPos(pattern, pos);
					}
					else
					{
						// check right tile
						// col+1 = L
						cellId = offset + col+1;
						if ( Program.TheMainForm.board.tilepos[cellId-1] > 0 )
						{
							tileId = Program.TheMainForm.board.tilepos[cellId-1];
							pattern = Program.TheMainForm.board.tileset[tileId-1].pattern[0];
							this.drawPatternAtPos(pattern, pos);
						}
					}

				}
				else
				{
					// squares num_cols+1 to (num_cols + num_cols-1) (16-31)
					col -= Board.num_cols - 1;
					// check top tile D
					cellId = offset + col;
					if ( Program.TheMainForm.board.tilepos[cellId-1] > 0 )
					{
						tileId = Program.TheMainForm.board.tilepos[cellId-1];
						pattern = Program.TheMainForm.board.tileset[tileId-1].pattern[3];
						this.drawPatternAtPos(pattern, pos);
					}
					else
					{
						// check bottom tile
						// row+1 = U
						cellId = offset + col + Board.num_cols;
						if ( Program.TheMainForm.board.tilepos[cellId-1] > 0 )
						{
							tileId = Program.TheMainForm.board.tilepos[cellId-1];
							pattern = Program.TheMainForm.board.tileset[tileId-1].pattern[1];
							this.drawPatternAtPos(pattern, pos);
						}
					}
				}
			}
		}

		// functions to draw tesselated shapes - tetrominoes, pentominoes, hexominoes etc

		// select the shape to place
		// hover on board beneath mouse cursor
		// mouse wheel to rotate shape
		// right click to delete
		// left click to place
		public void selectShape()
		{
			if ( CAF_Application.config.contains("tesselation_designer_tile_shapes") )
			{
				this.shapeDesign = CAF_Application.config.getValue("tesselation_designer_tile_shapes");
				this.shapeSize = this.shapeDesign.Length + 1;
			}
			if ( this.action == "draw_shapes" && CAF_Application.config.contains("tesselation_designer_tile_fill") )
			{
				// get shape filler
				this.shapePatterns = CAF_Application.config.getValue("tesselation_designer_tile_fill");
			}
			else
			{
				// use currently selected pattern
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(this.currentPattern, this.shapeSize);
				this.shapePatterns = sb.ToString();
			}
		}

		// rotate selected shape clockwise or counter clockwise
		public void rotateShape(bool clockwise)
		{
			this.removeShapeHover();
			// rotate
			if ( this.shapeRotation == 0 && !clockwise )
			{
				this.shapeRotation = 3;
			}
			else if ( this.shapeRotation == 3 && clockwise )
			{
				this.shapeRotation = 0;
			}
			else if ( clockwise )
			{
				this.shapeRotation++;
			}
			else if ( !clockwise )
			{
				this.shapeRotation--;
			}
			this.drawShapeHover();
		}

		public string getRotatedDesign(int rotation)
		{
			char[] chars = this.shapeDesign.ToCharArray();
			for ( int r = 1; r <= rotation; r++ )
			{
				// rotates direction clockwise
				for ( int i = 0; i < this.shapeDesign.Length; i++ )
				{
					switch ( chars[i] )
					{
						case '7':
							chars[i] = '9';
							break;
						case '8':
							chars[i] = '6';
							break;
						case '9':
							chars[i] = '3';
							break;
						case '4':
							chars[i] = '8';
							break;
						case '6':
							chars[i] = '2';
							break;
						case '1':
							chars[i] = '7';
							break;
						case '2':
							chars[i] = '4';
							break;
						case '3':
							chars[i] = '1';
							break;
					}
				}
			}
			string rv = String.Join("", et2.Utils.CharListToStringList(chars));
			return rv;
		}

		public List<string> getShapesList()
		{
			// shape is defined using keypad as drawing directions. north = 8, south = 2, east = 6, west = 4, north west = 7, south west = 1 etc...
			if ( this.action == "draw_shapes" && CAF_Application.config.contains("tesselation_designer_tile_shapes") )
			{
				// get list of shape definitions
				List<string> shapes = new List<string>(CAF_Application.config.getValue("tesselation_designer_tile_shapes").Split(','));
				return shapes;
			}
			else if ( CAF_Application.config.contains("tesselation_solver_shapes") )
			{
				// get basic list of distinct shapes
				List<string> shapes = new List<string>(CAF_Application.config.getValue("tesselation_solver_shapes").Split(','));
				// add all 4 rotations for each shape into shape list
				HashSet<string> shapelist = new HashSet<string>(CAF_Application.config.getValue("tesselation_solver_shapes").Split(','));
				foreach ( string shape in shapes )
				{
					this.shapeDesign = shape;
					for ( int r = 1; r <= 4; r++ )
					{
						// add this rotation
						string newshape = this.getRotatedDesign(r);
						shapelist.Add(newshape);
						// add mirror image as well
						newshape = this.flipShape(newshape);
						shapelist.Add(newshape);
					}
				}
				return new List<string>(shapelist);
			}
			return null;
		}

		// draw shape for when hovering with mouse (lighter colour)
		public void drawShapeHover()
		{
			string hoverShapeDesign = this.getHoverDesign();
			if ( this.current_pos != this.hoverShapePos || hoverShapeDesign != this.hoverShapeDesign )
			{
				this.hoverShapeDesign = hoverShapeDesign;
				this.hoverShapePos = this.current_pos;
				int pos = this.hoverShapePos;
				for ( int i = 0; i <= this.hoverShapeDesign.Length; i++ )
				{
					// move if placing piece > 0
					if ( i > 0 )
					{
						string direction = this.hoverShapeDesign[i-1].ToString();
						pos = this.movePos(pos, direction);
					}
					char pattern = this.shapePatterns[i];
					// only draw if valid position
					if ( this.boardPosList.ContainsKey(pos) )
					{
						int[] colrow = this.boardPosList[pos];
						this.drawPatternAtXY(pattern, colrow[0], colrow[1]);
					}
				}
			}
		}

		public void removeShapeHover()
		{
			// remove previous shape that was displayed while hovering
			int pos = this.hoverShapePos;
			for ( int i = 0; i <= this.hoverShapeDesign.Length; i++ )
			{
				// move if placing piece > 0
				if ( i > 0 )
				{
					string direction = this.hoverShapeDesign[i-1].ToString();
					pos = this.movePos(pos, direction);
				}
				if ( this.boardPosList.ContainsKey(pos) )
				{
					// redraw pattern if exists
					if ( this.board_squares[pos-1] != (char)0 )
					{
						int[] colrow = this.boardPosList[pos];
						char pattern = this.board_squares[pos-1];
						this.drawPatternAtXY(pattern, colrow[0], colrow[1]);
					}
					else
					{
						// else draw blank cell
						this.redrawCell(pos);
					}
				}
			}
		}

		// place shape on board
		public void placeShape()
		{
			string drawShapeDesign = this.getHoverDesign();
			int pos = this.current_pos;
			this.placeShapeOnBoard(drawShapeDesign, pos);
		}

		public Dictionary<int, char> getShapePlotPoints(string shape, int pos)
		{
			Dictionary<int, char> plotPoints = new Dictionary<int, char>();
			for ( int i = 0; i <= shape.Length; i++ )
			{
				// move if placing piece > 0
				if ( i > 0 )
				{
					string direction = shape[i-1].ToString();
					pos = this.movePos(pos, direction);
				}
				char pattern = this.shapePatterns[i];
				// only place shape on board if valid position and no other piece occupies the space
				if ( this.boardPosList.ContainsKey(pos) && this.board_squares[pos-1] == (char)0 )
				{
					plotPoints.Add(pos, pattern);
				}
				else
				{
					return new Dictionary<int, char>();
				}
			}
			return plotPoints;
		}

		public bool placeShapeOnBoard(string shape, int pos)
		{
			// only draw whole shape
			Dictionary<int, char> plotPoints = this.getShapePlotPoints(shape, pos);
			foreach ( int cellId in plotPoints.Keys )
			{
				this.drawPatternAtPos(plotPoints[cellId], cellId);
			}
			return true;
		}

		public void flipShapeHover()
		{
			this.shapeFlipped = !this.shapeFlipped;
		}

		public string flipShape(string shape)
		{
			// flip design (mirror)
			char[] chars = shape.ToCharArray();
			for ( int i = 0; i < shape.Length; i++ )
			{
				switch ( chars[i] )
				{
					case '7':
						chars[i] = '9';
						break;
					case '9':
						chars[i] = '7';
						break;
					case '4':
						chars[i] = '6';
						break;
					case '6':
						chars[i] = '4';
						break;
					case '1':
						chars[i] = '3';
						break;
					case '3':
						chars[i] = '1';
						break;
				}
			}
			return String.Join("", et2.Utils.CharListToStringList(chars));
		}

		public string getHoverDesign()
		{
			string rv = this.getRotatedDesign(this.shapeRotation);
			if ( this.shapeFlipped )
			{
				rv = this.flipShape(rv);
			}
			return rv;
		}

		public string getShapeFiller(char pattern)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(pattern, this.shapeSize);
			return sb.ToString();
		}

		// attempt to fill the board by tesselating it with specified shapes
		public void tesselate()
		{
			// starting cellId
			int cellId = 0;

			// list of shapes to try and tesselate the board with (contains all rotations & mirrors)
			List<string> shapes = this.getShapesList();

			// shape filler
			string shapeFillPatterns = this.currentPattern.ToString();
			if ( CAF_Application.config.contains("tesselation_solver_tile_fill") )
			{
				shapeFillPatterns = CAF_Application.config.getValue("tesselation_solver_tile_fill");
			}

			// queue : cellId : shapeNum
			Dictionary<int,int> queue = new Dictionary<int,int>();
			bool done = false;
			int numPlaced = 0;

			while ( ! done )
			{
				// get next available cellId
				cellId = this.getNextCellId(cellId);
				if ( cellId > 0 )
				{
					// try each shape in the list
					foreach ( string shape in shapes )
					{
						this.shapeSize = shape.Length + 1;
						// get shape fill pattern
						this.shapePatterns = this.getShapeFiller(shapeFillPatterns[numPlaced % shapeFillPatterns.Length]);

						// check that shape doesn't touch border
						bool onBorder = false;
						Dictionary<int, char> plotPoints = this.getShapePlotPoints(shape, cellId);
						foreach ( int pos in plotPoints.Keys )
						{
							if ( this.isBorderCell(pos) )
							{
								onBorder = true;
								break;
							}
						}
						if ( !onBorder && plotPoints.Count > 0 && this.placeShapeOnBoard(shape, cellId) )
						{
							numPlaced++;
							Program.TheMainForm.statusInstructions.Text = numPlaced + " shapes placed";
							break;
						}
					}
				}
				else
				{
					done = true;
				}
			}
		}

		public int getNextCellId(int lastCellId)
		{
			// exclude borders & occupied cells
			// return 0 if no more available cells
			for ( int i = lastCellId + 1; i <= this.num_pos; i++ )
			{
				if ( this.board_squares[i-1] == (char)0 && !this.isBorderCell(i) )
				{
					return i;
				}
			}
			return 0;
		}

		public string getNextShape()
		{
			return "";
		}

		public bool isBorderCell(int cellId)
		{
			int width = this.num_cols + this.num_cols - 1;
			// top row
			if ( cellId >= 1 && cellId <= this.num_cols - 1 )
			{
				return true;
			}
			// left col
			else if ( cellId % width == this.num_cols )
			{
				return true;
			}
			// right col
			else if ( cellId % width == 0 )
			{
				return true;
			}
			// bottom row
			else if ( cellId >= this.num_pos - (this.num_cols - 1) && cellId <= this.num_pos )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}

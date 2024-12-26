/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 24/11/2009
 * Time: 12:07 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ET2Solver
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class TileGraphics
	{
		// size of source pattern images
		int sourcePatternWidth = 109;
		int sourcePatternHeight = 109;

		public Image image = null;

		public TileGraphics(string pattern)
		{
			this.createTileImage(pattern);
		}

		public Image createBlankTile(int width, int height)
		{
			Image bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			// create Graphics object
			Graphics gimage = Graphics.FromImage(bitmap);

			// edge background = 112,112,112 = #707070
			Brush brush = new SolidBrush(Color.FromArgb(112,112,112));
			gimage.FillRectangle(brush, 0, 0, width, height);

			return bitmap;
		}

		public Image placeTilePattern(Image bitmap, char pattern, int pos)
		{
			Image patternImage = null;
			// load source pattern
			try
			{
				patternImage = (Image)Program.TheMainForm.board.patternImages[pattern.ToString()];
			}
			catch
			{
				Program.TheMainForm.log("Error loading pattern: " + pattern);
				return bitmap;
			}

			// create texture brush from source pattern
			TextureBrush tb = new TextureBrush(patternImage);

			// create Graphics object
			Graphics gimage = Graphics.FromImage(bitmap);

			// pos = 0:left, 1:top, 2:right, 3:bottom
			switch ( pos )
			{
				case 0:
					// rotate transform onto left
					gimage.TranslateTransform((float)bitmap.Width/2, (float)bitmap.Height/2);
					gimage.RotateTransform(-90);
					gimage.TranslateTransform(-(float)bitmap.Width/2, -(float)bitmap.Height);
					break;
				case 1:
					// rotate transform onto top
					gimage.TranslateTransform((float)bitmap.Width/2, (float)bitmap.Height/2);
					gimage.TranslateTransform(-(float)bitmap.Width/2, -(float)bitmap.Height);
					break;
				case 2:
					// rotate transform onto right
					gimage.TranslateTransform((float)bitmap.Width/2, (float)bitmap.Height/2);
					gimage.RotateTransform(90);
					gimage.TranslateTransform(-(float)bitmap.Width/2, -(float)bitmap.Height);
					break;
				case 3:
					// rotate transform onto bottom
					gimage.TranslateTransform((float)bitmap.Width/2, (float)bitmap.Height/2);
					gimage.RotateTransform(180);
					gimage.TranslateTransform(-(float)bitmap.Width/2, -(float)bitmap.Height);
					break;
			}

			// copy pattern to bitmap (from top of pattern)
			Point[] polygonPoints = new Point[5];
			polygonPoints[0] = new Point(0,55);
			polygonPoints[1] = new Point(108,55);
			polygonPoints[2] = new Point(55,108);
			polygonPoints[3] = new Point(53,108);
			polygonPoints[4] = new Point(0,55);
			gimage.FillPolygon(tb, polygonPoints);
			return bitmap;
		}

		public void createTileImage(string pattern)
		{
			//Image tile = this.createBlankTile(this.cellWidth, this.cellHeight);
			//return tile;

			if ( pattern.Length == 4 )
			{
				Image tile = this.createBlankTile(this.sourcePatternWidth, this.sourcePatternHeight);

				string pID = "";
				for ( int i = 0; i < 4; i++ )
				{
					pID = pattern[i].ToString();
					if ( Program.TheMainForm.board.patterns.Contains(pID) )
					{
						tile = this.placeTilePattern(tile, pattern[i], i);
					}
				}

				// draw black dividing lines
				Graphics g = Graphics.FromImage(tile);
				Pen pen = new Pen(Color.Black, 2.0f);
				g.DrawLine(pen, 0, 0, this.sourcePatternWidth, this.sourcePatternHeight);
				g.DrawLine(pen, 0, this.sourcePatternHeight, this.sourcePatternWidth, 0);

				// resize to fit board cells
				Image btile = this.createBlankTile(Program.TheMainForm.board.cellWidth+16, Program.TheMainForm.board.cellHeight+16);
				g = Graphics.FromImage(btile);
				g.DrawImage(tile, 0, 0, Program.TheMainForm.board.cellWidth+16, Program.TheMainForm.board.cellHeight+16);

				this.image = btile;
			}
			else
			{
				Program.TheMainForm.log("Error creating tile image, pattern length " + pattern.Length + " != 4");
			}
		}

	}
}

/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 23/11/2009
 * Time: 11:27 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace ET2Solver
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class Tile
	{
		public int id = 0;
		public string pattern = "";
		public string[] patterns = null;
		public int rotation = 1;
		public int col = 0;
		public int row = 0;

		// graphic properties
		public TileGraphics gtile = null;
		public Image image = null;
		
		public Tile(int id, string pattern, int rotation)
		{
			this.id = id;
			this.generatePatterns(pattern);
			this.rotate(rotation);
			//Program.TheMainForm.log("creating tileId: " + id + ", pattern: " + pattern + ", len = " + pattern.Length);
		}
		
		public void generatePatterns(string startingPattern)
		{
			//Program.TheMainForm.log("generatePatterns(" + startingPattern + ")");
	        // creates all pattern rotations, in clockwise sequence
	        // LTRB -> BLTR -> RBLT -> TRBL
	        this.patterns = new string[4];
	        string pattern = startingPattern;
	        this.patterns[0] = pattern;
	        for ( int i = 2; i <= 4; i++ )
	        {
	        	pattern = pattern.Substring(pattern.Length-1) + pattern.Substring(0, 3);
	        	this.patterns[i-1] = pattern;
	        	//Program.TheMainForm.log("generate pattern " + i + ": " + pattern);
	        }

		}
		
		public void updateImage()
		{
			//Program.TheMainForm.log("updateImage(tile: " + this.id + ", pattern: " + this.pattern);
			if ( this.gtile != null )
			{
				this.gtile.createTileImage(this.pattern);
			}
			else
			{
				this.gtile = new TileGraphics(this.pattern);
			}
			this.image = this.gtile.image;
		}
        
    	public void rotate(int rotation)
		{
    		if ( rotation == - 1 )
    		{
    			// leave as is
    			return;
    		}
	        // rotation = 1-4
	        if ( rotation == 0 )
	        {
	        	if ( this.rotation < 4 )
	        	{
	                this.rotation += 1;
	        	}
	            else
	            {
	                this.rotation = 1;
	            }
	            this.pattern = this.patterns[this.rotation-1];
	        }
	        else
	        {
	            this.pattern = this.patterns[rotation-1];
	            this.rotation = rotation;
	        }
	        //Program.TheMainForm.log("Rotate tile: " + this.id + ", rotation: " + rotation + ", pattern: " + this.pattern);
	        if ( Program.TheMainForm.useTileGraphics )
	        {
		        this.updateImage();
	        }
		}

		public string title()
		{
			/*
			#123:R1:--AB @ 0,0
			#123:R1:--AB @ 0,0
			#123:R1:--AB @ 0,0

			# 123 R1 --AB @ 0,0
			# 123 R1 --AB @ 0,0
			# 123 R1 --AB @ 0,0

			123 R 1 --AB @ 0,0
			123 R 1 --AB @ 0,0
			123 R 1 --AB @ 0,0

			123 1 --AB @ 0,0
			123 2 --AB @ 0,0
			123 3 --AB @ 0,0
			 */
			return String.Format("{0:000}", this.id) + " " + this.rotation + " " + this.pattern + " " + this.col + "," + this.row;
		}
		
		public bool moveTo(int col, int row)
		{
			this.col = col;
			this.row = row;
			// TODO call BoardGraphics to move tile image
			// check if tile matches adjacently
			return true;
		}
		
		public char left()
		{
	        return this.pattern[0];
		}
	        
	    public char up()
	    {
	        return this.pattern[1];
	    }
	
		public char right()
		{
	        return this.pattern[2];
		}
	
		public char down()
		{
	        return this.pattern[3];
		}

		// check if tile matches with tile to the left
		public bool matchLeft(Tile tile)
		{
			//Program.TheMainForm.log("matchLeft(" + this.title() + ":" + this.left() + "==" + tile.title() + ":" + tile.right() + ")");
			return tile.right() == this.left();
		}

		// check if tile matches with tile above
		public bool matchUp(Tile tile)
		{
			//Program.TheMainForm.log("matchUp(" + this.title() + ":" + this.up() + "==" + tile.title() + ":" + tile.down() + ")");
			return tile.down() == this.up();
		}

		// check if tile matches with tile to the right
		public bool matchRight(Tile tile)
		{
			//Program.TheMainForm.log("matchRight(" + this.title() + ":" + this.right() + "==" + tile.title() + ":" + tile.left() + ")");
			return tile.left() == this.right();
		}

		// check if tile matches with tile below
		public bool matchDown(Tile tile)
		{
			//Program.TheMainForm.log("matchDown(" + this.title() + ":" + this.down() + "==" + tile.title() + ":" + tile.up() + ")");
			return tile.up() == this.down();
		}
		
	}

}
/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 28/11/2009
 * Time: 9:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using et2;

namespace ET2Solver
{
	/// <summary>
	/// Description of RandomBoard.
	/// </summary>
	public class RandomBoard
	{
		public bool debug_log = false;
		
		public bool useRandom = true;
		public System.Random r = new Random();
		public int seed;
		public Dictionary<char, int> patternBank = new Dictionary<char, int>();
		public List<char> edgePatterns = new List<char>();
		public List<char> internalPatterns = new List<char>();
		public string[] tileset = new String[Board.max_tiles];
		public string result = "";

		public int numHalvesPerEdge = 12;
		public int numHalvesPerInner1 = 24;
		public int numHalvesPerInner2 = 25;
		public int numHalvesPerInner3 = 0;
		public int borderSize = 4 * ((Board.num_cols - 1)+(Board.num_rows - 1));
		public int internalSize = 4 * (Board.num_cols - 2) * (Board.num_rows - 2) + 2 * (Board.num_cols - 2) + 2 * (Board.num_rows - 2);
		public int numPatterns = Board.num_inner1 + Board.num_inner2 + Board.num_inner3;
		
		public string layout = "";
		public bool patternBankCreated = false;
		public Dictionary<char, int> backup_patternBank = new Dictionary<char, int>();
		public List<char> backup_edgePatterns = new List<char>();
		public List<char> backup_internalPatterns = new List<char>();
		
		public RandomBoard()
		{
		}

		public int getSeed()
		{
//			int unixtime = (int)(DateTime.UtcNow - new DateTime(1970,1,1,0,0,0)).TotalSeconds;
//			this.seed = unixtime;
//			return this.seed;

	        // 2-pass random generator to generate more balanced variety of random numbers... 
	        // 1st pass determines number range
	        // 2nd pass gets random number from that range
	        List<int> negnums = new List<int>();
	        negnums.Add(-1024);
	        negnums.Add(-65536);
	        negnums.Add(0);
	        negnums.Add(-16384);
	        negnums.Add(-2147483647);
	        negnums.Add(-32768);
	        negnums.Add(0);
	        negnums.Add(-8388608);
	        negnums.Add(0);
	        negnums.Add(-1048576);
	        negnums.Add(-256);
	        negnums.Add(0);
	        negnums.Add(-4194304);
	        negnums.Add(-8192);
	        negnums.Add(0);
	        negnums.Add(-16777216);
	        int minvalue = negnums[this.randomNumber(negnums.Count)];

	        List<int> posnums = new List<int>();
	        posnums.Add(256);
	        posnums.Add(4194304);
	        posnums.Add(8192);
	        posnums.Add(1048576);
	        posnums.Add(16384);
	        posnums.Add(2147483647);
	        posnums.Add(1024);
	        posnums.Add(32768);
	        posnums.Add(65536);
	        posnums.Add(8388608);
	        posnums.Add(16777216);
	        posnums.Add(64);
	        int maxvalue = posnums[this.randomNumber(posnums.Count)];

			string sticks = DateTime.Now.Ticks.ToString();
			int nticks = Convert.ToInt32(sticks.Substring(sticks.Length-12, 9));
			Random r = new Random(nticks);
	        return r.Next(minvalue, maxvalue);
		}
		
		public int randomNumber(int max)
		{
			string sticks = DateTime.Now.Ticks.ToString();
			int nticks = Convert.ToInt32(sticks.Substring(sticks.Length-12, 9));
			Random r = new Random(nticks);
			return r.Next(0, max);
		}
		
		public bool createPatternList()
		{
			// only need to create pattern list once per board layout (save time when called multiple times)
			if ( this.layout == Board.getLayout() && this.patternBankCreated )
			{
				this.patternBank = new Dictionary<char, int>(this.backup_patternBank);
				this.edgePatterns = new List<char>(this.backup_edgePatterns);
				this.internalPatterns = new List<char>(this.backup_internalPatterns);
				return true;
			}
			this.patternBank = new Dictionary<char, int>();
			// cols,rows,numEdges,numInner1,numInner1Halves,numInner2,numInner2Halves,numInner3,numInner3Halves
			// 16x16x5x5x240x12x600x0x0 E2 X256 Prize
			// 6x6x4x1x8x2x34x2x38 E2 X36 Clue 1&3
			// 12x6x4x2x148x2x40x0x0 E2 X72 Clue 2&4
			
			// defaults
			this.numHalvesPerEdge = 12;
			this.numHalvesPerInner1 = 24;
			this.numHalvesPerInner2 = 25;
			this.numHalvesPerInner3 = 0;
			this.borderSize = 4 * ((Board.num_cols - 1)+(Board.num_rows - 1));
			this.internalSize = 4 * (Board.num_cols - 2) * (Board.num_rows - 2) + 2 * (Board.num_cols - 2) + 2 * (Board.num_rows - 2);
			this.numPatterns = Board.num_inner1 + Board.num_inner2 + Board.num_inner3;

			// define edges
			if ( Board.num_edges > 0 )
			{
				this.numHalvesPerEdge = this.borderSize / Board.num_edges;
			}
			// define inner 1
			if ( Board.num_inner1 > 0 && Board.num_inner1h > Board.num_inner1 )
			{
				this.numHalvesPerInner1 = Board.num_inner1h / Board.num_inner1;
			}
			// define inner 2
			if ( Board.num_inner2 > 0 && Board.num_inner2h > Board.num_inner2 )
			{
				this.numHalvesPerInner2 = Board.num_inner2h / Board.num_inner2;
			}
			// define inner 3
			if ( Board.num_inner3 > 0 && Board.num_inner3h > Board.num_inner3 )
			{
				this.numHalvesPerInner3 = Board.num_inner3h / Board.num_inner3;
			}
			
			this.logBoardLayout();

			// check if layout is valid
			if ( (Board.num_edges + this.numPatterns) > 26 )
			{
				System.Windows.Forms.MessageBox.Show("Invalid board layout - cannot create board. Too many patterns (" + (Board.num_edges + this.numPatterns) + " > 26)");
				return false;
			}
			int layoutSize = Board.num_edges * this.numHalvesPerEdge + Board.num_inner1 * this.numHalvesPerInner1 + Board.num_inner2 * this.numHalvesPerInner2 + Board.num_inner3 * this.numHalvesPerInner3;
			if ( this.borderSize + this.internalSize != layoutSize )
			{
				System.Windows.Forms.MessageBox.Show("Invalid board layout - cannot create board. Pattern count mismatch. " + layoutSize + " != " + (borderSize + internalSize));
				return false;
			}
			
			// define pattern configuration
			int minPID = Convert.ToInt16('A');

			// edges A-E
			for ( int p = 0; p < Board.num_edges; p++ )
			{
				char pattern = Convert.ToChar(minPID + p);
				this.patternBank.Add(pattern, numHalvesPerEdge);
			}

			// inner1
			minPID += Board.num_edges;
			for ( int p = 0; p < Board.num_inner1; p++ )
			{
				char pattern = Convert.ToChar(minPID + p);
				this.patternBank.Add(pattern, numHalvesPerInner1);
			}

			// inner2
			minPID += Board.num_inner1;
			for ( int p = 0; p < Board.num_inner2; p++ )
			{
				char pattern = Convert.ToChar(minPID + p);
				this.patternBank.Add(pattern, numHalvesPerInner2);
			}

			// inner3
			minPID += Board.num_inner2;
			for ( int p = 0; p < Board.num_inner3; p++ )
			{
				char pattern = Convert.ToChar(minPID + p);
				this.patternBank.Add(pattern, numHalvesPerInner3);
			}
			
			// internal edge pattern distribution
			// tile 8,9 - piece 139

			this.edgePatterns = new System.Collections.Generic.List<char>();
			for ( int i = 0; i < Board.num_edges; i++ )
			{
				char pattern = Char.ConvertFromUtf32(65 + i).ToCharArray()[0];
				this.edgePatterns.Add(pattern);
			}
			
			this.internalPatterns = new System.Collections.Generic.List<char>();
			for ( int i = Board.num_edges; i < Board.num_edges + numPatterns; i++ )
			{
				char pattern = Char.ConvertFromUtf32(65 + i).ToCharArray()[0];
				this.internalPatterns.Add(pattern);
			}
        	//Program.TheMainForm.log("patternlist size: edgePatterns = " + this.edgePatterns.Count.ToString() + ", internalPatterns = " + this.internalPatterns.Count.ToString());
			
			// randomise the list
			/*
			if ( this.useRandom )
			{
				for ( int i = 0; i < this.edgePatterns.Count; i++ )
				{
					int pos = this.r.Next(this.edgePatterns.Count);
					char pa = this.edgePatterns[i];
					char pb = this.edgePatterns[pos];
					this.edgePatterns[i] = pb;
					this.edgePatterns[pos] = pa;
				}
				
				for ( int i = 0; i < this.internalPatterns.Count; i++ )
				{
					int pos = this.r.Next(this.internalPatterns.Count);
					char pa = this.internalPatterns[i];
					char pb = this.internalPatterns[pos];
					this.internalPatterns[i] = pb;
					this.internalPatterns[pos] = pa;
				}
			}
			*/

			// backup pattern layout for multiple re-uses
			this.layout = Board.getLayout();
			this.patternBankCreated = true;
			this.backup_patternBank = new Dictionary<char, int>(this.patternBank);
			this.backup_edgePatterns = new List<char>(this.edgePatterns);
			this.backup_internalPatterns = new List<char>(this.internalPatterns);

			return true;
		}
		
		public char fetchPattern(bool isBorder)
		{
			char pattern = '-';
			int pos = 0;
            if ( isBorder && this.edgePatterns.Count > 0 )
            {
				if ( this.useRandom )
				{
					pos = this.r.Next(this.edgePatterns.Count);
				}
				if ( pos > - 1 )
				{
	            	pattern = (char)this.edgePatterns[pos];
	            	this.patternBank[pattern]--;
	            	this.patternBank[pattern]--;
	            	if ( this.patternBank[pattern] == 0 )
	            	{
	            		this.edgePatterns.RemoveAt(pos);
	            	}
				}
            }
            else if ( !isBorder && this.internalPatterns.Count > 0 )
            {
				if ( this.useRandom )
				{
					pos = this.r.Next(this.internalPatterns.Count);
				}
				if ( pos > - 1 )
				{
	            	pattern = (char)this.internalPatterns[pos];
	            	this.patternBank[pattern]--;
	            	this.patternBank[pattern]--;
	            	if ( this.patternBank[pattern] == 0 )
	            	{
	            		this.internalPatterns.RemoveAt(pos);
	            	}
				}
            }
            return pattern;
		}
			
		public string createNewTileset(Int32 seed)
		{
			if ( Program.TheMainForm.cbVerboseCreateBoardLog.Checked )
			{
				this.debug_log = true;
			}
			this.seed = seed;
			this.r = new Random(seed);
			if ( !this.createPatternList() )
			{
				return "";
			}
			this.logPatternCount();
			this.tileset = new string[Board.max_tiles];
			
	        int numTiles = 0;
	        char pattern;
	        
	        // handle first series of squares horizontally L/R
			for ( int row = 1; row <= Board.num_rows; row++ )
			{
				for ( int col = 1; col <= Board.num_cols - 1; col++ )
				{
	                // get pattern
	                bool isBorder = this.isBorderCell(col, row, 'h');
	                pattern = this.fetchPattern(isBorder);
	                //Program.TheMainForm.log(col + "," + row + " isBorder: " + isBorder.ToString() + ", pattern: " + pattern);
	
	                this.placePattern(col, row, 'R', pattern);
	                this.placePattern(col+1, row,'L', pattern);
	                numTiles += 1;
				}
			}

	        // handle second series of squares vertically U/D
			for ( int row = 1; row <= Board.num_rows - 1; row++ )
			{
				for ( int col = 1; col <= Board.num_cols; col++ )
				{
	                // get pattern
	                bool isBorder = this.isBorderCell(col, row, 'v');
	                pattern = this.fetchPattern(isBorder);
	                //Program.TheMainForm.log(col + "," + row + " isBorder: " + isBorder.ToString() + ", pattern: " + pattern);
	
	                this.placePattern(col, row, 'D', pattern);
	                this.placePattern(col, row+1,'U', pattern);
	                numTiles += 1;
				}
			}
			
			// check if all tiles are unique
			int numUniqueTiles = Program.TheMainForm.board.countNumUniqueTiles(this.tileset);
			string isUnique = "";
			if ( numUniqueTiles == this.tileset.Length * 4 )
			{
				isUnique = "UNIQUE ";
			}
			Board.seed = seed;
			Board.setTitle();
			this.result = isUnique + "Board " + Board.title + " generated with " + numUniqueTiles + " / " + this.tileset.Length * 4 + " unique rotations using " + Board.max_tiles + " tile(s), hash = " + Utils.getSHA1(String.Join("\r\n", this.tileset).Trim());
			this.logPatternCount();

			//Program.TheMainForm.log("generated " + numTiles + " patterns");
			return String.Join("\r\n", this.tileset);
		}
		
	    public void placePattern(int col, int row, char loc, char pattern)
	    {
	    	//Program.TheMainForm.log("placePattern(" + col.ToString() + "," + row.ToString() + "," + loc.ToString() + "," + pattern.ToString() + ")");
	        int tile_id = (row - 1) * Board.num_cols + col;
	
	        string borderType = this.getBorderType(col, row);
	        string tilemap;
	        int pos = 0;
	        if ( this.tileset[tile_id-1] == "" || this.tileset[tile_id-1] == null )
	        {
	            tilemap = this.getTileBorder(borderType);
	        }
	        else
	        {
	            tilemap = this.tileset[tile_id-1];
	        }
	
	        switch ( loc )
	        {
	        	case 'L':
	        		pos = 0;
	        		break;
	        	case 'U':
	        		pos = 1;
	        		break;
	        	case 'R':
	        		pos = 2;
	        		break;
	        	case 'D':
	        		pos = 3;
	        		break;
	        }
	        //Program.TheMainForm.log(tile_id + "," + tilemap + "," + tilemap.Length);
	        tilemap = tilemap.Remove(pos, 1);
	        tilemap = tilemap.Insert(pos, pattern.ToString());
	        this.tileset[tile_id-1] = tilemap;
	        //Program.TheMainForm.log(tile_id + "," + tilemap + "," + tilemap.Length);
	        /*
	        if ( tilemap.Length != 4 )
	        {
	        	Program.TheMainForm.log("error creating pattern for tile " + tile_id + " " + tilemap + " in " + col + "," + row);
	        }
	        */
	    }
	            	
		public bool isBorderCell(int col, int row, char loc)
	    {
	        bool rv = false;
	        if ( (col == 1 || col == Board.num_cols) && loc == 'v' )
			{
				rv = true;
			}
	        else if ( (row == 1 || row == Board.num_rows) && loc == 'h' )
	        {
				rv = true;
	        }
	       	return rv;
	    }
		
		public string getBorderType(int col, int row)
		{
			string rv = "";
	        // return tile type - corner/edge/internal
	        if ( col == 1 && row == 1 )
	        {
	            rv = "corner_top_left";
	        }
	        else if ( col == Board.num_cols && row == 1 )
	        {
	        	rv = "corner_top_right";
	        }
	        else if ( col == 1 && row == Board.num_rows )
	        {
	            rv = "corner_bottom_left";
	        }
	        else if ( col == Board.num_cols && row == Board.num_rows )
	        {
	            rv = "corner_bottom_right";
	        }
	        else if ( col == 1 )
	        {
	            rv = "edge_left";
	        }
	        else if ( col == Board.num_cols )
	        {
	            rv = "edge_right";
	        }
	        else if ( row == 1 )
	        {
	            rv = "edge_top";
	        }
	        else if ( row == Board.num_rows )
	        {
	            rv = "edge_bottom";
	        }
	        else
	        {
	            rv = "internal";
	        }
	        return rv;
		}
	
	    public string getTileBorder(string id)
	    {
			/*
			# LTRB
			# - is edge/border
			# ? = to be replaced by pattern filler
			*/
			System.Collections.Hashtable borderTypes = new System.Collections.Hashtable();
			borderTypes.Add("corner_top_left", "--??");
			borderTypes.Add("corner_top_right", "?--?");
			borderTypes.Add("corner_bottom_left", "-??-");
			borderTypes.Add("corner_bottom_right", "??--");
			borderTypes.Add("edge_top", "?-??");
			borderTypes.Add("edge_bottom", "???-");
			borderTypes.Add("edge_left", "-???");
			borderTypes.Add("edge_right", "??-?");
			borderTypes.Add("internal", "????");
			return (string)borderTypes[id];
	    }
	    
	    public void logBoardLayout()
	    {
	    	if ( !this.debug_log )
	    	{
	    		return;
	    	}
			Program.TheMainForm.log("Board.num_edges = " + Board.num_edges);
			Program.TheMainForm.log("Board.num_inner1 = " + Board.num_inner1);
			Program.TheMainForm.log("Board.num_inner2 = " + Board.num_inner2);
			Program.TheMainForm.log("Board.num_inner3 = " + Board.num_inner3);
			Program.TheMainForm.log("numPatterns = " + this.numPatterns);
			Program.TheMainForm.log("numHalvesPerEdge = " + this.numHalvesPerEdge);
			Program.TheMainForm.log("numHalvesPerInner1 = " + this.numHalvesPerInner1);
			Program.TheMainForm.log("numHalvesPerInner2 = " + this.numHalvesPerInner2);
			Program.TheMainForm.log("numHalvesPerInner3 = " + this.numHalvesPerInner3);
	    }
	    
	    public void logPatternCount()
	    {
	    	if ( !this.debug_log )
	    	{
	    		return;
	    	}
			// check pattern counts
			Program.TheMainForm.log("Pattern Count");
			Program.TheMainForm.log("edgePatterns.Count = " + this.edgePatterns.Count);
			Program.TheMainForm.log("internalPatterns.Count = " + this.internalPatterns.Count);
			foreach ( char p in this.patternBank.Keys )
			{
				Program.TheMainForm.log(p + " = " + this.patternBank[p]);
			}
	    }

	}
}

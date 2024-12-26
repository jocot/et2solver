/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 8/11/2009
 * Time: 10:34 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using CAF;

namespace ET2Solver
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public string default_board = "16x16x5x17_e2";

		public bool useTileGraphics = true;
		public Timer timer = new Timer();
		public Board board = null;
		public Solver solver = null;
		public Solver2x2 solver2 = null;
		public string model_name = "";
		public string hints_name = "";
		public string logtext = "";

		// interactive actions
		public string action = "";
		public int action_step = 0;

		// select path
		public int select_start_col = 0;
		public int select_start_row = 0;
		public int select_end_col = 0;
		public int select_end_row = 0;

		public TileSelector ts;
		public TileSwapUI tsui;

		// move tiles
		public int move_tile_id = 0;
		public int[] move_tile_pos = new int[0];

		// cell pos
		public int col = 0;
		public int row = 0;

		// nav path when building paths
		public List<string> nav_path = new List<string>();

		// board designer instance
		public BoardDesigner design = null;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			this.loadConfig();
			this.defineLayouts();
			this.board = new Board();
			this.loadTileSetList();
		}

		// destructor
		~ MainForm()
		{
		}

		private void loadConfig()
		{
			// load default configuration (if set)
			CAF_Application.config.loadDefaults();

			tbConfig.Text = CAF_Application.config.getAsText();
			tbConfig.Update();

			if ( CAF_Application.config.filename != "" )
			{
				string timestamp = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt : ");
				this.statusInstructions.Text = timestamp + "loaded " + CAF_Application.config.filename;
			}
		}

		// apply any interactive configuration options after loading them
		// eg. pre-loading models etc
		public void applyConfiguration()
		{
			if ( CAF_Application.config.contains("load_model") )
			{
				this.board.loadModel(CAF_Application.config.getValue("load_model"));
				Program.TheMainForm.useTileGraphics = true;
				this.board.drawTiles();
			}

			if ( CAF_Application.config.contains("imageset") )
            {
                this.board.loadPatternImages(true);
            }
		}

		public void defineLayouts()
		{
			// load & define board layouts - cols,cows,numEdges,numInner1,numInner2
			this.selBoardLayout.Items.Clear();
			List<string> layouts = new List<string>();
			try
			{
				layouts = new List<string>(System.IO.File.ReadAllLines("config\\config-layouts.txt"));
			}
			catch
			{
				this.log("Creating default layouts file: config-layouts.txt");
			}
			if ( layouts.Count == 0 )
			{
				// create default layouts file
				layouts = new List<string>();
				layouts.Add("16x16x5x5x240x12x600x0x0 E2 X256 Prize");
				layouts.Add("6x6x4x1x8x1x34x1x38 E2 X36 Clue 1&3");
				layouts.Add("12x6x4x2x148x2x40x0x0 E2 X72 Clue 2&4");
				try
				{
					System.IO.File.WriteAllLines("config\\config-layouts.txt", layouts.ToArray());
				}
				catch
				{
					this.log("Error creating default layouts file: config\\config-layouts.txt");
				}
			}
			else
			{
				int ir = 0;
				for ( int i = 0; i < layouts.Count; i++ )
				{
					// strip comments
					string line = layouts[i];
					if ( line.Trim().StartsWith(";") || line.Trim().StartsWith("//") )
					{
						layouts.RemoveAt(ir);
					}
					else
					{
						ir++;
					}
				}
			}
			this.selBoardLayout.Items.AddRange(layouts.ToArray());
			if ( layouts.Count > 0 )
			{
				this.selBoardLayout.SelectedIndex = 0;
			}
		}

		public void log(string text)
		{
			this.logtext = this.logwindow.Text;
			if ( text.Contains("\r\n") )
			{
				this.logtext +=  text;
			}
			else
			{
				this.logtext += text + "\r\n";
			}
			this.logwindow.Text = this.logtext;
			this.logwindow.Refresh();
		}

		public void loadsavelog(string text)
		{
			if ( !text.Contains("\r\n") )
			{
				text += "\r\n";
			}
			this.textLoadSaveLog.Text += text;
			this.textLoadSaveLog.Refresh();
		}

		public void loadTileSetList()
		{
			string[] filenames = System.IO.Directory.GetFiles("tilesets");
			string id;
			this.selTilesets.Items.Clear();
			foreach (string filename in filenames)
			{
				id = System.IO.Path.GetFileNameWithoutExtension(filename);
				this.selTilesets.Items.Add(id);
			}
			this.selectTileSetListId();
		}

		public void selectTileSetListId()
		{
			this.labelTileset.Text = Board.title;
			int i = -1;
			if ( this.selTilesets.Items.Count > 0 )
			{
				this.selTilesets.SelectedIndex = 0;
			}
			for ( i = 0; i < this.selTilesets.Items.Count; i++ )
			{
				if ( this.selTilesets.Items[i].ToString() == Board.title )
				{
					this.selTilesets.SelectedIndex = i;
					break;
				}
			}
		}

		/*
		public void loadModelList()
		{
			string[] filenames = System.IO.Directory.GetFiles("models");
			Array.Sort(filenames);
			Array.Reverse(filenames);
			string id;
			this.selModels.Items.Clear();
			for ( int i = 0; i < filenames.Length; i++ )
			{
				string filename = filenames[i];
				if ( System.IO.File.Exists(filename) )
				{
					id = System.IO.Path.GetFileNameWithoutExtension(filename);
					// only show models relating to current tileset
					if ( id.Length > Board.title.Length && id.Substring(0, Board.title.Length + 1) == Board.title + "-" )
					{
						// remove tileset from model name
						id = id.Substring(Board.title.Length + 1);
						this.selModels.Items.Add(id);
					}
				}
			}
			this.selectModelListId();
		}
		*/

		/*
		public void selectModelListId()
		{
			this.labelModel.Text = this.model_name;
			int i = -1;
			if ( this.selModels.Items.Count > 0 )
			{
				this.selModels.SelectedIndex = 0;
			}
			for ( i = 0; i < this.selModels.Items.Count; i++ )
			{
				if ( this.selModels.Items[i].ToString() == this.model_name )
				{
					this.selModels.SelectedIndex = i;
					break;
				}
			}
		}
		*/

		// creates a solution model directly from the tileset
		public void createSolutionModel()
		{
			foreach ( Tile tile in this.board.tileset )
			{
				int[] colrow = Board.getColRowFromPos(tile.id);
				tile.col = colrow[0];
				tile.row = colrow[1];
				/*
				if ( tile.col == 0 || tile.row == 0 )
				{
					System.Diagnostics.Debugger.Break();
				}
				*/
				if ( tile.id <= this.board.tilepos.Length )
				{
					this.board.tilepos[tile.id-1] = tile.id;
				}
			}
			this.board.getModel();

			// create default model (v1 col,row,tileId,rotation
			/*
			string line;
			string model = "";
			int tileId = 0;
			this.model_name = "new";
			this.textModel.Text = "";
			for ( int row = 1; row <= Board.num_rows; row++ )
			{
				for ( int col = 1; col <= Board.num_cols; col++ )
				{
					tileId += 1;
					line = col + "," + row + "," + tileId + "," + 1;
					model += line + "\r\n";
				}
			}
			this.textModel.Text = model;
			this.textModel.Refresh();
			*/
		}

		public void updateStatusScore(int score)
		{
			this.statusScore.Text = score.ToString() + " / " + this.board.maxScore;
			this.statusBar.Refresh();
		}

		public void updateStatusTilesUsed(int numTiles)
		{
			this.statusTilesUsed.Text = numTiles.ToString() + " / " + this.board.tileset.Length;
			this.statusBar.Refresh();
		}

		public List<string> executeSQL(string sql)
		{
			List<string> rv = new List<string>();

			/*
			string MyConString = "SERVER=192.168.0.80;"
				+ "DATABASE=et2solver;"
				+ "UID=www;"
				+ "PASSWORD=www1user;default command timeout = 30;";
			*/
			/*
			string MyConString = "SERVER=localhost;"
				+ "DATABASE=et2solver;"
				+ "UID=root;"
				+ "PASSWORD=;default command timeout = 30;";
			*/

			try
			{
//		mysql disabled
				/*
				MySqlConnection connection = new MySqlConnection(MyConString);
				MySqlCommand command = connection.CreateCommand();
				MySqlDataReader Reader;
				command.CommandText = sql;
				connection.Open();

				Reader = command.ExecuteReader();
				while (Reader.Read())
				{
					List<string> line = new List<string>();
					for ( int i= 0; i < Reader.FieldCount; i++ )
					{
						line.Add(Reader.GetValue(i).ToString());
					}
					rv.Add(String.Join(",", line.ToArray()));
				}
				connection.Close();
				*/
			}
			catch (Exception e)
			{
				this.log("Exception: " + e.Message + ", source: " + e.StackTrace);
			}
			return rv;
		}

		public string getTimestamp()
		{
			return DateTime.Now.ToString("yyyy-MMdd-HHmm");
		}

		public void loadHintList()
		{
			if ( Board.title == "" )
			{
				System.Windows.Forms.MessageBox.Show("Load a tileset.");
				return;
			}
			string dirpath = "tilelists\\" + Board.title;
			if ( !System.IO.File.Exists(dirpath) )
			{
				System.IO.Directory.CreateDirectory(dirpath);
			}
			string[] filenames = System.IO.Directory.GetFiles(dirpath);
			Array.Sort(filenames);
			Array.Reverse(filenames);
			string id;
			this.selHints.Items.Clear();
			for ( int i = 0; i < filenames.Length; i++ )
			{
				string filename = filenames[i];
				if ( System.IO.File.Exists(filename) )
				{
					id = System.IO.Path.GetFileNameWithoutExtension(filename);
					if ( Regex.Match(id, "^hints_").Success )
					{
						this.selHints.Items.Add(id.Substring(6));
					}
				}
			}
		}

		public void initSolver(bool force)
		{
			if ( force || this.solver == null )
			{
				this.solver = new Solver();
				this.solver.loadTileset();
				this.solver.loadQueueList();
				this.selDebugLevel.SelectedIndex = 1;

				// set default
				if ( Program.TheMainForm.selS1SolveMethod.Items.Count > 1 )
				{
					Program.TheMainForm.selS1SolveMethod.SelectedIndex = 1;
				}
			}
		}


		void CbS2UseRandomSeedCheckedChanged(object sender, EventArgs e)
		{
			if ( this.cbS2UseRandomSeed.Checked )
			{
				this.grS2RandomTileQueue.Enabled = true;
			}
			else
			{
				this.grS2RandomTileQueue.Enabled = false;
			}
		}

		public bool isNumeric(string text)
		{
            Regex reg = new Regex("^[0-9]+$");
            return reg.IsMatch(text);
		}

		public void createNewTileset()
		{
			System.Int32 seed;
			this.board.setLayout();
			RandomBoard rb = new RandomBoard();
			if ( this.inputSeed.Text != "seed = random" && this.inputSeed.Text != "" )
			{
				seed = System.Convert.ToInt32(this.inputSeed.Text);
			}
			else
			{
				seed = rb.getSeed();
			}
			Board.seed = seed;
			Board.setTitle();
			this.model_name = seed.ToString();
			this.labelModel.Text = seed.ToString();
			this.labelTileset.Text = Board.title;
			this.textSaveTilesetName.Text = Board.title;
			this.textTileset.Text = rb.createNewTileset(seed);
			this.textLoadSaveLog.Text += rb.result + "\r\n";
			this.textLoadSaveLog.Update();
			this.board.setTileset();
			this.labelTileset.Refresh();
			this.textTileset.Refresh();

			if ( this.solver == null )
			{
				this.initSolver(false);
			}
			else
			{
				this.solver.loadTileset();
			}
		}

		public void selectPath()
		{
			this.action = "select_path";
			this.action_step = 1;
			this.statusInstructions.Text = "Select Path. 1 / 2. Select TOP LEFT cell.";
		}


		void BtRemoveTilesClick(object sender, EventArgs e)
		{
			this.board.removeSelectedTiles();
		}

		void BtCopyBoardLogClick(object sender, EventArgs e)
		{
			this.tbBoardLog.SelectAll();
			this.tbBoardLog.Copy();
		}

		void BtClearBoardLogClick(object sender, EventArgs e)
		{
			this.tbBoardLog.Clear();
		}

		void BtDumpBoardAsTilesetClick(object sender, EventArgs e)
		{
			this.board.dumpAsTileset();
		}

		void BtCopyCellFilterClick(object sender, EventArgs e)
		{
			this.inputCellFilter.SelectAll();
			this.inputCellFilter.Copy();
		}

		void BtClearCellFilterClick(object sender, EventArgs e)
		{
			this.inputCellFilter.Clear();
		}

		void BtClearTilesetClick(object sender, EventArgs e)
		{
			this.textTileset.Clear();
		}

		void BtCopyHintsClick(object sender, EventArgs e)
		{
			this.textHints.SelectAll();
			this.textHints.Copy();
		}

		void BtSaveConfigClick(object sender, EventArgs e)
		{
			// use file save dialog
			System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			if ( CAF_Application.config.filename != "" )
			{
				saveFileDialog1.FileName = CAF_Application.config.filename;
			}
			else
			{
				saveFileDialog1.FileName = "config-new.txt";
			}
			saveFileDialog1.Filter = "Config files (config-*.txt)|config-*.txt";
			saveFileDialog1.FilterIndex = 1;
			saveFileDialog1.RestoreDirectory = true;
			saveFileDialog1.InitialDirectory = "config";
			saveFileDialog1.Title = "Save Configuration";

			bool isFileSelected = false;
			if ( saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName != "" )
			{
				isFileSelected = true;
			}
			if ( !isFileSelected )
			{
				System.Windows.Forms.MessageBox.Show("No destination configuration filename entered");
				return;
			}
			string filename = saveFileDialog1.FileName;

			CAF_Application.config.setConfiguration(tbConfig.Lines);
			CAF_Application.config.saveConfiguration(filename);
			string timestamp = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt : ");
			this.statusInstructions.Text = timestamp + "saved " + filename;
		}

		void BtLoadConfigClick(object sender, EventArgs e)
		{
			// use file open dialog
			System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.FileName = "";
			openFileDialog1.Filter = "Config files (config-*.txt)|config-*.txt";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true;
			openFileDialog1.InitialDirectory = "config";
			openFileDialog1.Title = "Load Configuration";

			bool isFileSelected = false;
			if ( openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && openFileDialog1.FileName != "" )
			{
				isFileSelected = true;
			}
			if ( !isFileSelected )
			{
				System.Windows.Forms.MessageBox.Show("No configuration file selected");
				return;
			}

			CAF_Application.config.loadConfiguration(openFileDialog1.FileName);
			tbConfig.Lines = System.IO.File.ReadAllLines(openFileDialog1.FileName);
			tbConfig.Update();
			string timestamp = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt : ");
			this.statusInstructions.Text = timestamp + "loaded " + openFileDialog1.FileName;
			this.applyConfiguration();
		}

		void BtSetConfigClick(object sender, EventArgs e)
		{
			CAF_Application.config.setConfiguration(tbConfig.Lines);
			string timestamp = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt : ");
			this.statusInstructions.Text = timestamp + "set configuration (unsaved)";
			this.applyConfiguration();
		}

		void BtClearConfigClick(object sender, EventArgs e)
		{
			tbConfig.Clear();
		}


		void BtGenerateListsClick(object sender, EventArgs e)
		{
			Solver2x2 s = new Solver2x2();
			s.saveToFile = true;
//			s.generateList("2x2_corner_tl");
//			s.generateList("2x2_edge_left");
			s.generateList("2x2_internal");
		}
	}
}

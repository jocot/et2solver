/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 8/11/2009
 * Time: 10:34 AM
 *
 */

/*
// resize board for smaller sizes...
pb_board.SetBounds(pb_board.Left, pb_board.Top, 400, pb_board.Size.Height);
 */

using System;
using System.Collections.Generic;
using et2;
using CAF;

namespace ET2Solver
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabLoadSave = new System.Windows.Forms.TabPage();
			this.btCopyHints = new System.Windows.Forms.Button();
			this.btClearTileset = new System.Windows.Forms.Button();
			this.btCopyTileset = new System.Windows.Forms.Button();
			this.btCopyModel = new System.Windows.Forms.Button();
			this.cbShowMagicBorders = new System.Windows.Forms.CheckBox();
			this.cbShowUniqueBoards = new System.Windows.Forms.CheckBox();
			this.cbVerboseCreateBoardLog = new System.Windows.Forms.CheckBox();
			this.cbShowNonUniqueBoards = new System.Windows.Forms.CheckBox();
			this.btCalcHash = new System.Windows.Forms.Button();
			this.btShuffle = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.btSetLayout = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.selBoardLayout = new System.Windows.Forms.ComboBox();
			this.btDumpBoard = new System.Windows.Forms.Button();
			this.btSetModelAsHint = new System.Windows.Forms.Button();
			this.btClearHints = new System.Windows.Forms.Button();
			this.btClearModel = new System.Windows.Forms.Button();
			this.btTilesetR4 = new System.Windows.Forms.Button();
			this.btTilesetR3 = new System.Windows.Forms.Button();
			this.btTilesetR2 = new System.Windows.Forms.Button();
			this.btTilesetR1 = new System.Windows.Forms.Button();
			this.btClearLoadSaveLog = new System.Windows.Forms.Button();
			this.textLoadSaveLog = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.inputNumBoards = new System.Windows.Forms.TextBox();
			this.labelSeed = new System.Windows.Forms.Label();
			this.inputSeedStart = new System.Windows.Forms.TextBox();
			this.btCreateUniqueBoards = new System.Windows.Forms.Button();
			this.inputSeed = new System.Windows.Forms.TextBox();
			this.btNewTileset = new System.Windows.Forms.Button();
			this.btNew = new System.Windows.Forms.Button();
			this.btSetModel = new System.Windows.Forms.Button();
			this.textSaveHintsName = new System.Windows.Forms.TextBox();
			this.btSaveHints = new System.Windows.Forms.Button();
			this.textHints = new System.Windows.Forms.TextBox();
			this.selHints = new System.Windows.Forms.ComboBox();
			this.btLoadHints = new System.Windows.Forms.Button();
			this.btGetModel = new System.Windows.Forms.Button();
			this.btSetTileset = new System.Windows.Forms.Button();
			this.textSaveTilesetName = new System.Windows.Forms.TextBox();
			this.btSaveModel = new System.Windows.Forms.Button();
			this.btSaveTileset = new System.Windows.Forms.Button();
			this.textTileset = new System.Windows.Forms.TextBox();
			this.textModel = new System.Windows.Forms.TextBox();
			this.btLoadModel = new System.Windows.Forms.Button();
			this.selTilesets = new System.Windows.Forms.ComboBox();
			this.btLoadTileset = new System.Windows.Forms.Button();
			this.tabDesign = new System.Windows.Forms.TabPage();
			this.btSaveDesign = new System.Windows.Forms.Button();
			this.btLoadDesign = new System.Windows.Forms.Button();
			this.btRunDesignCmd = new System.Windows.Forms.Button();
			this.lbDesignCmd = new System.Windows.Forms.ListBox();
			this.btFillLine = new System.Windows.Forms.Button();
			this.btImportTileset = new System.Windows.Forms.Button();
			this.btExportTileset = new System.Windows.Forms.Button();
			this.btExportFilter = new System.Windows.Forms.Button();
			this.selDesignSize = new System.Windows.Forms.ComboBox();
			this.patternPanel = new System.Windows.Forms.Panel();
			this.btImportDesign = new System.Windows.Forms.Button();
			this.btExportDesign = new System.Windows.Forms.Button();
			this.btNewDesign = new System.Windows.Forms.Button();
			this.btCopyDesignLog = new System.Windows.Forms.Button();
			this.btClearDesignLog = new System.Windows.Forms.Button();
			this.tbDesignLog = new System.Windows.Forms.TextBox();
			this.pb_design = new System.Windows.Forms.PictureBox();
			this.tabBoard = new System.Windows.Forms.TabPage();
			this.btFindSwaps = new System.Windows.Forms.Button();
			this.btSaveBoardAsImage = new System.Windows.Forms.Button();
			this.btCompareToSolution = new System.Windows.Forms.Button();
			this.btCopyBoardLog = new System.Windows.Forms.Button();
			this.btClearBoardLog = new System.Windows.Forms.Button();
			this.tbBoardLog = new System.Windows.Forms.TextBox();
			this.btDumpBoardAsTileset = new System.Windows.Forms.Button();
			this.btRemoveTiles = new System.Windows.Forms.Button();
			this.btSelectTiles = new System.Windows.Forms.Button();
			this.btBuildFilter = new System.Windows.Forms.Button();
			this.btSelectPath = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.grTileAction = new System.Windows.Forms.GroupBox();
			this.rbActionOff = new System.Windows.Forms.RadioButton();
			this.rbBuildPath = new System.Windows.Forms.RadioButton();
			this.rbImportTiles = new System.Windows.Forms.RadioButton();
			this.rbDumpTiles = new System.Windows.Forms.RadioButton();
			this.rbMoveTiles = new System.Windows.Forms.RadioButton();
			this.labelNumSearchResultsUsed = new System.Windows.Forms.Label();
			this.labelNumSearchResultsFree = new System.Windows.Forms.Label();
			this.grSearchType = new System.Windows.Forms.GroupBox();
			this.cbSearchUniques = new System.Windows.Forms.CheckBox();
			this.rbSearchMatchAny = new System.Windows.Forms.RadioButton();
			this.rbSearchMatchAll = new System.Windows.Forms.RadioButton();
			this.rbSearchMatch4 = new System.Windows.Forms.RadioButton();
			this.rbSearchMatch3 = new System.Windows.Forms.RadioButton();
			this.rbSearchMatch2 = new System.Windows.Forms.RadioButton();
			this.rbSearchRegex = new System.Windows.Forms.RadioButton();
			this.btClearSearchPattenr = new System.Windows.Forms.Button();
			this.btSetBoardAsHint = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.dspNumScarceTiles = new System.Windows.Forms.Label();
			this.dspNumSwappableTiles = new System.Windows.Forms.Label();
			this.dspNumFillableTiles = new System.Windows.Forms.Label();
			this.dspNumInvalidCells = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.dspNumScarceCells = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.dspNumSwappableCells = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.dspNumFillableCells = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btClearOverlays = new System.Windows.Forms.Button();
			this.btCountIntersectingTiles = new System.Windows.Forms.Button();
			this.btPlaceTile = new System.Windows.Forms.Button();
			this.tabControl2 = new System.Windows.Forms.TabControl();
			this.tabResultsFree = new System.Windows.Forms.TabPage();
			this.searchFreeResultsImages = new System.Windows.Forms.ListView();
			this.tabResultsUsed = new System.Windows.Forms.TabPage();
			this.searchUsedResultsImages = new System.Windows.Forms.ListView();
			this.labelModel = new System.Windows.Forms.Label();
			this.labelTileset = new System.Windows.Forms.Label();
			this.selTilePattern = new System.Windows.Forms.Label();
			this.selColRow = new System.Windows.Forms.Label();
			this.selTile = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.selMatchPattern = new System.Windows.Forms.TextBox();
			this.btSearch = new System.Windows.Forms.Button();
			this.pb_board = new System.Windows.Forms.PictureBox();
			this.tabSolver1 = new System.Windows.Forms.TabPage();
			this.cbCountBorderTriplets = new System.Windows.Forms.CheckBox();
			this.cbPauseOnBorderTriplet = new System.Windows.Forms.CheckBox();
			this.cbPauseNumBGColours = new System.Windows.Forms.CheckBox();
			this.inputS1NumBGColours = new System.Windows.Forms.TextBox();
			this.cbBGColourStats = new System.Windows.Forms.CheckBox();
			this.cbSaveSolutions = new System.Windows.Forms.CheckBox();
			this.btCopyCellFilter = new System.Windows.Forms.Button();
			this.btClearCellFilter = new System.Windows.Forms.Button();
			this.btStepForward = new System.Windows.Forms.Button();
			this.btStepBack = new System.Windows.Forms.Button();
			this.btCopyCmd = new System.Windows.Forms.Button();
			this.btClearCmd = new System.Windows.Forms.Button();
			this.btRunCmd = new System.Windows.Forms.Button();
			this.inputCmd = new System.Windows.Forms.TextBox();
			this.btCopySolvePath = new System.Windows.Forms.Button();
			this.btClearSolvePath = new System.Windows.Forms.Button();
			this.selDumpOptions = new System.Windows.Forms.ComboBox();
			this.btCopyDump = new System.Windows.Forms.Button();
			this.cbTrackTileDistribution = new System.Windows.Forms.CheckBox();
			this.btStop = new System.Windows.Forms.Button();
			this.cbS1UseRegionRestrictions = new System.Windows.Forms.CheckBox();
			this.labelUniquePatternOrientations = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.labelSpeed = new System.Windows.Forms.Label();
			this.btClearScores = new System.Windows.Forms.Button();
			this.btClearSolutions = new System.Windows.Forms.Button();
			this.labelSolutions = new System.Windows.Forms.Label();
			this.labels11 = new System.Windows.Forms.Label();
			this.cbCellFilter = new System.Windows.Forms.CheckBox();
			this.inputCellFilter = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.selS1PathFilter = new System.Windows.Forms.ComboBox();
			this.button2 = new System.Windows.Forms.Button();
			this.labelS1Seed = new System.Windows.Forms.Label();
			this.label44 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.inputS1CurrentSolveMethod = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.cbS1EnableScoreTrigger = new System.Windows.Forms.CheckBox();
			this.cbS1EnableBacktrackLimit = new System.Windows.Forms.CheckBox();
			this.grS1IterationControl = new System.Windows.Forms.GroupBox();
			this.cbS1IterationLimitScoreProximity = new System.Windows.Forms.CheckBox();
			this.inputS1IterationScoreProximity = new System.Windows.Forms.TextBox();
			this.label31 = new System.Windows.Forms.Label();
			this.label39 = new System.Windows.Forms.Label();
			this.inputS1RunLength = new System.Windows.Forms.TextBox();
			this.cbS1IterationLimit = new System.Windows.Forms.CheckBox();
			this.cbS1AutoSaveByInterval = new System.Windows.Forms.CheckBox();
			this.inputS1AutoSaveIterations = new System.Windows.Forms.TextBox();
			this.inputS1MaxIterations = new System.Windows.Forms.TextBox();
			this.grS1AutoScoreTrigger = new System.Windows.Forms.GroupBox();
			this.cbS1PauseOnScore = new System.Windows.Forms.CheckBox();
			this.label33 = new System.Windows.Forms.Label();
			this.labelS1NextScoreTrigger = new System.Windows.Forms.Label();
			this.cbS1AutoIncrementScore = new System.Windows.Forms.CheckBox();
			this.cbS1ScoreAutoSave = new System.Windows.Forms.CheckBox();
			this.label28 = new System.Windows.Forms.Label();
			this.inputS1ScoreTrigger = new System.Windows.Forms.TextBox();
			this.inputS1SolvePath = new System.Windows.Forms.TextBox();
			this.label43 = new System.Windows.Forms.Label();
			this.selDebugLevel = new System.Windows.Forms.ComboBox();
			this.grS1RandomTileQueue = new System.Windows.Forms.GroupBox();
			this.cbS1UseRandomQueues = new System.Windows.Forms.CheckBox();
			this.cbS1UseRandomTileset = new System.Windows.Forms.CheckBox();
			this.label40 = new System.Windows.Forms.Label();
			this.inputS1SeedStep = new System.Windows.Forms.TextBox();
			this.cbS1RandomSeeds = new System.Windows.Forms.CheckBox();
			this.label37 = new System.Windows.Forms.Label();
			this.inputS1StartSeed = new System.Windows.Forms.TextBox();
			this.grS1BacktrackOptions = new System.Windows.Forms.GroupBox();
			this.label17 = new System.Windows.Forms.Label();
			this.inputS1MaxEmptyCells = new System.Windows.Forms.TextBox();
			this.cbUseAllTiles = new System.Windows.Forms.CheckBox();
			this.cbPauseEndCycle = new System.Windows.Forms.CheckBox();
			this.rbBacktrackQueueBackPedal = new System.Windows.Forms.RadioButton();
			this.rbBacktrackQueueJump = new System.Windows.Forms.RadioButton();
			this.cbS1BacktrackDepthLimit = new System.Windows.Forms.CheckBox();
			this.cbS1BacktrackStaleIterationTrigger = new System.Windows.Forms.CheckBox();
			this.inputS1BacktrackStaleIterations = new System.Windows.Forms.TextBox();
			this.rbS1BacktrackPause = new System.Windows.Forms.RadioButton();
			this.cbS1BacktrackMinScoreTrigger = new System.Windows.Forms.CheckBox();
			this.cbS1BacktrackIterationTrigger = new System.Windows.Forms.CheckBox();
			this.inputS1BacktrackMinScore = new System.Windows.Forms.TextBox();
			this.inputS1BacktrackMinIterations = new System.Windows.Forms.TextBox();
			this.inputS1BacktrackDepthLimit = new System.Windows.Forms.TextBox();
			this.rbS1BacktrackSkipCell = new System.Windows.Forms.RadioButton();
			this.cbS1EnforceFillableCells = new System.Windows.Forms.CheckBox();
			this.label35 = new System.Windows.Forms.Label();
			this.btS1Reset = new System.Windows.Forms.Button();
			this.btS1ClearStats = new System.Windows.Forms.Button();
			this.logS1Stats = new System.Windows.Forms.TextBox();
			this.cbS1UseRandomSeed = new System.Windows.Forms.CheckBox();
			this.btS1Stats = new System.Windows.Forms.Button();
			this.labelS1TilesPlaced = new System.Windows.Forms.Label();
			this.labelS1QueueProgress = new System.Windows.Forms.Label();
			this.labelS1NumIterations = new System.Windows.Forms.Label();
			this.btClearLogS1 = new System.Windows.Forms.Button();
			this.logS1 = new System.Windows.Forms.TextBox();
			this.btSaveQS1 = new System.Windows.Forms.Button();
			this.selQS1 = new System.Windows.Forms.ComboBox();
			this.btLoadQS1 = new System.Windows.Forms.Button();
			this.btResumeS1 = new System.Windows.Forms.Button();
			this.btPauseS1 = new System.Windows.Forms.Button();
			this.label32 = new System.Windows.Forms.Label();
			this.labelS1MaxTilesPlaced = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label38 = new System.Windows.Forms.Label();
			this.labelCellPath = new System.Windows.Forms.Label();
			this.label41 = new System.Windows.Forms.Label();
			this.label42 = new System.Windows.Forms.Label();
			this.labelS1BestScore = new System.Windows.Forms.Label();
			this.label46 = new System.Windows.Forms.Label();
			this.label49 = new System.Windows.Forms.Label();
			this.selS1SolveMethod = new System.Windows.Forms.ComboBox();
			this.btStartS1 = new System.Windows.Forms.Button();
			this.tabStats = new System.Windows.Forms.TabPage();
			this.selCommands = new System.Windows.Forms.ComboBox();
			this.btRun = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btCopySLog2 = new System.Windows.Forms.Button();
			this.btCopySLog1 = new System.Windows.Forms.Button();
			this.btClearSLog2 = new System.Windows.Forms.Button();
			this.tbSLog2 = new System.Windows.Forms.TextBox();
			this.btClearSLog1 = new System.Windows.Forms.Button();
			this.tbSLog1 = new System.Windows.Forms.TextBox();
			this.tabLog = new System.Windows.Forms.TabPage();
			this.button1 = new System.Windows.Forms.Button();
			this.cmdSQL = new System.Windows.Forms.TextBox();
			this.btClearLog = new System.Windows.Forms.Button();
			this.btSaveLog = new System.Windows.Forms.Button();
			this.logwindow = new System.Windows.Forms.TextBox();
			this.tabSolver2 = new System.Windows.Forms.TabPage();
			this.cbS2UseRandomSeed = new System.Windows.Forms.CheckBox();
			this.labelNumIterations = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.labelS2Seed = new System.Windows.Forms.Label();
			this.label50 = new System.Windows.Forms.Label();
			this.grS2RandomTileQueue = new System.Windows.Forms.GroupBox();
			this.label45 = new System.Windows.Forms.Label();
			this.inputS2SeedStep = new System.Windows.Forms.TextBox();
			this.cbS2RandomSeeds = new System.Windows.Forms.CheckBox();
			this.label47 = new System.Windows.Forms.Label();
			this.inputS2StartSeed = new System.Windows.Forms.TextBox();
			this.labelTilesPlaced = new System.Windows.Forms.Label();
			this.labelQueueProgress = new System.Windows.Forms.Label();
			this.button8 = new System.Windows.Forms.Button();
			this.btClearSolver2Log = new System.Windows.Forms.Button();
			this.textSolver2Log = new System.Windows.Forms.TextBox();
			this.label27 = new System.Windows.Forms.Label();
			this.labelSolver2Method = new System.Windows.Forms.Label();
			this.labelS2MaxQueueSize = new System.Windows.Forms.Label();
			this.textSolver2MaxQueueSize = new System.Windows.Forms.TextBox();
			this.label25 = new System.Windows.Forms.Label();
			this.btSaveQueue = new System.Windows.Forms.Button();
			this.selSolver2QueueList = new System.Windows.Forms.ComboBox();
			this.btLoadQueue = new System.Windows.Forms.Button();
			this.btResume = new System.Windows.Forms.Button();
			this.btPause = new System.Windows.Forms.Button();
			this.label26 = new System.Windows.Forms.Label();
			this.labelS2MostTilesPlaced = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.labelS2PathLength = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.labelS2CellNum = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.labelS2QueueSize = new System.Windows.Forms.Label();
			this.labelS2CellId = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.btLoadDataset = new System.Windows.Forms.Button();
			this.selSolver2Method = new System.Windows.Forms.ComboBox();
			this.btSolvePuzzle = new System.Windows.Forms.Button();
			this.btGenerateLists = new System.Windows.Forms.Button();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btClearConfig = new System.Windows.Forms.Button();
			this.btSetConfig = new System.Windows.Forms.Button();
			this.label48 = new System.Windows.Forms.Label();
			this.btSaveConfig = new System.Windows.Forms.Button();
			this.tbConfig = new System.Windows.Forms.TextBox();
			this.btLoadConfig = new System.Windows.Forms.Button();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.statusInstructions = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusScoreLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusScore = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusTilesUsedLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusTilesUsed = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabControl1.SuspendLayout();
			this.tabLoadSave.SuspendLayout();
			this.tabDesign.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pb_design)).BeginInit();
			this.tabBoard.SuspendLayout();
			this.grTileAction.SuspendLayout();
			this.grSearchType.SuspendLayout();
			this.tabControl2.SuspendLayout();
			this.tabResultsFree.SuspendLayout();
			this.tabResultsUsed.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pb_board)).BeginInit();
			this.tabSolver1.SuspendLayout();
			this.grS1IterationControl.SuspendLayout();
			this.grS1AutoScoreTrigger.SuspendLayout();
			this.grS1RandomTileQueue.SuspendLayout();
			this.grS1BacktrackOptions.SuspendLayout();
			this.tabStats.SuspendLayout();
			this.tabLog.SuspendLayout();
			this.tabSolver2.SuspendLayout();
			this.grS2RandomTileQueue.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.statusBar.SuspendLayout();
			this.SuspendLayout();
			//
			// tabControl1
			//
			this.tabControl1.Controls.Add(this.tabLoadSave);
			this.tabControl1.Controls.Add(this.tabDesign);
			this.tabControl1.Controls.Add(this.tabBoard);
			this.tabControl1.Controls.Add(this.tabSolver1);
			this.tabControl1.Controls.Add(this.tabStats);
			this.tabControl1.Controls.Add(this.tabLog);
			this.tabControl1.Controls.Add(this.tabSolver2);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(8, 3);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1134, 849);
			this.tabControl1.TabIndex = 0;
			//
			// tabLoadSave
			//
			this.tabLoadSave.BackColor = System.Drawing.Color.LightGray;
			this.tabLoadSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.tabLoadSave.Controls.Add(this.btCopyHints);
			this.tabLoadSave.Controls.Add(this.btClearTileset);
			this.tabLoadSave.Controls.Add(this.btCopyTileset);
			this.tabLoadSave.Controls.Add(this.btCopyModel);
			this.tabLoadSave.Controls.Add(this.cbShowMagicBorders);
			this.tabLoadSave.Controls.Add(this.cbShowUniqueBoards);
			this.tabLoadSave.Controls.Add(this.cbVerboseCreateBoardLog);
			this.tabLoadSave.Controls.Add(this.cbShowNonUniqueBoards);
			this.tabLoadSave.Controls.Add(this.btCalcHash);
			this.tabLoadSave.Controls.Add(this.btShuffle);
			this.tabLoadSave.Controls.Add(this.btCancel);
			this.tabLoadSave.Controls.Add(this.btSetLayout);
			this.tabLoadSave.Controls.Add(this.label9);
			this.tabLoadSave.Controls.Add(this.selBoardLayout);
			this.tabLoadSave.Controls.Add(this.btDumpBoard);
			this.tabLoadSave.Controls.Add(this.btSetModelAsHint);
			this.tabLoadSave.Controls.Add(this.btClearHints);
			this.tabLoadSave.Controls.Add(this.btClearModel);
			this.tabLoadSave.Controls.Add(this.btTilesetR4);
			this.tabLoadSave.Controls.Add(this.btTilesetR3);
			this.tabLoadSave.Controls.Add(this.btTilesetR2);
			this.tabLoadSave.Controls.Add(this.btTilesetR1);
			this.tabLoadSave.Controls.Add(this.btClearLoadSaveLog);
			this.tabLoadSave.Controls.Add(this.textLoadSaveLog);
			this.tabLoadSave.Controls.Add(this.label1);
			this.tabLoadSave.Controls.Add(this.inputNumBoards);
			this.tabLoadSave.Controls.Add(this.labelSeed);
			this.tabLoadSave.Controls.Add(this.inputSeedStart);
			this.tabLoadSave.Controls.Add(this.btCreateUniqueBoards);
			this.tabLoadSave.Controls.Add(this.inputSeed);
			this.tabLoadSave.Controls.Add(this.btNewTileset);
			this.tabLoadSave.Controls.Add(this.btNew);
			this.tabLoadSave.Controls.Add(this.btSetModel);
			this.tabLoadSave.Controls.Add(this.textSaveHintsName);
			this.tabLoadSave.Controls.Add(this.btSaveHints);
			this.tabLoadSave.Controls.Add(this.textHints);
			this.tabLoadSave.Controls.Add(this.selHints);
			this.tabLoadSave.Controls.Add(this.btLoadHints);
			this.tabLoadSave.Controls.Add(this.btGetModel);
			this.tabLoadSave.Controls.Add(this.btSetTileset);
			this.tabLoadSave.Controls.Add(this.textSaveTilesetName);
			this.tabLoadSave.Controls.Add(this.btSaveModel);
			this.tabLoadSave.Controls.Add(this.btSaveTileset);
			this.tabLoadSave.Controls.Add(this.textTileset);
			this.tabLoadSave.Controls.Add(this.textModel);
			this.tabLoadSave.Controls.Add(this.btLoadModel);
			this.tabLoadSave.Controls.Add(this.selTilesets);
			this.tabLoadSave.Controls.Add(this.btLoadTileset);
			this.tabLoadSave.Location = new System.Drawing.Point(4, 22);
			this.tabLoadSave.Name = "tabLoadSave";
			this.tabLoadSave.Padding = new System.Windows.Forms.Padding(3);
			this.tabLoadSave.Size = new System.Drawing.Size(1126, 823);
			this.tabLoadSave.TabIndex = 3;
			this.tabLoadSave.Text = "Load/Save";
			this.tabLoadSave.UseVisualStyleBackColor = true;
			this.tabLoadSave.Enter += new System.EventHandler(this.TabLoadSaveEnter);
			//
			// btCopyHints
			//
			this.btCopyHints.Location = new System.Drawing.Point(592, 263);
			this.btCopyHints.Name = "btCopyHints";
			this.btCopyHints.Size = new System.Drawing.Size(84, 23);
			this.btCopyHints.TabIndex = 196;
			this.btCopyHints.Text = "Copy";
			this.btCopyHints.UseVisualStyleBackColor = true;
			this.btCopyHints.Click += new System.EventHandler(this.BtCopyHintsClick);
			//
			// btClearTileset
			//
			this.btClearTileset.Location = new System.Drawing.Point(129, 291);
			this.btClearTileset.Name = "btClearTileset";
			this.btClearTileset.Size = new System.Drawing.Size(85, 22);
			this.btClearTileset.TabIndex = 195;
			this.btClearTileset.Text = "Clear";
			this.btClearTileset.UseVisualStyleBackColor = true;
			this.btClearTileset.Click += new System.EventHandler(this.BtClearTilesetClick);
			//
			// btCopyTileset
			//
			this.btCopyTileset.Location = new System.Drawing.Point(129, 262);
			this.btCopyTileset.Name = "btCopyTileset";
			this.btCopyTileset.Size = new System.Drawing.Size(84, 23);
			this.btCopyTileset.TabIndex = 194;
			this.btCopyTileset.Text = "Copy";
			this.btCopyTileset.UseVisualStyleBackColor = true;
			this.btCopyTileset.Click += new System.EventHandler(this.BtCopyTilesetClick);
			//
			// btCopyModel
			//
			this.btCopyModel.Location = new System.Drawing.Point(363, 262);
			this.btCopyModel.Name = "btCopyModel";
			this.btCopyModel.Size = new System.Drawing.Size(84, 23);
			this.btCopyModel.TabIndex = 193;
			this.btCopyModel.Text = "Copy";
			this.btCopyModel.UseVisualStyleBackColor = true;
			this.btCopyModel.Click += new System.EventHandler(this.BtCopyModelClick);
			//
			// cbShowMagicBorders
			//
			this.cbShowMagicBorders.BackColor = System.Drawing.Color.WhiteSmoke;
			this.cbShowMagicBorders.Location = new System.Drawing.Point(726, 138);
			this.cbShowMagicBorders.Name = "cbShowMagicBorders";
			this.cbShowMagicBorders.Size = new System.Drawing.Size(150, 19);
			this.cbShowMagicBorders.TabIndex = 66;
			this.cbShowMagicBorders.Text = "show magic borders";
			this.cbShowMagicBorders.UseVisualStyleBackColor = false;
			//
			// cbShowUniqueBoards
			//
			this.cbShowUniqueBoards.BackColor = System.Drawing.Color.WhiteSmoke;
			this.cbShowUniqueBoards.Checked = true;
			this.cbShowUniqueBoards.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbShowUniqueBoards.Location = new System.Drawing.Point(726, 95);
			this.cbShowUniqueBoards.Name = "cbShowUniqueBoards";
			this.cbShowUniqueBoards.Size = new System.Drawing.Size(150, 19);
			this.cbShowUniqueBoards.TabIndex = 65;
			this.cbShowUniqueBoards.Text = "show unique boards";
			this.cbShowUniqueBoards.UseVisualStyleBackColor = false;
			//
			// cbVerboseCreateBoardLog
			//
			this.cbVerboseCreateBoardLog.BackColor = System.Drawing.Color.WhiteSmoke;
			this.cbVerboseCreateBoardLog.Location = new System.Drawing.Point(726, 163);
			this.cbVerboseCreateBoardLog.Name = "cbVerboseCreateBoardLog";
			this.cbVerboseCreateBoardLog.Size = new System.Drawing.Size(150, 19);
			this.cbVerboseCreateBoardLog.TabIndex = 64;
			this.cbVerboseCreateBoardLog.Text = "verbose logging";
			this.cbVerboseCreateBoardLog.UseVisualStyleBackColor = false;
			//
			// cbShowNonUniqueBoards
			//
			this.cbShowNonUniqueBoards.BackColor = System.Drawing.Color.WhiteSmoke;
			this.cbShowNonUniqueBoards.Location = new System.Drawing.Point(726, 116);
			this.cbShowNonUniqueBoards.Name = "cbShowNonUniqueBoards";
			this.cbShowNonUniqueBoards.Size = new System.Drawing.Size(150, 19);
			this.cbShowNonUniqueBoards.TabIndex = 63;
			this.cbShowNonUniqueBoards.Text = "show non unique boards";
			this.cbShowNonUniqueBoards.UseVisualStyleBackColor = false;
			//
			// btCalcHash
			//
			this.btCalcHash.Location = new System.Drawing.Point(364, 113);
			this.btCalcHash.Name = "btCalcHash";
			this.btCalcHash.Size = new System.Drawing.Size(84, 24);
			this.btCalcHash.TabIndex = 62;
			this.btCalcHash.Text = "Calc Hash";
			this.btCalcHash.UseVisualStyleBackColor = true;
			this.btCalcHash.Click += new System.EventHandler(this.BtCalcHashClick);
			//
			// btShuffle
			//
			this.btShuffle.Location = new System.Drawing.Point(364, 141);
			this.btShuffle.Name = "btShuffle";
			this.btShuffle.Size = new System.Drawing.Size(84, 24);
			this.btShuffle.TabIndex = 61;
			this.btShuffle.Text = "Shuffle";
			this.btShuffle.UseVisualStyleBackColor = true;
			this.btShuffle.Click += new System.EventHandler(this.BtShuffleClick);
			//
			// btCancel
			//
			this.btCancel.Location = new System.Drawing.Point(830, 185);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(85, 23);
			this.btCancel.TabIndex = 60;
			this.btCancel.Text = "Cancel";
			this.btCancel.UseVisualStyleBackColor = true;
			this.btCancel.Click += new System.EventHandler(this.BtCancelClick);
			//
			// btSetLayout
			//
			this.btSetLayout.Location = new System.Drawing.Point(973, 19);
			this.btSetLayout.Name = "btSetLayout";
			this.btSetLayout.Size = new System.Drawing.Size(85, 23);
			this.btSetLayout.TabIndex = 59;
			this.btSetLayout.Text = "Set Layout";
			this.btSetLayout.UseVisualStyleBackColor = true;
			this.btSetLayout.Click += new System.EventHandler(this.BtSetLayoutClick);
			//
			// label9
			//
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(725, 4);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(118, 13);
			this.label9.TabIndex = 58;
			this.label9.Text = "Board Layout";
			//
			// selBoardLayout
			//
			this.selBoardLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selBoardLayout.FormattingEnabled = true;
			this.selBoardLayout.ImeMode = System.Windows.Forms.ImeMode.On;
			this.selBoardLayout.Location = new System.Drawing.Point(726, 20);
			this.selBoardLayout.Name = "selBoardLayout";
			this.selBoardLayout.Size = new System.Drawing.Size(241, 21);
			this.selBoardLayout.TabIndex = 57;
			//
			// btDumpBoard
			//
			this.btDumpBoard.Location = new System.Drawing.Point(364, 85);
			this.btDumpBoard.Name = "btDumpBoard";
			this.btDumpBoard.Size = new System.Drawing.Size(84, 24);
			this.btDumpBoard.TabIndex = 56;
			this.btDumpBoard.Text = "Dump Tiles";
			this.btDumpBoard.UseVisualStyleBackColor = true;
			this.btDumpBoard.Click += new System.EventHandler(this.BtDumpBoardClick);
			//
			// btSetModelAsHint
			//
			this.btSetModelAsHint.Location = new System.Drawing.Point(592, 64);
			this.btSetModelAsHint.Name = "btSetModelAsHint";
			this.btSetModelAsHint.Size = new System.Drawing.Size(108, 25);
			this.btSetModelAsHint.TabIndex = 55;
			this.btSetModelAsHint.Text = "Set Model as Hint";
			this.btSetModelAsHint.UseVisualStyleBackColor = true;
			this.btSetModelAsHint.Click += new System.EventHandler(this.BtSetModelAsHintClick);
			//
			// btClearHints
			//
			this.btClearHints.Location = new System.Drawing.Point(593, 292);
			this.btClearHints.Name = "btClearHints";
			this.btClearHints.Size = new System.Drawing.Size(85, 23);
			this.btClearHints.TabIndex = 54;
			this.btClearHints.Text = "Clear";
			this.btClearHints.UseVisualStyleBackColor = true;
			this.btClearHints.Click += new System.EventHandler(this.BtClearHintsClick);
			//
			// btClearModel
			//
			this.btClearModel.Location = new System.Drawing.Point(363, 291);
			this.btClearModel.Name = "btClearModel";
			this.btClearModel.Size = new System.Drawing.Size(85, 22);
			this.btClearModel.TabIndex = 53;
			this.btClearModel.Text = "Clear";
			this.btClearModel.UseVisualStyleBackColor = true;
			this.btClearModel.Click += new System.EventHandler(this.BtClearModelClick);
			//
			// btTilesetR4
			//
			this.btTilesetR4.Location = new System.Drawing.Point(178, 149);
			this.btTilesetR4.Name = "btTilesetR4";
			this.btTilesetR4.Size = new System.Drawing.Size(33, 23);
			this.btTilesetR4.TabIndex = 49;
			this.btTilesetR4.Text = "R4";
			this.btTilesetR4.UseVisualStyleBackColor = true;
			this.btTilesetR4.Click += new System.EventHandler(this.BtTilesetR4Click);
			//
			// btTilesetR3
			//
			this.btTilesetR3.Location = new System.Drawing.Point(139, 149);
			this.btTilesetR3.Name = "btTilesetR3";
			this.btTilesetR3.Size = new System.Drawing.Size(33, 23);
			this.btTilesetR3.TabIndex = 48;
			this.btTilesetR3.Text = "R3";
			this.btTilesetR3.UseVisualStyleBackColor = true;
			this.btTilesetR3.Click += new System.EventHandler(this.BtTilesetR3Click);
			//
			// btTilesetR2
			//
			this.btTilesetR2.Location = new System.Drawing.Point(178, 120);
			this.btTilesetR2.Name = "btTilesetR2";
			this.btTilesetR2.Size = new System.Drawing.Size(33, 23);
			this.btTilesetR2.TabIndex = 47;
			this.btTilesetR2.Text = "R2";
			this.btTilesetR2.UseVisualStyleBackColor = true;
			this.btTilesetR2.Click += new System.EventHandler(this.BtTilesetR2Click);
			//
			// btTilesetR1
			//
			this.btTilesetR1.Location = new System.Drawing.Point(139, 120);
			this.btTilesetR1.Name = "btTilesetR1";
			this.btTilesetR1.Size = new System.Drawing.Size(33, 23);
			this.btTilesetR1.TabIndex = 46;
			this.btTilesetR1.Text = "R1";
			this.btTilesetR1.UseVisualStyleBackColor = true;
			this.btTilesetR1.Click += new System.EventHandler(this.BtTilesetR1Click);
			//
			// btClearLoadSaveLog
			//
			this.btClearLoadSaveLog.Location = new System.Drawing.Point(6, 408);
			this.btClearLoadSaveLog.Name = "btClearLoadSaveLog";
			this.btClearLoadSaveLog.Size = new System.Drawing.Size(80, 23);
			this.btClearLoadSaveLog.TabIndex = 45;
			this.btClearLoadSaveLog.Text = "Clear Log";
			this.btClearLoadSaveLog.UseVisualStyleBackColor = true;
			this.btClearLoadSaveLog.Click += new System.EventHandler(this.BtClearLoadSaveLogClick);
			//
			// textLoadSaveLog
			//
			this.textLoadSaveLog.Location = new System.Drawing.Point(6, 437);
			this.textLoadSaveLog.Multiline = true;
			this.textLoadSaveLog.Name = "textLoadSaveLog";
			this.textLoadSaveLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textLoadSaveLog.Size = new System.Drawing.Size(997, 380);
			this.textLoadSaveLog.TabIndex = 44;
			//
			// label1
			//
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(849, 53);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 13);
			this.label1.TabIndex = 43;
			this.label1.Text = "# Boards";
			//
			// inputNumBoards
			//
			this.inputNumBoards.Location = new System.Drawing.Point(849, 69);
			this.inputNumBoards.Name = "inputNumBoards";
			this.inputNumBoards.Size = new System.Drawing.Size(118, 20);
			this.inputNumBoards.TabIndex = 42;
			this.inputNumBoards.Text = "10";
			//
			// labelSeed
			//
			this.labelSeed.BackColor = System.Drawing.Color.Transparent;
			this.labelSeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSeed.Location = new System.Drawing.Point(726, 53);
			this.labelSeed.Name = "labelSeed";
			this.labelSeed.Size = new System.Drawing.Size(117, 13);
			this.labelSeed.TabIndex = 41;
			this.labelSeed.Text = "Starting Seed";
			//
			// inputSeedStart
			//
			this.inputSeedStart.Location = new System.Drawing.Point(726, 69);
			this.inputSeedStart.Name = "inputSeedStart";
			this.inputSeedStart.Size = new System.Drawing.Size(118, 20);
			this.inputSeedStart.TabIndex = 40;
			this.inputSeedStart.Text = "1";
			//
			// btCreateUniqueBoards
			//
			this.btCreateUniqueBoards.Location = new System.Drawing.Point(725, 185);
			this.btCreateUniqueBoards.Name = "btCreateUniqueBoards";
			this.btCreateUniqueBoards.Size = new System.Drawing.Size(99, 23);
			this.btCreateUniqueBoards.TabIndex = 39;
			this.btCreateUniqueBoards.Text = "Create Boards";
			this.btCreateUniqueBoards.UseVisualStyleBackColor = true;
			this.btCreateUniqueBoards.Click += new System.EventHandler(this.BtCreateUniqueBoardsClick);
			//
			// inputSeed
			//
			this.inputSeed.Location = new System.Drawing.Point(3, 64);
			this.inputSeed.MaxLength = 32;
			this.inputSeed.Name = "inputSeed";
			this.inputSeed.Size = new System.Drawing.Size(118, 20);
			this.inputSeed.TabIndex = 38;
			this.inputSeed.Text = "seed = random";
			this.inputSeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.inputSeed.Enter += new System.EventHandler(this.InputSeedEnter);
			//
			// btNewTileset
			//
			this.btNewTileset.Location = new System.Drawing.Point(129, 64);
			this.btNewTileset.Name = "btNewTileset";
			this.btNewTileset.Size = new System.Drawing.Size(92, 23);
			this.btNewTileset.TabIndex = 37;
			this.btNewTileset.Text = "New";
			this.btNewTileset.UseVisualStyleBackColor = true;
			this.btNewTileset.Click += new System.EventHandler(this.BtNewTilesetClick);
			//
			// btNew
			//
			this.btNew.Location = new System.Drawing.Point(235, 6);
			this.btNew.Name = "btNew";
			this.btNew.Size = new System.Drawing.Size(41, 22);
			this.btNew.TabIndex = 36;
			this.btNew.Text = "New";
			this.btNew.UseVisualStyleBackColor = true;
			this.btNew.Click += new System.EventHandler(this.BtNewClick);
			//
			// btSetModel
			//
			this.btSetModel.Location = new System.Drawing.Point(363, 60);
			this.btSetModel.Name = "btSetModel";
			this.btSetModel.Size = new System.Drawing.Size(85, 22);
			this.btSetModel.TabIndex = 34;
			this.btSetModel.Text = "Set Model";
			this.btSetModel.UseVisualStyleBackColor = true;
			this.btSetModel.Click += new System.EventHandler(this.BtSetModelClick);
			//
			// textSaveHintsName
			//
			this.textSaveHintsName.Location = new System.Drawing.Point(466, 35);
			this.textSaveHintsName.Name = "textSaveHintsName";
			this.textSaveHintsName.Size = new System.Drawing.Size(118, 20);
			this.textSaveHintsName.TabIndex = 32;
			//
			// btSaveHints
			//
			this.btSaveHints.Location = new System.Drawing.Point(592, 34);
			this.btSaveHints.Name = "btSaveHints";
			this.btSaveHints.Size = new System.Drawing.Size(85, 23);
			this.btSaveHints.TabIndex = 31;
			this.btSaveHints.Text = "Save Hints";
			this.btSaveHints.UseVisualStyleBackColor = true;
			this.btSaveHints.Click += new System.EventHandler(this.BtSaveHintsClick);
			//
			// textHints
			//
			this.textHints.Location = new System.Drawing.Point(466, 64);
			this.textHints.Multiline = true;
			this.textHints.Name = "textHints";
			this.textHints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textHints.Size = new System.Drawing.Size(121, 249);
			this.textHints.TabIndex = 30;
			//
			// selHints
			//
			this.selHints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selHints.FormattingEnabled = true;
			this.selHints.ImeMode = System.Windows.Forms.ImeMode.On;
			this.selHints.Location = new System.Drawing.Point(466, 5);
			this.selHints.Name = "selHints";
			this.selHints.Size = new System.Drawing.Size(118, 21);
			this.selHints.Sorted = true;
			this.selHints.TabIndex = 29;
			//
			// btLoadHints
			//
			this.btLoadHints.Location = new System.Drawing.Point(592, 4);
			this.btLoadHints.Name = "btLoadHints";
			this.btLoadHints.Size = new System.Drawing.Size(85, 23);
			this.btLoadHints.TabIndex = 28;
			this.btLoadHints.Text = "Load Hints";
			this.btLoadHints.UseVisualStyleBackColor = true;
			this.btLoadHints.Click += new System.EventHandler(this.BtLoadHintsClick);
			//
			// btGetModel
			//
			this.btGetModel.Location = new System.Drawing.Point(363, 35);
			this.btGetModel.Name = "btGetModel";
			this.btGetModel.Size = new System.Drawing.Size(85, 22);
			this.btGetModel.TabIndex = 27;
			this.btGetModel.Text = "Get Model";
			this.btGetModel.UseVisualStyleBackColor = true;
			this.btGetModel.Click += new System.EventHandler(this.BtGetModelClick);
			//
			// btSetTileset
			//
			this.btSetTileset.Location = new System.Drawing.Point(129, 91);
			this.btSetTileset.Name = "btSetTileset";
			this.btSetTileset.Size = new System.Drawing.Size(92, 23);
			this.btSetTileset.TabIndex = 26;
			this.btSetTileset.Text = "Set Tileset";
			this.btSetTileset.UseVisualStyleBackColor = true;
			this.btSetTileset.Click += new System.EventHandler(this.BtSetTilesetClick);
			//
			// textSaveTilesetName
			//
			this.textSaveTilesetName.Location = new System.Drawing.Point(3, 35);
			this.textSaveTilesetName.Name = "textSaveTilesetName";
			this.textSaveTilesetName.Size = new System.Drawing.Size(118, 20);
			this.textSaveTilesetName.TabIndex = 24;
			//
			// btSaveModel
			//
			this.btSaveModel.Location = new System.Drawing.Point(364, 6);
			this.btSaveModel.Name = "btSaveModel";
			this.btSaveModel.Size = new System.Drawing.Size(84, 22);
			this.btSaveModel.TabIndex = 23;
			this.btSaveModel.Text = "Save Model";
			this.btSaveModel.UseVisualStyleBackColor = true;
			this.btSaveModel.Click += new System.EventHandler(this.BtSaveModelClick);
			//
			// btSaveTileset
			//
			this.btSaveTileset.Location = new System.Drawing.Point(129, 34);
			this.btSaveTileset.Name = "btSaveTileset";
			this.btSaveTileset.Size = new System.Drawing.Size(92, 23);
			this.btSaveTileset.TabIndex = 22;
			this.btSaveTileset.Text = "Save Tileset";
			this.btSaveTileset.UseVisualStyleBackColor = true;
			this.btSaveTileset.Click += new System.EventHandler(this.BtSaveTilesetClick);
			//
			// textTileset
			//
			this.textTileset.Location = new System.Drawing.Point(3, 90);
			this.textTileset.Multiline = true;
			this.textTileset.Name = "textTileset";
			this.textTileset.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textTileset.Size = new System.Drawing.Size(120, 223);
			this.textTileset.TabIndex = 21;
			//
			// textModel
			//
			this.textModel.Location = new System.Drawing.Point(237, 35);
			this.textModel.Multiline = true;
			this.textModel.Name = "textModel";
			this.textModel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textModel.Size = new System.Drawing.Size(121, 278);
			this.textModel.TabIndex = 20;
			//
			// btLoadModel
			//
			this.btLoadModel.Location = new System.Drawing.Point(284, 6);
			this.btLoadModel.Name = "btLoadModel";
			this.btLoadModel.Size = new System.Drawing.Size(75, 22);
			this.btLoadModel.TabIndex = 18;
			this.btLoadModel.Text = "Load Model";
			this.btLoadModel.UseVisualStyleBackColor = true;
			this.btLoadModel.Click += new System.EventHandler(this.btLoadModelClick);
			//
			// selTilesets
			//
			this.selTilesets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selTilesets.FormattingEnabled = true;
			this.selTilesets.ImeMode = System.Windows.Forms.ImeMode.On;
			this.selTilesets.Location = new System.Drawing.Point(3, 5);
			this.selTilesets.Name = "selTilesets";
			this.selTilesets.Size = new System.Drawing.Size(118, 21);
			this.selTilesets.Sorted = true;
			this.selTilesets.TabIndex = 17;
			//
			// btLoadTileset
			//
			this.btLoadTileset.Location = new System.Drawing.Point(129, 4);
			this.btLoadTileset.Name = "btLoadTileset";
			this.btLoadTileset.Size = new System.Drawing.Size(92, 23);
			this.btLoadTileset.TabIndex = 16;
			this.btLoadTileset.Text = "Load Tileset";
			this.btLoadTileset.UseVisualStyleBackColor = true;
			this.btLoadTileset.Click += new System.EventHandler(this.btLoadTilesetClick);
			//
			// tabDesign
			//
			this.tabDesign.Controls.Add(this.btSaveDesign);
			this.tabDesign.Controls.Add(this.btLoadDesign);
			this.tabDesign.Controls.Add(this.btRunDesignCmd);
			this.tabDesign.Controls.Add(this.lbDesignCmd);
			this.tabDesign.Controls.Add(this.btFillLine);
			this.tabDesign.Controls.Add(this.btImportTileset);
			this.tabDesign.Controls.Add(this.btExportTileset);
			this.tabDesign.Controls.Add(this.btExportFilter);
			this.tabDesign.Controls.Add(this.selDesignSize);
			this.tabDesign.Controls.Add(this.patternPanel);
			this.tabDesign.Controls.Add(this.btImportDesign);
			this.tabDesign.Controls.Add(this.btExportDesign);
			this.tabDesign.Controls.Add(this.btNewDesign);
			this.tabDesign.Controls.Add(this.btCopyDesignLog);
			this.tabDesign.Controls.Add(this.btClearDesignLog);
			this.tabDesign.Controls.Add(this.tbDesignLog);
			this.tabDesign.Controls.Add(this.pb_design);
			this.tabDesign.Location = new System.Drawing.Point(4, 22);
			this.tabDesign.Name = "tabDesign";
			this.tabDesign.Padding = new System.Windows.Forms.Padding(3);
			this.tabDesign.Size = new System.Drawing.Size(1126, 823);
			this.tabDesign.TabIndex = 7;
			this.tabDesign.Text = "Design";
			this.tabDesign.UseVisualStyleBackColor = true;
			this.tabDesign.Enter += new System.EventHandler(this.TabDesignEnter);
			//
			// btSaveDesign
			//
			this.btSaveDesign.Location = new System.Drawing.Point(137, 526);
			this.btSaveDesign.Name = "btSaveDesign";
			this.btSaveDesign.Size = new System.Drawing.Size(109, 23);
			this.btSaveDesign.TabIndex = 211;
			this.btSaveDesign.Text = "Save Design";
			this.btSaveDesign.UseVisualStyleBackColor = true;
			this.btSaveDesign.Click += new System.EventHandler(this.BtSaveDesignClick);
			//
			// btLoadDesign
			//
			this.btLoadDesign.Location = new System.Drawing.Point(137, 497);
			this.btLoadDesign.Name = "btLoadDesign";
			this.btLoadDesign.Size = new System.Drawing.Size(109, 23);
			this.btLoadDesign.TabIndex = 210;
			this.btLoadDesign.Text = "Load Design";
			this.btLoadDesign.UseVisualStyleBackColor = true;
			this.btLoadDesign.Click += new System.EventHandler(this.BtLoadDesignClick);
			//
			// btRunDesignCmd
			//
			this.btRunDesignCmd.Location = new System.Drawing.Point(137, 794);
			this.btRunDesignCmd.Name = "btRunDesignCmd";
			this.btRunDesignCmd.Size = new System.Drawing.Size(165, 23);
			this.btRunDesignCmd.TabIndex = 209;
			this.btRunDesignCmd.Text = "Run Cmd";
			this.btRunDesignCmd.UseVisualStyleBackColor = true;
			this.btRunDesignCmd.Click += new System.EventHandler(this.BtRunDesignCmdClick);
			//
			// lbDesignCmd
			//
			this.lbDesignCmd.FormattingEnabled = true;
			this.lbDesignCmd.Location = new System.Drawing.Point(137, 641);
			this.lbDesignCmd.Name = "lbDesignCmd";
			this.lbDesignCmd.Size = new System.Drawing.Size(165, 147);
			this.lbDesignCmd.TabIndex = 208;
			//
			// btFillLine
			//
			this.btFillLine.Location = new System.Drawing.Point(137, 468);
			this.btFillLine.Name = "btFillLine";
			this.btFillLine.Size = new System.Drawing.Size(109, 23);
			this.btFillLine.TabIndex = 206;
			this.btFillLine.Text = "Fill Line";
			this.btFillLine.UseVisualStyleBackColor = true;
			this.btFillLine.Click += new System.EventHandler(this.BtFillLineClick);
			//
			// btImportTileset
			//
			this.btImportTileset.Location = new System.Drawing.Point(6, 584);
			this.btImportTileset.Name = "btImportTileset";
			this.btImportTileset.Size = new System.Drawing.Size(109, 23);
			this.btImportTileset.TabIndex = 205;
			this.btImportTileset.Text = "Import Tileset";
			this.btImportTileset.UseVisualStyleBackColor = true;
			this.btImportTileset.Click += new System.EventHandler(this.BtImportTilesetClick);
			//
			// btExportTileset
			//
			this.btExportTileset.Location = new System.Drawing.Point(6, 613);
			this.btExportTileset.Name = "btExportTileset";
			this.btExportTileset.Size = new System.Drawing.Size(109, 23);
			this.btExportTileset.TabIndex = 204;
			this.btExportTileset.Text = "Export Tileset";
			this.btExportTileset.UseVisualStyleBackColor = true;
			this.btExportTileset.Click += new System.EventHandler(this.BtExportTilesetClick);
			//
			// btExportFilter
			//
			this.btExportFilter.Location = new System.Drawing.Point(6, 555);
			this.btExportFilter.Name = "btExportFilter";
			this.btExportFilter.Size = new System.Drawing.Size(109, 23);
			this.btExportFilter.TabIndex = 203;
			this.btExportFilter.Text = "Export Filter";
			this.btExportFilter.UseVisualStyleBackColor = true;
			this.btExportFilter.Click += new System.EventHandler(this.BtExportFilterClick);
			//
			// selDesignSize
			//
			this.selDesignSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selDesignSize.FormattingEnabled = true;
			this.selDesignSize.ImeMode = System.Windows.Forms.ImeMode.On;
			this.selDesignSize.Location = new System.Drawing.Point(6, 441);
			this.selDesignSize.Name = "selDesignSize";
			this.selDesignSize.Size = new System.Drawing.Size(280, 21);
			this.selDesignSize.TabIndex = 202;
			//
			// patternPanel
			//
			this.patternPanel.BackColor = System.Drawing.Color.Gainsboro;
			this.patternPanel.Location = new System.Drawing.Point(6, 5);
			this.patternPanel.Name = "patternPanel";
			this.patternPanel.Size = new System.Drawing.Size(280, 431);
			this.patternPanel.TabIndex = 201;
			//
			// btImportDesign
			//
			this.btImportDesign.Location = new System.Drawing.Point(6, 497);
			this.btImportDesign.Name = "btImportDesign";
			this.btImportDesign.Size = new System.Drawing.Size(109, 23);
			this.btImportDesign.TabIndex = 200;
			this.btImportDesign.Text = "Import Design";
			this.btImportDesign.UseVisualStyleBackColor = true;
			this.btImportDesign.Click += new System.EventHandler(this.BtImportDesignClick);
			//
			// btExportDesign
			//
			this.btExportDesign.Location = new System.Drawing.Point(6, 526);
			this.btExportDesign.Name = "btExportDesign";
			this.btExportDesign.Size = new System.Drawing.Size(109, 23);
			this.btExportDesign.TabIndex = 199;
			this.btExportDesign.Text = "Export Design";
			this.btExportDesign.UseVisualStyleBackColor = true;
			this.btExportDesign.Click += new System.EventHandler(this.BtExportDesignClick);
			//
			// btNewDesign
			//
			this.btNewDesign.Location = new System.Drawing.Point(6, 468);
			this.btNewDesign.Name = "btNewDesign";
			this.btNewDesign.Size = new System.Drawing.Size(109, 23);
			this.btNewDesign.TabIndex = 198;
			this.btNewDesign.Text = "New Design";
			this.btNewDesign.UseVisualStyleBackColor = true;
			this.btNewDesign.Click += new System.EventHandler(this.BtNewDesignClick);
			//
			// btCopyDesignLog
			//
			this.btCopyDesignLog.Location = new System.Drawing.Point(7, 794);
			this.btCopyDesignLog.Name = "btCopyDesignLog";
			this.btCopyDesignLog.Size = new System.Drawing.Size(59, 23);
			this.btCopyDesignLog.TabIndex = 197;
			this.btCopyDesignLog.Text = "Copy";
			this.btCopyDesignLog.UseVisualStyleBackColor = true;
			this.btCopyDesignLog.Click += new System.EventHandler(this.BtCopyDesignLogClick);
			//
			// btClearDesignLog
			//
			this.btClearDesignLog.Location = new System.Drawing.Point(69, 794);
			this.btClearDesignLog.Name = "btClearDesignLog";
			this.btClearDesignLog.Size = new System.Drawing.Size(57, 23);
			this.btClearDesignLog.TabIndex = 196;
			this.btClearDesignLog.Text = "Clear";
			this.btClearDesignLog.UseVisualStyleBackColor = true;
			this.btClearDesignLog.Click += new System.EventHandler(this.BtClearDesignLogClick);
			//
			// tbDesignLog
			//
			this.tbDesignLog.Location = new System.Drawing.Point(7, 642);
			this.tbDesignLog.Multiline = true;
			this.tbDesignLog.Name = "tbDesignLog";
			this.tbDesignLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbDesignLog.Size = new System.Drawing.Size(108, 146);
			this.tbDesignLog.TabIndex = 195;
			//
			// pb_design
			//
			this.pb_design.BackColor = System.Drawing.Color.LightGray;
			this.pb_design.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pb_design.ImageLocation = "";
			this.pb_design.InitialImage = null;
			this.pb_design.Location = new System.Drawing.Point(311, 0);
			this.pb_design.Name = "pb_design";
			this.pb_design.Size = new System.Drawing.Size(815, 823);
			this.pb_design.TabIndex = 1;
			this.pb_design.TabStop = false;
			this.pb_design.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Pb_designMouseMove);
			this.pb_design.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Pb_designMouseUp);
			//
			// tabBoard
			//
			this.tabBoard.BackColor = System.Drawing.Color.LightGray;
			this.tabBoard.Controls.Add(this.btFindSwaps);
			this.tabBoard.Controls.Add(this.btSaveBoardAsImage);
			this.tabBoard.Controls.Add(this.btCompareToSolution);
			this.tabBoard.Controls.Add(this.btCopyBoardLog);
			this.tabBoard.Controls.Add(this.btClearBoardLog);
			this.tabBoard.Controls.Add(this.tbBoardLog);
			this.tabBoard.Controls.Add(this.btDumpBoardAsTileset);
			this.tabBoard.Controls.Add(this.btRemoveTiles);
			this.tabBoard.Controls.Add(this.btSelectTiles);
			this.tabBoard.Controls.Add(this.btBuildFilter);
			this.tabBoard.Controls.Add(this.btSelectPath);
			this.tabBoard.Controls.Add(this.label5);
			this.tabBoard.Controls.Add(this.grTileAction);
			this.tabBoard.Controls.Add(this.labelNumSearchResultsUsed);
			this.tabBoard.Controls.Add(this.labelNumSearchResultsFree);
			this.tabBoard.Controls.Add(this.grSearchType);
			this.tabBoard.Controls.Add(this.btClearSearchPattenr);
			this.tabBoard.Controls.Add(this.btSetBoardAsHint);
			this.tabBoard.Controls.Add(this.label7);
			this.tabBoard.Controls.Add(this.label16);
			this.tabBoard.Controls.Add(this.dspNumScarceTiles);
			this.tabBoard.Controls.Add(this.dspNumSwappableTiles);
			this.tabBoard.Controls.Add(this.dspNumFillableTiles);
			this.tabBoard.Controls.Add(this.dspNumInvalidCells);
			this.tabBoard.Controls.Add(this.label13);
			this.tabBoard.Controls.Add(this.dspNumScarceCells);
			this.tabBoard.Controls.Add(this.label11);
			this.tabBoard.Controls.Add(this.dspNumSwappableCells);
			this.tabBoard.Controls.Add(this.label8);
			this.tabBoard.Controls.Add(this.dspNumFillableCells);
			this.tabBoard.Controls.Add(this.label2);
			this.tabBoard.Controls.Add(this.btClearOverlays);
			this.tabBoard.Controls.Add(this.btCountIntersectingTiles);
			this.tabBoard.Controls.Add(this.btPlaceTile);
			this.tabBoard.Controls.Add(this.tabControl2);
			this.tabBoard.Controls.Add(this.labelModel);
			this.tabBoard.Controls.Add(this.labelTileset);
			this.tabBoard.Controls.Add(this.selTilePattern);
			this.tabBoard.Controls.Add(this.selColRow);
			this.tabBoard.Controls.Add(this.selTile);
			this.tabBoard.Controls.Add(this.label6);
			this.tabBoard.Controls.Add(this.label4);
			this.tabBoard.Controls.Add(this.label3);
			this.tabBoard.Controls.Add(this.selMatchPattern);
			this.tabBoard.Controls.Add(this.btSearch);
			this.tabBoard.Controls.Add(this.pb_board);
			this.tabBoard.Location = new System.Drawing.Point(4, 22);
			this.tabBoard.Margin = new System.Windows.Forms.Padding(0);
			this.tabBoard.Name = "tabBoard";
			this.tabBoard.Size = new System.Drawing.Size(1126, 823);
			this.tabBoard.TabIndex = 0;
			this.tabBoard.Text = "Board";
			this.tabBoard.UseVisualStyleBackColor = true;
			//
			// btFindSwaps
			//
			this.btFindSwaps.Location = new System.Drawing.Point(196, 166);
			this.btFindSwaps.Name = "btFindSwaps";
			this.btFindSwaps.Size = new System.Drawing.Size(109, 23);
			this.btFindSwaps.TabIndex = 196;
			this.btFindSwaps.Text = "Find Swaps";
			this.btFindSwaps.UseVisualStyleBackColor = true;
			this.btFindSwaps.Click += new System.EventHandler(this.BtFindSwapsClick);
			//
			// btSaveBoardAsImage
			//
			this.btSaveBoardAsImage.Enabled = false;
			this.btSaveBoardAsImage.Location = new System.Drawing.Point(196, 191);
			this.btSaveBoardAsImage.Name = "btSaveBoardAsImage";
			this.btSaveBoardAsImage.Size = new System.Drawing.Size(109, 23);
			this.btSaveBoardAsImage.TabIndex = 195;
			this.btSaveBoardAsImage.Text = "Save As Image";
			this.btSaveBoardAsImage.UseVisualStyleBackColor = true;
			this.btSaveBoardAsImage.Click += new System.EventHandler(this.BtSaveBoardAsImageClick);
			//
			// btCompareToSolution
			//
			this.btCompareToSolution.Location = new System.Drawing.Point(196, 216);
			this.btCompareToSolution.Name = "btCompareToSolution";
			this.btCompareToSolution.Size = new System.Drawing.Size(109, 23);
			this.btCompareToSolution.TabIndex = 194;
			this.btCompareToSolution.Text = "Compare Solution";
			this.btCompareToSolution.UseVisualStyleBackColor = true;
			this.btCompareToSolution.Click += new System.EventHandler(this.BtCompareToSolutionClick);
			//
			// btCopyBoardLog
			//
			this.btCopyBoardLog.Location = new System.Drawing.Point(8, 290);
			this.btCopyBoardLog.Name = "btCopyBoardLog";
			this.btCopyBoardLog.Size = new System.Drawing.Size(59, 23);
			this.btCopyBoardLog.TabIndex = 193;
			this.btCopyBoardLog.Text = "Copy";
			this.btCopyBoardLog.UseVisualStyleBackColor = true;
			this.btCopyBoardLog.Click += new System.EventHandler(this.BtCopyBoardLogClick);
			//
			// btClearBoardLog
			//
			this.btClearBoardLog.Location = new System.Drawing.Point(70, 290);
			this.btClearBoardLog.Name = "btClearBoardLog";
			this.btClearBoardLog.Size = new System.Drawing.Size(57, 23);
			this.btClearBoardLog.TabIndex = 192;
			this.btClearBoardLog.Text = "Clear";
			this.btClearBoardLog.UseVisualStyleBackColor = true;
			this.btClearBoardLog.Click += new System.EventHandler(this.BtClearBoardLogClick);
			//
			// tbBoardLog
			//
			this.tbBoardLog.Location = new System.Drawing.Point(8, 109);
			this.tbBoardLog.Multiline = true;
			this.tbBoardLog.Name = "tbBoardLog";
			this.tbBoardLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbBoardLog.Size = new System.Drawing.Size(183, 179);
			this.tbBoardLog.TabIndex = 65;
			//
			// btDumpBoardAsTileset
			//
			this.btDumpBoardAsTileset.Location = new System.Drawing.Point(196, 6);
			this.btDumpBoardAsTileset.Name = "btDumpBoardAsTileset";
			this.btDumpBoardAsTileset.Size = new System.Drawing.Size(109, 23);
			this.btDumpBoardAsTileset.TabIndex = 64;
			this.btDumpBoardAsTileset.Text = "Dump as Tileset";
			this.btDumpBoardAsTileset.UseVisualStyleBackColor = true;
			this.btDumpBoardAsTileset.Click += new System.EventHandler(this.BtDumpBoardAsTilesetClick);
			//
			// btRemoveTiles
			//
			this.btRemoveTiles.Location = new System.Drawing.Point(196, 74);
			this.btRemoveTiles.Name = "btRemoveTiles";
			this.btRemoveTiles.Size = new System.Drawing.Size(109, 23);
			this.btRemoveTiles.TabIndex = 63;
			this.btRemoveTiles.Text = "Remove Tiles";
			this.btRemoveTiles.UseVisualStyleBackColor = true;
			this.btRemoveTiles.Click += new System.EventHandler(this.BtRemoveTilesClick);
			//
			// btSelectTiles
			//
			this.btSelectTiles.Location = new System.Drawing.Point(196, 118);
			this.btSelectTiles.Name = "btSelectTiles";
			this.btSelectTiles.Size = new System.Drawing.Size(109, 23);
			this.btSelectTiles.TabIndex = 62;
			this.btSelectTiles.Text = "Select Tiles";
			this.btSelectTiles.UseVisualStyleBackColor = true;
			this.btSelectTiles.Click += new System.EventHandler(this.BtSelectTilesClick);
			//
			// btBuildFilter
			//
			this.btBuildFilter.Location = new System.Drawing.Point(196, 96);
			this.btBuildFilter.Name = "btBuildFilter";
			this.btBuildFilter.Size = new System.Drawing.Size(109, 23);
			this.btBuildFilter.TabIndex = 61;
			this.btBuildFilter.Text = "Build Filter";
			this.btBuildFilter.UseVisualStyleBackColor = true;
			this.btBuildFilter.Click += new System.EventHandler(this.BtBuildFilterClick);
			//
			// btSelectPath
			//
			this.btSelectPath.Location = new System.Drawing.Point(196, 52);
			this.btSelectPath.Name = "btSelectPath";
			this.btSelectPath.Size = new System.Drawing.Size(109, 23);
			this.btSelectPath.TabIndex = 60;
			this.btSelectPath.Text = "Select Path";
			this.btSelectPath.UseVisualStyleBackColor = true;
			this.btSelectPath.Click += new System.EventHandler(this.BtSelectPathClick);
			//
			// label5
			//
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(5, 57);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(54, 21);
			this.label5.TabIndex = 59;
			this.label5.Text = "Pos";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// grTileAction
			//
			this.grTileAction.BackColor = System.Drawing.Color.Transparent;
			this.grTileAction.Controls.Add(this.rbActionOff);
			this.grTileAction.Controls.Add(this.rbBuildPath);
			this.grTileAction.Controls.Add(this.rbImportTiles);
			this.grTileAction.Controls.Add(this.rbDumpTiles);
			this.grTileAction.Controls.Add(this.rbMoveTiles);
			this.grTileAction.Location = new System.Drawing.Point(195, 710);
			this.grTileAction.Name = "grTileAction";
			this.grTileAction.Size = new System.Drawing.Size(109, 109);
			this.grTileAction.TabIndex = 58;
			this.grTileAction.TabStop = false;
			this.grTileAction.Text = "action";
			//
			// rbActionOff
			//
			this.rbActionOff.Checked = true;
			this.rbActionOff.Location = new System.Drawing.Point(6, 19);
			this.rbActionOff.Name = "rbActionOff";
			this.rbActionOff.Size = new System.Drawing.Size(40, 21);
			this.rbActionOff.TabIndex = 56;
			this.rbActionOff.TabStop = true;
			this.rbActionOff.Text = "off";
			this.rbActionOff.UseVisualStyleBackColor = true;
			//
			// rbBuildPath
			//
			this.rbBuildPath.Location = new System.Drawing.Point(21, 82);
			this.rbBuildPath.Name = "rbBuildPath";
			this.rbBuildPath.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbBuildPath.Size = new System.Drawing.Size(82, 21);
			this.rbBuildPath.TabIndex = 55;
			this.rbBuildPath.Text = "build path";
			this.rbBuildPath.UseVisualStyleBackColor = true;
			this.rbBuildPath.CheckedChanged += new System.EventHandler(this.RbBuildPathCheckedChanged);
			//
			// rbImportTiles
			//
			this.rbImportTiles.Location = new System.Drawing.Point(21, 61);
			this.rbImportTiles.Name = "rbImportTiles";
			this.rbImportTiles.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbImportTiles.Size = new System.Drawing.Size(82, 21);
			this.rbImportTiles.TabIndex = 54;
			this.rbImportTiles.Text = "import tiles";
			this.rbImportTiles.UseVisualStyleBackColor = true;
			//
			// rbDumpTiles
			//
			this.rbDumpTiles.Location = new System.Drawing.Point(21, 40);
			this.rbDumpTiles.Name = "rbDumpTiles";
			this.rbDumpTiles.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbDumpTiles.Size = new System.Drawing.Size(82, 21);
			this.rbDumpTiles.TabIndex = 53;
			this.rbDumpTiles.Text = "dump tiles";
			this.rbDumpTiles.UseVisualStyleBackColor = true;
			//
			// rbMoveTiles
			//
			this.rbMoveTiles.Location = new System.Drawing.Point(45, 19);
			this.rbMoveTiles.Name = "rbMoveTiles";
			this.rbMoveTiles.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbMoveTiles.Size = new System.Drawing.Size(58, 21);
			this.rbMoveTiles.TabIndex = 52;
			this.rbMoveTiles.Text = "move";
			this.rbMoveTiles.UseVisualStyleBackColor = true;
			//
			// labelNumSearchResultsUsed
			//
			this.labelNumSearchResultsUsed.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelNumSearchResultsUsed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelNumSearchResultsUsed.Location = new System.Drawing.Point(162, 386);
			this.labelNumSearchResultsUsed.Name = "labelNumSearchResultsUsed";
			this.labelNumSearchResultsUsed.Size = new System.Drawing.Size(142, 22);
			this.labelNumSearchResultsUsed.TabIndex = 54;
			this.labelNumSearchResultsUsed.Text = "000 used, 000 unique";
			this.labelNumSearchResultsUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// labelNumSearchResultsFree
			//
			this.labelNumSearchResultsFree.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelNumSearchResultsFree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelNumSearchResultsFree.Location = new System.Drawing.Point(7, 386);
			this.labelNumSearchResultsFree.Name = "labelNumSearchResultsFree";
			this.labelNumSearchResultsFree.Size = new System.Drawing.Size(139, 22);
			this.labelNumSearchResultsFree.TabIndex = 53;
			this.labelNumSearchResultsFree.Text = "000 free, 000 unique";
			this.labelNumSearchResultsFree.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// grSearchType
			//
			this.grSearchType.BackColor = System.Drawing.Color.Transparent;
			this.grSearchType.Controls.Add(this.cbSearchUniques);
			this.grSearchType.Controls.Add(this.rbSearchMatchAny);
			this.grSearchType.Controls.Add(this.rbSearchMatchAll);
			this.grSearchType.Controls.Add(this.rbSearchMatch4);
			this.grSearchType.Controls.Add(this.rbSearchMatch3);
			this.grSearchType.Controls.Add(this.rbSearchMatch2);
			this.grSearchType.Controls.Add(this.rbSearchRegex);
			this.grSearchType.Location = new System.Drawing.Point(11, 338);
			this.grSearchType.Name = "grSearchType";
			this.grSearchType.Size = new System.Drawing.Size(287, 45);
			this.grSearchType.TabIndex = 52;
			this.grSearchType.TabStop = false;
			this.grSearchType.Text = "search type";
			//
			// cbSearchUniques
			//
			this.cbSearchUniques.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbSearchUniques.Location = new System.Drawing.Point(147, 0);
			this.cbSearchUniques.Name = "cbSearchUniques";
			this.cbSearchUniques.Size = new System.Drawing.Size(61, 19);
			this.cbSearchUniques.TabIndex = 60;
			this.cbSearchUniques.Text = "unique";
			this.cbSearchUniques.UseVisualStyleBackColor = true;
			//
			// rbSearchMatchAny
			//
			this.rbSearchMatchAny.Location = new System.Drawing.Point(222, 19);
			this.rbSearchMatchAny.Name = "rbSearchMatchAny";
			this.rbSearchMatchAny.Size = new System.Drawing.Size(48, 24);
			this.rbSearchMatchAny.TabIndex = 57;
			this.rbSearchMatchAny.Text = "any";
			this.rbSearchMatchAny.UseVisualStyleBackColor = true;
			//
			// rbSearchMatchAll
			//
			this.rbSearchMatchAll.Location = new System.Drawing.Point(183, 19);
			this.rbSearchMatchAll.Name = "rbSearchMatchAll";
			this.rbSearchMatchAll.Size = new System.Drawing.Size(40, 24);
			this.rbSearchMatchAll.TabIndex = 56;
			this.rbSearchMatchAll.Text = "all";
			this.rbSearchMatchAll.UseVisualStyleBackColor = true;
			//
			// rbSearchMatch4
			//
			this.rbSearchMatch4.Location = new System.Drawing.Point(147, 19);
			this.rbSearchMatch4.Name = "rbSearchMatch4";
			this.rbSearchMatch4.Size = new System.Drawing.Size(33, 24);
			this.rbSearchMatch4.TabIndex = 55;
			this.rbSearchMatch4.Text = "4";
			this.rbSearchMatch4.UseVisualStyleBackColor = true;
			//
			// rbSearchMatch3
			//
			this.rbSearchMatch3.Location = new System.Drawing.Point(105, 19);
			this.rbSearchMatch3.Name = "rbSearchMatch3";
			this.rbSearchMatch3.Size = new System.Drawing.Size(39, 24);
			this.rbSearchMatch3.TabIndex = 54;
			this.rbSearchMatch3.Text = "3";
			this.rbSearchMatch3.UseVisualStyleBackColor = true;
			//
			// rbSearchMatch2
			//
			this.rbSearchMatch2.Location = new System.Drawing.Point(63, 19);
			this.rbSearchMatch2.Name = "rbSearchMatch2";
			this.rbSearchMatch2.Size = new System.Drawing.Size(39, 24);
			this.rbSearchMatch2.TabIndex = 53;
			this.rbSearchMatch2.Text = "2";
			this.rbSearchMatch2.UseVisualStyleBackColor = true;
			//
			// rbSearchRegex
			//
			this.rbSearchRegex.Checked = true;
			this.rbSearchRegex.Location = new System.Drawing.Point(6, 19);
			this.rbSearchRegex.Name = "rbSearchRegex";
			this.rbSearchRegex.Size = new System.Drawing.Size(54, 24);
			this.rbSearchRegex.TabIndex = 52;
			this.rbSearchRegex.TabStop = true;
			this.rbSearchRegex.Text = "regex";
			this.rbSearchRegex.UseVisualStyleBackColor = true;
			//
			// btClearSearchPattenr
			//
			this.btClearSearchPattenr.Location = new System.Drawing.Point(263, 311);
			this.btClearSearchPattenr.Name = "btClearSearchPattenr";
			this.btClearSearchPattenr.Size = new System.Drawing.Size(41, 25);
			this.btClearSearchPattenr.TabIndex = 45;
			this.btClearSearchPattenr.Text = "Clear";
			this.btClearSearchPattenr.UseVisualStyleBackColor = true;
			this.btClearSearchPattenr.Click += new System.EventHandler(this.BtClearSearchPattenrClick);
			//
			// btSetBoardAsHint
			//
			this.btSetBoardAsHint.Location = new System.Drawing.Point(196, 28);
			this.btSetBoardAsHint.Name = "btSetBoardAsHint";
			this.btSetBoardAsHint.Size = new System.Drawing.Size(109, 25);
			this.btSetBoardAsHint.TabIndex = 44;
			this.btSetBoardAsHint.Text = "Set Model as Hint";
			this.btSetBoardAsHint.UseVisualStyleBackColor = true;
			this.btSetBoardAsHint.Click += new System.EventHandler(this.BtSetBoardAsHintClick);
			//
			// label7
			//
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(125, 712);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(41, 17);
			this.label7.TabIndex = 43;
			this.label7.Text = "Tiles";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label16
			//
			this.label16.BackColor = System.Drawing.Color.Transparent;
			this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label16.Location = new System.Drawing.Point(91, 712);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(36, 17);
			this.label16.TabIndex = 42;
			this.label16.Text = "Cells";
			this.label16.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// dspNumScarceTiles
			//
			this.dspNumScarceTiles.BackColor = System.Drawing.Color.Orange;
			this.dspNumScarceTiles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dspNumScarceTiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.dspNumScarceTiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dspNumScarceTiles.Location = new System.Drawing.Point(130, 777);
			this.dspNumScarceTiles.Name = "dspNumScarceTiles";
			this.dspNumScarceTiles.Size = new System.Drawing.Size(30, 22);
			this.dspNumScarceTiles.TabIndex = 40;
			this.dspNumScarceTiles.Text = "0";
			this.dspNumScarceTiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// dspNumSwappableTiles
			//
			this.dspNumSwappableTiles.BackColor = System.Drawing.Color.Yellow;
			this.dspNumSwappableTiles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dspNumSwappableTiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.dspNumSwappableTiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dspNumSwappableTiles.Location = new System.Drawing.Point(130, 755);
			this.dspNumSwappableTiles.Name = "dspNumSwappableTiles";
			this.dspNumSwappableTiles.Size = new System.Drawing.Size(30, 22);
			this.dspNumSwappableTiles.TabIndex = 39;
			this.dspNumSwappableTiles.Text = "0";
			this.dspNumSwappableTiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// dspNumFillableTiles
			//
			this.dspNumFillableTiles.BackColor = System.Drawing.Color.LightGreen;
			this.dspNumFillableTiles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dspNumFillableTiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.dspNumFillableTiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dspNumFillableTiles.Location = new System.Drawing.Point(130, 733);
			this.dspNumFillableTiles.Name = "dspNumFillableTiles";
			this.dspNumFillableTiles.Size = new System.Drawing.Size(30, 22);
			this.dspNumFillableTiles.TabIndex = 38;
			this.dspNumFillableTiles.Text = "0";
			this.dspNumFillableTiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// dspNumInvalidCells
			//
			this.dspNumInvalidCells.BackColor = System.Drawing.Color.Red;
			this.dspNumInvalidCells.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dspNumInvalidCells.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.dspNumInvalidCells.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dspNumInvalidCells.Location = new System.Drawing.Point(94, 799);
			this.dspNumInvalidCells.Name = "dspNumInvalidCells";
			this.dspNumInvalidCells.Size = new System.Drawing.Size(30, 22);
			this.dspNumInvalidCells.TabIndex = 35;
			this.dspNumInvalidCells.Text = "0";
			this.dspNumInvalidCells.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label13
			//
			this.label13.BackColor = System.Drawing.Color.Transparent;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(6, 799);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(85, 22);
			this.label13.TabIndex = 34;
			this.label13.Text = "# Invalid";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// dspNumScarceCells
			//
			this.dspNumScarceCells.BackColor = System.Drawing.Color.Orange;
			this.dspNumScarceCells.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dspNumScarceCells.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.dspNumScarceCells.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dspNumScarceCells.Location = new System.Drawing.Point(94, 777);
			this.dspNumScarceCells.Name = "dspNumScarceCells";
			this.dspNumScarceCells.Size = new System.Drawing.Size(30, 22);
			this.dspNumScarceCells.TabIndex = 33;
			this.dspNumScarceCells.Text = "0";
			this.dspNumScarceCells.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label11
			//
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(6, 777);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(85, 22);
			this.label11.TabIndex = 32;
			this.label11.Text = "# Scarce";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// dspNumSwappableCells
			//
			this.dspNumSwappableCells.BackColor = System.Drawing.Color.Yellow;
			this.dspNumSwappableCells.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dspNumSwappableCells.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.dspNumSwappableCells.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dspNumSwappableCells.Location = new System.Drawing.Point(94, 755);
			this.dspNumSwappableCells.Name = "dspNumSwappableCells";
			this.dspNumSwappableCells.Size = new System.Drawing.Size(30, 22);
			this.dspNumSwappableCells.TabIndex = 31;
			this.dspNumSwappableCells.Text = "0";
			this.dspNumSwappableCells.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label8
			//
			this.label8.BackColor = System.Drawing.Color.Transparent;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(6, 755);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(85, 22);
			this.label8.TabIndex = 30;
			this.label8.Text = "# Swappable";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// dspNumFillableCells
			//
			this.dspNumFillableCells.BackColor = System.Drawing.Color.LightGreen;
			this.dspNumFillableCells.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dspNumFillableCells.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.dspNumFillableCells.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dspNumFillableCells.Location = new System.Drawing.Point(94, 733);
			this.dspNumFillableCells.Name = "dspNumFillableCells";
			this.dspNumFillableCells.Size = new System.Drawing.Size(30, 22);
			this.dspNumFillableCells.TabIndex = 29;
			this.dspNumFillableCells.Text = "0";
			this.dspNumFillableCells.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label2
			//
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(6, 733);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 22);
			this.label2.TabIndex = 28;
			this.label2.Text = "# Fillable";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// btClearOverlays
			//
			this.btClearOverlays.Location = new System.Drawing.Point(196, 263);
			this.btClearOverlays.Name = "btClearOverlays";
			this.btClearOverlays.Size = new System.Drawing.Size(109, 25);
			this.btClearOverlays.TabIndex = 27;
			this.btClearOverlays.Text = "Clear Overlays";
			this.btClearOverlays.UseVisualStyleBackColor = true;
			this.btClearOverlays.Click += new System.EventHandler(this.BtClearOverlaysClick);
			//
			// btCountIntersectingTiles
			//
			this.btCountIntersectingTiles.Location = new System.Drawing.Point(196, 238);
			this.btCountIntersectingTiles.Name = "btCountIntersectingTiles";
			this.btCountIntersectingTiles.Size = new System.Drawing.Size(109, 25);
			this.btCountIntersectingTiles.TabIndex = 26;
			this.btCountIntersectingTiles.Text = "Show Matches";
			this.btCountIntersectingTiles.UseVisualStyleBackColor = true;
			this.btCountIntersectingTiles.Click += new System.EventHandler(this.BtCountIntersectingTilesClick);
			//
			// btPlaceTile
			//
			this.btPlaceTile.Location = new System.Drawing.Point(196, 288);
			this.btPlaceTile.Name = "btPlaceTile";
			this.btPlaceTile.Size = new System.Drawing.Size(109, 23);
			this.btPlaceTile.TabIndex = 25;
			this.btPlaceTile.Text = "View Tileset";
			this.btPlaceTile.UseVisualStyleBackColor = true;
			this.btPlaceTile.Click += new System.EventHandler(this.BtPlaceTileClick);
			//
			// tabControl2
			//
			this.tabControl2.Controls.Add(this.tabResultsFree);
			this.tabControl2.Controls.Add(this.tabResultsUsed);
			this.tabControl2.Location = new System.Drawing.Point(0, 411);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(311, 298);
			this.tabControl2.TabIndex = 24;
			//
			// tabResultsFree
			//
			this.tabResultsFree.Controls.Add(this.searchFreeResultsImages);
			this.tabResultsFree.Location = new System.Drawing.Point(4, 22);
			this.tabResultsFree.Margin = new System.Windows.Forms.Padding(0);
			this.tabResultsFree.Name = "tabResultsFree";
			this.tabResultsFree.Size = new System.Drawing.Size(303, 272);
			this.tabResultsFree.TabIndex = 0;
			this.tabResultsFree.Text = "Free";
			this.tabResultsFree.UseVisualStyleBackColor = true;
			//
			// searchFreeResultsImages
			//
			this.searchFreeResultsImages.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.searchFreeResultsImages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.searchFreeResultsImages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.searchFreeResultsImages.HideSelection = false;
			this.searchFreeResultsImages.Location = new System.Drawing.Point(0, 0);
			this.searchFreeResultsImages.MultiSelect = false;
			this.searchFreeResultsImages.Name = "searchFreeResultsImages";
			this.searchFreeResultsImages.ShowGroups = false;
			this.searchFreeResultsImages.ShowItemToolTips = true;
			this.searchFreeResultsImages.Size = new System.Drawing.Size(303, 272);
			this.searchFreeResultsImages.TabIndex = 56;
			this.searchFreeResultsImages.TileSize = new System.Drawing.Size(50, 50);
			this.searchFreeResultsImages.UseCompatibleStateImageBehavior = false;
			this.searchFreeResultsImages.SelectedIndexChanged += new System.EventHandler(this.SearchFreeResultsImagesSelectedIndexChanged);
			//
			// tabResultsUsed
			//
			this.tabResultsUsed.Controls.Add(this.searchUsedResultsImages);
			this.tabResultsUsed.Location = new System.Drawing.Point(4, 22);
			this.tabResultsUsed.Margin = new System.Windows.Forms.Padding(0);
			this.tabResultsUsed.Name = "tabResultsUsed";
			this.tabResultsUsed.Size = new System.Drawing.Size(303, 272);
			this.tabResultsUsed.TabIndex = 1;
			this.tabResultsUsed.Text = "Used";
			this.tabResultsUsed.UseVisualStyleBackColor = true;
			//
			// searchUsedResultsImages
			//
			this.searchUsedResultsImages.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.searchUsedResultsImages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.searchUsedResultsImages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.searchUsedResultsImages.HideSelection = false;
			this.searchUsedResultsImages.Location = new System.Drawing.Point(0, 0);
			this.searchUsedResultsImages.MultiSelect = false;
			this.searchUsedResultsImages.Name = "searchUsedResultsImages";
			this.searchUsedResultsImages.ShowGroups = false;
			this.searchUsedResultsImages.ShowItemToolTips = true;
			this.searchUsedResultsImages.Size = new System.Drawing.Size(303, 272);
			this.searchUsedResultsImages.TabIndex = 57;
			this.searchUsedResultsImages.TileSize = new System.Drawing.Size(50, 50);
			this.searchUsedResultsImages.UseCompatibleStateImageBehavior = false;
			this.searchUsedResultsImages.SelectedIndexChanged += new System.EventHandler(this.SearchUsedResultsImagesSelectedIndexChanged);
			//
			// labelModel
			//
			this.labelModel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelModel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelModel.Location = new System.Drawing.Point(60, 31);
			this.labelModel.Name = "labelModel";
			this.labelModel.Size = new System.Drawing.Size(131, 22);
			this.labelModel.TabIndex = 16;
			this.labelModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// labelTileset
			//
			this.labelTileset.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelTileset.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelTileset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelTileset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTileset.Location = new System.Drawing.Point(60, 8);
			this.labelTileset.Name = "labelTileset";
			this.labelTileset.Size = new System.Drawing.Size(131, 22);
			this.labelTileset.TabIndex = 15;
			this.labelTileset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// selTilePattern
			//
			this.selTilePattern.BackColor = System.Drawing.Color.WhiteSmoke;
			this.selTilePattern.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.selTilePattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.selTilePattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.selTilePattern.Location = new System.Drawing.Point(114, 83);
			this.selTilePattern.Name = "selTilePattern";
			this.selTilePattern.Size = new System.Drawing.Size(77, 22);
			this.selTilePattern.TabIndex = 14;
			this.selTilePattern.Text = "R0 MMMM";
			this.selTilePattern.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// selColRow
			//
			this.selColRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.selColRow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.selColRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.selColRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.selColRow.Location = new System.Drawing.Point(60, 57);
			this.selColRow.Name = "selColRow";
			this.selColRow.Size = new System.Drawing.Size(131, 22);
			this.selColRow.TabIndex = 12;
			this.selColRow.Text = "0,0 : 0 : P000";
			this.selColRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// selTile
			//
			this.selTile.BackColor = System.Drawing.Color.WhiteSmoke;
			this.selTile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.selTile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.selTile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.selTile.Location = new System.Drawing.Point(60, 83);
			this.selTile.Name = "selTile";
			this.selTile.Size = new System.Drawing.Size(53, 22);
			this.selTile.TabIndex = 11;
			this.selTile.Text = "#000";
			this.selTile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label6
			//
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(7, 81);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(54, 21);
			this.label6.TabIndex = 10;
			this.label6.Text = "Tile";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label4
			//
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(3, 30);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 22);
			this.label4.TabIndex = 8;
			this.label4.Text = "Model";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// label3
			//
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(5, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 22);
			this.label3.TabIndex = 7;
			this.label3.Text = "Tileset";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// selMatchPattern
			//
			this.selMatchPattern.Location = new System.Drawing.Point(9, 314);
			this.selMatchPattern.Name = "selMatchPattern";
			this.selMatchPattern.Size = new System.Drawing.Size(182, 20);
			this.selMatchPattern.TabIndex = 2;
			//
			// btSearch
			//
			this.btSearch.Location = new System.Drawing.Point(196, 311);
			this.btSearch.Name = "btSearch";
			this.btSearch.Size = new System.Drawing.Size(67, 25);
			this.btSearch.TabIndex = 1;
			this.btSearch.Text = "Search";
			this.btSearch.UseVisualStyleBackColor = true;
			this.btSearch.Click += new System.EventHandler(this.BtSearchClick);
			//
			// pb_board
			//
			this.pb_board.BackColor = System.Drawing.Color.LightGray;
			this.pb_board.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pb_board.ImageLocation = "";
			this.pb_board.InitialImage = null;
			this.pb_board.Location = new System.Drawing.Point(311, 0);
			this.pb_board.Name = "pb_board";
			this.pb_board.Size = new System.Drawing.Size(815, 823);
			this.pb_board.TabIndex = 0;
			this.pb_board.TabStop = false;
			this.pb_board.MouseUp += new System.Windows.Forms.MouseEventHandler(this.handleBoardMouseClick);
			//
			// tabSolver1
			//
			this.tabSolver1.Controls.Add(this.cbCountBorderTriplets);
			this.tabSolver1.Controls.Add(this.cbPauseOnBorderTriplet);
			this.tabSolver1.Controls.Add(this.cbPauseNumBGColours);
			this.tabSolver1.Controls.Add(this.inputS1NumBGColours);
			this.tabSolver1.Controls.Add(this.cbBGColourStats);
			this.tabSolver1.Controls.Add(this.cbSaveSolutions);
			this.tabSolver1.Controls.Add(this.btCopyCellFilter);
			this.tabSolver1.Controls.Add(this.btClearCellFilter);
			this.tabSolver1.Controls.Add(this.btStepForward);
			this.tabSolver1.Controls.Add(this.btStepBack);
			this.tabSolver1.Controls.Add(this.btCopyCmd);
			this.tabSolver1.Controls.Add(this.btClearCmd);
			this.tabSolver1.Controls.Add(this.btRunCmd);
			this.tabSolver1.Controls.Add(this.inputCmd);
			this.tabSolver1.Controls.Add(this.btCopySolvePath);
			this.tabSolver1.Controls.Add(this.btClearSolvePath);
			this.tabSolver1.Controls.Add(this.selDumpOptions);
			this.tabSolver1.Controls.Add(this.btCopyDump);
			this.tabSolver1.Controls.Add(this.cbTrackTileDistribution);
			this.tabSolver1.Controls.Add(this.btStop);
			this.tabSolver1.Controls.Add(this.cbS1UseRegionRestrictions);
			this.tabSolver1.Controls.Add(this.labelUniquePatternOrientations);
			this.tabSolver1.Controls.Add(this.label14);
			this.tabSolver1.Controls.Add(this.label10);
			this.tabSolver1.Controls.Add(this.labelSpeed);
			this.tabSolver1.Controls.Add(this.btClearScores);
			this.tabSolver1.Controls.Add(this.btClearSolutions);
			this.tabSolver1.Controls.Add(this.labelSolutions);
			this.tabSolver1.Controls.Add(this.labels11);
			this.tabSolver1.Controls.Add(this.cbCellFilter);
			this.tabSolver1.Controls.Add(this.inputCellFilter);
			this.tabSolver1.Controls.Add(this.label15);
			this.tabSolver1.Controls.Add(this.selS1PathFilter);
			this.tabSolver1.Controls.Add(this.button2);
			this.tabSolver1.Controls.Add(this.labelS1Seed);
			this.tabSolver1.Controls.Add(this.label44);
			this.tabSolver1.Controls.Add(this.label29);
			this.tabSolver1.Controls.Add(this.inputS1CurrentSolveMethod);
			this.tabSolver1.Controls.Add(this.label30);
			this.tabSolver1.Controls.Add(this.cbS1EnableScoreTrigger);
			this.tabSolver1.Controls.Add(this.cbS1EnableBacktrackLimit);
			this.tabSolver1.Controls.Add(this.grS1IterationControl);
			this.tabSolver1.Controls.Add(this.grS1AutoScoreTrigger);
			this.tabSolver1.Controls.Add(this.inputS1SolvePath);
			this.tabSolver1.Controls.Add(this.label43);
			this.tabSolver1.Controls.Add(this.selDebugLevel);
			this.tabSolver1.Controls.Add(this.grS1RandomTileQueue);
			this.tabSolver1.Controls.Add(this.grS1BacktrackOptions);
			this.tabSolver1.Controls.Add(this.cbS1EnforceFillableCells);
			this.tabSolver1.Controls.Add(this.label35);
			this.tabSolver1.Controls.Add(this.btS1Reset);
			this.tabSolver1.Controls.Add(this.btS1ClearStats);
			this.tabSolver1.Controls.Add(this.logS1Stats);
			this.tabSolver1.Controls.Add(this.cbS1UseRandomSeed);
			this.tabSolver1.Controls.Add(this.btS1Stats);
			this.tabSolver1.Controls.Add(this.labelS1TilesPlaced);
			this.tabSolver1.Controls.Add(this.labelS1QueueProgress);
			this.tabSolver1.Controls.Add(this.labelS1NumIterations);
			this.tabSolver1.Controls.Add(this.btClearLogS1);
			this.tabSolver1.Controls.Add(this.logS1);
			this.tabSolver1.Controls.Add(this.btSaveQS1);
			this.tabSolver1.Controls.Add(this.selQS1);
			this.tabSolver1.Controls.Add(this.btLoadQS1);
			this.tabSolver1.Controls.Add(this.btResumeS1);
			this.tabSolver1.Controls.Add(this.btPauseS1);
			this.tabSolver1.Controls.Add(this.label32);
			this.tabSolver1.Controls.Add(this.labelS1MaxTilesPlaced);
			this.tabSolver1.Controls.Add(this.label34);
			this.tabSolver1.Controls.Add(this.label38);
			this.tabSolver1.Controls.Add(this.labelCellPath);
			this.tabSolver1.Controls.Add(this.label41);
			this.tabSolver1.Controls.Add(this.label42);
			this.tabSolver1.Controls.Add(this.labelS1BestScore);
			this.tabSolver1.Controls.Add(this.label46);
			this.tabSolver1.Controls.Add(this.label49);
			this.tabSolver1.Controls.Add(this.selS1SolveMethod);
			this.tabSolver1.Controls.Add(this.btStartS1);
			this.tabSolver1.Location = new System.Drawing.Point(4, 22);
			this.tabSolver1.Name = "tabSolver1";
			this.tabSolver1.Size = new System.Drawing.Size(1126, 823);
			this.tabSolver1.TabIndex = 5;
			this.tabSolver1.Text = "Solver1";
			this.tabSolver1.UseVisualStyleBackColor = true;
			this.tabSolver1.Enter += new System.EventHandler(this.TabSolver1Enter);
			//
			// cbCountBorderTriplets
			//
			this.cbCountBorderTriplets.AutoSize = true;
			this.cbCountBorderTriplets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbCountBorderTriplets.Location = new System.Drawing.Point(897, 7);
			this.cbCountBorderTriplets.Name = "cbCountBorderTriplets";
			this.cbCountBorderTriplets.Size = new System.Drawing.Size(119, 17);
			this.cbCountBorderTriplets.TabIndex = 250;
			this.cbCountBorderTriplets.Text = "count border triplets";
			this.cbCountBorderTriplets.UseVisualStyleBackColor = true;
			//
			// cbPauseOnBorderTriplet
			//
			this.cbPauseOnBorderTriplet.AutoSize = true;
			this.cbPauseOnBorderTriplet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbPauseOnBorderTriplet.Location = new System.Drawing.Point(897, 25);
			this.cbPauseOnBorderTriplet.Name = "cbPauseOnBorderTriplet";
			this.cbPauseOnBorderTriplet.Size = new System.Drawing.Size(131, 17);
			this.cbPauseOnBorderTriplet.TabIndex = 249;
			this.cbPauseOnBorderTriplet.Text = "pause on border triplet";
			this.cbPauseOnBorderTriplet.UseVisualStyleBackColor = true;
			//
			// cbPauseNumBGColours
			//
			this.cbPauseNumBGColours.AutoSize = true;
			this.cbPauseNumBGColours.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbPauseNumBGColours.Location = new System.Drawing.Point(897, 60);
			this.cbPauseNumBGColours.Name = "cbPauseNumBGColours";
			this.cbPauseNumBGColours.Size = new System.Drawing.Size(145, 17);
			this.cbPauseNumBGColours.TabIndex = 248;
			this.cbPauseNumBGColours.Text = "pause if numBGColours =";
			this.cbPauseNumBGColours.UseVisualStyleBackColor = true;
			//
			// inputS1NumBGColours
			//
			this.inputS1NumBGColours.Location = new System.Drawing.Point(1042, 57);
			this.inputS1NumBGColours.MaxLength = 3;
			this.inputS1NumBGColours.Name = "inputS1NumBGColours";
			this.inputS1NumBGColours.Size = new System.Drawing.Size(35, 20);
			this.inputS1NumBGColours.TabIndex = 247;
			this.inputS1NumBGColours.Text = "4";
			//
			// cbBGColourStats
			//
			this.cbBGColourStats.AutoSize = true;
			this.cbBGColourStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbBGColourStats.Location = new System.Drawing.Point(897, 42);
			this.cbBGColourStats.Name = "cbBGColourStats";
			this.cbBGColourStats.Size = new System.Drawing.Size(92, 17);
			this.cbBGColourStats.TabIndex = 246;
			this.cbBGColourStats.Text = "bgcolour stats";
			this.cbBGColourStats.UseVisualStyleBackColor = true;
			//
			// cbSaveSolutions
			//
			this.cbSaveSolutions.AutoSize = true;
			this.cbSaveSolutions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbSaveSolutions.Location = new System.Drawing.Point(717, 59);
			this.cbSaveSolutions.Name = "cbSaveSolutions";
			this.cbSaveSolutions.Size = new System.Drawing.Size(93, 17);
			this.cbSaveSolutions.TabIndex = 245;
			this.cbSaveSolutions.Text = "save solutions";
			this.cbSaveSolutions.UseVisualStyleBackColor = true;
			//
			// btCopyCellFilter
			//
			this.btCopyCellFilter.Location = new System.Drawing.Point(847, 795);
			this.btCopyCellFilter.Name = "btCopyCellFilter";
			this.btCopyCellFilter.Size = new System.Drawing.Size(46, 23);
			this.btCopyCellFilter.TabIndex = 244;
			this.btCopyCellFilter.Text = "Copy";
			this.btCopyCellFilter.UseVisualStyleBackColor = true;
			this.btCopyCellFilter.Click += new System.EventHandler(this.BtCopyCellFilterClick);
			//
			// btClearCellFilter
			//
			this.btClearCellFilter.Location = new System.Drawing.Point(897, 795);
			this.btClearCellFilter.Name = "btClearCellFilter";
			this.btClearCellFilter.Size = new System.Drawing.Size(51, 23);
			this.btClearCellFilter.TabIndex = 243;
			this.btClearCellFilter.Text = "Clear";
			this.btClearCellFilter.UseVisualStyleBackColor = true;
			this.btClearCellFilter.Click += new System.EventHandler(this.BtClearCellFilterClick);
			//
			// btStepForward
			//
			this.btStepForward.Location = new System.Drawing.Point(217, 5);
			this.btStepForward.Name = "btStepForward";
			this.btStepForward.Size = new System.Drawing.Size(33, 23);
			this.btStepForward.TabIndex = 242;
			this.btStepForward.Text = ">";
			this.btStepForward.UseVisualStyleBackColor = true;
			this.btStepForward.Click += new System.EventHandler(this.BtStepForwardClick);
			//
			// btStepBack
			//
			this.btStepBack.Location = new System.Drawing.Point(178, 5);
			this.btStepBack.Name = "btStepBack";
			this.btStepBack.Size = new System.Drawing.Size(33, 23);
			this.btStepBack.TabIndex = 241;
			this.btStepBack.Text = "<";
			this.btStepBack.UseVisualStyleBackColor = true;
			this.btStepBack.Click += new System.EventHandler(this.BtStepBackClick);
			//
			// btCopyCmd
			//
			this.btCopyCmd.Location = new System.Drawing.Point(623, 769);
			this.btCopyCmd.Name = "btCopyCmd";
			this.btCopyCmd.Size = new System.Drawing.Size(70, 23);
			this.btCopyCmd.TabIndex = 240;
			this.btCopyCmd.Text = "Copy Cmd";
			this.btCopyCmd.UseVisualStyleBackColor = true;
			this.btCopyCmd.Click += new System.EventHandler(this.BtCopyCmdClick);
			//
			// btClearCmd
			//
			this.btClearCmd.Location = new System.Drawing.Point(623, 797);
			this.btClearCmd.Name = "btClearCmd";
			this.btClearCmd.Size = new System.Drawing.Size(70, 23);
			this.btClearCmd.TabIndex = 239;
			this.btClearCmd.Text = "Clear Cmd";
			this.btClearCmd.UseVisualStyleBackColor = true;
			this.btClearCmd.Click += new System.EventHandler(this.BtClearCmdClick);
			//
			// btRunCmd
			//
			this.btRunCmd.Location = new System.Drawing.Point(623, 740);
			this.btRunCmd.Name = "btRunCmd";
			this.btRunCmd.Size = new System.Drawing.Size(70, 23);
			this.btRunCmd.TabIndex = 238;
			this.btRunCmd.Text = "Run Cmd";
			this.btRunCmd.UseVisualStyleBackColor = true;
			this.btRunCmd.Click += new System.EventHandler(this.BtRunCmdClick);
			//
			// inputCmd
			//
			this.inputCmd.Location = new System.Drawing.Point(9, 740);
			this.inputCmd.Multiline = true;
			this.inputCmd.Name = "inputCmd";
			this.inputCmd.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.inputCmd.Size = new System.Drawing.Size(608, 80);
			this.inputCmd.TabIndex = 237;
			//
			// btCopySolvePath
			//
			this.btCopySolvePath.Location = new System.Drawing.Point(596, 294);
			this.btCopySolvePath.Name = "btCopySolvePath";
			this.btCopySolvePath.Size = new System.Drawing.Size(46, 23);
			this.btCopySolvePath.TabIndex = 236;
			this.btCopySolvePath.Text = "Copy";
			this.btCopySolvePath.UseVisualStyleBackColor = true;
			this.btCopySolvePath.Click += new System.EventHandler(this.BtCopySolvePathClick);
			//
			// btClearSolvePath
			//
			this.btClearSolvePath.Location = new System.Drawing.Point(646, 294);
			this.btClearSolvePath.Name = "btClearSolvePath";
			this.btClearSolvePath.Size = new System.Drawing.Size(51, 23);
			this.btClearSolvePath.TabIndex = 235;
			this.btClearSolvePath.Text = "Clear";
			this.btClearSolvePath.UseVisualStyleBackColor = true;
			this.btClearSolvePath.Click += new System.EventHandler(this.BtClearSolvePathClick);
			//
			// selDumpOptions
			//
			this.selDumpOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selDumpOptions.FormattingEnabled = true;
			this.selDumpOptions.ImeMode = System.Windows.Forms.ImeMode.On;
			this.selDumpOptions.Items.AddRange(new object[] {
									"bgcolour stats",
									"board",
									"iteration log",
									"pattern distribution",
									"queue",
									"queue detail",
									"scores",
									"search stats",
									"solution stats",
									"stats",
									"tile distribution",
									"tile stats",
									"tileset",
									"used tiles"});
			this.selDumpOptions.Location = new System.Drawing.Point(9, 264);
			this.selDumpOptions.Name = "selDumpOptions";
			this.selDumpOptions.Size = new System.Drawing.Size(192, 21);
			this.selDumpOptions.Sorted = true;
			this.selDumpOptions.TabIndex = 234;
			//
			// btCopyDump
			//
			this.btCopyDump.Location = new System.Drawing.Point(269, 264);
			this.btCopyDump.Name = "btCopyDump";
			this.btCopyDump.Size = new System.Drawing.Size(48, 23);
			this.btCopyDump.TabIndex = 233;
			this.btCopyDump.Text = "Copy";
			this.btCopyDump.UseVisualStyleBackColor = true;
			this.btCopyDump.Click += new System.EventHandler(this.BtCopyDumpClick);
			//
			// cbTrackTileDistribution
			//
			this.cbTrackTileDistribution.AutoSize = true;
			this.cbTrackTileDistribution.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbTrackTileDistribution.Location = new System.Drawing.Point(717, 42);
			this.cbTrackTileDistribution.Name = "cbTrackTileDistribution";
			this.cbTrackTileDistribution.Size = new System.Drawing.Size(119, 17);
			this.cbTrackTileDistribution.TabIndex = 230;
			this.cbTrackTileDistribution.Text = "track tile distribution";
			this.cbTrackTileDistribution.UseVisualStyleBackColor = true;
			//
			// btStop
			//
			this.btStop.Location = new System.Drawing.Point(320, 32);
			this.btStop.Name = "btStop";
			this.btStop.Size = new System.Drawing.Size(59, 23);
			this.btStop.TabIndex = 232;
			this.btStop.Text = "Stop";
			this.btStop.UseVisualStyleBackColor = true;
			this.btStop.Click += new System.EventHandler(this.BtStopClick);
			//
			// cbS1UseRegionRestrictions
			//
			this.cbS1UseRegionRestrictions.AutoSize = true;
			this.cbS1UseRegionRestrictions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1UseRegionRestrictions.Location = new System.Drawing.Point(717, 7);
			this.cbS1UseRegionRestrictions.Name = "cbS1UseRegionRestrictions";
			this.cbS1UseRegionRestrictions.Size = new System.Drawing.Size(128, 17);
			this.cbS1UseRegionRestrictions.TabIndex = 231;
			this.cbS1UseRegionRestrictions.Text = "use region restrictions";
			this.cbS1UseRegionRestrictions.UseVisualStyleBackColor = true;
			//
			// labelUniquePatternOrientations
			//
			this.labelUniquePatternOrientations.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelUniquePatternOrientations.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelUniquePatternOrientations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelUniquePatternOrientations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelUniquePatternOrientations.Location = new System.Drawing.Point(241, 239);
			this.labelUniquePatternOrientations.Name = "labelUniquePatternOrientations";
			this.labelUniquePatternOrientations.Size = new System.Drawing.Size(92, 22);
			this.labelUniquePatternOrientations.TabIndex = 230;
			this.labelUniquePatternOrientations.Text = "0";
			this.labelUniquePatternOrientations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label14
			//
			this.label14.BackColor = System.Drawing.Color.Transparent;
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(239, 216);
			this.label14.Name = "label14";
			this.label14.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label14.Size = new System.Drawing.Size(94, 22);
			this.label14.TabIndex = 229;
			this.label14.Text = "Pattern Orients";
			this.label14.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label10
			//
			this.label10.BackColor = System.Drawing.Color.Transparent;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(11, 218);
			this.label10.Name = "label10";
			this.label10.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label10.Size = new System.Drawing.Size(49, 22);
			this.label10.TabIndex = 228;
			this.label10.Text = "Speed";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelSpeed
			//
			this.labelSpeed.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelSpeed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSpeed.Location = new System.Drawing.Point(58, 218);
			this.labelSpeed.Name = "labelSpeed";
			this.labelSpeed.Size = new System.Drawing.Size(99, 22);
			this.labelSpeed.TabIndex = 227;
			this.labelSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// btClearScores
			//
			this.btClearScores.Location = new System.Drawing.Point(162, 217);
			this.btClearScores.Name = "btClearScores";
			this.btClearScores.Size = new System.Drawing.Size(73, 23);
			this.btClearScores.TabIndex = 226;
			this.btClearScores.Text = "Clear";
			this.btClearScores.UseVisualStyleBackColor = true;
			this.btClearScores.Click += new System.EventHandler(this.BtClearScoresClick);
			//
			// btClearSolutions
			//
			this.btClearSolutions.Location = new System.Drawing.Point(325, 171);
			this.btClearSolutions.Name = "btClearSolutions";
			this.btClearSolutions.Size = new System.Drawing.Size(73, 23);
			this.btClearSolutions.TabIndex = 225;
			this.btClearSolutions.Text = "Clear";
			this.btClearSolutions.UseVisualStyleBackColor = true;
			this.btClearSolutions.Click += new System.EventHandler(this.BtClearSolutionsClick);
			//
			// labelSolutions
			//
			this.labelSolutions.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelSolutions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelSolutions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelSolutions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSolutions.Location = new System.Drawing.Point(325, 146);
			this.labelSolutions.Name = "labelSolutions";
			this.labelSolutions.Size = new System.Drawing.Size(73, 22);
			this.labelSolutions.TabIndex = 224;
			this.labelSolutions.Text = "0 / 0";
			this.labelSolutions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// labels11
			//
			this.labels11.BackColor = System.Drawing.Color.Transparent;
			this.labels11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labels11.Location = new System.Drawing.Point(323, 123);
			this.labels11.Name = "labels11";
			this.labels11.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.labels11.Size = new System.Drawing.Size(75, 22);
			this.labels11.TabIndex = 223;
			this.labels11.Text = "Solutions";
			this.labels11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// cbCellFilter
			//
			this.cbCellFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbCellFilter.Location = new System.Drawing.Point(717, 629);
			this.cbCellFilter.Name = "cbCellFilter";
			this.cbCellFilter.Size = new System.Drawing.Size(231, 20);
			this.cbCellFilter.TabIndex = 222;
			this.cbCellFilter.Text = "enable cell filter (cellId, pattern)";
			this.cbCellFilter.UseVisualStyleBackColor = true;
			//
			// inputCellFilter
			//
			this.inputCellFilter.Location = new System.Drawing.Point(717, 649);
			this.inputCellFilter.Multiline = true;
			this.inputCellFilter.Name = "inputCellFilter";
			this.inputCellFilter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.inputCellFilter.Size = new System.Drawing.Size(231, 140);
			this.inputCellFilter.TabIndex = 220;
			//
			// label15
			//
			this.label15.BackColor = System.Drawing.Color.Transparent;
			this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.Location = new System.Drawing.Point(396, 209);
			this.label15.Name = "label15";
			this.label15.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label15.Size = new System.Drawing.Size(86, 22);
			this.label15.TabIndex = 219;
			this.label15.Text = "Path Filter";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// selS1PathFilter
			//
			this.selS1PathFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selS1PathFilter.FormattingEnabled = true;
			this.selS1PathFilter.Location = new System.Drawing.Point(489, 207);
			this.selS1PathFilter.Name = "selS1PathFilter";
			this.selS1PathFilter.Size = new System.Drawing.Size(206, 21);
			this.selS1PathFilter.TabIndex = 218;
			this.selS1PathFilter.SelectedIndexChanged += new System.EventHandler(this.SelS1PathFilterSelectedIndexChanged);
			//
			// button2
			//
			this.button2.Location = new System.Drawing.Point(270, 91);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(102, 23);
			this.button2.TabIndex = 217;
			this.button2.Text = "Save Results";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			//
			// labelS1Seed
			//
			this.labelS1Seed.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS1Seed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS1Seed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS1Seed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS1Seed.Location = new System.Drawing.Point(13, 148);
			this.labelS1Seed.Name = "labelS1Seed";
			this.labelS1Seed.Size = new System.Drawing.Size(81, 22);
			this.labelS1Seed.TabIndex = 216;
			this.labelS1Seed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label44
			//
			this.label44.BackColor = System.Drawing.Color.Transparent;
			this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label44.Location = new System.Drawing.Point(11, 125);
			this.label44.Name = "label44";
			this.label44.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label44.Size = new System.Drawing.Size(83, 22);
			this.label44.TabIndex = 215;
			this.label44.Text = "Seed";
			this.label44.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label29
			//
			this.label29.BackColor = System.Drawing.Color.Transparent;
			this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label29.Location = new System.Drawing.Point(382, 259);
			this.label29.Name = "label29";
			this.label29.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label29.Size = new System.Drawing.Size(100, 22);
			this.label29.TabIndex = 214;
			this.label29.Text = "Current Method";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// inputS1CurrentSolveMethod
			//
			this.inputS1CurrentSolveMethod.Location = new System.Drawing.Point(489, 261);
			this.inputS1CurrentSolveMethod.Name = "inputS1CurrentSolveMethod";
			this.inputS1CurrentSolveMethod.Size = new System.Drawing.Size(206, 20);
			this.inputS1CurrentSolveMethod.TabIndex = 208;
			//
			// label30
			//
			this.label30.BackColor = System.Drawing.Color.Transparent;
			this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label30.Location = new System.Drawing.Point(396, 236);
			this.label30.Name = "label30";
			this.label30.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label30.Size = new System.Drawing.Size(86, 22);
			this.label30.TabIndex = 213;
			this.label30.Text = "Solve Method";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// cbS1EnableScoreTrigger
			//
			this.cbS1EnableScoreTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1EnableScoreTrigger.Location = new System.Drawing.Point(717, 467);
			this.cbS1EnableScoreTrigger.Name = "cbS1EnableScoreTrigger";
			this.cbS1EnableScoreTrigger.Size = new System.Drawing.Size(192, 21);
			this.cbS1EnableScoreTrigger.TabIndex = 12;
			this.cbS1EnableScoreTrigger.Text = "enable score trigger";
			this.cbS1EnableScoreTrigger.UseVisualStyleBackColor = true;
			this.cbS1EnableScoreTrigger.CheckedChanged += new System.EventHandler(this.CbS1EnableScoreTriggerCheckedChanged);
			//
			// cbS1EnableBacktrackLimit
			//
			this.cbS1EnableBacktrackLimit.AutoSize = true;
			this.cbS1EnableBacktrackLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1EnableBacktrackLimit.Location = new System.Drawing.Point(717, 106);
			this.cbS1EnableBacktrackLimit.Name = "cbS1EnableBacktrackLimit";
			this.cbS1EnableBacktrackLimit.Size = new System.Drawing.Size(165, 17);
			this.cbS1EnableBacktrackLimit.TabIndex = 9;
			this.cbS1EnableBacktrackLimit.Text = "enable backtrack trigger";
			this.cbS1EnableBacktrackLimit.UseVisualStyleBackColor = true;
			this.cbS1EnableBacktrackLimit.CheckedChanged += new System.EventHandler(this.CbS1EnableBacktrackLimitCheckedChanged);
			//
			// grS1IterationControl
			//
			this.grS1IterationControl.Controls.Add(this.cbS1IterationLimitScoreProximity);
			this.grS1IterationControl.Controls.Add(this.inputS1IterationScoreProximity);
			this.grS1IterationControl.Controls.Add(this.label31);
			this.grS1IterationControl.Controls.Add(this.label39);
			this.grS1IterationControl.Controls.Add(this.inputS1RunLength);
			this.grS1IterationControl.Controls.Add(this.cbS1IterationLimit);
			this.grS1IterationControl.Controls.Add(this.cbS1AutoSaveByInterval);
			this.grS1IterationControl.Controls.Add(this.inputS1AutoSaveIterations);
			this.grS1IterationControl.Controls.Add(this.inputS1MaxIterations);
			this.grS1IterationControl.Location = new System.Drawing.Point(424, 27);
			this.grS1IterationControl.Name = "grS1IterationControl";
			this.grS1IterationControl.Size = new System.Drawing.Size(271, 141);
			this.grS1IterationControl.TabIndex = 209;
			this.grS1IterationControl.TabStop = false;
			this.grS1IterationControl.Text = "iteration control";
			//
			// cbS1IterationLimitScoreProximity
			//
			this.cbS1IterationLimitScoreProximity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1IterationLimitScoreProximity.Location = new System.Drawing.Point(11, 68);
			this.cbS1IterationLimitScoreProximity.Name = "cbS1IterationLimitScoreProximity";
			this.cbS1IterationLimitScoreProximity.Size = new System.Drawing.Size(170, 24);
			this.cbS1IterationLimitScoreProximity.TabIndex = 222;
			this.cbS1IterationLimitScoreProximity.Text = "continue if max score delta <=";
			this.cbS1IterationLimitScoreProximity.UseVisualStyleBackColor = true;
			//
			// inputS1IterationScoreProximity
			//
			this.inputS1IterationScoreProximity.Location = new System.Drawing.Point(187, 70);
			this.inputS1IterationScoreProximity.MaxLength = 3;
			this.inputS1IterationScoreProximity.Name = "inputS1IterationScoreProximity";
			this.inputS1IterationScoreProximity.Size = new System.Drawing.Size(31, 20);
			this.inputS1IterationScoreProximity.TabIndex = 223;
			this.inputS1IterationScoreProximity.Text = "20";
			//
			// label31
			//
			this.label31.BackColor = System.Drawing.Color.Transparent;
			this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label31.Location = new System.Drawing.Point(11, 114);
			this.label31.Name = "label31";
			this.label31.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label31.Size = new System.Drawing.Size(47, 22);
			this.label31.TabIndex = 221;
			this.label31.Text = "# runs";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// label39
			//
			this.label39.AccessibleDescription = "gheyey";
			this.label39.BackColor = System.Drawing.Color.Transparent;
			this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label39.Location = new System.Drawing.Point(11, 90);
			this.label39.Name = "label39";
			this.label39.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label39.Size = new System.Drawing.Size(170, 22);
			this.label39.TabIndex = 220;
			this.label39.Text = "Run Length ( 0 = unlimited )";
			this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// inputS1RunLength
			//
			this.inputS1RunLength.Location = new System.Drawing.Point(64, 114);
			this.inputS1RunLength.Name = "inputS1RunLength";
			this.inputS1RunLength.Size = new System.Drawing.Size(143, 20);
			this.inputS1RunLength.TabIndex = 219;
			this.inputS1RunLength.Text = "0";
			//
			// cbS1IterationLimit
			//
			this.cbS1IterationLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1IterationLimit.Location = new System.Drawing.Point(11, 43);
			this.cbS1IterationLimit.Name = "cbS1IterationLimit";
			this.cbS1IterationLimit.Size = new System.Drawing.Size(114, 24);
			this.cbS1IterationLimit.TabIndex = 2;
			this.cbS1IterationLimit.Text = "Iteration limit / run";
			this.cbS1IterationLimit.UseVisualStyleBackColor = true;
			this.cbS1IterationLimit.CheckedChanged += new System.EventHandler(this.CbS1IterationLimitCheckedChanged);
			//
			// cbS1AutoSaveByInterval
			//
			this.cbS1AutoSaveByInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1AutoSaveByInterval.Location = new System.Drawing.Point(11, 18);
			this.cbS1AutoSaveByInterval.Name = "cbS1AutoSaveByInterval";
			this.cbS1AutoSaveByInterval.Size = new System.Drawing.Size(114, 24);
			this.cbS1AutoSaveByInterval.TabIndex = 0;
			this.cbS1AutoSaveByInterval.Text = "Auto save interval";
			this.cbS1AutoSaveByInterval.UseVisualStyleBackColor = true;
			this.cbS1AutoSaveByInterval.CheckedChanged += new System.EventHandler(this.CbS1AutoSaveByIntervalCheckedChanged);
			//
			// inputS1AutoSaveIterations
			//
			this.inputS1AutoSaveIterations.Enabled = false;
			this.inputS1AutoSaveIterations.Location = new System.Drawing.Point(187, 20);
			this.inputS1AutoSaveIterations.MaxLength = 16;
			this.inputS1AutoSaveIterations.Name = "inputS1AutoSaveIterations";
			this.inputS1AutoSaveIterations.Size = new System.Drawing.Size(77, 20);
			this.inputS1AutoSaveIterations.TabIndex = 1;
			this.inputS1AutoSaveIterations.Text = "10000";
			//
			// inputS1MaxIterations
			//
			this.inputS1MaxIterations.Enabled = false;
			this.inputS1MaxIterations.Location = new System.Drawing.Point(187, 45);
			this.inputS1MaxIterations.MaxLength = 16;
			this.inputS1MaxIterations.Name = "inputS1MaxIterations";
			this.inputS1MaxIterations.Size = new System.Drawing.Size(77, 20);
			this.inputS1MaxIterations.TabIndex = 3;
			this.inputS1MaxIterations.Text = "1000000";
			//
			// grS1AutoScoreTrigger
			//
			this.grS1AutoScoreTrigger.Controls.Add(this.cbS1PauseOnScore);
			this.grS1AutoScoreTrigger.Controls.Add(this.label33);
			this.grS1AutoScoreTrigger.Controls.Add(this.labelS1NextScoreTrigger);
			this.grS1AutoScoreTrigger.Controls.Add(this.cbS1AutoIncrementScore);
			this.grS1AutoScoreTrigger.Controls.Add(this.cbS1ScoreAutoSave);
			this.grS1AutoScoreTrigger.Controls.Add(this.label28);
			this.grS1AutoScoreTrigger.Controls.Add(this.inputS1ScoreTrigger);
			this.grS1AutoScoreTrigger.Enabled = false;
			this.grS1AutoScoreTrigger.Location = new System.Drawing.Point(717, 492);
			this.grS1AutoScoreTrigger.Name = "grS1AutoScoreTrigger";
			this.grS1AutoScoreTrigger.Size = new System.Drawing.Size(231, 135);
			this.grS1AutoScoreTrigger.TabIndex = 198;
			this.grS1AutoScoreTrigger.TabStop = false;
			this.grS1AutoScoreTrigger.Text = "action when num tiles reached";
			//
			// cbS1PauseOnScore
			//
			this.cbS1PauseOnScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1PauseOnScore.Location = new System.Drawing.Point(17, 67);
			this.cbS1PauseOnScore.Name = "cbS1PauseOnScore";
			this.cbS1PauseOnScore.Size = new System.Drawing.Size(146, 24);
			this.cbS1PauseOnScore.TabIndex = 1;
			this.cbS1PauseOnScore.Text = "Pause when triggered";
			this.cbS1PauseOnScore.UseVisualStyleBackColor = true;
            this.cbS1PauseOnScore.Checked = true;
			//
			// label33
			//
			this.label33.BackColor = System.Drawing.Color.Transparent;
			this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label33.Location = new System.Drawing.Point(61, 40);
			this.label33.Name = "label33";
			this.label33.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label33.Size = new System.Drawing.Size(105, 22);
			this.label33.TabIndex = 212;
			this.label33.Text = "next score trigger";
			this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// labelS1NextScoreTrigger
			//
			this.labelS1NextScoreTrigger.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS1NextScoreTrigger.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS1NextScoreTrigger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS1NextScoreTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS1NextScoreTrigger.Location = new System.Drawing.Point(17, 42);
			this.labelS1NextScoreTrigger.Name = "labelS1NextScoreTrigger";
			this.labelS1NextScoreTrigger.Size = new System.Drawing.Size(38, 22);
			this.labelS1NextScoreTrigger.TabIndex = 208;
			this.labelS1NextScoreTrigger.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// cbS1AutoIncrementScore
			//
			this.cbS1AutoIncrementScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1AutoIncrementScore.Location = new System.Drawing.Point(17, 107);
			this.cbS1AutoIncrementScore.Name = "cbS1AutoIncrementScore";
			this.cbS1AutoIncrementScore.Size = new System.Drawing.Size(155, 24);
			this.cbS1AutoIncrementScore.TabIndex = 3;
			this.cbS1AutoIncrementScore.Text = "Increment next score by 1";
			this.cbS1AutoIncrementScore.UseVisualStyleBackColor = true;
			//
			// cbS1ScoreAutoSave
			//
			this.cbS1ScoreAutoSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1ScoreAutoSave.Location = new System.Drawing.Point(17, 88);
			this.cbS1ScoreAutoSave.Name = "cbS1ScoreAutoSave";
			this.cbS1ScoreAutoSave.Size = new System.Drawing.Size(126, 24);
			this.cbS1ScoreAutoSave.TabIndex = 2;
			this.cbS1ScoreAutoSave.Text = "Auto save queue";
			this.cbS1ScoreAutoSave.UseVisualStyleBackColor = true;
			//
			// label28
			//
			this.label28.BackColor = System.Drawing.Color.Transparent;
			this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label28.Location = new System.Drawing.Point(61, 23);
			this.label28.Name = "label28";
			this.label28.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label28.Size = new System.Drawing.Size(85, 22);
			this.label28.TabIndex = 209;
			this.label28.Text = "score trigger";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// inputS1ScoreTrigger
			//
			this.inputS1ScoreTrigger.Location = new System.Drawing.Point(17, 19);
			this.inputS1ScoreTrigger.MaxLength = 3;
			this.inputS1ScoreTrigger.Name = "inputS1ScoreTrigger";
			this.inputS1ScoreTrigger.Size = new System.Drawing.Size(38, 20);
			this.inputS1ScoreTrigger.TabIndex = 0;
			//
			// inputS1SolvePath
			//
			this.inputS1SolvePath.Location = new System.Drawing.Point(386, 323);
			this.inputS1SolvePath.Multiline = true;
			this.inputS1SolvePath.Name = "inputS1SolvePath";
			this.inputS1SolvePath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.inputS1SolvePath.Size = new System.Drawing.Size(309, 202);
			this.inputS1SolvePath.TabIndex = 14;
			//
			// label43
			//
			this.label43.BackColor = System.Drawing.Color.Transparent;
			this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label43.Location = new System.Drawing.Point(10, 538);
			this.label43.Name = "label43";
			this.label43.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label43.Size = new System.Drawing.Size(72, 22);
			this.label43.TabIndex = 204;
			this.label43.Text = "Debug Level";
			this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// selDebugLevel
			//
			this.selDebugLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selDebugLevel.FormattingEnabled = true;
			this.selDebugLevel.Items.AddRange(new object[] {
									"Off",
									"Low",
									"Medium",
									"High"});
			this.selDebugLevel.Location = new System.Drawing.Point(88, 538);
			this.selDebugLevel.Name = "selDebugLevel";
			this.selDebugLevel.Size = new System.Drawing.Size(84, 21);
			this.selDebugLevel.TabIndex = 203;
			this.selDebugLevel.SelectedIndexChanged += new System.EventHandler(this.SelDebugLevelSelectedIndexChanged);
			//
			// grS1RandomTileQueue
			//
			this.grS1RandomTileQueue.Controls.Add(this.cbS1UseRandomQueues);
			this.grS1RandomTileQueue.Controls.Add(this.cbS1UseRandomTileset);
			this.grS1RandomTileQueue.Controls.Add(this.label40);
			this.grS1RandomTileQueue.Controls.Add(this.inputS1SeedStep);
			this.grS1RandomTileQueue.Controls.Add(this.cbS1RandomSeeds);
			this.grS1RandomTileQueue.Controls.Add(this.label37);
			this.grS1RandomTileQueue.Controls.Add(this.inputS1StartSeed);
			this.grS1RandomTileQueue.Enabled = false;
			this.grS1RandomTileQueue.Location = new System.Drawing.Point(717, 358);
			this.grS1RandomTileQueue.Name = "grS1RandomTileQueue";
			this.grS1RandomTileQueue.Size = new System.Drawing.Size(231, 103);
			this.grS1RandomTileQueue.TabIndex = 202;
			this.grS1RandomTileQueue.TabStop = false;
			this.grS1RandomTileQueue.Text = "randomisation options";
			//
			// cbS1UseRandomQueues
			//
			this.cbS1UseRandomQueues.Checked = true;
			this.cbS1UseRandomQueues.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbS1UseRandomQueues.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1UseRandomQueues.Location = new System.Drawing.Point(61, 19);
			this.cbS1UseRandomQueues.Name = "cbS1UseRandomQueues";
			this.cbS1UseRandomQueues.Size = new System.Drawing.Size(62, 22);
			this.cbS1UseRandomQueues.TabIndex = 207;
			this.cbS1UseRandomQueues.Text = "queues";
			this.cbS1UseRandomQueues.UseVisualStyleBackColor = true;
			//
			// cbS1UseRandomTileset
			//
			this.cbS1UseRandomTileset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1UseRandomTileset.Location = new System.Drawing.Point(6, 19);
			this.cbS1UseRandomTileset.Name = "cbS1UseRandomTileset";
			this.cbS1UseRandomTileset.Size = new System.Drawing.Size(63, 22);
			this.cbS1UseRandomTileset.TabIndex = 206;
			this.cbS1UseRandomTileset.Text = "tileset";
			this.cbS1UseRandomTileset.UseVisualStyleBackColor = true;
			//
			// label40
			//
			this.label40.BackColor = System.Drawing.Color.Transparent;
			this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label40.Location = new System.Drawing.Point(15, 71);
			this.label40.Name = "label40";
			this.label40.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label40.Size = new System.Drawing.Size(69, 22);
			this.label40.TabIndex = 205;
			this.label40.Text = "Seed Step";
			this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// inputS1SeedStep
			//
			this.inputS1SeedStep.Location = new System.Drawing.Point(90, 73);
			this.inputS1SeedStep.MaxLength = 16;
			this.inputS1SeedStep.Name = "inputS1SeedStep";
			this.inputS1SeedStep.Size = new System.Drawing.Size(116, 20);
			this.inputS1SeedStep.TabIndex = 2;
			this.inputS1SeedStep.Text = "1";
			//
			// cbS1RandomSeeds
			//
			this.cbS1RandomSeeds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1RandomSeeds.Location = new System.Drawing.Point(129, 19);
			this.cbS1RandomSeeds.Name = "cbS1RandomSeeds";
			this.cbS1RandomSeeds.Size = new System.Drawing.Size(96, 22);
			this.cbS1RandomSeeds.TabIndex = 1;
			this.cbS1RandomSeeds.Text = "random seeds";
			this.cbS1RandomSeeds.UseVisualStyleBackColor = true;
			this.cbS1RandomSeeds.CheckedChanged += new System.EventHandler(this.CbS1RandomSeedsCheckedChanged);
			//
			// label37
			//
			this.label37.BackColor = System.Drawing.Color.Transparent;
			this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label37.Location = new System.Drawing.Point(15, 45);
			this.label37.Name = "label37";
			this.label37.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label37.Size = new System.Drawing.Size(69, 22);
			this.label37.TabIndex = 202;
			this.label37.Text = "Start Seed";
			this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// inputS1StartSeed
			//
			this.inputS1StartSeed.Location = new System.Drawing.Point(90, 47);
			this.inputS1StartSeed.MaxLength = 16;
			this.inputS1StartSeed.Name = "inputS1StartSeed";
			this.inputS1StartSeed.Size = new System.Drawing.Size(116, 20);
			this.inputS1StartSeed.TabIndex = 0;
			this.inputS1StartSeed.Text = "0";
			//
			// grS1BacktrackOptions
			//
			this.grS1BacktrackOptions.Controls.Add(this.label17);
			this.grS1BacktrackOptions.Controls.Add(this.inputS1MaxEmptyCells);
			this.grS1BacktrackOptions.Controls.Add(this.cbUseAllTiles);
			this.grS1BacktrackOptions.Controls.Add(this.cbPauseEndCycle);
			this.grS1BacktrackOptions.Controls.Add(this.rbBacktrackQueueBackPedal);
			this.grS1BacktrackOptions.Controls.Add(this.rbBacktrackQueueJump);
			this.grS1BacktrackOptions.Controls.Add(this.cbS1BacktrackDepthLimit);
			this.grS1BacktrackOptions.Controls.Add(this.cbS1BacktrackStaleIterationTrigger);
			this.grS1BacktrackOptions.Controls.Add(this.inputS1BacktrackStaleIterations);
			this.grS1BacktrackOptions.Controls.Add(this.rbS1BacktrackPause);
			this.grS1BacktrackOptions.Controls.Add(this.cbS1BacktrackMinScoreTrigger);
			this.grS1BacktrackOptions.Controls.Add(this.cbS1BacktrackIterationTrigger);
			this.grS1BacktrackOptions.Controls.Add(this.inputS1BacktrackMinScore);
			this.grS1BacktrackOptions.Controls.Add(this.inputS1BacktrackMinIterations);
			this.grS1BacktrackOptions.Controls.Add(this.inputS1BacktrackDepthLimit);
			this.grS1BacktrackOptions.Controls.Add(this.rbS1BacktrackSkipCell);
			this.grS1BacktrackOptions.Enabled = false;
			this.grS1BacktrackOptions.Location = new System.Drawing.Point(717, 129);
			this.grS1BacktrackOptions.Name = "grS1BacktrackOptions";
			this.grS1BacktrackOptions.Size = new System.Drawing.Size(341, 207);
			this.grS1BacktrackOptions.TabIndex = 10;
			this.grS1BacktrackOptions.TabStop = false;
			this.grS1BacktrackOptions.Text = "backtrack trigger";
			//
			// label17
			//
			this.label17.Location = new System.Drawing.Point(219, 153);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(100, 19);
			this.label17.TabIndex = 246;
			this.label17.Text = "max empty cells";
			//
			// inputS1MaxEmptyCells
			//
			this.inputS1MaxEmptyCells.Location = new System.Drawing.Point(180, 150);
			this.inputS1MaxEmptyCells.MaxLength = 3;
			this.inputS1MaxEmptyCells.Name = "inputS1MaxEmptyCells";
			this.inputS1MaxEmptyCells.Size = new System.Drawing.Size(35, 20);
			this.inputS1MaxEmptyCells.TabIndex = 245;
			this.inputS1MaxEmptyCells.Text = "32";
			//
			// cbUseAllTiles
			//
			this.cbUseAllTiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbUseAllTiles.Location = new System.Drawing.Point(180, 123);
			this.cbUseAllTiles.Name = "cbUseAllTiles";
			this.cbUseAllTiles.Size = new System.Drawing.Size(143, 24);
			this.cbUseAllTiles.TabIndex = 244;
			this.cbUseAllTiles.Text = "use all tiles";
			this.cbUseAllTiles.UseVisualStyleBackColor = true;
			//
			// cbPauseEndCycle
			//
			this.cbPauseEndCycle.Checked = true;
			this.cbPauseEndCycle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbPauseEndCycle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbPauseEndCycle.Location = new System.Drawing.Point(180, 101);
			this.cbPauseEndCycle.Name = "cbPauseEndCycle";
			this.cbPauseEndCycle.Size = new System.Drawing.Size(146, 24);
			this.cbPauseEndCycle.TabIndex = 213;
			this.cbPauseEndCycle.Text = "pause at end of cycle";
			this.cbPauseEndCycle.UseVisualStyleBackColor = true;
			//
			// rbBacktrackQueueBackPedal
			//
			this.rbBacktrackQueueBackPedal.Location = new System.Drawing.Point(18, 150);
			this.rbBacktrackQueueBackPedal.Name = "rbBacktrackQueueBackPedal";
			this.rbBacktrackQueueBackPedal.Size = new System.Drawing.Size(174, 21);
			this.rbBacktrackQueueBackPedal.TabIndex = 229;
			this.rbBacktrackQueueBackPedal.Text = "queue back pedal";
			this.rbBacktrackQueueBackPedal.UseVisualStyleBackColor = true;
			//
			// rbBacktrackQueueJump
			//
			this.rbBacktrackQueueJump.Location = new System.Drawing.Point(18, 129);
			this.rbBacktrackQueueJump.Name = "rbBacktrackQueueJump";
			this.rbBacktrackQueueJump.Size = new System.Drawing.Size(174, 23);
			this.rbBacktrackQueueJump.TabIndex = 228;
			this.rbBacktrackQueueJump.Text = "queue jump start";
			this.rbBacktrackQueueJump.UseVisualStyleBackColor = true;
			//
			// cbS1BacktrackDepthLimit
			//
			this.cbS1BacktrackDepthLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1BacktrackDepthLimit.Location = new System.Drawing.Point(18, 177);
			this.cbS1BacktrackDepthLimit.Name = "cbS1BacktrackDepthLimit";
			this.cbS1BacktrackDepthLimit.Size = new System.Drawing.Size(143, 24);
			this.cbS1BacktrackDepthLimit.TabIndex = 227;
			this.cbS1BacktrackDepthLimit.Text = "abort if BT depth >";
			this.cbS1BacktrackDepthLimit.UseVisualStyleBackColor = true;
			//
			// cbS1BacktrackStaleIterationTrigger
			//
			this.cbS1BacktrackStaleIterationTrigger.AutoSize = true;
			this.cbS1BacktrackStaleIterationTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1BacktrackStaleIterationTrigger.Location = new System.Drawing.Point(18, 68);
			this.cbS1BacktrackStaleIterationTrigger.Name = "cbS1BacktrackStaleIterationTrigger";
			this.cbS1BacktrackStaleIterationTrigger.Size = new System.Drawing.Size(103, 17);
			this.cbS1BacktrackStaleIterationTrigger.TabIndex = 226;
			this.cbS1BacktrackStaleIterationTrigger.Text = "# stale iterations";
			this.cbS1BacktrackStaleIterationTrigger.UseVisualStyleBackColor = true;
			//
			// inputS1BacktrackStaleIterations
			//
			this.inputS1BacktrackStaleIterations.Location = new System.Drawing.Point(164, 66);
			this.inputS1BacktrackStaleIterations.MaxLength = 16;
			this.inputS1BacktrackStaleIterations.Name = "inputS1BacktrackStaleIterations";
			this.inputS1BacktrackStaleIterations.Size = new System.Drawing.Size(61, 20);
			this.inputS1BacktrackStaleIterations.TabIndex = 225;
			this.inputS1BacktrackStaleIterations.Text = "5000";
			//
			// rbS1BacktrackPause
			//
			this.rbS1BacktrackPause.Location = new System.Drawing.Point(18, 92);
			this.rbS1BacktrackPause.Name = "rbS1BacktrackPause";
			this.rbS1BacktrackPause.Size = new System.Drawing.Size(147, 18);
			this.rbS1BacktrackPause.TabIndex = 224;
			this.rbS1BacktrackPause.Text = "pause when triggered";
			this.rbS1BacktrackPause.UseVisualStyleBackColor = true;
			//
			// cbS1BacktrackMinScoreTrigger
			//
			this.cbS1BacktrackMinScoreTrigger.AutoSize = true;
			this.cbS1BacktrackMinScoreTrigger.Checked = true;
			this.cbS1BacktrackMinScoreTrigger.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbS1BacktrackMinScoreTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1BacktrackMinScoreTrigger.Location = new System.Drawing.Point(18, 45);
			this.cbS1BacktrackMinScoreTrigger.Name = "cbS1BacktrackMinScoreTrigger";
			this.cbS1BacktrackMinScoreTrigger.Size = new System.Drawing.Size(136, 17);
			this.cbS1BacktrackMinScoreTrigger.TabIndex = 223;
			this.cbS1BacktrackMinScoreTrigger.Text = "start after # tiles placed";
			this.cbS1BacktrackMinScoreTrigger.UseVisualStyleBackColor = true;
			//
			// cbS1BacktrackIterationTrigger
			//
			this.cbS1BacktrackIterationTrigger.AutoSize = true;
			this.cbS1BacktrackIterationTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1BacktrackIterationTrigger.Location = new System.Drawing.Point(18, 22);
			this.cbS1BacktrackIterationTrigger.Name = "cbS1BacktrackIterationTrigger";
			this.cbS1BacktrackIterationTrigger.Size = new System.Drawing.Size(125, 17);
			this.cbS1BacktrackIterationTrigger.TabIndex = 222;
			this.cbS1BacktrackIterationTrigger.Text = "start after # iterations";
			this.cbS1BacktrackIterationTrigger.UseVisualStyleBackColor = true;
			//
			// inputS1BacktrackMinScore
			//
			this.inputS1BacktrackMinScore.Location = new System.Drawing.Point(164, 43);
			this.inputS1BacktrackMinScore.MaxLength = 3;
			this.inputS1BacktrackMinScore.Name = "inputS1BacktrackMinScore";
			this.inputS1BacktrackMinScore.Size = new System.Drawing.Size(35, 20);
			this.inputS1BacktrackMinScore.TabIndex = 201;
			this.inputS1BacktrackMinScore.Text = "180";
			//
			// inputS1BacktrackMinIterations
			//
			this.inputS1BacktrackMinIterations.Location = new System.Drawing.Point(164, 20);
			this.inputS1BacktrackMinIterations.MaxLength = 16;
			this.inputS1BacktrackMinIterations.Name = "inputS1BacktrackMinIterations";
			this.inputS1BacktrackMinIterations.Size = new System.Drawing.Size(61, 20);
			this.inputS1BacktrackMinIterations.TabIndex = 199;
			this.inputS1BacktrackMinIterations.Text = "10000";
			//
			// inputS1BacktrackDepthLimit
			//
			this.inputS1BacktrackDepthLimit.Location = new System.Drawing.Point(164, 179);
			this.inputS1BacktrackDepthLimit.MaxLength = 3;
			this.inputS1BacktrackDepthLimit.Name = "inputS1BacktrackDepthLimit";
			this.inputS1BacktrackDepthLimit.Size = new System.Drawing.Size(35, 20);
			this.inputS1BacktrackDepthLimit.TabIndex = 0;
			this.inputS1BacktrackDepthLimit.Text = "100";
			//
			// rbS1BacktrackSkipCell
			//
			this.rbS1BacktrackSkipCell.Checked = true;
			this.rbS1BacktrackSkipCell.Location = new System.Drawing.Point(18, 109);
			this.rbS1BacktrackSkipCell.Name = "rbS1BacktrackSkipCell";
			this.rbS1BacktrackSkipCell.Size = new System.Drawing.Size(147, 24);
			this.rbS1BacktrackSkipCell.TabIndex = 1;
			this.rbS1BacktrackSkipCell.TabStop = true;
			this.rbS1BacktrackSkipCell.Text = "skip cell when triggered";
			this.rbS1BacktrackSkipCell.UseVisualStyleBackColor = true;
			//
			// cbS1EnforceFillableCells
			//
			this.cbS1EnforceFillableCells.AutoSize = true;
			this.cbS1EnforceFillableCells.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1EnforceFillableCells.Location = new System.Drawing.Point(717, 25);
			this.cbS1EnforceFillableCells.Name = "cbS1EnforceFillableCells";
			this.cbS1EnforceFillableCells.Size = new System.Drawing.Size(157, 17);
			this.cbS1EnforceFillableCells.TabIndex = 8;
			this.cbS1EnforceFillableCells.Text = "enforce fillable cells (slower)";
			this.cbS1EnforceFillableCells.UseVisualStyleBackColor = true;
			//
			// label35
			//
			this.label35.BackColor = System.Drawing.Color.Transparent;
			this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label35.Location = new System.Drawing.Point(424, 0);
			this.label35.Name = "label35";
			this.label35.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label35.Size = new System.Drawing.Size(59, 22);
			this.label35.TabIndex = 190;
			this.label35.Text = "Options";
			this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// btS1Reset
			//
			this.btS1Reset.Location = new System.Drawing.Point(250, 32);
			this.btS1Reset.Name = "btS1Reset";
			this.btS1Reset.Size = new System.Drawing.Size(59, 23);
			this.btS1Reset.TabIndex = 3;
			this.btS1Reset.Text = "Reset";
			this.btS1Reset.UseVisualStyleBackColor = true;
			this.btS1Reset.Click += new System.EventHandler(this.BtS1ResetClick);
			//
			// btS1ClearStats
			//
			this.btS1ClearStats.Location = new System.Drawing.Point(320, 264);
			this.btS1ClearStats.Name = "btS1ClearStats";
			this.btS1ClearStats.Size = new System.Drawing.Size(48, 23);
			this.btS1ClearStats.TabIndex = 185;
			this.btS1ClearStats.Text = "Clear";
			this.btS1ClearStats.UseVisualStyleBackColor = true;
			this.btS1ClearStats.Click += new System.EventHandler(this.BtClearSLog1Click);
			//
			// logS1Stats
			//
			this.logS1Stats.Location = new System.Drawing.Point(9, 288);
			this.logS1Stats.Multiline = true;
			this.logS1Stats.Name = "logS1Stats";
			this.logS1Stats.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logS1Stats.Size = new System.Drawing.Size(359, 237);
			this.logS1Stats.TabIndex = 183;
			//
			// cbS1UseRandomSeed
			//
			this.cbS1UseRandomSeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS1UseRandomSeed.Location = new System.Drawing.Point(717, 337);
			this.cbS1UseRandomSeed.Name = "cbS1UseRandomSeed";
			this.cbS1UseRandomSeed.Size = new System.Drawing.Size(192, 21);
			this.cbS1UseRandomSeed.TabIndex = 11;
			this.cbS1UseRandomSeed.Text = "randomise dataset";
			this.cbS1UseRandomSeed.UseVisualStyleBackColor = true;
			this.cbS1UseRandomSeed.CheckedChanged += new System.EventHandler(this.CbS1UseRandomSeedCheckedChanged);
			//
			// btS1Stats
			//
			this.btS1Stats.Location = new System.Drawing.Point(210, 264);
			this.btS1Stats.Name = "btS1Stats";
			this.btS1Stats.Size = new System.Drawing.Size(56, 23);
			this.btS1Stats.TabIndex = 168;
			this.btS1Stats.Text = "Dump";
			this.btS1Stats.UseVisualStyleBackColor = true;
			this.btS1Stats.Click += new System.EventHandler(this.BtS1StatsClick);
			//
			// labelS1TilesPlaced
			//
			this.labelS1TilesPlaced.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS1TilesPlaced.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS1TilesPlaced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS1TilesPlaced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS1TilesPlaced.Location = new System.Drawing.Point(80, 194);
			this.labelS1TilesPlaced.Name = "labelS1TilesPlaced";
			this.labelS1TilesPlaced.Size = new System.Drawing.Size(77, 22);
			this.labelS1TilesPlaced.TabIndex = 167;
			this.labelS1TilesPlaced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// labelS1QueueProgress
			//
			this.labelS1QueueProgress.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS1QueueProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS1QueueProgress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS1QueueProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS1QueueProgress.Location = new System.Drawing.Point(207, 146);
			this.labelS1QueueProgress.Name = "labelS1QueueProgress";
			this.labelS1QueueProgress.Size = new System.Drawing.Size(114, 22);
			this.labelS1QueueProgress.TabIndex = 166;
			this.labelS1QueueProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// labelS1NumIterations
			//
			this.labelS1NumIterations.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS1NumIterations.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS1NumIterations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS1NumIterations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS1NumIterations.Location = new System.Drawing.Point(97, 147);
			this.labelS1NumIterations.Name = "labelS1NumIterations";
			this.labelS1NumIterations.Size = new System.Drawing.Size(104, 22);
			this.labelS1NumIterations.TabIndex = 165;
			this.labelS1NumIterations.Text = "0";
			this.labelS1NumIterations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// btClearLogS1
			//
			this.btClearLogS1.Location = new System.Drawing.Point(639, 539);
			this.btClearLogS1.Name = "btClearLogS1";
			this.btClearLogS1.Size = new System.Drawing.Size(56, 23);
			this.btClearLogS1.TabIndex = 158;
			this.btClearLogS1.Text = "Clear";
			this.btClearLogS1.UseVisualStyleBackColor = true;
			this.btClearLogS1.Click += new System.EventHandler(this.BtClearLogS1Click);
			//
			// logS1
			//
			this.logS1.Location = new System.Drawing.Point(9, 565);
			this.logS1.Multiline = true;
			this.logS1.Name = "logS1";
			this.logS1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logS1.Size = new System.Drawing.Size(686, 169);
			this.logS1.TabIndex = 157;
			//
			// btSaveQS1
			//
			this.btSaveQS1.Location = new System.Drawing.Point(101, 91);
			this.btSaveQS1.Name = "btSaveQS1";
			this.btSaveQS1.Size = new System.Drawing.Size(85, 23);
			this.btSaveQS1.TabIndex = 6;
			this.btSaveQS1.Text = "Save Queue";
			this.btSaveQS1.UseVisualStyleBackColor = true;
			this.btSaveQS1.Click += new System.EventHandler(this.BtSaveQS1Click);
			//
			// selQS1
			//
			this.selQS1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selQS1.FormattingEnabled = true;
			this.selQS1.ImeMode = System.Windows.Forms.ImeMode.On;
			this.selQS1.Location = new System.Drawing.Point(9, 64);
			this.selQS1.Name = "selQS1";
			this.selQS1.Size = new System.Drawing.Size(363, 21);
			this.selQS1.Sorted = true;
			this.selQS1.TabIndex = 4;
			//
			// btLoadQS1
			//
			this.btLoadQS1.Location = new System.Drawing.Point(9, 91);
			this.btLoadQS1.Name = "btLoadQS1";
			this.btLoadQS1.Size = new System.Drawing.Size(85, 23);
			this.btLoadQS1.TabIndex = 5;
			this.btLoadQS1.Text = "Load Queue";
			this.btLoadQS1.UseVisualStyleBackColor = true;
			this.btLoadQS1.Click += new System.EventHandler(this.BtLoadQS1Click);
			//
			// btResumeS1
			//
			this.btResumeS1.Enabled = false;
			this.btResumeS1.Location = new System.Drawing.Point(178, 32);
			this.btResumeS1.Name = "btResumeS1";
			this.btResumeS1.Size = new System.Drawing.Size(62, 23);
			this.btResumeS1.TabIndex = 2;
			this.btResumeS1.Text = "Resume";
			this.btResumeS1.UseVisualStyleBackColor = true;
			this.btResumeS1.Click += new System.EventHandler(this.BtResumeS1Click);
			//
			// btPauseS1
			//
			this.btPauseS1.Enabled = false;
			this.btPauseS1.Location = new System.Drawing.Point(106, 32);
			this.btPauseS1.Name = "btPauseS1";
			this.btPauseS1.Size = new System.Drawing.Size(62, 23);
			this.btPauseS1.TabIndex = 1;
			this.btPauseS1.Text = "Pause";
			this.btPauseS1.UseVisualStyleBackColor = true;
			this.btPauseS1.Click += new System.EventHandler(this.BtPauseS1Click);
			//
			// label32
			//
			this.label32.BackColor = System.Drawing.Color.Transparent;
			this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label32.Location = new System.Drawing.Point(161, 168);
			this.label32.Name = "label32";
			this.label32.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label32.Size = new System.Drawing.Size(77, 22);
			this.label32.TabIndex = 146;
			this.label32.Text = "Max Placed";
			this.label32.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelS1MaxTilesPlaced
			//
			this.labelS1MaxTilesPlaced.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS1MaxTilesPlaced.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS1MaxTilesPlaced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS1MaxTilesPlaced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS1MaxTilesPlaced.Location = new System.Drawing.Point(161, 194);
			this.labelS1MaxTilesPlaced.Name = "labelS1MaxTilesPlaced";
			this.labelS1MaxTilesPlaced.Size = new System.Drawing.Size(77, 22);
			this.labelS1MaxTilesPlaced.TabIndex = 145;
			this.labelS1MaxTilesPlaced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label34
			//
			this.label34.BackColor = System.Drawing.Color.Transparent;
			this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label34.Location = new System.Drawing.Point(80, 168);
			this.label34.Name = "label34";
			this.label34.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label34.Size = new System.Drawing.Size(77, 22);
			this.label34.TabIndex = 144;
			this.label34.Text = "Tiles Placed";
			this.label34.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label38
			//
			this.label38.BackColor = System.Drawing.Color.Transparent;
			this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label38.Location = new System.Drawing.Point(11, 168);
			this.label38.Name = "label38";
			this.label38.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label38.Size = new System.Drawing.Size(66, 22);
			this.label38.TabIndex = 140;
			this.label38.Text = "Cell Path";
			this.label38.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelCellPath
			//
			this.labelCellPath.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelCellPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelCellPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelCellPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCellPath.Location = new System.Drawing.Point(11, 194);
			this.labelCellPath.Name = "labelCellPath";
			this.labelCellPath.Size = new System.Drawing.Size(66, 22);
			this.labelCellPath.TabIndex = 139;
			this.labelCellPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label41
			//
			this.label41.BackColor = System.Drawing.Color.Transparent;
			this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label41.Location = new System.Drawing.Point(206, 124);
			this.label41.Name = "label41";
			this.label41.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label41.Size = new System.Drawing.Size(115, 22);
			this.label41.TabIndex = 137;
			this.label41.Text = "Queue Progress";
			this.label41.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label42
			//
			this.label42.BackColor = System.Drawing.Color.Transparent;
			this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label42.Location = new System.Drawing.Point(239, 168);
			this.label42.Name = "label42";
			this.label42.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label42.Size = new System.Drawing.Size(79, 22);
			this.label42.TabIndex = 136;
			this.label42.Text = "Best Score";
			this.label42.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelS1BestScore
			//
			this.labelS1BestScore.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS1BestScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS1BestScore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS1BestScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS1BestScore.Location = new System.Drawing.Point(239, 194);
			this.labelS1BestScore.Name = "labelS1BestScore";
			this.labelS1BestScore.Size = new System.Drawing.Size(78, 22);
			this.labelS1BestScore.TabIndex = 133;
			this.labelS1BestScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label46
			//
			this.label46.BackColor = System.Drawing.Color.Transparent;
			this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label46.Location = new System.Drawing.Point(2, 5);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(153, 22);
			this.label46.TabIndex = 132;
			this.label46.Text = "E2 Puzzle Solver";
			this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label49
			//
			this.label49.BackColor = System.Drawing.Color.Transparent;
			this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label49.Location = new System.Drawing.Point(95, 124);
			this.label49.Name = "label49";
			this.label49.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label49.Size = new System.Drawing.Size(106, 22);
			this.label49.TabIndex = 129;
			this.label49.Text = "# Iterations";
			this.label49.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// selS1SolveMethod
			//
			this.selS1SolveMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selS1SolveMethod.FormattingEnabled = true;
			this.selS1SolveMethod.Location = new System.Drawing.Point(489, 234);
			this.selS1SolveMethod.Name = "selS1SolveMethod";
			this.selS1SolveMethod.Size = new System.Drawing.Size(206, 21);
			this.selS1SolveMethod.TabIndex = 13;
			this.selS1SolveMethod.SelectedIndexChanged += new System.EventHandler(this.SelS1SolvePathSelectedIndexChanged);
			//
			// btStartS1
			//
			this.btStartS1.Location = new System.Drawing.Point(10, 32);
			this.btStartS1.Name = "btStartS1";
			this.btStartS1.Size = new System.Drawing.Size(86, 23);
			this.btStartS1.TabIndex = 0;
			this.btStartS1.Text = "Start";
			this.btStartS1.UseVisualStyleBackColor = true;
			this.btStartS1.Click += new System.EventHandler(this.BtStartS1Click);
			//
			// tabStats
			//
			this.tabStats.BackColor = System.Drawing.Color.LightGray;
			this.tabStats.Controls.Add(this.selCommands);
			this.tabStats.Controls.Add(this.btRun);
			this.tabStats.Controls.Add(this.panel1);
			this.tabStats.Controls.Add(this.btCopySLog2);
			this.tabStats.Controls.Add(this.btCopySLog1);
			this.tabStats.Controls.Add(this.btClearSLog2);
			this.tabStats.Controls.Add(this.tbSLog2);
			this.tabStats.Controls.Add(this.btClearSLog1);
			this.tabStats.Controls.Add(this.tbSLog1);
			this.tabStats.Location = new System.Drawing.Point(4, 22);
			this.tabStats.Name = "tabStats";
			this.tabStats.Size = new System.Drawing.Size(1126, 823);
			this.tabStats.TabIndex = 4;
			this.tabStats.Text = "Stats";
			this.tabStats.UseVisualStyleBackColor = true;
			//
			// selCommands
			//
			this.selCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selCommands.FormattingEnabled = true;
			this.selCommands.ImeMode = System.Windows.Forms.ImeMode.On;
			this.selCommands.Items.AddRange(new object[] {
									"bgcolour_all_stats",
									"bgcolour_internal_stats",
									"compare_solution",
									"pattern_stats",
									"pattern_stats_individual",
									"shape_stats",
									"tile_analysis",
									"tile_bgcolour_count",
									"tile_bgcolour_distinct_count",
									"tile_pattern_count",
									"tile_pattern_distinct_count",
									"tile_shape_count",
									"tile_shape_distinct_count"});
			this.selCommands.Location = new System.Drawing.Point(300, 5);
			this.selCommands.Name = "selCommands";
			this.selCommands.Size = new System.Drawing.Size(322, 21);
			this.selCommands.TabIndex = 195;
			//
			// btRun
			//
			this.btRun.Location = new System.Drawing.Point(626, 5);
			this.btRun.Name = "btRun";
			this.btRun.Size = new System.Drawing.Size(59, 23);
			this.btRun.TabIndex = 194;
			this.btRun.Text = "Run";
			this.btRun.UseVisualStyleBackColor = true;
			this.btRun.Click += new System.EventHandler(this.BtRunClick);
			//
			// panel1
			//
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.panel1.Location = new System.Drawing.Point(8, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(275, 817);
			this.panel1.TabIndex = 193;
			//
			// btCopySLog2
			//
			this.btCopySLog2.Location = new System.Drawing.Point(968, 47);
			this.btCopySLog2.Name = "btCopySLog2";
			this.btCopySLog2.Size = new System.Drawing.Size(59, 23);
			this.btCopySLog2.TabIndex = 192;
			this.btCopySLog2.Text = "Copy";
			this.btCopySLog2.UseVisualStyleBackColor = true;
			this.btCopySLog2.Click += new System.EventHandler(this.BtCopy2Click);
			//
			// btCopySLog1
			//
			this.btCopySLog1.Location = new System.Drawing.Point(563, 47);
			this.btCopySLog1.Name = "btCopySLog1";
			this.btCopySLog1.Size = new System.Drawing.Size(59, 23);
			this.btCopySLog1.TabIndex = 191;
			this.btCopySLog1.Text = "Copy";
			this.btCopySLog1.UseVisualStyleBackColor = true;
			this.btCopySLog1.Click += new System.EventHandler(this.BtCopyClick);
			//
			// btClearSLog2
			//
			this.btClearSLog2.Location = new System.Drawing.Point(1034, 47);
			this.btClearSLog2.Name = "btClearSLog2";
			this.btClearSLog2.Size = new System.Drawing.Size(52, 23);
			this.btClearSLog2.TabIndex = 188;
			this.btClearSLog2.Text = "Clear";
			this.btClearSLog2.UseVisualStyleBackColor = true;
			this.btClearSLog2.Click += new System.EventHandler(this.BtS1ClearDumpClick);
			//
			// tbSLog2
			//
			this.tbSLog2.Location = new System.Drawing.Point(701, 76);
			this.tbSLog2.Multiline = true;
			this.tbSLog2.Name = "tbSLog2";
			this.tbSLog2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbSLog2.Size = new System.Drawing.Size(385, 744);
			this.tbSLog2.TabIndex = 187;
			//
			// btClearSLog1
			//
			this.btClearSLog1.Location = new System.Drawing.Point(628, 47);
			this.btClearSLog1.Name = "btClearSLog1";
			this.btClearSLog1.Size = new System.Drawing.Size(57, 23);
			this.btClearSLog1.TabIndex = 25;
			this.btClearSLog1.Text = "Clear";
			this.btClearSLog1.UseVisualStyleBackColor = true;
			this.btClearSLog1.Click += new System.EventHandler(this.btClearSLog1Click);
			//
			// tbSLog1
			//
			this.tbSLog1.Location = new System.Drawing.Point(300, 76);
			this.tbSLog1.Multiline = true;
			this.tbSLog1.Name = "tbSLog1";
			this.tbSLog1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbSLog1.Size = new System.Drawing.Size(385, 744);
			this.tbSLog1.TabIndex = 22;
			//
			// tabLog
			//
			this.tabLog.BackColor = System.Drawing.Color.LightGray;
			this.tabLog.Controls.Add(this.button1);
			this.tabLog.Controls.Add(this.cmdSQL);
			this.tabLog.Controls.Add(this.btClearLog);
			this.tabLog.Controls.Add(this.btSaveLog);
			this.tabLog.Controls.Add(this.logwindow);
			this.tabLog.Location = new System.Drawing.Point(4, 22);
			this.tabLog.Name = "tabLog";
			this.tabLog.Padding = new System.Windows.Forms.Padding(3);
			this.tabLog.Size = new System.Drawing.Size(1126, 823);
			this.tabLog.TabIndex = 1;
			this.tabLog.Text = "Log";
			this.tabLog.UseVisualStyleBackColor = true;
			//
			// button1
			//
			this.button1.Location = new System.Drawing.Point(498, 763);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(92, 23);
			this.button1.TabIndex = 20;
			this.button1.Text = "Execute SQL";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			//
			// cmdSQL
			//
			this.cmdSQL.Location = new System.Drawing.Point(106, 670);
			this.cmdSQL.Multiline = true;
			this.cmdSQL.Name = "cmdSQL";
			this.cmdSQL.Size = new System.Drawing.Size(386, 116);
			this.cmdSQL.TabIndex = 19;
			//
			// btClearLog
			//
			this.btClearLog.Location = new System.Drawing.Point(6, 35);
			this.btClearLog.Name = "btClearLog";
			this.btClearLog.Size = new System.Drawing.Size(92, 23);
			this.btClearLog.TabIndex = 18;
			this.btClearLog.Text = "Clear Log";
			this.btClearLog.UseVisualStyleBackColor = true;
			this.btClearLog.Click += new System.EventHandler(this.BtClearLogClick);
			//
			// btSaveLog
			//
			this.btSaveLog.Location = new System.Drawing.Point(6, 6);
			this.btSaveLog.Name = "btSaveLog";
			this.btSaveLog.Size = new System.Drawing.Size(92, 23);
			this.btSaveLog.TabIndex = 17;
			this.btSaveLog.Text = "Save Log";
			this.btSaveLog.UseVisualStyleBackColor = true;
			//
			// logwindow
			//
			this.logwindow.BackColor = System.Drawing.SystemColors.Control;
			this.logwindow.Location = new System.Drawing.Point(106, 3);
			this.logwindow.Multiline = true;
			this.logwindow.Name = "logwindow";
			this.logwindow.ReadOnly = true;
			this.logwindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logwindow.Size = new System.Drawing.Size(902, 650);
			this.logwindow.TabIndex = 13;
			//
			// tabSolver2
			//
			this.tabSolver2.Controls.Add(this.cbS2UseRandomSeed);
			this.tabSolver2.Controls.Add(this.labelNumIterations);
			this.tabSolver2.Controls.Add(this.label19);
			this.tabSolver2.Controls.Add(this.labelS2Seed);
			this.tabSolver2.Controls.Add(this.label50);
			this.tabSolver2.Controls.Add(this.grS2RandomTileQueue);
			this.tabSolver2.Controls.Add(this.labelTilesPlaced);
			this.tabSolver2.Controls.Add(this.labelQueueProgress);
			this.tabSolver2.Controls.Add(this.button8);
			this.tabSolver2.Controls.Add(this.btClearSolver2Log);
			this.tabSolver2.Controls.Add(this.textSolver2Log);
			this.tabSolver2.Controls.Add(this.label27);
			this.tabSolver2.Controls.Add(this.labelSolver2Method);
			this.tabSolver2.Controls.Add(this.labelS2MaxQueueSize);
			this.tabSolver2.Controls.Add(this.textSolver2MaxQueueSize);
			this.tabSolver2.Controls.Add(this.label25);
			this.tabSolver2.Controls.Add(this.btSaveQueue);
			this.tabSolver2.Controls.Add(this.selSolver2QueueList);
			this.tabSolver2.Controls.Add(this.btLoadQueue);
			this.tabSolver2.Controls.Add(this.btResume);
			this.tabSolver2.Controls.Add(this.btPause);
			this.tabSolver2.Controls.Add(this.label26);
			this.tabSolver2.Controls.Add(this.labelS2MostTilesPlaced);
			this.tabSolver2.Controls.Add(this.label24);
			this.tabSolver2.Controls.Add(this.label23);
			this.tabSolver2.Controls.Add(this.labelS2PathLength);
			this.tabSolver2.Controls.Add(this.label22);
			this.tabSolver2.Controls.Add(this.labelS2CellNum);
			this.tabSolver2.Controls.Add(this.label21);
			this.tabSolver2.Controls.Add(this.label20);
			this.tabSolver2.Controls.Add(this.label18);
			this.tabSolver2.Controls.Add(this.labelS2QueueSize);
			this.tabSolver2.Controls.Add(this.labelS2CellId);
			this.tabSolver2.Controls.Add(this.label12);
			this.tabSolver2.Controls.Add(this.btLoadDataset);
			this.tabSolver2.Controls.Add(this.selSolver2Method);
			this.tabSolver2.Controls.Add(this.btSolvePuzzle);
			this.tabSolver2.Controls.Add(this.btGenerateLists);
			this.tabSolver2.Location = new System.Drawing.Point(4, 22);
			this.tabSolver2.Name = "tabSolver2";
			this.tabSolver2.Size = new System.Drawing.Size(1126, 823);
			this.tabSolver2.TabIndex = 6;
			this.tabSolver2.Text = "Solver2";
			this.tabSolver2.UseVisualStyleBackColor = true;
			//
			// cbS2UseRandomSeed
			//
			this.cbS2UseRandomSeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS2UseRandomSeed.Location = new System.Drawing.Point(462, 14);
			this.cbS2UseRandomSeed.Name = "cbS2UseRandomSeed";
			this.cbS2UseRandomSeed.Size = new System.Drawing.Size(192, 21);
			this.cbS2UseRandomSeed.TabIndex = 221;
			this.cbS2UseRandomSeed.Text = "randomise tile queue";
			this.cbS2UseRandomSeed.UseVisualStyleBackColor = true;
			this.cbS2UseRandomSeed.CheckedChanged += new System.EventHandler(this.CbS2UseRandomSeedCheckedChanged);
			//
			// labelNumIterations
			//
			this.labelNumIterations.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelNumIterations.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelNumIterations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelNumIterations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelNumIterations.Location = new System.Drawing.Point(97, 266);
			this.labelNumIterations.Name = "labelNumIterations";
			this.labelNumIterations.Size = new System.Drawing.Size(106, 22);
			this.labelNumIterations.TabIndex = 220;
			this.labelNumIterations.Text = "0";
			this.labelNumIterations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label19
			//
			this.label19.BackColor = System.Drawing.Color.Transparent;
			this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label19.Location = new System.Drawing.Point(97, 243);
			this.label19.Name = "label19";
			this.label19.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label19.Size = new System.Drawing.Size(106, 22);
			this.label19.TabIndex = 219;
			this.label19.Text = "# Iterations";
			this.label19.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelS2Seed
			//
			this.labelS2Seed.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS2Seed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS2Seed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS2Seed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS2Seed.Location = new System.Drawing.Point(14, 266);
			this.labelS2Seed.Name = "labelS2Seed";
			this.labelS2Seed.Size = new System.Drawing.Size(81, 22);
			this.labelS2Seed.TabIndex = 218;
			this.labelS2Seed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label50
			//
			this.label50.BackColor = System.Drawing.Color.Transparent;
			this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label50.Location = new System.Drawing.Point(12, 243);
			this.label50.Name = "label50";
			this.label50.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label50.Size = new System.Drawing.Size(83, 22);
			this.label50.TabIndex = 217;
			this.label50.Text = "Seed";
			this.label50.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// grS2RandomTileQueue
			//
			this.grS2RandomTileQueue.Controls.Add(this.label45);
			this.grS2RandomTileQueue.Controls.Add(this.inputS2SeedStep);
			this.grS2RandomTileQueue.Controls.Add(this.cbS2RandomSeeds);
			this.grS2RandomTileQueue.Controls.Add(this.label47);
			this.grS2RandomTileQueue.Controls.Add(this.inputS2StartSeed);
			this.grS2RandomTileQueue.Enabled = false;
			this.grS2RandomTileQueue.Location = new System.Drawing.Point(462, 41);
			this.grS2RandomTileQueue.Name = "grS2RandomTileQueue";
			this.grS2RandomTileQueue.Size = new System.Drawing.Size(231, 101);
			this.grS2RandomTileQueue.TabIndex = 203;
			this.grS2RandomTileQueue.TabStop = false;
			this.grS2RandomTileQueue.Text = "randomisation options";
			//
			// label45
			//
			this.label45.BackColor = System.Drawing.Color.Transparent;
			this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label45.Location = new System.Drawing.Point(15, 43);
			this.label45.Name = "label45";
			this.label45.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label45.Size = new System.Drawing.Size(69, 22);
			this.label45.TabIndex = 205;
			this.label45.Text = "Seed Step";
			this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// inputS2SeedStep
			//
			this.inputS2SeedStep.Location = new System.Drawing.Point(90, 45);
			this.inputS2SeedStep.MaxLength = 16;
			this.inputS2SeedStep.Name = "inputS2SeedStep";
			this.inputS2SeedStep.Size = new System.Drawing.Size(116, 20);
			this.inputS2SeedStep.TabIndex = 2;
			this.inputS2SeedStep.Text = "1";
			//
			// cbS2RandomSeeds
			//
			this.cbS2RandomSeeds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbS2RandomSeeds.Location = new System.Drawing.Point(90, 71);
			this.cbS2RandomSeeds.Name = "cbS2RandomSeeds";
			this.cbS2RandomSeeds.Size = new System.Drawing.Size(125, 22);
			this.cbS2RandomSeeds.TabIndex = 1;
			this.cbS2RandomSeeds.Text = "random seeds";
			this.cbS2RandomSeeds.UseVisualStyleBackColor = true;
			//
			// label47
			//
			this.label47.BackColor = System.Drawing.Color.Transparent;
			this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label47.Location = new System.Drawing.Point(15, 17);
			this.label47.Name = "label47";
			this.label47.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label47.Size = new System.Drawing.Size(69, 22);
			this.label47.TabIndex = 202;
			this.label47.Text = "Start Seed";
			this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// inputS2StartSeed
			//
			this.inputS2StartSeed.Location = new System.Drawing.Point(90, 19);
			this.inputS2StartSeed.MaxLength = 16;
			this.inputS2StartSeed.Name = "inputS2StartSeed";
			this.inputS2StartSeed.Size = new System.Drawing.Size(116, 20);
			this.inputS2StartSeed.TabIndex = 0;
			this.inputS2StartSeed.Text = "0";
			//
			// labelTilesPlaced
			//
			this.labelTilesPlaced.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelTilesPlaced.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelTilesPlaced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelTilesPlaced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTilesPlaced.Location = new System.Drawing.Point(12, 372);
			this.labelTilesPlaced.Name = "labelTilesPlaced";
			this.labelTilesPlaced.Size = new System.Drawing.Size(77, 22);
			this.labelTilesPlaced.TabIndex = 146;
			this.labelTilesPlaced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// labelQueueProgress
			//
			this.labelQueueProgress.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelQueueProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelQueueProgress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelQueueProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelQueueProgress.Location = new System.Drawing.Point(166, 315);
			this.labelQueueProgress.Name = "labelQueueProgress";
			this.labelQueueProgress.Size = new System.Drawing.Size(77, 22);
			this.labelQueueProgress.TabIndex = 145;
			this.labelQueueProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// button8
			//
			this.button8.Location = new System.Drawing.Point(95, 485);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(116, 23);
			this.button8.TabIndex = 126;
			this.button8.Text = "Generate Statistics";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.Button8Click);
			//
			// btClearSolver2Log
			//
			this.btClearSolver2Log.Location = new System.Drawing.Point(9, 485);
			this.btClearSolver2Log.Name = "btClearSolver2Log";
			this.btClearSolver2Log.Size = new System.Drawing.Size(80, 23);
			this.btClearSolver2Log.TabIndex = 125;
			this.btClearSolver2Log.Text = "Clear Log";
			this.btClearSolver2Log.UseVisualStyleBackColor = true;
			this.btClearSolver2Log.Click += new System.EventHandler(this.BtClearSolver2LogClick);
			//
			// textSolver2Log
			//
			this.textSolver2Log.Location = new System.Drawing.Point(8, 514);
			this.textSolver2Log.Multiline = true;
			this.textSolver2Log.Name = "textSolver2Log";
			this.textSolver2Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textSolver2Log.Size = new System.Drawing.Size(995, 295);
			this.textSolver2Log.TabIndex = 124;
			//
			// label27
			//
			this.label27.BackColor = System.Drawing.Color.Transparent;
			this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label27.Location = new System.Drawing.Point(68, 201);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(52, 22);
			this.label27.TabIndex = 123;
			this.label27.Text = "Method";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// labelSolver2Method
			//
			this.labelSolver2Method.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelSolver2Method.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelSolver2Method.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelSolver2Method.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSolver2Method.Location = new System.Drawing.Point(126, 201);
			this.labelSolver2Method.Name = "labelSolver2Method";
			this.labelSolver2Method.Size = new System.Drawing.Size(160, 22);
			this.labelSolver2Method.TabIndex = 122;
			this.labelSolver2Method.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// labelS2MaxQueueSize
			//
			this.labelS2MaxQueueSize.BackColor = System.Drawing.Color.Transparent;
			this.labelS2MaxQueueSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS2MaxQueueSize.Location = new System.Drawing.Point(10, 147);
			this.labelS2MaxQueueSize.Name = "labelS2MaxQueueSize";
			this.labelS2MaxQueueSize.Size = new System.Drawing.Size(110, 22);
			this.labelS2MaxQueueSize.TabIndex = 121;
			this.labelS2MaxQueueSize.Text = "Max Queue Size";
			this.labelS2MaxQueueSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// textSolver2MaxQueueSize
			//
			this.textSolver2MaxQueueSize.Location = new System.Drawing.Point(126, 149);
			this.textSolver2MaxQueueSize.Name = "textSolver2MaxQueueSize";
			this.textSolver2MaxQueueSize.Size = new System.Drawing.Size(61, 20);
			this.textSolver2MaxQueueSize.TabIndex = 120;
			this.textSolver2MaxQueueSize.Text = "0";
			//
			// label25
			//
			this.label25.BackColor = System.Drawing.Color.Transparent;
			this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label25.Location = new System.Drawing.Point(30, 174);
			this.label25.Name = "label25";
			this.label25.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label25.Size = new System.Drawing.Size(90, 22);
			this.label25.TabIndex = 119;
			this.label25.Text = "Solve Method";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// btSaveQueue
			//
			this.btSaveQueue.Location = new System.Drawing.Point(94, 112);
			this.btSaveQueue.Name = "btSaveQueue";
			this.btSaveQueue.Size = new System.Drawing.Size(85, 23);
			this.btSaveQueue.TabIndex = 118;
			this.btSaveQueue.Text = "Save Queue";
			this.btSaveQueue.UseVisualStyleBackColor = true;
			this.btSaveQueue.Click += new System.EventHandler(this.BtSaveQueueClick);
			//
			// selSolver2QueueList
			//
			this.selSolver2QueueList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selSolver2QueueList.FormattingEnabled = true;
			this.selSolver2QueueList.ImeMode = System.Windows.Forms.ImeMode.On;
			this.selSolver2QueueList.Location = new System.Drawing.Point(8, 83);
			this.selSolver2QueueList.Name = "selSolver2QueueList";
			this.selSolver2QueueList.Size = new System.Drawing.Size(371, 21);
			this.selSolver2QueueList.Sorted = true;
			this.selSolver2QueueList.TabIndex = 117;
			//
			// btLoadQueue
			//
			this.btLoadQueue.Location = new System.Drawing.Point(7, 112);
			this.btLoadQueue.Name = "btLoadQueue";
			this.btLoadQueue.Size = new System.Drawing.Size(85, 23);
			this.btLoadQueue.TabIndex = 116;
			this.btLoadQueue.Text = "Load Queue";
			this.btLoadQueue.UseVisualStyleBackColor = true;
			this.btLoadQueue.Click += new System.EventHandler(this.BtLoadQueueClick);
			//
			// btResume
			//
			this.btResume.Enabled = false;
			this.btResume.Location = new System.Drawing.Point(170, 25);
			this.btResume.Name = "btResume";
			this.btResume.Size = new System.Drawing.Size(62, 23);
			this.btResume.TabIndex = 115;
			this.btResume.Text = "Resume";
			this.btResume.UseVisualStyleBackColor = true;
			this.btResume.Click += new System.EventHandler(this.BtResumeClick);
			//
			// btPause
			//
			this.btPause.Enabled = false;
			this.btPause.Location = new System.Drawing.Point(102, 25);
			this.btPause.Name = "btPause";
			this.btPause.Size = new System.Drawing.Size(62, 23);
			this.btPause.TabIndex = 114;
			this.btPause.Text = "Pause";
			this.btPause.UseVisualStyleBackColor = true;
			this.btPause.Click += new System.EventHandler(this.BtPauseClick);
			//
			// label26
			//
			this.label26.BackColor = System.Drawing.Color.Transparent;
			this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label26.Location = new System.Drawing.Point(95, 346);
			this.label26.Name = "label26";
			this.label26.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label26.Size = new System.Drawing.Size(77, 22);
			this.label26.TabIndex = 113;
			this.label26.Text = "Max Placed";
			this.label26.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelS2MostTilesPlaced
			//
			this.labelS2MostTilesPlaced.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS2MostTilesPlaced.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS2MostTilesPlaced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS2MostTilesPlaced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS2MostTilesPlaced.Location = new System.Drawing.Point(95, 372);
			this.labelS2MostTilesPlaced.Name = "labelS2MostTilesPlaced";
			this.labelS2MostTilesPlaced.Size = new System.Drawing.Size(77, 22);
			this.labelS2MostTilesPlaced.TabIndex = 112;
			this.labelS2MostTilesPlaced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label24
			//
			this.label24.BackColor = System.Drawing.Color.Transparent;
			this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label24.Location = new System.Drawing.Point(12, 346);
			this.label24.Name = "label24";
			this.label24.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label24.Size = new System.Drawing.Size(77, 22);
			this.label24.TabIndex = 111;
			this.label24.Text = "Tiles Placed";
			this.label24.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label23
			//
			this.label23.BackColor = System.Drawing.Color.Transparent;
			this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label23.Location = new System.Drawing.Point(62, 289);
			this.label23.Name = "label23";
			this.label23.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label23.Size = new System.Drawing.Size(54, 22);
			this.label23.TabIndex = 109;
			this.label23.Text = "Length";
			this.label23.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelS2PathLength
			//
			this.labelS2PathLength.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS2PathLength.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS2PathLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS2PathLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS2PathLength.Location = new System.Drawing.Point(62, 315);
			this.labelS2PathLength.Name = "labelS2PathLength";
			this.labelS2PathLength.Size = new System.Drawing.Size(54, 22);
			this.labelS2PathLength.TabIndex = 108;
			this.labelS2PathLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label22
			//
			this.label22.BackColor = System.Drawing.Color.Transparent;
			this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label22.Location = new System.Drawing.Point(13, 289);
			this.label22.Name = "label22";
			this.label22.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label22.Size = new System.Drawing.Size(45, 22);
			this.label22.TabIndex = 107;
			this.label22.Text = "Cell #";
			this.label22.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelS2CellNum
			//
			this.labelS2CellNum.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS2CellNum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS2CellNum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS2CellNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS2CellNum.Location = new System.Drawing.Point(13, 315);
			this.labelS2CellNum.Name = "labelS2CellNum";
			this.labelS2CellNum.Size = new System.Drawing.Size(44, 22);
			this.labelS2CellNum.TabIndex = 106;
			this.labelS2CellNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label21
			//
			this.label21.BackColor = System.Drawing.Color.Transparent;
			this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label21.Location = new System.Drawing.Point(249, 290);
			this.label21.Name = "label21";
			this.label21.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label21.Size = new System.Drawing.Size(77, 22);
			this.label21.TabIndex = 105;
			this.label21.Text = "Queue Size";
			this.label21.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label20
			//
			this.label20.BackColor = System.Drawing.Color.Transparent;
			this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label20.Location = new System.Drawing.Point(166, 289);
			this.label20.Name = "label20";
			this.label20.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label20.Size = new System.Drawing.Size(77, 22);
			this.label20.TabIndex = 104;
			this.label20.Text = "Queue #";
			this.label20.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// label18
			//
			this.label18.BackColor = System.Drawing.Color.Transparent;
			this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label18.Location = new System.Drawing.Point(119, 289);
			this.label18.Name = "label18";
			this.label18.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.label18.Size = new System.Drawing.Size(45, 22);
			this.label18.TabIndex = 103;
			this.label18.Text = "Cell ID";
			this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// labelS2QueueSize
			//
			this.labelS2QueueSize.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS2QueueSize.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS2QueueSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS2QueueSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS2QueueSize.Location = new System.Drawing.Point(249, 316);
			this.labelS2QueueSize.Name = "labelS2QueueSize";
			this.labelS2QueueSize.Size = new System.Drawing.Size(77, 22);
			this.labelS2QueueSize.TabIndex = 102;
			this.labelS2QueueSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// labelS2CellId
			//
			this.labelS2CellId.BackColor = System.Drawing.Color.WhiteSmoke;
			this.labelS2CellId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelS2CellId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelS2CellId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelS2CellId.Location = new System.Drawing.Point(119, 315);
			this.labelS2CellId.Name = "labelS2CellId";
			this.labelS2CellId.Size = new System.Drawing.Size(44, 22);
			this.labelS2CellId.TabIndex = 100;
			this.labelS2CellId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label12
			//
			this.label12.BackColor = System.Drawing.Color.Transparent;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(3, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(180, 22);
			this.label12.TabIndex = 99;
			this.label12.Text = "E2 2x2 Puzzle Solver";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// btLoadDataset
			//
			this.btLoadDataset.Location = new System.Drawing.Point(10, 54);
			this.btLoadDataset.Name = "btLoadDataset";
			this.btLoadDataset.Size = new System.Drawing.Size(86, 23);
			this.btLoadDataset.TabIndex = 98;
			this.btLoadDataset.Text = "Load Dataset";
			this.btLoadDataset.UseVisualStyleBackColor = true;
			this.btLoadDataset.Click += new System.EventHandler(this.BtLoadDatasetClick);
			//
			// selSolver2Method
			//
			this.selSolver2Method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selSolver2Method.FormattingEnabled = true;
			this.selSolver2Method.Location = new System.Drawing.Point(126, 175);
			this.selSolver2Method.Name = "selSolver2Method";
			this.selSolver2Method.Size = new System.Drawing.Size(161, 21);
			this.selSolver2Method.TabIndex = 91;
			//
			// btSolvePuzzle
			//
			this.btSolvePuzzle.Location = new System.Drawing.Point(10, 25);
			this.btSolvePuzzle.Name = "btSolvePuzzle";
			this.btSolvePuzzle.Size = new System.Drawing.Size(86, 23);
			this.btSolvePuzzle.TabIndex = 90;
			this.btSolvePuzzle.Text = "Start Solving";
			this.btSolvePuzzle.UseVisualStyleBackColor = true;
			this.btSolvePuzzle.Click += new System.EventHandler(this.BtSolvePuzzleClick);
			//
			// btGenerateLists
			//
			this.btGenerateLists.Location = new System.Drawing.Point(100, 54);
			this.btGenerateLists.Name = "btGenerateLists";
			this.btGenerateLists.Size = new System.Drawing.Size(86, 23);
			this.btGenerateLists.TabIndex = 89;
			this.btGenerateLists.Text = "Create Dataset";
			this.btGenerateLists.UseVisualStyleBackColor = true;
			this.btGenerateLists.Click += new System.EventHandler(this.BtGenerateListsClick);
			//
			// tabPage1
			//
			this.tabPage1.Controls.Add(this.btClearConfig);
			this.tabPage1.Controls.Add(this.btSetConfig);
			this.tabPage1.Controls.Add(this.label48);
			this.tabPage1.Controls.Add(this.btSaveConfig);
			this.tabPage1.Controls.Add(this.tbConfig);
			this.tabPage1.Controls.Add(this.btLoadConfig);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1126, 823);
			this.tabPage1.TabIndex = 8;
			this.tabPage1.Text = "Config";
			this.tabPage1.UseVisualStyleBackColor = true;
			//
			// btClearConfig
			//
			this.btClearConfig.Location = new System.Drawing.Point(432, 794);
			this.btClearConfig.Name = "btClearConfig";
			this.btClearConfig.Size = new System.Drawing.Size(85, 22);
			this.btClearConfig.TabIndex = 196;
			this.btClearConfig.Text = "Clear";
			this.btClearConfig.UseVisualStyleBackColor = true;
			this.btClearConfig.Click += new System.EventHandler(this.BtClearConfigClick);
			//
			// btSetConfig
			//
			this.btSetConfig.Location = new System.Drawing.Point(8, 794);
			this.btSetConfig.Name = "btSetConfig";
			this.btSetConfig.Size = new System.Drawing.Size(85, 23);
			this.btSetConfig.TabIndex = 134;
			this.btSetConfig.Text = "Set Config";
			this.btSetConfig.UseVisualStyleBackColor = true;
			this.btSetConfig.Click += new System.EventHandler(this.BtSetConfigClick);
			//
			// label48
			//
			this.label48.BackColor = System.Drawing.Color.Transparent;
			this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label48.Location = new System.Drawing.Point(7, 3);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(243, 22);
			this.label48.TabIndex = 133;
			this.label48.Text = "E2 Puzzle Solver Configuration";
			this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// btSaveConfig
			//
			this.btSaveConfig.Location = new System.Drawing.Point(190, 794);
			this.btSaveConfig.Name = "btSaveConfig";
			this.btSaveConfig.Size = new System.Drawing.Size(85, 23);
			this.btSaveConfig.TabIndex = 34;
			this.btSaveConfig.Text = "Save Config";
			this.btSaveConfig.UseVisualStyleBackColor = true;
			this.btSaveConfig.Click += new System.EventHandler(this.BtSaveConfigClick);
			//
			// tbConfig
			//
			this.tbConfig.Location = new System.Drawing.Point(8, 26);
			this.tbConfig.Multiline = true;
			this.tbConfig.Name = "tbConfig";
			this.tbConfig.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbConfig.Size = new System.Drawing.Size(509, 762);
			this.tbConfig.TabIndex = 33;
			//
			// btLoadConfig
			//
			this.btLoadConfig.Location = new System.Drawing.Point(99, 794);
			this.btLoadConfig.Name = "btLoadConfig";
			this.btLoadConfig.Size = new System.Drawing.Size(85, 23);
			this.btLoadConfig.TabIndex = 32;
			this.btLoadConfig.Text = "Load Config";
			this.btLoadConfig.UseVisualStyleBackColor = true;
			this.btLoadConfig.Click += new System.EventHandler(this.BtLoadConfigClick);
			//
			// statusBar
			//
			this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.statusInstructions,
									this.statusScoreLabel,
									this.statusScore,
									this.toolStripStatusLabel1,
									this.statusTilesUsedLabel,
									this.statusTilesUsed});
			this.statusBar.Location = new System.Drawing.Point(0, 853);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(1134, 22);
			this.statusBar.SizingGrip = false;
			this.statusBar.TabIndex = 19;
			this.statusBar.Text = "Status";
			//
			// statusInstructions
			//
			this.statusInstructions.AutoSize = false;
			this.statusInstructions.Name = "statusInstructions";
			this.statusInstructions.Size = new System.Drawing.Size(920, 17);
			this.statusInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// statusScoreLabel
			//
			this.statusScoreLabel.Name = "statusScoreLabel";
			this.statusScoreLabel.Size = new System.Drawing.Size(38, 17);
			this.statusScoreLabel.Text = "Score:";
			this.statusScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// statusScore
			//
			this.statusScore.AutoSize = false;
			this.statusScore.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.statusScore.Name = "statusScore";
			this.statusScore.Size = new System.Drawing.Size(57, 17);
			this.statusScore.Text = "000 / 000";
			this.statusScore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// toolStripStatusLabel1
			//
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			//
			// statusTilesUsedLabel
			//
			this.statusTilesUsedLabel.Name = "statusTilesUsedLabel";
			this.statusTilesUsedLabel.Size = new System.Drawing.Size(59, 17);
			this.statusTilesUsedLabel.Text = "Tiles Used:";
			this.statusTilesUsedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// statusTilesUsed
			//
			this.statusTilesUsed.AutoSize = false;
			this.statusTilesUsed.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.statusTilesUsed.Name = "statusTilesUsed";
			this.statusTilesUsed.Size = new System.Drawing.Size(57, 17);
			this.statusTilesUsed.Text = "000 / 000";
			this.statusTilesUsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// MainForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1134, 875);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.ImeMode = System.Windows.Forms.ImeMode.Alpha;
			this.IsMdiContainer = true;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ET2Solver";
			this.TransparencyKey = System.Drawing.Color.Transparent;
			this.tabControl1.ResumeLayout(false);
			this.tabLoadSave.ResumeLayout(false);
			this.tabLoadSave.PerformLayout();
			this.tabDesign.ResumeLayout(false);
			this.tabDesign.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pb_design)).EndInit();
			this.tabBoard.ResumeLayout(false);
			this.tabBoard.PerformLayout();
			this.grTileAction.ResumeLayout(false);
			this.grSearchType.ResumeLayout(false);
			this.tabControl2.ResumeLayout(false);
			this.tabResultsFree.ResumeLayout(false);
			this.tabResultsUsed.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pb_board)).EndInit();
			this.tabSolver1.ResumeLayout(false);
			this.tabSolver1.PerformLayout();
			this.grS1IterationControl.ResumeLayout(false);
			this.grS1IterationControl.PerformLayout();
			this.grS1AutoScoreTrigger.ResumeLayout(false);
			this.grS1AutoScoreTrigger.PerformLayout();
			this.grS1RandomTileQueue.ResumeLayout(false);
			this.grS1RandomTileQueue.PerformLayout();
			this.grS1BacktrackOptions.ResumeLayout(false);
			this.grS1BacktrackOptions.PerformLayout();
			this.tabStats.ResumeLayout(false);
			this.tabStats.PerformLayout();
			this.tabLog.ResumeLayout(false);
			this.tabLog.PerformLayout();
			this.tabSolver2.ResumeLayout(false);
			this.tabSolver2.PerformLayout();
			this.grS2RandomTileQueue.ResumeLayout(false);
			this.grS2RandomTileQueue.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btLoadDesign;
		private System.Windows.Forms.Button btSaveDesign;
		private System.Windows.Forms.Button btFindSwaps;
		private System.Windows.Forms.Button btClearConfig;
		private System.Windows.Forms.Button btSetConfig;
		private System.Windows.Forms.Button btLoadConfig;
		public System.Windows.Forms.TextBox tbConfig;
		private System.Windows.Forms.Button btSaveConfig;
		private System.Windows.Forms.Label label48;
		private System.Windows.Forms.TabPage tabPage1;
		public System.Windows.Forms.CheckBox cbPauseOnBorderTriplet;
		public System.Windows.Forms.CheckBox cbCountBorderTriplets;
		public System.Windows.Forms.TextBox inputS1NumBGColours;
		public System.Windows.Forms.CheckBox cbPauseNumBGColours;
		public System.Windows.Forms.CheckBox cbBGColourStats;
		private System.Windows.Forms.Button btClearTileset;
		private System.Windows.Forms.Button btCopyHints;
		public System.Windows.Forms.CheckBox cbSaveSolutions;
		private System.Windows.Forms.Button btClearCellFilter;
		private System.Windows.Forms.Button btCopyCellFilter;
		public System.Windows.Forms.ListBox lbDesignCmd;
		private System.Windows.Forms.Button btRunDesignCmd;
		private System.Windows.Forms.Button btFillLine;
		private System.Windows.Forms.Button btExportTileset;
		private System.Windows.Forms.Button btImportTileset;
		private System.Windows.Forms.Button btExportFilter;
		private System.Windows.Forms.Button btSaveBoardAsImage;
		private System.Windows.Forms.Button btImportDesign;
		private System.Windows.Forms.Button btExportDesign;
		private System.Windows.Forms.Button btNewDesign;
		public System.Windows.Forms.Panel patternPanel;
		public System.Windows.Forms.PictureBox pb_design;
		public System.Windows.Forms.TextBox tbDesignLog;
		private System.Windows.Forms.Button btClearDesignLog;
		private System.Windows.Forms.Button btCopyDesignLog;
		public System.Windows.Forms.ComboBox selDesignSize;
		public System.Windows.Forms.TabPage tabDesign;
		public System.Windows.Forms.TextBox inputS1MaxEmptyCells;
		private System.Windows.Forms.Label label17;
		public System.Windows.Forms.CheckBox cbPauseEndCycle;
		public System.Windows.Forms.CheckBox cbUseAllTiles;
		private System.Windows.Forms.Button btCompareToSolution;
		private System.Windows.Forms.Button btRun;
		public System.Windows.Forms.ComboBox selCommands;
		private System.Windows.Forms.Button btDumpBoardAsTileset;
		private System.Windows.Forms.Button btClearBoardLog;
		private System.Windows.Forms.Button btCopyBoardLog;
		public System.Windows.Forms.TextBox tbBoardLog;
		private System.Windows.Forms.Button btRemoveTiles;
		private System.Windows.Forms.Button btSelectTiles;
		public System.Windows.Forms.ToolStripStatusLabel statusInstructions;
		private System.Windows.Forms.Button btSelectPath;
		private System.Windows.Forms.Button btBuildFilter;
		private System.Windows.Forms.Button btCopyModel;
		private System.Windows.Forms.Button btCopyTileset;
		public System.Windows.Forms.TextBox tbSLog1;
		private System.Windows.Forms.Button btClearSLog1;
		private System.Windows.Forms.Button btClearSLog2;
		public System.Windows.Forms.TextBox tbSLog2;
		private System.Windows.Forms.Button btCopySLog2;
		private System.Windows.Forms.Button btCopySLog1;
		public System.Windows.Forms.CheckBox cbShowUniqueBoards;
		public System.Windows.Forms.CheckBox cbShowMagicBorders;
		public System.Windows.Forms.CheckBox cbVerboseCreateBoardLog;
		public System.Windows.Forms.CheckBox cbShowNonUniqueBoards;
		public System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btStepBack;
		private System.Windows.Forms.Button btStepForward;
		private System.Windows.Forms.Button btCopyCmd;
		public System.Windows.Forms.TextBox inputCmd;
		private System.Windows.Forms.Button btRunCmd;
		private System.Windows.Forms.Button btClearCmd;
		private System.Windows.Forms.Button btClearSolvePath;
		private System.Windows.Forms.Button btCopySolvePath;
		private System.Windows.Forms.Button btCopyDump;
		public System.Windows.Forms.ComboBox selDumpOptions;
		public System.Windows.Forms.CheckBox cbTrackTileDistribution;
		private System.Windows.Forms.Button btCalcHash;
		private System.Windows.Forms.Button btShuffle;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Button btSetLayout;
		private System.Windows.Forms.Label label9;
		public System.Windows.Forms.ComboBox selBoardLayout;
		public System.Windows.Forms.Button btStop;
		public System.Windows.Forms.RadioButton rbBacktrackQueueBackPedal;
		public System.Windows.Forms.RadioButton rbBacktrackQueueJump;
		public System.Windows.Forms.CheckBox cbS1UseRegionRestrictions;
		private System.Windows.Forms.Label label14;
		public System.Windows.Forms.Label labelUniquePatternOrientations;
		public System.Windows.Forms.CheckBox cbS1BacktrackMinScoreTrigger;
		public System.Windows.Forms.CheckBox cbS1BacktrackIterationTrigger;
		public System.Windows.Forms.CheckBox cbS1BacktrackStaleIterationTrigger;
		public System.Windows.Forms.TextBox inputS1IterationScoreProximity;
		public System.Windows.Forms.CheckBox cbS1IterationLimitScoreProximity;
		public System.Windows.Forms.CheckBox cbS1BacktrackDepthLimit;
		public System.Windows.Forms.TextBox inputS1BacktrackStaleIterations;
		public System.Windows.Forms.Label labelSpeed;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button btClearScores;
		private System.Windows.Forms.Button btClearSolutions;
		private System.Windows.Forms.Label labels11;
		public System.Windows.Forms.Label labelSolutions;
		public System.Windows.Forms.CheckBox cbCellFilter;
		public System.Windows.Forms.TextBox inputCellFilter;
		public System.Windows.Forms.CheckBox cbSearchUniques;
		public System.Windows.Forms.RadioButton rbActionOff;
		public System.Windows.Forms.RadioButton rbMoveTiles;
		public System.Windows.Forms.RadioButton rbDumpTiles;
		public System.Windows.Forms.RadioButton rbImportTiles;
		public System.Windows.Forms.RadioButton rbBuildPath;
		public System.Windows.Forms.ListView searchUsedResultsImages;
		public System.Windows.Forms.ListView searchFreeResultsImages;
		private System.Windows.Forms.GroupBox grTileAction;
		private System.Windows.Forms.Button btDumpBoard;
		public System.Windows.Forms.RadioButton rbSearchMatchAny;
		public System.Windows.Forms.Label labelNumSearchResultsFree;
		public System.Windows.Forms.Label labelNumSearchResultsUsed;
		public System.Windows.Forms.RadioButton rbSearchMatch4;
		public System.Windows.Forms.RadioButton rbSearchRegex;
		public System.Windows.Forms.RadioButton rbSearchMatch2;
		public System.Windows.Forms.RadioButton rbSearchMatch3;
		public System.Windows.Forms.RadioButton rbSearchMatchAll;
		private System.Windows.Forms.GroupBox grSearchType;
		private System.Windows.Forms.Button btClearSearchPattenr;
		public System.Windows.Forms.ComboBox selS1PathFilter;
		private System.Windows.Forms.Label label15;
		public System.Windows.Forms.CheckBox cbS1UseRandomTileset;
		public System.Windows.Forms.CheckBox cbS1UseRandomQueues;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button btSetModelAsHint;
		public System.Windows.Forms.Label labelS2Seed;
		public System.Windows.Forms.GroupBox grS2RandomTileQueue;
		public System.Windows.Forms.CheckBox cbS2UseRandomSeed;
		public System.Windows.Forms.TextBox inputS2StartSeed;
		public System.Windows.Forms.CheckBox cbS2RandomSeeds;
		public System.Windows.Forms.TextBox inputS2SeedStep;
		public System.Windows.Forms.Label labelNumIterations;
		private System.Windows.Forms.Label label50;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.Label label45;
		public System.Windows.Forms.RadioButton rbS1BacktrackPause;
		public System.Windows.Forms.TextBox inputS1BacktrackMinScore;
		public System.Windows.Forms.TextBox inputS1BacktrackMinIterations;
		private System.Windows.Forms.Label label31;
		public System.Windows.Forms.CheckBox cbS1AutoIncrementScore;
		private System.Windows.Forms.Label label44;
		public System.Windows.Forms.Label labelS1Seed;
		public System.Windows.Forms.TextBox inputS1CurrentSolveMethod;
		public System.Windows.Forms.ComboBox selS1SolveMethod;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		public System.Windows.Forms.GroupBox grS1BacktrackOptions;
		public System.Windows.Forms.GroupBox grS1AutoScoreTrigger;
		public System.Windows.Forms.GroupBox grS1IterationControl;
		public System.Windows.Forms.CheckBox cbS1PauseOnScore;
		public System.Windows.Forms.CheckBox cbS1EnableScoreTrigger;
		private System.Windows.Forms.Label labelS2MaxQueueSize;
		public System.Windows.Forms.CheckBox cbS1EnableBacktrackLimit;
		public System.Windows.Forms.CheckBox cbS1IterationLimit;
		public System.Windows.Forms.CheckBox cbS1AutoSaveByInterval;
		public System.Windows.Forms.Label labelS1NextScoreTrigger;
		public System.Windows.Forms.Label labelS1BestScore;
		private System.Windows.Forms.Label label33;
		public System.Windows.Forms.CheckBox cbS1UseRandomSeed;
		public System.Windows.Forms.TextBox inputS1StartSeed;
		public System.Windows.Forms.CheckBox cbS1EnforceFillableCells;
		public System.Windows.Forms.TextBox inputS1BacktrackDepthLimit;
		public System.Windows.Forms.RadioButton rbS1BacktrackSkipCell;
		public System.Windows.Forms.CheckBox cbS1RandomSeeds;
		public System.Windows.Forms.TextBox inputS1RunLength;
		public System.Windows.Forms.TextBox inputS1SeedStep;
		public System.Windows.Forms.TextBox inputS1MaxIterations;
		public System.Windows.Forms.TextBox inputS1SolvePath;
		public System.Windows.Forms.CheckBox cbS1ScoreAutoSave;
		public System.Windows.Forms.TextBox inputS1ScoreTrigger;
		private System.Windows.Forms.Label label28;
		public System.Windows.Forms.ComboBox selDebugLevel;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.Label label40;
		public System.Windows.Forms.GroupBox grS1RandomTileQueue;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label label35;
		public System.Windows.Forms.Button btS1Reset;
		public System.Windows.Forms.TextBox logS1Stats;
		private System.Windows.Forms.Button btS1ClearStats;
		private System.Windows.Forms.Button btSetBoardAsHint;
		private System.Windows.Forms.Button btClearModel;
		private System.Windows.Forms.Button btClearHints;
		public System.Windows.Forms.TextBox inputS1AutoSaveIterations;
		private System.Windows.Forms.Button btS1Stats;
		public System.Windows.Forms.Label labelS1TilesPlaced;
		private System.Windows.Forms.Button btClearLogS1;
		public System.Windows.Forms.TextBox logS1;
		private System.Windows.Forms.Button btSaveQS1;
		public System.Windows.Forms.ComboBox selQS1;
		private System.Windows.Forms.Button btLoadQS1;
		public System.Windows.Forms.Button btResumeS1;
		public System.Windows.Forms.Button btPauseS1;
		public System.Windows.Forms.Button btStartS1;
		public System.Windows.Forms.Label labelS1MaxTilesPlaced;
		public System.Windows.Forms.Label labelS1QueueProgress;
		public System.Windows.Forms.Label labelS1NumIterations;
		private System.Windows.Forms.Button button8;
		public System.Windows.Forms.Label labelTilesPlaced;
		public System.Windows.Forms.Label labelCellPath;
		public System.Windows.Forms.Label labelQueueProgress;
		private System.Windows.Forms.Label label49;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label32;
		public System.Windows.Forms.TextBox textSolver2MaxQueueSize;
		public System.Windows.Forms.Label labelSolver2Method;
		public System.Windows.Forms.Label labelS2QueueSize;
		public System.Windows.Forms.Label labelS2CellId;
		public System.Windows.Forms.Label labelS2CellNum;
		public System.Windows.Forms.Label labelS2PathLength;
		public System.Windows.Forms.Label labelS2MostTilesPlaced;
		public System.Windows.Forms.ComboBox selSolver2Method;
		public System.Windows.Forms.ComboBox selSolver2QueueList;
		public System.Windows.Forms.TextBox textSolver2Log;
		private System.Windows.Forms.Button btClearSolver2Log;
		public System.Windows.Forms.TabPage tabSolver2;
		public System.Windows.Forms.TabPage tabSolver1;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.TextBox cmdSQL;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Button btLoadQueue;
		private System.Windows.Forms.Button btSaveQueue;
		public System.Windows.Forms.Button btPause;
		public System.Windows.Forms.Button btResume;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Button btLoadDataset;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label19;
		public System.Windows.Forms.Button btSolvePuzzle;
		private System.Windows.Forms.Button btGenerateLists;
		private System.Windows.Forms.Button btTilesetR1;
		private System.Windows.Forms.Button btTilesetR2;
		private System.Windows.Forms.Button btTilesetR3;
		private System.Windows.Forms.Button btTilesetR4;
		private System.Windows.Forms.Button btCreateUniqueBoards;
		public System.Windows.Forms.TextBox inputSeedStart;
		private System.Windows.Forms.Label labelSeed;
		public System.Windows.Forms.TextBox inputNumBoards;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox textLoadSaveLog;
		private System.Windows.Forms.Button btClearLoadSaveLog;
		public System.Windows.Forms.TextBox inputSeed;
		private System.Windows.Forms.Button btNewTileset;
		public System.Windows.Forms.TabPage tabStats;
		private System.Windows.Forms.Button btNew;
		private System.Windows.Forms.Button btSetModel;
		private System.Windows.Forms.Button btSaveHints;
		private System.Windows.Forms.Button btLoadHints;
		private System.Windows.Forms.Button btGetModel;
		private System.Windows.Forms.Button btSetTileset;
		private System.Windows.Forms.Button btLoadModel;
		private System.Windows.Forms.Button btLoadTileset;
		private System.Windows.Forms.Label label7;
		public System.Windows.Forms.Label dspNumFillableTiles;
		public System.Windows.Forms.Label dspNumSwappableTiles;
		public System.Windows.Forms.Label dspNumScarceTiles;
		private System.Windows.Forms.Label label16;
		public System.Windows.Forms.Label dspNumFillableCells;
		public System.Windows.Forms.Label dspNumSwappableCells;
		public System.Windows.Forms.Label dspNumScarceCells;
		public System.Windows.Forms.Label dspNumInvalidCells;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btClearOverlays;
		private System.Windows.Forms.Button btPlaceTile;
		private System.Windows.Forms.Button btCountIntersectingTiles;
		public System.Windows.Forms.TabPage tabResultsUsed;
		public System.Windows.Forms.TabPage tabResultsFree;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TextBox selMatchPattern;
		private System.Windows.Forms.Label selTilePattern;
		private System.Windows.Forms.Label selTile;
		private System.Windows.Forms.Label selColRow;
		private System.Windows.Forms.Label labelModel;
		private System.Windows.Forms.Label labelTileset;
		public System.Windows.Forms.TextBox logwindow;
		private System.Windows.Forms.Button btSaveLog;
		private System.Windows.Forms.Button btClearLog;
		private System.Windows.Forms.TabPage tabLog;
		public System.Windows.Forms.ComboBox selTilesets;
		public System.Windows.Forms.TextBox textModel;
		public System.Windows.Forms.TextBox textTileset;
		private System.Windows.Forms.Button btSaveTileset;
		private System.Windows.Forms.Button btSaveModel;
		public System.Windows.Forms.TextBox textSaveTilesetName;
		public System.Windows.Forms.ComboBox selHints;
		public System.Windows.Forms.TextBox textHints;
		public System.Windows.Forms.TextBox textSaveHintsName;
		public System.Windows.Forms.TabPage tabLoadSave;
		public System.Windows.Forms.PictureBox pb_board;
		private System.Windows.Forms.Button btSearch;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		public System.Windows.Forms.TabPage tabBoard;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel statusTilesUsed;
		private System.Windows.Forms.ToolStripStatusLabel statusTilesUsedLabel;
		private System.Windows.Forms.ToolStripStatusLabel statusScoreLabel;
		private System.Windows.Forms.ToolStripStatusLabel statusScore;
		private System.Windows.Forms.StatusStrip statusBar;

		public void btLoadTilesetClick(object sender, System.EventArgs e)
		{
			this.timer.start("loadTileset");

			this.board.loadTileSet(this.selTilesets.SelectedItem.ToString());
			if ( this.solver == null )
			{
				this.initSolver(false);
			}
			else
			{
				this.solver.loadTileset();
			}

			this.timer.stop("loadTileset");
			this.log(this.timer.results("loadTileset"));
		}

		public void btLoadModelClick(object sender, System.EventArgs e)
		{
			if ( this.board != null )
			{
				this.timer.start("loadModel");

				// use file open dialog
				System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
				openFileDialog1.FileName = "";
				openFileDialog1.Filter = "All files (*.*)|*.*";
				openFileDialog1.FilterIndex = 1;
				openFileDialog1.RestoreDirectory = true;
				openFileDialog1.InitialDirectory = "models";
				openFileDialog1.Title = "Load Model";
//				openFileDialog1.ShowDialog();

				bool isFileSelected = false;
				if ( openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && openFileDialog1.FileName != "" )
				{
					isFileSelected = true;
				}
				if ( !isFileSelected )
				{
					System.Windows.Forms.MessageBox.Show("No model selected");
					return;
				}
				this.board.loadModel(openFileDialog1.FileName);
				Program.TheMainForm.useTileGraphics = true;
				this.board.drawTiles();

//				System.Windows.Forms.MessageBox.Show(openFileDialog1.FileName);

				/*
				if ( this.selModels.SelectedItem == null )
				{
					System.Windows.Forms.MessageBox.Show("No model selected");
					return;
				}
				this.board.loadModel(this.selModels.SelectedItem.ToString());
				this.textSaveModelName.Text = this.selModels.SelectedItem.ToString();
				*/

				this.timer.stop("loadModel");
				this.log(this.timer.results("loadModel"));
			}
			else
			{
				System.Windows.Forms.MessageBox.Show("Load a tileset first", "No tileset loaded");
			}
		}

		public void BtSearchClick(object sender, System.EventArgs e)
		{
			this.board.searchTiles(this.selMatchPattern.Text);
		}

		public void TabLoadSaveEnter(object sender, System.EventArgs e)
		{
			this.loadTileSetList();
//			this.loadModelList();
			if ( Board.title == "" )
			{
				if ( this.selTilesets.Items.Contains(this.default_board) )
				{
					this.board.loadTileSet(this.default_board);
				}
			}
			this.loadHintList();
		}

		public void BtClearLogClick(object sender, System.EventArgs e)
		{
			this.logwindow.Text = "";
			this.logwindow.Refresh();
		}

		public int[] getColRow()
		{
			int[] rv = new int[2];
			rv[0] = this.col;
			rv[1] = this.row;
			return rv;
		}

		public void handleBoardClick(object sender, System.EventArgs e)
		{
			System.Windows.Forms.MouseEventArgs me = (System.Windows.Forms.MouseEventArgs)e;
			//this.log("mouse click XY @ " + me.X + "," + me.Y);
			int[] col_row = this.board.getColRowFromXY(me.X, me.Y);
	        this.col = col_row[0];
	        this.row = col_row[1];
	        int pos = (this.row - 1) * Board.num_cols + this.col;
	        string boardref = Char.ConvertFromUtf32(this.row+64) + this.col;
	        this.selColRow.Text = this.col + "," + this.row + " : " + pos + " : " + boardref;
	        this.selColRow.Update();
			//this.log("mouse click Col/Row @ " + col + "," + row);

			switch ( me.Button.ToString() )
			{
				case "Left":
					// select_path
					if ( this.action == "select_path" )
					{
						switch ( this.action_step )
						{
							case 1:
								// mark top left cell
								this.select_start_col = this.col;
								this.select_start_row = this.row;
								this.statusInstructions.Text = "Select Path. 2 / 2. Select BOTTOM RIGHT cell.";
								this.action_step++;
								break;
							case 2:
								// mark bottom right cell
								this.select_end_col = this.col;
								this.select_end_row = this.row;
								this.statusInstructions.Text = String.Format("Path Selected: {0},{1} to {2},{3}", this.select_start_col, this.select_start_row, this.select_end_col, this.select_end_row);
								if ( this.solver == null )
								{
									this.initSolver(false);
								}
								this.board.clearOverlays();
								this.solver.pb.clear();
								int numCols = this.select_end_col - this.select_start_col + 1;
								for ( int r = this.select_start_row; r <= this.select_end_row; r++ )
								{
									for ( int c = this.select_start_col; c <= this.select_end_col; c++ )
									{
										this.board.createCellOverlay(c, r, c + "," + r, System.Drawing.Color.LightBlue);
									}
									this.solver.pb.addRowRange("right", this.select_start_col, r, numCols);
								}
								this.selS1SolveMethod.Text = "Custom";
								this.inputS1SolvePath.Text = String.Join(",", this.solver.pb.path.ConvertAll<string>(delegate(int i){ return i.ToString(); }).ToArray());
								this.action_step = 0;
								this.action = "";
								break;
						}
						break;
					}

					// import tiles from tileview window and insert into column down/right from col,row
					if ( this.rbImportTiles.Checked )
					{
						int icol = this.col;
						int irow = this.row;
						List<string> items = new List<string>(this.ts.textTileDump.Text.Split('\n'));

						// import direction
						string import_direction = "h";
						if ( CAF_Application.config.contains("import_direction") )
						{
							import_direction = CAF_Application.config.getValue("import_direction");
						}

						foreach (string line in items)
						{
							// import single tile patterns into columns
							if ( !line.StartsWith("//") && line.Trim().Length == 4 )
							{
								// place on board, skipping existing tiles
								int tile_id = this.board.getTileIdFromColRow(icol, irow);
								if ( tile_id == 0 )
								{
									Tile tile = this.board.getTileByPattern(line.Trim());
									if ( tile != null && !this.board.isTileUsed(tile.id) )
									{
										this.board.drawTile(icol, irow, tile);
									}
								}
								if ( import_direction == "v" )
								{
									if ( irow < Board.num_rows )
									{
										irow++;
									}
									else if ( icol < Board.num_cols )
									{
										icol++;
										irow = 1;
									}
									else
									{
										System.Windows.Forms.MessageBox.Show("Not enough room to import tiles!");
									}
								}
								else if ( import_direction == "h" )
								{
									if ( icol < Board.num_cols )
									{
										icol++;
									}
									else if ( irow < Board.num_rows )
									{
										irow++;
										icol = 1;
									}
									else
									{
										System.Windows.Forms.MessageBox.Show("Not enough room to import tiles!");
									}
								}
							}
							// import 2x2 tile patterns into columns
							else if ( !line.StartsWith("//") && line.Trim().Length == 19 && icol < Board.num_cols && irow < Board.num_rows )
							{
								List<string> tilelist = new List<string>(line.Trim().Split(','));

								string tileA = tilelist[0];
								// place on board, overwrite existing tiles
								Tile tile = this.board.getTileByPattern(tileA);
								if ( tile != null && !this.board.isTileUsed(tile.id) )
								{
									this.board.drawTile(icol, irow, tile);
								}

								string tileB = tilelist[1];
								// place on board, overwrite existing tiles
								tile = this.board.getTileByPattern(tileB);
								if ( tile != null && !this.board.isTileUsed(tile.id) )
								{
									this.board.drawTile(icol+1, irow, tile);
								}

								string tileC = tilelist[2];
								// place on board, overwrite existing tiles
								tile = this.board.getTileByPattern(tileC);
								if ( tile != null && !this.board.isTileUsed(tile.id) )
								{
									this.board.drawTile(icol, irow+1, tile);
								}

								string tileD = tilelist[3];
								// place on board, overwrite existing tiles
								tile = this.board.getTileByPattern(tileD);
								if ( tile != null && !this.board.isTileUsed(tile.id) )
								{
									this.board.drawTile(icol+1, irow+1, tile);
								}

								if ( import_direction == "v" )
								{
									if ( irow < Board.num_rows - 2 )
									{
										irow += 2;
									}
									else if ( icol < Board.num_cols - 2 )
									{
										icol += 2;
										irow = 1;
									}
									else
									{
										System.Windows.Forms.MessageBox.Show("Not enough room to import tiles!");
									}
								}
								else if ( import_direction == "h" )
								{
									if ( icol < Board.num_cols - 2 )
									{
										icol += 2;
									}
									else if ( irow < Board.num_rows - 2 )
									{
										irow += 2;
										icol = 1;
									}
									else
									{
										System.Windows.Forms.MessageBox.Show("Not enough room to import tiles!");
									}
								}
							}
						}
						this.rbImportTiles.Checked = false;
						this.rbActionOff.Checked = true;
						this.pb_board.Refresh();
						this.board.updateScore();
						this.board.updateTileCount();
					}
					// dump tile patterns to loadsave log if checkbox ticked
					if ( this.rbDumpTiles.Checked )
					{
						int tile_id = this.board.getTileIdFromColRow(col, row);
						if ( tile_id > 0 )
						{
							string pattern = this.board.tileset[tile_id-1].pattern;
							Program.TheMainForm.tbSLog1.Text += pattern + "\r\n";
						}
					}
					// build nav path for PathBuilder
					if ( this.rbBuildPath.Checked )
					{
						int bpos = (row - 1) * Board.num_cols + col;
						if ( this.nav_path.Contains(bpos.ToString()) )
						{
							// remove overlay
							int overlayId = this.nav_path.IndexOf(bpos.ToString());
							if ( this.board.overlays.Count > overlayId )
							{
								System.Windows.Forms.Label overlay = (System.Windows.Forms.Label)this.board.overlays[overlayId];
								overlay.Dispose();
								this.board.overlays.Remove(overlay);
							}

							// remove from path
							this.nav_path.Remove(bpos.ToString());
						}
						else
						{
							this.nav_path.Add(bpos.ToString());
							this.board.createCellOverlay(col, row, this.nav_path.Count.ToString() + ". " + bpos.ToString(), System.Drawing.Color.LightGreen);
						}
						this.selS1SolveMethod.Text = "Custom";
						this.inputS1SolvePath.Text = String.Join(",", this.nav_path.ToArray());
					}
					// check if move tile mode
					if ( this.rbMoveTiles.Checked )
					{
						if ( this.move_tile_id == 0 )
						{
							// select tile to move
							this.move_tile_id = this.board.getTileIdFromColRow(col, row);
							this.move_tile_pos = new int[2];
							this.move_tile_pos[0] = col;
							this.move_tile_pos[1] = row;
							this.log("selecting tile to move: " + this.move_tile_id);
						}
//						else if ( this.selTile.Text != "" )
						else
						{
							if ( this.move_tile_pos[0] == col && this.move_tile_pos[1] == row )
							{
								// avoid moving tile onto self
								break;
							}

							Tile destTile = this.board.getTileFromColRow(col, row);
							if ( destTile == null )
							{
								this.log("moving to dest cell: " + col + "," + row);
								// move tile
								// remove from old cell
								int srcPos = (this.move_tile_pos[1] - 1) * Board.num_cols + this.move_tile_pos[0];
								this.board.tilepos[srcPos-1] = 0;
								this.board.clearCell(this.move_tile_pos[0], this.move_tile_pos[1]);

								// move tile to new cell
								int destPos = (row - 1) * Board.num_cols + col;
								this.board.tilepos[destPos-1] = this.move_tile_id;
								this.board.drawTileId(col, row, this.move_tile_id, -1);

								// update screen
								this.board.updateScore();
								this.board.refresh();

								// cleanup
								this.move_tile_id = 0;
								this.move_tile_pos = new int[0];
							}
							else
							{
								// swap tiles
								// remove from old cell
								int srcPos = (this.move_tile_pos[1] - 1) * Board.num_cols + this.move_tile_pos[0];
								this.board.tilepos[srcPos-1] = 0;

								// move src tile to dest cell
								int destPos = (row - 1) * Board.num_cols + col;
								int destTileId = this.board.tilepos[destPos-1];
								this.board.tilepos[destPos-1] = this.move_tile_id;
								this.board.drawTileId(col, row, this.move_tile_id, -1);

								// move dest tile to src cell
								this.board.tilepos[srcPos-1] = destTileId;
								this.board.drawTileId(this.move_tile_pos[0], this.move_tile_pos[1], destTileId, -1);

								// update screen
								this.board.updateScore();
								this.board.refresh();

								// cleanup
								this.move_tile_id = 0;
								this.move_tile_pos = new int[0];
							}
						}
					}

					// show info / place tile
					this.showTileInfo(col, row);
					break;
				case "Right":
					// rotate tile
					this.board.rotateTile(col, row);
					this.showTileInfo(col, row);
					this.board.refresh();
					break;
				case "Middle":
					// remove tile
					this.board.removeTile(col, row);
					this.clearTileInfo();
					this.board.refresh();
					break;
			}
		}

		public void handleBoardMouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.handleBoardClick(sender, (System.EventArgs)e);
		}

		public void showTileInfo(int col, int row)
		{
			Tile tile = this.board.getTileFromColRow(col, row);
			if ( tile != null )
			{
				this.selTile.Text = String.Format("#{0:000}", tile.id);
				this.selTilePattern.Text = "R" + tile.rotation.ToString() + " " + tile.pattern;
			}
			else
			{
				this.clearTileInfo();
			}
			// update match pattern
			if ( Program.TheMainForm.rbSearchRegex.Checked )
			{
				this.selMatchPattern.Text = this.board.getTileMatchString(col, row);
			}
			else
			{
				// substring search
				this.board.getTileMatchString(col, row);
				this.selMatchPattern.Text = this.board.searchMatch;
			}
		}

		public void clearTileInfo()
		{
			this.selTile.Text = "";
			this.selTilePattern.Text = "";
		}

		public void checkMoveStatus(object sender, System.EventArgs e)
		{
			// clear move selection
			this.move_tile_id = 0;
			this.move_tile_pos = new int[0];
		}

		public void tabBoardEnter(object sender, System.EventArgs e)
		{
			this.board.drawTiles();
		}

		void BtPlaceTileClick(object sender, System.EventArgs e)
		{
			// place tile - popup
			if ( this.ts == null || this.ts.IsDisposed )
			{
				this.ts = new TileSelector();
				this.ts.loadImages(Board.title);
			}
			this.ts.Show();
			this.ts.Focus();
		}


		void BtCountIntersectingTilesClick(object sender, System.EventArgs e)
		{
			this.board.countIntersectingTiles();
		}

		void BtClearOverlaysClick(object sender, System.EventArgs e)
		{
			this.board.clearOverlays();
		}

		void BtGetModelClick(object sender, System.EventArgs e)
		{
			this.board.getModel();
		}

		void BtSaveModelClick(object sender, System.EventArgs e)
		{
			this.board.saveModel();
		}

		void BtSetModelClick(object sender, System.EventArgs e)
		{
			this.board.setModel();
		}

		void BtNewClick(object sender, System.EventArgs e)
		{
			this.createSolutionModel();
			this.board.setModel();
		}

		void BtStatsPatternCountClick(object sender, System.EventArgs e)
		{

		}

		void BtNewTilesetClick(object sender, System.EventArgs e)
		{
			this.createNewTileset();
		}

		void InputSeedEnter(object sender, System.EventArgs e)
		{
			if ( this.inputSeed.Text == "seed = random" )
			{
				this.inputSeed.Text = "";
			}
		}

		void BtSetTilesetClick(object sender, System.EventArgs e)
		{
			this.board.setTileset();
		}

		void BtSaveTilesetClick(object sender, System.EventArgs e)
		{
			this.board.saveTileset();
		}

		void BtStatisticsClick(object sender, System.EventArgs e)
		{
			this.board.updateStatistics();
		}

		void BtCreateUniqueBoardsClick(object sender, System.EventArgs e)
		{
			this.board.createUniqueBoards();
		}

		void BtClearLoadSaveLogClick(object sender, System.EventArgs e)
		{
			this.textLoadSaveLog.Text = "";
			this.textLoadSaveLog.Refresh();
		}

		void btClearSLog1Click(object sender, System.EventArgs e)
		{
			this.tbSLog1.Text = "";
			this.tbSLog1.Refresh();
			this.board.clearPatternStats();
		}

		void BtTilesetR1Click(object sender, System.EventArgs e)
		{
			string[] tiles = this.textTileset.Text.Split('\n');
			this.textTileset.Text = this.board.rotateTileset(tiles, 1);
			this.textTileset.Refresh();
			this.board.setTileset();
		}

		void BtTilesetR2Click(object sender, System.EventArgs e)
		{
			string[] tiles = this.textTileset.Text.Split('\n');
			this.textTileset.Text = this.board.rotateTileset(tiles, 2);
			this.textTileset.Refresh();
			this.board.setTileset();
		}

		void BtTilesetR3Click(object sender, System.EventArgs e)
		{
			string[] tiles = this.textTileset.Text.Split('\n');
			this.textTileset.Text = this.board.rotateTileset(tiles, 3);
			this.textTileset.Refresh();
			this.board.setTileset();
		}

		void BtTilesetR4Click(object sender, System.EventArgs e)
		{
			string[] tiles = this.textTileset.Text.Split('\n');
			this.textTileset.Text = this.board.rotateTileset(tiles, 4);
			this.textTileset.Refresh();
			this.board.setTileset();
		}

		void BtGenerate3x3listClick(object sender, System.EventArgs e)
		{
			Solver2x2 s = new Solver2x2();
			s.saveToFile = true;
			s.generateList("2x2_corner_tl");
			s.generateList("2x2_edge_left");
//			s.generateList("2x2_internal");
//			s.generateList("2x2_internal_139");

//			s.generateList("3x3_corner");
//			s.generateList("3x3_internal");
		}

		void BtSolvePuzzleClick(object sender, System.EventArgs e)
		{
			if ( this.solver2 != null )
			{
				this.solver2.isPaused = false;
				this.solver2.isResumed = false;
				this.solver2.solve();
			}
			else
			{
				System.Windows.Forms.MessageBox.Show("Load Dataset first.");
			}
		}

		void BtLoadDatasetClick(object sender, System.EventArgs e)
		{
			this.solver2 = new Solver2x2();
			this.solver2.loadLists();
		}


		void BtPauseClick(object sender, System.EventArgs e)
		{
			if ( this.solver2 != null )
			{
				this.solver2.pause();
			}
		}

		void BtResumeClick(object sender, System.EventArgs e)
		{
			if ( this.solver2 != null )
			{
				this.solver2.resume();
			}
		}

		void BtSaveQueueClick(object sender, System.EventArgs e)
		{
			if ( this.solver2 != null )
			{
				this.solver2.saveQueue();
			}
		}

		void BtLoadQueueClick(object sender, System.EventArgs e)
		{
			if ( this.solver2 != null )
			{
				string qid = Program.TheMainForm.selSolver2QueueList.SelectedItem.ToString();
				this.solver2.loadQueue(qid);
			}
		}

		void Button1Click(object sender, System.EventArgs e)
		{
			// execute SQL
			this.logwindow.Text = "";
			this.timer.start("sql");
			string sql = this.cmdSQL.Text;
			List<string> rows = this.executeSQL(sql);
			string result = String.Join("\r\n", rows.ToArray()) + "\r\n";
			this.log(rows.Count.ToString() + " result(s)");
			this.timer.stop("sql");
			this.log(this.timer.results("sql"));
			this.log("\r\n" + result);
		}

		void BtClearSolver2LogClick(object sender, EventArgs e)
		{
			this.textSolver2Log.Text = "";
			this.textSolver2Log.Refresh();
		}

		void Button8Click(object sender, EventArgs e)
		{
			if ( this.solver2.isPaused )
			{
				this.solver2.generateStats();
			}
		}

		void TabSolver1Enter(object sender, EventArgs e)
		{
			this.initSolver(false);
		}

		void BtStartS1Click(object sender, EventArgs e)
		{
			if ( this.solver != null )
			{
				this.solver.isPaused = false;
				this.solver.isResumed = false;
				this.solver.isStopped = false;

				// run solver
				this.solver.start();
			}
		}

		void BtPauseS1Click(object sender, EventArgs e)
		{
			if ( this.solver != null )
			{
				this.solver.pause();
			}
		}


		void BtResumeS1Click(object sender, EventArgs e)
		{
			if ( this.solver != null )
			{
				this.solver.resume();
			}
		}

		void BtLoadQS1Click(object sender, EventArgs e)
		{
			this.solver.loadQueue(this.selQS1.Text);
		}

		void BtSaveQS1Click(object sender, EventArgs e)
		{
			this.solver.saveQueue();
		}

		void BtClearLogS1Click(object sender, EventArgs e)
		{
			this.logS1.Clear();
		}

		void BtS1StatsClick(object sender, EventArgs e)
		{
			this.solver.dumpInfo();
		}

		void BtS1DumpBoardClick(object sender, EventArgs e)
		{
			this.solver.dumpBoard();
		}

		void BtS1DumpQueueClick(object sender, EventArgs e)
		{
			this.solver.dumpQueue(true);
		}

		void BtLoadHintsClick(object sender, EventArgs e)
		{
			this.initSolver(false);
			this.solver.loadHints(this.selHints.Text);
			this.solver.applyHints();
//			this.solver.setModel();
		}

		void BtSaveHintsClick(object sender, EventArgs e)
		{
			if ( this.solver != null )
			{
				if ( this.textSaveHintsName.Text == "" )
				{
					System.Windows.Forms.MessageBox.Show("Please enter a hints filename to save.");
					return;
				}
				this.solver.saveHints(this.textSaveHintsName.Text);
			}
		}

		void BtClearModelClick(object sender, EventArgs e)
		{
			this.textModel.Clear();
		}

		void BtClearHintsClick(object sender, EventArgs e)
		{
			this.textHints.Clear();
			if ( this.solver != null )
			{
				this.solver.hintTiles = new Dictionary<int, int>();
			}
		}

		void BtSetBoardAsHintClick(object sender, EventArgs e)
		{
			this.board.setBoardAsHint();
		}

		void BtClearSLog1Click(object sender, EventArgs e)
		{
			this.logS1Stats.Clear();
		}

		void BtS1ClearDumpClick(object sender, EventArgs e)
		{
			this.tbSLog2.Clear();
		}

		void BtS1ResetClick(object sender, EventArgs e)
		{
			this.solver.reset();
		}

		void SelS1SolvePathSelectedIndexChanged(object sender, EventArgs e)
		{
			this.inputS1CurrentSolveMethod.Text = this.selS1SolveMethod.Text;
			if ( this.selS1SolveMethod.Text != "Custom" )
			{
				this.solver.setSolveMethod(this.inputS1CurrentSolveMethod.Text, 0);
				this.solver.updateStats(true);
			}
		}

		void CbS1EnableScoreTriggerCheckedChanged(object sender, EventArgs e)
		{
			if ( this.cbS1EnableScoreTrigger.Checked )
			{
				this.grS1AutoScoreTrigger.Enabled = true;
			}
			else
			{
				this.grS1AutoScoreTrigger.Enabled = false;
			}
		}

		void CbS1EnableBacktrackLimitCheckedChanged(object sender, EventArgs e)
		{
			if ( this.cbS1EnableBacktrackLimit.Checked )
			{
				this.grS1BacktrackOptions.Enabled = true;
			}
			else
			{
				this.grS1BacktrackOptions.Enabled = false;
			}
		}

		void CbS1UseRandomSeedCheckedChanged(object sender, EventArgs e)
		{
			if ( this.cbS1UseRandomSeed.Checked )
			{
				this.grS1RandomTileQueue.Enabled = true;
			}
			else
			{
				this.grS1RandomTileQueue.Enabled = false;
			}
		}

		void CbS1AutoSaveByIntervalCheckedChanged(object sender, EventArgs e)
		{
			if ( this.cbS1AutoSaveByInterval.Checked )
			{
				this.inputS1AutoSaveIterations.Enabled = true;
			}
			else
			{
				this.inputS1AutoSaveIterations.Enabled = false;
			}
		}

		void CbS1IterationLimitCheckedChanged(object sender, EventArgs e)
		{
			if ( this.cbS1IterationLimit.Checked )
			{
				this.inputS1MaxIterations.Enabled = true;
			}
			else
			{
				this.inputS1MaxIterations.Enabled = false;
			}
		}

		void SelDebugLevelSelectedIndexChanged(object sender, EventArgs e)
		{
			this.initSolver(false);
			this.solver.setDebugLevel();
		}

		void CbS1RandomSeedsCheckedChanged(object sender, EventArgs e)
		{
			if ( this.cbS1RandomSeeds.Checked )
			{
				this.inputS1StartSeed.Enabled = false;
				this.inputS1SeedStep.Enabled = false;
			}
			else
			{
				this.inputS1StartSeed.Enabled = true;
				this.inputS1SeedStep.Enabled = true;
			}
		}


		void BtSetModelAsHintClick(object sender, EventArgs e)
		{
			this.board.setBoardAsHint();
		}

		// save results
		void Button2Click(object sender, EventArgs e)
		{
			this.initSolver(false);
			this.solver.saveResults(true);
			this.solver.saveLog();
		}

		void BtDumpTilesetClick(object sender, EventArgs e)
		{
			this.initSolver(false);
			this.solver.dumpTileset();
		}

		void SelS1PathFilterSelectedIndexChanged(object sender, EventArgs e)
		{
			this.solver.pb.selected_filter = this.selS1PathFilter.Text;
			this.solver.updateStats(true);
		}


		void BtClearSearchPattenrClick(object sender, EventArgs e)
		{
			this.selMatchPattern.Clear();
		}

		void BtPatternTallyClick(object sender, EventArgs e)
		{
			// count patterns from tiles in log
			List<string> tiles = new List<string>(Program.TheMainForm.tbSLog1.Text.Split('\n'));

			PatternStats ps = new PatternStats();
			foreach (string line in tiles)
			{
				// import only lines with tile patterns ABCD
				if ( !line.StartsWith("//") && line.Trim().Length == 4 )
				{
					ps.patterns.Add(line.Trim());
				}
			}
			Program.TheMainForm.tbSLog2.Text = ps.getStatsCSV();
			Program.TheMainForm.tbSLog2.Update();

		}

		void BtCopyClick(object sender, EventArgs e)
		{
			this.tbSLog1.SelectAll();
			this.tbSLog1.Copy();
		}

		void BtCopy2Click(object sender, EventArgs e)
		{
			this.tbSLog2.SelectAll();
			this.tbSLog2.Copy();
		}


		void BtDumpBoardClick(object sender, EventArgs e)
		{
			this.board.dumpTiles();
		}

		void RbBuildPathCheckedChanged(object sender, EventArgs e)
		{
			if ( this.rbBuildPath.Checked )
			{
				this.board.clearOverlays();
				this.nav_path = new List<string>();
				this.inputS1SolvePath.Clear();
			}
		}

		void SearchUsedResultsImagesSelectedIndexChanged(object sender, EventArgs e)
		{
			if ( this.searchUsedResultsImages.SelectedIndices.Count > 0 )
			{
				// highlight used tiles
//				this.board.highlightSwappableTile((int)this.searchUsedResultsImages.SelectedIndices[0]);

				// extract tileId & rotation
				string[] parts = this.searchUsedResultsImages.SelectedItems[0].Text.Split(' ');
				// tileId,rotation,pattern
				int tileId = System.Convert.ToInt16(parts[0]);
				int rotation = System.Convert.ToInt16(parts[1]);
				int[] colrow = this.getColRow();
				if ( colrow[0] > 0 && colrow[1] > 0 )
				{
					// remove from previous position
					string[] prevpos = parts[3].Split(',');
					this.board.removeTile(System.Convert.ToInt16(prevpos[0]), System.Convert.ToInt16(prevpos[1]));

					// place tile
					this.board.drawTileId(colrow[0], colrow[1], tileId, rotation);
					this.board.updateScore();
					this.board.updateTileCount();
					this.board.refresh();
				}
			}
		}


		void SearchFreeResultsImagesSelectedIndexChanged(object sender, EventArgs e)
		{
			if ( this.searchFreeResultsImages.SelectedIndices.Count > 0 )
			{
				// extract tileId & rotation
				string[] parts = this.searchFreeResultsImages.SelectedItems[0].Text.Split(' ');
				// tileId,rotation,pattern
				int tileId = System.Convert.ToInt16(parts[0]);
				int rotation = System.Convert.ToInt16(parts[1]);
				int[] colrow = this.getColRow();
				if ( colrow[0] > 0 && colrow[1] > 0 )
				{
					this.board.drawTileId(colrow[0], colrow[1], tileId, rotation);
					this.board.updateScore();
					this.board.updateTileCount();
					this.board.refresh();
				}
			}
		}


		void BtClearSolutionsClick(object sender, EventArgs e)
		{
			this.solver.clearSolutions();
		}

		void BtClearScoresClick(object sender, EventArgs e)
		{
			this.solver.clearScores();
		}

		void BtStopClick(object sender, EventArgs e)
		{
			this.solver.stop();
		}

		void BtSetLayoutClick(object sender, EventArgs e)
		{
			this.board.setLayout();
		}

		void BtCancelClick(object sender, EventArgs e)
		{
			this.board.isCancel = true;
		}


		void BtShuffleClick(object sender, EventArgs e)
		{
			this.board.shuffle();
			this.createSolutionModel();
		}

		void BtCalcHashClick(object sender, EventArgs e)
		{
			// tileset hash
			string thash = Utils.getSHA1(this.textTileset.Text.Trim());

			// get model hash
			List<string> data = new List<string>();
			for ( int i = 0; i < this.board.tilepos.Length; i++ )
			{
				if ( this.board.tilepos[i] > 0 )
				{
					data.Add(this.board.tileset[this.board.tilepos[i]-1].pattern.Trim());
				}
			}
			string mhash = Utils.getSHA1(String.Join("\r\n", data.ToArray()));

			Program.TheMainForm.textLoadSaveLog.Text += "Board: " + Board.title + ", hash = " + thash + "\r\n";
			Program.TheMainForm.textLoadSaveLog.Text += "Model: " + Board.title + ", hash = " + mhash + "\r\n";
			Program.TheMainForm.textLoadSaveLog.Update();
		}

		void BtCopyDumpClick(object sender, EventArgs e)
		{
			this.logS1Stats.SelectAll();
			this.logS1Stats.Copy();
		}

		void BtCopySolvePathClick(object sender, EventArgs e)
		{
			this.inputS1SolvePath.SelectAll();
			this.inputS1SolvePath.Copy();
		}

		void BtClearSolvePathClick(object sender, EventArgs e)
		{
			this.inputS1SolvePath.Clear();
		}

		void BtRunCmdClick(object sender, EventArgs e)
		{
			this.solver.runCmd();
		}

		void BtCopyCmdClick(object sender, EventArgs e)
		{
			this.inputCmd.SelectAll();
			this.inputCmd.Copy();
		}

		void BtClearCmdClick(object sender, EventArgs e)
		{
			this.inputCmd.Clear();
		}

		void BtStepBackClick(object sender, EventArgs e)
		{
			if ( this.solver.isPaused )
			{
				this.solver.stepBack();
			}
		}

		void BtStepForwardClick(object sender, EventArgs e)
		{
			if ( this.solver.isPaused )
			{
				this.solver.stepForward();
			}
		}

		void BtCopyTilesetClick(object sender, EventArgs e)
		{
			this.textTileset.SelectAll();
			this.textTileset.Copy();
		}

		void BtCopyModelClick(object sender, EventArgs e)
		{
			this.textModel.SelectAll();
			this.textModel.Copy();
		}

		void BtSelectPathClick(object sender, EventArgs e)
		{
			this.selectPath();
		}

		void BtBuildFilterClick(object sender, EventArgs e)
		{
			if ( this.solver == null )
			{
				this.initSolver(false);
			}
			if ( this.solver != null )
			{
				this.statusInstructions.Text = "Building filter...";
				this.solver.buildCellFilter();
				this.statusInstructions.Text = "";
			}
			else
			{
				this.statusInstructions.Text = "Solver not initialised!";
			}
		}

		void BtSelectTilesClick(object sender, EventArgs e)
		{
			if ( this.solver == null )
			{
				this.initSolver(false);
			}
			if ( this.solver != null )
			{
				this.solver.selectTiles();
			}
			else
			{
				this.statusInstructions.Text = "Solver not initialised!";
			}
		}


		void BtRunClick(object sender, EventArgs e)
		{
			switch ( this.selCommands.Text )
			{
				case "compare_solution":
					this.board.compareBoardToTileset();
					break;
				case "pattern_stats":
					this.board.showPatternStats();
					break;
				case "pattern_stats_individual":
					this.board.showIndividualPatternStats();
					break;
				case "bgcolour_internal_stats":
					this.board.showBGColourStats("internal");
					break;
				case "bgcolour_all_stats":
					this.board.showBGColourStats("all");
					break;
				case "shape_stats":
					this.board.showShapeStats();
					break;
				case "tile_analysis":
					this.board.showTileAnalysis();
					break;
				case "tile_bgcolour_count":
					this.board.showBGColourCount(false);
					break;
				case "tile_bgcolour_distinct_count":
					this.board.showBGColourCount(true);
					break;
				case "tile_pattern_count":
					this.board.showTilePatternCount(false);
					break;
				case "tile_pattern_distinct_count":
					this.board.showTilePatternCount(true);
					break;
				case "tile_shape_count":
					this.board.showShapeCount(false);
					break;
				case "tile_shape_distinct_count":
					this.board.showShapeCount(true);
					break;
			}
		}

		void BtCompareToSolutionClick(object sender, EventArgs e)
		{
			this.board.compareBoardToTileset();
		}

		void BtNewDesignClick(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.newDesign();
		}

		void BtImportDesignClick(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.importDesign();
		}

		void BtExportDesignClick(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.exportDesign();
		}

		void BtCopyDesignLogClick(object sender, EventArgs e)
		{
			this.tbDesignLog.SelectAll();
			this.tbDesignLog.Copy();
		}

		void BtClearDesignLogClick(object sender, EventArgs e)
		{
			this.tbDesignLog.Clear();
		}

		void Pb_designMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.handleBoardClick(sender, e);
		}

		void BtSaveBoardAsImageClick(object sender, EventArgs e)
		{
			bool setting = this.board.saveSolutionImages;
			this.board.saveSolutionImages = true;
			this.board.saveModelImage(Board.title);
			this.board.saveSolutionImages = setting;
		}

		void TabDesignEnter(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
		}

		void BtExportFilterClick(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.exportFilter("");
		}

		void BtExportTilesetClick(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.exportTileset();
		}

		void BtImportTilesetClick(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.importTileset();
		}

		void BtFillLineClick(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.fillLine();
		}

		void BtRunDesignCmdClick(object sender, EventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.runCmd(this.lbDesignCmd.Text);
		}

		void BtFindSwapsClick(object sender, EventArgs e)
		{
			if ( this.tsui == null || this.tsui.IsDisposed )
			{
				this.tsui = new TileSwapUI();
				//this.board.findSwaps();
			}
			this.tsui.Show();
			this.tsui.Focus();
		}



		void Pb_designMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.handleMouseMovement(sender, e);
		}

		public void Pb_designMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.handleMouseWheel(sender, e);
		}


		void BtLoadDesignClick(object sender, EventArgs e)
		{
			// use file open dialog
			System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.FileName = "";
			openFileDialog1.Filter = "Board designs|*.txt|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true;
			openFileDialog1.InitialDirectory = "designs";
			openFileDialog1.Title = "Load Design";

			bool isFileSelected = false;
			if ( openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && openFileDialog1.FileName != "" )
			{
				isFileSelected = true;
			}
			if ( !isFileSelected )
			{
				System.Windows.Forms.MessageBox.Show("No design selected");
				return;
			}
			this.tbDesignLog.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
			if ( this.design == null )
			{
				this.design = new BoardDesigner();
			}
			this.design.newDesign();
			this.design.importDesign();
			this.statusInstructions.Text = "Loaded design from " + openFileDialog1.FileName;
		}

		void BtSaveDesignClick(object sender, EventArgs e)
		{
			System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			saveFileDialog1.FileName = "design.txt";
			saveFileDialog1.Filter = "Board designs (*.txt)|*.txt";
			saveFileDialog1.FilterIndex = 1;
			saveFileDialog1.RestoreDirectory = true;
			saveFileDialog1.InitialDirectory = "designs";
			saveFileDialog1.Title = "Save Board Design";

			bool isFileSelected = false;
			if ( saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName != "" )
			{
				isFileSelected = true;
			}
			if ( !isFileSelected )
			{
				System.Windows.Forms.MessageBox.Show("No destination filename entered");
				return;
			}
			this.design.exportDesign();
			System.IO.File.WriteAllText(saveFileDialog1.FileName, this.tbDesignLog.Text);
			this.statusInstructions.Text = "Saved design to " + saveFileDialog1.FileName;
		}

	}
}

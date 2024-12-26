/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 25/11/2009
 * Time: 8:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ET2Solver
{
	partial class TileSelector
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
			Program.TheMainForm.board.refresh();
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
			this.btClose = new System.Windows.Forms.Button();
			this.listTilesetImages = new System.Windows.Forms.ListView();
			this.textTileDump = new System.Windows.Forms.TextBox();
			this.btClearDump = new System.Windows.Forms.Button();
			this.btCopy = new System.Windows.Forms.Button();
			this.btImport = new System.Windows.Forms.Button();
			this.searchResults = new System.Windows.Forms.TextBox();
			this.btViewTileset = new System.Windows.Forms.Button();
			this.input2x2Filter = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btCopySearchResults = new System.Windows.Forms.Button();
			this.btCopyExcludeFilter = new System.Windows.Forms.Button();
			this.btClearExcludeFilter = new System.Windows.Forms.Button();
			this.cbCountOnly = new System.Windows.Forms.CheckBox();
			this.btSearchTileset = new System.Windows.Forms.Button();
			this.btClearInputSearch = new System.Windows.Forms.Button();
			this.inputSearch = new System.Windows.Forms.TextBox();
			this.btSearch2x2 = new System.Windows.Forms.Button();
			this.labelSearchStatus = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbImportTiles = new System.Windows.Forms.CheckBox();
			this.cbUniquesOnly = new System.Windows.Forms.CheckBox();
			this.cbCountPatterns = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// btClose
			// 
			this.btClose.Location = new System.Drawing.Point(759, 803);
			this.btClose.Name = "btClose";
			this.btClose.Size = new System.Drawing.Size(75, 23);
			this.btClose.TabIndex = 34;
			this.btClose.Text = "Close";
			this.btClose.UseVisualStyleBackColor = true;
			this.btClose.Click += new System.EventHandler(this.BtCloseClick);
			// 
			// listTilesetImages
			// 
			this.listTilesetImages.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listTilesetImages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listTilesetImages.HideSelection = false;
			this.listTilesetImages.Location = new System.Drawing.Point(1, 2);
			this.listTilesetImages.MultiSelect = false;
			this.listTilesetImages.Name = "listTilesetImages";
			this.listTilesetImages.ShowGroups = false;
			this.listTilesetImages.ShowItemToolTips = true;
			this.listTilesetImages.Size = new System.Drawing.Size(836, 568);
			this.listTilesetImages.TabIndex = 56;
			this.listTilesetImages.TileSize = new System.Drawing.Size(50, 50);
			this.listTilesetImages.UseCompatibleStateImageBehavior = false;
			this.listTilesetImages.SelectedIndexChanged += new System.EventHandler(this.ListTilesetImagesSelectedIndexChanged);
			// 
			// textTileDump
			// 
			this.textTileDump.Location = new System.Drawing.Point(1, 594);
			this.textTileDump.Multiline = true;
			this.textTileDump.Name = "textTileDump";
			this.textTileDump.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textTileDump.Size = new System.Drawing.Size(154, 201);
			this.textTileDump.TabIndex = 57;
			// 
			// btClearDump
			// 
			this.btClearDump.Location = new System.Drawing.Point(111, 801);
			this.btClearDump.Name = "btClearDump";
			this.btClearDump.Size = new System.Drawing.Size(44, 23);
			this.btClearDump.TabIndex = 58;
			this.btClearDump.Text = "Clear";
			this.btClearDump.UseVisualStyleBackColor = true;
			this.btClearDump.Click += new System.EventHandler(this.BtClearDumpClick);
			// 
			// btCopy
			// 
			this.btCopy.Location = new System.Drawing.Point(1, 801);
			this.btCopy.Name = "btCopy";
			this.btCopy.Size = new System.Drawing.Size(48, 23);
			this.btCopy.TabIndex = 59;
			this.btCopy.Text = "Copy";
			this.btCopy.UseVisualStyleBackColor = true;
			this.btCopy.Click += new System.EventHandler(this.BtCopyClick);
			// 
			// btImport
			// 
			this.btImport.Location = new System.Drawing.Point(54, 801);
			this.btImport.Name = "btImport";
			this.btImport.Size = new System.Drawing.Size(51, 23);
			this.btImport.TabIndex = 60;
			this.btImport.Text = "Import";
			this.btImport.UseVisualStyleBackColor = true;
			this.btImport.Click += new System.EventHandler(this.BtImportClick);
			// 
			// searchResults
			// 
			this.searchResults.Location = new System.Drawing.Point(161, 594);
			this.searchResults.Multiline = true;
			this.searchResults.Name = "searchResults";
			this.searchResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.searchResults.Size = new System.Drawing.Size(257, 201);
			this.searchResults.TabIndex = 64;
			// 
			// btViewTileset
			// 
			this.btViewTileset.Location = new System.Drawing.Point(665, 803);
			this.btViewTileset.Name = "btViewTileset";
			this.btViewTileset.Size = new System.Drawing.Size(88, 23);
			this.btViewTileset.TabIndex = 65;
			this.btViewTileset.Text = "Refresh Tileset";
			this.btViewTileset.UseVisualStyleBackColor = true;
			this.btViewTileset.Click += new System.EventHandler(this.BtViewTilesetClick);
			// 
			// input2x2Filter
			// 
			this.input2x2Filter.Location = new System.Drawing.Point(424, 594);
			this.input2x2Filter.Multiline = true;
			this.input2x2Filter.Name = "input2x2Filter";
			this.input2x2Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.input2x2Filter.Size = new System.Drawing.Size(169, 201);
			this.input2x2Filter.TabIndex = 67;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(424, 575);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(169, 16);
			this.label1.TabIndex = 68;
			this.label1.Text = "Exclude Filter";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(161, 575);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(169, 16);
			this.label2.TabIndex = 69;
			this.label2.Text = "Search Results";
			// 
			// btCopySearchResults
			// 
			this.btCopySearchResults.Location = new System.Drawing.Point(161, 801);
			this.btCopySearchResults.Name = "btCopySearchResults";
			this.btCopySearchResults.Size = new System.Drawing.Size(50, 23);
			this.btCopySearchResults.TabIndex = 71;
			this.btCopySearchResults.Text = "Copy";
			this.btCopySearchResults.UseVisualStyleBackColor = true;
			this.btCopySearchResults.Click += new System.EventHandler(this.BtCopySearchResultsClick);
			// 
			// btCopyExcludeFilter
			// 
			this.btCopyExcludeFilter.Location = new System.Drawing.Point(424, 801);
			this.btCopyExcludeFilter.Name = "btCopyExcludeFilter";
			this.btCopyExcludeFilter.Size = new System.Drawing.Size(48, 23);
			this.btCopyExcludeFilter.TabIndex = 73;
			this.btCopyExcludeFilter.Text = "Copy";
			this.btCopyExcludeFilter.UseVisualStyleBackColor = true;
			this.btCopyExcludeFilter.Click += new System.EventHandler(this.BtCopyExcludeFilterClick);
			// 
			// btClearExcludeFilter
			// 
			this.btClearExcludeFilter.Location = new System.Drawing.Point(478, 801);
			this.btClearExcludeFilter.Name = "btClearExcludeFilter";
			this.btClearExcludeFilter.Size = new System.Drawing.Size(44, 23);
			this.btClearExcludeFilter.TabIndex = 74;
			this.btClearExcludeFilter.Text = "Clear";
			this.btClearExcludeFilter.UseVisualStyleBackColor = true;
			this.btClearExcludeFilter.Click += new System.EventHandler(this.BtClearExcludeFilterClick);
			// 
			// cbCountOnly
			// 
			this.cbCountOnly.Location = new System.Drawing.Point(599, 706);
			this.cbCountOnly.Name = "cbCountOnly";
			this.cbCountOnly.Size = new System.Drawing.Size(104, 19);
			this.cbCountOnly.TabIndex = 76;
			this.cbCountOnly.Text = "Count Only";
			this.cbCountOnly.UseVisualStyleBackColor = true;
			// 
			// btSearchTileset
			// 
			this.btSearchTileset.Location = new System.Drawing.Point(599, 677);
			this.btSearchTileset.Name = "btSearchTileset";
			this.btSearchTileset.Size = new System.Drawing.Size(86, 23);
			this.btSearchTileset.TabIndex = 80;
			this.btSearchTileset.Text = "Search Tileset";
			this.btSearchTileset.UseVisualStyleBackColor = true;
			this.btSearchTileset.Click += new System.EventHandler(this.BtSearchTilesetClick);
			// 
			// btClearInputSearch
			// 
			this.btClearInputSearch.Location = new System.Drawing.Point(772, 677);
			this.btClearInputSearch.Name = "btClearInputSearch";
			this.btClearInputSearch.Size = new System.Drawing.Size(58, 23);
			this.btClearInputSearch.TabIndex = 79;
			this.btClearInputSearch.Text = "Clear";
			this.btClearInputSearch.UseVisualStyleBackColor = true;
			this.btClearInputSearch.Click += new System.EventHandler(this.BtClearInputSearchClick);
			// 
			// inputSearch
			// 
			this.inputSearch.Location = new System.Drawing.Point(599, 594);
			this.inputSearch.Multiline = true;
			this.inputSearch.Name = "inputSearch";
			this.inputSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.inputSearch.Size = new System.Drawing.Size(235, 77);
			this.inputSearch.TabIndex = 78;
			// 
			// btSearch2x2
			// 
			this.btSearch2x2.Location = new System.Drawing.Point(691, 677);
			this.btSearch2x2.Name = "btSearch2x2";
			this.btSearch2x2.Size = new System.Drawing.Size(75, 23);
			this.btSearch2x2.TabIndex = 77;
			this.btSearch2x2.Text = "Search 2x2s";
			this.btSearch2x2.UseVisualStyleBackColor = true;
			// 
			// labelSearchStatus
			// 
			this.labelSearchStatus.BackColor = System.Drawing.SystemColors.ControlLight;
			this.labelSearchStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelSearchStatus.Location = new System.Drawing.Point(599, 774);
			this.labelSearchStatus.Name = "labelSearchStatus";
			this.labelSearchStatus.Size = new System.Drawing.Size(231, 21);
			this.labelSearchStatus.TabIndex = 81;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(599, 575);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 82;
			this.label3.Text = "Search Strings";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(1, 575);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 16);
			this.label4.TabIndex = 83;
			this.label4.Text = "Import List";
			// 
			// cbImportTiles
			// 
			this.cbImportTiles.Location = new System.Drawing.Point(599, 752);
			this.cbImportTiles.Name = "cbImportTiles";
			this.cbImportTiles.Size = new System.Drawing.Size(104, 19);
			this.cbImportTiles.TabIndex = 84;
			this.cbImportTiles.Text = "Import Tiles";
			this.cbImportTiles.UseVisualStyleBackColor = true;
			// 
			// cbUniquesOnly
			// 
			this.cbUniquesOnly.Location = new System.Drawing.Point(599, 729);
			this.cbUniquesOnly.Name = "cbUniquesOnly";
			this.cbUniquesOnly.Size = new System.Drawing.Size(104, 19);
			this.cbUniquesOnly.TabIndex = 85;
			this.cbUniquesOnly.Text = "Uniques Only";
			this.cbUniquesOnly.UseVisualStyleBackColor = true;
			// 
			// cbCountPatterns
			// 
			this.cbCountPatterns.Location = new System.Drawing.Point(722, 706);
			this.cbCountPatterns.Name = "cbCountPatterns";
			this.cbCountPatterns.Size = new System.Drawing.Size(104, 19);
			this.cbCountPatterns.TabIndex = 86;
			this.cbCountPatterns.Text = "Count Patterns";
			this.cbCountPatterns.UseVisualStyleBackColor = true;
			// 
			// TileSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(838, 828);
			this.Controls.Add(this.cbCountPatterns);
			this.Controls.Add(this.cbUniquesOnly);
			this.Controls.Add(this.cbImportTiles);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelSearchStatus);
			this.Controls.Add(this.btSearchTileset);
			this.Controls.Add(this.btClearInputSearch);
			this.Controls.Add(this.inputSearch);
			this.Controls.Add(this.btSearch2x2);
			this.Controls.Add(this.cbCountOnly);
			this.Controls.Add(this.btClearExcludeFilter);
			this.Controls.Add(this.btCopyExcludeFilter);
			this.Controls.Add(this.btCopySearchResults);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.input2x2Filter);
			this.Controls.Add(this.btViewTileset);
			this.Controls.Add(this.searchResults);
			this.Controls.Add(this.btImport);
			this.Controls.Add(this.btCopy);
			this.Controls.Add(this.btClearDump);
			this.Controls.Add(this.textTileDump);
			this.Controls.Add(this.listTilesetImages);
			this.Controls.Add(this.btClose);
			this.Location = new System.Drawing.Point(-413, 0);
			this.Name = "TileSelector";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tileset View";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.CheckBox cbCountPatterns;
		private System.Windows.Forms.Button btClearInputSearch;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox cbCountOnly;
		private System.Windows.Forms.CheckBox cbImportTiles;
		private System.Windows.Forms.Button btClearExcludeFilter;
		private System.Windows.Forms.Button btCopyExcludeFilter;
		private System.Windows.Forms.CheckBox cbUniquesOnly;
		private System.Windows.Forms.TextBox inputSearch;
		private System.Windows.Forms.Button btCopySearchResults;
		public System.Windows.Forms.TextBox searchResults;
		private System.Windows.Forms.Button btSearchTileset;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox input2x2Filter;
		private System.Windows.Forms.Label labelSearchStatus;
		private System.Windows.Forms.Button btViewTileset;
		private System.Windows.Forms.Button btSearch2x2;
		private System.Windows.Forms.Button btImport;
		private System.Windows.Forms.Button btCopy;
		private System.Windows.Forms.Button btClearDump;
		public System.Windows.Forms.TextBox textTileDump;
		public System.Windows.Forms.ListView listTilesetImages;
		private System.Windows.Forms.Button btClose;
	}
}

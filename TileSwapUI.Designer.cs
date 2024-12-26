/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 24/10/2010
 * Time: 1:16 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ET2Solver
{
	partial class TileSwapUI
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
			this.btClose = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.btStart = new System.Windows.Forms.Button();
			this.dataSet1 = new System.Data.DataSet();
			this.tileswap_status = new System.Windows.Forms.Label();
			this.inputStartRegion = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.inputEndRegion = new System.Windows.Forms.TextBox();
			this.cbRegions = new System.Windows.Forms.CheckBox();
			this.optRegions = new System.Windows.Forms.GroupBox();
			this.tileswap_solver_status = new System.Windows.Forms.Label();
			this.btStop = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
			this.optRegions.SuspendLayout();
			this.SuspendLayout();
			// 
			// btClose
			// 
			this.btClose.Location = new System.Drawing.Point(1059, 556);
			this.btClose.Name = "btClose";
			this.btClose.Size = new System.Drawing.Size(75, 23);
			this.btClose.TabIndex = 0;
			this.btClose.Text = "Close";
			this.btClose.UseVisualStyleBackColor = true;
			this.btClose.Click += new System.EventHandler(this.BtCloseClick);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(12, 35);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.Size = new System.Drawing.Size(1122, 470);
			this.dataGridView1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(705, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Find Tile Swap Opportunities";
			// 
			// btStart
			// 
			this.btStart.Location = new System.Drawing.Point(897, 556);
			this.btStart.Name = "btStart";
			this.btStart.Size = new System.Drawing.Size(75, 23);
			this.btStart.TabIndex = 3;
			this.btStart.Text = "Start";
			this.btStart.UseVisualStyleBackColor = true;
			this.btStart.Click += new System.EventHandler(this.BtStartClick);
			// 
			// dataSet1
			// 
			this.dataSet1.DataSetName = "NewDataSet";
			// 
			// tileswap_status
			// 
			this.tileswap_status.Location = new System.Drawing.Point(12, 508);
			this.tileswap_status.Name = "tileswap_status";
			this.tileswap_status.Size = new System.Drawing.Size(809, 23);
			this.tileswap_status.TabIndex = 4;
			// 
			// inputStartRegion
			// 
			this.inputStartRegion.Location = new System.Drawing.Point(89, 12);
			this.inputStartRegion.Name = "inputStartRegion";
			this.inputStartRegion.Size = new System.Drawing.Size(50, 20);
			this.inputStartRegion.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Start Region";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(145, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "End Region";
			// 
			// inputEndRegion
			// 
			this.inputEndRegion.Location = new System.Drawing.Point(220, 12);
			this.inputEndRegion.Name = "inputEndRegion";
			this.inputEndRegion.Size = new System.Drawing.Size(47, 20);
			this.inputEndRegion.TabIndex = 7;
			// 
			// cbRegions
			// 
			this.cbRegions.AutoSize = true;
			this.cbRegions.Location = new System.Drawing.Point(842, 523);
			this.cbRegions.Name = "cbRegions";
			this.cbRegions.Size = new System.Drawing.Size(15, 14);
			this.cbRegions.TabIndex = 9;
			this.cbRegions.UseVisualStyleBackColor = true;
			// 
			// optRegions
			// 
			this.optRegions.Controls.Add(this.label2);
			this.optRegions.Controls.Add(this.inputStartRegion);
			this.optRegions.Controls.Add(this.label3);
			this.optRegions.Controls.Add(this.inputEndRegion);
			this.optRegions.Location = new System.Drawing.Point(863, 512);
			this.optRegions.Name = "optRegions";
			this.optRegions.Size = new System.Drawing.Size(271, 36);
			this.optRegions.TabIndex = 10;
			this.optRegions.TabStop = false;
			this.optRegions.Text = "Region Selection";
			// 
			// tileswap_solver_status
			// 
			this.tileswap_solver_status.Location = new System.Drawing.Point(12, 535);
			this.tileswap_solver_status.Name = "tileswap_solver_status";
			this.tileswap_solver_status.Size = new System.Drawing.Size(809, 23);
			this.tileswap_solver_status.TabIndex = 11;
			// 
			// btStop
			// 
			this.btStop.Location = new System.Drawing.Point(978, 556);
			this.btStop.Name = "btStop";
			this.btStop.Size = new System.Drawing.Size(75, 23);
			this.btStop.TabIndex = 12;
			this.btStop.Text = "Stop";
			this.btStop.UseVisualStyleBackColor = true;
			this.btStop.Click += new System.EventHandler(this.BtStopClick);
			// 
			// TileSwapUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1146, 586);
			this.Controls.Add(this.btStop);
			this.Controls.Add(this.tileswap_solver_status);
			this.Controls.Add(this.optRegions);
			this.Controls.Add(this.cbRegions);
			this.Controls.Add(this.tileswap_status);
			this.Controls.Add(this.btStart);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.btClose);
			this.Location = new System.Drawing.Point(0, 219);
			this.Name = "TileSwapUI";
			this.Text = "TileSwapUI";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
			this.optRegions.ResumeLayout(false);
			this.optRegions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btStop;
		private System.Windows.Forms.Label tileswap_solver_status;
		private System.Windows.Forms.GroupBox optRegions;
		private System.Windows.Forms.CheckBox cbRegions;
		private System.Windows.Forms.TextBox inputEndRegion;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox inputStartRegion;
		private System.Windows.Forms.Label tileswap_status;
		private System.Data.DataSet dataSet1;
		private System.Windows.Forms.Button btStart;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button btClose;
	}
}

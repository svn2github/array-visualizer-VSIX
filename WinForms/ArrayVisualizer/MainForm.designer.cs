using WinFormsArrayVisualizerControls;

namespace WinFormsArrayVisualizer
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.panel1 = new System.Windows.Forms.Panel();
      this.label12 = new System.Windows.Forms.Label();
      this.numericUpDownCellHeight = new System.Windows.Forms.NumericUpDown();
      this.label13 = new System.Windows.Forms.Label();
      this.numericUpDownCellWidth = new System.Windows.Forms.NumericUpDown();
      this.resizePanel = new System.Windows.Forms.Panel();
      this.label2A = new System.Windows.Forms.Label();
      this.numericUpDownA1 = new System.Windows.Forms.NumericUpDown();
      this.label2Z = new System.Windows.Forms.Label();
      this.numericUpDownZ1 = new System.Windows.Forms.NumericUpDown();
      this.label2Y = new System.Windows.Forms.Label();
      this.numericUpDownY1 = new System.Windows.Forms.NumericUpDown();
      this.label2X = new System.Windows.Forms.Label();
      this.numericUpDownX1 = new System.Windows.Forms.NumericUpDown();
      this.resizeButton = new System.Windows.Forms.Button();
      this.rotatePanel = new System.Windows.Forms.Panel();
      this.domainUpDownAngle = new System.Windows.Forms.DomainUpDown();
      this.domainUpDownAxis = new System.Windows.Forms.DomainUpDown();
      this.rotateButton = new System.Windows.Forms.Button();
      this.labelAngle = new System.Windows.Forms.Label();
      this.labelAxis = new System.Windows.Forms.Label();
      this.initialPanel = new System.Windows.Forms.Panel();
      this.label1A = new System.Windows.Forms.Label();
      this.numericUpDownA = new System.Windows.Forms.NumericUpDown();
      this.renderButton = new System.Windows.Forms.Button();
      this.label1Z = new System.Windows.Forms.Label();
      this.numericUpDownZ = new System.Windows.Forms.NumericUpDown();
      this.label1Y = new System.Windows.Forms.Label();
      this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
      this.label1X = new System.Windows.Forms.Label();
      this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
      this.label10 = new System.Windows.Forms.Label();
      this.numericUpDownInc = new System.Windows.Forms.NumericUpDown();
      this.label9 = new System.Windows.Forms.Label();
      this.numericUpDownStart = new System.Windows.Forms.NumericUpDown();
      this.dimensionSelector = new System.Windows.Forms.TabControl();
      this.tabPage2D = new System.Windows.Forms.TabPage();
      this.tabPage3D = new System.Windows.Forms.TabPage();
      this.tabPage4D = new System.Windows.Forms.TabPage();
      this.panel2 = new System.Windows.Forms.Panel();
      this.panelTextInput = new System.Windows.Forms.Panel();
      this.textBoxData = new System.Windows.Forms.TextBox();
      this.radioButtonAutoFill = new System.Windows.Forms.RadioButton();
      this.radioButtonManualFill = new System.Windows.Forms.RadioButton();
      this.radioButtonFileFill = new System.Windows.Forms.RadioButton();
      this.panel4 = new System.Windows.Forms.Panel();
      this.lblFile = new System.Windows.Forms.Label();
      this.buttonSelectFile = new System.Windows.Forms.Button();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.buttonSave = new System.Windows.Forms.Button();
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.mainPanel = new System.Windows.Forms.Panel();
      this.array2D1 = new WinFormsArrayVisualizerControls.Array2D();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCellHeight)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCellWidth)).BeginInit();
      this.resizePanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownA1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX1)).BeginInit();
      this.rotatePanel.SuspendLayout();
      this.initialPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownA)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInc)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart)).BeginInit();
      this.dimensionSelector.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panelTextInput.SuspendLayout();
      this.panel4.SuspendLayout();
      this.mainPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.array2D1)).BeginInit();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add(this.label12);
      this.panel1.Controls.Add(this.numericUpDownCellHeight);
      this.panel1.Controls.Add(this.label13);
      this.panel1.Controls.Add(this.numericUpDownCellWidth);
      resources.ApplyResources(this.panel1, "panel1");
      this.panel1.Name = "panel1";
      // 
      // label12
      // 
      resources.ApplyResources(this.label12, "label12");
      this.label12.Name = "label12";
      // 
      // numericUpDownCellHeight
      // 
      resources.ApplyResources(this.numericUpDownCellHeight, "numericUpDownCellHeight");
      this.numericUpDownCellHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownCellHeight.Name = "numericUpDownCellHeight";
      this.numericUpDownCellHeight.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
      // 
      // label13
      // 
      resources.ApplyResources(this.label13, "label13");
      this.label13.Name = "label13";
      // 
      // numericUpDownCellWidth
      // 
      resources.ApplyResources(this.numericUpDownCellWidth, "numericUpDownCellWidth");
      this.numericUpDownCellWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownCellWidth.Name = "numericUpDownCellWidth";
      this.numericUpDownCellWidth.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
      // 
      // resizePanel
      // 
      this.resizePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.resizePanel.Controls.Add(this.label2A);
      this.resizePanel.Controls.Add(this.numericUpDownA1);
      this.resizePanel.Controls.Add(this.label2Z);
      this.resizePanel.Controls.Add(this.numericUpDownZ1);
      this.resizePanel.Controls.Add(this.label2Y);
      this.resizePanel.Controls.Add(this.numericUpDownY1);
      this.resizePanel.Controls.Add(this.label2X);
      this.resizePanel.Controls.Add(this.numericUpDownX1);
      this.resizePanel.Controls.Add(this.resizeButton);
      resources.ApplyResources(this.resizePanel, "resizePanel");
      this.resizePanel.Name = "resizePanel";
      // 
      // label2A
      // 
      resources.ApplyResources(this.label2A, "label2A");
      this.label2A.Name = "label2A";
      // 
      // numericUpDownA1
      // 
      resources.ApplyResources(this.numericUpDownA1, "numericUpDownA1");
      this.numericUpDownA1.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
      this.numericUpDownA1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownA1.Name = "numericUpDownA1";
      this.numericUpDownA1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // label2Z
      // 
      resources.ApplyResources(this.label2Z, "label2Z");
      this.label2Z.Name = "label2Z";
      // 
      // numericUpDownZ1
      // 
      resources.ApplyResources(this.numericUpDownZ1, "numericUpDownZ1");
      this.numericUpDownZ1.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
      this.numericUpDownZ1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownZ1.Name = "numericUpDownZ1";
      this.numericUpDownZ1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // label2Y
      // 
      resources.ApplyResources(this.label2Y, "label2Y");
      this.label2Y.Name = "label2Y";
      // 
      // numericUpDownY1
      // 
      resources.ApplyResources(this.numericUpDownY1, "numericUpDownY1");
      this.numericUpDownY1.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
      this.numericUpDownY1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownY1.Name = "numericUpDownY1";
      this.numericUpDownY1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // label2X
      // 
      resources.ApplyResources(this.label2X, "label2X");
      this.label2X.Name = "label2X";
      // 
      // numericUpDownX1
      // 
      resources.ApplyResources(this.numericUpDownX1, "numericUpDownX1");
      this.numericUpDownX1.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
      this.numericUpDownX1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownX1.Name = "numericUpDownX1";
      this.numericUpDownX1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // resizeButton
      // 
      resources.ApplyResources(this.resizeButton, "resizeButton");
      this.resizeButton.Name = "resizeButton";
      this.resizeButton.UseVisualStyleBackColor = true;
      this.resizeButton.Click += new System.EventHandler(this.resizeButton_Click);
      // 
      // rotatePanel
      // 
      this.rotatePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.rotatePanel.Controls.Add(this.domainUpDownAngle);
      this.rotatePanel.Controls.Add(this.domainUpDownAxis);
      this.rotatePanel.Controls.Add(this.rotateButton);
      this.rotatePanel.Controls.Add(this.labelAngle);
      this.rotatePanel.Controls.Add(this.labelAxis);
      resources.ApplyResources(this.rotatePanel, "rotatePanel");
      this.rotatePanel.Name = "rotatePanel";
      // 
      // domainUpDownAngle
      // 
      this.domainUpDownAngle.Items.Add(resources.GetString("domainUpDownAngle.Items"));
      this.domainUpDownAngle.Items.Add(resources.GetString("domainUpDownAngle.Items1"));
      this.domainUpDownAngle.Items.Add(resources.GetString("domainUpDownAngle.Items2"));
      this.domainUpDownAngle.Items.Add(resources.GetString("domainUpDownAngle.Items3"));
      this.domainUpDownAngle.Items.Add(resources.GetString("domainUpDownAngle.Items4"));
      resources.ApplyResources(this.domainUpDownAngle, "domainUpDownAngle");
      this.domainUpDownAngle.Name = "domainUpDownAngle";
      // 
      // domainUpDownAxis
      // 
      this.domainUpDownAxis.Items.Add(resources.GetString("domainUpDownAxis.Items"));
      this.domainUpDownAxis.Items.Add(resources.GetString("domainUpDownAxis.Items1"));
      this.domainUpDownAxis.Items.Add(resources.GetString("domainUpDownAxis.Items2"));
      this.domainUpDownAxis.Items.Add(resources.GetString("domainUpDownAxis.Items3"));
      resources.ApplyResources(this.domainUpDownAxis, "domainUpDownAxis");
      this.domainUpDownAxis.Name = "domainUpDownAxis";
      // 
      // rotateButton
      // 
      resources.ApplyResources(this.rotateButton, "rotateButton");
      this.rotateButton.Name = "rotateButton";
      this.rotateButton.UseVisualStyleBackColor = true;
      this.rotateButton.Click += new System.EventHandler(this.rotateButton_Click);
      // 
      // labelAngle
      // 
      resources.ApplyResources(this.labelAngle, "labelAngle");
      this.labelAngle.Name = "labelAngle";
      // 
      // labelAxis
      // 
      resources.ApplyResources(this.labelAxis, "labelAxis");
      this.labelAxis.Name = "labelAxis";
      // 
      // initialPanel
      // 
      this.initialPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.initialPanel.Controls.Add(this.label1A);
      this.initialPanel.Controls.Add(this.numericUpDownA);
      this.initialPanel.Controls.Add(this.renderButton);
      this.initialPanel.Controls.Add(this.label1Z);
      this.initialPanel.Controls.Add(this.numericUpDownZ);
      this.initialPanel.Controls.Add(this.label1Y);
      this.initialPanel.Controls.Add(this.numericUpDownY);
      this.initialPanel.Controls.Add(this.label1X);
      this.initialPanel.Controls.Add(this.numericUpDownX);
      resources.ApplyResources(this.initialPanel, "initialPanel");
      this.initialPanel.Name = "initialPanel";
      // 
      // label1A
      // 
      resources.ApplyResources(this.label1A, "label1A");
      this.label1A.Name = "label1A";
      // 
      // numericUpDownA
      // 
      resources.ApplyResources(this.numericUpDownA, "numericUpDownA");
      this.numericUpDownA.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
      this.numericUpDownA.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownA.Name = "numericUpDownA";
      this.numericUpDownA.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
      // 
      // renderButton
      // 
      resources.ApplyResources(this.renderButton, "renderButton");
      this.renderButton.Name = "renderButton";
      this.renderButton.UseVisualStyleBackColor = true;
      this.renderButton.Click += new System.EventHandler(this.renderButton_Click);
      // 
      // label1Z
      // 
      resources.ApplyResources(this.label1Z, "label1Z");
      this.label1Z.Name = "label1Z";
      // 
      // numericUpDownZ
      // 
      resources.ApplyResources(this.numericUpDownZ, "numericUpDownZ");
      this.numericUpDownZ.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
      this.numericUpDownZ.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownZ.Name = "numericUpDownZ";
      this.numericUpDownZ.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
      // 
      // label1Y
      // 
      resources.ApplyResources(this.label1Y, "label1Y");
      this.label1Y.Name = "label1Y";
      // 
      // numericUpDownY
      // 
      resources.ApplyResources(this.numericUpDownY, "numericUpDownY");
      this.numericUpDownY.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
      this.numericUpDownY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownY.Name = "numericUpDownY";
      this.numericUpDownY.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
      // 
      // label1X
      // 
      resources.ApplyResources(this.label1X, "label1X");
      this.label1X.Name = "label1X";
      // 
      // numericUpDownX
      // 
      resources.ApplyResources(this.numericUpDownX, "numericUpDownX");
      this.numericUpDownX.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
      this.numericUpDownX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownX.Name = "numericUpDownX";
      this.numericUpDownX.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
      // 
      // label10
      // 
      resources.ApplyResources(this.label10, "label10");
      this.label10.Name = "label10";
      // 
      // numericUpDownInc
      // 
      this.numericUpDownInc.DecimalPlaces = 1;
      this.numericUpDownInc.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      resources.ApplyResources(this.numericUpDownInc, "numericUpDownInc");
      this.numericUpDownInc.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.numericUpDownInc.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
      this.numericUpDownInc.Name = "numericUpDownInc";
      this.numericUpDownInc.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownInc.Enter += new System.EventHandler(this.numericUpDownInc_Enter);
      // 
      // label9
      // 
      resources.ApplyResources(this.label9, "label9");
      this.label9.Name = "label9";
      // 
      // numericUpDownStart
      // 
      this.numericUpDownStart.DecimalPlaces = 1;
      this.numericUpDownStart.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
      resources.ApplyResources(this.numericUpDownStart, "numericUpDownStart");
      this.numericUpDownStart.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.numericUpDownStart.Name = "numericUpDownStart";
      this.numericUpDownStart.Enter += new System.EventHandler(this.numericUpDownStart_Enter);
      // 
      // dimensionSelector
      // 
      this.dimensionSelector.Controls.Add(this.tabPage2D);
      this.dimensionSelector.Controls.Add(this.tabPage3D);
      this.dimensionSelector.Controls.Add(this.tabPage4D);
      resources.ApplyResources(this.dimensionSelector, "dimensionSelector");
      this.dimensionSelector.Name = "dimensionSelector";
      this.dimensionSelector.SelectedIndex = 0;
      this.dimensionSelector.Selected += new System.Windows.Forms.TabControlEventHandler(this.dimensionSelector_Selected);
      // 
      // tabPage2D
      // 
      resources.ApplyResources(this.tabPage2D, "tabPage2D");
      this.tabPage2D.Name = "tabPage2D";
      this.tabPage2D.UseVisualStyleBackColor = true;
      // 
      // tabPage3D
      // 
      resources.ApplyResources(this.tabPage3D, "tabPage3D");
      this.tabPage3D.Name = "tabPage3D";
      this.tabPage3D.UseVisualStyleBackColor = true;
      // 
      // tabPage4D
      // 
      resources.ApplyResources(this.tabPage4D, "tabPage4D");
      this.tabPage4D.Name = "tabPage4D";
      this.tabPage4D.UseVisualStyleBackColor = true;
      // 
      // panel2
      // 
      this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel2.Controls.Add(this.label9);
      this.panel2.Controls.Add(this.numericUpDownStart);
      this.panel2.Controls.Add(this.numericUpDownInc);
      this.panel2.Controls.Add(this.label10);
      resources.ApplyResources(this.panel2, "panel2");
      this.panel2.Name = "panel2";
      // 
      // panelTextInput
      // 
      this.panelTextInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelTextInput.Controls.Add(this.textBoxData);
      resources.ApplyResources(this.panelTextInput, "panelTextInput");
      this.panelTextInput.Name = "panelTextInput";
      // 
      // textBoxData
      // 
      resources.ApplyResources(this.textBoxData, "textBoxData");
      this.textBoxData.Name = "textBoxData";
      this.textBoxData.Enter += new System.EventHandler(this.textBoxData_Enter);
      this.textBoxData.Leave += new System.EventHandler(this.textBoxData_Leave);
      // 
      // radioButtonAutoFill
      // 
      resources.ApplyResources(this.radioButtonAutoFill, "radioButtonAutoFill");
      this.radioButtonAutoFill.Checked = true;
      this.radioButtonAutoFill.Name = "radioButtonAutoFill";
      this.radioButtonAutoFill.TabStop = true;
      // 
      // radioButtonManualFill
      // 
      resources.ApplyResources(this.radioButtonManualFill, "radioButtonManualFill");
      this.radioButtonManualFill.Name = "radioButtonManualFill";
      // 
      // radioButtonFileFill
      // 
      resources.ApplyResources(this.radioButtonFileFill, "radioButtonFileFill");
      this.radioButtonFileFill.Name = "radioButtonFileFill";
      // 
      // panel4
      // 
      this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel4.Controls.Add(this.lblFile);
      this.panel4.Controls.Add(this.buttonSelectFile);
      resources.ApplyResources(this.panel4, "panel4");
      this.panel4.Name = "panel4";
      // 
      // lblFile
      // 
      this.lblFile.AutoEllipsis = true;
      resources.ApplyResources(this.lblFile, "lblFile");
      this.lblFile.Name = "lblFile";
      // 
      // buttonSelectFile
      // 
      resources.ApplyResources(this.buttonSelectFile, "buttonSelectFile");
      this.buttonSelectFile.Name = "buttonSelectFile";
      this.buttonSelectFile.UseVisualStyleBackColor = true;
      this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
      // 
      // openFileDialog
      // 
      resources.ApplyResources(this.openFileDialog, "openFileDialog");
      // 
      // buttonSave
      // 
      resources.ApplyResources(this.buttonSave, "buttonSave");
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      // 
      // saveFileDialog
      // 
      resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
      // 
      // mainPanel
      // 
      this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(14)))));
      this.mainPanel.Controls.Add(this.array2D1);
      resources.ApplyResources(this.mainPanel, "mainPanel");
      this.mainPanel.Name = "mainPanel";
      // 
      // array2D1
      // 
      this.array2D1.CellHeight = 55;
      this.array2D1.CellPadding = 10;
      this.array2D1.CellSize = new System.Drawing.Size(80, 55);
      this.array2D1.CellWidth = 80;
      this.array2D1.Data = null;
      this.array2D1.Formatter = null;
      resources.ApplyResources(this.array2D1, "array2D1");
      this.array2D1.Name = "array2D1";
      this.array2D1.RenderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.array2D1.TabStop = false;
      // 
      // MainForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.mainPanel);
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.radioButtonManualFill);
      this.Controls.Add(this.panelTextInput);
      this.Controls.Add(this.radioButtonFileFill);
      this.Controls.Add(this.panel4);
      this.Controls.Add(this.radioButtonAutoFill);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.rotatePanel);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.resizePanel);
      this.Controls.Add(this.initialPanel);
      this.Controls.Add(this.dimensionSelector);
      this.Name = "MainForm";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCellHeight)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCellWidth)).EndInit();
      this.resizePanel.ResumeLayout(false);
      this.resizePanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownA1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX1)).EndInit();
      this.rotatePanel.ResumeLayout(false);
      this.rotatePanel.PerformLayout();
      this.initialPanel.ResumeLayout(false);
      this.initialPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownA)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInc)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart)).EndInit();
      this.dimensionSelector.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panelTextInput.ResumeLayout(false);
      this.panelTextInput.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.mainPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.array2D1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.NumericUpDown numericUpDownCellHeight;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.NumericUpDown numericUpDownCellWidth;
    private System.Windows.Forms.Panel resizePanel;
    private System.Windows.Forms.Label label2A;
    private System.Windows.Forms.NumericUpDown numericUpDownA1;
    private System.Windows.Forms.Label label2Z;
    private System.Windows.Forms.NumericUpDown numericUpDownZ1;
    private System.Windows.Forms.Label label2Y;
    private System.Windows.Forms.NumericUpDown numericUpDownY1;
    private System.Windows.Forms.Label label2X;
    private System.Windows.Forms.NumericUpDown numericUpDownX1;
    private System.Windows.Forms.Button resizeButton;
    private System.Windows.Forms.Panel rotatePanel;
    private System.Windows.Forms.DomainUpDown domainUpDownAngle;
    private System.Windows.Forms.DomainUpDown domainUpDownAxis;
    private System.Windows.Forms.Button rotateButton;
    private System.Windows.Forms.Label labelAngle;
    private System.Windows.Forms.Label labelAxis;
    private System.Windows.Forms.Panel initialPanel;
    private System.Windows.Forms.Label label1A;
    private System.Windows.Forms.NumericUpDown numericUpDownA;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.NumericUpDown numericUpDownInc;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.NumericUpDown numericUpDownStart;
    private System.Windows.Forms.Button renderButton;
    private System.Windows.Forms.Label label1Z;
    private System.Windows.Forms.NumericUpDown numericUpDownZ;
    private System.Windows.Forms.Label label1Y;
    private System.Windows.Forms.NumericUpDown numericUpDownY;
    private System.Windows.Forms.Label label1X;
    private System.Windows.Forms.NumericUpDown numericUpDownX;
    private System.Windows.Forms.TabControl dimensionSelector;
    private System.Windows.Forms.TabPage tabPage2D;
    private System.Windows.Forms.TabPage tabPage3D;
    private System.Windows.Forms.TabPage tabPage4D;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel panelTextInput;
    private System.Windows.Forms.TextBox textBoxData;
    private System.Windows.Forms.RadioButton radioButtonAutoFill;
    private System.Windows.Forms.RadioButton radioButtonManualFill;
    private System.Windows.Forms.RadioButton radioButtonFileFill;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Label lblFile;
    private System.Windows.Forms.Button buttonSelectFile;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.Panel mainPanel;
    private Array2D array2D1;
  }
}
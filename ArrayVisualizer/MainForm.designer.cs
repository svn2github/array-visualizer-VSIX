namespace ArrayVisualizer
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
      this.mainPanel = new System.Windows.Forms.Panel();
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
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.buttonSave = new System.Windows.Forms.Button();
      this.array2D1 = new ArrayVisualizer.Array2D();
      this.mainPanel.SuspendLayout();
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
      ((System.ComponentModel.ISupportInitialize)(this.array2D1)).BeginInit();
      this.SuspendLayout();
      // 
      // mainPanel
      // 
      this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.mainPanel.AutoScroll = true;
      this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(14)))));
      this.mainPanel.Controls.Add(this.array2D1);
      this.mainPanel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.mainPanel.Location = new System.Drawing.Point(196, 12);
      this.mainPanel.Name = "mainPanel";
      this.mainPanel.Size = new System.Drawing.Size(1108, 761);
      this.mainPanel.TabIndex = 5;
      // 
      // panel1
      // 
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add(this.label12);
      this.panel1.Controls.Add(this.numericUpDownCellHeight);
      this.panel1.Controls.Add(this.label13);
      this.panel1.Controls.Add(this.numericUpDownCellWidth);
      this.panel1.Location = new System.Drawing.Point(24, 665);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(150, 64);
      this.panel1.TabIndex = 9;
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(8, 42);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(58, 13);
      this.label12.TabIndex = 2;
      this.label12.Text = "Cell Height";
      // 
      // numericUpDownCellHeight
      // 
      this.numericUpDownCellHeight.Location = new System.Drawing.Point(83, 35);
      this.numericUpDownCellHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownCellHeight.Name = "numericUpDownCellHeight";
      this.numericUpDownCellHeight.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownCellHeight.TabIndex = 3;
      this.numericUpDownCellHeight.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(8, 11);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(55, 13);
      this.label13.TabIndex = 0;
      this.label13.Text = "Cell Width";
      // 
      // numericUpDownCellWidth
      // 
      this.numericUpDownCellWidth.Location = new System.Drawing.Point(83, 9);
      this.numericUpDownCellWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownCellWidth.Name = "numericUpDownCellWidth";
      this.numericUpDownCellWidth.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownCellWidth.TabIndex = 1;
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
      this.resizePanel.Enabled = false;
      this.resizePanel.Location = new System.Drawing.Point(24, 280);
      this.resizePanel.Name = "resizePanel";
      this.resizePanel.Size = new System.Drawing.Size(150, 137);
      this.resizePanel.TabIndex = 8;
      // 
      // label2A
      // 
      this.label2A.AutoSize = true;
      this.label2A.Location = new System.Drawing.Point(3, 83);
      this.label2A.Name = "label2A";
      this.label2A.Size = new System.Drawing.Size(17, 13);
      this.label2A.TabIndex = 6;
      this.label2A.Text = "A:";
      // 
      // numericUpDownA1
      // 
      this.numericUpDownA1.Location = new System.Drawing.Point(91, 81);
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
      this.numericUpDownA1.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownA1.TabIndex = 7;
      this.numericUpDownA1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // label2Z
      // 
      this.label2Z.AutoSize = true;
      this.label2Z.Location = new System.Drawing.Point(3, 57);
      this.label2Z.Name = "label2Z";
      this.label2Z.Size = new System.Drawing.Size(17, 13);
      this.label2Z.TabIndex = 4;
      this.label2Z.Text = "Z:";
      // 
      // numericUpDownZ1
      // 
      this.numericUpDownZ1.Location = new System.Drawing.Point(91, 55);
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
      this.numericUpDownZ1.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownZ1.TabIndex = 5;
      this.numericUpDownZ1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // label2Y
      // 
      this.label2Y.AutoSize = true;
      this.label2Y.Location = new System.Drawing.Point(3, 31);
      this.label2Y.Name = "label2Y";
      this.label2Y.Size = new System.Drawing.Size(17, 13);
      this.label2Y.TabIndex = 2;
      this.label2Y.Text = "Y:";
      // 
      // numericUpDownY1
      // 
      this.numericUpDownY1.Location = new System.Drawing.Point(91, 29);
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
      this.numericUpDownY1.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownY1.TabIndex = 3;
      this.numericUpDownY1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // label2X
      // 
      this.label2X.AutoSize = true;
      this.label2X.Location = new System.Drawing.Point(3, 5);
      this.label2X.Name = "label2X";
      this.label2X.Size = new System.Drawing.Size(17, 13);
      this.label2X.TabIndex = 0;
      this.label2X.Text = "X:";
      // 
      // numericUpDownX1
      // 
      this.numericUpDownX1.Location = new System.Drawing.Point(91, 3);
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
      this.numericUpDownX1.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownX1.TabIndex = 1;
      this.numericUpDownX1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // resizeButton
      // 
      this.resizeButton.Location = new System.Drawing.Point(3, 107);
      this.resizeButton.Name = "resizeButton";
      this.resizeButton.Size = new System.Drawing.Size(142, 23);
      this.resizeButton.TabIndex = 8;
      this.resizeButton.Text = "Resize";
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
      this.rotatePanel.Enabled = false;
      this.rotatePanel.Location = new System.Drawing.Point(24, 189);
      this.rotatePanel.Name = "rotatePanel";
      this.rotatePanel.Size = new System.Drawing.Size(150, 85);
      this.rotatePanel.TabIndex = 7;
      // 
      // domainUpDownAngle
      // 
      this.domainUpDownAngle.Items.Add("90");
      this.domainUpDownAngle.Items.Add("180");
      this.domainUpDownAngle.Items.Add("270");
      this.domainUpDownAngle.Items.Add("360");
      this.domainUpDownAngle.Items.Add("450");
      this.domainUpDownAngle.Location = new System.Drawing.Point(91, 3);
      this.domainUpDownAngle.Name = "domainUpDownAngle";
      this.domainUpDownAngle.Size = new System.Drawing.Size(54, 20);
      this.domainUpDownAngle.TabIndex = 3;
      this.domainUpDownAngle.Text = "90";
      // 
      // domainUpDownAxis
      // 
      this.domainUpDownAxis.Items.Add("X");
      this.domainUpDownAxis.Items.Add("Y");
      this.domainUpDownAxis.Items.Add("Z");
      this.domainUpDownAxis.Items.Add("A");
      this.domainUpDownAxis.Location = new System.Drawing.Point(91, 29);
      this.domainUpDownAxis.Name = "domainUpDownAxis";
      this.domainUpDownAxis.Size = new System.Drawing.Size(54, 20);
      this.domainUpDownAxis.TabIndex = 1;
      this.domainUpDownAxis.Text = "Z";
      // 
      // rotateButton
      // 
      this.rotateButton.Location = new System.Drawing.Point(3, 55);
      this.rotateButton.Name = "rotateButton";
      this.rotateButton.Size = new System.Drawing.Size(142, 23);
      this.rotateButton.TabIndex = 4;
      this.rotateButton.Text = "Rotate";
      this.rotateButton.UseVisualStyleBackColor = true;
      this.rotateButton.Click += new System.EventHandler(this.rotateButton_Click);
      // 
      // labelAngle
      // 
      this.labelAngle.AutoSize = true;
      this.labelAngle.Location = new System.Drawing.Point(3, 5);
      this.labelAngle.Name = "labelAngle";
      this.labelAngle.Size = new System.Drawing.Size(37, 13);
      this.labelAngle.TabIndex = 2;
      this.labelAngle.Text = "Angle:";
      // 
      // labelAxis
      // 
      this.labelAxis.AutoSize = true;
      this.labelAxis.Location = new System.Drawing.Point(3, 31);
      this.labelAxis.Name = "labelAxis";
      this.labelAxis.Size = new System.Drawing.Size(29, 13);
      this.labelAxis.TabIndex = 0;
      this.labelAxis.Text = "Axis:";
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
      this.initialPanel.Location = new System.Drawing.Point(24, 46);
      this.initialPanel.Name = "initialPanel";
      this.initialPanel.Size = new System.Drawing.Size(150, 137);
      this.initialPanel.TabIndex = 6;
      // 
      // label1A
      // 
      this.label1A.AutoSize = true;
      this.label1A.Location = new System.Drawing.Point(3, 83);
      this.label1A.Name = "label1A";
      this.label1A.Size = new System.Drawing.Size(17, 13);
      this.label1A.TabIndex = 6;
      this.label1A.Text = "A:";
      // 
      // numericUpDownA
      // 
      this.numericUpDownA.Location = new System.Drawing.Point(91, 81);
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
      this.numericUpDownA.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownA.TabIndex = 7;
      this.numericUpDownA.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
      // 
      // renderButton
      // 
      this.renderButton.Location = new System.Drawing.Point(3, 107);
      this.renderButton.Name = "renderButton";
      this.renderButton.Size = new System.Drawing.Size(142, 23);
      this.renderButton.TabIndex = 12;
      this.renderButton.Text = "Render";
      this.renderButton.UseVisualStyleBackColor = true;
      this.renderButton.Click += new System.EventHandler(this.renderButton_Click);
      // 
      // label1Z
      // 
      this.label1Z.AutoSize = true;
      this.label1Z.Location = new System.Drawing.Point(3, 57);
      this.label1Z.Name = "label1Z";
      this.label1Z.Size = new System.Drawing.Size(17, 13);
      this.label1Z.TabIndex = 4;
      this.label1Z.Text = "Z:";
      // 
      // numericUpDownZ
      // 
      this.numericUpDownZ.Location = new System.Drawing.Point(91, 55);
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
      this.numericUpDownZ.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownZ.TabIndex = 5;
      this.numericUpDownZ.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
      // 
      // label1Y
      // 
      this.label1Y.AutoSize = true;
      this.label1Y.Location = new System.Drawing.Point(3, 31);
      this.label1Y.Name = "label1Y";
      this.label1Y.Size = new System.Drawing.Size(17, 13);
      this.label1Y.TabIndex = 2;
      this.label1Y.Text = "Y:";
      // 
      // numericUpDownY
      // 
      this.numericUpDownY.Location = new System.Drawing.Point(91, 29);
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
      this.numericUpDownY.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownY.TabIndex = 3;
      this.numericUpDownY.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
      // 
      // label1X
      // 
      this.label1X.AutoSize = true;
      this.label1X.Location = new System.Drawing.Point(3, 5);
      this.label1X.Name = "label1X";
      this.label1X.Size = new System.Drawing.Size(17, 13);
      this.label1X.TabIndex = 0;
      this.label1X.Text = "X:";
      // 
      // numericUpDownX
      // 
      this.numericUpDownX.Location = new System.Drawing.Point(91, 3);
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
      this.numericUpDownX.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownX.TabIndex = 1;
      this.numericUpDownX.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(3, 43);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(32, 13);
      this.label10.TabIndex = 10;
      this.label10.Text = "Step:";
      // 
      // numericUpDownInc
      // 
      this.numericUpDownInc.DecimalPlaces = 1;
      this.numericUpDownInc.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.numericUpDownInc.Location = new System.Drawing.Point(91, 41);
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
      this.numericUpDownInc.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownInc.TabIndex = 11;
      this.numericUpDownInc.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownInc.Enter += new System.EventHandler(this.numericUpDownInc_Enter);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(3, 17);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(62, 13);
      this.label9.TabIndex = 8;
      this.label9.Text = "Start Value:";
      // 
      // numericUpDownStart
      // 
      this.numericUpDownStart.DecimalPlaces = 1;
      this.numericUpDownStart.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
      this.numericUpDownStart.Location = new System.Drawing.Point(91, 15);
      this.numericUpDownStart.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.numericUpDownStart.Name = "numericUpDownStart";
      this.numericUpDownStart.Size = new System.Drawing.Size(54, 20);
      this.numericUpDownStart.TabIndex = 9;
      this.numericUpDownStart.Enter += new System.EventHandler(this.numericUpDownStart_Enter);
      // 
      // dimensionSelector
      // 
      this.dimensionSelector.Controls.Add(this.tabPage2D);
      this.dimensionSelector.Controls.Add(this.tabPage3D);
      this.dimensionSelector.Controls.Add(this.tabPage4D);
      this.dimensionSelector.Location = new System.Drawing.Point(12, 12);
      this.dimensionSelector.Name = "dimensionSelector";
      this.dimensionSelector.SelectedIndex = 0;
      this.dimensionSelector.Size = new System.Drawing.Size(178, 727);
      this.dimensionSelector.TabIndex = 10;
      this.dimensionSelector.Selected += new System.Windows.Forms.TabControlEventHandler(this.dimensionSelector_Selected);
      // 
      // tabPage2D
      // 
      this.tabPage2D.Location = new System.Drawing.Point(4, 22);
      this.tabPage2D.Name = "tabPage2D";
      this.tabPage2D.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2D.Size = new System.Drawing.Size(170, 701);
      this.tabPage2D.TabIndex = 0;
      this.tabPage2D.Text = "2D";
      this.tabPage2D.UseVisualStyleBackColor = true;
      // 
      // tabPage3D
      // 
      this.tabPage3D.Location = new System.Drawing.Point(4, 22);
      this.tabPage3D.Name = "tabPage3D";
      this.tabPage3D.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage3D.Size = new System.Drawing.Size(170, 701);
      this.tabPage3D.TabIndex = 1;
      this.tabPage3D.Text = "3D";
      this.tabPage3D.UseVisualStyleBackColor = true;
      // 
      // tabPage4D
      // 
      this.tabPage4D.Location = new System.Drawing.Point(4, 22);
      this.tabPage4D.Name = "tabPage4D";
      this.tabPage4D.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage4D.Size = new System.Drawing.Size(170, 701);
      this.tabPage4D.TabIndex = 2;
      this.tabPage4D.Text = "4D";
      this.tabPage4D.UseVisualStyleBackColor = true;
      // 
      // panel2
      // 
      this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel2.Controls.Add(this.label9);
      this.panel2.Controls.Add(this.numericUpDownStart);
      this.panel2.Controls.Add(this.numericUpDownInc);
      this.panel2.Controls.Add(this.label10);
      this.panel2.Location = new System.Drawing.Point(24, 431);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(150, 69);
      this.panel2.TabIndex = 9;
      // 
      // panelTextInput
      // 
      this.panelTextInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelTextInput.Controls.Add(this.textBoxData);
      this.panelTextInput.Location = new System.Drawing.Point(24, 514);
      this.panelTextInput.Name = "panelTextInput";
      this.panelTextInput.Size = new System.Drawing.Size(150, 55);
      this.panelTextInput.TabIndex = 12;
      // 
      // textBoxData
      // 
      this.textBoxData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxData.Location = new System.Drawing.Point(3, 14);
      this.textBoxData.Multiline = true;
      this.textBoxData.Name = "textBoxData";
      this.textBoxData.Size = new System.Drawing.Size(142, 36);
      this.textBoxData.TabIndex = 0;
      this.textBoxData.Enter += new System.EventHandler(this.textBoxData_Enter);
      this.textBoxData.Leave += new System.EventHandler(this.textBoxData_Leave);
      // 
      // radioButtonAutoFill
      // 
      this.radioButtonAutoFill.AutoSize = true;
      this.radioButtonAutoFill.Checked = true;
      this.radioButtonAutoFill.Location = new System.Drawing.Point(31, 423);
      this.radioButtonAutoFill.Name = "radioButtonAutoFill";
      this.radioButtonAutoFill.Size = new System.Drawing.Size(62, 17);
      this.radioButtonAutoFill.TabIndex = 14;
      this.radioButtonAutoFill.TabStop = true;
      this.radioButtonAutoFill.Text = "Auto Fill";
      // 
      // radioButtonManualFill
      // 
      this.radioButtonManualFill.AutoSize = true;
      this.radioButtonManualFill.Location = new System.Drawing.Point(31, 506);
      this.radioButtonManualFill.Name = "radioButtonManualFill";
      this.radioButtonManualFill.Size = new System.Drawing.Size(87, 17);
      this.radioButtonManualFill.TabIndex = 15;
      this.radioButtonManualFill.Text = "Manual Input";
      // 
      // radioButtonFileFill
      // 
      this.radioButtonFileFill.AutoSize = true;
      this.radioButtonFileFill.Location = new System.Drawing.Point(31, 575);
      this.radioButtonFileFill.Name = "radioButtonFileFill";
      this.radioButtonFileFill.Size = new System.Drawing.Size(94, 17);
      this.radioButtonFileFill.TabIndex = 17;
      this.radioButtonFileFill.Text = "Load From File";
      // 
      // panel4
      // 
      this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel4.Controls.Add(this.lblFile);
      this.panel4.Controls.Add(this.buttonSelectFile);
      this.panel4.Location = new System.Drawing.Point(24, 583);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(150, 76);
      this.panel4.TabIndex = 16;
      // 
      // lblFile
      // 
      this.lblFile.AutoEllipsis = true;
      this.lblFile.Location = new System.Drawing.Point(3, 21);
      this.lblFile.Name = "lblFile";
      this.lblFile.Size = new System.Drawing.Size(142, 24);
      this.lblFile.TabIndex = 10;
      // 
      // buttonSelectFile
      // 
      this.buttonSelectFile.Location = new System.Drawing.Point(3, 48);
      this.buttonSelectFile.Name = "buttonSelectFile";
      this.buttonSelectFile.Size = new System.Drawing.Size(142, 23);
      this.buttonSelectFile.TabIndex = 9;
      this.buttonSelectFile.Text = "Select File";
      this.buttonSelectFile.UseVisualStyleBackColor = true;
      this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
      // 
      // openFileDialog
      // 
      this.openFileDialog.Filter = "Text Files|*.txt|Csv Files|*.csv|All Files|*.*";
      this.openFileDialog.Title = "Select Input File";
      // 
      // saveFileDialog
      // 
      this.saveFileDialog.Filter = "Text Files|*.txt|Comma Delimited Files|*.csv|All Files|*.*";
      this.saveFileDialog.Title = "Save Array File";
      // 
      // buttonSave
      // 
      this.buttonSave.Enabled = false;
      this.buttonSave.Location = new System.Drawing.Point(12, 750);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(91, 23);
      this.buttonSave.TabIndex = 18;
      this.buttonSave.Text = "Save";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      // 
      // array2D1
      // 
      this.array2D1.CellHeight = 80;
      this.array2D1.CellPadding = 10;
      this.array2D1.CellSize = new System.Drawing.Size(80, 80);
      this.array2D1.CellWidth = 80;
      this.array2D1.Data = null;
      this.array2D1.Formatter = null;
      this.array2D1.Location = new System.Drawing.Point(3, 3);
      this.array2D1.Name = "array2D1";
      this.array2D1.RederFont = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.array2D1.Size = new System.Drawing.Size(427, 289);
      this.array2D1.TabIndex = 0;
      this.array2D1.TabStop = false;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1316, 785);
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
      this.Controls.Add(this.mainPanel);
      this.Name = "MainForm";
      this.Text = "Array Visualizer";
      this.mainPanel.ResumeLayout(false);
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
      ((System.ComponentModel.ISupportInitialize)(this.array2D1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel mainPanel;
    private Array2D array2D1;
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
  }
}
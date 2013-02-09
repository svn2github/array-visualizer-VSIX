using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using LinqLib.Array;
using LinqLib.Sequence;

using WinFormsArrayVisualizer.Properties;

using WinFormsArrayVisualizerControls;

namespace WinFormsArrayVisualizer
{
  public partial class MainForm : Form
  {
    #region Fields

    private ArrayXD arrCtl;
    private int dims;

    #endregion

    #region Constructors and Destructors

    public MainForm()
    {
      this.InitializeComponent();
      this.SetControls();
    }

    #endregion

    #region Methods

    private static string[] GetItems(string list)
    {
      list = list.Replace(" ", string.Empty);
      list = list.Replace('\r', ',');
      list = list.Replace('\n', ',');
      while (list.IndexOf(",,", StringComparison.CurrentCulture) != -1)
        list = list.Replace(",,", ",");

      return list.Split(new[] { ',' });
    }

    private Array Get2DArray(int x, int y)
    {
      if (this.radioButtonAutoFill.Checked)
        return Enumerator.Generate(this.numericUpDownStart.Value, this.numericUpDownInc.Value, x * y).Select(V => (double)V).ToArray(y, x);
      else if (this.radioButtonManualFill.Checked)
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(Resources.InvalidInputFormat, ex);
        }
      else
        // file
        try
        {
          string[] items = this.GetFileItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(y, x);
        }
        catch (FormatException ex)
        {
          throw new FormatException(Resources.InvalidFileContent, ex);
        }
        catch
        {
          throw;
        }
    }

    private Array Get3DArray(int x, int y, int z)
    {
      if (this.radioButtonAutoFill.Checked)
        return Enumerator.Generate(this.numericUpDownStart.Value, this.numericUpDownInc.Value, x * y * z).Select(V => (double)V).ToArray(z, y, x);
      else if (this.radioButtonManualFill.Checked)
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(z, y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(Resources.InvalidInputFormat, ex);
        }
      else
        // file
        try
        {
          string[] items = this.GetFileItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(z, y, x);
        }
        catch (FormatException ex)
        {
          throw new FormatException(Resources.InvalidFileContent, ex);
        }
        catch
        {
          throw;
        }
    }

    private Array Get4DArray(int x, int y, int z, int a)
    {
      if (this.radioButtonAutoFill.Checked)
        return Enumerator.Generate(this.numericUpDownStart.Value, this.numericUpDownInc.Value, x * y * z * a).Select(V => (double)V).ToArray(a, z, y, x);
      else if (this.radioButtonManualFill.Checked)
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(a, z, y, x);
        }
        catch
        {
          throw new FormatException(Resources.InvalidInputFormat);
        }
      else
        // file
        try
        {
          string[] items = this.GetFileItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat))
                      .ToArray(a, z, y, x);
        }
        catch (FormatException)
        {
          throw new FormatException(Resources.InvalidFileContent);
        }
        catch
        {
          throw;
        }
    }

    private Array GetData(int dimensions)
    {
      var x = (int)this.numericUpDownX.Value;
      var y = (int)this.numericUpDownY.Value;
      var z = (int)this.numericUpDownZ.Value;
      var a = (int)this.numericUpDownA.Value;

      switch (dimensions)
      {
        case 2:
          return this.Get2DArray(x, y);
        case 3:
          return this.Get3DArray(x, y, z);
        case 4:
          return this.Get4DArray(x, y, z, a);
        default:
          throw new ArrayTypeMismatchException(string.Format(Thread.CurrentThread.CurrentUICulture.NumberFormat, Resources.ArrayNotValidDimsException, dimensions));
      }
    }

    private string[] GetFileItems()
    {
      var name = (string)this.lblFile.Tag;
      string list = string.Join(",", File.ReadAllLines(name));
      return GetItems(list);
    }

    private string[] GetManualItems()
    {
      return GetItems(this.textBoxData.Text);
    }

    private void SaveToFile(string fileName)
    {
      double[] values = this.arrCtl.Data.AsEnumerable<double>().ToArray();
      string list = string.Join(",", values);

      File.WriteAllText(fileName, list);
    }

    private void SetControls()
    {
      int temp = this.dimensionSelector.SelectedIndex + 2;
      if (temp != this.dims)
      {
        this.dims = temp;

        this.domainUpDownAngle.Items.Clear();
        this.domainUpDownAxis.Items.Clear();

        this.domainUpDownAngle.Items.Add("90");
        this.domainUpDownAngle.Items.Add("180");
        this.domainUpDownAngle.Items.Add("270");

        this.domainUpDownAxis.Items.Add("X");
        this.domainUpDownAxis.Items.Add("Y");
        this.domainUpDownAxis.Items.Add("Z");

        if (this.dims >= 3)
        {
          this.numericUpDownZ.Visible = true;
          this.label1Z.Visible = true;

          this.domainUpDownAxis.Visible = true;
          this.labelAxis.Visible = true;

          this.numericUpDownZ1.Visible = true;
          this.label2Z.Visible = true;
        }
        else
        {
          this.numericUpDownZ.Visible = false;
          this.label1Z.Visible = false;

          this.domainUpDownAxis.Visible = false;
          this.labelAxis.Visible = false;

          this.numericUpDownZ1.Visible = false;
          this.label2Z.Visible = false;
        }

        if (this.dims >= 4)
        {
          this.domainUpDownAngle.Items.Add("360");
          this.domainUpDownAngle.Items.Add("450");

          this.domainUpDownAxis.Items.Add("A");

          this.numericUpDownA.Visible = true;
          this.label1A.Visible = true;

          this.numericUpDownA1.Visible = true;
          this.label2A.Visible = true;
        }
        else
        {
          this.numericUpDownA.Visible = false;
          this.label1A.Visible = false;

          this.numericUpDownA1.Visible = false;
          this.label2A.Visible = false;
        }

        this.rotatePanel.Enabled = false;
        this.resizePanel.Enabled = false;
      }
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
        this.SaveToFile(this.saveFileDialog.FileName);
    }

    private void buttonSelectFile_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog.ShowDialog() == DialogResult.OK)
      {
        this.radioButtonFileFill.Checked = true;
        string name = this.openFileDialog.FileName;
        this.lblFile.Tag = name;
        this.lblFile.Text = Path.GetFileName(name);
        this.toolTip.SetToolTip(this.lblFile, name);
      }
    }

    private void dimensionSelector_Selected(object sender, TabControlEventArgs e)
    {
      this.SetControls();
    }

    private void numericUpDownInc_Enter(object sender, EventArgs e)
    {
      this.radioButtonAutoFill.Checked = true;
    }

    private void numericUpDownStart_Enter(object sender, EventArgs e)
    {
      this.radioButtonAutoFill.Checked = true;
    }

    private void renderButton_Click(object sender, EventArgs e)
    {
      try
      {
        this.mainPanel.Controls.Clear();

        switch (this.dims)
        {
          case 2:
            this.arrCtl = new Array2D();
            break;
          case 3:
            this.arrCtl = new Array3D();
            break;
          case 4:
            this.arrCtl = new Array4D();
            break;
          default:
            throw new ArrayTypeMismatchException(string.Format(Thread.CurrentThread.CurrentUICulture.NumberFormat, Resources.ArrayNotValidDimsException, this.dims));
        }

        this.mainPanel.Controls.Add(this.arrCtl);

        this.arrCtl.CellWidth = (int)this.numericUpDownCellWidth.Value;
        this.arrCtl.CellHeight = (int)this.numericUpDownCellHeight.Value;
        this.arrCtl.Formatter = "0.##";

        this.arrCtl.Data = this.GetData(this.dims);

        this.rotatePanel.Enabled = true;
        this.resizePanel.Enabled = true;
        this.buttonSave.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void resizeButton_Click(object sender, EventArgs e)
    {
      var x = (int)this.numericUpDownX1.Value;
      var y = (int)this.numericUpDownY1.Value;
      var z = (int)this.numericUpDownZ1.Value;
      var a = (int)this.numericUpDownA1.Value;

      switch (this.dims)
      {
        case 2:
          this.arrCtl.Data = ((double[,])this.arrCtl.Data).Resize(y, x);
          break;
        case 3:
          this.arrCtl.Data = ((double[, ,])this.arrCtl.Data).Resize(z, y, x);
          break;
        case 4:
          this.arrCtl.Data = ((double[, , ,])this.arrCtl.Data).Resize(a, z, y, x);
          break;
        default:
          throw new ArrayTypeMismatchException();
      }
    }

    private void rotateButton_Click(object sender, EventArgs e)
    {
      var r = RotateAxis.RotateNone;
      int angle = int.Parse(this.domainUpDownAngle.Text, Thread.CurrentThread.CurrentUICulture.NumberFormat);

      switch (this.domainUpDownAxis.Text)
      {
        case "X":
          r = RotateAxis.RotateX;
          break;
        case "Y":
          r = RotateAxis.RotateY;
          break;
        case "Z":
          r = RotateAxis.RotateZ;
          break;
        case "A":
          r = RotateAxis.RotateA;
          break;
      }

      switch (this.dims)
      {
        case 2:
          this.arrCtl.Data = ((double[,])this.arrCtl.Data).Rotate(angle);
          break;
        case 3:
          this.arrCtl.Data = ((double[, ,])this.arrCtl.Data).Rotate(r, angle);
          break;
        case 4:
          this.arrCtl.Data = ((double[, , ,])this.arrCtl.Data).Rotate(r, angle);
          break;
      }
    }

    private void textBoxData_Enter(object sender, EventArgs e)
    {
      this.radioButtonManualFill.Checked = true;
      this.panelTextInput.Width = this.mainPanel.Width + this.panelTextInput.Width;
      this.panelTextInput.Height = 218;
    }

    private void textBoxData_Leave(object sender, EventArgs e)
    {
      this.panelTextInput.Width = 150;
      this.panelTextInput.Height = 55;
    }

    #endregion
  }
}
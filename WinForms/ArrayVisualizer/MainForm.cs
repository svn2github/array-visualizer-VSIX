// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="">
//   
// </copyright>
// <summary>
//   The main form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinFormsArrayVisualizer
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Threading;
  using System.Windows.Forms;

  using LinqLib.Array;
  using LinqLib.Sequence;

  using WinFormsArrayVisualizer.Properties;

  using WinFormsArrayVisualizerControls;

  /// <summary>
  /// The main form.
  /// </summary>
  public partial class MainForm : Form
  {
    #region Fields

    /// <summary>
    /// The arr ctl.
    /// </summary>
    private ArrayXD arrCtl;

    /// <summary>
    /// The dims.
    /// </summary>
    private int dims;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
      this.InitializeComponent();
      this.SetControls();
    }

    #endregion

    #region Methods

    /// <summary>
    /// The get items.
    /// </summary>
    /// <param name="list">
    /// The list.
    /// </param>
    /// <returns>
    /// The <see cref="string[]"/>.
    /// </returns>
    private static string[] GetItems(string list)
    {
      list = list.Replace(" ", string.Empty);
      list = list.Replace('\r', ',');
      list = list.Replace('\n', ',');
      while (list.IndexOf(",,", StringComparison.CurrentCulture) != -1)
      {
        list = list.Replace(",,", ",");
      }

      return list.Split(new[] { ',' });
    }

    /// <summary>
    /// The get 2 d array.
    /// </summary>
    /// <param name="x">
    /// The x.
    /// </param>
    /// <param name="y">
    /// The y.
    /// </param>
    /// <returns>
    /// The <see cref="Array"/>.
    /// </returns>
    /// <exception cref="FormatException">
    /// </exception>
    private Array Get2DArray(int x, int y)
    {
      if (this.radioButtonAutoFill.Checked)
      {
        return
          Enumerator.Generate(this.numericUpDownStart.Value, this.numericUpDownInc.Value, x * y)
                    .Select(V => (double)V)
                    .ToArray(y, x);
      }
      else if (this.radioButtonManualFill.Checked)
      {
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(Resources.InvalidInputFormat, ex);
        }
      }
      else
      {
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
    }

    /// <summary>
    /// The get 3 d array.
    /// </summary>
    /// <param name="x">
    /// The x.
    /// </param>
    /// <param name="y">
    /// The y.
    /// </param>
    /// <param name="z">
    /// The z.
    /// </param>
    /// <returns>
    /// The <see cref="Array"/>.
    /// </returns>
    /// <exception cref="FormatException">
    /// </exception>
    private Array Get3DArray(int x, int y, int z)
    {
      if (this.radioButtonAutoFill.Checked)
      {
        return
          Enumerator.Generate(this.numericUpDownStart.Value, this.numericUpDownInc.Value, x * y * z)
                    .Select(V => (double)V)
                    .ToArray(z, y, x);
      }
      else if (this.radioButtonManualFill.Checked)
      {
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(z, y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(Resources.InvalidInputFormat, ex);
        }
      }
      else
      {
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
    }

    /// <summary>
    /// The get 4 d array.
    /// </summary>
    /// <param name="x">
    /// The x.
    /// </param>
    /// <param name="y">
    /// The y.
    /// </param>
    /// <param name="z">
    /// The z.
    /// </param>
    /// <param name="a">
    /// The a.
    /// </param>
    /// <returns>
    /// The <see cref="Array"/>.
    /// </returns>
    /// <exception cref="FormatException">
    /// </exception>
    private Array Get4DArray(int x, int y, int z, int a)
    {
      if (this.radioButtonAutoFill.Checked)
      {
        return
          Enumerator.Generate(this.numericUpDownStart.Value, this.numericUpDownInc.Value, x * y * z * a)
                    .Select(V => (double)V)
                    .ToArray(a, z, y, x);
      }
      else if (this.radioButtonManualFill.Checked)
      {
        try
        {
          string[] items = this.GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat))
                      .ToArray(a, z, y, x);
        }
        catch
        {
          throw new FormatException(Resources.InvalidInputFormat);
        }
      }
      else
      {
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
    }

    /// <summary>
    /// The get data.
    /// </summary>
    /// <param name="dimensions">
    /// The dimensions.
    /// </param>
    /// <returns>
    /// The <see cref="Array"/>.
    /// </returns>
    /// <exception cref="ArrayTypeMismatchException">
    /// </exception>
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
          throw new ArrayTypeMismatchException(
            string.Format(
              Thread.CurrentThread.CurrentUICulture.NumberFormat, Resources.ArrayNotValidDimsException, dimensions));
      }
    }

    /// <summary>
    /// The get file items.
    /// </summary>
    /// <returns>
    /// The <see cref="string[]"/>.
    /// </returns>
    private string[] GetFileItems()
    {
      var name = (string)this.lblFile.Tag;
      string list = string.Join(",", File.ReadAllLines(name));
      return GetItems(list);
    }

    /// <summary>
    /// The get manual items.
    /// </summary>
    /// <returns>
    /// The <see cref="string[]"/>.
    /// </returns>
    private string[] GetManualItems()
    {
      return GetItems(this.textBoxData.Text);
    }

    /// <summary>
    /// The save to file.
    /// </summary>
    /// <param name="fileName">
    /// The file name.
    /// </param>
    private void SaveToFile(string fileName)
    {
      double[] values = this.arrCtl.Data.AsEnumerable<double>().ToArray();
      string list = string.Join(",", values);

      File.WriteAllText(fileName, list);
    }

    /// <summary>
    /// The set controls.
    /// </summary>
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

    /// <summary>
    /// The button save_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void buttonSave_Click(object sender, EventArgs e)
    {
      if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        this.SaveToFile(this.saveFileDialog.FileName);
      }
    }

    /// <summary>
    /// The button select file_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
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

    /// <summary>
    /// The dimension selector_ selected.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void dimensionSelector_Selected(object sender, TabControlEventArgs e)
    {
      this.SetControls();
    }

    /// <summary>
    /// The numeric up down inc_ enter.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void numericUpDownInc_Enter(object sender, EventArgs e)
    {
      this.radioButtonAutoFill.Checked = true;
    }

    /// <summary>
    /// The numeric up down start_ enter.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void numericUpDownStart_Enter(object sender, EventArgs e)
    {
      this.radioButtonAutoFill.Checked = true;
    }

    /// <summary>
    /// The render button_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    /// <exception cref="ArrayTypeMismatchException">
    /// </exception>
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
            throw new ArrayTypeMismatchException(
              string.Format(
                Thread.CurrentThread.CurrentUICulture.NumberFormat, Resources.ArrayNotValidDimsException, this.dims));
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

    /// <summary>
    /// The resize button_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    /// <exception cref="ArrayTypeMismatchException">
    /// </exception>
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
          this.arrCtl.Data = ((double[,,])this.arrCtl.Data).Resize(z, y, x);
          break;
        case 4:
          this.arrCtl.Data = ((double[,,,])this.arrCtl.Data).Resize(a, z, y, x);
          break;
        default:
          throw new ArrayTypeMismatchException();
      }
    }

    /// <summary>
    /// The rotate button_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
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
          this.arrCtl.Data = ((double[,,])this.arrCtl.Data).Rotate(r, angle);
          break;
        case 4:
          this.arrCtl.Data = ((double[,,,])this.arrCtl.Data).Rotate(r, angle);
          break;
      }
    }

    /// <summary>
    /// The text box data_ enter.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void textBoxData_Enter(object sender, EventArgs e)
    {
      this.radioButtonManualFill.Checked = true;
      this.panelTextInput.Width = this.mainPanel.Width + this.panelTextInput.Width;
      this.panelTextInput.Height = 218;
    }

    /// <summary>
    /// The text box data_ leave.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void textBoxData_Leave(object sender, EventArgs e)
    {
      this.panelTextInput.Width = 150;
      this.panelTextInput.Height = 55;
    }

    #endregion
  }
}
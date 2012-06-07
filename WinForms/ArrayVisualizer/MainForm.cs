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
    ArrayXD arrCtl;
    int dims;

    public MainForm()
    {
      InitializeComponent();
      SetControls();
    }

    
    private void dimensionSelector_Selected(object sender, TabControlEventArgs e)
    {
      SetControls();
    }

    private void rotateButton_Click(object sender, EventArgs e)
    {
      RotateAxis r = RotateAxis.RotateNone;
      int angle = int.Parse(domainUpDownAngle.Text, Thread.CurrentThread.CurrentUICulture.NumberFormat);

      switch (domainUpDownAxis.Text)
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

      switch (dims)
      {
        case 2:
          arrCtl.Data = ((double[,])arrCtl.Data).Rotate(angle);
          break;
        case 3:
          arrCtl.Data = ((double[, ,])arrCtl.Data).Rotate(r, angle);
          break;
        case 4:
          arrCtl.Data = ((double[, , ,])arrCtl.Data).Rotate(r, angle);
          break;
      }

    }

    private void resizeButton_Click(object sender, EventArgs e)
    {
      int x = (int)numericUpDownX1.Value;
      int y = (int)numericUpDownY1.Value;
      int z = (int)numericUpDownZ1.Value;
      int a = (int)numericUpDownA1.Value;

      switch (dims)
      {
        case 2:
          arrCtl.Data = ((double[,])arrCtl.Data).Resize(y, x);
          break;
        case 3:
          arrCtl.Data = ((double[, ,])arrCtl.Data).Resize(z, y, x);
          break;
        case 4:
          arrCtl.Data = ((double[, , ,])arrCtl.Data).Resize(a, z, y, x);
          break;
        default:
          throw new ArrayTypeMismatchException();
      }
    }

    private void textBoxData_Enter(object sender, EventArgs e)
    {
      radioButtonManualFill.Checked = true;
      panelTextInput.Width = mainPanel.Width + panelTextInput.Width;
      panelTextInput.Height = 218;
    }

    private void textBoxData_Leave(object sender, EventArgs e)
    {
      panelTextInput.Width = 150;
      panelTextInput.Height = 55;
    }

    private void renderButton_Click(object sender, EventArgs e)
    {
      try
      {
        mainPanel.Controls.Clear();

        switch (dims)
        {
          case 2:
            arrCtl = new Array2D();
            break;
          case 3:
            arrCtl = new Array3D();
            break;
          case 4:
            arrCtl = new Array4D();
            break;
          default:
            throw new ArrayTypeMismatchException(string.Format(Thread.CurrentThread.CurrentUICulture.NumberFormat, Resources.ArrayNotValidDimsException, dims));
        }
        mainPanel.Controls.Add(arrCtl);

        arrCtl.CellWidth = (int)numericUpDownCellWidth.Value;
        arrCtl.CellHeight = (int)numericUpDownCellHeight.Value;
        arrCtl.Formatter= "0.##";

        arrCtl.Data = GetData(dims);

        rotatePanel.Enabled = true;
        resizePanel.Enabled = true;
        buttonSave.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void numericUpDownStart_Enter(object sender, EventArgs e)
    {
      radioButtonAutoFill.Checked = true;
    }

    private void numericUpDownInc_Enter(object sender, EventArgs e)
    {
      radioButtonAutoFill.Checked = true;
    }

    private void buttonSelectFile_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        radioButtonFileFill.Checked = true;
        string name = openFileDialog.FileName;
        lblFile.Tag = name;
        lblFile.Text = Path.GetFileName(name);
        this.toolTip.SetToolTip(this.lblFile, name);
      }
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        SaveToFile(saveFileDialog.FileName);
    }
    

    private void SetControls()
    {
      int temp = dimensionSelector.SelectedIndex + 2;
      if (temp != dims)
      {
        dims = temp;

        this.domainUpDownAngle.Items.Clear();
        this.domainUpDownAxis.Items.Clear();

        this.domainUpDownAngle.Items.Add("90");
        this.domainUpDownAngle.Items.Add("180");
        this.domainUpDownAngle.Items.Add("270");

        this.domainUpDownAxis.Items.Add("X");
        this.domainUpDownAxis.Items.Add("Y");
        this.domainUpDownAxis.Items.Add("Z");


        if (dims >= 3)
        {
          numericUpDownZ.Visible = true;
          label1Z.Visible = true;

          domainUpDownAxis.Visible = true;
          labelAxis.Visible = true;

          numericUpDownZ1.Visible = true;
          label2Z.Visible = true;
        }
        else
        {
          numericUpDownZ.Visible = false;
          label1Z.Visible = false;

          domainUpDownAxis.Visible = false;
          labelAxis.Visible = false;

          numericUpDownZ1.Visible = false;
          label2Z.Visible = false;
        }

        if (dims >= 4)
        {
          this.domainUpDownAngle.Items.Add("360");
          this.domainUpDownAngle.Items.Add("450");

          this.domainUpDownAxis.Items.Add("A");

          numericUpDownA.Visible = true;
          label1A.Visible = true;

          numericUpDownA1.Visible = true;
          label2A.Visible = true;
        }
        else
        {
          numericUpDownA.Visible = false;
          label1A.Visible = false;

          numericUpDownA1.Visible = false;
          label2A.Visible = false;
        }

        rotatePanel.Enabled = false;
        resizePanel.Enabled = false;
      }
    }

    private Array GetData(int dimensions)
    {
      int x = (int)numericUpDownX.Value;
      int y = (int)numericUpDownY.Value;
      int z = (int)numericUpDownZ.Value;
      int a = (int)numericUpDownA.Value;

      switch (dimensions)
      {
        case 2:
          return Get2DArray(x, y);
        case 3:
          return Get3DArray(x, y, z);
        case 4:
          return Get4DArray(x, y, z, a);
        default:
          throw new ArrayTypeMismatchException(string.Format(Thread.CurrentThread.CurrentUICulture.NumberFormat, Resources.ArrayNotValidDimsException, dimensions));
      }
    }

    private Array Get4DArray(int x, int y, int z, int a)
    {
      if (radioButtonAutoFill.Checked)
        return Enumerator.Generate(numericUpDownStart.Value, numericUpDownInc.Value, x * y * z * a).Select(V => (double)V).ToArray(a, z, y, x);
      else if (radioButtonManualFill.Checked)
      {
        try
        {
          string[] items = GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(a, z, y, x);
        }
        catch
        {
          throw new FormatException(Resources.InvalidInputFormat);
        }
      }
      else //file
      {
        try
        {
          string[] items = GetFileItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(a, z, y, x);
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

    private Array Get3DArray(int x, int y, int z)
    {
      if (radioButtonAutoFill.Checked)
        return Enumerator.Generate(numericUpDownStart.Value, numericUpDownInc.Value, x * y * z).Select(V => (double)V).ToArray(z, y, x);
      else if (radioButtonManualFill.Checked)
      {
        try
        {
          string[] items = GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(z, y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(Resources.InvalidInputFormat, ex);
        }
      }
      else //file
      {
        try
        {
          string[] items = GetFileItems();
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

    private Array Get2DArray(int x, int y)
    {
      if (radioButtonAutoFill.Checked)
        return Enumerator.Generate(numericUpDownStart.Value, numericUpDownInc.Value, x * y).Select(V => (double)V).ToArray(y, x);
      else if (radioButtonManualFill.Checked)
      {
        try
        {
          string[] items = GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(Resources.InvalidInputFormat, ex);
        }
      }
      else //file
      {
        try
        {
          string[] items = GetFileItems();
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

    private string[] GetFileItems()
    {
      string name = (string)lblFile.Tag;
      string list = string.Join(",", File.ReadAllLines(name));
      return GetItems(list);
    }

    private string[] GetManualItems()
    {
      return GetItems(textBoxData.Text);
    }

    private static string[] GetItems(string list)
    {
      list = list.Replace(" ", "");
      list = list.Replace('\r', ',');
      list = list.Replace('\n', ',');
      while (list.IndexOf(",,", StringComparison.CurrentCulture) != -1)
        list = list.Replace(",,", ",");

      return list.Split(new char[] { ',' });
    }

    private void SaveToFile(string fileName)
    {
      double[] values = arrCtl.Data.AsEnumerable<double>().ToArray();
      string list = string.Join(",", values);

      File.WriteAllText(fileName, list);
    }
  }
}

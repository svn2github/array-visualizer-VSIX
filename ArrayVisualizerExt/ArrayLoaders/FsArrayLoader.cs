using System.Collections.Generic;
using System.Linq;

namespace ArrayVisualizerExt.ArrayLoaders
{
  internal class FsArrayLoader : IArrayLoader
  {
    #region IArrayLoader Members

    public void ArraysLoader(Dictionary<string, EnvDTE.Expression> arrayExpressions, string prefix, EnvDTE.Expression expression)
    {
      if (expression.DataMembers.Count == 0)
        return;

      string expType = Helper.RemoveBrackets(expression.Type);
      if (Helper.IsExpressionFsArrayType(expType))
      {
        string item = prefix + expression.Name + " - " + expression.Value;
        arrayExpressions.Add(item, expression);
      }
    }

    public int[] DimensionsLoader(EnvDTE.Expression expression)
    {
      string dims = expression.Value;
      dims = dims.Substring(dims.IndexOf("[") + 1);
      dims = dims.Substring(0, dims.IndexOf("]"));

      int[] dimenstions = dims.Split(',').Select(X => int.Parse(X)).ToArray();
      return dimenstions;
    }

    public bool IsExpressionArrayType(string typeExpression)
    {
      return Helper.IsExpressionFsArrayType(Helper.RemoveBrackets(typeExpression));
    }

    #endregion
  }
}

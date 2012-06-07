using System.Collections.Generic;
using System.Linq;

namespace ArrayVisualizerExt.ArrayLoaders
{
  internal class VbArrayLoader : IArrayLoader
  {
    #region IArrayLoader Members

    public void ArraysLoader(Dictionary<string, EnvDTE.Expression> arrayExpressions, string prefix, EnvDTE.Expression expression)
    {
      if (expression.DataMembers.Count == 0)
        return;

      //System.Diagnostics.Debug.WriteLine(expression.Name);

      string name = expression.Name;
      string expType;
      if (expression.Type == "System.Array")
      {
        expType = expression.Value;
        expression = expression.DataMembers.Item(1);
      }
      else
        expType = expression.Type;

      expType = RemoveBrackets(expType);

      if (expression.DataMembers.Count > 0)
        if (expType.EndsWith(")") && (expType.EndsWith("()") || expType.EndsWith("(,)") || expType.EndsWith("(,,)") || expType.EndsWith("(,,,)")))
        {
          expType = expType.Substring(0, expType.IndexOf("("));
          expType = expType + "(" + string.Join(",", DimensionsLoader(expression)) + ")";
          string item = prefix + name + " - " + expType;
          arrayExpressions.Add(item, expression);
        }
        else if (expression.Name == "Me")
          foreach (EnvDTE.Expression subExpression in expression.DataMembers)
            ArraysLoader(arrayExpressions, "Me.", subExpression);
    }

    public int[] DimensionsLoader(EnvDTE.Expression expression)
    {
      int last = expression.DataMembers.Count;
      string dims = expression.DataMembers.Item(last).Name;
      dims = dims.Substring(dims.IndexOf("(") + 1);
      dims = dims.Substring(0, dims.IndexOf(")"));

      int[] dimenstions = dims.Split(',').Select(X => int.Parse(X) + 1).ToArray();
      return dimenstions;
    }

    #endregion

    private static string RemoveBrackets(string expType)
    {
      return expType.Replace("}", "").Replace("{", "");
    }
  }
}

using System.Collections.Generic;
using System.Linq;


namespace ArrayVisualizerExt.ArrayLoaders
{
  internal class CsArrayLoader : IArrayLoader
  {
    #region IArrayLoader Members

    public void ArraysLoader(Dictionary<string, EnvDTE.Expression> arrayExpressions, string prefix, EnvDTE.Expression expression)
    {
      if (expression.DataMembers.Count == 0)
        return;

      string expType = Helper.RemoveBrackets(expression.Type);
      if (Helper.IsExpressionCsArrayType(expType))
      {
        string item = prefix + expression.Name + " - " + expression.Value;
        arrayExpressions.Add(item, expression);
      }
      else if (Helper.IsExpressionSharpDXType(expType))
      {
        string displayName = Helper.GetSharpDxDisplayName(expression, this.LeftBracket, this.RightBracket);
        string item = prefix + expression.Name + " - " + displayName;
        arrayExpressions.Add(item, expression);
      }
      else if (expression.Name == "this")
        foreach (EnvDTE.Expression subExpression in expression.DataMembers)
          this.ArraysLoader(arrayExpressions, "this.", subExpression);
    }

    public int[] DimensionsLoader(EnvDTE.Expression expression)
    {
      int[] dimenstions;

      string dims = expression.Value;
      dims = dims.Substring(dims.IndexOf(this.LeftBracket) + 1);
      dims = dims.Substring(0, dims.IndexOf(this.RightBracket));

      dimenstions = dims.Split(',').Select(X => int.Parse(X)).ToArray();

      return dimenstions;
    }

    public bool IsExpressionArrayType(string typeExpression)
    {
      return Helper.IsExpressionCsArrayType(Helper.RemoveBrackets(typeExpression)) || Helper.IsExpressionSharpDXType(typeExpression);
    }

    public char LeftBracket { get { return '['; } }

    public char RightBracket { get { return ']'; } }

    public char NsSeporator { get { return '.'; } }

    public bool LoadStaticElements { get { return true; } }

    #endregion
  }
}

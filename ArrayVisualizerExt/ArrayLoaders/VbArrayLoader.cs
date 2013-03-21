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

      string name = expression.Name;
      string expType;
      if (expression.Type == "System.Array")
      {
        expType = expression.Value;
        expression = expression.DataMembers.Item(1);
      }
      else
        expType = expression.Type;

      expType = Helper.RemoveBrackets(expType);

      if (expression.DataMembers.Count > 0)
        if (Helper.IsExpressionVbArrayType(expType))
        {
          expType = expType.Substring(0, expType.IndexOf(this.LeftBracket));
          expType = expType + this.LeftBracket + string.Join(",", DimensionsLoader(expression)) + this.RightBracket;
          string item = prefix + name + " - " + expType;
          arrayExpressions.Add(item, expression);
        }
        else if (Helper.IsExpressionSharpDXType(expType))
        {
          string displayName = Helper.GetSharpDxDisplayName(expression, this.LeftBracket, this.RightBracket);
          string item = prefix + expression.Name + " - " + displayName;
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
      dims = dims.Substring(dims.IndexOf(this.LeftBracket) + 1);
      dims = dims.Substring(0, dims.IndexOf(this.RightBracket));

      int[] dimenstions = dims.Split(',').Select(X => int.Parse(X) + 1).ToArray();
      return dimenstions;
    }

    public bool IsExpressionArrayType(string typeExpression)
    {
      return Helper.IsExpressionVbArrayType(Helper.RemoveBrackets(typeExpression)) || Helper.IsExpressionSharpDXType(typeExpression);
    }

    public char LeftBracket { get { return '('; } }

    public char RightBracket { get { return ')'; } }

    public char NsSeporator { get { return '.'; } }

    public bool LoadStaticElements { get { return true ; } }

    #endregion
  }
}

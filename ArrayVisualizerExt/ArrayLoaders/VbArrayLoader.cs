using System.Collections.Generic;
using System.Linq;
using ArrayVisualizerExt.TypeParsers;
using EnvDTE;

namespace ArrayVisualizerExt.ArrayLoaders
{
  internal class VbArrayLoader : IArrayLoader
  {
    #region IArrayLoader Members

    public char LeftBracket { get { return '('; } }

    public char RightBracket { get { return ')'; } }

    public bool IsExpressionArrayType(Expression expression)
    {
      string expressionType;
      if (expression.Type == "System.Array")
        expressionType = expression.Value;
      else
        expressionType = expression.Type;

      expressionType = Helper.RemoveBrackets(expressionType);
      return expressionType.EndsWith(")") && (expressionType.EndsWith("()") || expressionType.EndsWith("(,)") || expressionType.EndsWith("(,,)") || expressionType.EndsWith("(,,,)"));
    }

    public string GetDisplayName(Expression expression)
    {
      if (expression.Type == "System.Array")
        return GetDisplayName(expression.DataMembers.Item(1));

      string expType = expression.Type;
      expType = expType.Substring(0, expType.IndexOf(this.LeftBracket));
      expType = expType + this.LeftBracket + string.Join(",", GetDimensions(expression)) + this.RightBracket;
      return expType;
    }

    public IEnumerable<ExpressionInfo> GetArrays(string section, Expression expression, TypeParsers.Parsers parsers, int sectionCode)
    {
      if (expression.Value == "Nothing" || expression.DataMembers.Count == 0)
        yield break;

      foreach (ITypeParser parser in parsers)
        if (parser.IsExpressionTypeSupported(expression))
        {
          yield return new ExpressionInfo(expression.Name, section, parser.GetDisplayName(expression), expression, sectionCode);
          break;
        }

      if (expression.Name == "Me")
        foreach (Expression subExpression in expression.DataMembers)
          foreach (ExpressionInfo item in this.GetArrays("Me.", subExpression, parsers, -1))
            yield return item;
    }

    public int GetMembersCount(EnvDTE.Expression expression)
    {
      if (expression.Type == "System.Array")
        return GetMembersCount(expression.DataMembers.Item(1));

      return expression.DataMembers.Count;
    }

    public int[] GetDimensions(Expression expression)
    {
      if (expression.Type == "System.Array")
        return GetDimensions(expression.DataMembers.Item(1));

      int last = expression.DataMembers.Count;
      string dims = expression.DataMembers.Item(last).Name;
      dims = dims.Substring(dims.IndexOf(this.LeftBracket) + 1);
      dims = dims.Substring(0, dims.IndexOf(this.RightBracket));

      int[] dimenstions = dims.Split(',').Select(X => int.Parse(X) + 1).ToArray();
      return dimenstions;
    }

    public object[] GetValues(Expression expression)
    {
      if (expression.Type == "System.Array")
        return GetValues(expression.DataMembers.Item(1));

      object[] values;
      if (expression.DataMembers.Item(1).Type.Contains(this.LeftBracket))
        values = expression.DataMembers.Cast<EnvDTE.Expression>().ToArray();
      else
        values = expression.DataMembers.Cast<EnvDTE.Expression>().Select(e => e.Value).ToArray();
      return values;
    }

    #endregion
  }
}

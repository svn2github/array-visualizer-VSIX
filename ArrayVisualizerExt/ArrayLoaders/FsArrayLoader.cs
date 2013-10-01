﻿using System.Collections.Generic;
using System.Linq;
using ArrayVisualizerExt.TypeParsers;
using EnvDTE;

namespace ArrayVisualizerExt.ArrayLoaders
{
  internal class FsArrayLoader : IArrayLoader
  {
    #region IArrayLoader Members

    public char LeftBracket { get { return '['; } }

    public char RightBracket { get { return ']'; } }

    public bool IsExpressionArrayType(Expression expression)
    {
      string expressionType = Helper.RemoveBrackets(expression.Type);
      return expressionType.EndsWith("]") && (expressionType.EndsWith("[]") || expressionType.EndsWith("[,]") || expressionType.EndsWith("[,,]") || expressionType.EndsWith("[,,,]"));
    }

    public string GetDisplayName(Expression expression)
    {
      return expression.Value;
    }

    public IEnumerable<ExpressionInfo> GetArrays(string section, Expression expression, Parsers parsers, int sectionCode)
    {
        if (expression.DataMembers.Count == 0)
        yield break;

        foreach (ITypeParser parser in parsers.Where(P => P.IsExpressionTypeSupported(expression)))
        {
            yield return new ExpressionInfo(expression.Name, section, parser.GetDisplayName(expression), expression, sectionCode);
            break;
        }
    }

      public int GetMembersCount(Expression expression)
    {
      return expression.DataMembers.Count;
    }

    public int[] GetDimensions(Expression expression)
    {
      string dims = expression.Value;
      dims = dims.Substring(dims.IndexOf(LeftBracket) + 1);
      dims = dims.Substring(0, dims.IndexOf(RightBracket));

      int[] dimenstions = dims.Split(',').Select(X => int.Parse(X)).ToArray();

      return dimenstions;
    }

    public object[] GetValues(Expression expression)
    {
      object[] values;
      if (expression.DataMembers.Item(1).Type.IndexOf(LeftBracket) != -1)
        values = expression.DataMembers.Cast<Expression>().ToArray();
      else
        values = expression.DataMembers.Cast<Expression>().Select(E => E.Value).ToArray();
      return values;
    }

    #endregion
  }
}

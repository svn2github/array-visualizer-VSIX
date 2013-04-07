using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace ArrayVisualizerExt.TypeParsers
{
  public interface ITypeParser
  {
    bool IsExpressionTypeSupported(Expression expression);

    string GetDisplayName(Expression expression);
    int[] GetDimensions(Expression expression);
    int GetMembersCount(Expression expression);
    object[] GetValues(Expression expression);

    char LeftBracket { get; set; }
    char RightBracket { get; set; }
  }
}

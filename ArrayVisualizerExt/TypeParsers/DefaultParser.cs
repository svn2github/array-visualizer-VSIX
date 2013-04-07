using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArrayVisualizerExt.ArrayLoaders;

namespace ArrayVisualizerExt.TypeParsers
{
  public class DefaultParser : ITypeParser
  {
    private IArrayLoader arrayLoader;

    public DefaultParser(IArrayLoader arrayLoader)
    {
      this.arrayLoader = arrayLoader;
    }

    #region ITypeParser Members

    public char LeftBracket { get; set; }
    public char RightBracket { get; set; }

    public bool IsExpressionTypeSupported(EnvDTE.Expression expression)
    {
      return this.arrayLoader.IsExpressionArrayType(expression);
    }

    public string GetDisplayName(EnvDTE.Expression expression)
    {
      return this.arrayLoader.GetDisplayName(expression);
    }

    public int[] GetDimensions(EnvDTE.Expression expression)
    {
      return this.arrayLoader.GetDimensions(expression);
    }

    public int GetMembersCount(EnvDTE.Expression expression)
    {
      return this.arrayLoader.GetMembersCount(expression);
    }

    public object[] GetValues(EnvDTE.Expression expression)
    {
      return this.arrayLoader.GetValues(expression);
    }

    #endregion
  }
}

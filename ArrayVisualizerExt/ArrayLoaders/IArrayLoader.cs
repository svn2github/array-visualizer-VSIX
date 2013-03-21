using System.Collections.Generic;

namespace ArrayVisualizerExt.ArrayLoaders
{
  public interface IArrayLoader
  {
    char LeftBracket { get; }
    char RightBracket { get; }
    char NsSeporator { get; }

    bool LoadStaticElements { get; }

    void ArraysLoader(Dictionary<string, EnvDTE.Expression> arrayExpressions, string prefix, EnvDTE.Expression expression);
    int[] DimensionsLoader(EnvDTE.Expression expression);
    bool IsExpressionArrayType(string typeExpression);
  }
}

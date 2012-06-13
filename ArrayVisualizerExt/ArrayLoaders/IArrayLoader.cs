using System.Collections.Generic;

namespace ArrayVisualizerExt.ArrayLoaders
{
  public interface IArrayLoader
  {
    void ArraysLoader(Dictionary<string, EnvDTE.Expression> arrayExpressions, string prefix, EnvDTE.Expression expression);
    int[] DimensionsLoader(EnvDTE.Expression expression);
    bool IsExpressionArrayType(string typeExpression);
  }
}

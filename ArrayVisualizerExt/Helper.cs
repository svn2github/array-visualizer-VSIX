using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayVisualizerExt
{
  internal static class Helper
  {
    internal static string RemoveBrackets(string typeExpression)
    {
      return typeExpression.Replace("}", "").Replace("{", "");
    }

    internal static bool IsExpressionCsArrayType(string typeExpression)
    {
      return typeExpression.StartsWith("SharpDX.Matrix") ||typeExpression.StartsWith("SharpDX.Vector")
        || (typeExpression.EndsWith("]") && (typeExpression.EndsWith("[]") || typeExpression.EndsWith("[,]") || typeExpression.EndsWith("[,,]") || typeExpression.EndsWith("[,,,]")));
    }

    internal static bool IsExpressionFsArrayType(string typeExpression)
    {
      return IsExpressionCsArrayType(typeExpression);
    }

    internal static bool IsExpressionVbArrayType(string typeExpression)
    {
      return typeExpression.StartsWith("SharpDX.Matrix") ||typeExpression.StartsWith("SharpDX.Vector")
        || (typeExpression.EndsWith(")") && (typeExpression.EndsWith("()") || typeExpression.EndsWith("(,)") || typeExpression.EndsWith("(,,)") || typeExpression.EndsWith("(,,,)")));
    }
  }
}

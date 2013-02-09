using System.Linq;

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
      return typeExpression.EndsWith("]") && (typeExpression.EndsWith("[]") || typeExpression.EndsWith("[,]") || typeExpression.EndsWith("[,,]") || typeExpression.EndsWith("[,,,]"));
    }

    internal static bool IsExpressionFsArrayType(string typeExpression)
    {
      return IsExpressionCsArrayType(typeExpression);
    }

    internal static bool IsExpressionVbArrayType(string typeExpression)
    {
      return typeExpression.EndsWith(")") && (typeExpression.EndsWith("()") || typeExpression.EndsWith("(,)") || typeExpression.EndsWith("(,,)") || typeExpression.EndsWith("(,,,)"));
    }

    internal static bool IsExpressionSharpDXType(string typeExpression)
    {
      return typeExpression.StartsWith("SharpDX.Matrix") || typeExpression.StartsWith("SharpDX.Vector");
    }

    internal static string GetSharpDxDisplayName(EnvDTE.Expression expression, char leftBracket = '[', char rightBracket = ']')
    {
      int elements = expression.Value.Where(C => C == ':').Count();
      string formatter;
      switch (elements)
      {
        case 2:
          formatter = "Vector{0}2{1}";
          break;
        case 3:
          formatter = "Vector{0}3{1}";
          break;
        case 4:
          formatter = "Vector{0}4{1}";
          break;
        case 16:
          formatter = "Matrix{0}4,4{1}";
          break;
        case 6:
          formatter = "Matrix{0}3,2{1}";
          break;
        case 20:
          formatter = "Matrix{0}5,4{1}";
          break;
        default:
          throw new System.Exception();
      }

      return string.Format(formatter, leftBracket, rightBracket);
    }
  }
}
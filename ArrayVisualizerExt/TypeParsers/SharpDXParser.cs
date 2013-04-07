using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using LinqLib.Sequence;

namespace ArrayVisualizerExt.TypeParsers
{
  public class SharpDXParser : ITypeParser
  {
    #region ITypeParser Members

    public char LeftBracket { get; set; }
    public char RightBracket { get; set; }

    public bool IsExpressionTypeSupported(Expression expression)
    {
      string expressionType = expression.Type;
      return expressionType.StartsWith("SharpDX.Matrix") || expressionType.StartsWith("SharpDX.Vector");
    }

    public string GetDisplayName(EnvDTE.Expression expression)
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

      return string.Format(formatter, this.LeftBracket, this.RightBracket);
    }

    public int[] GetDimensions(EnvDTE.Expression expression)
    {
      switch (expression.Type)
      {
        case "SharpDX.Matrix":
          return new[] { 4, 4 };
        case "SharpDX.Matrix3x2":
          return new[] { 3, 2 };
        case "SharpDX.Matrix5x4":
          return new[] { 5, 4 };
        case "SharpDX.Vector2":
          return new[] { 2 };
        case "SharpDX.Vector3":
          return new[] { 3 };
        case "SharpDX.Vector4":
          return new[] { 4 };
        default:
          throw new NotSupportedException(string.Format("'{0} is not supported.'", expression.Type));
      }
    }

    public int GetMembersCount(EnvDTE.Expression expression)
    {
      int[] dims = GetDimensions(expression);
      if (dims.Length == 1)
        return dims[0];
      else
        return dims[0] * dims[1];
    }
  
    public object[] GetValues(EnvDTE.Expression expression)
    {
      Predicate<Expression> predicate;
      bool rotate = false;
      switch (expression.Type)
      {
        case "SharpDX.Matrix":
        case "SharpDX.Matrix3x2":
        case "SharpDX.Matrix5x4":
          predicate = e => 'M' == e.Name[0];
          break;
        case "SharpDX.Vector2":
          predicate = e => "XY".Contains(e.Name.Last());
          break;
        case "SharpDX.Vector3":
          predicate = e => "XYZ".Contains(e.Name.Last());
          break;
        case "SharpDX.Vector4":
          predicate = e => "XYZW".Contains(e.Name.Last());
          rotate = true;
          break;
        default:
          predicate = null;
          break;
      }

      object[] values;
      IEnumerable<Expression> query = expression.DataMembers.Cast<Expression>().Where(e => predicate(e));

      if (rotate)
        query = query.RotateLeft(1);

      values = new object[0];
      if (expression.DataMembers.Item(1).Type.Contains(this.LeftBracket))
        values = query.ToArray();
      else
        values = query.Select(e => e.Value).ToArray();
      return values;
    }

    #endregion
  }
}
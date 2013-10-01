using ArrayVisualizerExt.ArrayLoaders;

namespace ArrayVisualizerExt.TypeParsers
{
  public class DefaultParser : ITypeParser
  {
    private readonly IArrayLoader arrayLoader;

    public DefaultParser(IArrayLoader arrayLoader)
    {
      this.arrayLoader = arrayLoader;
    }

    #region ITypeParser Members

    public char LeftBracket { get; set; }
    public char RightBracket { get; set; }

    public bool IsExpressionTypeSupported(EnvDTE.Expression expression)
    {
      return arrayLoader.IsExpressionArrayType(expression);
    }

    public string GetDisplayName(EnvDTE.Expression expression)
    {
      return arrayLoader.GetDisplayName(expression);
    }

    public int[] GetDimensions(EnvDTE.Expression expression)
    {
      return arrayLoader.GetDimensions(expression);
    }

    public int GetMembersCount(EnvDTE.Expression expression)
    {
      return arrayLoader.GetMembersCount(expression);
    }

    public object[] GetValues(EnvDTE.Expression expression)
    {
      return arrayLoader.GetValues(expression);
    }

    #endregion
  }
}

using System;
using System.Collections.Generic;
using ArrayVisualizerExt.ArrayLoaders;

namespace ArrayVisualizerExt.TypeParsers
{
  public class Parsers : IEnumerable<ITypeParser>
  {
    private readonly List<ITypeParser> parsers;
    private readonly IArrayLoader arrayLoader;

    public Parsers(IArrayLoader arrayLoader, IEnumerable<Type> selectedParsers)
    {
      this.arrayLoader = arrayLoader;
      parsers = new List<ITypeParser>();

      foreach (Type parserType in selectedParsers)
        AddParser((ITypeParser)Activator.CreateInstance(parserType));

      AddParser(new DefaultParser(arrayLoader)); //must be last!
    }

    private void AddParser(ITypeParser parser)
    {
      parser.LeftBracket = arrayLoader.LeftBracket;
      parser.RightBracket = arrayLoader.RightBracket;
      parsers.Add(parser);
    }

    #region IEnumerable<ITypeParser> Members

    public IEnumerator<ITypeParser> GetEnumerator()
    {
      return parsers.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    #endregion
  }
}

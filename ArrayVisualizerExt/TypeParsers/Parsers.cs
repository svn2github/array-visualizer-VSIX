using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArrayVisualizerExt.ArrayLoaders;

namespace ArrayVisualizerExt.TypeParsers
{
  public class Parsers : IEnumerable<ITypeParser>
  {
    private List<ITypeParser> parsers;
    private IArrayLoader arrayLoader;

    public Parsers(IArrayLoader arrayLoader, IEnumerable<Type> selectedParsers)
    {
      this.arrayLoader = arrayLoader;
      this.parsers = new List<ITypeParser>();

      foreach (Type parserType in selectedParsers)
        AddParser((ITypeParser)Activator.CreateInstance(parserType));

      AddParser(new DefaultParser(this.arrayLoader)); //must be last!
    }

    private void AddParser(ITypeParser parser)
    {
      parser.LeftBracket = this.arrayLoader.LeftBracket;
      parser.RightBracket = this.arrayLoader.RightBracket;
      parsers.Add(parser);
    }

    #region IEnumerable<ITypeParser> Members

    public IEnumerator<ITypeParser> GetEnumerator()
    {
      return this.parsers.GetEnumerator();
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


using EnvDTE;
namespace ArrayVisualizerExt
{
  public class ExpressionInfo
  {
    public ExpressionInfo(string name, string sectionType, string value, Expression expression, int sectionCode)
    {      
      this.Name = name;
      this.Section = sectionType;
      this.SectionCode = sectionCode;
      this.Expression = expression;
      this.Value = value;
    }

    public string FullName
    {
      get { return this.Section + this.Name + " - " + this.Value; }
    }

    public int SectionCode { get; set; }
    public string Name { get; set; }
    public string Section { get; set; }
    public string Value { get; set; }
    public Expression Expression { get; set; }
  }
}

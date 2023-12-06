namespace GeoWallECompiler;
public class Import : Statement
{
    public Import(string library) => Library = library;
    public string Library { get; }
    public override void Accept(IStatementVisitor visitor) => visitor.VisitImportStatement(this);
}

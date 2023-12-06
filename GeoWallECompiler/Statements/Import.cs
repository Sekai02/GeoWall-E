namespace GeoWallECompiler;
public class Import : Statement
{
    public Import(string library, Container fetchedProgram)
    {
        Library = library;
        FetchedProgram = fetchedProgram;
    }

    public string Library { get; }
    public Container FetchedProgram { get; }
    public override void Accept(IStatementVisitor visitor) => visitor.VisitImportStatement(this);
}

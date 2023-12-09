namespace GeoWallECompiler.Expressions;
public class Reciever : Statement
{
    public Reciever(GTypeNames parameterType, string identifier, bool isSequence = false)
    {
        ParameterType = parameterType;
        Identifier = identifier;
        IsSequence = isSequence;
    }
    public GTypeNames ParameterType { get; }
    public string Identifier { get; }
    public bool IsSequence { get; }
    public override void Accept(IStatementVisitor visitor) => visitor.VisitRecieverStatement(this);
}

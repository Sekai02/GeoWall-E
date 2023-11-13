namespace GeoWallECompiler;
public class ConstantsDeclaration : Statement
{
    public override void Accept(IStatementVisitor visitor) => visitor.visitConstantDeclaration(this);
    public ConstantsDeclaration(List<string> constantNames, GSharpExpression value)
    {
        //if (constantNames.Count == 1)
        //{
        //    Constant constant = new(constantNames[0], value);
        //    DeclaredConstants = new() { constant };
        //}
        //else if (value.GetValue() is not GSharpSequence)
        //    throw new SemanticError("Match declaration", "squence", value.CheckType().ToString());
        //else
        //{
        //    //el llamado siguiente a get value puede traer problemas 
        //    GSharpSequence? sequence = value.GetValue() as GSharpSequence;
        //    List<Constant> declaredConstants = new();
        //    int index = 0;
        //    foreach(GSharpObject obj in sequence.Sequence)
        //    {
        //        if(index == constantNames.Count - 1)
        //        {
        //            Literal constantValue = new(new GSharpSequence(sequence, index));
        //            declaredConstants.Add(new Constant(constantNames[index], constantValue));
        //            break;
        //        }
        //        declaredConstants.Add(new Constant(constantNames[index], new Literal(obj)));
        //        index++;
        //    }
        //    if(index < constantNames.Count - 1)
        //    {
        //        for(int i = index; index < constantNames.Count; i++)
        //            declaredConstants.Add(new Constant(constantNames[index], null));
        //    }
        //    DeclaredConstants = declaredConstants;
        //}
    }
    public List<Constant> DeclaredConstants { get; private set; }
}

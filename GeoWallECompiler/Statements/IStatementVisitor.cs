using GeoWallECompiler.Expressions;

namespace GeoWallECompiler;
public interface IStatementVisitor
{
    void VisitExpressionStatement(ExpressionStatement expression);
    void VisitFunctionDeclaration(FunctionDeclaration declaration);
    void VisitConstantDeclaration(ConstantsDeclaration declaration);
    void VisitDrawStatment(DrawStatement drawStatement);
    void VisitColorStatent(ColorStatement color);
    void VisitRestoreStatement(Restore restore);
    void VisitRecieverStatement(Reciever reciever);
}

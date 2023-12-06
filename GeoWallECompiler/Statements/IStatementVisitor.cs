using GeoWallECompiler.Expressions;

namespace GeoWallECompiler;
public interface IStatementVisitor
{
    void VisitStatements(List<Statement> statements);
    void VisitExpressionStatement(ExpressionStatement expression);
    void VisitFunctionDeclaration(FunctionDeclaration declaration);
    void VisitConstantDeclaration(ConstantsDeclaration declaration);
    void VisitDrawStatement(DrawStatement drawStatement);
    void VisitColorStatent(ColorStatement color);
    void VisitRestoreStatement(Restore restore);
    void VisitRecieverStatement(Reciever reciever);
    void VisitPrintStatement(PrintStatement printStatement);
    void VisitImportStatement(Import import);
}

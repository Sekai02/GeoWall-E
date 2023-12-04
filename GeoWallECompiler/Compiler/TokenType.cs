namespace GeoWallECompiler;

/// <summary>
/// Tipos de Tokens de GSharp
/// </summary>
public enum TokenType
{
    //Tokens with one character
    LEFT_PAREN,
    RIGHT_PAREN,
    LEFT_BRACE,
    RIGHT_BRACE,
    COMMA,
    MINUS,
    PLUS,
    SEMICOLON,
    INSTRUCTION_SEPARATOR,
    ASSIGN_SEPARATOR,
    DIVISION,
    MOD,
    PRODUCT,
    POWER,
    AND,
    OR,

    //Tokens with one or two characters
    NOT,
    BANG_EQUAL,
    EQUAL,
    EQUAL_EQUAL,
    GREATER,
    GREATER_EQUAL,
    LESS,
    LESS_EQUAL,

    //Tokens with 3 characters
    ELLIPSIS,

    //Literal Tokens
    IDENTIFIER,
    STRING,
    NUMBER,

    //Keywords

    //Language Keywords
    IF,
    ELSE,
    THEN,
    TRUE,
    FALSE,
    FUNCTION,
    CLASS,
    FOR,
    WHILE,
    RETURN,
    NULL,
    PRINT,
    SIN,
    COS,
    LOG,
    EXP,
    SQRT,
    RAND,
    LET,
    IN,

    //Geo Keywords
    SEQUENCE,

    //Drawing
    DRAW,
    COLOR,
    RESTORE,
    IMPORT,

    //Structures
    POINT,
    LINE,
    SEGMENT,
    RAY,
    CIRCLE,
    ARC,

    //Constants
    UNDEFINED,
    PI,
    EULER,

    EOF
}
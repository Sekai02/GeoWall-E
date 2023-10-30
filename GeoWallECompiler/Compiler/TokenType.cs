namespace GeoWallECompiler;

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
    DIVISION,
    MOD,
    PRODUCT,
    POWER,
    AND,
    OR,

    //Tokens with one or two characters
    BANG,
    BANG_EQUAL,
    EQUAL,
    EQUAL_EQUAL,
    GREATER,
    GREATER_EQUAL,
    LESS,
    LESS_EQUAL,

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
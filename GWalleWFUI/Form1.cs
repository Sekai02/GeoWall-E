using GeoWallECompiler;
using GeoWallECompiler.Expressions;
using GeoWallECompiler.StandardLibrary;
using GeoWallECompiler.Visitors;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GWalleWFUI;

public partial class Aplication : Form, IWalleUI
{
    private Pen Pencil;
    private Color InkColor;
    private string ProgramPath;
    public Aplication()
    {
        InitializeComponent();
        InkColor = Color.Black;
        Pencil = new Pen(new SolidBrush(InkColor))
        {
            Width = 2
        };
        ProgramPath = "";
        //Test();
    }
    private void Test()
    {
        Bitmap image = new(pictureBox1.Width, pictureBox1.Height);
        PictureDrawer drawer = new(Graphics.FromImage(image), Pencil, pictureBox1.Height, pictureBox1.Width);
        pictureBox1.Image = image;
        //var reciever = new Reciever(GTypeNames.Line, "p", false);
        //var call = new FunctionCall("points", new() { new Constant((GString)"p") });
        //var drawStatement = new DrawStatement(call, null);

        var match = new ConstantsDeclaration(new() { "a" }, new InfiniteRangeExpression(new LiteralNumber((GSNumber)1)));
        var count = new FunctionCall("count", new() { new Constant("a") });
        var countcall = new PrintStatement(count);

        List<Statement> statements = new() { match /*reciever, drawStatement*/, countcall };
        Evaluator evaluator = new(drawer, this);
        Resolver resolver = new(evaluator);
        TypeChecker typeChecker = new(evaluator);
        resolver.VisitStatements(statements);
        if (ErrorHandler.HadError)
            return;
        typeChecker.VisitStatements(statements);
        if (ErrorHandler.HadError)
            return;
        evaluator.VisitStatements(statements);
        if (ErrorHandler.HadError)
            return;
        //GSharp.CanvasHeight = pictureBox1.Height;
        //GSharp.CanvasWidth = pictureBox1.Width;
        //for (int i = 0; i < 5; i++)
        //{
        //    drawer.Reset();
        //    var c = GSPoint.GetRandomInstance();
        //    var p1 = GSPoint.GetRandomInstance();
        //    var p2 = GSPoint.GetRandomInstance();
        //    var radius = Measure.GetRandomInstance();
        //    var a =  new Arc(c, p1, p2, radius);
        //    var l1 = new Segment(c, p1);
        //    var l2 = new Segment(c, p2);
        //    a.Draw(drawer);
        //    //drawer.SetColor("blue");
        //    //c.Draw(drawer, (GString)"c");
        //    //l1.Draw(drawer, (GString)"1");
        //    //l2.Draw(drawer, (GString)"2");
        //    drawer.SetColor("red");
        //    for (int j = 0; j < 30; j++)
        //    {
        //        drawer.DrawPoint(a.GetRandomPoint());
        //    }
        //    image.Save("C:\\Users\\Jossue\\Cosas\\test.bmp");
        //}
    }
    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Dispose();
    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        treeView1.Nodes.Clear();
        folderBrowserDialog1.ShowDialog();
        Cursor.Current = Cursors.WaitCursor;
        if (folderBrowserDialog1.SelectedPath == "")
            return;
        foreach (var item in Directory.GetDirectories(folderBrowserDialog1.SelectedPath))
        {
            DirectoryInfo info = new(item);
            var node = treeView1.Nodes.Add(info.Name);
            node.Tag = info;
        }
        foreach (var item in Directory.GetFiles(folderBrowserDialog1.SelectedPath))
        {
            FileInfo info = new(item);
            var node = treeView1.Nodes.Add(info.Name);
            node.Tag = info;
        }
        Cursor.Current = Cursors.Default;
    }
    private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node.Tag is null)
            return;
        if (e.Node.Tag.GetType() == typeof(DirectoryInfo))
        {
            e.Node.Nodes.Clear();
            var directory = (DirectoryInfo)e.Node.Tag;
            foreach (var item in Directory.GetDirectories(directory.FullName))
            {
                DirectoryInfo info = new(item);
                var node = e.Node.Nodes.Add(info.Name);
                node.Tag = info;
            }
            foreach (var item in Directory.GetFiles(directory.FullName))
            {
                FileInfo info = new(item);
                var node = e.Node.Nodes.Add(info.Name);
                node.Tag = info;
            }
            e.Node.Expand();
            return;
        }
        var file = (FileInfo)e.Node.Tag;
        ProgramPath = file.FullName;
        Entry.Text = File.ReadAllText(file.FullName);
    }
    private void Entry_TextChanged(object sender, EventArgs e)
    {
        string keywords = @"\b(draw|color|restore|import|point|line|sequence|circle|ray|segment|print|arc|and|or|not)\b";
        MatchCollection keywordsMatch = Regex.Matches(Entry.Text, keywords);
        string strings = "\".*?\"";
        MatchCollection stringsMatch = Regex.Matches(Entry.Text, strings);
        string tertiaryOperators = @"\b(let|in|if|then|else)\b";
        MatchCollection tertiaryOperatorsMatch = Regex.Matches(Entry.Text, tertiaryOperators);
        string functions = @"\b([A-Z]|[a-z]|\d)+\(";
        MatchCollection functionsMatch = Regex.Matches(Entry.Text, functions);
        string numbers = @"\b\d+\b";
        MatchCollection numbersMatch = Regex.Matches(Entry.Text, numbers);
        string variables = @"\b([A-Z]|[a-z]|\d)+\b";
        MatchCollection variablesMatch = Regex.Matches(Entry.Text, variables);

        int originalIndex = Entry.SelectionStart;
        int originalLength = Entry.SelectionLength;
        Color originalColor = Color.White;

        label1.Focus();

        Entry.SelectionStart = 0;
        Entry.SelectionLength = Entry.Text.Length;
        Entry.SelectionColor = originalColor;

        foreach (Match m in variablesMatch)
        {
            Entry.SelectionStart = m.Index;
            Entry.SelectionLength = m.Length;
            Entry.SelectionColor = Color.FromArgb(112, 205, 254);
        }
        foreach (Match m in numbersMatch)
        {
            Entry.SelectionStart = m.Index;
            Entry.SelectionLength = m.Length;
            Entry.SelectionColor = Color.FromArgb(156, 206, 168);
        }
        foreach (Match m in keywordsMatch)
        {
            Entry.SelectionStart = m.Index;
            Entry.SelectionLength = m.Length;
            Entry.SelectionColor = Color.FromArgb(76, 156, 214);
        }
        foreach (Match m in stringsMatch)
        {
            Entry.SelectionStart = m.Index;
            Entry.SelectionLength = m.Length;
            Entry.SelectionColor = Color.FromArgb(212, 157, 133);
        }
        foreach (Match m in tertiaryOperatorsMatch)
        {
            Entry.SelectionStart = m.Index;
            Entry.SelectionLength = m.Length;
            Entry.SelectionColor = Color.FromArgb(216, 160, 223);
        }
        foreach (Match m in functionsMatch)
        {
            Entry.SelectionStart = m.Index;
            Entry.SelectionLength = m.Length - 1;
            Entry.SelectionColor = Color.FromArgb(220, 220, 167);
        }
        Entry.SelectionStart = originalIndex;
        Entry.SelectionLength = originalLength;
        Entry.SelectionColor = originalColor;
        Entry.Focus();
    }
    private void Input_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Return)
            return;

        e.Handled = true;
        e.SuppressKeyPress = true; //evita el sonido cuando se presiona enter

        if (Input.Text == "")
            return;
        if (Input.Text == "clear")
            Terminal.Text = "";
        else if (Input.Text == "run")
            StartButtonClick(sender, e);
        else
            AppendLineWithColor(Terminal, $"Invalid command '{Input.Text}'", Color.Orange);
        Input.Text = "";
    }
    private void StartButtonClick(object sender, EventArgs e)
    {
        if (ProgramPath == "")
        {
            saveFileDialog1.ShowDialog();
            ProgramPath = saveFileDialog1.FileName;
        }
        if (ProgramPath == "")
            return;
        Run();
    }
    private void Run()
    {
        Bitmap image = new(pictureBox1.Width, pictureBox1.Height);
        PictureDrawer drawer = new(Graphics.FromImage(image), Pencil, pictureBox1.Height, pictureBox1.Width);
        pictureBox1.Image = image;

        //File.Create(ProgramPath);
        File.WriteAllText(ProgramPath, Entry.Text);
        GSharp.RunFile(ProgramPath, drawer, this);
        if (!ErrorHandler.HadError)
        {
            AppendLineWithColor(Terminal, "Process exited whitout errors", Color.White);
            return;
        }
        Terminal.SelectionStart = Terminal.Text.Length;
        foreach (var error in ErrorHandler.GetErrors())
            AppendLineWithColor(Terminal, error.Message, Color.Red);
        ErrorHandler.Reset();
    }
    private static void AppendLineWithColor(RichTextBox box, string text, Color color)
    {
        if (box.Text != "")
            text = "\n" + text;
        box.SelectionStart = box.TextLength;
        box.SelectionLength = 0;
        box.SelectionColor = color;
        box.AppendText(text);
        box.SelectionColor = box.ForeColor;
    }
    public void Print(object? obj)
    {
        if (obj is null)
        {
            AppendLineWithColor(Terminal, "undefined", Color.White);
            return;
        }
        AppendLineWithColor(Terminal, obj.ToString()!, Color.White);
    }

    public void PrintError(GSharpException ex) => AppendLineWithColor(Terminal, ex.Message, Color.Red);
}

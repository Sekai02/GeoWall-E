using GeoWallECompiler;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GWalleWFUI;

public partial class Aplication : Form
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
        string keywords = @"\b(draw|color|restore|import|point|line|circle|ray|segment|and|or|not)\b";
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
    private void Test()
    {
        Bitmap image = new(pictureBox1.Width, pictureBox1.Height);
        var center = new GSharpPoint(new GSharpNumber(60), new GSharpNumber(60));
        var p1 = new GSharpPoint(new GSharpNumber(155), new GSharpNumber(112));
        var p2 = new GSharpPoint(new GSharpNumber(140), new GSharpNumber(500));
        var r = new GSharpNumber(50);

        var p1S = new GSharpString("punto 1");
        var p2S = new GSharpString("punto 2");
        var centroS = new GSharpString("center");
        var arcS = new GSharpString("arco");
        var segmento = new GSharpString("segmento");

        Arc arc = new(center, p1, p2, r);
        PictureDrawer drawer = new(Graphics.FromImage(image), Pencil);
        Segment segment1 = new(center, p1);
        Segment segment2 = new(center, p2);
        drawer.DrawArc(arc, arcS);
        drawer.DrawSegment(segment1);
        drawer.DrawSegment(segment2);
        drawer.DrawPoint(p1, p1S);
        drawer.DrawPoint(p2, p2S);
        pictureBox1.Image = image;
    }
    private void Input_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Return)
            return;
        if (Input.Text == "")
            return;
        if (Input.Text == "clear")
            Terminal.Text = "";
        else if (Input.Text == "run")
        {
            StartButtonClick(sender, e);
            e.Handled = true;
        }
        else
        {
            AppendLine(Terminal, $"Invalid command '{Input.Text}'", Color.Orange);
        }
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
        //File.Create(ProgramPath);
        File.WriteAllText(ProgramPath, Entry.Text);
        GSharp.RunFile(ProgramPath);

        if (!ErrorHandler.HadError)
        {
            AppendLine(Terminal, "Process exited whitout errors", Color.White);
            return;
        }

        Terminal.SelectionStart = Terminal.Text.Length;
        foreach (var error in ErrorHandler.GetErrors())
        {
            AppendLine(Terminal, error.Message, Color.Red);
        }
        ErrorHandler.Reset();
    }
    private static void AppendLine(RichTextBox box, string text, Color color)
    {
        if (box.Text != "")
            text = "\n" + text;
        box.SelectionStart = box.TextLength;
        box.SelectionLength = 0;
        box.SelectionColor = color;
        box.AppendText(text);
        box.SelectionColor = box.ForeColor;
    }
}

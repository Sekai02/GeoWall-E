using GeoWallECompiler;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;

namespace GWalleWFUI;

public partial class Aplication : Form
{
    private Pen Pencil;
    private Color InkColor;
    public Aplication()
    {
        InitializeComponent();
        InkColor = Color.Black;
        Pencil = new Pen(new SolidBrush(InkColor));
        Pencil.Width = 2;
        Test();
    }
    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Dispose();
    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
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
        richTextBox1.Text = File.ReadAllText(file.FullName);
    }
    private void richTextBox1_TextChanged(object sender, EventArgs e)
    {
        string keywords = @"\b(draw|color|restore|import|point|line|circle|ray|segment|and|or|not)\b";
        MatchCollection keywordsMatch = Regex.Matches(richTextBox1.Text, keywords);
        string strings = "\".*?\"";
        MatchCollection stringsMatch = Regex.Matches(richTextBox1.Text, strings);
        string tertiaryOperators = @"\b(let|in|if|then|else)\b";
        MatchCollection tertiaryOperatorsMatch = Regex.Matches(richTextBox1.Text, tertiaryOperators);
        string functions = @"\b([A-Z]|[a-z]|\d)+\(";
        MatchCollection functionsMatch = Regex.Matches(richTextBox1.Text, functions);
        string numbers = @"\b\d+\b";
        MatchCollection numbersMatch = Regex.Matches(richTextBox1.Text, numbers);
        string variables = @"\b([A-Z]|[a-z]|\d)+\b";
        MatchCollection variablesMatch = Regex.Matches(richTextBox1.Text, variables);

        int originalIndex = richTextBox1.SelectionStart;
        int originalLength = richTextBox1.SelectionLength;
        Color originalColor = Color.White;

        label1.Focus();

        richTextBox1.SelectionStart = 0;
        richTextBox1.SelectionLength = richTextBox1.Text.Length;
        richTextBox1.SelectionColor = originalColor;

        foreach (Match m in variablesMatch)
        {
            richTextBox1.SelectionStart = m.Index;
            richTextBox1.SelectionLength = m.Length;
            richTextBox1.SelectionColor = Color.FromArgb(112, 205, 254);
        }
        foreach (Match m in numbersMatch)
        {
            richTextBox1.SelectionStart = m.Index;
            richTextBox1.SelectionLength = m.Length;
            richTextBox1.SelectionColor = Color.FromArgb(156, 206, 168);
        }
        foreach (Match m in keywordsMatch)
        {
            richTextBox1.SelectionStart = m.Index;
            richTextBox1.SelectionLength = m.Length;
            richTextBox1.SelectionColor = Color.FromArgb(76, 156, 214);
        }
        foreach (Match m in stringsMatch)
        {
            richTextBox1.SelectionStart = m.Index;
            richTextBox1.SelectionLength = m.Length;
            richTextBox1.SelectionColor = Color.FromArgb(212, 157, 133);
        }
        foreach (Match m in tertiaryOperatorsMatch)
        {
            richTextBox1.SelectionStart = m.Index;
            richTextBox1.SelectionLength = m.Length;
            richTextBox1.SelectionColor = Color.FromArgb(216, 160, 223);
        }
        foreach (Match m in functionsMatch)
        {
            richTextBox1.SelectionStart = m.Index;
            richTextBox1.SelectionLength = m.Length - 1;
            richTextBox1.SelectionColor = Color.FromArgb(220, 220, 167);
        }
        richTextBox1.SelectionStart = originalIndex;
        richTextBox1.SelectionLength = originalLength;
        richTextBox1.SelectionColor = originalColor;
        richTextBox1.Focus();
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
}

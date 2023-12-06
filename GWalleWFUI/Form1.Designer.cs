namespace GWalleWFUI;

partial class Aplication
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Aplication));
        treeView1 = new TreeView();
        toolStrip1 = new ToolStrip();
        fileButton = new ToolStripDropDownButton();
        newToolStripMenuItem = new ToolStripMenuItem();
        openToolStripMenuItem = new ToolStripMenuItem();
        saveToolStripMenuItem = new ToolStripMenuItem();
        codeToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem1 = new ToolStripMenuItem();
        exitToolStripMenuItem = new ToolStripMenuItem();
        DebugButton = new ToolStripDropDownButton();
        startWithoutDebuggingToolStripMenuItem = new ToolStripMenuItem();
        Entry = new RichTextBox();
        pictureBox1 = new PictureBox();
        folderBrowserDialog1 = new FolderBrowserDialog();
        label1 = new Label();
        Terminal = new RichTextBox();
        Input = new TextBox();
        saveFileDialog1 = new SaveFileDialog();
        compileLibraryToolStripMenuItem = new ToolStripMenuItem();
        toolStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // treeView1
        // 
        treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        treeView1.BackColor = Color.FromArgb(30, 30, 30);
        treeView1.ForeColor = SystemColors.Window;
        treeView1.Location = new Point(12, 30);
        treeView1.Name = "treeView1";
        treeView1.Size = new Size(175, 741);
        treeView1.TabIndex = 0;
        treeView1.NodeMouseDoubleClick += treeView1_NodeMouseDoubleClick;
        // 
        // toolStrip1
        // 
        toolStrip1.ImageScalingSize = new Size(20, 20);
        toolStrip1.Items.AddRange(new ToolStripItem[] { fileButton, DebugButton });
        toolStrip1.Location = new Point(0, 0);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Size = new Size(1546, 27);
        toolStrip1.TabIndex = 2;
        toolStrip1.Text = "toolStrip1";
        // 
        // fileButton
        // 
        fileButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        fileButton.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, exitToolStripMenuItem });
        fileButton.Image = (Image)resources.GetObject("fileButton.Image");
        fileButton.ImageTransparentColor = Color.Magenta;
        fileButton.Name = "fileButton";
        fileButton.Size = new Size(46, 24);
        fileButton.Text = "File";
        // 
        // newToolStripMenuItem
        // 
        newToolStripMenuItem.Name = "newToolStripMenuItem";
        newToolStripMenuItem.Size = new Size(128, 26);
        newToolStripMenuItem.Text = "New";
        // 
        // openToolStripMenuItem
        // 
        openToolStripMenuItem.Name = "openToolStripMenuItem";
        openToolStripMenuItem.Size = new Size(128, 26);
        openToolStripMenuItem.Text = "Open";
        openToolStripMenuItem.Click += openToolStripMenuItem_Click;
        // 
        // saveToolStripMenuItem
        // 
        saveToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { codeToolStripMenuItem, toolStripMenuItem1 });
        saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        saveToolStripMenuItem.Size = new Size(128, 26);
        saveToolStripMenuItem.Text = "Save";
        // 
        // codeToolStripMenuItem
        // 
        codeToolStripMenuItem.Name = "codeToolStripMenuItem";
        codeToolStripMenuItem.Size = new Size(127, 26);
        codeToolStripMenuItem.Text = "Code";
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(127, 26);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(128, 26);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // DebugButton
        // 
        DebugButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        DebugButton.DropDownItems.AddRange(new ToolStripItem[] { startWithoutDebuggingToolStripMenuItem, compileLibraryToolStripMenuItem });
        DebugButton.Image = (Image)resources.GetObject("DebugButton.Image");
        DebugButton.ImageTransparentColor = Color.Magenta;
        DebugButton.Name = "DebugButton";
        DebugButton.Size = new Size(57, 24);
        DebugButton.Text = "Build";
        // 
        // startWithoutDebuggingToolStripMenuItem
        // 
        startWithoutDebuggingToolStripMenuItem.Name = "startWithoutDebuggingToolStripMenuItem";
        startWithoutDebuggingToolStripMenuItem.Size = new Size(224, 26);
        startWithoutDebuggingToolStripMenuItem.Text = "Run";
        startWithoutDebuggingToolStripMenuItem.Click += StartButtonClick;
        // 
        // Entry
        // 
        Entry.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        Entry.BackColor = Color.FromArgb(30, 30, 30);
        Entry.Font = new Font("Cascadia Code", 11F, FontStyle.Regular, GraphicsUnit.Point);
        Entry.ForeColor = SystemColors.Window;
        Entry.Location = new Point(193, 30);
        Entry.Name = "Entry";
        Entry.Size = new Size(672, 535);
        Entry.TabIndex = 3;
        Entry.Text = "";
        Entry.WordWrap = false;
        Entry.TextChanged += Entry_TextChanged;
        // 
        // pictureBox1
        // 
        pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        pictureBox1.BackColor = SystemColors.ControlLightLight;
        pictureBox1.Location = new Point(871, 30);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(663, 741);
        pictureBox1.TabIndex = 4;
        pictureBox1.TabStop = false;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(0, 708);
        label1.Name = "label1";
        label1.Size = new Size(12, 20);
        label1.TabIndex = 6;
        label1.Text = ".";
        // 
        // Terminal
        // 
        Terminal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        Terminal.BackColor = Color.FromArgb(30, 30, 30);
        Terminal.Font = new Font("Cascadia Code", 10F, FontStyle.Regular, GraphicsUnit.Point);
        Terminal.ForeColor = SystemColors.Window;
        Terminal.Location = new Point(193, 571);
        Terminal.Name = "Terminal";
        Terminal.ReadOnly = true;
        Terminal.Size = new Size(672, 164);
        Terminal.TabIndex = 7;
        Terminal.Text = "";
        // 
        // Input
        // 
        Input.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        Input.BackColor = Color.FromArgb(30, 30, 30);
        Input.Font = new Font("Cascadia Code", 11F, FontStyle.Regular, GraphicsUnit.Point);
        Input.ForeColor = SystemColors.Window;
        Input.Location = new Point(193, 741);
        Input.Name = "Input";
        Input.Size = new Size(672, 29);
        Input.TabIndex = 8;
        Input.KeyDown += Input_KeyDown;
        // 
        // saveFileDialog1
        // 
        saveFileDialog1.DefaultExt = "gw";
        // 
        // compileLibraryToolStripMenuItem
        // 
        compileLibraryToolStripMenuItem.Name = "compileLibraryToolStripMenuItem";
        compileLibraryToolStripMenuItem.Size = new Size(224, 26);
        compileLibraryToolStripMenuItem.Text = "Compile Library";
        compileLibraryToolStripMenuItem.Click += compileLibraryToolStripMenuItem_Click;
        // 
        // Aplication
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(17, 17, 17);
        ClientSize = new Size(1546, 780);
        Controls.Add(Input);
        Controls.Add(Terminal);
        Controls.Add(label1);
        Controls.Add(pictureBox1);
        Controls.Add(Entry);
        Controls.Add(toolStrip1);
        Controls.Add(treeView1);
        Name = "Aplication";
        Text = "Geo Wall-E Interpreter";
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TreeView treeView1;
    private ToolStrip toolStrip1;
    private ToolStripDropDownButton fileButton;
    private ToolStripMenuItem newToolStripMenuItem;
    private ToolStripMenuItem openToolStripMenuItem;
    private ToolStripMenuItem saveToolStripMenuItem;
    private ToolStripMenuItem codeToolStripMenuItem;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripDropDownButton DebugButton;
    private ToolStripMenuItem startWithoutDebuggingToolStripMenuItem;
    private RichTextBox Entry;
    private PictureBox pictureBox1;
    private ToolStripMenuItem exitToolStripMenuItem;
    private FolderBrowserDialog folderBrowserDialog1;
    private Label label1;
    private RichTextBox Terminal;
    private TextBox Input;
    private SaveFileDialog saveFileDialog1;
    private ToolStripMenuItem compileLibraryToolStripMenuItem;
}

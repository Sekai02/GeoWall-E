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
        richTextBox1 = new RichTextBox();
        pictureBox1 = new PictureBox();
        ErrorsList = new ListBox();
        folderBrowserDialog1 = new FolderBrowserDialog();
        label1 = new Label();
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
        treeView1.Size = new Size(175, 685);
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
        DebugButton.DropDownItems.AddRange(new ToolStripItem[] { startWithoutDebuggingToolStripMenuItem });
        DebugButton.Image = (Image)resources.GetObject("DebugButton.Image");
        DebugButton.ImageTransparentColor = Color.Magenta;
        DebugButton.Name = "DebugButton";
        DebugButton.Size = new Size(68, 24);
        DebugButton.Text = "Debug";
        // 
        // startWithoutDebuggingToolStripMenuItem
        // 
        startWithoutDebuggingToolStripMenuItem.Name = "startWithoutDebuggingToolStripMenuItem";
        startWithoutDebuggingToolStripMenuItem.Size = new Size(123, 26);
        startWithoutDebuggingToolStripMenuItem.Text = "Start";
        // 
        // richTextBox1
        // 
        richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        richTextBox1.BackColor = Color.FromArgb(30, 30, 30);
        richTextBox1.Font = new Font("Cascadia Code", 11F, FontStyle.Regular, GraphicsUnit.Point);
        richTextBox1.ForeColor = SystemColors.Window;
        richTextBox1.Location = new Point(193, 30);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(672, 515);
        richTextBox1.TabIndex = 3;
        richTextBox1.Text = "";
        richTextBox1.WordWrap = false;
        richTextBox1.TextChanged += richTextBox1_TextChanged;
        // 
        // pictureBox1
        // 
        pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        pictureBox1.BackColor = SystemColors.ControlLightLight;
        pictureBox1.Location = new Point(871, 30);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(663, 685);
        pictureBox1.TabIndex = 4;
        pictureBox1.TabStop = false;
        // 
        // ErrorsList
        // 
        ErrorsList.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        ErrorsList.BackColor = Color.FromArgb(30, 30, 30);
        ErrorsList.ForeColor = Color.Red;
        ErrorsList.FormattingEnabled = true;
        ErrorsList.ItemHeight = 20;
        ErrorsList.Location = new Point(193, 551);
        ErrorsList.Name = "ErrorsList";
        ErrorsList.Size = new Size(672, 164);
        ErrorsList.TabIndex = 5;
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
        // Aplication
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(17, 17, 17);
        ClientSize = new Size(1546, 724);
        Controls.Add(label1);
        Controls.Add(ErrorsList);
        Controls.Add(pictureBox1);
        Controls.Add(richTextBox1);
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
    private RichTextBox richTextBox1;
    private PictureBox pictureBox1;
    private ListBox ErrorsList;
    private ToolStripMenuItem exitToolStripMenuItem;
    private FolderBrowserDialog folderBrowserDialog1;
    private Label label1;
}

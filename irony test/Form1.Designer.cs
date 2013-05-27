namespace ONeil
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtONeilCode = new System.Windows.Forms.TextBox();
            this.tvParse = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.tvAstTree = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpParse = new System.Windows.Forms.TabPage();
            this.tbpAst = new System.Windows.Forms.TabPage();
            this.txtCSharpCode = new System.Windows.Forms.TextBox();
            this.generatorCCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbpParse.SuspendLayout();
            this.tbpAst.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtONeilCode
            // 
            this.txtONeilCode.Location = new System.Drawing.Point(12, 27);
            this.txtONeilCode.Multiline = true;
            this.txtONeilCode.Name = "txtONeilCode";
            this.txtONeilCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtONeilCode.Size = new System.Drawing.Size(328, 326);
            this.txtONeilCode.TabIndex = 0;
            // 
            // tvParse
            // 
            this.tvParse.Location = new System.Drawing.Point(6, 6);
            this.tvParse.Name = "tvParse";
            this.tvParse.Size = new System.Drawing.Size(400, 401);
            this.tvParse.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.codeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1118, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.createTreeToolStripMenuItem,
            this.generatorCCodeToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // createTreeToolStripMenuItem
            // 
            this.createTreeToolStripMenuItem.Name = "createTreeToolStripMenuItem";
            this.createTreeToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.createTreeToolStripMenuItem.Text = "Create Tree";
            this.createTreeToolStripMenuItem.Click += new System.EventHandler(this.createTreeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // codeToolStripMenuItem
            // 
            this.codeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addClassToolStripMenuItem});
            this.codeToolStripMenuItem.Name = "codeToolStripMenuItem";
            this.codeToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.codeToolStripMenuItem.Text = "Code";
            // 
            // addClassToolStripMenuItem
            // 
            this.addClassToolStripMenuItem.Name = "addClassToolStripMenuItem";
            this.addClassToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.addClassToolStripMenuItem.Text = "Add Class";
            this.addClassToolStripMenuItem.Click += new System.EventHandler(this.addClassToolStripMenuItem_Click);
            // 
            // txtErrors
            // 
            this.txtErrors.Location = new System.Drawing.Point(12, 359);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ReadOnly = true;
            this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtErrors.Size = new System.Drawing.Size(667, 109);
            this.txtErrors.TabIndex = 3;
            // 
            // tvAstTree
            // 
            this.tvAstTree.Location = new System.Drawing.Point(6, 6);
            this.tvAstTree.Name = "tvAstTree";
            this.tvAstTree.Size = new System.Drawing.Size(400, 401);
            this.tvAstTree.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbpParse);
            this.tabControl1.Controls.Add(this.tbpAst);
            this.tabControl1.Location = new System.Drawing.Point(686, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(420, 439);
            this.tabControl1.TabIndex = 6;
            // 
            // tbpParse
            // 
            this.tbpParse.Controls.Add(this.tvParse);
            this.tbpParse.Location = new System.Drawing.Point(4, 22);
            this.tbpParse.Name = "tbpParse";
            this.tbpParse.Padding = new System.Windows.Forms.Padding(3);
            this.tbpParse.Size = new System.Drawing.Size(412, 413);
            this.tbpParse.TabIndex = 0;
            this.tbpParse.Text = "Parse Tree";
            this.tbpParse.UseVisualStyleBackColor = true;
            this.tbpParse.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // tbpAst
            // 
            this.tbpAst.Controls.Add(this.tvAstTree);
            this.tbpAst.Location = new System.Drawing.Point(4, 22);
            this.tbpAst.Name = "tbpAst";
            this.tbpAst.Padding = new System.Windows.Forms.Padding(3);
            this.tbpAst.Size = new System.Drawing.Size(412, 413);
            this.tbpAst.TabIndex = 1;
            this.tbpAst.Text = "Ast Tree";
            this.tbpAst.UseVisualStyleBackColor = true;
            // 
            // txtCSharpCode
            // 
            this.txtCSharpCode.Location = new System.Drawing.Point(346, 28);
            this.txtCSharpCode.Multiline = true;
            this.txtCSharpCode.Name = "txtCSharpCode";
            this.txtCSharpCode.ReadOnly = true;
            this.txtCSharpCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCSharpCode.Size = new System.Drawing.Size(333, 326);
            this.txtCSharpCode.TabIndex = 7;
            // 
            // generatorCCodeToolStripMenuItem
            // 
            this.generatorCCodeToolStripMenuItem.Name = "generatorCCodeToolStripMenuItem";
            this.generatorCCodeToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.generatorCCodeToolStripMenuItem.Text = "Generator C# Code";
            this.generatorCCodeToolStripMenuItem.Click += new System.EventHandler(this.generatorCCodeToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 479);
            this.Controls.Add(this.txtCSharpCode);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtErrors);
            this.Controls.Add(this.txtONeilCode);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tbpParse.ResumeLayout(false);
            this.tbpAst.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtONeilCode;
        private System.Windows.Forms.TreeView tvParse;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox txtErrors;
        private System.Windows.Forms.ToolStripMenuItem createTreeToolStripMenuItem;
        private System.Windows.Forms.TreeView tvAstTree;
        private System.Windows.Forms.ToolStripMenuItem codeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addClassToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbpParse;
        private System.Windows.Forms.TabPage tbpAst;
        private System.Windows.Forms.TextBox txtCSharpCode;
        private System.Windows.Forms.ToolStripMenuItem generatorCCodeToolStripMenuItem;
    }
}


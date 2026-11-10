namespace ContactsSystemWinForms
{
    partial class frmListContacts
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
            components = new System.ComponentModel.Container();
            Button btnAddNewContact;
            Button button1;
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblTitle = new Label();
            dgvAllContacts = new DataGridView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            editToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip2 = new ContextMenuStrip(components);
            textBox3 = new TextBox();
            lblSearchEmail = new Label();
            textBox2 = new TextBox();
            lblSearchPhone = new Label();
            textBox1 = new TextBox();
            lblSearchName = new Label();
            pnlSearch = new Panel();
            btnAddNewContact = new Button();
            button1 = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAllContacts).BeginInit();
            contextMenuStrip1.SuspendLayout();
            pnlSearch.SuspendLayout();
            SuspendLayout();
            // 
            // btnAddNewContact
            // 
            btnAddNewContact.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewContact.BackColor = Color.FromArgb(41, 128, 185);
            btnAddNewContact.Cursor = Cursors.Hand;
            btnAddNewContact.FlatAppearance.BorderSize = 0;
            btnAddNewContact.FlatStyle = FlatStyle.Flat;
            btnAddNewContact.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnAddNewContact.ForeColor = Color.White;
            btnAddNewContact.Location = new Point(1107, 26);
            btnAddNewContact.Margin = new Padding(4);
            btnAddNewContact.Name = "btnAddNewContact";
            btnAddNewContact.Size = new Size(180, 48);
            btnAddNewContact.TabIndex = 1;
            btnAddNewContact.Text = "+ Add New Contact";
            btnAddNewContact.UseVisualStyleBackColor = false;
            btnAddNewContact.Click += btnAddNewContact_Click;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.BackColor = Color.FromArgb(41, 128, 185);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(823, 26);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(180, 48);
            button1.TabIndex = 6;
            button1.Text = "+ Add New Contact";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(41, 65, 92);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1325, 88);
            pnlHeader.TabIndex = 3;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 19.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(427, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(219, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "List Contacts";
            // 
            // dgvAllContacts
            // 
            dgvAllContacts.AllowUserToAddRows = false;
            dgvAllContacts.AllowUserToDeleteRows = false;
            dgvAllContacts.AllowUserToOrderColumns = true;
            dgvAllContacts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(247, 249, 251);
            dgvAllContacts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvAllContacts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAllContacts.BackgroundColor = Color.White;
            dgvAllContacts.BorderStyle = BorderStyle.None;
            dgvAllContacts.CausesValidation = false;
            dgvAllContacts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAllContacts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(41, 65, 92);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.Padding = new Padding(4);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(41, 65, 92);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvAllContacts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvAllContacts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAllContacts.ContextMenuStrip = contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new Padding(2);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(214, 233, 248);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvAllContacts.DefaultCellStyle = dataGridViewCellStyle3;
            dgvAllContacts.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvAllContacts.EnableHeadersVisualStyles = false;
            dgvAllContacts.Font = new Font("Segoe UI", 9.5F);
            dgvAllContacts.GridColor = Color.FromArgb(230, 230, 230);
            dgvAllContacts.Location = new Point(24, 227);
            dgvAllContacts.Margin = new Padding(4);
            dgvAllContacts.Name = "dgvAllContacts";
            dgvAllContacts.ReadOnly = true;
            dgvAllContacts.RowHeadersVisible = false;
            dgvAllContacts.RowHeadersWidth = 51;
            dgvAllContacts.RowTemplate.Height = 32;
            dgvAllContacts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAllContacts.Size = new Size(1217, 325);
            dgvAllContacts.TabIndex = 0;
            dgvAllContacts.CellClick += dgvAllContacts_CellClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Font = new Font("Segoe UI", 9.5F);
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(125, 56);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(124, 26);
            editToolStripMenuItem.Text = "Edit";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(124, 26);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.ImageScalingSize = new Size(20, 20);
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(61, 4);
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.Font = new Font("Segoe UI", 9.5F);
            textBox3.Location = new Point(536, 38);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(220, 29);
            textBox3.TabIndex = 5;
            // 
            // lblSearchEmail
            // 
            lblSearchEmail.AutoSize = true;
            lblSearchEmail.Font = new Font("Segoe UI", 9F);
            lblSearchEmail.ForeColor = Color.FromArgb(64, 64, 64);
            lblSearchEmail.Location = new Point(536, 18);
            lblSearchEmail.Name = "lblSearchEmail";
            lblSearchEmail.Size = new Size(46, 20);
            lblSearchEmail.TabIndex = 5;
            lblSearchEmail.Text = "Email";
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Font = new Font("Segoe UI", 9.5F);
            textBox2.Location = new Point(280, 38);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(220, 29);
            textBox2.TabIndex = 4;
            // 
            // lblSearchPhone
            // 
            lblSearchPhone.AutoSize = true;
            lblSearchPhone.Font = new Font("Segoe UI", 9F);
            lblSearchPhone.ForeColor = Color.FromArgb(64, 64, 64);
            lblSearchPhone.Location = new Point(280, 18);
            lblSearchPhone.Name = "lblSearchPhone";
            lblSearchPhone.Size = new Size(50, 20);
            lblSearchPhone.TabIndex = 3;
            lblSearchPhone.Text = "Phone";
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 9.5F);
            textBox1.Location = new Point(24, 38);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(220, 29);
            textBox1.TabIndex = 2;
            // 
            // lblSearchName
            // 
            lblSearchName.AutoSize = true;
            lblSearchName.Font = new Font("Segoe UI", 9F);
            lblSearchName.ForeColor = Color.FromArgb(64, 64, 64);
            lblSearchName.Location = new Point(24, 18);
            lblSearchName.Name = "lblSearchName";
            lblSearchName.Size = new Size(49, 20);
            lblSearchName.TabIndex = 0;
            lblSearchName.Text = "Name";
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.WhiteSmoke;
            pnlSearch.Controls.Add(button1);
            pnlSearch.Controls.Add(lblSearchName);
            pnlSearch.Controls.Add(textBox1);
            pnlSearch.Controls.Add(lblSearchPhone);
            pnlSearch.Controls.Add(textBox2);
            pnlSearch.Controls.Add(lblSearchEmail);
            pnlSearch.Controls.Add(textBox3);
            pnlSearch.Controls.Add(btnAddNewContact);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(0, 88);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Padding = new Padding(20, 12, 20, 12);
            pnlSearch.Size = new Size(1325, 108);
            pnlSearch.TabIndex = 4;
            // 
            // frmListContacts
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1325, 646);
            Controls.Add(dgvAllContacts);
            Controls.Add(pnlSearch);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            Margin = new Padding(4);
            MinimumSize = new Size(900, 500);
            Name = "frmListContacts";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmListContacts";
            Load += frmListContacts_Load;
            Shown += frmListContacts_Shown;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAllContacts).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAllContacts;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label lblSearchEmail;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblSearchPhone;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.Panel pnlSearch;
    }
}
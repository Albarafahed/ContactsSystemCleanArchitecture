namespace ContactsSystemWinForms
{
    partial class frmAddEditContact
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
            pnlHeader = new Panel();
            lblMode = new Label();
            label8 = new Label();
            lblContactID = new Label();
            pnlPhoto = new Panel();
            pictureBox1 = new PictureBox();
            llOpenFileDialog = new LinkLabel();
            llRemoveImage = new LinkLabel();
            pnlActions = new Panel();
            btnSave = new Button();
            btnClose = new Button();
            label1 = new Label();
            label2 = new Label();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();
            label3 = new Label();
            label4 = new Label();
            txtAddress = new TextBox();
            label5 = new Label();
            dtpDateOfBirth = new DateTimePicker();
            label6 = new Label();
            label7 = new Label();
            cbCountry = new ComboBox();
            openFileDialog1 = new OpenFileDialog();
            pnlHeader.SuspendLayout();
            pnlPhoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlActions.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(41, 65, 92);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Controls.Add(label8);
            pnlHeader.Controls.Add(lblContactID);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(660, 68);
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(24, 18);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(216, 35);
            lblMode.TabIndex = 17;
            lblMode.Text = "Add New Contact";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 8.5F);
            label8.ForeColor = Color.FromArgb(180, 195, 210);
            label8.Location = new Point(474, 16);
            label8.Name = "label8";
            label8.Size = new Size(79, 20);
            label8.TabIndex = 18;
            label8.Text = "Contact ID";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblContactID
            // 
            lblContactID.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblContactID.AutoSize = true;
            lblContactID.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblContactID.ForeColor = Color.White;
            lblContactID.Location = new Point(558, 14);
            lblContactID.Name = "lblContactID";
            lblContactID.Size = new Size(34, 23);
            lblContactID.TabIndex = 19;
            lblContactID.Text = "???";
            // 
            // pnlPhoto
            // 
            pnlPhoto.BackColor = Color.FromArgb(247, 249, 251);
            pnlPhoto.Controls.Add(pictureBox1);
            pnlPhoto.Controls.Add(llOpenFileDialog);
            pnlPhoto.Controls.Add(llRemoveImage);
            pnlPhoto.Location = new Point(410, 93);
            pnlPhoto.Name = "pnlPhoto";
            pnlPhoto.Size = new Size(210, 260);
            pnlPhoto.TabIndex = 14;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(14, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(182, 188);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // llOpenFileDialog
            // 
            llOpenFileDialog.ActiveLinkColor = Color.FromArgb(41, 128, 185);
            llOpenFileDialog.AutoSize = true;
            llOpenFileDialog.Font = new Font("Segoe UI", 9.5F);
            llOpenFileDialog.LinkColor = Color.FromArgb(41, 128, 185);
            llOpenFileDialog.Location = new Point(14, 216);
            llOpenFileDialog.Name = "llOpenFileDialog";
            llOpenFileDialog.Size = new Size(79, 21);
            llOpenFileDialog.TabIndex = 1;
            llOpenFileDialog.TabStop = true;
            llOpenFileDialog.Text = "Set Image";
            llOpenFileDialog.LinkClicked += llOpenFileDialog_LinkClicked;
            // 
            // llRemoveImage
            // 
            llRemoveImage.ActiveLinkColor = Color.FromArgb(192, 57, 43);
            llRemoveImage.AutoSize = true;
            llRemoveImage.Font = new Font("Segoe UI", 9.5F);
            llRemoveImage.LinkColor = Color.FromArgb(192, 57, 43);
            llRemoveImage.Location = new Point(108, 216);
            llRemoveImage.Name = "llRemoveImage";
            llRemoveImage.Size = new Size(67, 21);
            llRemoveImage.TabIndex = 2;
            llRemoveImage.TabStop = true;
            llRemoveImage.Text = "Remove";
            llRemoveImage.Visible = false;
            llRemoveImage.LinkClicked += llRemoveImage_LinkClicked;
            // 
            // pnlActions
            // 
            pnlActions.BackColor = Color.FromArgb(247, 249, 251);
            pnlActions.Controls.Add(btnSave);
            pnlActions.Controls.Add(btnClose);
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Location = new Point(0, 540);
            pnlActions.Name = "pnlActions";
            pnlActions.Size = new Size(660, 76);
            pnlActions.TabIndex = 16;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(39, 174, 96);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(410, 16);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(115, 44);
            btnSave.TabIndex = 15;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(224, 227, 230);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 9.5F);
            btnClose.ForeColor = Color.FromArgb(64, 64, 64);
            btnClose.Location = new Point(283, 16);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(115, 44);
            btnClose.TabIndex = 16;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.5F);
            label1.ForeColor = Color.FromArgb(64, 64, 64);
            label1.Location = new Point(24, 96);
            label1.Name = "label1";
            label1.Size = new Size(86, 21);
            label1.TabIndex = 0;
            label1.Text = "First Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.5F);
            label2.ForeColor = Color.FromArgb(64, 64, 64);
            label2.Location = new Point(24, 140);
            label2.Name = "label2";
            label2.Size = new Size(84, 21);
            label2.TabIndex = 2;
            label2.Text = "Last Name";
            // 
            // txtFirstName
            // 
            txtFirstName.BorderStyle = BorderStyle.FixedSingle;
            txtFirstName.Font = new Font("Segoe UI", 9.5F);
            txtFirstName.Location = new Point(150, 93);
            txtFirstName.MaxLength = 50;
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(220, 29);
            txtFirstName.TabIndex = 1;
            // 
            // txtLastName
            // 
            txtLastName.BorderStyle = BorderStyle.FixedSingle;
            txtLastName.Font = new Font("Segoe UI", 9.5F);
            txtLastName.Location = new Point(150, 137);
            txtLastName.MaxLength = 50;
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(220, 29);
            txtLastName.TabIndex = 3;
            // 
            // txtPhone
            // 
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.Font = new Font("Segoe UI", 9.5F);
            txtPhone.Location = new Point(150, 225);
            txtPhone.MaxLength = 20;
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(220, 29);
            txtPhone.TabIndex = 7;
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 9.5F);
            txtEmail.Location = new Point(150, 181);
            txtEmail.MaxLength = 50;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(220, 29);
            txtEmail.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.5F);
            label3.ForeColor = Color.FromArgb(64, 64, 64);
            label3.Location = new Point(24, 228);
            label3.Name = "label3";
            label3.Size = new Size(54, 21);
            label3.TabIndex = 6;
            label3.Text = "Phone";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.5F);
            label4.ForeColor = Color.FromArgb(64, 64, 64);
            label4.Location = new Point(24, 184);
            label4.Name = "label4";
            label4.Size = new Size(48, 21);
            label4.TabIndex = 4;
            label4.Text = "Email";
            // 
            // txtAddress
            // 
            txtAddress.BorderStyle = BorderStyle.FixedSingle;
            txtAddress.Font = new Font("Segoe UI", 9.5F);
            txtAddress.Location = new Point(150, 357);
            txtAddress.MaxLength = 200;
            txtAddress.Multiline = true;
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(220, 96);
            txtAddress.TabIndex = 13;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9.5F);
            label5.ForeColor = Color.FromArgb(64, 64, 64);
            label5.Location = new Point(24, 360);
            label5.Name = "label5";
            label5.Size = new Size(66, 21);
            label5.TabIndex = 12;
            label5.Text = "Address";
            // 
            // dtpDateOfBirth
            // 
            dtpDateOfBirth.CalendarMonthBackground = Color.White;
            dtpDateOfBirth.Font = new Font("Segoe UI", 9.5F);
            dtpDateOfBirth.Location = new Point(150, 269);
            dtpDateOfBirth.Name = "dtpDateOfBirth";
            dtpDateOfBirth.Size = new Size(220, 29);
            dtpDateOfBirth.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9.5F);
            label6.ForeColor = Color.FromArgb(64, 64, 64);
            label6.Location = new Point(24, 272);
            label6.Name = "label6";
            label6.Size = new Size(100, 21);
            label6.TabIndex = 8;
            label6.Text = "Date Of Birth";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9.5F);
            label7.ForeColor = Color.FromArgb(64, 64, 64);
            label7.Location = new Point(24, 316);
            label7.Name = "label7";
            label7.Size = new Size(66, 21);
            label7.TabIndex = 10;
            label7.Text = "Country";
            // 
            // cbCountry
            // 
            cbCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCountry.Font = new Font("Segoe UI", 9.5F);
            cbCountry.FormattingEnabled = true;
            cbCountry.Location = new Point(150, 313);
            cbCountry.Name = "cbCountry";
            cbCountry.Size = new Size(220, 29);
            cbCountry.TabIndex = 11;
            cbCountry.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmAddEditContact
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(660, 616);
            Controls.Add(pnlPhoto);
            Controls.Add(label1);
            Controls.Add(txtFirstName);
            Controls.Add(label2);
            Controls.Add(txtLastName);
            Controls.Add(label4);
            Controls.Add(txtEmail);
            Controls.Add(label3);
            Controls.Add(txtPhone);
            Controls.Add(label6);
            Controls.Add(dtpDateOfBirth);
            Controls.Add(label7);
            Controls.Add(cbCountry);
            Controls.Add(label5);
            Controls.Add(txtAddress);
            Controls.Add(pnlActions);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddEditContact";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add/Edit Contact";
            Load += frmAddEditContact_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlPhoto.ResumeLayout(false);
            pnlPhoto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlActions.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpDateOfBirth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbCountry;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblContactID;
        private System.Windows.Forms.LinkLabel llOpenFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.LinkLabel llRemoveImage;
        private Panel pnlHeader;
        private Panel pnlPhoto;
        private Panel pnlActions;
    }
}

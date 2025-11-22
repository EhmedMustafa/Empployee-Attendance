namespace Empployee_Attendance
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblHeader;
        private Label lblEmployeeName;
        private Label lblStore;
        private Label lblTime;
        private Label lblDate;
        private Label lblStatus;
        private Label lblShiftInfo;
        private Button btnCheckIn;
        private Button btnCheckOut;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblHeader = new Label();
            lblEmployeeName = new Label();
            lblStore = new Label();
            lblTime = new Label();
            lblDate = new Label();
            lblStatus = new Label();
            lblShiftInfo = new Label();
            btnCheckIn = new Button();
            btnCheckOut = new Button();
            panel1 = new Panel();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            panel2 = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // lblHeader
            // 
            lblHeader.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Location = new Point(15, 58);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(600, 40);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Davamiyyət Sistemi";
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEmployeeName
            // 
            lblEmployeeName.Font = new Font("Segoe UI", 11F);
            lblEmployeeName.ForeColor = Color.White;
            lblEmployeeName.Location = new Point(20, 109);
            lblEmployeeName.Name = "lblEmployeeName";
            lblEmployeeName.Size = new Size(300, 25);
            lblEmployeeName.TabIndex = 1;
            // 
            // lblStore
            // 
            lblStore.Font = new Font("Segoe UI", 11F);
            lblStore.ForeColor = Color.White;
            lblStore.Location = new Point(350, 109);
            lblStore.Name = "lblStore";
            lblStore.Size = new Size(230, 25);
            lblStore.TabIndex = 2;
            lblStore.TextAlign = ContentAlignment.TopRight;
            // 
            // lblTime
            // 
            lblTime.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTime.ForeColor = Color.Lime;
            lblTime.Location = new Point(16, 159);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(600, 35);
            lblTime.TabIndex = 3;
            lblTime.Text = "00:00:00";
            lblTime.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDate
            // 
            lblDate.Font = new Font("Segoe UI", 11F);
            lblDate.ForeColor = Color.White;
            lblDate.Location = new Point(16, 199);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(600, 25);
            lblDate.TabIndex = 4;
            lblDate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(16, 319);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(600, 25);
            lblStatus.TabIndex = 5;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblShiftInfo
            // 
            lblShiftInfo.Font = new Font("Segoe UI", 10F);
            lblShiftInfo.ForeColor = Color.White;
            lblShiftInfo.Location = new Point(16, 349);
            lblShiftInfo.Name = "lblShiftInfo";
            lblShiftInfo.Size = new Size(600, 25);
            lblShiftInfo.TabIndex = 6;
            lblShiftInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnCheckIn
            // 
            btnCheckIn.BackColor = Color.Teal;
            btnCheckIn.Cursor = Cursors.Hand;
            btnCheckIn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCheckIn.ForeColor = SystemColors.ButtonHighlight;
            btnCheckIn.Location = new Point(130, 249);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Size = new Size(150, 50);
            btnCheckIn.TabIndex = 7;
            btnCheckIn.Text = "GİRİŞ ET";
            btnCheckIn.UseVisualStyleBackColor = false;
            btnCheckIn.Click += btnCheckIn_Click;
            // 
            // btnCheckOut
            // 
            btnCheckOut.BackColor = Color.Red;
            btnCheckOut.Cursor = Cursors.Hand;
            btnCheckOut.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCheckOut.ForeColor = SystemColors.ButtonHighlight;
            btnCheckOut.Location = new Point(340, 249);
            btnCheckOut.Name = "btnCheckOut";
            btnCheckOut.Size = new Size(150, 50);
            btnCheckOut.TabIndex = 8;
            btnCheckOut.Text = "ÇIXIŞ ET";
            btnCheckOut.UseVisualStyleBackColor = false;
            btnCheckOut.Click += btnCheckOut_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(10, 90, 131);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(631, 55);
            panel1.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = Cursors.Hand;
            label1.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(592, 13);
            label1.Name = "label1";
            label1.Size = new Size(30, 29);
            label1.TabIndex = 13;
            label1.Text = "X";
            label1.Click += label1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.iyde_beyaz;
            pictureBox1.Location = new Point(4, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(228, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = Properties.Resources.exit;
            pictureBox2.Location = new Point(565, 2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(45, 45);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 12;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(34, 49, 71);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 411);
            panel2.Name = "panel2";
            panel2.Size = new Size(631, 48);
            panel2.TabIndex = 13;
            // 
            // MainForm
            // 
            BackColor = Color.FromArgb(34, 31, 46);
            ClientSize = new Size(631, 459);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(lblHeader);
            Controls.Add(lblEmployeeName);
            Controls.Add(lblStore);
            Controls.Add(lblTime);
            Controls.Add(lblDate);
            Controls.Add(lblStatus);
            Controls.Add(lblShiftInfo);
            Controls.Add(btnCheckIn);
            Controls.Add(btnCheckOut);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Davamiyyət Sistemi";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);

        }
        #endregion

        //private Timer timer1;
        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label1;
        private PictureBox pictureBox2;
        private Panel panel2;
        private System.Windows.Forms.Timer timer1;
    }
}
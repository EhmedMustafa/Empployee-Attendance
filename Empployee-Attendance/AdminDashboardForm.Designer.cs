namespace Empployee_Attendance
{
    public partial class AdminDashboardForm
    {

        private System.ComponentModel.IContainer components = null;

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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            lblTitle = new Label();
            lblTime = new Label();
            tabControl = new TabControl();
            tabEmployees = new TabPage();
            dgvEmployees = new DataGridView();
            txtSearchEmployee = new TextBox();
            btnAddEmployee = new Button();
            btnEditEmployee = new Button();
            btnDeleteEmployee = new Button();
            tabAttendance = new TabPage();
            FilterStatus = new ComboBox();
            panel1 = new Panel();
            txtaxtar = new TextBox();
            dgvAttendance = new DataGridView();
            cmbStore = new ComboBox();
            label1 = new Label();
            dtpDate = new DateTimePicker();
            btnViewAttendance = new Button();
            timer = new System.Windows.Forms.Timer(components);
            tabControl.SuspendLayout();
            tabEmployees.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
            tabAttendance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.FlatStyle = FlatStyle.Flat;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Blue;
            lblTitle.Location = new Point(20, 15);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(307, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Admin Davamiyyət Paneli";
            // 
            // lblTime
            // 
            lblTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTime.ForeColor = Color.Fuchsia;
            lblTime.Location = new Point(1582, 25);
            lblTime.Margin = new Padding(4, 0, 4, 0);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(72, 21);
            lblTime.TabIndex = 1;
            lblTime.Text = "00:00:00";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabEmployees);
            tabControl.Controls.Add(tabAttendance);
            tabControl.Font = new Font("Segoe UI", 10F);
            tabControl.Location = new Point(20, 60);
            tabControl.Margin = new Padding(4, 3, 4, 3);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1646, 658);
            tabControl.TabIndex = 2;
            tabControl.Enter += tabControl_Enter;
            // 
            // tabEmployees
            // 
            tabEmployees.BackColor = Color.WhiteSmoke;
            tabEmployees.Controls.Add(dgvEmployees);
            tabEmployees.Controls.Add(txtSearchEmployee);
            tabEmployees.Controls.Add(btnAddEmployee);
            tabEmployees.Controls.Add(btnEditEmployee);
            tabEmployees.Controls.Add(btnDeleteEmployee);
            tabEmployees.Font = new Font("Segoe UI", 10F);
            tabEmployees.Location = new Point(4, 26);
            tabEmployees.Margin = new Padding(4, 3, 4, 3);
            tabEmployees.Name = "tabEmployees";
            tabEmployees.Padding = new Padding(4, 3, 4, 3);
            tabEmployees.RightToLeft = RightToLeft.No;
            tabEmployees.Size = new Size(1638, 628);
            tabEmployees.TabIndex = 0;
            tabEmployees.Text = "İşçilər";
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.BackgroundColor = Color.White;
            dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmployees.Location = new Point(20, 80);
            dgvEmployees.Margin = new Padding(4, 3, 4, 3);
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersVisible = false;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Size = new Size(996, 370);
            dgvEmployees.TabIndex = 4;
            // 
            // txtSearchEmployee
            // 
            txtSearchEmployee.Font = new Font("Segoe UI", 10F);
            txtSearchEmployee.Location = new Point(20, 30);
            txtSearchEmployee.Margin = new Padding(4, 3, 4, 3);
            txtSearchEmployee.Name = "txtSearchEmployee";
            txtSearchEmployee.Size = new Size(300, 25);
            txtSearchEmployee.TabIndex = 0;
            // 
            // btnAddEmployee
            // 
            btnAddEmployee.BackColor = Color.MediumSeaGreen;
            btnAddEmployee.FlatStyle = FlatStyle.Flat;
            btnAddEmployee.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddEmployee.ForeColor = Color.White;
            btnAddEmployee.Location = new Point(340, 28);
            btnAddEmployee.Margin = new Padding(4, 3, 4, 3);
            btnAddEmployee.Name = "btnAddEmployee";
            btnAddEmployee.Size = new Size(100, 30);
            btnAddEmployee.TabIndex = 1;
            btnAddEmployee.Text = "Əlavə et";
            btnAddEmployee.UseVisualStyleBackColor = false;
            // 
            // btnEditEmployee
            // 
            btnEditEmployee.BackColor = Color.Goldenrod;
            btnEditEmployee.FlatStyle = FlatStyle.Flat;
            btnEditEmployee.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEditEmployee.ForeColor = Color.White;
            btnEditEmployee.Location = new Point(450, 28);
            btnEditEmployee.Margin = new Padding(4, 3, 4, 3);
            btnEditEmployee.Name = "btnEditEmployee";
            btnEditEmployee.Size = new Size(100, 30);
            btnEditEmployee.TabIndex = 2;
            btnEditEmployee.Text = "Redaktə et";
            btnEditEmployee.UseVisualStyleBackColor = false;
            // 
            // btnDeleteEmployee
            // 
            btnDeleteEmployee.BackColor = Color.IndianRed;
            btnDeleteEmployee.FlatStyle = FlatStyle.Flat;
            btnDeleteEmployee.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDeleteEmployee.ForeColor = Color.White;
            btnDeleteEmployee.Location = new Point(560, 28);
            btnDeleteEmployee.Margin = new Padding(4, 3, 4, 3);
            btnDeleteEmployee.Name = "btnDeleteEmployee";
            btnDeleteEmployee.Size = new Size(100, 30);
            btnDeleteEmployee.TabIndex = 3;
            btnDeleteEmployee.Text = "Sil";
            btnDeleteEmployee.UseVisualStyleBackColor = false;
            // 
            // tabAttendance
            // 
            tabAttendance.BackColor = Color.LightBlue;
            tabAttendance.Controls.Add(FilterStatus);
            tabAttendance.Controls.Add(panel1);
            tabAttendance.Controls.Add(txtaxtar);
            tabAttendance.Controls.Add(dgvAttendance);
            tabAttendance.Controls.Add(cmbStore);
            tabAttendance.Controls.Add(label1);
            tabAttendance.Controls.Add(dtpDate);
            tabAttendance.Controls.Add(btnViewAttendance);
            tabAttendance.Location = new Point(4, 26);
            tabAttendance.Margin = new Padding(4, 3, 4, 3);
            tabAttendance.Name = "tabAttendance";
            tabAttendance.Padding = new Padding(4, 3, 4, 3);
            tabAttendance.Size = new Size(1638, 628);
            tabAttendance.TabIndex = 1;
            tabAttendance.Text = "Davranış Cədvəli";
            tabAttendance.Enter += tabAttendance_Enter;
            // 
            // FilterStatus
            // 
            FilterStatus.FormattingEnabled = true;
            FilterStatus.Location = new Point(1438, 40);
            FilterStatus.Margin = new Padding(4, 3, 4, 3);
            FilterStatus.Name = "FilterStatus";
            FilterStatus.Size = new Size(159, 25);
            FilterStatus.TabIndex = 7;
            FilterStatus.SelectedIndexChanged += FilterStatus_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(0, 0, 192);
            panel1.Location = new Point(834, 68);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(315, 1);
            panel1.TabIndex = 6;
            // 
            // txtaxtar
            // 
            txtaxtar.BackColor = Color.LightBlue;
            txtaxtar.BorderStyle = BorderStyle.None;
            txtaxtar.Font = new Font("Segoe UI", 14F, FontStyle.Italic, GraphicsUnit.Point, 1, true);
            txtaxtar.Location = new Point(834, 36);
            txtaxtar.Margin = new Padding(4, 3, 4, 3);
            txtaxtar.Multiline = true;
            txtaxtar.Name = "txtaxtar";
            txtaxtar.Size = new Size(310, 29);
            txtaxtar.TabIndex = 5;
            txtaxtar.Text = "axtar....";
            txtaxtar.Click += txtaxtar_Click;
            txtaxtar.TextChanged += txtaxtar_TextChanged;
            // 
            // dgvAttendance
            // 
            dgvAttendance.AllowUserToAddRows = false;
            dgvAttendance.AllowUserToDeleteRows = false;
            dgvAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAttendance.BackgroundColor = Color.DarkCyan;
            dgvAttendance.BorderStyle = BorderStyle.None;
            dgvAttendance.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvAttendance.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgvAttendance.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(128, 128, 255);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(128, 128, 255);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvAttendance.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvAttendance.ColumnHeadersHeight = 50;
            dgvAttendance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.LightBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvAttendance.DefaultCellStyle = dataGridViewCellStyle2;
            dgvAttendance.EnableHeadersVisualStyles = false;
            dgvAttendance.Location = new Point(20, 90);
            dgvAttendance.Margin = new Padding(4, 3, 4, 3);
            dgvAttendance.Name = "dgvAttendance";
            dgvAttendance.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(192, 255, 255);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(187, 220, 219);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(187, 220, 219);
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvAttendance.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvAttendance.RowHeadersVisible = false;
            dgvAttendance.RowHeadersWidth = 40;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 12F);
            dgvAttendance.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvAttendance.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAttendance.RowTemplate.Height = 35;
            dgvAttendance.RowTemplate.Resizable = DataGridViewTriState.True;
            dgvAttendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttendance.Size = new Size(1591, 526);
            dgvAttendance.TabIndex = 4;
            dgvAttendance.CellFormatting += dgvAttendance_CellFormatting;
            // 
            // cmbStore
            // 
            cmbStore.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStore.Font = new Font("Segoe UI", 10F);
            cmbStore.FormattingEnabled = true;
            cmbStore.Items.AddRange(new object[] { "---Hamsi---" });
            cmbStore.Location = new Point(20, 40);
            cmbStore.Margin = new Padding(4, 3, 4, 3);
            cmbStore.Name = "cmbStore";
            cmbStore.Size = new Size(200, 25);
            cmbStore.TabIndex = 0;
            cmbStore.SelectedIndexChanged += cmbStore_SelectedIndexChanged;
            cmbStore.Enter += cmbStore_Enter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(240, 44);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(38, 19);
            label1.TabIndex = 1;
            label1.Text = "Tarix:";
            // 
            // dtpDate
            // 
            dtpDate.Font = new Font("Segoe UI", 10F);
            dtpDate.Location = new Point(294, 40);
            dtpDate.Margin = new Padding(4, 3, 4, 3);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(277, 25);
            dtpDate.TabIndex = 2;
            // 
            // btnViewAttendance
            // 
            btnViewAttendance.BackColor = Color.SteelBlue;
            btnViewAttendance.FlatStyle = FlatStyle.Flat;
            btnViewAttendance.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnViewAttendance.ForeColor = Color.White;
            btnViewAttendance.Location = new Point(601, 40);
            btnViewAttendance.Margin = new Padding(4, 3, 4, 3);
            btnViewAttendance.Name = "btnViewAttendance";
            btnViewAttendance.Size = new Size(150, 30);
            btnViewAttendance.TabIndex = 3;
            btnViewAttendance.Text = "Cədvəli göstər";
            btnViewAttendance.UseVisualStyleBackColor = false;
            btnViewAttendance.Click += btnViewAttendance_Click;
            // 
            // timer
            // 
            timer.Interval = 1000;
            // 
            // AdminDashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightBlue;
            ClientSize = new Size(1701, 732);
            Controls.Add(tabControl);
            Controls.Add(lblTime);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "AdminDashboardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Dashboard";
            FormClosing += AdminDashboardForm_FormClosing;
            Load += AdminDashboardForm_Load;
            Enter += AdminDashboardForm_Enter;
            tabControl.ResumeLayout(false);
            tabEmployees.ResumeLayout(false);
            tabEmployees.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
            tabAttendance.ResumeLayout(false);
            tabAttendance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabEmployees;
        private System.Windows.Forms.TabPage tabAttendance;
        private System.Windows.Forms.DataGridView dgvEmployees;
        private System.Windows.Forms.TextBox txtSearchEmployee;
        private System.Windows.Forms.Button btnAddEmployee;
        private System.Windows.Forms.Button btnEditEmployee;
        private System.Windows.Forms.Button btnDeleteEmployee;
        private System.Windows.Forms.DataGridView dgvAttendance;
        private System.Windows.Forms.ComboBox cmbStore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnViewAttendance;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox txtaxtar;
        private System.Windows.Forms.ComboBox FilterStatus;
    }
}
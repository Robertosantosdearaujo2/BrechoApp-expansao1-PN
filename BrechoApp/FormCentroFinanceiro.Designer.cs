namespace BrechoApp
{
    partial class FormCentroFinanceiro
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvCentros = new System.Windows.Forms.DataGridView();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnFechar = new System.Windows.Forms.Button();

            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSaldo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAtivo = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvCentros)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // GRID
            // ============================================================
            this.dgvCentros.AllowUserToAddRows = false;
            this.dgvCentros.AllowUserToDeleteRows = false;
            this.dgvCentros.ReadOnly = true;
            this.dgvCentros.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCentros.MultiSelect = false;
            this.dgvCentros.RowHeadersVisible = false;
            this.dgvCentros.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.dgvCentros.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colId,
                this.colNome,
                this.colTipo,
                this.colSaldo,
                this.colAtivo
            });

            this.dgvCentros.Location = new System.Drawing.Point(10, 10);
            this.dgvCentros.Size = new System.Drawing.Size(560, 300);

            // ============================================================
            // BOTÕES
            // ============================================================
            this.btnExportarExcel.Text = "Exportar para Excel";
            this.btnExportarExcel.Location = new System.Drawing.Point(10, 320);
            this.btnExportarExcel.Size = new System.Drawing.Size(160, 35);
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);

            this.btnFechar.Text = "Fechar";
            this.btnFechar.Location = new System.Drawing.Point(470, 320);
            this.btnFechar.Size = new System.Drawing.Size(100, 35);
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);

            // ============================================================
            // FORM
            // ============================================================
            this.ClientSize = new System.Drawing.Size(580, 370);
            this.Controls.Add(this.dgvCentros);
            this.Controls.Add(this.btnExportarExcel);
            this.Controls.Add(this.btnFechar);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Centros Financeiros";

            ((System.ComponentModel.ISupportInitialize)(this.dgvCentros)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCentros;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnFechar;

        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNome;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSaldo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAtivo;
    }
}
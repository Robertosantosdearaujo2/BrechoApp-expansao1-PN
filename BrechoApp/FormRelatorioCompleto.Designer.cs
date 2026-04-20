namespace BrechoApp
{
    partial class FormRelatorioCompleto
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvRelatorioCompleto;
        private System.Windows.Forms.Label lblTotalGeralCompleto;
        private System.Windows.Forms.Button btnAtualizarCompleto;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvRelatorioCompleto = new System.Windows.Forms.DataGridView();
            this.lblTotalGeralCompleto = new System.Windows.Forms.Label();
            this.btnAtualizarCompleto = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatorioCompleto)).BeginInit();
            this.SuspendLayout();

            // dgvRelatorioCompleto
            this.dgvRelatorioCompleto.AllowUserToAddRows = false;
            this.dgvRelatorioCompleto.AllowUserToDeleteRows = false;
            this.dgvRelatorioCompleto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRelatorioCompleto.Location = new System.Drawing.Point(12, 12);
            this.dgvRelatorioCompleto.Name = "dgvRelatorioCompleto";
            this.dgvRelatorioCompleto.ReadOnly = true;
            this.dgvRelatorioCompleto.Size = new System.Drawing.Size(650, 250);
            this.dgvRelatorioCompleto.TabIndex = 0;

            // lblTotalGeralCompleto
            this.lblTotalGeralCompleto.AutoSize = true;
            this.lblTotalGeralCompleto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalGeralCompleto.Location = new System.Drawing.Point(12, 270);
            this.lblTotalGeralCompleto.Name = "lblTotalGeralCompleto";
            this.lblTotalGeralCompleto.Size = new System.Drawing.Size(140, 19);
            this.lblTotalGeralCompleto.TabIndex = 1;
            this.lblTotalGeralCompleto.Text = "Total Geral: R$ 0,00";

            // btnAtualizarCompleto
            this.btnAtualizarCompleto.Location = new System.Drawing.Point(570, 268);
            this.btnAtualizarCompleto.Name = "btnAtualizarCompleto";
            this.btnAtualizarCompleto.Size = new System.Drawing.Size(92, 23);
            this.btnAtualizarCompleto.TabIndex = 2;
            this.btnAtualizarCompleto.Text = "Atualizar";
            this.btnAtualizarCompleto.UseVisualStyleBackColor = true;
            this.btnAtualizarCompleto.Click += new System.EventHandler(this.btnAtualizarCompleto_Click);

            // FormRelatorioCompleto
            this.ClientSize = new System.Drawing.Size(674, 561);
            this.Controls.Add(this.btnAtualizarCompleto);
            this.Controls.Add(this.lblTotalGeralCompleto);
            this.Controls.Add(this.dgvRelatorioCompleto);
            this.Name = "FormRelatorioCompleto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatório Financeiro Completo";
            this.Load += new System.EventHandler(this.FormRelatorioCompleto_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatorioCompleto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

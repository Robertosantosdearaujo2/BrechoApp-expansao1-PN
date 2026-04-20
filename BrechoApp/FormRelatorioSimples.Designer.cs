namespace BrechoApp
{
    partial class FormRelatorioSimples
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvRelatorioSimples;
        private System.Windows.Forms.Label lblTotalGeralSimples;
        private System.Windows.Forms.Button btnAtualizarSimples;

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
            this.dgvRelatorioSimples = new System.Windows.Forms.DataGridView();
            this.lblTotalGeralSimples = new System.Windows.Forms.Label();
            this.btnAtualizarSimples = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatorioSimples)).BeginInit();
            this.SuspendLayout();

            // 
            // dgvRelatorioSimples
            // 
            this.dgvRelatorioSimples.AllowUserToAddRows = false;
            this.dgvRelatorioSimples.AllowUserToDeleteRows = false;
            this.dgvRelatorioSimples.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRelatorioSimples.Location = new System.Drawing.Point(12, 12);
            this.dgvRelatorioSimples.Name = "dgvRelatorioSimples";
            this.dgvRelatorioSimples.ReadOnly = true;
            this.dgvRelatorioSimples.Size = new System.Drawing.Size(460, 250);
            this.dgvRelatorioSimples.TabIndex = 0;

            // 
            // lblTotalGeralSimples
            // 
            this.lblTotalGeralSimples.AutoSize = true;
            this.lblTotalGeralSimples.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalGeralSimples.Location = new System.Drawing.Point(12, 270);
            this.lblTotalGeralSimples.Name = "lblTotalGeralSimples";
            this.lblTotalGeralSimples.Size = new System.Drawing.Size(140, 19);
            this.lblTotalGeralSimples.TabIndex = 1;
            this.lblTotalGeralSimples.Text = "Total Geral: R$ 0,00";

            // 
            // btnAtualizarSimples
            // 
            this.btnAtualizarSimples.Location = new System.Drawing.Point(380, 268);
            this.btnAtualizarSimples.Name = "btnAtualizarSimples";
            this.btnAtualizarSimples.Size = new System.Drawing.Size(92, 23);
            this.btnAtualizarSimples.TabIndex = 2;
            this.btnAtualizarSimples.Text = "Atualizar";
            this.btnAtualizarSimples.UseVisualStyleBackColor = true;
            this.btnAtualizarSimples.Click += new System.EventHandler(this.btnAtualizarSimples_Click);

            // 
            // FormRelatorioSimples
            // 
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.Controls.Add(this.btnAtualizarSimples);
            this.Controls.Add(this.lblTotalGeralSimples);
            this.Controls.Add(this.dgvRelatorioSimples);
            this.Name = "FormRelatorioSimples";
            this.Text = "Relatório Financeiro Simples";
            this.Load += new System.EventHandler(this.FormRelatorioSimples_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatorioSimples)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
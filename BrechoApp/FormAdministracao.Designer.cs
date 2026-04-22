namespace BrechoApp
{
    partial class FormAdministracao
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
            this.btnCategoriasFinanceiras = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // btnCategoriasFinanceiras
            // 
            this.btnCategoriasFinanceiras.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCategoriasFinanceiras.Location = new System.Drawing.Point(20, 20);
            this.btnCategoriasFinanceiras.Name = "btnCategoriasFinanceiras";
            this.btnCategoriasFinanceiras.Size = new System.Drawing.Size(260, 45);
            this.btnCategoriasFinanceiras.TabIndex = 0;
            this.btnCategoriasFinanceiras.Text = "Categorias Financeiras";
            this.btnCategoriasFinanceiras.UseVisualStyleBackColor = true;
            this.btnCategoriasFinanceiras.Click += new System.EventHandler(this.btnCategoriasFinanceiras_Click);

            // 
            // btnVoltar
            // 
            this.btnVoltar.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnVoltar.Location = new System.Drawing.Point(20, 80);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(260, 45);
            this.btnVoltar.TabIndex = 1;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);

            // 
            // FormAdministracao
            // 
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.btnCategoriasFinanceiras);
            this.Controls.Add(this.btnVoltar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administração";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnCategoriasFinanceiras;
        private System.Windows.Forms.Button btnVoltar;
    }
}

namespace BrechoApp
{
    partial class FormPagamentoPN
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
            this.lblMensagem = new System.Windows.Forms.Label();
            this.btnFechar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblMensagem.Text = "Funcionalidade em desenvolvimento.";
            this.lblMensagem.Location = new System.Drawing.Point(20, 20);
            this.lblMensagem.Size = new System.Drawing.Size(360, 40);

            this.btnFechar.Text = "Fechar";
            this.btnFechar.Location = new System.Drawing.Point(150, 80);
            this.btnFechar.Size = new System.Drawing.Size(100, 30);
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);

            this.ClientSize = new System.Drawing.Size(400, 130);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.btnFechar);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pagamento/Recebimento de PN";

            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblMensagem;
        private System.Windows.Forms.Button btnFechar;
    }
}

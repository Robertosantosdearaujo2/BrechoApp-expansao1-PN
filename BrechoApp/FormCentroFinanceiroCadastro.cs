using System;
using System.Windows.Forms;
using BrechoApp.Data;
using BrechoApp.Models;

namespace BrechoApp
{
    public partial class FormCentroFinanceiroCadastro : Form
    {
        private readonly CentroFinanceiroRepository _repo;
        private int? _idCentro = null;

        public FormCentroFinanceiroCadastro()
        {
            // This form should no longer be available because CentrosFinanceiros are fixed.
            // Disable initialization to prevent usage. Show message and close if invoked.
            InitializeComponent();
            _repo = new CentroFinanceiroRepository();
            MessageBox.Show("Edição de Centros Financeiros foi desativada. Centros são gerados pelo sistema.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Load += (s, e) => this.Close();
        }

        public FormCentroFinanceiroCadastro(int idCentro)
        {
            // Editing by id is disabled. Inform and close.
            InitializeComponent();
            _repo = new CentroFinanceiroRepository();
            MessageBox.Show("Edição de Centros Financeiros foi desativada. Centros são gerados pelo sistema.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Load += (s, e) => this.Close();
        }

        private void CarregarDados()
        {
            var centro = _repo.BuscarPorId(_idCentro.Value);

            if (centro == null)
            {
                MessageBox.Show("Centro financeiro não encontrado.");
                this.Close();
                return;
            }

            txtNome.Text = centro.Nome;
            cboTipo.SelectedItem = centro.Tipo;
            txtSaldoInicial.Text = centro.SaldoAtual.ToString();
            chkAtivo.Checked = centro.Ativo;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Disabled: saving is not allowed
            MessageBox.Show("Edição/Criação de Centros Financeiros desativada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
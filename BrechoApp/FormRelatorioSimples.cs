using System;
using System.Windows.Forms;
using BrechoApp.Data;

namespace BrechoApp
{
    public partial class FormRelatorioSimples : Form
    {
        public FormRelatorioSimples()
        {
            InitializeComponent();
        }

        private void FormRelatorioSimples_Load(object sender, EventArgs e)
        {
            CarregarRelatorioSimples();
        }

        private void btnAtualizarSimples_Click(object sender, EventArgs e)
        {
            CarregarRelatorioSimples();
        }

        // ============================================================
        // RELATÓRIO SIMPLES
        // ============================================================
        private void CarregarRelatorioSimples()
        {
            var repo = new CentroFinanceiroRepository();
            var lista = repo.Listar();

            dgvRelatorioSimples.Rows.Clear();
            dgvRelatorioSimples.Columns.Clear();

            dgvRelatorioSimples.Columns.Add("Centro", "Centro Financeiro");
            dgvRelatorioSimples.Columns.Add("Saldo", "Saldo Atual");

            decimal totalGeral = 0;

            foreach (var c in lista)
            {
                dgvRelatorioSimples.Rows.Add(
                    c.Nome,
                    c.SaldoAtual.ToString("C2")
                );

                totalGeral += c.SaldoAtual;
            }

            lblTotalGeralSimples.Text = $"Total Geral: {totalGeral:C2}";
        }
    }
}

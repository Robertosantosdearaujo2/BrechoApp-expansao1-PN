using System;
using System.Windows.Forms;
using BrechoApp.Data;

namespace BrechoApp
{
    public partial class FormRelatorioCompleto : Form
    {
        public FormRelatorioCompleto()
        {
            InitializeComponent();
        }

        private void FormRelatorioCompleto_Load(object sender, EventArgs e)
        {
            CarregarRelatorioCompleto();
        }

        private void btnAtualizarCompleto_Click(object sender, EventArgs e)
        {
            CarregarRelatorioCompleto();
        }

        // ============================================================
        // RELATÓRIO COMPLETO DE CENTROS FINANCEIROS
        // ============================================================
        private void CarregarRelatorioCompleto()
        {
            try
            {
                var repo = new CentroFinanceiroRepository();
                var lista = repo.Listar();

                dgvRelatorioCompleto.Rows.Clear();
                dgvRelatorioCompleto.Columns.Clear();

                dgvRelatorioCompleto.Columns.Add("Centro", "Centro Financeiro");
                dgvRelatorioCompleto.Columns.Add("Entradas", "Entradas");
                dgvRelatorioCompleto.Columns.Add("Saidas", "Saídas");
                dgvRelatorioCompleto.Columns.Add("Atual", "Saldo Atual");
                dgvRelatorioCompleto.Columns.Add("Percentual", "% do Total");

                decimal totalGeral = 0;

                foreach (var c in lista)
                    totalGeral += c.SaldoAtual;

                foreach (var c in lista)
                {
                    decimal entradas = 0; // se no futuro você tiver esses valores, é só preencher
                    decimal saidas = 0;

                    decimal percentual = totalGeral > 0
                        ? (c.SaldoAtual / totalGeral) * 100
                        : 0;

                    dgvRelatorioCompleto.Rows.Add(
                        c.Nome,
                        entradas.ToString("C2"),
                        saidas.ToString("C2"),
                        c.SaldoAtual.ToString("C2"),
                        percentual.ToString("N2") + "%"
                    );
                }

                lblTotalGeralCompleto.Text = $"Total Geral: {totalGeral:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao carregar relatório completo:\n\n{ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}

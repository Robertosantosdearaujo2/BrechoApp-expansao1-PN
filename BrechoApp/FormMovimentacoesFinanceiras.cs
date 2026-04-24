using System;
using System.Windows.Forms;
using BrechoApp.Data;
using ClosedXML.Excel;   // Necessário para gerar Excel
using System.IO;

namespace BrechoApp
{
    public partial class FormMovimentacoesFinanceiras : Form
    {
        private readonly MovimentacaoFinanceiraRepository _repository;

        public FormMovimentacoesFinanceiras()
        {
            InitializeComponent();
            _repository = new MovimentacaoFinanceiraRepository();

            dtInicio.Value = DateTime.Today.AddDays(-7);
            dtFim.Value = DateTime.Today;

            CarregarMovimentacoes();
        }

        // ============================================================
        // CARREGAR MOVIMENTAÇÕES NO GRID
        // ============================================================
        private void CarregarMovimentacoes()
        {
            dgvMov.Rows.Clear();

            var lista = _repository.Listar(dtInicio.Value, dtFim.Value);

            foreach (var m in lista)
            {
                dgvMov.Rows.Add(
                    m.IdMovimentacao,
                    m.Data.ToString("dd/MM/yyyy"),
                    m.Tipo,
                    m.Valor.ToString("C2"),
                    m.IdCentroOrigem,
                    m.IdCentroDestino,
                    m.Categoria,
                    m.Descricao
                );
            }
        }

        // ============================================================
        // BOTÃO: FILTRAR
        // ============================================================
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarMovimentacoes();
        }

        // ============================================================
        // BOTÃO: NOVA ENTRADA
        // ============================================================
        private void btnEntrada_Click(object sender, EventArgs e)
        {
            var form = new FormMovimentacaoCadastro("Entrada");
            form.ShowDialog();
            CarregarMovimentacoes();
        }

        // ============================================================
        // BOTÃO: NOVA SAÍDA
        // ============================================================
        private void btnSaida_Click(object sender, EventArgs e)
        {
            var form = new FormMovimentacaoCadastro("Saida");
            form.ShowDialog();
            CarregarMovimentacoes();
        }

        // ============================================================
        // BOTÃO: TRANSFERÊNCIA
        // ============================================================
        private void btnTransferencia_Click(object sender, EventArgs e)
        {
            var form = new FormMovimentacaoCadastro("Transferencia");
            form.ShowDialog();
            CarregarMovimentacoes();
        }

        private void btnPagamentoPN_Click(object sender, EventArgs e)
        {
            var form = new FormPagamentoPN();
            form.ShowDialog();
        }

        // ============================================================
        // BOTÃO: EXPORTAR EXCEL
        // ============================================================
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvMov.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Arquivo Excel (*.xlsx)|*.xlsx";
            sfd.FileName = $"Movimentacoes_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Movimentações");

                    // Cabeçalhos
                    for (int i = 0; i < dgvMov.Columns.Count; i++)
                    {
                        ws.Cell(1, i + 1).Value = dgvMov.Columns[i].HeaderText;
                        ws.Cell(1, i + 1).Style.Font.Bold = true;
                    }

                    // Dados
                    for (int r = 0; r < dgvMov.Rows.Count; r++)
                    {
                        for (int c = 0; c < dgvMov.Columns.Count; c++)
                        {
                            ws.Cell(r + 2, c + 1).Value =
                                dgvMov.Rows[r].Cells[c].Value?.ToString();  // <<< CORREÇÃO AQUI
                        }
                    }

                    ws.Columns().AdjustToContents();
                    workbook.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Arquivo exportado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO: FECHAR
        // ============================================================
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

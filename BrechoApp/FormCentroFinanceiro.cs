using System;
using System.Windows.Forms;
using BrechoApp.Data;
using ClosedXML.Excel;

namespace BrechoApp
{
    public partial class FormCentroFinanceiro : Form
    {
        private readonly CentroFinanceiroRepository _repository;
        // Export requires Microsoft.Office.Interop.Excel or use CSV fallback. We'll implement CSV export to keep dependencies minimal.
        public FormCentroFinanceiro()
        {
            InitializeComponent();
            _repository = new CentroFinanceiroRepository();
            CarregarCentros();
        }

        // ============================================================
        // CARREGAR LISTA
        // ============================================================
        private void CarregarCentros()
        {
            dgvCentros.Rows.Clear();

            var lista = _repository.Listar();

            foreach (var c in lista)
            {
                dgvCentros.Rows.Add(
                    c.IdCentroFinanceiro,
                    c.Nome,
                    c.Tipo,
                    c.SaldoAtual.ToString("C2"),
                    c.Ativo ? "Sim" : "Não"
                );
            }
        }

        // CRUD removed: Centros Financeiros are fixed. Only export is available.

        // ============================================================
        // BOTÃO: EXPORTAR PARA EXCEL (CSV)
        // ============================================================
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var lista = _repository.Listar();

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    sfd.FileName = "centros_financeiros.xlsx";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Centros Financeiros");

                        ws.Cell(1, 1).Value = "CENTROS FINANCEIROS";
                        ws.Cell(1, 1).Style.Font.Bold = true;
                        ws.Cell(1, 1).Style.Font.FontSize = 14;

                        ws.Cell(2, 1).Value = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}";
                        ws.Cell(2, 1).Style.Font.Italic = true;

                        int row = 4;

                        string[] headers = new[] { "Id", "Nome", "Tipo", "SaldoAtual", "Ativo" };

                        for (int i = 0; i < headers.Length; i++)
                            ws.Cell(row, i + 1).Value = headers[i];

                        ws.Range(row, 1, row, headers.Length).Style.Font.Bold = true;

                        row++;

                        foreach (var c in lista)
                        {
                            ws.Cell(row, 1).Value = c.IdCentroFinanceiro;
                            ws.Cell(row, 2).Value = c.Nome;
                            ws.Cell(row, 3).Value = c.Tipo;
                            ws.Cell(row, 4).Value = (double)c.SaldoAtual;
                            ws.Cell(row, 4).Style.NumberFormat.Format = "R$ #,##0.00";
                            ws.Cell(row, 5).Value = c.Ativo ? "Sim" : "Não";
                            row++;
                        }

                        ws.Columns().AdjustToContents();

                        wb.SaveAs(sfd.FileName);
                    }
                }

                MessageBox.Show("Arquivo Excel gerado com sucesso.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
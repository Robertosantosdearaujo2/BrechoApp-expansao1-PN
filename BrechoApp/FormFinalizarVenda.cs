using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrechoApp.Enums;
using BrechoApp.Models;
using BrechoApp.Data;

namespace BrechoApp
{
    public partial class FormFinalizarVenda : Form
    {
        private readonly Venda _venda;

        // Lista final de pagamentos que será retornada para a tela principal
        public List<Pagamento> PagamentosSelecionados { get; private set; } = new List<Pagamento>();

        public FormFinalizarVenda(Venda venda)
        {
            InitializeComponent();
            _venda = venda;

            lblValorTotal.Text = venda.ValorTotalFinal.ToString("C2");

            // ============================================================
            // CARREGA FORMAS DE PAGAMENTO (ENUM)
            // ============================================================
            cmbFormaPagamento.Items.AddRange(Enum.GetNames(typeof(TipoPagamento)));

            // Adiciona a opção especial "Combinado"
            cmbFormaPagamento.Items.Add("Combinado");

            // ============================================================
            // CARREGA CENTROS FINANCEIROS
            // ============================================================
            CarregarCentrosFinanceiros();
        }

        // ============================================================
        // CARREGAR CENTROS FINANCEIROS NO COMBOBOX
        // ============================================================
        private void CarregarCentrosFinanceiros()
        {
            var repo = new CentroFinanceiroRepository();
            var lista = repo.Listar();

            cmbCentroFinanceiro.DataSource = lista;
            cmbCentroFinanceiro.DisplayMember = "Nome";               // o que aparece para o usuário
            cmbCentroFinanceiro.ValueMember = "IdCentroFinanceiro";   // valor interno usado no pagamento
        }

        // ============================================================
        // MOSTRAR SALDO DO CENTRO FINANCEIRO SELECIONADO
        // ============================================================
        private void cmbCentroFinanceiro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCentroFinanceiro.SelectedItem is BrechoApp.Models.CentroFinanceiro c)
            {
                lblSaldoCentro.Text = $"Saldo: {c.SaldoAtual:C2}";
            }
        }

        // ============================================================
        // BOTÃO CONFIRMAR
        // ============================================================
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            var selecionado = cmbFormaPagamento.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selecionado))
            {
                MessageBox.Show("Selecione uma forma de pagamento.");
                return;
            }

            // Verifica se o usuário escolheu um centro financeiro
            if (cmbCentroFinanceiro.SelectedItem == null)
            {
                MessageBox.Show("Selecione um centro financeiro.");
                return;
            }

            // Obtém o ID do centro financeiro selecionado
            int idCentro = (int)cmbCentroFinanceiro.SelectedValue;

            // ============================================================
            // PAGAMENTO COMBINADO
            // ============================================================
            if (selecionado == "Combinado")
            {
                using (var form = new FormPagamentosCombinados(_venda))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // A tela de pagamentos combinados retorna uma lista de pagamentos
                        PagamentosSelecionados = form.Pagamentos;

                        // Adiciona o centro financeiro em cada pagamento
                        foreach (var pag in PagamentosSelecionados)
                            pag.IdCentroFinanceiro = idCentro;

                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                return;
            }

            // ============================================================
            // PAGAMENTO SIMPLES
            // ============================================================
            var tipo = (TipoPagamento)Enum.Parse(typeof(TipoPagamento), selecionado);

            PagamentosSelecionados.Add(new Pagamento
            {
                Tipo = tipo,

                // 🔥 CORREÇÃO AQUI: double → decimal
                Valor = Convert.ToDecimal(_venda.ValorTotalFinal),

                IdCentroFinanceiro = idCentro
            });

            DialogResult = DialogResult.OK;
            Close();
        }

        // ============================================================
        // BOTÃO CANCELAR
        // ============================================================
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

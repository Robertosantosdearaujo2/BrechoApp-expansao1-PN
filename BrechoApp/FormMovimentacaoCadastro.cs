using System;
using System.Windows.Forms;
using BrechoApp.Data;
using BrechoApp.Models;

namespace BrechoApp
{
    public partial class FormMovimentacaoCadastro : Form
    {
        private readonly MovimentacaoFinanceiraRepository _repositoryMov;
        private readonly CentroFinanceiroRepository _repositoryCentros;
        private readonly CategoriaFinanceiraRepository _repositoryCategorias;

        private readonly string _tipoMov; // Entrada, Saida, Transferencia

        public FormMovimentacaoCadastro(string tipoMov)
        {
            InitializeComponent();

            _repositoryMov = new MovimentacaoFinanceiraRepository();
            _repositoryCentros = new CentroFinanceiroRepository();
            _repositoryCategorias = new CategoriaFinanceiraRepository();

            _tipoMov = tipoMov;

            ConfigurarTela();
            CarregarCentros();
            CarregarCategoriasFinanceiras();
        }

        // ============================================================
        // CONFIGURA A TELA DE ACORDO COM O TIPO
        // ============================================================
        private void ConfigurarTela()
        {
            lblTipo.Text = $"Tipo: {_tipoMov}";

            if (_tipoMov == "Entrada")
            {
                lblOrigem.Visible = false;
                cmbOrigem.Visible = false;
            }
            else if (_tipoMov == "Saida")
            {
                lblDestino.Visible = false;
                cmbDestino.Visible = false;
            }
        }

        // ============================================================
        // CARREGAR CATEGORIAS FINANCEIRAS
        // ============================================================
        private void CarregarCategoriasFinanceiras()
        {
            try
            {
                var lista = _repositoryCategorias.ListarTodas();

                // Agrupar por Grupo e adicionar itens agrupados no ComboBox
                cmbCategoria.Items.Clear();

                string currentGroup = null;
                foreach (var c in lista)
                {
                    if (!string.IsNullOrWhiteSpace(c.Grupo) && c.Grupo != currentGroup)
                    {
                        // adicionar entrada de grupo (não selecionável)
                        cmbCategoria.Items.Add(new GroupItem(c.Grupo));
                        currentGroup = c.Grupo;
                    }

                    // adicionar subcategoria (selecionável)
                    cmbCategoria.Items.Add(new CategoryItem(c.Nome, c.Grupo));
                }
            }
            catch
            {
                // Falha silenciosa — mantém comportamento atual
            }
        }

        // ============================================================
        // CARREGAR CENTROS FINANCEIROS
        // ============================================================
        private void CarregarCentros()
        {
            var lista = _repositoryCentros.Listar();

            cmbOrigem.Items.Clear();
            cmbDestino.Items.Clear();

            foreach (var c in lista)
            {
                if (c.Ativo)
                {
                    cmbOrigem.Items.Add(new ComboItem(c.Nome, c.IdCentroFinanceiro));
                    cmbDestino.Items.Add(new ComboItem(c.Nome, c.IdCentroFinanceiro));
                }
            }
        }

        // ============================================================
        // BOTÃO: SALVAR
        // ============================================================
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (numValor.Value <= 0)
            {
                MessageBox.Show("Informe um valor válido.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_tipoMov != "Entrada" && cmbOrigem.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione o centro de origem.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_tipoMov != "Saida" && cmbDestino.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione o centro de destino.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Determinar subcategoria e grupo a partir do item selecionado
            string categoriaSelecionada = string.Empty;
            string grupoSelecionado = string.Empty;

            if (cmbCategoria.SelectedItem is CategoryItem ci)
            {
                categoriaSelecionada = ci.Name;
                grupoSelecionado = ci.Group;
            }
            else
            {
                // Não é uma subcategoria selecionável - impedir salvar
                MessageBox.Show("Selecione uma categoria financeira válida (subcategoria).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
                return;
            }

            var mov = new MovimentacaoFinanceira
            {
                Data = dtData.Value,
                Tipo = _tipoMov,
                Valor = numValor.Value,
                Categoria = categoriaSelecionada,
                Grupo = grupoSelecionado,
                Descricao = txtDescricao.Text.Trim(),
                Previsto = false
            };

            if (_tipoMov == "Entrada")
            {
                mov.IdCentroOrigem = null;
                mov.IdCentroDestino = ((ComboItem)cmbDestino.SelectedItem).Value;
            }
            else if (_tipoMov == "Saida")
            {
                mov.IdCentroOrigem = ((ComboItem)cmbOrigem.SelectedItem).Value;
                mov.IdCentroDestino = null;
            }
            else // Transferência
            {
                mov.IdCentroOrigem = ((ComboItem)cmbOrigem.SelectedItem).Value;
                mov.IdCentroDestino = ((ComboItem)cmbDestino.SelectedItem).Value;
            }

            // Criar automaticamente um pagamento único usando a primeira forma ativa
            try
            {
                var formaRepo = new FormaPagamentoRepository();
                var formas = formaRepo.ListarAtivas();
                if (formas != null && formas.Count > 0)
                {
                    var formaPadrao = formas[0];

                    var pagamento = new MovimentacaoPagamento
                    {
                        IdFormaPagamento = formaPadrao.IdFormaPagamento,
                        IdCentroFinanceiro = (_tipoMov == "Entrada") ? ((ComboItem)cmbDestino.SelectedItem).Value : ((ComboItem)cmbOrigem.SelectedItem).Value,
                        Valor = numValor.Value
                    };

                    mov.Pagamentos = new System.Collections.Generic.List<MovimentacaoPagamento> { pagamento };
                }
            }
            catch
            {
                // Se falhar ao obter forma padrão, deixar sem pagamentos e confiar na validação do repositório
            }

            _repositoryMov.Inserir(mov);

            MessageBox.Show("Movimentação registrada com sucesso!",
                "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        // ============================================================
        // BOTÃO: CANCELAR
        // ============================================================
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Prevent selecting group headers in the ComboBox
        private void cmbCategoria_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbCategoria.SelectedItem is GroupItem)
            {
                // move selection to next selectable item (CategoryItem) if possible
                int idx = cmbCategoria.SelectedIndex;
                int next = -1;
                for (int i = idx + 1; i < cmbCategoria.Items.Count; i++)
                {
                    if (cmbCategoria.Items[i] is CategoryItem) { next = i; break; }
                }

                if (next >= 0)
                    cmbCategoria.SelectedIndex = next;
                else
                    cmbCategoria.SelectedIndex = -1;
            }
        }
    }

    // ============================================================
    // CLASSE AUXILIAR PARA COMBOBOX
    // ============================================================
    public class ComboItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public ComboItem(string text, int value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString() => Text;
    }

    // Item representando um grupo (não selecionável)
    public class GroupItem
    {
        public string GroupName { get; set; }
        public GroupItem(string name) { GroupName = name; }
        public override string ToString() => GroupName;
    }

    // Item representando uma subcategoria (selecionável)
    public class CategoryItem
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public CategoryItem(string name, string group) { Name = name; Group = group; }
        public override string ToString() => "  - " + Name; // visual indent
    }
}

using BrechoApp.Data;
using BrechoApp.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrechoApp
{
    public partial class FormCadastroCategoriasFinanceiras : Form
    {
        private readonly CategoriaFinanceiraRepository _repo = new CategoriaFinanceiraRepository();
        private CategoriaFinanceira _selecionada = null;

        public FormCadastroCategoriasFinanceiras()
        {
            InitializeComponent();
            CarregarGrupos();
            CarregarCategorias();
        }

        private void CarregarGrupos()
        {
            try
            {
                var grupos = _repo.ListarGrupos();
                cboGrupo.Items.Clear();
                foreach (var g in grupos)
                    cboGrupo.Items.Add(g);
            }
            catch
            {
                // ignore
            }
        }

        private void CarregarCategorias()
        {
            var lista = _repo.ListarTodas();
            dgvCategorias.DataSource = lista;

            if (lista != null && lista.Count > 0 && dgvCategorias.Columns.Count > 0)
            {
                if (dgvCategorias.Columns.Contains("Id"))
                    dgvCategorias.Columns["Id"].Visible = false;

                if (dgvCategorias.Columns.Contains("Nome"))
                {
                    dgvCategorias.Columns["Nome"].HeaderText = "Categoria Financeira";
                    dgvCategorias.Columns["Nome"].Width = 300;
                }

                if (dgvCategorias.Columns.Contains("Grupo"))
                {
                    dgvCategorias.Columns["Grupo"].HeaderText = "Grupo";
                    dgvCategorias.Columns["Grupo"].Width = 200;
                }

                if (dgvCategorias.Columns.Contains("DataCriacao"))
                {
                    dgvCategorias.Columns["DataCriacao"].HeaderText = "Data de Criação";
                    dgvCategorias.Columns["DataCriacao"].Width = 150;
                }
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Digite o nome da categoria.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            var grupo = string.IsNullOrWhiteSpace(cboGrupo.Text) ? null : cboGrupo.Text.Trim();

            if (_repo.Existe(txtNome.Text.Trim()))
            {
                MessageBox.Show("Esta categoria já existe.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            var cat = new CategoriaFinanceira
            {
                Nome = txtNome.Text.Trim(),
                Grupo = grupo ?? string.Empty,
                DataCriacao = DateTime.Now
            };

            _repo.Adicionar(cat);
            MessageBox.Show("Categoria financeira adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtNome.Clear();
            CarregarGrupos();
            CarregarCategorias();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_selecionada == null)
            {
                MessageBox.Show("Selecione uma categoria para editar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Digite o nome da categoria.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            if (_repo.Existe(txtNome.Text.Trim(), _selecionada.Id))
            {
                MessageBox.Show("Já existe outra categoria com este nome.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            _selecionada.Nome = txtNome.Text.Trim();
            _selecionada.Grupo = string.IsNullOrWhiteSpace(cboGrupo.Text) ? string.Empty : cboGrupo.Text.Trim();
            _repo.Atualizar(_selecionada);

            MessageBox.Show("Categoria financeira atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtNome.Clear();
            cboGrupo.Text = string.Empty;
            _selecionada = null;
            CarregarGrupos();
            CarregarCategorias();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (_selecionada == null)
            {
                MessageBox.Show("Selecione uma categoria para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var res = MessageBox.Show($"Deseja realmente excluir a categoria '{_selecionada.Nome}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                _repo.Excluir(_selecionada.Id);
                MessageBox.Show("Categoria excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Clear();
                cboGrupo.Text = string.Empty;
                _selecionada = null;
                CarregarGrupos();
                CarregarCategorias();
            }
        }

        private void dgvCategorias_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow != null)
            {
                _selecionada = (CategoriaFinanceira)dgvCategorias.CurrentRow.DataBoundItem;
                txtNome.Text = _selecionada.Nome;
                cboGrupo.Text = _selecionada.Grupo;
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

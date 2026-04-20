namespace BrechoApp
{
    partial class FormRelatoriosGerenciais
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnRelatorioVendasMes = new System.Windows.Forms.Button();
            this.btnRelatorioAnaliticoVendas = new System.Windows.Forms.Button();
            this.btnListarProdutosDisponiveis = new System.Windows.Forms.Button();

            // === NOVOS BOTÕES ===
            this.btnRelatorioFinanceiroSimples = new System.Windows.Forms.Button();
            this.btnRelatorioFinanceiroCompleto = new System.Windows.Forms.Button();

            this.btnRelatorioCaixa = new System.Windows.Forms.Button();
            this.btnRelatorioLucro = new System.Windows.Forms.Button();
            this.btnRelatorioFinanceiro = new System.Windows.Forms.Button();
            this.btnRelatorioComissoes = new System.Windows.Forms.Button();
            this.btnRelatorioProdutosDevolvidos = new System.Windows.Forms.Button();
            this.btnRelatorioEstoque = new System.Windows.Forms.Button();
            this.btnRelatorioFornecedores = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(360, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Relatórios Gerenciais";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // btnRelatorioVendasMes
            this.btnRelatorioVendasMes.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioVendasMes.Location = new System.Drawing.Point(20, 70);
            this.btnRelatorioVendasMes.Name = "btnRelatorioVendasMes";
            this.btnRelatorioVendasMes.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioVendasMes.TabIndex = 1;
            this.btnRelatorioVendasMes.Text = "Relatório de Vendas do Mês";
            this.btnRelatorioVendasMes.UseVisualStyleBackColor = true;
            this.btnRelatorioVendasMes.Click += new System.EventHandler(this.btnRelatorioVendasMes_Click);

            // btnRelatorioAnaliticoVendas
            this.btnRelatorioAnaliticoVendas.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioAnaliticoVendas.Location = new System.Drawing.Point(20, 130);
            this.btnRelatorioAnaliticoVendas.Name = "btnRelatorioAnaliticoVendas";
            this.btnRelatorioAnaliticoVendas.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioAnaliticoVendas.TabIndex = 2;
            this.btnRelatorioAnaliticoVendas.Text = "Relatório Analítico de Vendas";
            this.btnRelatorioAnaliticoVendas.UseVisualStyleBackColor = true;
            this.btnRelatorioAnaliticoVendas.Click += new System.EventHandler(this.btnRelatorioAnaliticoVendas_Click);

            // btnListarProdutosDisponiveis
            this.btnListarProdutosDisponiveis.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnListarProdutosDisponiveis.Location = new System.Drawing.Point(20, 190);
            this.btnListarProdutosDisponiveis.Name = "btnListarProdutosDisponiveis";
            this.btnListarProdutosDisponiveis.Size = new System.Drawing.Size(360, 50);
            this.btnListarProdutosDisponiveis.TabIndex = 3;
            this.btnListarProdutosDisponiveis.Text = "Listar Produtos Disponíveis";
            this.btnListarProdutosDisponiveis.UseVisualStyleBackColor = true;
            this.btnListarProdutosDisponiveis.Click += new System.EventHandler(this.btnListarProdutosDisponiveis_Click);

            // btnRelatorioFinanceiroSimples
            this.btnRelatorioFinanceiroSimples.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioFinanceiroSimples.Location = new System.Drawing.Point(20, 250);
            this.btnRelatorioFinanceiroSimples.Name = "btnRelatorioFinanceiroSimples";
            this.btnRelatorioFinanceiroSimples.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioFinanceiroSimples.TabIndex = 4;
            this.btnRelatorioFinanceiroSimples.Text = "Relatório Financeiro Simples";
            this.btnRelatorioFinanceiroSimples.UseVisualStyleBackColor = true;
            this.btnRelatorioFinanceiroSimples.Click += new System.EventHandler(this.btnRelatorioFinanceiroSimples_Click);

            // btnRelatorioFinanceiroCompleto
            this.btnRelatorioFinanceiroCompleto.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioFinanceiroCompleto.Location = new System.Drawing.Point(20, 310);
            this.btnRelatorioFinanceiroCompleto.Name = "btnRelatorioFinanceiroCompleto";
            this.btnRelatorioFinanceiroCompleto.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioFinanceiroCompleto.TabIndex = 5;
            this.btnRelatorioFinanceiroCompleto.Text = "Relatório Financeiro Completo";
            this.btnRelatorioFinanceiroCompleto.UseVisualStyleBackColor = true;
            this.btnRelatorioFinanceiroCompleto.Click += new System.EventHandler(this.btnRelatorioFinanceiroCompleto_Click);

            // btnRelatorioCaixa
            this.btnRelatorioCaixa.Enabled = false;
            this.btnRelatorioCaixa.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioCaixa.Location = new System.Drawing.Point(20, 370);
            this.btnRelatorioCaixa.Name = "btnRelatorioCaixa";
            this.btnRelatorioCaixa.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioCaixa.TabIndex = 6;
            this.btnRelatorioCaixa.Text = "Relatório de Caixa (Em Desenvolvimento)";
            this.btnRelatorioCaixa.UseVisualStyleBackColor = true;
            this.btnRelatorioCaixa.Click += new System.EventHandler(this.btnRelatorioCaixa_Click);

            // btnRelatorioLucro
            this.btnRelatorioLucro.Enabled = false;
            this.btnRelatorioLucro.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioLucro.Location = new System.Drawing.Point(20, 430);
            this.btnRelatorioLucro.Name = "btnRelatorioLucro";
            this.btnRelatorioLucro.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioLucro.TabIndex = 7;
            this.btnRelatorioLucro.Text = "Relatório de Lucro (Em Desenvolvimento)";
            this.btnRelatorioLucro.UseVisualStyleBackColor = true;
            this.btnRelatorioLucro.Click += new System.EventHandler(this.btnRelatorioLucro_Click);

            // btnRelatorioFinanceiro
            this.btnRelatorioFinanceiro.Enabled = false;
            this.btnRelatorioFinanceiro.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioFinanceiro.Location = new System.Drawing.Point(20, 490);
            this.btnRelatorioFinanceiro.Name = "btnRelatorioFinanceiro";
            this.btnRelatorioFinanceiro.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioFinanceiro.TabIndex = 8;
            this.btnRelatorioFinanceiro.Text = "Relatório Financeiro (Em Desenvolvimento)";
            this.btnRelatorioFinanceiro.UseVisualStyleBackColor = true;

            // btnRelatorioComissoes
            this.btnRelatorioComissoes.Enabled = false;
            this.btnRelatorioComissoes.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioComissoes.Location = new System.Drawing.Point(20, 550);
            this.btnRelatorioComissoes.Name = "btnRelatorioComissoes";
            this.btnRelatorioComissoes.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioComissoes.TabIndex = 9;
            this.btnRelatorioComissoes.Text = "Relatório de Comissões (Em Desenvolvimento)";
            this.btnRelatorioComissoes.UseVisualStyleBackColor = true;
            this.btnRelatorioComissoes.Click += new System.EventHandler(this.btnRelatorioComissoes_Click);

            // btnRelatorioProdutosDevolvidos
            this.btnRelatorioProdutosDevolvidos.Enabled = false;
            this.btnRelatorioProdutosDevolvidos.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioProdutosDevolvidos.Location = new System.Drawing.Point(20, 610);
            this.btnRelatorioProdutosDevolvidos.Name = "btnRelatorioProdutosDevolvidos";
            this.btnRelatorioProdutosDevolvidos.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioProdutosDevolvidos.TabIndex = 10;
            this.btnRelatorioProdutosDevolvidos.Text = "Relatório de Produtos Devolvidos (Em Desenvolvimento)";
            this.btnRelatorioProdutosDevolvidos.UseVisualStyleBackColor = true;
            this.btnRelatorioProdutosDevolvidos.Click += new System.EventHandler(this.btnRelatorioProdutosDevolvidos_Click);

            // btnRelatorioEstoque
            this.btnRelatorioEstoque.Enabled = false;
            this.btnRelatorioEstoque.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioEstoque.Location = new System.Drawing.Point(20, 670);
            this.btnRelatorioEstoque.Name = "btnRelatorioEstoque";
            this.btnRelatorioEstoque.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioEstoque.TabIndex = 11;
            this.btnRelatorioEstoque.Text = "Relatório de Estoque (Em Desenvolvimento)";
            this.btnRelatorioEstoque.UseVisualStyleBackColor = true;
            this.btnRelatorioEstoque.Click += new System.EventHandler(this.btnRelatorioEstoque_Click);

            // btnRelatorioFornecedores
            this.btnRelatorioFornecedores.Enabled = false;
            this.btnRelatorioFornecedores.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRelatorioFornecedores.Location = new System.Drawing.Point(20, 730);
            this.btnRelatorioFornecedores.Name = "btnRelatorioFornecedores";
            this.btnRelatorioFornecedores.Size = new System.Drawing.Size(360, 50);
            this.btnRelatorioFornecedores.TabIndex = 12;
            this.btnRelatorioFornecedores.Text = "Relatório de Fornecedores (Em Desenvolvimento)";
            this.btnRelatorioFornecedores.UseVisualStyleBackColor = true;
            this.btnRelatorioFornecedores.Click += new System.EventHandler(this.btnRelatorioFornecedores_Click);

            // btnVoltar
            this.btnVoltar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnVoltar.Location = new System.Drawing.Point(20, 790);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(360, 40);
            this.btnVoltar.TabIndex = 13;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);

            // FormRelatoriosGerenciais
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 860);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.btnRelatorioFornecedores);
            this.Controls.Add(this.btnRelatorioEstoque);
            this.Controls.Add(this.btnRelatorioProdutosDevolvidos);
            this.Controls.Add(this.btnRelatorioComissoes);
            this.Controls.Add(this.btnRelatorioFinanceiro);
            this.Controls.Add(this.btnRelatorioLucro);
            this.Controls.Add(this.btnRelatorioCaixa);

            // === NOVOS BOTÕES ADICIONADOS AO FORM ===
            this.Controls.Add(this.btnRelatorioFinanceiroCompleto);
            this.Controls.Add(this.btnRelatorioFinanceiroSimples);

            this.Controls.Add(this.btnListarProdutosDisponiveis);
            this.Controls.Add(this.btnRelatorioAnaliticoVendas);
            this.Controls.Add(this.btnRelatorioVendasMes);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRelatoriosGerenciais";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatórios Gerenciais";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnRelatorioVendasMes;
        private System.Windows.Forms.Button btnRelatorioAnaliticoVendas;
        private System.Windows.Forms.Button btnListarProdutosDisponiveis;

        private System.Windows.Forms.Button btnRelatorioFinanceiroSimples;
        private System.Windows.Forms.Button btnRelatorioFinanceiroCompleto;

        private System.Windows.Forms.Button btnRelatorioCaixa;
        private System.Windows.Forms.Button btnRelatorioLucro;
        private System.Windows.Forms.Button btnRelatorioFinanceiro;
        private System.Windows.Forms.Button btnRelatorioComissoes;
        private System.Windows.Forms.Button btnRelatorioProdutosDevolvidos;
        private System.Windows.Forms.Button btnRelatorioEstoque;
        private System.Windows.Forms.Button btnRelatorioFornecedores;
        private System.Windows.Forms.Button btnVoltar;
    }
}

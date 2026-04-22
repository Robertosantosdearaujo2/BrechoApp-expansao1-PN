using Microsoft.Data.Sqlite;

namespace BrechoApp.Data
{
    /// <summary>
    /// Responsável por criar todas as tabelas do banco SQLite.
    /// Este arquivo define o schema oficial do sistema.
    /// Sempre que um novo campo é adicionado em um modelo ou repositório,
    /// ele deve ser incluído aqui.
    /// </summary>
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using var connection = Conexao.GetConnection();
            connection.Open();

            // -----------------------------------------------------------------
            // IMPORTANTE:
            // SQLite NÃO altera tabelas existentes.
            // Se você mudar o schema, precisa APAGAR o arquivo brecho.db
            // para que ele seja recriado com a estrutura nova.
            // -----------------------------------------------------------------

            string sql = @"

                ---------------------------------------------------------
                -- TABELA: PARCEIROS DE NEGÓCIO (PN)
                -- CpfCnpj aceita CPF ou CNPJ (até 18 caracteres formatado)
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS ParceirosNegocio (
                    CodigoParceiro TEXT PRIMARY KEY,
                    Nome TEXT NOT NULL,
                    TipoParceiro TEXT DEFAULT 'Outro',
                    CpfCnpj TEXT NOT NULL,
                    Apelido TEXT NOT NULL,
                    Telefone TEXT NOT NULL,

                    Endereco TEXT,
                    Email TEXT,

                    Banco TEXT,
                    Agencia TEXT,
                    Conta TEXT,
                    Pix TEXT,

                    PercentualComissao REAL,
                    ComissaoDeVendedor REAL DEFAULT NULL,
                    AutorizaDoacao INTEGER,

                    Observacao TEXT,
                    Aniversario TEXT,

                    SaldoCredito REAL,

                    DataCriacao TEXT,
                    UltimaAtualizacao TEXT
                );

                ---------------------------------------------------------
                -- TABELA DE VENDEDORES
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS Vendedores (
                    CodigoVendedor TEXT PRIMARY KEY,
                    Nome TEXT NOT NULL,
                    CPF TEXT,
                    Telefone TEXT,
                    Email TEXT,
                    Endereco TEXT,

                    Banco TEXT,
                    Agencia TEXT,
                    Conta TEXT,

                    ComissaoVendedor REAL DEFAULT 0,
                    Observacao TEXT,

                    Ativo INTEGER DEFAULT 1
                );

                ---------------------------------------------------------
                -- TABELA DE LOTE DE RECEBIMENTO
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS LoteRecebimento (
                    CodigoLoteRecebimento TEXT PRIMARY KEY,
                    CodigoParceiro TEXT NOT NULL,

                    DataCriacao TEXT NOT NULL,
                    DataRecebimento TEXT,
                    DataAprovacao TEXT,

                    StatusLote TEXT NOT NULL,

                    TotalSugerido REAL DEFAULT 0,
                    TotalVenda REAL DEFAULT 0,
                    TotalComissao REAL DEFAULT 0,
                    TotalCreditoParceiro REAL DEFAULT 0,

                    Observacoes TEXT,
                    UltimaAtualizacao TEXT,

                    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE ITENS DO LOTE
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS ItemLote (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,

                    CodigoLoteRecebimento TEXT NOT NULL,
                    CodigoParceiro TEXT NOT NULL,

                    NomeDoItem TEXT NOT NULL,
                    MarcaDoItem TEXT NOT NULL,
                    CategoriaDoItem TEXT NOT NULL,
                    TamanhoCorDoItem TEXT NOT NULL,

                    ObservacaoDoItem TEXT,

                    PrecoSugeridoDoItem REAL DEFAULT 0,
                    PrecoVendaDoItem REAL NOT NULL,

                    StatusItem TEXT NOT NULL,

                    CodigoProdutoGerado TEXT,

                    DataCriacao TEXT NOT NULL,
                    UltimaAtualizacao TEXT NOT NULL,

                    FOREIGN KEY (CodigoLoteRecebimento) REFERENCES LoteRecebimento (CodigoLoteRecebimento),
                    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE CLIENTES
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS Clientes (
                    CodigoCliente TEXT PRIMARY KEY,
                    Nome TEXT NOT NULL,
                    CPF TEXT,
                    Telefone TEXT,
                    Endereco TEXT,
                    Email TEXT,
                    Observacao TEXT,
                    Aniversario TEXT,
                    SaldoCredito REAL DEFAULT 0
                );

                ---------------------------------------------------------
                -- TABELA DE PRODUTOS
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS Produtos (
                    CodigoProduto TEXT PRIMARY KEY,
                    CodigoLoteRecebimento TEXT NOT NULL,

                    NomeDoItem TEXT NOT NULL,
                    MarcaDoItem TEXT NOT NULL,
                    ObservacaoDoItem TEXT,
                    CategoriaDoItem TEXT NOT NULL,
                    TamanhoCorDoItem TEXT NOT NULL,

                    PrecoVendaDoItem REAL NOT NULL,
                    StatusDoProduto TEXT NOT NULL,

                    CodigoParceiro TEXT NOT NULL,

                    DataCriacao TEXT NOT NULL,
                    UltimaAtualizacao TEXT NOT NULL,

                    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio (CodigoParceiro),
                    FOREIGN KEY (CodigoLoteRecebimento) REFERENCES LoteRecebimento (CodigoLoteRecebimento)
                );

                ---------------------------------------------------------
                -- TABELA DE CRÉDITOS DO PARCEIRO
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS CreditoParceiro (
                    CodigoCredito TEXT PRIMARY KEY,
                    CodigoParceiro TEXT NOT NULL,
                    ValorCredito REAL NOT NULL,
                    DataCredito TEXT NOT NULL,
                    OrigemCredito TEXT,

                    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE LOTE DE DEVOLUÇÃO
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS LoteDevolucao (
                    CodigoLoteDevolucao TEXT PRIMARY KEY,
                    CodigoParceiro TEXT NOT NULL,
                    DataDevolucao TEXT,
                    StatusDevolucao TEXT,

                    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE PRODUTOS DEVOLVIDOS
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS ProdutosDevolucao (
                    CodigoProdutoDevolucao TEXT PRIMARY KEY,
                    CodigoLoteDevolucao TEXT NOT NULL,
                    CodigoProduto TEXT NOT NULL,
                    MotivoDevolucao TEXT,
                    DataDevolucao TEXT,

                    FOREIGN KEY (CodigoLoteDevolucao) REFERENCES LoteDevolucao (CodigoLoteDevolucao),
                    FOREIGN KEY (CodigoProduto) REFERENCES Produtos (CodigoProduto)
                );
                ---------------------------------------------------------
                -- TABELA: CONTAS A RECEBER
                -- Registra valores que o PN deve ao brechó (Futuro)
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS ContasAReceber (
                    IdContasAReceber INTEGER PRIMARY KEY AUTOINCREMENT,

                    CodigoParceiro TEXT NOT NULL,
                    IdVenda INTEGER,

                    ValorOriginal REAL NOT NULL,
                    ValorAberto REAL NOT NULL,

                    DataCriacao TEXT NOT NULL,
                    DataBaixa TEXT,

                    Status TEXT NOT NULL, -- Aberto, Parcial, Baixado
                    Observacao TEXT,

                    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio (CodigoParceiro),
                    FOREIGN KEY (IdVenda) REFERENCES Vendas (IdVenda)
                );

                ---------------------------------------------------------
                -- TABELA: COMISSOES A PAGAR
                -- Registra valores que o brechó deve ao PN (comissões)
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS ComissoesAPagar (
                    IdComissao INTEGER PRIMARY KEY AUTOINCREMENT,

                    CodigoParceiro TEXT NOT NULL,

                    ValorOriginal REAL NOT NULL,
                    ValorAberto REAL NOT NULL,

                    MesReferencia INTEGER NOT NULL,
                    AnoReferencia INTEGER NOT NULL,

                    DataCriacao TEXT NOT NULL,
                    DataBaixa TEXT,

                    Status TEXT NOT NULL, -- Aberto, Parcial, Baixado
                    Observacao TEXT,

                    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA: FECHAMENTO DE COMISSOES (CABECALHO)
                -- Registra cada fechamento mensal realizado
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS FechamentoComissoes (
                    IdFechamento INTEGER PRIMARY KEY AUTOINCREMENT,

                    Mes INTEGER NOT NULL,
                    Ano INTEGER NOT NULL,

                    DataFechamento TEXT NOT NULL,
                    Observacao TEXT
                );

                ---------------------------------------------------------
                -- TABELA: FECHAMENTO DE COMISSOES - ITENS
                -- Saldo final de cada PN no fechamento mensal
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS FechamentoComissoesItem (
                    IdFechamentoItem INTEGER PRIMARY KEY AUTOINCREMENT,

                    IdFechamento INTEGER NOT NULL,
                    CodigoParceiro TEXT NOT NULL,

                    TotalComissoes REAL NOT NULL,
                    TotalContasAReceber REAL NOT NULL,

                    SaldoFinal REAL NOT NULL, -- Comissoes - CAR
                    TipoSaldo TEXT NOT NULL,  -- A Favor, Contra, Zero

                    DataRegistro TEXT NOT NULL,

                    FOREIGN KEY (IdFechamento) REFERENCES FechamentoComissoes (IdFechamento),
                    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE VENDAS
                -- Sistema completo de vendas com suporte a múltiplos itens
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS Vendas (
                    IdVenda INTEGER PRIMARY KEY AUTOINCREMENT,
                    CodigoVenda TEXT NOT NULL UNIQUE,
                    IdVendedor TEXT NOT NULL,
                    IdCliente TEXT NOT NULL,
                    DataVenda TEXT NOT NULL,
                    ValorTotalOriginal REAL NOT NULL,
                    DescontoPercentual REAL DEFAULT 0,
                    DescontoValor REAL DEFAULT 0,
                    Campanha TEXT,
                    DescontoCampanhaPercentual REAL DEFAULT 0,
                    DescontoCampanha REAL DEFAULT 0,
                    ValorTotalFinal REAL NOT NULL,
                    FormaPagamento TEXT NOT NULL,
                    Observacoes TEXT,
                    DataCriacao TEXT NOT NULL,
                    
                    FOREIGN KEY (IdVendedor) REFERENCES ParceirosNegocio (CodigoParceiro),
                    FOREIGN KEY (IdCliente) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE ITENS DAS VENDAS
                -- Armazena cada produto vendido em uma venda
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS VendasItens (
                    IdVendaItem INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdVenda INTEGER NOT NULL,
                    IdProduto TEXT NOT NULL,
                    IdFornecedor TEXT NOT NULL,
                    PrecoOriginal REAL NOT NULL,
                    PrecoFinalNegociado REAL NOT NULL,
                    
                    FOREIGN KEY (IdVenda) REFERENCES Vendas (IdVenda),
                    FOREIGN KEY (IdProduto) REFERENCES Produtos (CodigoProduto),
                    FOREIGN KEY (IdFornecedor) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE CENTROS FINANCEIROS
                -- Usado para controlar contas, caixas, bancos, fundos etc.
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS CentrosFinanceiros (
                    IdCentroFinanceiro INTEGER PRIMARY KEY AUTOINCREMENT,

                    Nome TEXT NOT NULL UNIQUE,
                    Tipo TEXT NOT NULL, -- Caixa, ContaCorrente, CartaoAReceber, ContasAReceber, ComissoesAPagar etc.

                    SaldoInicial REAL NOT NULL DEFAULT 0,
                    SaldoAtual REAL NOT NULL DEFAULT 0,

                    Ativo INTEGER NOT NULL DEFAULT 1
                );

                ---------------------------------------------------------
                -- TABELA: FORMAS DE PAGAMENTO
                -- Lista de métodos aceitos pelo sistema (Dinheiro, Pix, Cartão, Futuro...)
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS FormasPagamento (
                    IdFormaPagamento INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL UNIQUE,
                    Codigo TEXT,
                    Ativo INTEGER NOT NULL DEFAULT 1
                );

                ---------------------------------------------------------
                -- TABELA: PAGAMENTOS POR MOVIMENTAÇÃO
                -- Cada movimentação financeira pode ser dividida em várias linhas de pagamento
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS MovimentacaoPagamentos (
                    IdPagamento INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdMovimentacao INTEGER NOT NULL,
                    IdFormaPagamento INTEGER NOT NULL,
                    IdCentroFinanceiro INTEGER NOT NULL,
                    Valor REAL NOT NULL,

                    FOREIGN KEY (IdMovimentacao) REFERENCES MovimentacoesFinanceiras (IdMovimentacao),
                    FOREIGN KEY (IdFormaPagamento) REFERENCES FormasPagamento (IdFormaPagamento),
                    FOREIGN KEY (IdCentroFinanceiro) REFERENCES CentrosFinanceiros (IdCentroFinanceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE PERÍODOS DE COMISSÃO
                -- Controla o fechamento mensal de comissões
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS ComissaoPeriodo (
                    IdPeriodo INTEGER PRIMARY KEY AUTOINCREMENT,

                    Mes INTEGER NOT NULL,
                    Ano INTEGER NOT NULL,

                    DataAbertura TEXT NOT NULL DEFAULT (datetime('now')),
                    DataFechamento TEXT,

                    Status TEXT NOT NULL
                );

                ---------------------------------------------------------
                -- TABELA DE SALDOS CONSOLIDADOS POR PN
                -- Armazena comissões, contas a receber e saldo final
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS ComissaoSaldoPN (
                    IdSaldo INTEGER PRIMARY KEY AUTOINCREMENT,

                    IdPeriodo INTEGER NOT NULL,
                    CodigoPN TEXT NOT NULL,

                    ComissoesAPagar REAL NOT NULL DEFAULT 0,
                    ContasAReceber REAL NOT NULL DEFAULT 0,

                    SaldoFinal REAL NOT NULL DEFAULT 0,
                    SaldoCompensado REAL NOT NULL DEFAULT 0,

                    Status TEXT NOT NULL,

                    FOREIGN KEY (IdPeriodo) REFERENCES ComissaoPeriodo (IdPeriodo)
                );

                ---------------------------------------------------------
                -- TABELA DE MOVIMENTOS DE COMISSÃO
                -- Registra pagamentos e recebimentos do PN
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS ComissaoMovimento (
                    IdMovimento INTEGER PRIMARY KEY AUTOINCREMENT,

                    IdSaldo INTEGER NOT NULL,

                    Tipo TEXT NOT NULL, -- PagamentoPN ou RecebimentoPN
                    Valor REAL NOT NULL,
                    FormaPagamento TEXT NOT NULL,

                    DataMovimento TEXT NOT NULL DEFAULT (datetime('now')),
                    Observacao TEXT,

                    FOREIGN KEY (IdSaldo) REFERENCES ComissaoSaldoPN (IdSaldo)
                );

                ---------------------------------------------------------
                -- TABELA DE MOVIMENTAÇÕES FINANCEIRAS
                -- Registra entradas, saídas e transferências
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS MovimentacoesFinanceiras (
                    IdMovimentacao INTEGER PRIMARY KEY AUTOINCREMENT,
                    Data TEXT NOT NULL,
                    Tipo TEXT NOT NULL, -- Entrada, Saída, Transferência
                    Valor REAL NOT NULL,

                    IdCentroOrigem INTEGER,
                    IdCentroDestino INTEGER,

                    Categoria TEXT,
                    Descricao TEXT,

                    IdVenda INTEGER,
                    IdParceiro TEXT,

                    Previsto INTEGER NOT NULL DEFAULT 0,

                    FOREIGN KEY (IdCentroOrigem) REFERENCES CentrosFinanceiros (IdCentroFinanceiro),
                    FOREIGN KEY (IdCentroDestino) REFERENCES CentrosFinanceiros (IdCentroFinanceiro),
                    FOREIGN KEY (IdVenda) REFERENCES Vendas (IdVenda),
                    FOREIGN KEY (IdParceiro) REFERENCES ParceirosNegocio (CodigoParceiro)
                );

                ---------------------------------------------------------
                -- TABELA DE CATEGORIAS DE PRODUTOS
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS Categorias (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NomeCategoria TEXT NOT NULL UNIQUE,
                    DataCriacao TEXT NOT NULL
                );

                ---------------------------------------------------------
                -- TABELA DE CATEGORIAS FINANCEIRAS
                -- Usada para classificação de movimentações financeiras
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS CategoriasFinanceiras (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL UNIQUE,
                    Grupo TEXT,
                    DataCriacao TEXT NOT NULL
                );

                ---------------------------------------------------------
                -- TABELA DE PAGAMENTOS DA VENDA
                -- Armazena cada linha de pagamento (suporte a Combinado)
                ---------------------------------------------------------
                CREATE TABLE IF NOT EXISTS VendaPagamentos (
                    IdVendaPagamento INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdVenda INTEGER NOT NULL,
                    FormaPagamento TEXT NOT NULL,
                    Valor REAL NOT NULL,
                    IdCentroFinanceiro INTEGER,

                    FOREIGN KEY (IdVenda) REFERENCES Vendas (IdVenda),
                    FOREIGN KEY (IdCentroFinanceiro) REFERENCES CentrosFinanceiros (IdCentroFinanceiro)
                );
            ";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.ExecuteNonQuery();

            // Seed initial CentrosFinanceiros (only if not exists)
            string seedCentros = @"
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Caixa', 'Caixa', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Caixa');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Conta Corrente Brecho', 'ContaCorrente', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Conta Corrente Brecho');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Conta Corrente Socio1', 'ContaCorrente', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Conta Corrente Socio1');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Conta Corrente Socio2', 'ContaCorrente', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Conta Corrente Socio2');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Conta Corrente Socio3', 'ContaCorrente', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Conta Corrente Socio3');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Contas a Receber', 'ContasAReceber', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Contas a Receber');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Contas a Pagar', 'ContasAPagar', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Contas a Pagar');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Cartão Credito1', 'Cartao', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Cartão Credito1');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Cartão Credito2', 'Cartao', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Cartão Credito2');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Cartão Debito1', 'Cartao', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Cartão Debito1');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Cartão Debito2', 'Cartao', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Cartão Debito2');
                INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
                SELECT 'Comissao PN', 'ComissoesAPagar', 0, 0, 1 WHERE NOT EXISTS(SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Comissao PN');
            ";
            using var seedCmd = new SqliteCommand(seedCentros, connection);
            seedCmd.ExecuteNonQuery();

            // Seed initial FormasPagamento
            string seedFormas = @"
                INSERT INTO FormasPagamento (Nome, Codigo, Ativo)
                SELECT 'Dinheiro', 'DIN', 1 WHERE NOT EXISTS(SELECT 1 FROM FormasPagamento WHERE Nome = 'Dinheiro');
                INSERT INTO FormasPagamento (Nome, Codigo, Ativo)
                SELECT 'Depósito / Transferência / Pix', 'PIX', 1 WHERE NOT EXISTS(SELECT 1 FROM FormasPagamento WHERE Nome = 'Depósito / Transferência / Pix');
                INSERT INTO FormasPagamento (Nome, Codigo, Ativo)
                SELECT 'Futuro', 'FUT', 1 WHERE NOT EXISTS(SELECT 1 FROM FormasPagamento WHERE Nome = 'Futuro');
                INSERT INTO FormasPagamento (Nome, Codigo, Ativo)
                SELECT 'Cartão de Crédito', 'CC', 1 WHERE NOT EXISTS(SELECT 1 FROM FormasPagamento WHERE Nome = 'Cartão de Crédito');
                INSERT INTO FormasPagamento (Nome, Codigo, Ativo)
                SELECT 'Cartão de Débito', 'CD', 1 WHERE NOT EXISTS(SELECT 1 FROM FormasPagamento WHERE Nome = 'Cartão de Débito');
            ";
            using var seedFormasCmd = new SqliteCommand(seedFormas, connection);
            seedFormasCmd.ExecuteNonQuery();

            // Seed initial CategoriasFinanceiras (only if not exists)
            string seedCategoriasFinanceiras = @"
                -- Grupos principais
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'RECEITAS', NULL, datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'RECEITAS');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'CUSTOS', NULL, datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'CUSTOS');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'DESPESAS OPERACIONAIS', NULL, datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'DESPESAS OPERACIONAIS');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'DESPESAS COM PESSOAL', NULL, datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'DESPESAS COM PESSOAL');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'DESPESAS FINANCEIRAS', NULL, datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'DESPESAS FINANCEIRAS');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'INVESTIMENTOS', NULL, datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'INVESTIMENTOS');

                -- Subcategorias
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Receita de Vendas', 'RECEITAS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Receita de Vendas');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Outras Receitas', 'RECEITAS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Outras Receitas');

                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Compra de Mercadorias', 'CUSTOS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Compra de Mercadorias');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Repasse a Consignados / PN', 'CUSTOS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Repasse a Consignados / PN');

                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Despesas Administrativas', 'DESPESAS OPERACIONAIS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Despesas Administrativas');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Despesas de Marketing', 'DESPESAS OPERACIONAIS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Despesas de Marketing');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Despesas com Serviços', 'DESPESAS OPERACIONAIS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Despesas com Serviços');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Despesas Bancárias / Tarifas', 'DESPESAS OPERACIONAIS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Despesas Bancárias / Tarifas');

                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Pró-Labore dos Sócios', 'DESPESAS COM PESSOAL', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Pró-Labore dos Sócios');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Colaboradores-Empregados', 'DESPESAS COM PESSOAL', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Colaboradores-Empregados');

                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Juros / Multas', 'DESPESAS FINANCEIRAS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Juros / Multas');
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Descontos Concedidos', 'DESPESAS FINANCEIRAS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Descontos Concedidos');

                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                SELECT 'Investimentos / Imobilizado', 'INVESTIMENTOS', datetime('now') WHERE NOT EXISTS(SELECT 1 FROM CategoriasFinanceiras WHERE Nome = 'Investimentos / Imobilizado');
            ";
            using var seedCategoriasFinanceirasCmd = new SqliteCommand(seedCategoriasFinanceiras, connection);
            seedCategoriasFinanceirasCmd.ExecuteNonQuery();

            // Run migrations for existing databases
            RunMigrations(connection);
        }

        /// <summary>
        /// Executa migrações para adicionar novas colunas em bancos de dados existentes.
        /// </summary>
        private static void RunMigrations(SqliteConnection connection)
        {
            // Migration: Add DescontoCampanhaPercentual column to Vendas table if it doesn't exist
            try
            {
                string checkColumnSql = "SELECT COUNT(*) FROM pragma_table_info('Vendas') WHERE name='DescontoCampanhaPercentual'";
                using var checkCmd = new SqliteCommand(checkColumnSql, connection);
                var columnExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;

                if (!columnExists)
                {
                    string addColumnSql = "ALTER TABLE Vendas ADD COLUMN DescontoCampanhaPercentual REAL DEFAULT 0";
                    using var alterCmd = new SqliteCommand(addColumnSql, connection);
                    alterCmd.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                // Log the error but don't fail initialization
                // The column might already exist or the table might not exist yet
                System.Diagnostics.Debug.WriteLine($"Migration warning: {ex.Message}");
            }

            // Nota: a criação da tabela `VendaPagamentos` foi mantida no schema principal
            // (bloco SQL em Initialize). Removido aqui para evitar duplicação.
        }
    }
}

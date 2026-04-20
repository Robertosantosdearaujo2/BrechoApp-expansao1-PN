using System;
using Microsoft.Data.Sqlite;

namespace BrechoApp.Data
{
    /// <summary>
    /// Responsável por criar todas as tabelas do banco SQLite.
    /// Este arquivo define o schema oficial do sistema.
    /// Sempre que um novo campo for adicionado em um modelo ou repositório,
    /// ele deve ser incluído aqui.
    ///
    /// IMPORTANTE:
    /// SQLite NÃO altera tabelas existentes.
    /// Se você mudar o schema, precisa APAGAR o arquivo brecho.db
    /// para que ele seja recriado com a estrutura nova.
    /// </summary>
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using var connection = Conexao.GetConnection();
            connection.Open();

            string sql = @"

---------------------------------------------------------------
-- TABELA: PARCEIROS DE NEGÓCIO
-- Representa clientes, vendedores, fornecedores e a própria loja
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ParceirosNegocio (
    CodigoParceiro TEXT PRIMARY KEY,
    Nome TEXT NOT NULL,
    TipoParceiro TEXT NOT NULL DEFAULT 'Outro', -- Cliente, Vendedor, Loja, etc.
    CpfCnpj TEXT NOT NULL,
    Apelido TEXT NOT NULL,
    Telefone TEXT,
    Endereco TEXT,
    Email TEXT,
    Banco TEXT,
    Agencia TEXT,
    Conta TEXT,
    Pix TEXT,
    PercentualComissao REAL DEFAULT 0,
    ComissaoDeVendedor REAL DEFAULT NULL,
    AutorizaDoacao INTEGER DEFAULT 0,
    Observacao TEXT,
    Aniversario TEXT,
    SaldoCredito REAL DEFAULT 0,
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now')),
    UltimaAtualizacao TEXT
);

---------------------------------------------------------------
-- TABELA: VENDEDORES
-- Mantida apenas se o sistema realmente usa cadastro separado
---------------------------------------------------------------
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
    Ativo INTEGER NOT NULL DEFAULT 1
);

---------------------------------------------------------------
-- TABELA: LOTE DE RECEBIMENTO
-- Agrupa itens recebidos de um parceiro
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS LoteRecebimento (
    CodigoLoteRecebimento TEXT PRIMARY KEY,
    CodigoParceiro TEXT NOT NULL,
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now')),
    DataRecebimento TEXT,
    DataAprovacao TEXT,
    StatusLote TEXT NOT NULL, -- Criado, Recebido, Aprovado, etc.
    TotalSugerido REAL DEFAULT 0,
    TotalVenda REAL DEFAULT 0,
    TotalComissao REAL DEFAULT 0,
    TotalCreditoParceiro REAL DEFAULT 0,
    Observacoes TEXT,
    UltimaAtualizacao TEXT,
    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio(CodigoParceiro)
);

---------------------------------------------------------------
-- TABELA: ITENS DO LOTE
-- Cada item recebido de um parceiro
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ItensLote (
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
    StatusItem TEXT NOT NULL, -- Aprovado, Reprovado, Vendido, etc.
    CodigoProdutoGerado TEXT,
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now')),
    UltimaAtualizacao TEXT,
    FOREIGN KEY (CodigoLoteRecebimento) REFERENCES LoteRecebimento(CodigoLoteRecebimento),
    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio(CodigoParceiro)
);

---------------------------------------------------------------
-- TABELA: PRODUTOS
-- Produtos disponíveis para venda
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Produtos (
    CodigoProduto TEXT PRIMARY KEY,
    CodigoLoteRecebimento TEXT NOT NULL,
    CodigoParceiro TEXT NOT NULL,
    NomeDoItem TEXT NOT NULL,
    MarcaDoItem TEXT NOT NULL,
    ObservacaoDoItem TEXT,
    CategoriaDoItem TEXT NOT NULL,
    TamanhoCorDoItem TEXT NOT NULL,
    PrecoVendaDoItem REAL NOT NULL,
    StatusDoProduto TEXT NOT NULL, -- Disponível, Vendido, Reservado, etc.
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now')),
    UltimaAtualizacao TEXT,
    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio(CodigoParceiro),
    FOREIGN KEY (CodigoLoteRecebimento) REFERENCES LoteRecebimento(CodigoLoteRecebimento)
);

---------------------------------------------------------------
-- TABELA: VENDAS
-- Registro principal de cada venda realizada
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Vendas (
    IdVenda INTEGER PRIMARY KEY AUTOINCREMENT,
    CodigoVenda TEXT NOT NULL UNIQUE,
    IdVendedor TEXT NOT NULL, -- FK para ParceirosNegocio
    IdCliente TEXT NOT NULL,  -- FK para ParceirosNegocio
    DataVenda TEXT NOT NULL DEFAULT (datetime('now')),
    ValorTotalOriginal REAL NOT NULL,
    DescontoPercentual REAL DEFAULT 0,
    DescontoValor REAL DEFAULT 0,
    DescontoCampanhaPercentual REAL DEFAULT 0,
    DescontoCampanhaValor REAL DEFAULT 0,
    ValorTotalFinal REAL NOT NULL,
    Observacoes TEXT,
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (IdVendedor) REFERENCES ParceirosNegocio(CodigoParceiro),
    FOREIGN KEY (IdCliente) REFERENCES ParceirosNegocio(CodigoParceiro)
);

---------------------------------------------------------------
-- TABELA: VENDAS ITENS
-- Cada produto vendido dentro de uma venda
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS VendasItens (
    IdVendaItem INTEGER PRIMARY KEY AUTOINCREMENT,
    IdVenda INTEGER NOT NULL,
    IdProduto TEXT NOT NULL,
    PrecoOriginal REAL NOT NULL,
    PrecoFinalNegociado REAL NOT NULL,
    FOREIGN KEY (IdVenda) REFERENCES Vendas(IdVenda),
    FOREIGN KEY (IdProduto) REFERENCES Produtos(CodigoProduto)
);

---------------------------------------------------------------
-- TABELA: PAGAMENTOS DA VENDA
-- Suporte a múltiplas formas de pagamento por venda
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS VendaPagamentos (
    IdVendaPagamento INTEGER PRIMARY KEY AUTOINCREMENT,
    IdVenda INTEGER NOT NULL,
    FormaPagamento TEXT NOT NULL, -- Dinheiro, Pix, Cartão, etc.
    Valor REAL NOT NULL,
    IdCentroFinanceiro INTEGER,
    FOREIGN KEY (IdVenda) REFERENCES Vendas(IdVenda),
    FOREIGN KEY (IdCentroFinanceiro) REFERENCES CentrosFinanceiros(IdCentroFinanceiro)
);

---------------------------------------------------------------
-- TABELA: CENTROS FINANCEIROS
-- Representa caixas, contas bancárias, fundos, cartões etc.
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS CentrosFinanceiros (
    IdCentroFinanceiro INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome TEXT NOT NULL UNIQUE,
    Tipo TEXT NOT NULL, -- Caixa, ContaCorrente, CartaoReceber, etc.
    SaldoInicial REAL NOT NULL DEFAULT 0,
    SaldoAtual REAL NOT NULL DEFAULT 0,
    Ativo INTEGER NOT NULL DEFAULT 1
);

---------------------------------------------------------------
-- TABELA: MOVIMENTAÇÕES FINANCEIRAS
-- Registra entradas, saídas e transferências
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS MovimentacoesFinanceiras (
    IdMovimentacao INTEGER PRIMARY KEY AUTOINCREMENT,
    Data TEXT NOT NULL DEFAULT (datetime('now')),
    Tipo TEXT NOT NULL, -- Entrada, Saída, Transferência
    Valor REAL NOT NULL,
    IdCentroOrigem INTEGER,
    IdCentroDestino INTEGER,
    Categoria TEXT,
    Descricao TEXT,
    IdVenda INTEGER,
    IdParceiro TEXT,
    Previsto INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (IdCentroOrigem) REFERENCES CentrosFinanceiros(IdCentroFinanceiro),
    FOREIGN KEY (IdCentroDestino) REFERENCES CentrosFinanceiros(IdCentroFinanceiro),
    FOREIGN KEY (IdVenda) REFERENCES Vendas(IdVenda),
    FOREIGN KEY (IdParceiro) REFERENCES ParceirosNegocio(CodigoParceiro)
);

---------------------------------------------------------------
-- TABELA: CONTAS A RECEBER
-- Valores que o PN deve ao brechó
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ContasAReceber (
    IdContasAReceber INTEGER PRIMARY KEY AUTOINCREMENT,
    CodigoParceiro TEXT NOT NULL,
    IdVenda INTEGER,
    ValorOriginal REAL NOT NULL,
    ValorAberto REAL NOT NULL,
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now')),
    DataBaixa TEXT,
    Status TEXT NOT NULL, -- Aberto, Parcial, Baixado
    Observacao TEXT,
    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio(CodigoParceiro),
    FOREIGN KEY (IdVenda) REFERENCES Vendas(IdVenda)
);

---------------------------------------------------------------
-- TABELA: COMISSÕES A PAGAR
-- Valores que o brechó deve ao PN
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ComissoesAPagar (
    IdComissao INTEGER PRIMARY KEY AUTOINCREMENT,
    CodigoParceiro TEXT NOT NULL,
    ValorOriginal REAL NOT NULL,
    ValorAberto REAL NOT NULL,
    MesReferencia INTEGER NOT NULL,
    AnoReferencia INTEGER NOT NULL,
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now')),
    DataBaixa TEXT,
    Status TEXT NOT NULL, -- Aberto, Parcial, Baixado
    Observacao TEXT,
    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio(CodigoParceiro)
);

---------------------------------------------------------------
-- TABELA: PERÍODOS DE COMISSÃO
-- Cada período mensal de fechamento
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ComissaoPeriodo (
    IdPeriodo INTEGER PRIMARY KEY AUTOINCREMENT,
    Mes INTEGER NOT NULL,
    Ano INTEGER NOT NULL,
    DataAbertura TEXT NOT NULL DEFAULT (datetime('now')),
    DataFechamento TEXT,
    Status TEXT NOT NULL -- Aberto, Fechado
);

---------------------------------------------------------------
-- TABELA: SALDOS CONSOLIDADOS POR PN
-- Armazena comissões, contas a receber e saldo final
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ComissaoSaldoPN (
    IdSaldo INTEGER PRIMARY KEY AUTOINCREMENT,
    IdPeriodo INTEGER NOT NULL,
    IdParceiro TEXT NOT NULL,
    ComissaoAPagar REAL NOT NULL DEFAULT 0,
    ContasAReceber REAL NOT NULL DEFAULT 0,
    SaldoFinal REAL NOT NULL DEFAULT 0,
    Status TEXT NOT NULL, -- A Favor, Contra, Zero
    FOREIGN KEY (IdPeriodo) REFERENCES ComissaoPeriodo(IdPeriodo),
    FOREIGN KEY (IdParceiro) REFERENCES ParceirosNegocio(CodigoParceiro)
);

---------------------------------------------------------------
-- TABELA: MOVIMENTOS DE COMISSÃO
-- Registra pagamentos e recebimentos de PN
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ComissaoMovimento (
    IdMovimento INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSaldo INTEGER NOT NULL,
    Tipo TEXT NOT NULL, -- PagamentoPN ou RecebimentoPN
    FormaPagamento TEXT NOT NULL,
    DataMovimento TEXT NOT NULL DEFAULT (datetime('now')),
    Observacao TEXT,
    FOREIGN KEY (IdSaldo) REFERENCES ComissaoSaldoPN(IdSaldo)
);

---------------------------------------------------------------
-- TABELA: FECHAMENTO DE COMISSÕES (CABECALHO)
-- Registra cada fechamento mensal realizado
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS FechamentoComissoes (
    IdFechamento INTEGER PRIMARY KEY AUTOINCREMENT,
    Mes INTEGER NOT NULL,
    Ano INTEGER NOT NULL,
    DataFechamento TEXT NOT NULL,
    Observacao TEXT
);

---------------------------------------------------------------
-- TABELA: FECHAMENTO DE COMISSÕES - ITENS
-- Saldo final de cada PN no fechamento mensal
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS FechamentoComissoesItens (
    IdFechamentoItem INTEGER PRIMARY KEY AUTOINCREMENT,
    IdFechamento INTEGER NOT NULL,
    CodigoParceiro TEXT NOT NULL,
    TotalComissoes REAL NOT NULL,
    TotalContasAReceber REAL NOT NULL,
    SaldoFinal REAL NOT NULL, -- Comissoes - CAR
    TipoSaldo TEXT NOT NULL, -- A Favor, Contra, Zero
    DataRegistro TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (IdFechamento) REFERENCES FechamentoComissoes(IdFechamento),
    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio(CodigoParceiro)
);

---------------------------------------------------------------
-- TABELA: CATEGORIAS DE PRODUTOS
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Categorias (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    NomeCategoria TEXT NOT NULL UNIQUE,
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now'))
);

---------------------------------------------------------------
-- TABELA: LOTE DE DEVOLUÇÃO
-- Agrupa devoluções de produtos por parceiro
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS LoteDevolucao (
    CodigoLoteDevolucao TEXT PRIMARY KEY,
    CodigoLoteRecebimento TEXT NOT NULL,
    CodigoParceiro TEXT NOT NULL,
    DataDevolucao TEXT,
    StatusDevolucao TEXT,
    FOREIGN KEY (CodigoParceiro) REFERENCES ParceirosNegocio(CodigoParceiro),
    FOREIGN KEY (CodigoLoteRecebimento) REFERENCES LoteRecebimento(CodigoLoteRecebimento)
);

---------------------------------------------------------------
-- TABELA: PRODUTOS DEVOLVIDOS
---------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ProdutosDevolucao (
    CodigoProdutoDevolucao TEXT PRIMARY KEY,
    CodigoLoteDevolucao TEXT NOT NULL,
    CodigoProduto TEXT NOT NULL,
    MotivoDevolucao TEXT,
    DataDevolucao TEXT,
    FOREIGN KEY (CodigoLoteDevolucao) REFERENCES LoteDevolucao(CodigoLoteDevolucao),
    FOREIGN KEY (CodigoProduto) REFERENCES Produtos(CodigoProduto)
);

---------------------------------------------------------------
-- PARCEIRO PADRÃO: LOJA (PN0)
---------------------------------------------------------------
INSERT OR IGNORE INTO ParceirosNegocio (
    CodigoParceiro,
    Nome,
    TipoParceiro,
    CpfCnpj,
    Apelido,
    Telefone,
    Endereco,
    Email,
    Banco,
    Agencia,
    Conta,
    Pix,
    PercentualComissao,
    ComissaoDeVendedor,
    AutorizaDoacao,
    Observacao,
    Aniversario,
    SaldoCredito,
    DataCriacao,
    UltimaAtualizacao
)
VALUES (
    'PN0',
    'Loja',
    'Loja',
    '00.000.000/0000-00',
    'Loja',
    '',
    '',
    '',
    '',
    '',
    '',
    '',
    0,
    NULL,
    1,
    'Parceiro interno usado para operações da loja',
    '01/01/2000',
    0,
    datetime('now'),
    datetime('now')
);

---------------------------------------------------------------
-- CENTROS FINANCEIROS PADRÃO
---------------------------------------------------------------

-- Conta Corrente Sócio 1
INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
SELECT 'Conta Corrente Sócio 1', 'ContaCorrente', 0, 0, 1
WHERE NOT EXISTS (
    SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Conta Corrente Sócio 1'
);

-- Conta Corrente Sócio 2
INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
SELECT 'Conta Corrente Sócio 2', 'ContaCorrente', 0, 0, 1
WHERE NOT EXISTS (
    SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Conta Corrente Sócio 2'
);

-- Caixa físico
INSERT INTO CentrosFinanceiros (Nome, Tipo, SaldoInicial, SaldoAtual, Ativo)
SELECT 'Caixa', 'Caixa', 0, 0, 1
WHERE NOT EXISTS (
    SELECT 1 FROM CentrosFinanceiros WHERE Nome = 'Caixa'
);

";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
    }
}

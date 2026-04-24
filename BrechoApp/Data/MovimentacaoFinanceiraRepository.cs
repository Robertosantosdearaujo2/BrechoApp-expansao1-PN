using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using BrechoApp.Models;

namespace BrechoApp.Data
{
    public class MovimentacaoFinanceiraRepository
    {
        private readonly string _connectionString = DatabaseConfig.ConnectionString;

        // ============================================================
        // LISTAR MOVIMENTAÇÕES POR PERÍODO
        // ============================================================
        public List<MovimentacaoFinanceira> Listar(DateTime inicio, DateTime fim)
        {
            var lista = new List<MovimentacaoFinanceira>();

            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                @"SELECT * FROM MovimentacoesFinanceiras
                  WHERE date(Data) BETWEEN date($inicio) AND date($fim)
                  ORDER BY Data DESC";

                cmd.Parameters.AddWithValue("$inicio", inicio);
                cmd.Parameters.AddWithValue("$fim", fim);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mov = new MovimentacaoFinanceira
                        {
                            IdMovimentacao = reader.GetInt32(0),
                            Data = reader.GetDateTime(1),
                            Tipo = reader.GetString(2),
                            Valor = reader.GetDecimal(3),
                            IdCentroOrigem = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                            IdCentroDestino = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        Categoria = reader.IsDBNull(6) ? "" : reader.GetString(6),
                        Grupo = "",
                            Descricao = reader.IsDBNull(7) ? "" : reader.GetString(7),
                            IdVenda = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                            IdParceiro = reader.IsDBNull(9) ? null : reader.GetString(9),
                            Previsto = reader.GetBoolean(10)
                        };

                        // carregar pagamentos associados
                        mov.Pagamentos = new List<MovimentacaoPagamento>();
                        using (var cmd2 = conn.CreateCommand())
                        {
                            cmd2.CommandText = @"SELECT IdPagamento, IdMovimentacao, IdFormaPagamento, IdCentroFinanceiro, Valor
                                                  FROM MovimentacaoPagamentos WHERE IdMovimentacao = $id";
                            cmd2.Parameters.AddWithValue("$id", mov.IdMovimentacao);
                            using (var r2 = cmd2.ExecuteReader())
                            {
                                while (r2.Read())
                                {
                                    mov.Pagamentos.Add(new MovimentacaoPagamento
                                    {
                                        IdPagamento = r2.GetInt32(0),
                                        IdMovimentacao = r2.GetInt32(1),
                                        IdFormaPagamento = r2.GetInt32(2),
                                        IdCentroFinanceiro = r2.GetInt32(3),
                                        Valor = r2.GetDecimal(4)
                                    });
                                }
                            }
                        }

                        lista.Add(mov);
                    }
                }
            }
                // Após carregar todas movimentações, preencher o campo Grupo consultando as categorias
                var repoCat = new CategoriaFinanceiraRepository();
                var cats = repoCat.ListarTodas();
                var dict = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var c in cats)
                {
                    if (!string.IsNullOrWhiteSpace(c.Nome))
                        dict[c.Nome] = c.Grupo ?? string.Empty;
                }

                foreach (var m in lista)
                {
                    if (!string.IsNullOrWhiteSpace(m.Categoria) && dict.ContainsKey(m.Categoria))
                        m.Grupo = dict[m.Categoria];
                }


            return lista;
        }

        // ============================================================
        // INSERIR MOVIMENTAÇÃO + ATUALIZAR SALDOS
        // - Validate payments before DB actions
        // - Use transaction to avoid partial inserts
        // ============================================================
        public void Inserir(MovimentacaoFinanceira m)
        {
            if (m == null)
                throw new ArgumentNullException(nameof(m));

            // validate pagamentos early (prevent partial DB writes)
            if (m.Pagamentos == null || m.Pagamentos.Count == 0)
                throw new InvalidOperationException("A movimentação financeira deve possuir ao menos um pagamento com forma e centro financeiro.");

            foreach (var p in m.Pagamentos)
            {
                if (p == null)
                    throw new InvalidOperationException("Pagamento inválido.");
                if (p.IdFormaPagamento <= 0)
                    throw new InvalidOperationException("Cada pagamento deve possuir uma forma de pagamento válida.");
                if (p.IdCentroFinanceiro <= 0)
                    throw new InvalidOperationException("Cada pagamento deve possuir um centro financeiro válido.");
                if (p.Valor <= 0)
                    throw new InvalidOperationException("Cada pagamento deve possuir valor maior que zero.");
            }

            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = conn.CreateCommand();
                        cmd.Transaction = tran;
                        cmd.CommandText =
                        @"INSERT INTO MovimentacoesFinanceiras
                          (Data, Tipo, Valor, IdCentroOrigem, IdCentroDestino, Categoria, Descricao, IdVenda, IdParceiro, Previsto)
                          VALUES ($data, $tipo, $valor, $origem, $destino, $categoria, $descricao, $idVenda, $idParceiro, $previsto);";

                        cmd.Parameters.AddWithValue("$data", m.Data);
                        cmd.Parameters.AddWithValue("$tipo", m.Tipo);
                        cmd.Parameters.AddWithValue("$valor", m.Valor);
                        cmd.Parameters.AddWithValue("$origem", (object)m.IdCentroOrigem ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("$destino", (object)m.IdCentroDestino ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("$categoria", m.Categoria);
                        cmd.Parameters.AddWithValue("$descricao", m.Descricao);
                        cmd.Parameters.AddWithValue("$idVenda", (object)m.IdVenda ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("$idParceiro", (object)m.IdParceiro ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("$previsto", m.Previsto);

                        cmd.ExecuteNonQuery();

                        // obter id inserido
                        cmd.CommandText = "SELECT last_insert_rowid();";
                        var idMov = Convert.ToInt32(cmd.ExecuteScalar());

                        // inserir pagamentos usando mesma transação
                        foreach (var p in m.Pagamentos)
                        {
                            var cmdP = conn.CreateCommand();
                            cmdP.Transaction = tran;
                            cmdP.CommandText = @"INSERT INTO MovimentacaoPagamentos (IdMovimentacao, IdFormaPagamento, IdCentroFinanceiro, Valor)
                                                 VALUES ($idMov, $idForma, $idCentro, $valor);";
                            cmdP.Parameters.AddWithValue("$idMov", idMov);
                            cmdP.Parameters.AddWithValue("$idForma", p.IdFormaPagamento);
                            cmdP.Parameters.AddWithValue("$idCentro", p.IdCentroFinanceiro);
                            cmdP.Parameters.AddWithValue("$valor", p.Valor);
                            cmdP.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        throw;
                    }
                }
            }
            // ============================================================
            // ATUALIZAR SALDOS AUTOMATICAMENTE (baseado nos pagamentos enviados)
            // ============================================================
            var repoCentros = new CentroFinanceiroRepository();

            // agrupa por centro e soma valores
            var dict = new System.Collections.Generic.Dictionary<int, decimal>();
            foreach (var p in m.Pagamentos)
            {
                if (!dict.ContainsKey(p.IdCentroFinanceiro))
                    dict[p.IdCentroFinanceiro] = 0;
                dict[p.IdCentroFinanceiro] += p.Valor;
            }

            if (m.Tipo == "Entrada")
            {
                foreach (var kv in dict)
                    repoCentros.SomarSaldo(kv.Key, kv.Value);
            }
            else if (m.Tipo == "Saida")
            {
                foreach (var kv in dict)
                    repoCentros.SubtrairSaldo(kv.Key, kv.Value);
            }
            else if (m.Tipo == "Transferencia")
            {
                // para transferências, considere IdCentroOrigem e IdCentroDestino
                if (m.IdCentroOrigem.HasValue)
                {
                    // subtrai do centro origem o total dos pagamentos que apontam para ele
                    if (dict.ContainsKey(m.IdCentroOrigem.Value))
                        repoCentros.SubtrairSaldo(m.IdCentroOrigem.Value, dict[m.IdCentroOrigem.Value]);
                }
                if (m.IdCentroDestino.HasValue)
                {
                    if (dict.ContainsKey(m.IdCentroDestino.Value))
                        repoCentros.SomarSaldo(m.IdCentroDestino.Value, dict[m.IdCentroDestino.Value]);
                }
            }
        }
    }
}
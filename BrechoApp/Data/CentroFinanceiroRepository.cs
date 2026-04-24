using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using BrechoApp.Models;

namespace BrechoApp.Data
{
    public class CentroFinanceiroRepository
    {
        private readonly string _connectionString = DatabaseConfig.ConnectionString;

        // ============================================================
        // LISTAR TODOS
        // ============================================================
        public List<CentroFinanceiro> Listar()
        {
            var lista = new List<CentroFinanceiro>();

            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT IdCentroFinanceiro, Nome, Tipo, SaldoAtual, Ativo 
                                    FROM CentrosFinanceiros
                                    ORDER BY Nome";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new CentroFinanceiro
                        {
                            IdCentroFinanceiro = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Tipo = reader.GetString(2),
                            SaldoAtual = reader.GetDecimal(3),
                            Ativo = reader.GetBoolean(4)
                        });
                    }
                }
            }

            return lista;
        }

        // ============================================================
        // BUSCAR POR ID
        // ============================================================
        public CentroFinanceiro BuscarPorId(int id)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT IdCentroFinanceiro, Nome, Tipo, SaldoAtual, Ativo
                                    FROM CentrosFinanceiros
                                    WHERE IdCentroFinanceiro = $id";

                cmd.Parameters.AddWithValue("$id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CentroFinanceiro
                        {
                            IdCentroFinanceiro = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Tipo = reader.GetString(2),
                            SaldoAtual = reader.GetDecimal(3),
                            Ativo = reader.GetBoolean(4)
                        };
                    }
                }
            }

            return null;
        }

        // ============================================================
        // INSERIR
        // ============================================================
        public void Inserir(CentroFinanceiro c)
        {
            throw new InvalidOperationException("Operação não permitida: Centros Financeiros são entidades fixas gerenciadas pelo sistema.");
        }

        // ============================================================
        // ATUALIZAR
        // ============================================================
        public void Atualizar(CentroFinanceiro c)
        {
            throw new InvalidOperationException("Operação não permitida: Centros Financeiros são entidades fixas gerenciadas pelo sistema.");
        }

        // ============================================================
        // EXCLUIR
        // ============================================================
        public void Excluir(int id)
        {
            throw new InvalidOperationException("Operação não permitida: Centros Financeiros são entidades fixas gerenciadas pelo sistema.");
        }

        // ============================================================
        // BUSCAR POR NOME EXATO
        // ============================================================
        public CentroFinanceiro BuscarPorNome(string nome)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT IdCentroFinanceiro, Nome, Tipo, SaldoAtual, Ativo
                                    FROM CentrosFinanceiros
                                    WHERE Nome = $nome
                                    LIMIT 1";

                cmd.Parameters.AddWithValue("$nome", nome);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CentroFinanceiro
                        {
                            IdCentroFinanceiro = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Tipo = reader.GetString(2),
                            SaldoAtual = reader.GetDecimal(3),
                            Ativo = reader.GetBoolean(4)
                        };
                    }
                }
            }

            return null;
        }

        // ============================================================
        // SOMAR SALDO
        // ============================================================
        public void SomarSaldo(int idCentro, decimal valor)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                @"UPDATE CentrosFinanceiros
                  SET SaldoAtual = SaldoAtual + $valor
                  WHERE IdCentroFinanceiro = $id";

                cmd.Parameters.AddWithValue("$valor", valor);
                cmd.Parameters.AddWithValue("$id", idCentro);

                cmd.ExecuteNonQuery();
            }
        }

        // ============================================================
        // SUBTRAIR SALDO
        // ============================================================
        public void SubtrairSaldo(int idCentro, decimal valor)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                @"UPDATE CentrosFinanceiros
                  SET SaldoAtual = SaldoAtual - $valor
                  WHERE IdCentroFinanceiro = $id";

                cmd.Parameters.AddWithValue("$valor", valor);
                cmd.Parameters.AddWithValue("$id", idCentro);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
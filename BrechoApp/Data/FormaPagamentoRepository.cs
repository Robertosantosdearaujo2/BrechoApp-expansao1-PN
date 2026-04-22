using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using BrechoApp.Models;

namespace BrechoApp.Data
{
    public class FormaPagamentoRepository
    {
        private readonly string _connectionString = DatabaseConfig.ConnectionString;

        public List<FormaPagamento> ListarAtivas()
        {
            var lista = new List<FormaPagamento>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT IdFormaPagamento, Nome, Codigo, Ativo FROM FormasPagamento WHERE Ativo = 1 ORDER BY Nome";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FormaPagamento
                {
                    IdFormaPagamento = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Codigo = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Ativo = reader.GetBoolean(3)
                });
            }

            return lista;
        }
    }
}

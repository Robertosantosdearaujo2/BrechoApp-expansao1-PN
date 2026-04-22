using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using BrechoApp.Models;

namespace BrechoApp.Data
{
    public class CategoriaFinanceiraRepository
    {
        private readonly string _connectionString = DatabaseConfig.ConnectionString;

        public List<CategoriaFinanceira> ListarTodas()
        {
            var lista = new List<CategoriaFinanceira>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, Grupo, DataCriacao FROM CategoriasFinanceiras ORDER BY Grupo, Nome";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new CategoriaFinanceira
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Grupo = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DataCriacao = reader.IsDBNull(3) ? DateTime.MinValue : DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
                });
            }

            return lista;
        }

        public List<string> ListarGrupos()
        {
            var lista = new List<string>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT Grupo FROM CategoriasFinanceiras WHERE Grupo IS NOT NULL ORDER BY Grupo";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    lista.Add(reader.GetString(0));
            }

            return lista;
        }

        public void Adicionar(CategoriaFinanceira cat)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO CategoriasFinanceiras (Nome, Grupo, DataCriacao)
                VALUES ($nome, $grupo, $criacao)";

            cmd.Parameters.AddWithValue("$nome", cat.Nome);
            cmd.Parameters.AddWithValue("$grupo", string.IsNullOrWhiteSpace(cat.Grupo) ? (object)DBNull.Value : cat.Grupo);
            cmd.Parameters.AddWithValue("$criacao", cat.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }

        public void Atualizar(CategoriaFinanceira cat)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE CategoriasFinanceiras
                SET Nome = $nome, Grupo = $grupo
                WHERE Id = $id";

            cmd.Parameters.AddWithValue("$nome", cat.Nome);
            cmd.Parameters.AddWithValue("$grupo", string.IsNullOrWhiteSpace(cat.Grupo) ? (object)DBNull.Value : cat.Grupo);
            cmd.Parameters.AddWithValue("$id", cat.Id);
            cmd.ExecuteNonQuery();
        }

        public void Excluir(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM CategoriasFinanceiras WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        public bool Existe(string nome, int? idExcluir = null)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            if (idExcluir.HasValue)
            {
                cmd.CommandText = "SELECT COUNT(*) FROM CategoriasFinanceiras WHERE Nome = $nome AND Id != $id";
                cmd.Parameters.AddWithValue("$id", idExcluir.Value);
            }
            else
            {
                cmd.CommandText = "SELECT COUNT(*) FROM CategoriasFinanceiras WHERE Nome = $nome";
            }
            cmd.Parameters.AddWithValue("$nome", nome);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }
}

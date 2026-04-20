using System;
using Microsoft.Data.Sqlite;
using BrechoApp.Data;
using BrechoApp.Models;

public class FinanceiroService
{
    private readonly string _connectionString;

    public FinanceiroService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // ============================================================
    // ATUALIZAR SALDO (Entrada ou Saída)
    // ============================================================
    public void AtualizarSaldoCentro(int idCentro, decimal valor, string tipo)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        // Buscar saldo atual
        var cmdSelect = conn.CreateCommand();
        cmdSelect.CommandText = @"
            SELECT SaldoAtual 
            FROM CentrosFinanceiros 
            WHERE IdCentroFinanceiro = $id
        ";
        cmdSelect.Parameters.AddWithValue("$id", idCentro);

        var result = cmdSelect.ExecuteScalar();
        if (result == null)
            throw new Exception("Centro financeiro não encontrado.");

        decimal saldoAtual = Convert.ToDecimal(result);

        if (tipo == "Entrada")
        {
            saldoAtual += valor;
        }
        else if (tipo == "Saída")
        {
            if (saldoAtual < valor)
                throw new Exception("Saldo insuficiente no centro financeiro.");

            saldoAtual -= valor;
        }

        // Atualizar saldo
        var cmdUpdate = conn.CreateCommand();
        cmdUpdate.CommandText = @"
            UPDATE CentrosFinanceiros
            SET SaldoAtual = $saldo
            WHERE IdCentroFinanceiro = $id
        ";
        cmdUpdate.Parameters.AddWithValue("$saldo", saldoAtual);
        cmdUpdate.Parameters.AddWithValue("$id", idCentro);

        cmdUpdate.ExecuteNonQuery();
    }

    // ============================================================
    // TRANSFERÊNCIA ENTRE CENTROS
    // ============================================================
    public void AtualizarSaldoTransferencia(int idOrigem, int idDestino, decimal valor)
    {
        if (idOrigem == idDestino)
            throw new Exception("Origem e destino não podem ser iguais.");

        AtualizarSaldoCentro(idOrigem, valor, "Saída");
        AtualizarSaldoCentro(idDestino, valor, "Entrada");
    }

    // ============================================================
    // REGISTRAR MOVIMENTAÇÃO FINANCEIRA
    // ============================================================
    public void RegistrarMovimentacao(MovimentacaoFinanceira mov)
    {
        var repo = new CentroFinanceiroRepository();
        repo.RegistrarMovimentacao(mov);

        // Atualiza o saldo automaticamente
        AtualizarSaldoCentro(mov.IdCentroFinanceiro, mov.Valor, mov.Tipo);
    }
}
namespace BrechoApp.Models
{
    public class MovimentacaoPagamento
    {
        public int IdPagamento { get; set; }
        public int IdMovimentacao { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdCentroFinanceiro { get; set; }
        public decimal Valor { get; set; }
    }
}

using BrechoApp.Enums;

namespace BrechoApp.Models
{
    public class Pagamento
    {
        public TipoPagamento Tipo { get; set; }

        // Usar decimal para valores monetários
        public decimal Valor { get; set; }

        // Necessário para registrar movimentações financeiras
        public int IdCentroFinanceiro { get; set; }
    }
}
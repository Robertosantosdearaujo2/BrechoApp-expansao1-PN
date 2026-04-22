namespace BrechoApp.Models
{
    public class FormaPagamento
    {
        public int IdFormaPagamento { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
    }
}

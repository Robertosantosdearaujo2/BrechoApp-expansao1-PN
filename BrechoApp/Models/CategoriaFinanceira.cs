namespace BrechoApp.Models
{
    public class CategoriaFinanceira
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}

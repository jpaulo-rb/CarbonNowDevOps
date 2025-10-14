namespace CarbonNowAPI.Model {
    public class Eletricidade {
        public int Id { get; set; }
        public UnidadeEletricidade UnidadeEletricidade { get; set; }
        public decimal ValorEletricidade { get; set; }
        public DateTime DataEstimacao { get; set; }
        public decimal CarbonoKg { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }

    public enum UnidadeEletricidade {
        KWH, MWH
    }
}

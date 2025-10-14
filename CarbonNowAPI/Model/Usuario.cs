namespace CarbonNowAPI.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Regra Regra { get; set; }
        public List<Eletricidade> Eletricidade { get; set; } = new();
        public List<Transporte> Transporte { get; set; } = new();
    }

    public enum Regra
    {
        normal, admin
    }
}
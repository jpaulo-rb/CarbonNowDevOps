namespace CarbonNowAPI.Model {
    public class Transporte {

        public int Id { get; set; }
        public decimal ValorPesoKg { get; set; }
        public decimal ValorDistanciaKm { get; set; }
        public MetodoTransporte MetodoTransporte { get; set; }
        public DateTime DataEstimacao { get; set; }
        public decimal CarbonoKg { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }

    public enum MetodoTransporte {
        Ship, Train, Truck, Plane
    }
}
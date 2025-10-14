namespace CarbonNowAPI.Exceptions {
    public class NotFoundException : Exception {
        public string Campo { get; }
        public string Valor { get; }
        public NotFoundException(string campo, string valor)
            : base($"{campo}: '{valor}' não encontrado.") {
            Campo = campo;
            Valor = valor;
        }
    }
}

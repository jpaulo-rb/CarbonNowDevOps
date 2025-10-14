namespace CarbonNowAPI.Exceptions {
    public class ConflictException : Exception {
        public string Campo { get; }
        public string Valor { get; }
        public ConflictException(string campo, string valor)
            : base($"{campo}: '{valor}' já está cadastrado.") {
            Campo = campo;
            Valor = valor;
        }
    }
}

namespace CarbonNowAPI.ViewModel {
    public class Paginacao<T> {
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }
        public IEnumerable<T> Itens { get; set; }
        public int TotalItens { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalItens / TamanhoPagina);
        public bool TemProximaPagina => PaginaAtual < TotalPaginas;
        public string ProximaPaginaUrl { get; set; }
        public bool TemPaginaAnterior => PaginaAtual > 1;
        public string PaginaAnteriorUrl { get; set; }
    }
}
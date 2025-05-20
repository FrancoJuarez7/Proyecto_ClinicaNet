namespace ClinicaNet.ViewModel
{
    public class PaginadorViewModel
    {
        public int PaginaActual { get; set; } // Número de la página actualmente seleccionada (comienza en 1).

        public int RegistrosPorPagina { get; set; } // Cantidad de registros que se muestran por página.

        public int TotalRegistros { get; set; } // Total de registros disponibles luego de aplicar filtros.

        public int TotalPagina => (int)(Math.Ceiling((decimal)TotalRegistros / RegistrosPorPagina)); // Cantidad total de páginas (calculada automáticamente).

        public Dictionary<string, string> ValoresQueryString { get; set; } = new();
        // Filtros o parámetros adicionales (como búsqueda, orden, etc.) a mantener en la URL al paginar.
    }
}
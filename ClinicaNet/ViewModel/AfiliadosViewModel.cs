using ClinicaNet.Models;

namespace ClinicaNet.ViewModel
{
    public class AfiliadosViewModel
    {
        public List<Afiliado>? Afiliados { get; set; }
        public string? buscarNombre { get; set; }
        public string? buscarApellido { get; set; }
        public long? buscarDNI { get; set; }
        public PaginadorViewModel Paginador { get; set; } = new PaginadorViewModel();
    }
}

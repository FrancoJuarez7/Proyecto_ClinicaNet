using ClinicaNet.Models;

namespace ClinicaNet.ViewModel
{
    public class EstadosViewModel
    {
        public List<Estado>? Estados { get; set; }

        public PaginadorViewModel Paginador { get; set; } = new PaginadorViewModel();
    }
}
